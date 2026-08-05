using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Diagnostics;

namespace logo;

public sealed class HeadlessCaptureWindow : Window
{
    public HeadlessCaptureWindow()
    {
        Opened += OnOpened;
    }

    private async void OnOpened(object? sender, EventArgs e)
    {
        Debug.Assert(PlayerLaunchContext.Current.Headless, "Headless capture window should only be used in headless mode.");
        if (string.IsNullOrWhiteSpace(PlayerLaunchContext.Current.InputPath))
        {
            throw new InvalidOperationException("Headless mode requires an input path.");
        }

        if (string.IsNullOrWhiteSpace(PlayerLaunchContext.Current.ScreenshotDirectory))
        {
            throw new InvalidOperationException("Headless mode requires a screenshot directory.");
        }

        string inputPath = PlayerLaunchContext.Current.InputPath;
        string screenshotDirectory = PlayerLaunchContext.Current.ScreenshotDirectory;

        await HeadlessPlayerRunner.RunAsync(inputPath, screenshotDirectory);
        Close();
    }
}