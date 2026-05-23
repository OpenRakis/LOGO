---
description: "Absolute prohibition on fallbacks, heuristics, defensive masking, and silent boundary clipping anywhere in the repository. Applies to all production source. High-fidelity reverse-engineering work must fail loudly, never invent behavior."
applyTo: "**/*.cs"
---

# Repository Rule: NO FALLBACK. EVER.

This repository implements a reverse-engineered DOS animation player. High fidelity to the original behavior is the highest priority. Fallbacks of any kind hide divergence from the original program and are forbidden.

## Forbidden Patterns

The following patterns are strictly forbidden in any production source file:

- **Heuristic classification**: Do not guess the type, format, or kind of a record by sniffing bytes. Either follow the original format definition exactly, or fail loudly.
- **Best-effort decoding**: Do not silently substitute zero, default, or invented values when a decode step cannot read what the original required. Either decode exactly, or fail loudly.
- **Silent boundary clipping**: Do not silently `return`, `break`, or `continue` when a source cursor, destination cursor, or counter would exceed an expected limit. Either match the original's exact behavior at that boundary, or fail loudly.
- **"Resilient" raw fallback paths**: Do not run a raw/uncompressed code path when a compressed/expected code path fails. Either implement the original code path exactly, or fail loudly.
- **Default values on missing input**: Do not return `0`, `null`, `empty`, or any default value to "keep going" when the original required a specific value. Either provide the correct value, or fail loudly.
- **UI pacing hacks**: Do not clamp, throttle, or otherwise modify timing math to "look better". Match the original's timing math exactly.
- **Invented control flow**: Do not add branches, retries, or alternate paths that are not in the original assembly or documented format.

## What "Fail Loudly" Means

When the original behavior cannot be reproduced exactly because evidence is missing or the input does not match expectations:

1. In `GeneratedCode*.cs` and `MyOverrideSupplier.cs`: throw `FailAsUntested(...)` per the override conventions.
2. In all other production code: throw an exception that names the exact violated invariant, OR add a `Debug.Assert(false, "...")` and let the code crash on the next step.
3. Add a comment that points to the original asm reference (segment:offset and the function it lives in).

Do not catch and ignore. Do not return a sentinel and continue. Do not silently truncate.

## Allowed Patterns

- Strict parsing that throws when the format is violated.
- Assertions (`Debug.Assert`) that capture invariants discovered during reverse engineering.
- Explicit `throw new InvalidDataException(...)`, `throw new InvalidOperationException(...)` etc. when the input cannot conform to the original format.
- Strict equality checks against original header values, magic numbers, control words, and mode flags.

## Why

Fallbacks make a divergence between the player and the original program invisible. Once invisible, they accumulate into incorrect playback that looks "close enough" but is wrong in ways that are very hard to debug. The repository's stated goal is exact fidelity; fallbacks defeat that goal at every level.

## Enforcement

- Code review must reject any patch that introduces a fallback as defined above.
- If a fallback already exists in legacy code, it must be removed in the next reverse-engineering pass and replaced with a strict implementation or a loud failure.
- If the original behavior is genuinely unknown, the code must say so with `FailAsUntested` or a throw, not invent behavior.
