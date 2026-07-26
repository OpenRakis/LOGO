using SixLabors.ImageSharp.PixelFormats;

namespace Hnm0Encoder;

internal static class PaletteRecordWriter
{
    public static byte[] BuildPaletteRecordPayload(Rgba32[] palette)
    {
        if (palette.Length == 0)
        {
            throw new InvalidDataException("Quantized palette must contain at least one color.");
        }

        if (palette.Length > 256)
        {
            throw new InvalidDataException($"Quantized palette length {palette.Length} exceeds the 256-color HNM limit.");
        }

        using var buffer = new MemoryStream();
        int position = 0;
        while (position < palette.Length)
        {
            int remaining = palette.Length - position;
            int count = remaining > byte.MaxValue ? byte.MaxValue : remaining;
            buffer.WriteByte((byte)position);
            buffer.WriteByte((byte)count);

            for (int i = 0; i < count; i++)
            {
                Rgba32 color = palette[position + i];
                buffer.WriteByte(ReduceToVgaDac(color.R));
                buffer.WriteByte(ReduceToVgaDac(color.G));
                buffer.WriteByte(ReduceToVgaDac(color.B));
            }

            position += count;
        }

        buffer.WriteByte(0xFF);
        return buffer.ToArray();
    }

    private static byte ReduceToVgaDac(byte value)
    {
        return (byte)(value >> 2);
    }
}
