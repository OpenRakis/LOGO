// Debug your project with "-e path/to/exe" in your launch settings.
// Use "-a "some string"" to pass arguments to the emulated program.
// See https://github.com/OpenRakis/Spice86 for more information

using logo;

using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();
services.AddLogging();
ServiceProvider serviceProvider = services.BuildServiceProvider();
Program.ServiceProvider = serviceProvider;

// Put the SHA256 checksum of your target DOS program here.
Spice86.Program.RunWithOverrides<MyOverrideSupplier>(args, "896a55f02555f708b57c6fd7576c8404aa479c1ec6e90fbbb230130bc7a31921");

public partial class Program {
    public static ServiceProvider? ServiceProvider { get; set; }
}