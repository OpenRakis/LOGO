using System.Buffers.Binary;
using SixLabors.ImageSharp.PixelFormats;

namespace Hnm0Encoder;

internal static class Hnm0Writer
{
    public static void Write(string outputPath, IReadOnlyList<string> framePaths)
    {
        if (framePaths.Count == 0)
        {
            throw new InvalidOperationException("Cannot encode an empty frame list.");
        }

        var chunks = new List<byte[]>(framePaths.Count + 1);

        CanonicalFrame firstFrame = FrameLoader.LoadCanonicalFrame(framePaths[0]);
        QuantizedFrame firstQuantizedFrame = FrameQuantizer.Quantize(firstFrame);
        chunks.Add(BuildInitialPaletteChunk(firstQuantizedFrame.Palette));
        chunks.Add(BuildFrameChunk(firstQuantizedFrame));

        for (int i = 1; i < framePaths.Count; i++)
        {
            CanonicalFrame frame = FrameLoader.LoadCanonicalFrame(framePaths[i]);
            QuantizedFrame quantized = FrameQuantizer.Quantize(frame);
            chunks.Add(BuildFrameChunk(quantized));
        }

        int totalLength = 0;
        foreach (byte[] chunk in chunks)
        {
            totalLength += chunk.Length;
        }

        byte[] encodedBytes = new byte[totalLength];
        int writeOffset = 0;
        foreach (byte[] chunk in chunks)
        {
            Buffer.BlockCopy(chunk, 0, encodedBytes, writeOffset, chunk.Length);
            writeOffset += chunk.Length;
        }

        File.WriteAllBytes(outputPath, encodedBytes);
        HnmStreamValidator.Validate(encodedBytes, framePaths.Count);
    }

    private static byte[] BuildInitialPaletteChunk(Rgba32[] palette)
    {
        byte[] paletteBytes = PaletteRecordWriter.BuildPaletteRecordPayload(palette);
        return BuildChunk(paletteBytes);
    }

    private static byte[] BuildFrameChunk(QuantizedFrame frame)
    {
        if (frame.Width != FramePreflight.ExpectedWidth || frame.Height != FramePreflight.ExpectedHeight)
        {
            throw new InvalidOperationException(
                $"Quantized frame must be exactly {FramePreflight.ExpectedWidth}x{FramePreflight.ExpectedHeight}.");
        }

        byte[] payload = new byte[8 + frame.Indices.Length];
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(0, 2), 0x0140);
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(2, 2), 0xFEC8);
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(4, 2), 0x0000);
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(6, 2), 0x0000);
        frame.Indices.CopyTo(payload.AsSpan(8));

        return BuildChunk(payload);
    }

    private static byte[] BuildChunk(ReadOnlySpan<byte> payload)
    {
        int totalLength = payload.Length + 2;
        if (totalLength > ushort.MaxValue)
        {
            throw new InvalidDataException($"Chunk length {totalLength} exceeds the 16-bit HNM size field.");
        }

        byte[] chunk = new byte[totalLength];
        BinaryPrimitives.WriteUInt16LittleEndian(chunk.AsSpan(0, 2), (ushort)totalLength);
        payload.CopyTo(chunk.AsSpan(2));
        return chunk;
    }
}
