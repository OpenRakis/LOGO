# AGENTS

## Purpose

This repository is a reverse-engineered DOS animation player for Dune, implemented with Spice86 C# overrides.

Use this file as the entry point for agent behavior. Prefer linking to existing docs instead of duplicating their content.

## Start Here

Read these first, in order:

1. [README.md](README.md)
2. [Program.cs](Program.cs)
3. [GeneratedCode.cs](GeneratedCode.cs)
4. [CodeGeneratorConfig.json](CodeGeneratorConfig.json)
5. [.github/instructions/reverse-engineering.instructions.md](.github/instructions/reverse-engineering.instructions.md)

## Build And Run

- Build: `dotnet build --configuration Release`
- Run: `dotnet run -- -e /path/to/LOGO.EXE -d false`
- Required DOS binary checksum is enforced in [Program.cs](Program.cs).
- Default run profile is cloud-friendly: headless minimal UI, MCP on `8081`, and `UseCodeOverride=false` unless explicitly overridden.
- To run with repository C# overrides, pass `--OverrideSupplierClassName logo.MyOverrideSupplier --UseCodeOverride true`.
- Main project targets `net10.0` in [logo.csproj](logo.csproj).

## File Ownership And Edit Rules

- Edit [GeneratedCode.cs](GeneratedCode.cs) for high-level/manual override behavior.
- Treat [GeneratedCode_OriginalAsm.cs](GeneratedCode_OriginalAsm.cs) as generated/reference-oriented code.
- Treat [GeneratedCode_DecompiledAsm.cs](GeneratedCode_DecompiledAsm.cs) as optional intermediate output.
- If changing [GeneratedCode.cs](GeneratedCode.cs), [GeneratedCode_OriginalAsm.cs](GeneratedCode_OriginalAsm.cs), [GeneratedCode_DecompiledAsm.cs](GeneratedCode_DecompiledAsm.cs), or [MyOverrideSupplier.cs](MyOverrideSupplier.cs), follow [.github/instructions/reverse-engineering.instructions.md](.github/instructions/reverse-engineering.instructions.md).

## Reverse-Engineering Workflow (Mandatory For Override Changes)

- Gather evidence first (live Spice86/GDB or existing dump files) before changing behavior.
- Preserve calling contract exactly: return type (`NearRet()`/`FarRet()`), register/FLAGS side effects, and stack behavior.
- Keep function naming traceable to segment/offset/linear address.
- For unobserved code paths, fail explicitly (`FailAsUntested`) rather than guessing behavior.

See full rules: [.github/instructions/reverse-engineering.instructions.md](.github/instructions/reverse-engineering.instructions.md).

## Useful Repo Resources

- Live inspection prompt: [.github/prompts/inspect-live-spice86.prompt.md](.github/prompts/inspect-live-spice86.prompt.md)
- Spice86 MCP skill: [.github/skills/spice86-mcp/SKILL.md](.github/skills/spice86-mcp/SKILL.md)
- HSQ decompression skill: [.github/skills/dune-dat-and-hsq-decompression/SKILL.md](.github/skills/dune-dat-and-hsq-decompression/SKILL.md)
- Execution flow dump: [spice86dumpExecutionFlow.json](spice86dumpExecutionFlow.json)
- Ghidra symbol dump: [spice86dumpGhidraSymbols.txt](spice86dumpGhidraSymbols.txt)

## Validation Expectations

There is no dedicated automated test suite in this repo.
When behavior changes are made, validate by running the emulated program and checking in-game behavior against expected/original flow.
