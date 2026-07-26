using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Hnm0Encoder;

internal static class FrameLoader
{
    public static CanonicalFrame LoadCanonicalFrame(string framePath)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(framePath);
        NormalizeToPlayerFrame(image);

        var pixels = new Rgba32[image.Width * image.Height];
        image.ProcessPixelRows(accessor =>
        {
            int destinationIndex = 0;
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> row = accessor.GetRowSpan(y);
                row.CopyTo(pixels.AsSpan(destinationIndex, row.Length));
                destinationIndex += row.Length;
            }
        });

        return new CanonicalFrame(image.Width, image.Height, pixels);
    }

    private static void NormalizeToPlayerFrame(Image<Rgba32> image)
    {
        if (image.Width == FramePreflight.ExpectedWidth && image.Height == FramePreflight.ExpectedHeight)
        {
            return;
        }

        image.Mutate(context => context.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Stretch,
            Size = new Size(FramePreflight.ExpectedWidth, FramePreflight.ExpectedHeight),
            Sampler = KnownResamplers.Lanczos3
        }));
    }
}
