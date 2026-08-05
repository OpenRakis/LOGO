using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace Hnm0Encoder;

internal static class FrameQuantizer
{
    public static QuantizedFrame Quantize(CanonicalFrame frame)
    {
        if (frame.Width != FramePreflight.ExpectedWidth || frame.Height != FramePreflight.ExpectedHeight)
        {
            throw new InvalidOperationException(
                $"Canonical frame must be normalized to {FramePreflight.ExpectedWidth}x{FramePreflight.ExpectedHeight} before quantization.");
        }

        using Image<Rgba32> image = Image.LoadPixelData<Rgba32>(frame.Pixels, frame.Width, frame.Height);
        var options = new QuantizerOptions
        {
            MaxColors = 256,
            Dither = null,
            DitherScale = 0,
            ColorMatchingMode = ColorMatchingMode.Coarse
        };

        OctreeQuantizer<Rgba32> quantizer = new(Configuration.Default, options);
        IndexedImageFrame<Rgba32> indexed = QuantizerUtilities.BuildPaletteAndQuantizeFrame(
            quantizer,
            image.Frames.RootFrame,
            new Rectangle(0, 0, frame.Width, frame.Height));

        Rgba32[] palette = indexed.Palette.ToArray();
        byte[] indices = new byte[frame.Width * frame.Height];

        for (int y = 0; y < frame.Height; y++)
        {
            ReadOnlySpan<byte> row = indexed.DangerousGetRowSpan(y);
            row.CopyTo(indices.AsSpan(y * frame.Width, frame.Width));
        }

        if (palette.Length == 0 || palette.Length > 256)
        {
            throw new InvalidDataException($"Quantized palette size {palette.Length} is outside the supported 0..256 range.");
        }

        return new QuantizedFrame(frame.Width, frame.Height, palette, indices);
    }
}
