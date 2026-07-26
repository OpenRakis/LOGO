using Avalonia;
using Avalonia.Headless;

namespace logo;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        PlayerLaunchOptions launchOptions = PlayerCommandLineParser.Parse(args);
        PlayerLaunchContext.Current = launchOptions;
        BuildAvaloniaApp(launchOptions).StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp(PlayerLaunchOptions launchOptions)
    {
        AppBuilder builder = AppBuilder.Configure<App>();
        if (launchOptions.Headless)
        {
            builder = builder.UseHeadless(new Avalonia.Headless.AvaloniaHeadlessPlatformOptions());
        }
        else
        {
            builder = builder.UsePlatformDetect();
        }

        return builder;
    }
}