using SixLabors.ImageSharp.PixelFormats;

namespace Hnm0Encoder;

internal sealed record QuantizedFrame(
    int Width,
    int Height,
    Rgba32[] Palette,
    byte[] Indices)
{
    public int PaletteLength => Palette.Length;
    public int IndexCount => Indices.Length;
}
