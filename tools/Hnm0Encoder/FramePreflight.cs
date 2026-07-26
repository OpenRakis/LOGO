using SixLabors.ImageSharp.PixelFormats;

namespace Hnm0Encoder;

internal static class FramePreflight
{
    public const int ExpectedWidth = 320;
    public const int ExpectedHeight = 200;

    public static FramePreflightReport Analyze(IReadOnlyList<string> framePaths, bool verbose)
    {
        if (framePaths.Count == 0)
        {
            throw new InvalidOperationException("Cannot analyze an empty frame list.");
        }

        int maxColors = 0;
        int minColors = int.MaxValue;

        for (int i = 0; i < framePaths.Count; i++)
        {
            string framePath = framePaths[i];
            CanonicalFrame frame = FrameLoader.LoadCanonicalFrame(framePath);

            int uniqueColors = CountUniqueColors(frame.Pixels);
            QuantizedFrame quantized = FrameQuantizer.Quantize(frame);
            int paletteColors = quantized.PaletteLength;
            maxColors = Math.Max(maxColors, paletteColors);
            minColors = Math.Min(minColors, paletteColors);

            if (verbose)
            {
                Console.WriteLine($"[preflight] {i:D5} {Path.GetFileName(framePath)} sourceColors={uniqueColors} paletteColors={paletteColors}");
            }
        }

        return new FramePreflightReport(framePaths.Count, minColors, maxColors);
    }

    private static int CountUniqueColors(ReadOnlySpan<Rgba32> pixels)
    {
        var colors = new HashSet<Rgba32>();

        for (int i = 0; i < pixels.Length; i++)
        {
            colors.Add(pixels[i]);
        }

        return colors.Count;
    }
}

internal sealed record FramePreflightReport(
    int FrameCount,
    int MinUniqueColorsInFrame,
    int MaxUniqueColorsInFrame);
