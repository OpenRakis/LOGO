using System.Buffers.Binary;

namespace Hnm0Encoder;

internal static class HnmStreamValidator
{
    private const int FrameWidth = 320;
    private const int FrameHeight = 200;

    public static void Validate(byte[] bytes, int expectedFrameCount)
    {
        if (bytes.Length < 2)
        {
            throw new InvalidDataException("Encoded stream is too short to contain even one chunk size field.");
        }

        int offset = 0;
        int chunkIndex = 0;

        while (offset + 2 <= bytes.Length)
        {
            ushort size = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(offset, 2));
            if (size == 0)
            {
                break;
            }

            if (size < 2)
            {
                throw new InvalidDataException($"Chunk {chunkIndex} at offset 0x{offset:X} has invalid size 0x{size:X}.");
            }

            int chunkEnd = offset + size;
            if (chunkEnd > bytes.Length)
            {
                throw new InvalidDataException($"Chunk {chunkIndex} at offset 0x{offset:X} extends beyond encoded length 0x{bytes.Length:X}.");
            }

            ReadOnlySpan<byte> payload = bytes.AsSpan(offset + 2, size - 2);
            if (chunkIndex == 0)
            {
                ValidatePaletteChunk(payload);
            }
            else
            {
                ValidateFrameChunk(payload, chunkIndex - 1);
            }

            offset = chunkEnd;
            chunkIndex++;
        }

        if (chunkIndex != expectedFrameCount + 1)
        {
            throw new InvalidDataException($"Encoded stream contains {chunkIndex - 1} frame chunk(s) but {expectedFrameCount} were expected.");
        }

        if (offset != bytes.Length)
        {
            throw new InvalidDataException($"Encoded stream has trailing bytes after the final chunk at offset 0x{offset:X}.");
        }
    }

    private static void ValidatePaletteChunk(ReadOnlySpan<byte> payload)
    {
        if (payload.Length < 3)
        {
            throw new InvalidDataException("Palette chunk is too short to hold at least one palette record and terminator.");
        }

        int position = 0;
        bool sawRecord = false;

        while (position < payload.Length)
        {
            byte index = payload[position];
            if (index == 0xFF)
            {
                if (!sawRecord)
                {
                    throw new InvalidDataException("Palette chunk terminator appeared before any palette records.");
                }

                return;
            }

            if (position + 2 > payload.Length)
            {
                throw new InvalidDataException($"Palette record at offset 0x{position:X} is truncated.");
            }

            byte count = payload[position + 1];
            int recordLength = 2 + (count * 3);
            if (position + recordLength > payload.Length)
            {
                throw new InvalidDataException($"Palette record at offset 0x{position:X} extends beyond the chunk payload.");
            }

            position += recordLength;
            sawRecord = true;
        }

        throw new InvalidDataException("Palette chunk is missing the required 0xFF terminator.");
    }

    private static void ValidateFrameChunk(ReadOnlySpan<byte> payload, int frameIndex)
    {
        if (payload.Length < 8)
        {
            throw new InvalidDataException($"Frame chunk {frameIndex} is too short to contain the mandatory frame header.");
        }

        ushort flags = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(0, 2));
        ushort controlWord = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(2, 2));
        ushort dx = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(4, 2));
        ushort bx = BinaryPrimitives.ReadUInt16LittleEndian(payload.Slice(6, 2));

        if ((controlWord >> 8) < 0xFE)
        {
            throw new InvalidDataException($"Frame chunk {frameIndex} uses unsupported frame mode 0x{(controlWord >> 8):X2}.");
        }

        if ((flags & 0x01FF) == 0)
        {
            throw new InvalidDataException($"Frame chunk {frameIndex} has zero row width.");
        }

        if (dx >= FrameWidth)
        {
            throw new InvalidDataException($"Frame chunk {frameIndex} has DX=0x{dx:X4} outside the visible frame width.");
        }

        if (bx >= FrameHeight)
        {
            throw new InvalidDataException($"Frame chunk {frameIndex} has BX=0x{bx:X4} outside the visible frame height.");
        }
    }
}
