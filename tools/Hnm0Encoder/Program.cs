namespace Hnm0Encoder;

internal static class Program
{
    public static int Main(string[] args)
    {
        EncoderOptions options = CommandLineParser.Parse(args);
        IReadOnlyList<string> orderedFrames = FrameDiscovery.DiscoverPngFrames(options.InputDirectory);
        IReadOnlyList<string> selectedFrames = FrameDiscovery.SelectFrameWindow(
            orderedFrames,
            options.StartIndex,
            options.MaxFrames);

        if (selectedFrames.Count == 0)
        {
            throw new InvalidDataException("No PNG frames were selected after applying --start-index/--max-frames.");
        }

        FramePreflightReport report = FramePreflight.Analyze(selectedFrames, options.Verbose);
        PrintSummary(options, report);

        Hnm0Writer.Write(options.OutputPath, selectedFrames);
        Console.WriteLine($"Wrote {selectedFrames.Count} frame(s) to {options.OutputPath}");
        return 0;
    }

    private static void PrintSummary(EncoderOptions options, FramePreflightReport report)
    {
        Console.WriteLine("HNM0 encoder preflight");
        Console.WriteLine($"Input directory: {options.InputDirectory}");
        Console.WriteLine($"Output path: {options.OutputPath}");
        Console.WriteLine($"Frames selected: {report.FrameCount}");
        Console.WriteLine($"Width x Height: {FramePreflight.ExpectedWidth}x{FramePreflight.ExpectedHeight}");
        Console.WriteLine($"Max palette colors in a frame: {report.MaxUniqueColorsInFrame}");
        Console.WriteLine($"Min palette colors in a frame: {report.MinUniqueColorsInFrame}");
        Console.WriteLine($"FPS: {options.FramesPerSecond:0.###}");
    }
}
