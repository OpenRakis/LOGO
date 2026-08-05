using SixLabors.ImageSharp.PixelFormats;

namespace Hnm0Encoder;

internal sealed record CanonicalFrame(int Width, int Height, Rgba32[] Pixels)
{
    public int PixelCount => Pixels.Length;
}
