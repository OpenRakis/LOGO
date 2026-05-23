## Plan: Avalonia High-Fidelity LOGO Player

Build a pure C# Avalonia desktop app that reproduces LOGO.EXE playback without x86 emulation. The standalone player now lives in [HnmPlayer/HnmPlayer.csproj](c:/Users/noalm/source/repos/LOGO/HnmPlayer/HnmPlayer.csproj) so it can evolve independently from the root emulator project. I’m treating this as a reverse-engineering fidelity project, not a generic media app: the target is the exact playback contract in [GeneratedCode.cs](c:/Users/noalm/source/repos/LOGO/GeneratedCode.cs#L37), the format notes in [LOGO.MD](c:/Users/noalm/source/repos/LOGO/LOGO.MD), and the assembly-aligned reference in [GeneratedCode_OriginalAsm.cs](c:/Users/noalm/source/repos/LOGO/GeneratedCode_OriginalAsm.cs#L1807). Prioritize absolute correctness over convenience, surface area, or UI polish.

**Current State**
- The root LOGO emulator project still builds unchanged.
- The standalone Avalonia player project under `HnmPlayer/` builds successfully on its own.
- The player is forward-only for now, with no seek UI and no debug overlay.
- The playback core now classifies chunks up front as palette, frame, or unknown before stepping through them.
- The frame path now uses a direct header-driven blit mode in the standalone engine instead of routing every frame through a provisional speculative decode step.
- The frame parser now restores the original gated decode path: when frame header bit `0x0200` is set, payload is first decompressed and `DX/BX` are read from decoded output before blitting.
- The blitter now uses assembly-aligned control semantics: `CH` selects mode (`0xFE`/`0xFF`), `CL` sets row count, low 9 bits of flags set row width, and sign/`0x2000`/`0x4000` bits drive forward vs reverse branch families.
- Chunk classification now rejects more false positives by validating palette termination/ranges and frame mode/rows/width invariants before dispatch.
- Playback cadence now uses chunk-derived BIOS-tick pacing (matching the integer timing math in `HNMUnknown_1000_0FEA_10FEA`) and playback terminates strictly on stream completion rather than "no visual change".
- Remaining fidelity risk is now concentrated in timing/termination parity and in strict branch-level verification against the original runtime trace.
- Remaining fidelity risk is now concentrated in strict branch-level verification against original runtime traces and input/exit parity edge cases.

**Steps**
1. Freeze the observable behavior first: startup loads LOGO.HNM, palette records are consumed before the frame loop, frame/control records follow, the decoder skips a 6-byte preamble, and keyboard/timing behavior must match the original flow.
2. Replace segmented DOS state with managed runtime state: a sequential HNM reader, palette state, 320x200 indexed framebuffer, timing controller, and keyboard input poller.
3. Implement the HNM parser and decoder as plain C# services. The outer stream should be parsed as a self-sized chunk chain, and the inner codec should reproduce the literal, short back-reference, long back-reference, and end-of-stream behavior already documented in [LOGO.MD](c:/Users/noalm/source/repos/LOGO/LOGO.MD#L1).
4. Build the rendering pipeline around indexed output. Decode into a managed byte buffer, then map palette + framebuffer data to RGBA for display in Avalonia.
5. Add a thin Avalonia host without MVVM unless it proves useful. Use a single window, a drawing surface, and a small playback controller class rather than a view-model layer.
6. Reproduce timing and input fidelity. Keep the roughly 60 FPS cadence, the per-frame wait behavior, and the early-exit key polling semantics.
7. Keep playback strictly forward-only for the first implementation. Do not add seeking yet, but structure the decoder and player state so a seek index can be added later without redesigning the core.
8. Do not add debugging UI in the first pass, but keep the player state and frame pipeline observable enough that a debug overlay or trace mode can be added later without changing the core decoder.
9. Validate against the original behavior and the repo reference GIF. Compare chunk boundaries, decoded frames, palette progression, and end-of-stream behavior against the reverse-engineered trace.
10. Document the contract and any remaining unknowns once the implementation is stable.

**Relevant files**
- [HnmPlayer/HnmPlayer.csproj](c:/Users/noalm/source/repos/LOGO/HnmPlayer/HnmPlayer.csproj) — standalone Avalonia player project.
- [HnmPlayer/Program.cs](c:/Users/noalm/source/repos/LOGO/HnmPlayer/Program.cs) — standalone app entry point.
- [HnmPlayer/App.cs](c:/Users/noalm/source/repos/LOGO/HnmPlayer/App.cs) — Avalonia application bootstrap.
- [HnmPlayer/MainWindow.cs](c:/Users/noalm/source/repos/LOGO/HnmPlayer/MainWindow.cs) — player window and surface wiring.
- [HnmPlayer/HnmPlaybackEngine.cs](c:/Users/noalm/source/repos/LOGO/HnmPlayer/HnmPlaybackEngine.cs) — managed HNM parsing, decoding, and rendering core.
- [GeneratedCode.cs](c:/Users/noalm/source/repos/LOGO/GeneratedCode.cs) — control flow, palette handling, decoder entry points, timing loop.
- [GeneratedCode_OriginalAsm.cs](c:/Users/noalm/source/repos/LOGO/GeneratedCode_OriginalAsm.cs) — assembly-aligned behavior reference.
- [LOGO.MD](c:/Users/noalm/source/repos/LOGO/LOGO.MD) — current HNM format notes and reconstructed file layout.
- [PaletteData.cs](c:/Users/noalm/source/repos/LOGO/PaletteData.cs) — palette record traversal model.
- [Program.cs](c:/Users/noalm/source/repos/LOGO/Program.cs) — app host entry point.
- [logo.csproj](c:/Users/noalm/source/repos/LOGO/logo.csproj) — target framework and project wiring.
- [README.md](c:/Users/noalm/source/repos/LOGO/README.md) — existing behavior summary and visual reference.

**Verification**
1. Parse LOGO.HNM end-to-end and confirm the chunk walk matches the observed offsets and sizes in [LOGO.MD](c:/Users/noalm/source/repos/LOGO/LOGO.MD#L1).
2. Decode at least one palette sequence and one frame sequence and compare the output to the original playback path.
3. Run the app against the real LOGO.HNM file and verify it exits on the same conditions as the original.
4. Compare rendered output against the reference behavior for timing, palette sequencing, and frame appearance.
5. If something diverges, isolate whether the issue is parser, codec, palette mapping, or timing, then fix that slice only.
6. Leave clear extension points for a future seek index and optional debug overlay, but do not enable either in the first release.

**Next Work Slice**
1. Add a reproducible frame-flow comparison harness against [GeneratedCode_OriginalAsm.cs](c:/Users/noalm/source/repos/LOGO/GeneratedCode_OriginalAsm.cs#L565) to confirm branch parity on real data.
2. Fix any divergences found by that comparison before adding seek or diagnostic features.
3. Tighten input/exit parity edge cases to match the original "any key" behavior without breaking explicit standalone controls.

**Decisions**
- No MVVM by default. The app is a linear playback engine, so a direct host plus a small controller is the simpler and more faithful architecture.
- No emulator dependency. DOS interrupts and VGA access are replaced with managed equivalents that preserve observable behavior.
- Scope is limited to LOGO.EXE and LOGO.HNM playback. Broader Dune engine work is out of scope.
- High fidelity means exact sequencing and behavior first; UI polish is secondary.
- Seek and debugging support are intentionally deferred, but the internal architecture should allow both to be added later without rewriting the decoder or renderer.

**Further Considerations**
1. Confirm whether you want the first implementation to prioritize exact decoder correctness or the full Avalonia shell first. Recommendation: decoder and parser first.
2. Keep the first release forward-only. If seeking becomes necessary later, add it as an overlay on top of a stable frame index rather than baking it into the initial design.
3. Keep debugging out of the initial UI. If validation tooling is needed later, add it as an optional trace mode or diagnostic overlay that does not alter playback behavior.
