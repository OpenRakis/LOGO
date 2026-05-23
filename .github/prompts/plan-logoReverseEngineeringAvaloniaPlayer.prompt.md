## Plan: Full LOGO Reverse Engineering And Independent Avalonia Player

Deliver a complete understanding of LOGO.EXE and LOGO.HNM by combining static analysis, runtime tracing with Spice86 MCP using UseCodeOverride false, and deterministic data extraction using the UnHsq workflow, then build a standalone Avalonia 11.2.x MVVM player that reproduces playback behavior without emulator dependencies. For MCP, rely primarily on CFG CPU graph analysis (`read_cfg_cpu_graph`) and use other MCP tools as supporting evidence.

**Steps**
1. Phase 1 - Ground truth setup and reproducibility
1.0 Execution precedence for this plan: complete Phase 1 and steps 2.1-2.3 before executing step 3.3 and later dynamic trace steps.
1.1 Verify exact inputs and hashes for LOGO.EXE and LOGO.HNM, then lock a reproducible baseline run profile with emulator overrides disabled (UseCodeOverride false).
1.2 Produce a run manifest that records CLI options, Spice86 version, MCP endpoint and ports, and capture locations for traces and screenshots.
1.3 Define canonical playback checkpoints (for example first frame, palette transition points, end frame, early keypress exit path) to compare all future outputs against.

2. Phase 2 - Artifact preparation and static inventory (2.1-2.3 precede 3.3+, and 2.4 overlaps with Phase 3)
2.1 Decompress LOGO.HNM with the existing UnHsq tool workflow and generate LOGO.UNHSQ as the primary offline artifact.
2.2 Build binary maps for both LOGO.HNM and LOGO.UNHSQ: offsets, repeating patterns, candidate headers, candidate frame boundaries, and entropy regions.
2.3 Create a function inventory from existing dump files and generated code references: startup path, file I/O handlers, decompression path, VGA palette writes, frame blit path, and keyboard polling path.
2.4 Tag unknown functions by role hypotheses and confidence levels so dynamic tracing can explicitly confirm or reject each hypothesis.

3. Phase 3 - Dynamic behavior tracing via Spice86 MCP with no override code (starts after 2.1-2.3; overlaps only with 2.4)
3.0 MCP priority: use CFG CPU graph (`read_cfg_cpu_graph`) as the primary discovery and control-flow truth source; treat disassembly, memory reads, and screenshots as secondary validation signals.
3.1 Start Spice86 MCP and run LOGO.EXE with UseCodeOverride false to observe original behavior only.
3.1.1 Capture CFG snapshots over time (startup, first decode loop, mid-playback, termination) and diff them to identify newly activated nodes/edges.
3.1.2 Cluster CFG nodes into behavior regions (file I/O, decode, palette, blit, input polling) before deep-diving with any other MCP tool.
3.2 Capture baseline CPU and DOS state at entry, before file open, during each file read, during decode, and during render loops.
3.3 Add execution breakpoints around known hot addresses from dump files and around interrupt handlers (INT 21h and INT 10h paths) to capture call chronology.
3.4 At each read/decode/render step, use CFG-selected hotspots first, then snapshot memory ranges used as source and destination buffers; correlate with file offsets to reconstruct data flow.
3.5 Capture VGA DAC palette state and periodic screenshots across timeline checkpoints; store paired evidence with cycle counts.
3.6 Build a chronological event ledger: read chunk, decode call, palette load, frame presentation, keyboard check, loop control decision.

4. Phase 4 - Reconstruction and complete specification (depends on 2 and 3)
4.1 Produce an executable behavior spec for LOGO.EXE: startup, resource open, decode loop, render loop, input handling, and termination state machine.
4.2 Produce a binary format spec for LOGO.HNM and LOGO.UNHSQ: header schema, chunk types, frame payload encoding, palette encoding, timing semantics, and termination markers.
4.3 Resolve all previously unknown high-impact functions by mapping each to a named responsibility and documented side effects.
4.4 Validate spec completeness by replaying full animation from spec-derived parser outputs and confirming all playback-critical bytes are explained; any non-critical unknown regions are explicitly documented.

5. Phase 5 - Standalone player architecture and implementation plan (depends on 4)
5.1 Create a separate desktop app architecture with clear layers: format parser, decode pipeline, playback engine, and Avalonia MVVM presentation.
5.2 Define domain models: container metadata, frame descriptor, palette command stream, decoded frame surface, and playback cursor/timing model.
5.3 Implement parser and decoder as pure library components with deterministic outputs and no UI dependencies.
5.4 Implement playback engine that reproduces original sequencing rules: frame cadence, palette transition timing, key interrupt semantics, and end-of-stream behavior.
5.5 Build Avalonia 11.2.x MVVM app shell with controls for open/play/pause/stop/seek/frame-step, timeline state, and diagnostics panel for frame and palette indices.
5.6 Integrate pixel pipeline for indexed-color rendering and palette application to display frame-accurate output.

6. Phase 6 - Parity validation and hardening (depends on 5)
6.1 Create golden baseline set from Spice86 run: selected screenshots, palette snapshots, and event timestamps.
6.2 Compare standalone output against baseline at checkpoints: frame content parity (pixel diff = 0 at checkpoint frames), palette parity (byte-exact DAC values), ordering parity (identical checkpoint sequence), and total playback duration tolerance (within +/-50 ms).
6.3 Test edge paths: malformed inputs, truncated file, unexpected markers, and user interruption behavior.
6.4 Document residual differences if any and classify as intentional, acceptable variance, or bug.

7. Phase 7 - Documentation and handoff (depends on 6)
7.1 Write reverse engineering dossier with evidence links from each behavioral claim to static and dynamic proof.
7.2 Write format specification document for LOGO.HNM and LOGO.UNHSQ suitable for independent implementation.
7.3 Write developer guide for the standalone player architecture, extension points, and verification workflow.
7.4 Publish a repeatable validation checklist so future refactors preserve parity with original LOGO.EXE behavior.

**Relevant files**
- c:/Users/noalm/source/repos/LOGO/README.md — baseline behavior narrative and run entry guidance.
- c:/Users/noalm/source/repos/LOGO/Program.cs — current Spice86 launch entry point and checksum guard.
- c:/Users/noalm/source/repos/LOGO/CodeGeneratorConfig.json — known injection and event-check anchor addresses useful for trace waypoints.
- c:/Users/noalm/source/repos/LOGO/GeneratedCode.cs — current manually lifted behavior reference for naming and semantic hints.
- c:/Users/noalm/source/repos/LOGO/GeneratedCode_OriginalAsm.cs — closest generated representation of original executable control flow.
- c:/Users/noalm/source/repos/LOGO/spice86dumpExecutionFlow.json — observed execution graph and path coverage hints.
- c:/Users/noalm/source/repos/LOGO/spice86dumpFunctionsDetails.txt — callers/callees/return contract map for prioritizing investigation.
- c:/Users/noalm/source/repos/LOGO/spice86dumpGhidraSymbols.txt — symbol naming anchors for mapping unknown functions.
- c:/Users/noalm/source/repos/LOGO/tools/UnHsq/Program.cs — authoritative HSQ decompression implementation already used in this repo.
- c:/Users/noalm/source/repos/LOGO/.github/skills/dune-dat-and-hsq-decompression/SKILL.md — required decompression workflow and reporting discipline.
- c:/Users/noalm/source/repos/LOGO/.github/skills/spice86-mcp/SKILL.md — MCP handshake and tool usage requirements.
- c:/Users/noalm/source/repos/LOGO/.github/instructions/reverse-engineering.instructions.md — evidence-first and contract-preservation rules.
- Primary MCP tool emphasis: `read_cfg_cpu_graph` for coverage-first analysis, then `read_disassembly` / `read_memory` for node-level validation.

**Verification**
1. Reproducibility check: two independent runs with UseCodeOverride false produce the same event ledger structure and equivalent frame checkpoint outputs.
2. Format coverage check: parser can account for all bytes in LOGO.HNM and LOGO.UNHSQ relevant to playback, with unknown regions explicitly documented.
3. Behavioral parity check: standalone player matches checkpoint screenshots and palette states from Spice86 with checkpoint pixel diff = 0, byte-exact palette values, and total playback duration within +/-50 ms.
4. Control-flow completeness check: all high-frequency runtime functions in dump data have assigned semantic roles and evidence citations.
5. Robustness check: corrupted and truncated input cases fail gracefully with deterministic errors.

**Decisions**
- Included scope: full reverse-engineering of playback path and complete standalone playback implementation for logo.hnm.
- Included scope: no dependency on Spice86 or override code at runtime for the final player.
- Excluded scope: adding new emulator features inside Spice86 itself.
- Excluded scope: broad Dune engine reverse engineering outside LOGO.EXE and logo.hnm playback concerns.

**Further Considerations**
1. Timing fidelity default: dual mode with switchable timing profile; default runtime mode is strict original cadence emulation.
2. Packaging default: produce both self-contained and framework-dependent builds using a CI matrix.
3. Decoder strategy default: strict by default with an explicit optional recovery mode for damaged files.
