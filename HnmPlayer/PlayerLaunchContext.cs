namespace logo;

internal static class PlayerLaunchContext
{
    public static PlayerLaunchOptions Current { get; set; } = PlayerLaunchOptions.UiDefault;
}

public sealed record PlayerLaunchOptions(bool Headless, string? InputPath, string? ScreenshotDirectory)
{
    public static PlayerLaunchOptions UiDefault { get; } = new(false, null, null);
}

internal static class PlayerCommandLineParser
{
    public static PlayerLaunchOptions Parse(string[] args)
    {
        bool headless = false;
        string? inputPath = null;
        string? screenshotDirectory = null;

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            switch (arg)
            {
                case "--headless":
                    headless = true;
                    break;
                case "--input":
                    inputPath = ReadValue(args, ref i, arg);
                    break;
                case "--screenshot-dir":
                    screenshotDirectory = ReadValue(args, ref i, arg);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown argument '{arg}'.");
            }
        }

        if (!headless)
        {
            return PlayerLaunchOptions.UiDefault;
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new InvalidOperationException("Missing required argument --input for headless mode.");
        }

        if (string.IsNullOrWhiteSpace(screenshotDirectory))
        {
            throw new InvalidOperationException("Missing required argument --screenshot-dir for headless mode.");
        }

        return new PlayerLaunchOptions(
            true,
            Path.GetFullPath(inputPath),
            Path.GetFullPath(screenshotDirectory));
    }

    private static string ReadValue(string[] args, ref int index, string argName)
    {
        if (index + 1 >= args.Length)
        {
            throw new InvalidOperationException($"Missing value after {argName}.");
        }

        index++;
        string value = args[index];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Argument {argName} requires a non-empty value.");
        }

        return value;
    }
}