using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using System.Runtime.Versioning;

namespace logo;

public sealed class App : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }

    [SupportedOSPlatform("windows")]
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = PlayerLaunchContext.Current.Headless
                ? new HeadlessCaptureWindow()
                : new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}