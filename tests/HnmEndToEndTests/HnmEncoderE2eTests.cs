using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Drawing;
using System.Drawing.Imaging;
using logo;
using Xunit;

namespace HnmEndToEndTests;

[SupportedOSPlatform("windows")]
public sealed class HnmEncoderE2eTests
{
    [Fact]
    public async Task EncoderCliOutput_DecodesThroughPlayerParser_AndMatchesSourceFrames()
    {
        string repoRoot = FindRepoRoot();
        string workDir = Path.Combine(Path.GetTempPath(), "hnm0-e2e", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workDir);

        string framesDir = Path.Combine(workDir, "frames");
        Directory.CreateDirectory(framesDir);

        string outputPath = Path.Combine(workDir, "out.hnm");
        IReadOnlyList<string> sourceFrames = CreateSourceFrames(framesDir);

        string encoderProject = Path.Combine(repoRoot, "tools", "Hnm0Encoder", "Hnm0Encoder.csproj");
        RunEncoderCli(repoRoot, encoderProject, framesDir, outputPath);

        byte[] encodedBytes = await File.ReadAllBytesAsync(outputPath);
        List<HnmChunk> chunks = new();
        HnmFileParser.ParseChunks(encodedBytes, chunks);

        Assert.Equal(sourceFrames.Count + 1, chunks.Count);

        byte[] framebuffer = new byte[HnmPlaybackEngine.VgaSegmentByteCount];
        byte[] palette = new byte[256 * 3];
        byte[] compressedFrameBuffer = new byte[HnmPlaybackEngine.VgaSegmentByteCount];

        Assert.True(HnmPaletteParser.TryApplyPaletteRecords(chunks[0].GetPayloadSpan(), palette));

        for (int frameIndex = 0; frameIndex < sourceFrames.Count; frameIndex++)
        {
            Assert.True(HnmFrameParser.TryRenderFrameChunk(chunks[frameIndex + 1].GetPayloadSpan(), framebuffer, compressedFrameBuffer));
            AssertFrameMatchesPng(sourceFrames[frameIndex], framebuffer, palette);
        }
    }

    [Fact]
    public void HeadlessPlayerCli_WritesScreenshots()
    {
        string repoRoot = FindRepoRoot();
        string workDir = Path.Combine(Path.GetTempPath(), "hnm-player-headless-e2e", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workDir);

        string screenshotDir = Path.Combine(workDir, "screenshots");
        string inputPath = Path.Combine(repoRoot, "LOGO.HNM");
        string playerProject = Path.Combine(repoRoot, "HnmPlayer", "HnmPlayer.csproj");

        RunHeadlessPlayerCli(repoRoot, playerProject, inputPath, screenshotDir);

        string[] screenshots = Directory.GetFiles(screenshotDir, "*.png");
        Assert.NotEmpty(screenshots);

        using System.Drawing.Bitmap firstScreenshot = new(screenshots[0]);
        Assert.Equal(HnmPlaybackEngine.FrameWidth, firstScreenshot.Width);
        Assert.Equal(HnmPlaybackEngine.FrameHeight, firstScreenshot.Height);
    }

    private static string FindRepoRoot()
    {
        DirectoryInfo? current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "logo.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not locate the LOGO repository root from the test output path.");
    }

    private static IReadOnlyList<string> CreateSourceFrames(string framesDir)
    {
        Color colorA = Color.FromArgb(64, 128, 192);
        Color colorB = Color.FromArgb(192, 64, 128);

        var paths = new List<string>(2)
        {
            DrawCheckerboard(Path.Combine(framesDir, "frame0001.png"), colorA, colorB, swapCells: false),
            DrawCheckerboard(Path.Combine(framesDir, "frame0002.png"), colorA, colorB, swapCells: true)
        };

        return paths;
    }

    private static string DrawCheckerboard(string path, Color colorA, Color colorB, bool swapCells)
    {
        using var bitmap = new System.Drawing.Bitmap(HnmPlaybackEngine.FrameWidth, HnmPlaybackEngine.FrameHeight);
        using Graphics graphics = Graphics.FromImage(bitmap);

        for (int y = 0; y < HnmPlaybackEngine.FrameHeight; y++)
        {
            for (int x = 0; x < HnmPlaybackEngine.FrameWidth; x++)
            {
                bool useFirst = ((x / 16) + (y / 16)) % 2 == 0;
                if (swapCells)
                {
                    useFirst = !useFirst;
                }

                bitmap.SetPixel(x, y, useFirst ? colorA : colorB);
            }
        }

        bitmap.Save(path, ImageFormat.Png);
        return path;
    }

    private static void RunEncoderCli(string repoRoot, string encoderProject, string inputDir, string outputPath)
    {
        string arguments = string.Join(" ", new[]
        {
            "run",
            "--project",
            Quote(encoderProject),
            "--",
            "--input-dir",
            Quote(inputDir),
            "--output",
            Quote(outputPath),
            "--fps",
            "1",
            "--overwrite"
        });

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start dotnet run for the encoder CLI.");
        string stdout = process.StandardOutput.ReadToEnd();
        string stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Encoder CLI failed with exit code {process.ExitCode}.\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
        }

        Assert.True(File.Exists(outputPath), "Encoder CLI did not create the expected HNM output file.");
    }

    private static void RunHeadlessPlayerCli(string repoRoot, string playerProject, string inputPath, string screenshotDir)
    {
        string arguments = string.Join(" ", new[]
        {
            "run",
            "--project",
            Quote(playerProject),
            "--",
            "--headless",
            "--input",
            Quote(inputPath),
            "--screenshot-dir",
            Quote(screenshotDir)
        });

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start dotnet run for the headless player CLI.");
        string stdout = process.StandardOutput.ReadToEnd();
        string stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Headless player CLI failed with exit code {process.ExitCode}.\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
        }

        Assert.True(Directory.Exists(screenshotDir), "Headless player CLI did not create the screenshot directory.");
    }

    private static string Quote(string value)
    {
        return $"\"{value}\"";
    }

    private static void AssertFrameMatchesPng(string pngPath, byte[] framebuffer, byte[] palette)
    {
        using System.Drawing.Bitmap png = new(pngPath);
        Assert.Equal(HnmPlaybackEngine.FrameWidth, png.Width);
        Assert.Equal(HnmPlaybackEngine.FrameHeight, png.Height);

        for (int y = 0; y < png.Height; y++)
        {
            for (int x = 0; x < png.Width; x++)
            {
                Color expected = png.GetPixel(x, y);
                byte index = framebuffer[(y * HnmPlaybackEngine.FrameWidth) + x];
                int paletteOffset = index * 3;
                Color actual = Color.FromArgb(palette[paletteOffset + 0] << 2, palette[paletteOffset + 1] << 2, palette[paletteOffset + 2] << 2);

                Assert.Equal(expected.R & 0xFC, actual.R);
                Assert.Equal(expected.G & 0xFC, actual.G);
                Assert.Equal(expected.B & 0xFC, actual.B);
            }
        }
    }
}