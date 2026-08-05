using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Diagnostics;

namespace logo;

internal static class HeadlessPlayerRunner
{
    public static async Task RunAsync(string inputPath, string screenshotDirectory)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(inputPath), "Input path must not be empty.");
        Debug.Assert(!string.IsNullOrWhiteSpace(screenshotDirectory), "Screenshot directory must not be empty.");

        Directory.CreateDirectory(screenshotDirectory);

        var engine = new HnmPlaybackEngine();
        await engine.LoadAsync(inputPath);

        using WriteableBitmap bitmap = new(
            new PixelSize(HnmPlaybackEngine.FrameWidth, HnmPlaybackEngine.FrameHeight),
            new Vector(96, 96),
            Avalonia.Platform.PixelFormat.Bgra8888,
            AlphaFormat.Opaque);

        int screenshotIndex = 0;
        SaveScreenshot(engine, bitmap, Path.Combine(screenshotDirectory, FormatScreenshotName(screenshotIndex++)));

        while (true)
        {
            HnmStepResult stepResult = engine.Step();
            if (!stepResult.Advanced)
            {
                break;
            }

            if (stepResult.VisualChanged)
            {
                SaveScreenshot(engine, bitmap, Path.Combine(screenshotDirectory, FormatScreenshotName(screenshotIndex++)));
            }
        }
    }

    private static void SaveScreenshot(HnmPlaybackEngine engine, WriteableBitmap bitmap, string path)
    {
        engine.RenderCurrentFrame(bitmap);
        WriteableBitmapToPng(bitmap, path);
    }

    private static void WriteableBitmapToPng(WriteableBitmap bitmap, string path)
    {
        using FileStream stream = File.Create(path);
        bitmap.Save(stream);
    }

    private static string FormatScreenshotName(int index)
    {
        return $"frame{index:0000}.png";
    }
}