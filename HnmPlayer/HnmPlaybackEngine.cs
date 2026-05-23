using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;

namespace logo;

public sealed class HnmPlaybackEngine
{
    private const double BiosTickDurationMs = 54.9254;
    private const double VgaMode13DotClockHz = 25_175_000.0;
    private const int VgaMode13HorizontalTotalClocks = 800;
    private const int VgaMode13FrameTotalScanlines = 449;
    private const int VgaMode13RetraceStartScanline = 412;
    private const int VgaMode13RetraceEndScanline = 414;
    private const double CirclesPreludeFrameDurationMs = (VgaMode13HorizontalTotalClocks * VgaMode13FrameTotalScanlines * 1000.0) / VgaMode13DotClockHz;
    private const ushort DefaultPlaybackDelaySeed = 0x000C;
    private const int CirclesPaletteBufferLength = 0xF0;
    private const int CirclesPaletteTableOffset = 0xCBC;
    private const int CirclesPaletteTableEndOffset = 0xDDE;
    private const int CirclesPaletteTableLength = CirclesPaletteTableEndOffset - CirclesPaletteTableOffset;
    private const int CirclesPreludeStepCount = 0xFB;
    private const int CirclesPaletteTargetIndex = 0x50;
    private const int CirclesPaletteTargetEntryCount = 0x50;

    public const int FrameWidth = 320;
    public const int FrameHeight = 200;
    public const int VisibleFrameByteCount = FrameWidth * FrameHeight;
    public const int VgaSegmentByteCount = 0x10000;

    private readonly byte[] _framebuffer = new byte[VgaSegmentByteCount];
    private readonly byte[] _palette = new byte[256 * 3];
    private readonly byte[] _compressedFrameBuffer = new byte[VgaSegmentByteCount];
    private readonly byte[] _circlesPaletteBuffer = new byte[CirclesPaletteBufferLength];
    private readonly byte[] _circlesPaletteTable = new byte[CirclesPaletteTableLength];
    private static readonly long CirclesPreludeFrameStopwatchTicks = checked((long)Math.Ceiling((CirclesPreludeFrameDurationMs * Stopwatch.Frequency) / 1000.0));
    private static readonly long CirclesPreludeRetraceStartStopwatchTicks = checked((long)Math.Ceiling((((double)VgaMode13HorizontalTotalClocks * VgaMode13RetraceStartScanline * Stopwatch.Frequency) / VgaMode13DotClockHz)));
    private static readonly long CirclesPreludeRetraceEndStopwatchTicks = checked((long)Math.Ceiling((((double)VgaMode13HorizontalTotalClocks * VgaMode13RetraceEndScanline * Stopwatch.Frequency) / VgaMode13DotClockHz)));
    private readonly List<HnmChunk> _chunks = new();
    private int _currentChunkIndex;
    private ushort _playbackDelaySeed = DefaultPlaybackDelaySeed;
    private int _circlesPaletteTablePosition;
    private ushort _circlesPaletteFramesRemaining;
    private ushort _circlesPaletteStepWidth;
    private ushort _circlesPaletteAccumulatorR;
    private ushort _circlesPaletteAccumulatorG;
    private ushort _circlesPaletteAccumulatorB;
    private int _circlesPreludeStepsRemaining;
    private long _circlesPreludeEpochTimestamp;
    private bool _circlesPaletteStateInitialized;

    public string? InputPath { get; private set; }
    public string StatusText { get; private set; } = "Idle";
    public double PendingWaitMilliseconds { get; private set; }
    public int TotalChunks => _chunks.Count;
    public int CurrentChunkIndex => _currentChunkIndex;
    public IReadOnlyList<HnmChunk> Chunks => _chunks;
    public bool IsPlaybackComplete => _currentChunkIndex >= _chunks.Count;

    public async Task LoadAsync(string path)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(path), "Input path must not be empty.");
        Debug.Assert(FrameWidth == 320 && FrameHeight == 200, "Unexpected framebuffer dimensions.");

        InputPath = Path.GetFullPath(path);
        Debug.Assert(File.Exists(InputPath), "Input file should exist before read.");
        byte[] bytes = await File.ReadAllBytesAsync(InputPath);
        Debug.Assert(bytes.Length >= 2, "HNM file should be at least 2 bytes.");

        _chunks.Clear();
        _currentChunkIndex = 0;
        _playbackDelaySeed = DefaultPlaybackDelaySeed;
        PendingWaitMilliseconds = 0;
        Array.Clear(_framebuffer);
        Array.Clear(_palette);
        Array.Clear(_compressedFrameBuffer);
        Array.Clear(_circlesPaletteBuffer);
        Array.Clear(_circlesPaletteTable);
        _circlesPaletteTablePosition = 0;
        _circlesPaletteFramesRemaining = 0;
        _circlesPaletteStepWidth = 0;
        _circlesPaletteAccumulatorR = 0;
        _circlesPaletteAccumulatorG = 0;
        _circlesPaletteAccumulatorB = 0;
        _circlesPreludeStepsRemaining = 0;
        _circlesPreludeEpochTimestamp = 0;
        _circlesPaletteStateInitialized = false;

        HnmFileParser.ParseChunks(bytes, _chunks);
        AssertChunkLayout(_chunks, bytes.Length);

        if (_chunks.Count == 0)
        {
            StatusText = "No chunks found.";
            return;
        }

        PrimeInitialPaletteChunk();
        InitializeCirclesPaletteAnimationState();
        PrimeInitialDisplayChunk();
        ApplyCirclesPreludeFramebufferTransform();
        InitializeCirclesPreludeWaitState();
        _circlesPreludeStepsRemaining = CirclesPreludeStepCount;
        PendingWaitMilliseconds = ComputeInitialCirclesPreludeWaitMilliseconds();
        StatusText = $"Ready: {_chunks.Count} chunks parsed.";
    }

    public HnmStepResult Step()
    {
        Debug.Assert(_currentChunkIndex >= 0 && _currentChunkIndex <= _chunks.Count, "Current chunk index out of bounds.");

        if (_circlesPreludeStepsRemaining > 0)
        {
            bool preludeChanged = AdvanceCirclesPaletteAnimationState(applyPalette: true);
            int completedSteps = (CirclesPreludeStepCount - _circlesPreludeStepsRemaining) + 1;
            _circlesPreludeStepsRemaining--;
            PendingWaitMilliseconds = ComputeNextCirclesPreludeWaitMilliseconds();
            StatusText = $"Prelude {completedSteps}/{CirclesPreludeStepCount} (changed={preludeChanged}).";
            return new HnmStepResult(true, preludeChanged, false, 0, PendingWaitMilliseconds);
        }

        if (_currentChunkIndex >= _chunks.Count)
        {
            StatusText = "Playback complete.";
            return new HnmStepResult(false, false, true, 0, 0);
        }

        HnmChunk chunk = _chunks[_currentChunkIndex];
        Debug.Assert(chunk.Length >= 2, "Chunk must include at least the size word.");
        Debug.Assert(chunk.Offset >= 0 && chunk.Offset + chunk.Length <= chunk.Bytes.Length, "Chunk range is out of source bounds.");

        bool paletteChanged = AdvanceCirclesPaletteAnimationState(applyPalette: true);
        (bool frameChanged, string chunkAction) = ApplyDisplayChunk(chunk);
        bool changed = paletteChanged || frameChanged;

        int waitTicks = ComputeWaitTicks(_playbackDelaySeed);
        double waitMilliseconds = waitTicks * BiosTickDurationMs;
        Debug.Assert(waitTicks >= 0, "Wait ticks should never be negative.");
        Debug.Assert(!double.IsNaN(waitMilliseconds) && !double.IsInfinity(waitMilliseconds), "Wait milliseconds must be finite.");
        PendingWaitMilliseconds = waitMilliseconds;

        _currentChunkIndex++;
        if (_currentChunkIndex >= _chunks.Count)
        {
            StatusText = $"Playback complete after {_chunks.Count} chunks.";
        }
        else
        {
            StatusText = $"Chunk {_currentChunkIndex}/{_chunks.Count} ({chunkAction}, changed={changed}, wait {waitTicks} ticks).";
        }

        return new HnmStepResult(true, changed, _currentChunkIndex >= _chunks.Count, waitTicks, waitMilliseconds);
    }

    private static int ComputeWaitTicks(ushort delaySeed)
    {
        Debug.Assert(delaySeed > 0, "Playback delay seed should be positive while playback is active.");

        // EntryPoint_OpenLogoHnmFileAndRun_1000_0000_10000 seeds [0x52] with AX,
        // defaulting to 0x0C. The HNM loop reloads AX from [0x52] before each call to
        // HNMUnknown_1000_0FEA_10FEA, so the wait source is a stable playback-delay seed,
        // not the current chunk length.
        //
        // Mirrors HNMUnknown_1000_0FEA_10FEA integer math on AX:
        // BP = AX >> 3; AX = BP >> 2; BP -= AX.
        int bp = delaySeed >> 3;
        int ax = bp >> 2;
        bp -= ax;
        Debug.Assert(bp >= 0 && bp <= delaySeed, "Wait ticks should remain bounded by the source delay seed.");
        return bp;
    }

    private void InitializeCirclesPreludeWaitState()
    {
        // SetVideoMode_1000_0970_10970 calibrates CS:0x6F to the shorter CRT status phase
        // before CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8 starts waiting
        // for that phase on port 0x3DA. In standard VGA mode 13h the shorter phase is vertical
        // retrace (bit 3 set), so the standalone player aligns palette writes to the standard
        // BIOS mode-13h retrace grid instead of approximating with a fixed sleep.
        if (CirclesPreludeFrameStopwatchTicks <= 0)
        {
            throw new InvalidOperationException("Mode 13h frame duration must produce a positive stopwatch interval.");
        }

        if (CirclesPreludeRetraceStartStopwatchTicks <= 0 || CirclesPreludeRetraceStartStopwatchTicks >= CirclesPreludeFrameStopwatchTicks)
        {
            throw new InvalidOperationException("Mode 13h retrace start must fall strictly inside one frame.");
        }

        if (CirclesPreludeRetraceEndStopwatchTicks <= CirclesPreludeRetraceStartStopwatchTicks || CirclesPreludeRetraceEndStopwatchTicks > CirclesPreludeFrameStopwatchTicks)
        {
            throw new InvalidOperationException("Mode 13h retrace end must fall after retrace start and within one frame.");
        }

        _circlesPreludeEpochTimestamp = Stopwatch.GetTimestamp();
    }

    private double ComputeInitialCirclesPreludeWaitMilliseconds()
    {
        long frameOffset = GetCirclesPreludeFrameOffset(Stopwatch.GetTimestamp());
        if (frameOffset >= CirclesPreludeRetraceStartStopwatchTicks && frameOffset < CirclesPreludeRetraceEndStopwatchTicks)
        {
            return 0;
        }

        return ComputeNextCirclesPreludeWaitMilliseconds();
    }

    private double ComputeNextCirclesPreludeWaitMilliseconds()
    {
        long frameOffset = GetCirclesPreludeFrameOffset(Stopwatch.GetTimestamp());
        long waitTicks = CirclesPreludeRetraceStartStopwatchTicks - frameOffset;
        if (waitTicks <= 0)
        {
            waitTicks += CirclesPreludeFrameStopwatchTicks;
        }

        Debug.Assert(waitTicks > 0 && waitTicks <= CirclesPreludeFrameStopwatchTicks, "Prelude retrace wait should stay within one synthetic VGA frame.");
        return StopwatchTicksToMilliseconds(waitTicks);
    }

    private long GetCirclesPreludeFrameOffset(long nowTimestamp)
    {
        if (_circlesPreludeEpochTimestamp == 0)
        {
            throw new InvalidOperationException("Circles prelude wait state must be initialized before wait scheduling.");
        }

        long elapsedTicks = nowTimestamp - _circlesPreludeEpochTimestamp;
        if (elapsedTicks < 0)
        {
            throw new InvalidOperationException("Stopwatch time moved backwards while scheduling the circles prelude.");
        }

        return elapsedTicks % CirclesPreludeFrameStopwatchTicks;
    }

    private static double StopwatchTicksToMilliseconds(long stopwatchTicks)
    {
        Debug.Assert(stopwatchTicks >= 0, "Stopwatch tick duration should never be negative.");
        return (stopwatchTicks * 1000.0) / Stopwatch.Frequency;
    }

    public void RenderCurrentFrame(WriteableBitmap bitmap)
    {
        Debug.Assert(bitmap.PixelSize.Width == FrameWidth && bitmap.PixelSize.Height == FrameHeight, "Bitmap dimensions must match framebuffer dimensions.");
        Debug.Assert(_framebuffer.Length == VgaSegmentByteCount, "Framebuffer buffer size mismatch.");
        Debug.Assert(_palette.Length == 256 * 3, "Palette buffer size mismatch.");

        using var lockedFramebuffer = bitmap.Lock();
        int stride = lockedFramebuffer.RowBytes;
        Debug.Assert(stride >= FrameWidth * 4, "Bitmap stride should fit a full BGRA row.");
        byte[] bgra = new byte[stride * FrameHeight];

        for (int y = 0; y < FrameHeight; y++)
        {
            int rowOffset = y * FrameWidth;
            int bgraRowOffset = y * stride;
            for (int x = 0; x < FrameWidth; x++)
            {
                byte index = _framebuffer[rowOffset + x];
                int paletteOffset = index * 3;
                byte r = (byte)(_palette[paletteOffset + 0] << 2);
                byte g = (byte)(_palette[paletteOffset + 1] << 2);
                byte b = (byte)(_palette[paletteOffset + 2] << 2);

                int pixel = bgraRowOffset + (x * 4);
                bgra[pixel + 0] = b;
                bgra[pixel + 1] = g;
                bgra[pixel + 2] = r;
                bgra[pixel + 3] = 0xFF;
            }
        }

        Marshal.Copy(bgra, 0, lockedFramebuffer.Address, bgra.Length);
    }

    private void PrimeInitialPaletteChunk()
    {
        Debug.Assert(_chunks.Count >= 0, "Chunk collection should be initialized.");

        if (_chunks.Count == 0)
        {
            return;
        }

        HnmChunk firstChunk = _chunks[0];
        ReadOnlySpan<byte> payload = firstChunk.GetPayloadSpan();
        if (payload.Length < 4)
        {
            throw new InvalidDataException($"Initial chunk at offset 0x{firstChunk.Offset:X} is too short for the original palette header contract.");
        }

        ushort controlWord = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(2, 2));
        if ((controlWord & 0x00FF) != 0)
        {
            throw new InvalidDataException($"Initial chunk at offset 0x{firstChunk.Offset:X} must be palette-only (CL == 0) before the first display chunk.");
        }

        if (!HnmPaletteParser.TryApplyPaletteRecords(payload, _palette))
        {
            throw new InvalidDataException($"Initial palette chunk at offset 0x{firstChunk.Offset:X} failed strict decode during startup palette priming.");
        }

        _currentChunkIndex = 1;

        Debug.Assert(_palette.Length == 256 * 3, "Palette buffer should remain fixed size.");
    }

    private void PrimeInitialDisplayChunk()
    {
        if (_currentChunkIndex >= _chunks.Count)
        {
            PendingWaitMilliseconds = 0;
            return;
        }

        // CirclesAnimation_1000_0DDE_10DDE performs the startup palette loop, advances to the
        // first display chunk, and then immediately enters CommonUnknown_1000_0E49_10E49.
        // That means the first HNM frame is drawn during load before the visible CircleMainLoop
        // prelude transforms it and begins the 0xFB timed palette steps.
        HnmChunk chunk = _chunks[_currentChunkIndex];
        ApplyDisplayChunk(chunk);
        _currentChunkIndex++;
    }

    private void InitializeCirclesPaletteAnimationState()
    {
        if (CirclesPaletteData.Buffer.Length != CirclesPaletteBufferLength)
        {
            throw new InvalidOperationException($"Embedded circles palette buffer must be exactly 0x{CirclesPaletteBufferLength:X} bytes.");
        }

        if (CirclesPaletteData.Table.Length != CirclesPaletteTableLength)
        {
            throw new InvalidOperationException($"Embedded circles palette table must cover CS:0x{CirclesPaletteTableOffset:X}..0x{CirclesPaletteTableEndOffset:X} exactly.");
        }

        Array.Copy(CirclesPaletteData.Buffer, _circlesPaletteBuffer, CirclesPaletteBufferLength);
        Array.Copy(CirclesPaletteData.Table, _circlesPaletteTable, CirclesPaletteTableLength);

        _circlesPaletteStepWidth = CirclesPaletteData.StepWidth;
        if (_circlesPaletteStepWidth == 0)
        {
            throw new InvalidOperationException("Embedded circles palette step width must be non-zero.");
        }

        int shiftedByteCount = _circlesPaletteStepWidth * 3;
        if (shiftedByteCount > CirclesPaletteBufferLength)
        {
            throw new InvalidDataException($"Circles palette step width {_circlesPaletteStepWidth} exceeds the 0x{CirclesPaletteBufferLength:X} palette buffer.");
        }

        _circlesPaletteTablePosition = 0;
        _circlesPaletteFramesRemaining = BinaryPrimitives.ReadUInt16LittleEndian(_circlesPaletteTable.AsSpan(0, 2));
        if (_circlesPaletteFramesRemaining == 0)
        {
            throw new InvalidDataException("Circles palette table starts with a zero frame count.");
        }

        _circlesPaletteAccumulatorR = 0;
        _circlesPaletteAccumulatorG = 0;
        _circlesPaletteAccumulatorB = 0;
        _circlesPaletteStateInitialized = true;
    }

    private void ApplyCirclesPreludeFramebufferTransform()
    {
        ushort si = 0;
        ushort di = 0x0140;
        ushort dx = 0x0064;
        do
        {
            ushort cx = 0x0050;
            do
            {
                ushort value = ReadFramebufferWord(si);
                si = (ushort)(si + 2);
                value = (ushort)((value >> 8) | (value << 8));
                di = (ushort)(di - 2);
                WriteFramebufferWord(di, value);
                cx = (ushort)(cx - 1);
            }
            while (cx != 0);

            si = (ushort)(si + 0x00A0);
            di = (ushort)(di + 0x01E0);
            dx = (ushort)(dx - 1);
        }
        while (dx != 0);

        si = 0;
        di = 0xF8C0;
        dx = 0x0064;
        do
        {
            ushort cx = 0x00A0;
            while (cx != 0)
            {
                cx = (ushort)(cx - 1);
                WriteFramebufferWord(di, ReadFramebufferWord(si));
                si = (ushort)(si + 2);
                di = (ushort)(di + 2);
            }

            di = (ushort)(di - 0x0280);
            dx = (ushort)(dx - 1);
        }
        while (dx != 0);
    }

    private ushort ReadFramebufferWord(ushort offset)
    {
        byte low = _framebuffer[offset];
        byte high = _framebuffer[(ushort)(offset + 1)];
        return (ushort)(low | (high << 8));
    }

    private void WriteFramebufferWord(ushort offset, ushort value)
    {
        _framebuffer[offset] = (byte)(value & 0x00FF);
        _framebuffer[(ushort)(offset + 1)] = (byte)(value >> 8);
    }

    private bool AdvanceCirclesPaletteAnimationState(bool applyPalette)
    {
        if (!_circlesPaletteStateInitialized)
        {
            throw new InvalidOperationException("Circles palette state must be initialized before playback can advance.");
        }

        // CirclesDrawStep_1000_0D22_10D22 skips the update when the frame counter is negative.
        if ((_circlesPaletteFramesRemaining & 0x8000) != 0)
        {
            return false;
        }

        int shiftedByteCount = _circlesPaletteStepWidth * 3;
        Array.Copy(_circlesPaletteBuffer, shiftedByteCount, _circlesPaletteBuffer, 0, CirclesPaletteBufferLength - shiftedByteCount);

        int writeOffset = CirclesPaletteBufferLength - shiftedByteCount;
        for (int i = 0; i < _circlesPaletteStepWidth; i++)
        {
            writeOffset = AppendNextCirclesPaletteTriplet(writeOffset);
        }

        if (!applyPalette)
        {
            return false;
        }

        _circlesPaletteBuffer.AsSpan(0, CirclesPaletteTargetEntryCount * 3).CopyTo(_palette.AsSpan(CirclesPaletteTargetIndex * 3, CirclesPaletteTargetEntryCount * 3));
        return true;
    }

    private int AppendNextCirclesPaletteTriplet(int writeOffset)
    {
        _circlesPaletteFramesRemaining = (ushort)(_circlesPaletteFramesRemaining - 1);
        if (_circlesPaletteFramesRemaining == 0)
        {
            _circlesPaletteTablePosition += 8;
            if (_circlesPaletteTablePosition + 8 > _circlesPaletteTable.Length)
            {
                throw new InvalidDataException($"Circles palette table ran past 0x{CirclesPaletteTableEndOffset:X} while advancing to the next entry.");
            }

            _circlesPaletteFramesRemaining = BinaryPrimitives.ReadUInt16LittleEndian(_circlesPaletteTable.AsSpan(_circlesPaletteTablePosition, 2));
            if (_circlesPaletteFramesRemaining == 0)
            {
                throw new InvalidDataException($"Circles palette table entry at 0x{CirclesPaletteTableOffset + _circlesPaletteTablePosition:X} has a zero frame count.");
            }
        }

        _circlesPaletteAccumulatorR = AccumulateCirclesPaletteChannel(_circlesPaletteAccumulatorR, BinaryPrimitives.ReadUInt16LittleEndian(_circlesPaletteTable.AsSpan(_circlesPaletteTablePosition + 2, 2)), out byte r);
        _circlesPaletteAccumulatorG = AccumulateCirclesPaletteChannel(_circlesPaletteAccumulatorG, BinaryPrimitives.ReadUInt16LittleEndian(_circlesPaletteTable.AsSpan(_circlesPaletteTablePosition + 4, 2)), out byte g);
        _circlesPaletteAccumulatorB = AccumulateCirclesPaletteChannel(_circlesPaletteAccumulatorB, BinaryPrimitives.ReadUInt16LittleEndian(_circlesPaletteTable.AsSpan(_circlesPaletteTablePosition + 6, 2)), out byte b);

        _circlesPaletteBuffer[writeOffset + 0] = r;
        _circlesPaletteBuffer[writeOffset + 1] = g;
        _circlesPaletteBuffer[writeOffset + 2] = b;
        return writeOffset + 3;
    }

    private static ushort AccumulateCirclesPaletteChannel(ushort accumulator, ushort delta, out byte output)
    {
        ushort sum = (ushort)(accumulator + delta);
        output = (byte)(((ushort)(sum + 0x0080) >> 8) & 0x3F);
        return sum;
    }

    private (bool changed, string chunkAction) ApplyDisplayChunk(HnmChunk chunk)
    {
        ReadOnlySpan<byte> payload = chunk.GetPayloadSpan();
        if (payload.Length < 4)
        {
            throw new InvalidDataException($"Display chunk at offset 0x{chunk.Offset:X} is too short for flags/control.");
        }

        ushort controlWord = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(2, 2));

        // CommonUnknown_display_1000_0E59_10E59 always reads DI/CX from the current chunk.
        // CL == 0 means "no display work for this chunk"; otherwise it enters the frame path.
        if ((controlWord & 0x00FF) == 0)
        {
            return (false, "header-skip");
        }

        bool changed = HnmFrameParser.TryRenderFrameChunk(payload, _framebuffer, _compressedFrameBuffer)
            ? true
            : throw new InvalidDataException($"Frame chunk at offset 0x{chunk.Offset:X} failed strict decode.");
        return (changed, $"frame 0x{(controlWord >> 8):X2}");
    }

    [Conditional("DEBUG")]
    private static void AssertChunkLayout(IReadOnlyList<HnmChunk> chunks, int sourceLength)
    {
        int previousOffset = -1;
        foreach (HnmChunk chunk in chunks)
        {
            Debug.Assert(chunk.Length >= 2, "Chunk length should include size field.");
            Debug.Assert(chunk.Offset > previousOffset, "Chunk offsets should strictly increase.");
            Debug.Assert(chunk.Offset + chunk.Length <= sourceLength, "Chunk range exceeds source file length.");
            previousOffset = chunk.Offset;
        }
    }
}

public static class HnmFileParser
{
    public static void ParseChunks(byte[] bytes, List<HnmChunk> chunks)
    {
        Debug.Assert(bytes is not null, "Source byte array is required.");
        Debug.Assert(chunks is not null, "Target chunk list is required.");

        chunks.Clear();
        int offset = 0;

        while (offset + 2 <= bytes.Length)
        {
            ushort size = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(offset, 2));
            if (size == 0)
            {
                break;
            }

            Debug.Assert(size >= 2, "Chunk size should include at least the size field.");

            int chunkLength = size;
            if (offset + chunkLength > bytes.Length)
            {
                throw new InvalidDataException($"Chunk at offset 0x{offset:X} claims length 0x{chunkLength:X} beyond source length 0x{bytes.Length:X}.");
            }

            var chunk = HnmChunk.Create(bytes, offset, chunkLength);
            chunks.Add(chunk);
            offset += size;
            Debug.Assert(offset > chunk.Offset, "Offset must progress after chunk consumption.");
        }

        Debug.Assert(offset <= bytes.Length || chunks.Count > 0, "Parser should never walk far beyond source length.");
    }
}

public readonly record struct HnmChunk(byte[] Bytes, int Offset, int Length)
{
    public static HnmChunk Create(byte[] bytes, int offset, int length)
    {
        Debug.Assert(bytes is not null, "Chunk source bytes are required.");
        Debug.Assert(offset >= 0 && length >= 0, "Chunk offset and length must be non-negative.");
        Debug.Assert(offset <= bytes.Length, "Chunk offset is outside source bounds.");

        return new HnmChunk(bytes, offset, length);
    }

    public ReadOnlySpan<byte> GetPayloadSpan()
    {
        Debug.Assert(Offset >= 0 && Length >= 0, "Invalid chunk geometry.");
        Debug.Assert(Offset + Length <= Bytes.Length, "Chunk exceeds source bounds.");

        if (Length <= 2)
        {
            return ReadOnlySpan<byte>.Empty;
        }

        return new ReadOnlySpan<byte>(Bytes, Offset + 2, Length - 2);
    }
}

public readonly record struct HnmStepResult(bool Advanced, bool VisualChanged, bool PlaybackComplete, int WaitBiosTicks, double WaitMilliseconds);

public static class HnmPaletteParser
{
    public static bool TryApplyPaletteRecords(ReadOnlySpan<byte> payload, byte[] palette)
    {
        Debug.Assert(palette.Length == 256 * 3, "Palette storage must be exactly 256 RGB entries.");

        int position = 0;
        bool any = false;

        while (position < payload.Length)
        {
            byte index = payload[position];
            if (index == 0xFF)
            {
                return any;
            }

            if (position + 2 > payload.Length)
            {
                return false;
            }

            byte count = payload[position + 1];
            Debug.Assert(index + count <= 0x100, "Palette range must remain within 256 entries.");
            int dataLength = count * 3;
            if (position + 2 + dataLength > payload.Length)
            {
                return false;
            }

            int paletteOffset = index * 3;
            payload.Slice(position + 2, dataLength).CopyTo(palette.AsSpan(paletteOffset, dataLength));
            position += 2 + dataLength;
            any = true;
        }

        return any;
    }
}

public static class HnmFrameParser
{
    public static bool TryRenderFrameChunk(ReadOnlySpan<byte> payload, byte[] framebuffer, byte[] compressedFrameBuffer)
    {
        Debug.Assert(framebuffer.Length == HnmPlaybackEngine.VgaSegmentByteCount, "Framebuffer must model the full 64K VGA segment.");
        Debug.Assert(compressedFrameBuffer.Length == HnmPlaybackEngine.VgaSegmentByteCount, "Compressed frame buffer must model the original 64K auxiliary segment.");

        if (payload.Length < 8)
        {
            return false;
        }

        ushort flags = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(0, 2));
        ushort controlWord = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(2, 2));
        Debug.Assert((controlWord >> 8) >= 0xFE, "Frame mode should be FE or FF for blit path.");
        if ((controlWord & 0x00FF) == 0)
        {
            return false;
        }

        ushort dx;
        ushort bx;
        ReadOnlySpan<byte> source;

        if ((flags & 0x0200) != 0)
        {
            if (payload.Length < 10)
            {
                return false;
            }

            // Original code consumes flags/control, then the decoder prologue skips an
            // additional 6-byte compressed-frame preamble before reading bitstream data.
            int decodedLength = HnmBitstreamDecoder.Decode(payload.Slice(10), compressedFrameBuffer);
            Debug.Assert(decodedLength >= 0 && decodedLength <= compressedFrameBuffer.Length, "Decoded length must be in destination range.");
            if (decodedLength < 4)
            {
                return false;
            }

            flags = (ushort)(flags & 0xFDFF);
            dx = BinaryPrimitives.ReadUInt16LittleEndian(compressedFrameBuffer.AsSpan(0, 2));
            bx = BinaryPrimitives.ReadUInt16LittleEndian(compressedFrameBuffer.AsSpan(2, 2));

            // CommonUnknown_1000_0EBD_10EBD restores DS to the fixed auxiliary segment and the
            // blitter then reads from that 64K segment starting at SI=4. The original code does
            // not communicate the decoder's output length to CommonUnknown_1000_0B9A_10B9A.
            source = compressedFrameBuffer.AsSpan(4);
        }
        else
        {
            dx = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(4, 2));
            bx = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(6, 2));
            source = payload.Slice(8);
        }

        Debug.Assert((flags & 0x01FF) <= 0x01FF, "Blit width must remain a 9-bit field.");
        HnmBlitter.Blit(source, framebuffer, flags, controlWord, dx, bx);
        return true;
    }
}

public static class HnmBitstreamDecoder
{
    public static int Decode(ReadOnlySpan<byte> source, Span<byte> destination)
    {
        Debug.Assert(destination.Length > 0, "Destination buffer must not be empty.");

        int si = 0;
        int di = 0;
        ushort bp = 0;
        ushort ax = 0;
        ushort cx = 0;
        bool carry = false;
        bool zero;

        static (ushort value, bool carry, bool zero) Shr(ushort value)
        {
            bool carryOut = (value & 1) != 0;
            ushort result = (ushort)(value >> 1);
            return (result, carryOut, result == 0);
        }

        static (ushort value, bool carry, bool zero) Rcr(ushort value, bool carryIn)
        {
            bool carryOut = (value & 1) != 0;
            ushort result = (ushort)((value >> 1) | (carryIn ? 0x8000 : 0));
            return (result, carryOut, result == 0);
        }

        static (ushort value, bool carry, bool zero) Rcl(ushort value, bool carryIn)
        {
            bool carryOut = (value & 0x8000) != 0;
            ushort result = (ushort)((value << 1) | (carryIn ? 1 : 0));
            return (result, carryOut, result == 0);
        }

        static ushort ReadUInt16(ReadOnlySpan<byte> bytes, int offset)
        {
            // Per NO FALLBACK rule: out-of-range reads must fail loudly. The original asm
            // reads two bytes from DS:SI; if our source span is too small, the upstream
            // frame parser produced a malformed payload and we must surface that.
            if (offset + 1 >= bytes.Length)
            {
                throw new InvalidDataException($"Bitstream decoder ran past end of source at offset 0x{offset:X4} (length 0x{bytes.Length:X4}).");
            }
            return (ushort)(bytes[offset] | (bytes[offset + 1] << 8));
        }

        static byte ReadByte(ReadOnlySpan<byte> bytes, int offset, string context)
        {
            if ((uint)offset >= (uint)bytes.Length)
            {
                throw new InvalidDataException($"Bitstream decoder ran past end of source during {context} at offset 0x{offset:X4} (length 0x{bytes.Length:X4}).");
            }
            return bytes[offset];
        }

        int outputStart = di;

        while (true)
        {
            (bp, carry, zero) = Shr(bp);
            if (!zero)
            {
                if (!carry)
                {
                    goto label_4;
                }
            }
            else
            {
                goto label_3;
            }

        label_2:
            // Original asm executes LODSB; STOSB unconditionally. A bounds violation here
            // means the upstream payload is malformed; fail loudly per NO FALLBACK rule.
            if (si >= source.Length)
            {
                throw new InvalidDataException($"Bitstream decoder source exhausted before EOS marker at si=0x{si:X4}.");
            }
            if (di >= destination.Length)
            {
                throw new InvalidDataException($"Bitstream decoder destination overflow at di=0x{di:X4}.");
            }

            destination[di++] = source[si++];
            continue;

        label_3:
            ax = ReadUInt16(source, si);
            si += 2;
            bp = ax;
            carry = true;
            (bp, carry, zero) = Rcr(bp, carry);
            if (carry)
            {
                goto label_2;
            }

        label_4:
            cx = 0;
            (bp, carry, zero) = Shr(bp);
            if (zero)
            {
                ax = ReadUInt16(source, si);
                si += 2;
                bp = ax;
                carry = true;
                (bp, carry, zero) = Rcr(bp, carry);
            }
            if (!carry)
            {
                (bp, carry, zero) = Shr(bp);
                if (zero)
                {
                    ax = ReadUInt16(source, si);
                    si += 2;
                    bp = ax;
                    carry = true;
                    (bp, carry, zero) = Rcr(bp, carry);
                }
                (cx, carry, zero) = Rcl(cx, carry);
                (bp, carry, zero) = Shr(bp);
                if (zero)
                {
                    ax = ReadUInt16(source, si);
                    si += 2;
                    bp = ax;
                    carry = true;
                    (bp, carry, zero) = Rcr(bp, carry);
                }
                (cx, carry, zero) = Rcl(cx, carry);
                // Original asm does LODSB followed by MOV AH,0xFF, producing a signed
                // negative near offset in AX before the ADD AX,DI address calculation.
                ax = (ushort)(0xFF00 | ReadByte(source, si, "short back-reference offset"));
                si++;
            }
            else
            {
                goto label_16;
            }

            goto label_12;

        label_16:
            ax = ReadUInt16(source, si);
            si += 2;
            byte clByte = (byte)(ax & 0xFF);
            ax >>= 1;
            ax >>= 1;
            ax >>= 1;
            ax = (ushort)(ax | 0xE000);
            clByte = (byte)(clByte & 7);
            if (clByte == 0)
            {
                ushort savedAx = ax;
                ax = ReadByte(source, si, "long back-reference length");
                si++;
                clByte = (byte)ax;
                ax = savedAx;
                if (clByte == 0)
                {
                    break;
                }
            }

            // CommonUnknownSplit_1000_0F30_10F30 loads the long back-reference length into CL
            // before falling through label_12, where CX is incremented twice for the final copy
            // count. Preserve that exact handoff instead of leaving CX at zero.
            cx = clByte;

        label_12:
            // Original asm does ADD AX,DI on 16-bit registers before XCHG AX,SI.
            // That is a wraparound address calculation, not a clamp.
            ushort sourceIndex = (ushort)(ax + di);

            int savedSi = si;
            si = sourceIndex;
            ushort backReferenceSi = sourceIndex;
            ++cx;
            cx = (ushort)(cx + 1);
            while (cx != 0)
            {
                cx--;
                if (di >= destination.Length)
                {
                    throw new InvalidDataException($"Back-reference destination overflow: di=0x{di:X4}.");
                }

                // CommonUnknownSplit_1000_0F30_10F30 copies with MOVSB on 16-bit SI/DI.
                // Source reads therefore wrap at 0x10000 instead of faulting on 0xFFFF + 1.
                destination[di++] = destination[backReferenceSi];
                backReferenceSi = (ushort)(backReferenceSi + 1);
            }
            si = savedSi;
            continue;
        }

        Debug.Assert(di >= outputStart && di <= destination.Length, "Decoder output cursor is out of destination range.");
        return di - outputStart;
    }
}

public static class HnmBlitter
{
    // Strict, NO-FALLBACK blitter. Mirrors the original asm in
    // CommonUnknownSplit_1000_0F30_10F30 and related branches. All bounds
    // violations raise InvalidDataException; no silent return paths.

    private static byte NextSource(ReadOnlySpan<byte> source, ref int si)
    {
        if ((uint)si >= (uint)source.Length)
        {
            throw new InvalidDataException($"Blitter source exhausted at si=0x{si:X4} (length 0x{source.Length:X4}).");
        }
        return source[si++];
    }

    private static void WriteDestinationByte(byte[] framebuffer, int destination, byte value)
    {
        framebuffer[(ushort)destination] = value;
    }

    private static int ComputeBaseOffset(ushort dx, ushort bx)
    {
        // ConvertLineNumberToArrayIndex_1000_0A22_10A22 clamps BX to 0xC7 before
        // computing DI = BX * 320 + DX with 16-bit arithmetic. Match that contract exactly.
        ushort clampedRow = bx >= HnmPlaybackEngine.FrameHeight
            ? (ushort)(HnmPlaybackEngine.FrameHeight - 1)
            : bx;
        return (ushort)(clampedRow * HnmPlaybackEngine.FrameWidth + dx);
    }

    public static void Blit(ReadOnlySpan<byte> source, byte[] framebuffer, ushort flags, ushort controlWord, ushort dx, ushort bx)
    {
        byte mode = (byte)(controlWord >> 8);
        int rows = controlWord & 0x00FF;
        int rowWidth = flags & 0x01FF;
        Debug.Assert(framebuffer.Length == HnmPlaybackEngine.VgaSegmentByteCount, "Framebuffer must model the full 64K VGA segment.");
        Debug.Assert(mode >= 0xFE, "Unexpected blit mode for frame payload.");
        Debug.Assert(rows > 0, "Blitter must receive a positive row count.");
        Debug.Assert(rowWidth > 0 && rowWidth <= 0x01FF, "Blitter row width must stay within the original 9-bit field.");

        if (mode < 0xFE)
        {
            throw new InvalidDataException($"Blitter mode 0x{mode:X2} is not supported by the original (must be 0xFE or 0xFF).");
        }

        if (rows <= 0 || rowWidth <= 0)
        {
            throw new InvalidDataException($"Blitter received non-positive geometry rows={rows} width={rowWidth}.");
        }

        int baseOffset = ComputeBaseOffset(dx, bx);
        Debug.Assert(baseOffset >= 0, "Blit base offset should be non-negative before clipping checks.");
        int si = 0;

        if ((flags & 0x8000) == 0)
        {
            if (mode == 0xFF)
            {
                RenderForwardTransparentRows(source, framebuffer, ref si, baseOffset, rowWidth, rows);
                return;
            }

            RenderForwardWordRows(source, framebuffer, ref si, baseOffset, rowWidth, rows);
            return;
        }

        int startOffset = baseOffset;
        int rowStride = HnmPlaybackEngine.FrameWidth;
        if ((flags & 0x2000) != 0)
        {
            startOffset += (rows - 1) * HnmPlaybackEngine.FrameWidth;
            rowStride = -HnmPlaybackEngine.FrameWidth;
        }

        if ((flags & 0x4000) == 0)
        {
            if (mode == 0xFF)
            {
                RenderRleTransparentForward(source, framebuffer, ref si, startOffset, rowWidth, rows, rowStride);
                return;
            }

            RenderRleOpaqueForward(source, framebuffer, ref si, startOffset, rowWidth, rows, rowStride);
            return;
        }

        startOffset += rowWidth - 1;
        if (mode == 0xFF)
        {
            RenderRleTransparentReverse(source, framebuffer, ref si, startOffset, rowWidth, rows, rowStride);
            return;
        }

        RenderRleOpaqueReverse(source, framebuffer, ref si, startOffset, rowWidth, rows, rowStride);
    }

    private static void RenderForwardWordRows(ReadOnlySpan<byte> source, byte[] framebuffer, ref int si, int startOffset, int rowWidth, int rowCount)
    {
        Debug.Assert(rowWidth > 0 && rowCount > 0, "Forward word rows require positive dimensions.");

        int wordCount = rowWidth >> 1;
        bool copyTrailingByte = (rowWidth & 1) != 0;

        for (int row = 0; row < rowCount; row++)
        {
            int rowDestination = startOffset + row * HnmPlaybackEngine.FrameWidth;
            for (int i = 0; i < wordCount; i++)
            {
                WriteDestinationByte(framebuffer, rowDestination++, NextSource(source, ref si));
                WriteDestinationByte(framebuffer, rowDestination++, NextSource(source, ref si));
            }

            if (copyTrailingByte)
            {
                WriteDestinationByte(framebuffer, rowDestination, NextSource(source, ref si));
            }
        }
    }

    private static void RenderForwardTransparentRows(ReadOnlySpan<byte> source, byte[] framebuffer, ref int si, int startOffset, int rowWidth, int rowCount)
    {
        Debug.Assert(rowWidth > 0 && rowCount > 0, "Forward transparent rows require positive dimensions.");

        for (int row = 0; row < rowCount; row++)
        {
            int rowDestination = startOffset + row * HnmPlaybackEngine.FrameWidth;
            for (int i = 0; i < rowWidth; i++)
            {
                byte value = NextSource(source, ref si);
                if (value != 0)
                {
                    WriteDestinationByte(framebuffer, rowDestination, value);
                }
                rowDestination++;
            }
        }
    }

    private static void RenderRleTransparentForward(ReadOnlySpan<byte> source, byte[] framebuffer, ref int si, int startOffset, int rowWidth, int rowCount, int rowStride)
    {
        Debug.Assert(rowWidth > 0 && rowCount > 0, "RLE transparent forward requires positive dimensions.");

        int rowStart = startOffset;
        for (int row = 0; row < rowCount; row++)
        {
            int di = rowStart;
            int remaining = rowWidth;
            while (remaining > 0)
            {
                byte op = NextSource(source, ref si);
                if ((op & 0x80) == 0)
                {
                    int count = op + 1;
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE literal run {count} exceeds remaining row width {remaining}.");
                    }

                    for (int i = 0; i < count; i++)
                    {
                        byte value = NextSource(source, ref si);
                        if (value != 0)
                        {
                            WriteDestinationByte(framebuffer, di, value);
                        }
                        di++;
                    }

                    remaining -= count;
                }
                else
                {
                    int count = 257 - op;
                    byte value = NextSource(source, ref si);
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE fill run {count} exceeds remaining row width {remaining}.");
                    }

                    if (value == 0)
                    {
                        di += count;
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            WriteDestinationByte(framebuffer, di++, value);
                        }
                    }

                    remaining -= count;
                }
            }

            rowStart += rowStride;
        }
    }

    private static void RenderRleOpaqueForward(ReadOnlySpan<byte> source, byte[] framebuffer, ref int si, int startOffset, int rowWidth, int rowCount, int rowStride)
    {
        Debug.Assert(rowWidth > 0 && rowCount > 0, "RLE opaque forward requires positive dimensions.");

        int rowStart = startOffset;
        for (int row = 0; row < rowCount; row++)
        {
            int di = rowStart;
            int remaining = rowWidth;
            while (remaining > 0)
            {
                byte op = NextSource(source, ref si);
                if ((op & 0x80) == 0)
                {
                    int count = op + 1;
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE literal run {count} exceeds remaining row width {remaining}.");
                    }

                    for (int i = 0; i < count; i++)
                    {
                        WriteDestinationByte(framebuffer, di++, NextSource(source, ref si));
                    }

                    remaining -= count;
                }
                else
                {
                    int count = 257 - op;
                    byte value = NextSource(source, ref si);
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE fill run {count} exceeds remaining row width {remaining}.");
                    }

                    for (int i = 0; i < count; i++)
                    {
                        WriteDestinationByte(framebuffer, di++, value);
                    }

                    remaining -= count;
                }
            }

            rowStart += rowStride;
        }
    }

    private static void RenderRleTransparentReverse(ReadOnlySpan<byte> source, byte[] framebuffer, ref int si, int startOffset, int rowWidth, int rowCount, int rowStride)
    {
        Debug.Assert(rowWidth > 0 && rowCount > 0, "RLE transparent reverse requires positive dimensions.");

        int rowStart = startOffset;
        for (int row = 0; row < rowCount; row++)
        {
            int di = rowStart;
            int remaining = rowWidth;
            while (remaining > 0)
            {
                byte op = NextSource(source, ref si);
                // The reverse-family asm bodies at 0xAD5/0xB64 do not reuse the forward
                // sign-bit selector. They take op==0 as the fill/skip branch and any non-zero
                // op as a literal run of op+1 bytes.
                if (op != 0)
                {
                    int count = op + 1;
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE literal run {count} exceeds remaining row width {remaining}.");
                    }

                    for (int i = 0; i < count; i++)
                    {
                        byte value = NextSource(source, ref si);
                        if (value != 0)
                        {
                            WriteDestinationByte(framebuffer, di, value);
                        }
                        di--;
                    }

                    remaining -= count;
                }
                else
                {
                    int count = 257 - op;
                    byte value = NextSource(source, ref si);
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE fill run {count} exceeds remaining row width {remaining}.");
                    }

                    if (value == 0)
                    {
                        di -= count;
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            WriteDestinationByte(framebuffer, di--, value);
                        }
                    }

                    remaining -= count;
                }
            }

            rowStart += rowStride;
        }
    }

    private static void RenderRleOpaqueReverse(ReadOnlySpan<byte> source, byte[] framebuffer, ref int si, int startOffset, int rowWidth, int rowCount, int rowStride)
    {
        Debug.Assert(rowWidth > 0 && rowCount > 0, "RLE opaque reverse requires positive dimensions.");

        int rowStart = startOffset;
        for (int row = 0; row < rowCount; row++)
        {
            int di = rowStart;
            int remaining = rowWidth;
            while (remaining > 0)
            {
                byte op = NextSource(source, ref si);
                if (op != 0)
                {
                    int count = op + 1;
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE literal run {count} exceeds remaining row width {remaining}.");
                    }

                    for (int i = 0; i < count; i++)
                    {
                        WriteDestinationByte(framebuffer, di--, NextSource(source, ref si));
                    }

                    remaining -= count;
                }
                else
                {
                    int count = 257 - op;
                    byte value = NextSource(source, ref si);
                    if (count > remaining)
                    {
                        throw new InvalidDataException($"RLE fill run {count} exceeds remaining row width {remaining}.");
                    }

                    for (int i = 0; i < count; i++)
                    {
                        WriteDestinationByte(framebuffer, di--, value);
                    }

                    remaining -= count;
                }
            }

            rowStart += rowStride;
        }
    }
}