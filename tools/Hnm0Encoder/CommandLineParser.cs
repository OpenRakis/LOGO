using System.Globalization;

namespace Hnm0Encoder;

internal static class CommandLineParser
{
    public static EncoderOptions Parse(string[] args)
    {
        if (args.Length == 0)
        {
            throw new InvalidOperationException(BuildUsage());
        }

        string? inputDir = null;
        string? outputPath = null;
        double? fps = null;
        int startIndex = 0;
        int? maxFrames = null;
        bool overwrite = false;
        bool verbose = false;

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            switch (arg)
            {
                case "--input-dir":
                    inputDir = ReadValue(args, ref i, "--input-dir");
                    break;
                case "--output":
                    outputPath = ReadValue(args, ref i, "--output");
                    break;
                case "--fps":
                    fps = ParsePositiveDouble(ReadValue(args, ref i, "--fps"), "--fps");
                    break;
                case "--start-index":
                    startIndex = ParseNonNegativeInt(ReadValue(args, ref i, "--start-index"), "--start-index");
                    break;
                case "--max-frames":
                    maxFrames = ParsePositiveInt(ReadValue(args, ref i, "--max-frames"), "--max-frames");
                    break;
                case "--overwrite":
                    overwrite = true;
                    break;
                case "--verbose":
                    verbose = true;
                    break;
                case "--help":
                case "-h":
                    throw new InvalidOperationException(BuildUsage());
                default:
                    throw new InvalidOperationException($"Unknown argument '{arg}'.{Environment.NewLine}{BuildUsage()}");
            }
        }

        if (string.IsNullOrWhiteSpace(inputDir))
        {
            throw new InvalidOperationException("Missing required argument --input-dir.");
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new InvalidOperationException("Missing required argument --output.");
        }

        if (fps is null)
        {
            throw new InvalidOperationException("Missing required argument --fps.");
        }

        string fullInputDir = Path.GetFullPath(inputDir);
        if (!Directory.Exists(fullInputDir))
        {
            throw new DirectoryNotFoundException($"Input directory does not exist: {fullInputDir}");
        }

        string fullOutputPath = Path.GetFullPath(outputPath);
        if (File.Exists(fullOutputPath) && !overwrite)
        {
            throw new IOException($"Output file already exists: {fullOutputPath}. Pass --overwrite to replace it.");
        }

        return new EncoderOptions(
            fullInputDir,
            fullOutputPath,
            fps.Value,
            startIndex,
            maxFrames,
            overwrite,
            verbose);
    }

    private static string ReadValue(string[] args, ref int index, string option)
    {
        if (index + 1 >= args.Length)
        {
            throw new InvalidOperationException($"Missing value after {option}.");
        }

        index++;
        return args[index];
    }

    private static double ParsePositiveDouble(string text, string option)
    {
        double value = double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

        if (value <= 0 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new InvalidOperationException($"{option} must be a finite value greater than zero.");
        }

        return value;
    }

    private static int ParseNonNegativeInt(string text, string option)
    {
        int value = int.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);

        if (value < 0)
        {
            throw new InvalidOperationException($"{option} must be >= 0.");
        }

        return value;
    }

    private static int ParsePositiveInt(string text, string option)
    {
        int value = ParseNonNegativeInt(text, option);
        if (value == 0)
        {
            throw new InvalidOperationException($"{option} must be > 0.");
        }

        return value;
    }

    private static string BuildUsage()
    {
        return "Usage: dotnet run --project tools/Hnm0Encoder -- " +
               "--input-dir <folder> --output <file.hnm0> --fps <value> " +
               "[--start-index <n>] [--max-frames <n>] [--overwrite] [--verbose]";
    }
}
