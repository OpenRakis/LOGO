namespace Hnm0Encoder;

internal sealed record EncoderOptions(
    string InputDirectory,
    string OutputPath,
    double FramesPerSecond,
    int StartIndex,
    int? MaxFrames,
    bool Overwrite,
    bool Verbose);
