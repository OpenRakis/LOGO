using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Diagnostics;

namespace logo;

[SupportedOSPlatform("windows")]
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
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Headless screenshot capture requires Windows GDI+ support.");
        }

        using var png = new System.Drawing.Bitmap(bitmap.PixelSize.Width, bitmap.PixelSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        System.Drawing.Rectangle rect = new(0, 0, png.Width, png.Height);
        BitmapData data = png.LockBits(rect, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        try
        {
            using var lockedFramebuffer = bitmap.Lock();
            int rowLength = bitmap.PixelSize.Width * 4;
            byte[] rowBuffer = new byte[rowLength];

            for (int y = 0; y < bitmap.PixelSize.Height; y++)
            {
                IntPtr sourceRow = IntPtr.Add(lockedFramebuffer.Address, y * lockedFramebuffer.RowBytes);
                Marshal.Copy(sourceRow, rowBuffer, 0, rowLength);
                IntPtr destinationRow = IntPtr.Add(data.Scan0, y * data.Stride);
                Marshal.Copy(rowBuffer, 0, destinationRow, rowLength);
            }
        }
        finally
        {
            png.UnlockBits(data);
        }

        png.Save(path, ImageFormat.Png);
    }

    private static string FormatScreenshotName(int index)
    {
        return $"frame{index:0000}.png";
    }
}