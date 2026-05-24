# LOGO

Reverse-engineering and preservation project for the Dune PC floppy logo animation player.

This repository documents and reimplements the original DOS behavior of `LOGO.EXE` and `LOGO.HNM`, with a strong focus on execution fidelity rather than approximation.

<https://github.com/user-attachments/assets/9d0987ae-4c27-4c83-b6d7-79880e20c76a>

## What This Repo Contains

The repo currently serves two complementary goals.

- Emulator-accurate reverse engineering of the original DOS binary (`LOGO.EXE`) through Spice86.
- Standalone playback implementation (`HnmPlayer`) that reproduces the same animation contract in managed desktop code.

### Project layout

- Root project: Spice86-based execution environment for the original executable and override development.
- `HnmPlayer`: independent desktop HNM player focused on deterministic playback of the Dune floppy stream.

## Data Provenance

`LOGO.EXE` and `LOGO.HNM` come from the Dune PC floppy release.

This repository does **not** ship `LOGO.EXE`. If you run the root emulator project against the original binary, provide your own copy from Dune PC floppy 2.1.

Reference checksum for `LOGO.EXE`:

    896a55f02555f708b57c6fd7576c8404aa479c1ec6e90fbbb230130bc7a31921

## Execution Model (Original DOS)

At a high level, the original animation path performs the following sequence:

- Open `LOGO.HNM` via DOS interrupt services.
- Parse and apply palette records.
- Read frame records and decode compressed payloads.
- Render into VGA memory and update DAC entries.
- Wait on video timing and poll keyboard to allow early exit.
- Close file and terminate through DOS interrupt paths.

For deep format internals, see [LOGO.MD](LOGO.MD).

## Quick Start

### Root emulator project (original `LOGO.EXE` path)

    dotnet run -- -e /path/to/LOGO.EXE -d false

### Standalone player (`HnmPlayer`)

`HnmPlayer` is now independent from runtime `LOGO.EXE` loading. It targets the Dune floppy HNM playback contract directly.

Build:

    dotnet build HnmPlayer/HnmPlayer.csproj --configuration Debug

## MCP Default Profile

By default, the root project starts Spice86 with:

- MCP HTTP server on port `8081` (`--mcp-http-port 8081`)
- C# overrides disabled (`--UseCodeOverride false`)

To re-enable reverse-engineered overrides, pass both flags together:

    --OverrideSupplierClassName logo.MyOverrideSupplier --UseCodeOverride true

## Core Reverse-Engineering Files

- `Program.cs`: root entry point and runtime wiring.
- `CodeGeneratorConfig.json`: Ghidra code-generation configuration (`Spice86CodeGenerator.java`).
- `GeneratedCode_OriginalAsm.cs`: direct generated assembly-to-C# form.
- `GeneratedCode_DecompiledAsm.cs`: optional decompiled cleanup stage.
- `GeneratedCode.cs`: hand-maintained high-level translation.
- `HnmPlayer/*`: independent playback implementation.

### CheckExternalEvents guidance

Interrupt handling requires frequent `CheckExternalEvents` calls. You can inject broadly or at selected hot points.

Example targeted injection:

    "CodeToInject": {
      "CheckExternalEvents({nextSegment}, {nextOffset});": ["CS1:100B", "CS1:09C1", "CS1:09EC", "CS1:098C", "CS1:09D9"]
    }

## Documentation Index

- [LOGO.MD](LOGO.MD): deep technical specification and evidence-backed reconstruction of `LOGO.HNM` and the `LOGO.EXE` playback contract.
- [AGENTS.md](AGENTS.md): repository workflow guidance and reverse-engineering constraints.
