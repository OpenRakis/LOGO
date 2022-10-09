# LOGO
A fully reverse-engineered real DOS app exemple (WIP), from the floppy version of DUNE.

The gif below is what LOGO.EXE does. LOGO.EXE and LOGO.HNM only exist in the PC Floppy version of DUNE.

<div style="height: 25px; width: 160px; border: 1px solid red; white-space: nowrap; text-align: center; margin: 1em 0;">
    <span style="display: inline-block; height: 100%; vertical-align: middle;"></span><img src="logo.gif" style="vertical-align: middle; max-height: 400px; max-width: 640px;" height="400" />
</div>


In this animation there is a lot.

Here is a higi level view:

- Call to the DOS Open File Interrupt (LOGO.HNM is the argument)
- The code checks 288 times overall (via the BIOS Keyboard interrupts) for any key received in the input buffer, and exit to DOS in that case.
- Constant changes to the VGA palette
- Loops to animate colors on the screen (and wait ~16.667 ms for the VGA retrace in between)
- Call to the DOS Read File Interrupt (several times)
- Copy part of the file into the main memory
- Decompression of video frames read in memory previously
- Show it to the screen (which means changes to the VGA palette, and deciding where to write which color on the screen)
- Call to the DOS Close File Interrupt
- Call to the "Quit with exit code" DOS Interrupt (here, in Spice86, Cpu.IsRunning is set to false, marking the end of the emulation loop)
- Call to to the DOS Print String Interrupt (for some reason)
- Call to the "Exit to DOS" Interrupt (here, the Spice86 emulation loop exits, since the DOS command prompt isn't emulated)

This repository serves as an example in order to document and show case the usage of Spice86.
