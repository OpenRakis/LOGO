// Debug your project with "-e path/to/exe" in your launch settings.
// Use "-a "some string"" to pass arguments to the emulated program.
// See https://github.com/OpenRakis/Spice86 for more information

// Must match the documented LOGO.EXE checksum in README.md.
const string ExpectedChecksum = "896a55f02555f708b57c6fd7576c8404aa479c1ec6e90fbbb230130bc7a31921";

List<string> spice86Args = [..args];

if (!spice86Args.Contains("-x") && !spice86Args.Contains("--ExpectedChecksum")) {
    spice86Args.Add("-x");
    spice86Args.Add(ExpectedChecksum);
}
if (!spice86Args.Contains("-h") && !spice86Args.Contains("--HeadlessMode")) {
    spice86Args.Add("-h");
    spice86Args.Add("Minimal");
}
if (!spice86Args.Contains("--mcp-http-port")) {
    spice86Args.Add("--mcp-http-port");
    spice86Args.Add("8081");
}
if (!spice86Args.Contains("-u") && !spice86Args.Contains("--UseCodeOverride")) {
    spice86Args.Add("-u");
    spice86Args.Add("false");
}

Spice86.Program.Main([..spice86Args]);

public partial class Program {
}