# LOGO.HNM and LOGO.EXE: Deep Technical Notes

This document is a working technical specification for the Dune floppy logo animation path.

It focuses on two artifacts:

- `LOGO.EXE`: the original DOS program that controls playback.
- `LOGO.HNM`: the animation data stream consumed by that program.

The intent is evidence-first documentation. When behavior is unknown, this file marks it explicitly instead of guessing.

## Executive Summary

- `LOGO.HNM` is **not** a plain HSQ file.
- The file is consumed as a chain of self-sized outer chunks.
- Those chunks drive two major pipelines: palette updates and frame decoding.
- Frame payloads are decoded through a bit-level LZ-style codec.
- `LOGO.EXE` owns orchestration: I/O, timing, palette evolution, frame dispatch, input polling, and termination.

## Scope and Evidence Rules

Statements below are based on one or more of:

- direct execution behavior in this repository
- generated translation in `GeneratedCode.cs`
- assembly-aligned output in `GeneratedCode_OriginalAsm.cs`
- raw byte inspection of `LOGO.HNM`

If a field name or semantic role is uncertain, it is labeled as provisional.

## What LOGO.EXE Actually Does

`LOGO.EXE` is not just a decoder. It is a playback state machine with hard timing and device-facing behavior.

### High-level responsibilities

- Open `LOGO.HNM` through DOS file services.
- Maintain current stream pointer and chunk stepping state.
- Decode and apply palette records to VGA DAC.
- Decode frame payloads, optionally through an auxiliary segment.
- Blit decoded bytes into VGA memory (`A000:` space).
- Wait and sync against video cadence.
- Poll keyboard for early exit.
- Close file and terminate through DOS interrupt paths.

### Why this matters

If you only copy the compression algorithm but ignore orchestration (palette loop ordering, chunk stepping, timing gates, retrace-aligned behavior), playback diverges even when decoded bytes look plausible.

## Verified Control-Flow Anchors

The following control points are directly visible in code and assembly mappings.

- Startup opens `LOGO.HNM` and enters the playback path.
- Stream advancement is pointer-based (`DS:[0x56]` / `DS:[0x58]`) and chunk-size driven.
- Palette records are consumed in a dedicated loop until `0xFF` terminator.
- Frame handler reads a compact header, branches on flags, then decodes and blits.
- Inner decode routine reports produced byte count via `CX`.

Representative references:

- `GeneratedCode.cs` entry and file open path
- `UpdatePaletteDataAddress_1000_0E86_10E86`
- `CommonUnknown_display_1000_0E59_10E59`
- `CommonUnknown_1000_0EBD_10EBD`
- `CommonUnknown_1000_0EFE_10EFE`
- `CommonUnknownSplit_1000_0F30_10F30`

## LOGO.HNM Outer Container

`LOGO.HNM` length in this repository: `18417` bytes.

The strongest supported outer structure is:

```text
while not EOF:
  u16 chunk_size_le   ; includes this 2-byte field
  u8  payload[chunk_size_le - 2]
```

Chunk stepping mirrors the program logic:

```text
current = [segment:offset]
size    = word(current)
current = current + size
normalize segmented pointer
```

`CommonUnknown_1000_0EB2_10EB2` leaving `CX = size - 2` is key evidence that the leading word is a total record size, not a payload-only length.

### Observed chunk chain

The offsets below were produced by walking the file with that exact arithmetic:

| Index | Offset | Size | Leading words |
|---:|---:|---:|---|
| 0 | 0x0000 | 0x01B4 | `0x5655`, `0x0000` |
| 1 | 0x01B4 | 0x09D4 | `0x02A0`, `0xFE64` |
| 2 | 0x0B88 | 0x0616 | `0x025C`, `0xFE60` |
| 3 | 0x119E | 0x0054 | `0x820F`, `0xFF0F` |
| 4 | 0x11F2 | 0x04AB | `0x0253`, `0xFF49` |
| 5 | 0x169D | 0x04E7 | `0x025E`, `0xFF60` |
| 6 | 0x1B84 | 0x0613 | `0x025E`, `0xFF60` |
| 7 | 0x2197 | 0x06D5 | `0x025E`, `0xFF5F` |
| 8 | 0x286C | 0x07A0 | `0x025E`, `0xFF60` |
| 9 | 0x300C | 0x081D | `0x025D`, `0xFF60` |
| 10 | 0x3829 | 0x0394 | `0x025E`, `0xFF5E` |
| 11 | 0x3BBD | 0x0410 | `0x025C`, `0xFF60` |
| 12 | 0x3FCD | 0x0006 | `0x8200`, `0xFF00` |
| 13 | 0x3FD3 | 0x0006 | `0x8200`, `0xFF00` |
| 14 | 0x3FD9 | 0x0006 | `0x8200`, `0xFF00` |
| 15 | 0x3FDF | 0x0006 | `0x8200`, `0xFF00` |
| 16 | 0x3FE5 | 0x0006 | `0x8200`, `0xFF00` |
| 17 | 0x3FEB | 0x0006 | `0x8200`, `0xFF00` |
| 18 | 0x3FF1 | 0x0006 | `0x8200`, `0xFF00` |
| 19 | 0x3FF7 | 0x076D | `0x0263`, `0xFF82` |
| 20 | 0x4764 | 0x0006 | `0x8200`, `0xFF00` |
| 21 | 0x476A | 0x0006 | `0x8200`, `0xFF00` |
| 22 | 0x4770 | 0x0006 | `0x8200`, `0xFF00` |
| 23 | 0x4776 | 0x0006 | `0x8200`, `0xFF00` |
| 24 | 0x477C | 0x0021 | `0x0207`, `0xFF04` |
| 25 | 0x479D | 0x0006 | `0x8200`, `0xFF00` |
| 26 | 0x47A3 | 0x0006 | `0x8200`, `0xFF00` |
| 27 | 0x47A9 | 0x0006 | `0x8200`, `0xFF00` |
| 28 | 0x47AF | 0x0006 | `0x8200`, `0xFF00` |
| 29 | 0x47B5 | 0x0006 | `0x8200`, `0xFF00` |
| 30 | 0x47BB | 0x0006 | `0x8200`, `0xFF00` |
| 31 | 0x47C1 | 0x0006 | `0x8200`, `0xFF00` |
| 32 | 0x47C7 | 0x0006 | `0x8200`, `0xFF00` |
| 33 | 0x47CD | 0x0006 | `0x8200`, `0xFF00` |
| 34 | 0x47D3 | 0x0006 | `0x8200`, `0xFF00` |
| 35 | 0x47D9 | 0x0006 | `0x8200`, `0xFF00` |
| 36 | 0x47DF | 0x0006 | `0x8200`, `0xFF00` |
| 37 | 0x47E5 | 0x0006 | `0x8200`, `0xFF00` |
| 38 | 0x47EB | 0x0006 | `0x8200`, `0xFF00` |

The repeated `0x0006` tail chunks are real data, not parser artifacts.

## Inner Record Families

The outer container holds different payload families.

### 1) Palette record stream

Observed shape:

```text
u8 start_index
u8 color_count
u8 rgb[color_count * 3]
```

Termination rule: `start_index == 0xFF` ends the palette record list.

Operational behavior:

- Program issues VGA DAC update through INT 10h (`AX = 1012h`).
- `BL` carries start index, `CL` carries count.
- `DX` points to triplet data.

### 2) Frame/control record stream

In `CommonUnknown_display_1000_0E59_10E59`, the stream is interpreted as a compact header plus payload controls.

Observed semantics:

- `DI`: control/flag word
- `CX`: length/size-related word
- `CX == 0`: early out for this record
- `DI & 0x0200`: auxiliary decode pre-pass

After header interpretation, additional words are read and used by blit/decode logic.

## Compression Core

Decoder anchor: `CommonUnknownSplit_1000_0F30_10F30`.

### Conceptual model

The codec is a bitstream LZ variant with:

- literal byte emissions
- short backward references
- long backward references
- in-band end marker

### Bitstream mechanics

- Bit reservoir lives in 16-bit `BP`.
- Control path shifts bits one at a time.
- On depletion, fetch next 16-bit word from stream.
- Carry/zero flag combinations drive token selection.

This is not byte-framed coding; token boundaries are bit-granular.

### Decoder preamble

`CommonUnknown_1000_0EFE_10EFE` skips 6 bytes before entering the token loop.

Current safest interpretation:

```text
u8 preamble[6]
u8 compressed_bitstream[]
```

Exact semantic naming of all six preamble bytes remains open.

### Token behavior details

Literal token:

```text
dst[DI] = src[SI]
SI++
DI++
```

Short back-reference token:

- Reads compact length from control bits.
- Reads one-byte offset.
- Extends to 16-bit source location in already produced output window.
- Copies from prior output to current output.

Long back-reference token:

- Reads 16-bit packed descriptor.
- Decodes larger backward offset.
- Length is inline 3-bit value or extended byte.
- Extended length value `0` is end-of-stream.

Copy source is the already decoded output segment, not a separate dictionary.

Reported output byte count after decode pass:

```text
CX = DI - DI_start
```

## `DI & 0x0200` Auxiliary Path

When bit `0x0200` is set:

- control bit is cleared before continuing
- decode target switches to auxiliary segment (`0x1131` path in current reconstruction)
- state is saved/restored around decode call

This path allows the same coded payload family to be routed through an intermediate segment before final display handling.

## Timing Contract and Playback Cadence

The format is consumed in a timed loop, not in one bulk decode.

- Render/update stage executes per record.
- Program waits using BIOS/video timing behavior.
- Keyboard is polled between iterations to permit early termination.

Implication: high-fidelity reproduction must preserve ordering and pacing, not just decode bytes correctly.

## Relationship Between LOGO.EXE and LOGO.HNM

Think of the pair as engine + script:

- `LOGO.HNM` carries encoded visual instructions and payloads.
- `LOGO.EXE` interprets those instructions, owns timing, and controls VGA side effects.

Neither artifact alone fully defines observed behavior.

## Current Unknowns

Still unresolved or intentionally provisional:

- exact semantic role of each byte in the 6-byte decode preamble
- complete semantic naming for frame-header fields (`DI`, `CX`, later words)
- whether every tiny `0x0006` tail chunk is control, padding, or mixed role
- full taxonomy of all record subtypes in late-stream control sections

## Practical One-Line Description

`LOGO.HNM` is a chunked animation stream whose frame payloads are decoded by a bit-level LZ-style codec, while `LOGO.EXE` provides orchestration, timing, palette behavior, and device-facing playback control.

## References

- `README.md`
- `GeneratedCode.cs`
- `GeneratedCode_OriginalAsm.cs`
- `tools/UnHsq/Program.cs`

Note on HSQ: `UnHsq` rejects `LOGO.HNM` (`cannot apply UNHSQ algorithm`). This is expected and reinforces that `LOGO.HNM` is its own container/codec path, not a vanilla HSQ archive.
