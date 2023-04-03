using Spice86.Shared.Interfaces;

namespace logo;

/// <summary>
/// Provides functions overrides for the DOS program.
/// </summary>
public class MyOverrideSupplier : IOverrideSupplier
{
    public Dictionary<SegmentedAddress, FunctionInformation> GenerateFunctionInformations(
        int programStartAddress,
        Machine machine)
    {
        var generatedCode = new GeneratedOverrides(new(), machine, (ILoggerService)Program.ServiceProvider!.GetService(typeof(ILoggerService)));
        return generatedCode.FunctionInformations;
    }
}