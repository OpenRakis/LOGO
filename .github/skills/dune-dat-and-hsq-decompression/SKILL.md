---
name: dune-dat-and-hsq-decompression
description: 'Decompress HSQ-compressed files into .UNHSQ using OpenRakis UnHsq. Works even when source extension is not .HSQ (for example LOGO.HNM).'
argument-hint: 'Input file path(s) or directory, plus output policy: skip-existing (default) or overwrite'
user-invocable: true
---

# HSQ Decompression Only (UnHsq)

Use this workflow to produce reproducible .UNHSQ outputs from HSQ-compressed files.
This skill only handles decompression.
Do not extract DUNE.DAT here and do not reimplement HSQ decoding logic.

## When To Use
- You already have HSQ-compressed files on disk.
- Inputs may use extensions other than .HSQ (for example LOGO.HNM).
- You need .UNHSQ outputs for reverse engineering or offline analysis.
- You want repeatable decompression with clear success/failure reporting.

## Inputs
- Required: one of:
   - One or more explicit file paths that are expected to be HSQ-compressed, or
   - A directory to scan recursively.
- Optional output policy:
  - skip-existing (default)
  - overwrite

Default policy for this skill:
- If a target .UNHSQ already exists, skip decompression for that file.

## Procedure

### 1. Preflight Checks
1. Resolve all provided paths to absolute paths.
2. Confirm the decompressor project exists:
   - tools/UnHsq/UnHsq.csproj
3. Do not require a .HSQ extension; extension is not authoritative.
4. If scanning directories, prioritize files with .HSQ extension and explicitly requested known cases like LOGO.HNM.
5. Decide output policy before running.

Decision points:
- If the UnHsq project is missing, stop and report the expected path.
- If no candidate inputs remain after validation/filtering, stop with a clear "nothing to process" report.

### 2. Build Target HSQ List
1. If explicit files were provided, keep only existing files.
2. If a directory was provided, recursively discover candidates:
   - include *.HSQ
   - include known HSQ-compressed payloads when specified (for example LOGO.HNM)
3. De-duplicate the final list.
4. For skip-existing policy, mark files whose sibling .UNHSQ already exists as skipped.

### 3. Run Decompression
1. Invoke UnHsq with the selected input files:
   - dotnet run --project tools/UnHsq/UnHsq.csproj -- <file1> <file2> ...
2. For large input sets, execute in batches to keep command size manageable.
3. Continue processing remaining batches if one batch fails, and record per-batch failures.

Decision points:
- If UnHsq throws "cannot apply UNHSQ algorithm", record the file as failed and continue.
- If overwrite is selected and .UNHSQ exists, allow replacement.

### 4. Validate Outputs
1. For each processed input file, confirm corresponding sibling .UNHSQ exists.
2. Record counts:
   - total candidates discovered
   - selected inputs (after filtering)
   - processed
   - skipped
   - succeeded
   - failed
3. Optionally record size comparisons (HSQ bytes vs UNHSQ bytes).

### 5. Final Report
Provide a concise report containing:
1. Input roots/files used.
2. Tool project path used.
3. HSQ discovery and selection counts.
4. Decompression counts.
5. Failed files and error snippets (if any).

## Completion Criteria
- Candidate enumeration completed from the provided inputs.
- Targeted files were attempted according to policy.
- .UNHSQ outputs exist for successful files.
- A structured report is delivered with counts and paths.

## Safety And Consistency Rules
- Always use the OpenRakis UnHsq project; do not rewrite decompression logic.
- Do not modify source input files.
- Keep operations idempotent under skip-existing policy.
- Resolve ambiguous paths to absolute paths before execution.

## Example
- Decompress LOGO.HNM directly:
   - dotnet run --project tools/UnHsq/UnHsq.csproj -- C:/Users/noalm/source/repos/LOGO/LOGO.HNM
- Expected output:
  - C:/Users/noalm/source/repos/LOGO/LOGO.UNHSQ
