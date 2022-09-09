// Debug your project with "-e path/to/exe" in your launch settings.
// Use "-a "some string"" to pass arguments to the emulated program.
// See https://github.com/OpenRakis/Spice86 for more information

using logo;

// Put the SHA256 checksum of your target DOS program here.
Spice86.Program.RunWithOverrides<MyOverrideSupplier>(args, "f83e879e80af7db21903e664b8bdc7645d521856994fa2dc321bad0051b96aa8");