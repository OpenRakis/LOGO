# LOGO.HNM Format Notes

This document records the current best reconstruction of the `LOGO.HNM` file as used by `LOGO.EXE` in the PC floppy version of Dune.

The short version is:

- `LOGO.HNM` is not plain HSQ data.
- The repository's `UnHsq` tool rejects it with `cannot apply UNHSQ algorithm`, so the file should be treated as a custom HNM animation container, not as a vanilla HSQ archive.
- The playback code consumes the file as a chain of self-sized chunks, then interprets those chunks as palette updates and compressed frame payloads.
- The actual frame compression is a small bitstream codec with literal bytes, short back-references, long back-references, and an end-of-stream marker.

The goal here is not to pretend that every byte is solved. The goal is to document what is directly supported by the code and by raw-file inspection, and to mark the remaining uncertain areas explicitly.

## What Was Verified

The following points are directly supported by the generated code and the original assembly dump:

- The program opens `LOGO.HNM` directly at startup. See [GeneratedCode.cs](GeneratedCode.cs#L32) and [README.md](README.md#L1).
- The file is read in large blocks through DOS Int 21h read calls, then closed when the stream is exhausted. See [HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A](GeneratedCode.cs#L1008).
- Playback advances through a pointer stored in `DS:[0x56]` / `DS:[0x58]`, which behaves like a current stream address. See [WritePaletteDataAddress](GeneratedCode.cs#L858) and [UpdatePaletteDataAddress_1000_0E86_10E86](GeneratedCode.cs#L559).
- The visible palette list is decoded from the stream before frame rendering begins. See [CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4](GeneratedCode.cs#L871).
- The frame path reads a small header, optionally decodes into an auxiliary buffer when flag bit `0x200` is set, then runs a bitstream decoder into the VGA framebuffer. See [CommonUnknown_display_1000_0E59_10E59](GeneratedCode.cs#L579), [CommonUnknown_1000_0EBD_10EBD](GeneratedCode.cs#L660), [CommonUnknown_1000_0EFE_10EFE](GeneratedCode.cs#L699), and [CommonUnknownSplit_1000_0F30_10F30](GeneratedCode.cs#L714).
- The decoder returns the number of bytes produced in `CX` and uses the carry flag / zero flag contract as part of its control flow. See the same decoder block in [GeneratedCode_OriginalAsm.cs](GeneratedCode_OriginalAsm.cs#L2334).

## High-Level File Shape

The raw `LOGO.HNM` file on disk is `18417` bytes long.

The file is not a single monolithic blob. It is a sequence of variable-size chunks. The first word of each chunk is a little-endian size, and the next chunk begins by advancing that many bytes from the current chunk start.

That behavior is implemented in `UpdatePaletteDataAddress_1000_0E86_10E86`:

```text
current = [segment:offset]
size    = word at current
current = current + size
normalize current back to segment:offset
```

`CommonUnknown_1000_0EB2_10EB2` then loads the size word and leaves `CX = size - 2`. That is the strongest evidence that the leading word is a self-size field, not just a payload length.

### Observed Outer Chunk Chain

The following offsets and sizes were observed by walking the raw file with the same size arithmetic used by the program:

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

The repeated `0x0006` chunks at the end are real on disk. They are not a guessed artifact of the parser.

## Outer Container Layout

The top-level view that best fits the evidence is:

```text
HNM file
  repeated chunk:
    u16 chunk_size   ; includes the size word itself
    u8  payload[chunk_size - 2]
```

The payload is not uniform across all chunks. The same file contains at least two distinct kinds of payloads:

- palette update records, which are parsed by `CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4`
- frame/control records, which are parsed by `CommonUnknown_display_1000_0E59_10E59`

The code does not give those payloads friendly names, so this document keeps the more generic label `chunk` for the outer wrapper and `record` for the inner logical structures.

## Palette Record Structure

The palette path starts at the stream pointer, skips the leading size word, and then reads a stream of palette records until it sees a terminator byte `0xFF` in `AL`.

The observed layout of one palette record is:

```text
u8 start_index
u8 color_count
u8 rgb[color_count * 3]
```

The palette loader then:

- loads the palette into the VGA DAC with Int 10h function `AX = 1012h`
- uses `BL = start_index`
- uses `CL = color_count`
- uses `DX` as the source address of the RGB triplets
- copies the RGB data into a code segment scratch area with a helper call

The termination rule is simple:

- if the first byte of the record header is `0xFF`, the palette list ends

That is why the first word of the very first chunk matters so much. On disk, it begins with `0x5655`, which means the first palette record starts at index `0x55` and contains `0x56` colors.

## Frame Header Structure

The frame path is visible in `CommonUnknown_display_1000_0E59_10E59`.

The routine reads a small header from the stream and interprets it like this:

- `DI` receives the first word after the chunk-size word and acts as flags or a control word
- `CX` receives the second word and acts as a length or record-size field
- if `CX == 0`, the frame branch ends early
- if `DI & 0x200` is set, a special auxiliary decode path is taken first
- after that, two more words are read into `DX` and `BX`
- the main display path writes to VGA segment `0xA000` and invokes the blitter

The important part is that the code treats the first two words as a header and the next two words as additional parameters.

### The `0x200` Bit

The flag `0x200` is special.

When the bit is set, the code enters `CommonUnknown_1000_0EBD_10EBD`, which:

- clears bit `0x200` from `DI`
- redirects output to segment `0x1131`
- saves and restores `CX`, `DI`, `DS`, and related stack state
- calls the inner decoder at `CommonUnknown_1000_0EFE_10EFE`

That means the same compressed stream can be decoded either into a scratch buffer or directly into the visible buffer, depending on the header flags.

### The Six-Byte Decoder Prologue

`CommonUnknown_1000_0EFE_10EFE` skips six bytes before the bitstream decoder starts.

That is the strongest evidence for the decoder prologue being a fixed-size subheader:

```text
u8[6] decoder_preamble
compressed_bitstream
```

The exact semantic name of those six bytes is not fully proven yet, so the safest statement is that the decoder consumes a 6-byte preamble and then starts bit-level decoding.

## Compression Algorithm

The core of the file compression is the function at [CommonUnknownSplit_1000_0F30_10F30](GeneratedCode.cs#L714). It is best described as a prefix-code stream with LZ-style back-references.

### Bit Buffer Behavior

The decoder maintains a 16-bit bit buffer in `BP`.

The flow is:

- shift `BP` right by one bit each iteration
- if the buffer becomes empty, load the next word from `DS:SI`
- use `RCR` to seed the carry flag from the new stream word
- carry and zero flag combinations choose the next token type

This is not a byte-aligned format. It is a bit-level prefix stream.

### Token Types

The decoder can emit three kinds of output:

1. A literal byte copied directly from the input stream to the output buffer.
2. A short back-reference, using a one-byte signed-ish offset and a compact length.
3. A long back-reference, using a wider offset and a short or extended length.

The end of the stream is also encoded in-band.

### Literal Path

The literal path is the simplest one.

When the control bits select the literal branch, the decoder performs a byte copy from the stream source to the output destination.

In the generated code this is the `label_2` path:

```text
output[DI] = input[SI]
SI++
DI++
```

### Short Back-Reference Path

The short-copy path constructs a small length value from control bits and then reads a single offset byte from the stream.

The offset byte is expanded into a 16-bit address by forcing the high byte to `0xFF`, then it is added to `DI` to form a source pointer inside the already-decoded output window.

The code then temporarily swaps `SI` with that computed source address and copies bytes from the output buffer back into the current destination.

The practical effect is LZ-style copy-from-previous-output.

### Long Back-Reference Path

The long-copy path reads a 16-bit word and then repacks it into:

- a larger backward offset
- a 3-bit inline length, or
- an extended length byte when the low 3 bits are zero

The extended-length case is also the stream terminator case:

- if the extension length byte is zero, the decoder stops

That gives the format an in-band end marker without needing a separate chunk terminator inside the compressed stream.

### Copy Semantics

The copy helper temporarily swaps `DS` to the destination segment so the decoder can copy from already written output bytes while still reading the input stream from the original location when done.

This matters because the decoder is not copying from a separate dictionary buffer. It is copying from its own output window.

The final byte count is returned in `CX` as:

```text
CX = DI - original_DI
```

That is, the number of bytes written by the decode pass.

## Timing and Playback Loop

The visual playback loop is separate from the format itself, but it matters because it explains why the HNM stream is consumed in short bursts.

After rendering each frame, the code waits for approximately one video frame interval by polling the BIOS tick counter at `0000:046C`, then it checks for keyboard input to allow early exit.

That means the HNM stream is not merely decoded once. It is consumed over time, one frame at a time, as part of the animation loop.

The relevant entry point is [HNMUnknown_1000_0FEA_10FEA](GeneratedCode.cs#L884).

## Practical Reconstruction Summary

The best current reconstruction of the format is:

```text
LOGO.HNM
  chunk 0
    u16 size
    payload
      palette record list and/or header data
  chunk 1
    u16 size
    payload
      frame/control data
  chunk 2
    u16 size
    payload
      more frame/control data
  ...
```

Inside the playback path, the stream splits into two logical layers:

- an outer chunked container with self-sized records
- an inner compressed bitstream used for frame expansion

If you want the most implementation-friendly one-line description, it is this:

> `LOGO.HNM` is a chunked animation stream whose frame payloads are bitstream-compressed with a compact LZ-style decoder, not an HSQ archive.

## What Is Still Uncertain

The following parts are still best treated as provisional:

- the exact semantic name of the 6-byte decoder preamble
- the exact meaning of the `DI` and `CX` header fields beyond their observed control flow
- whether every chunk type uses the same logical internal record structure
- whether the repeated `0x0006` chunks near the end are pure padding, palette-control records, or a distinct control subformat

Where this document uses a generic name like `chunk`, `record`, or `header`, that is intentional. It avoids turning an inference into a false certainty.

## Source References

- [README.md](README.md) for the high-level startup and file-loading summary.
- [GeneratedCode.cs](GeneratedCode.cs#L32) for the entry point that opens `LOGO.HNM`.
- [GeneratedCode.cs](GeneratedCode.cs#L559) for the chunk-pointer update logic.
- [GeneratedCode.cs](GeneratedCode.cs#L579) for the frame-header reader.
- [GeneratedCode.cs](GeneratedCode.cs#L660) for the auxiliary decode path.
- [GeneratedCode.cs](GeneratedCode.cs#L699) for the 6-byte prologue before bitstream decoding.
- [GeneratedCode.cs](GeneratedCode.cs#L714) for the core compressed-stream decoder.
- [GeneratedCode.cs](GeneratedCode.cs#L871) for the palette-loading loop.
- [GeneratedCode.cs](GeneratedCode.cs#L1008) for file read / close handling.
- [GeneratedCode_OriginalAsm.cs](GeneratedCode_OriginalAsm.cs#L1807) and [GeneratedCode_OriginalAsm.cs](GeneratedCode_OriginalAsm.cs#L2334) for the original assembly-aligned versions of the same logic.

## Note On Decompression

The repository contains a reusable HSQ decompressor at [tools/UnHsq/Program.cs](tools/UnHsq/Program.cs), but it does not apply to `LOGO.HNM` as-is. The raw file rejects that path, so the correct analysis target here is the custom HNM animation format documented above.
