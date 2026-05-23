using Spice86.Core.CLI;
using Spice86.Core.Emulator.Devices.Video;
using Spice86.Shared.Emulator.Memory;
using Spice86.Shared.Interfaces;

namespace logo;

/// <summary>
/// This is the resulting conversion by hand to high level code.
/// Work in progress.
/// </summary>
public class RewrittenOverrides : CSharpOverrideHelper
{
    private const ushort NumberOfFramesRemainingOffset_CB6 = 0xCB6;
    /// <summary>
    /// Observed entry segment address at generation time
    /// </summary>
    private const ushort EntrySegmentAddress = 0x1000;

    public RewrittenOverrides(Dictionary<SegmentedAddress, FunctionInformation> functionInformations, Machine machine, ILoggerService loggerService, Configuration configuration, ushort entrySegment = 0x1000) : base(functionInformations, machine, loggerService, configuration)
    {
        DefineGeneratedCodeOverrides();
        SetProvidedInterruptHandlersAsOverridden();
    }

    public void DefineGeneratedCodeOverrides()
    {
        DefineFunction(EntrySegmentAddress, 0x0, EntryPoint_OpenLogoHnmFileAndRun_1000_0000_10000, false);
    }

    /// <summary>
    /// Program entry point. Builds the "LOGO.HNM" filename, opens the file via DOS Int21 OpenFile,
    /// switches the VGA to mode 13h, runs the circles intro animation, then quits to DOS. The trailing
    /// PrintString / second QuitWithExitCode after the main quit are unreachable in normal execution.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public virtual Action EntryPoint_OpenLogoHnmFileAndRun_1000_0000_10000(int offset)
    {
        DS = 0x111C;
        AX = UInt16[ES, 2];
        UInt16[DS, 0x50] = AX;
        BX = 8;
        AX = 0xC;
        SI = 0x8E;
        Alu8.Sub(UInt8[DS, SI], 0);
        UInt16[DS, 0x52] = AX;
        DX = 0x5A;
        CirclesUnknown_1000_105F_1105F();
        Alu8.Sub(UInt8[DS, DI], 46);
        if (!ZeroFlag)
        {
            Memory.SetZeroTerminatedString(MemoryUtils.ToPhysicalAddress(DS, DI), ".HNM", 5);
        }
        // Open file handle (LOGO.HNM)
        Machine.Dos.DosInt21Handler.OpenFileOrDevice(false);
        DX = 0x2E;
        if (!CarryFlag)
        {
            Stack.Push16(AX);
            SetVideoMode_1000_0970_10970();
            CirclesUnknown_1000_1019_11019();
            BX = Stack.Pop16();
            AX = UInt16[DS, 0x52];
            CirclesAnimation_1000_0DDE_10DDE();
            DX = 0;
            AL = 0;
            // End of LOGO.EXE
            // Quit With Exit Code (Cpu.IsRunning set to false)
            Machine.Dos.DosInt21Handler.QuitWithExitCode();
        }
        // End of LOGO.EXE
        // Print string: "\u0006<zw\u0002"
        Machine.Dos.DosInt21Handler.PrintString();
        // End of LOGO.EXE
        // Quit to DOS
        Machine.Dos.DosInt21Handler.QuitWithExitCode(); ;
        return () => { };
    }

    /// <summary>
    /// Sets Video mode VGA to 0x13
    /// </summary>
    public void SetVideoMode_1000_0970_10970()
    {
        AX = 0x13;
        Machine.VideoInt10Handler.SetVideoMode();

    }

    /// <summary>
    /// Sleeps ~one VGA frame (~16.67 ms) then writes <c>CX * 3</c> palette bytes (RGB triples)
    /// starting at <c>DS:DX</c> into the VGA DAC, using <c>BL</c> as the starting DAC index.
    /// </summary>
    public void CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8()
    {
        int colors = CX;
        ushort colorOffset = DX;
        Thread.Sleep(1000 / 60);
        IVideoState videoState = Machine.VgaRegisters;
        videoState.DacRegisters.IndexRegisterWriteMode = BL;
        for (int i = 0; i < colors * 3; i++)
        {
            videoState.DacRegisters.DataRegister = UInt8[DS, (ushort)(colorOffset + i)];
        }

    }

    /// <summary>
    /// Computes a linear VGA mode 13h screen offset for a given row/column: <c>DI = BX * 320 + DX</c>.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void ConvertLineNumberToArrayIndex_1000_0A22_10A22()
    {
        DI = (ushort)(BX * 320 + DX);

    }

    /// <summary>
    /// Blits a rectangle of <c>BX</c> rows by <c>BP</c> columns from <c>DS:SI</c> to <c>ES:DI</c>
    /// with a 320-byte row stride (VGA mode 13h). The exact code path depends on the value of
    /// <c>CH</c> (0xFE vs 0xFF) and the sign of <c>DI</c>: it performs either a plain word/byte copy,
    /// or a sparse copy where a zero byte from the source skips one destination byte and a non-zero
    /// byte is written. Used to draw the animated circles into the VGA framebuffer.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CommonUnknown_1000_0B9A_10B9A()
    {
        Alu8.Sub(CH, 0xFE);
        DI = Alu16.Or(DI, DI);
        if (!SignFlag)
        {
            BP = DI;
            BP = Alu16.And(BP, 511);
            AX = DI;
            ConvertLineNumberToArrayIndex_1000_0A22_10A22();
            BX = CX;
            BH = 0;
            Alu8.Sub(CH, byte.MaxValue);
            if (!ZeroFlag)
            {
                BP = Alu16.Shr(BP, 1);
                AX = DI;
                if (!CarryFlag)
                {
                    do
                    {
                        CX = BP;
                        DI = AX;
                        while (CX != 0)
                        {
                            CX--;
                            UInt16[ES, DI] = UInt16[DS, SI];
                            SI += (ushort)(uint)Direction16;
                            DI += (ushort)(uint)Direction16;
                        }
                        AX += 320;
                        BX = Alu16.Dec(BX);
                    }
                    while (!ZeroFlag);
                    return;
                }
            }
            DX = DI;
        label_76:
            do
            {
                CX = BP;
                DI = DX;
                ushort num3 = 1;
                do
                {
                    AL = UInt8[DS, SI];
                    SI += (ushort)(uint)Direction8;
                    AL = Alu8.Or(AL, AL);
                    if (!ZeroFlag)
                    {
                        UInt8[ES, DI] = AL;
                        DI += (ushort)(uint)Direction8;
                        if (--CX == 0)
                        {
                            DX += 320;
                            BX = Alu16.Dec(BX);
                            if (ZeroFlag)
                                return;
                            goto label_76;
                        }
                    }
                    else
                    {
                        DI = Alu16.Inc(DI);
                        num3 = --CX;
                    }
                }
                while (num3 != 0);
                DX += 320;
                BX = Alu16.Dec(BX);
            }
            while (!ZeroFlag);
            return;
        }
        BP = DI;
        BP = Alu16.And(BP, 511);
        AX = DI;
        ConvertLineNumberToArrayIndex_1000_0A22_10A22();
        BX = CX;
        BH = 0;
        Alu16.And(AX, 16384);
    label_3:
        do
        {
            AL = UInt8[DS, SI];
            SI += (ushort)(uint)Direction8;
            AL = Alu8.Or(AL, AL);
            if (!SignFlag)
            {
                CX = AX;
                CH = 0;
                ++CX;
                BP = Alu16.Sub(BP, CX);
                while (true)
                {
                    AL = UInt8[DS, SI];
                    SI += (ushort)(uint)Direction8;
                    AL = Alu8.Or(AL, AL);
                    if (!ZeroFlag)
                    {
                        UInt8[ES, DI] = AL;
                        DI += (ushort)(uint)Direction8;
                        if (--CX == 0)
                        {
                            BP = Alu16.Or(BP, BP);
                            if (ZeroFlag)
                            {
                                BX = Alu16.Dec(BX);
                            }
                            else
                            {
                                goto label_3;
                            }
                        }
                    }
                }
            }
            else
            {
                CX = 0x101;
                AH = 0;
                CX -= (ushort)(uint)AX;
                BP = Alu16.Sub(BP, CX);
                AL = UInt8[DS, SI];
                SI += (ushort)(uint)Direction8;
                AL = Alu8.Or(AL, AL);
                goto label_18;
            }
        label_18:
            DI = Alu16.Add(DI, CX);
            BP = Alu16.Or(BP, BP);
        }
        while (!CarryFlag && !ZeroFlag);
        BX = Alu16.Dec(BX);
        UInt8[EntrySegmentAddress, 0xA71] = 0xC7;
        UInt8[EntrySegmentAddress, 0xB2F] = 0xC7;
    }

    /// <summary>
    /// Performs a vertical mirror/flip blit of the VGA mode 13h framebuffer (segment 0xA000):
    /// the first pass copies the top 100 lines into rows 1..100 with per-word byte-swap,
    /// the second pass copies them into the area starting at offset 0xF8C0. Effectively
    /// produces the symmetric reflection used by the circles animation backdrop.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CirclesUnknown_display_1000_0C72_10C72()
    {
        Stack.Push16(DS);
        AX = 0xA000;
        ES = AX;
        DS = AX;
        SI = 0;
        DI = 320;
        DX = 0x64;
        do
        {
            CX = 0x50;
            ushort num;
            do
            {
                AX = UInt16[DS, SI];
                SI += (ushort)Direction16;
                (AH, AL) = (AL, AH);
                DI = Alu16.Sub(DI, 2);
                UInt16[DS, DI] = AX;
                num = --CX;
            }
            while (num != 0);
            SI += 0xA0;
            DI += 0x1E0;
            DX = Alu16.Dec(DX);
        }
        while (!ZeroFlag);
        SI = 0;
        DI = 0xF8C0;
        DX = 0x64;
        do
        {
            CX = 0xA0;
            while (CX != 0)
            {
                --CX;
                UInt16[ES, DI] = UInt16[DS, SI];
                SI += (ushort)Direction16;
                DI += (ushort)Direction16;
            }
            DI -= 0x280;
            DX = Alu16.Dec(DX);
        }
        while (!ZeroFlag);
        DS = Stack.Pop16();

    }

    /// <summary>
    /// Main loop driving the circles animation. Performs the initial framebuffer flip blit, then
    /// initializes the per-frame counters at <c>EntrySegment:0xCB6/0xCBA/0xCB0/0xCB2/0xCB4</c>
    /// (frames-remaining, palette-stream pointer, R/G/B accumulators) and iterates
    /// <see cref="CirclesDrawStep_1000_0D22_10D22"/> 0xFB (251) times, polling the keyboard
    /// between frames to allow an early exit.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CircleMainLoop_1000_0CF4_10CF4()
    {
        CirclesUnknown_display_1000_0C72_10C72();
        SI = 0xCBC;
        AX = UInt16[EntrySegmentAddress, SI];
        UInt16[EntrySegmentAddress, NumberOfFramesRemainingOffset_CB6] = AX;
        UInt16[EntrySegmentAddress, 0xCBA] = SI;
        AX = 0;
        UInt16[EntrySegmentAddress, 0xCB0] = AX;
        UInt16[EntrySegmentAddress, 0xCB2] = AX;
        UInt16[EntrySegmentAddress, 0xCB4] = AX;
        CX = 0xFB;
        ushort num;
        do
        {
            Stack.Push16(CX);
            CirclesDrawStep_1000_0D22_10D22();
            CX = Stack.Pop16();
            CommonCheckForAnyKeyStroke_1000_1085_11085();
            num = --CX;
        }
        while (num != 0 && ZeroFlag);

    }

    /// <summary>
    /// One step of the circles animation. While the frames-remaining counter is non-negative,
    /// copies the next ring of palette source bytes into the working buffer at offset 0x160,
    /// computes the next interpolated RGB triple via <see cref="CommonComputeNextVgaPalette_1000_0D5F_10D5F"/>,
    /// then waits one frame and pushes 0x50 (80) palette entries to the VGA DAC.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CirclesDrawStep_1000_0D22_10D22()
    {
        Stack.Push16(DS);
        Stack.Push16(ES);
        Stack.Push16(EntrySegmentAddress);
        Stack.Push16(EntrySegmentAddress);
        DS = Stack.Pop16();
        ES = Stack.Pop16();
        Alu16.Sub(UInt16[EntrySegmentAddress, NumberOfFramesRemainingOffset_CB6], 0);
        if (!SignFlag)
        {
            DI = 0x160;
            SI = DI;
            CX = 0xF0;
            DX = UInt16[EntrySegmentAddress, 0xCB8];
            AX = DX;
            AX <<= 1;
            AX += DX;
            SI += AX;
            CX = Alu16.Sub(CX, AX);
            while (CX != 0)
            {
                CX--;
                UInt8[ES, DI] = UInt8[DS, SI];
                SI += (ushort)Direction8;
                DI += (ushort)Direction8;
            }
            CX = DX;
            CommonComputeNextVgaPalette_1000_0D5F_10D5F();
            DX = 0x160;
            BX = 0x50;
            CX = 0x50;
            CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8();
        }
        ES = Stack.Pop16();
        DS = Stack.Pop16();

    }

    /// <summary>
    /// Computes the next RGB triple for the rolling circles palette. For each of the R, G and B
    /// channels the per-channel accumulator stored at <c>EntrySegment:0xCB0/0xCB2/0xCB4</c> is
    /// advanced by a delta read from the current palette-stream entry, and the high byte
    /// (shifted left by 1) masked with 0x3F is written to the destination at <c>ES:DI</c>.
    /// When the frames-remaining counter at <c>0xCB6</c> reaches zero the palette-stream
    /// pointer at <c>0xCBA</c> is advanced by 8 bytes to the next segment.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CommonComputeNextVgaPalette_1000_0D5F_10D5F()
    {
        SI = UInt16[EntrySegmentAddress, 0xCBA];
        UInt16[EntrySegmentAddress, NumberOfFramesRemainingOffset_CB6] = Alu16.Dec(UInt16[EntrySegmentAddress, NumberOfFramesRemainingOffset_CB6]);
        if (ZeroFlag)
        {
            SI = Alu16.Add(SI, 8);
            UInt16[EntrySegmentAddress, 0xCBA] = SI;
            AX = UInt16[DS, SI];
            UInt16[EntrySegmentAddress, NumberOfFramesRemainingOffset_CB6] = AX;
        }
        AX = UInt16[DS, (ushort)(SI + 2U)];
        AX = Alu16.Add(AX, UInt16[EntrySegmentAddress, 0xCB0]);
        UInt16[EntrySegmentAddress, 0xCB0] = AX;
        AL = Alu8.Shl(AL, 1);
        AH = Alu8.Adc(AH, 0);
        AL = AH;
        AL = Alu8.And(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        AX = UInt16[DS, (ushort)(SI + 0x4)];
        AX = Alu16.Add(AX, UInt16[EntrySegmentAddress, 0xCB2]);
        UInt16[EntrySegmentAddress, 0xCB2] = AX;
        AL = Alu8.Shl(AL, 1);
        AH = Alu8.Adc(AH, 0);
        AL = AH;
        AL = Alu8.And(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        AX = UInt16[DS, (ushort)(SI + 0x6)];
        AX = Alu16.Add(AX, UInt16[EntrySegmentAddress, 0xCB4]);
        UInt16[EntrySegmentAddress, 0xCB4] = AX;
        AL = Alu8.Shl(AL, 1);
        AH = Alu8.Adc(AH, 0);
        AL = AH;
        AL = Alu8.And(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;

    }

    /// <summary>
    /// Refills the palette/HNM data buffer pointed to by <c>DS:[0x4C]:[0x4E]</c>: records the
    /// current address, compares the remaining capacity against 0x3A02, calls
    /// <see cref="HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A"/> to read the next chunk
    /// from the HNM file, then advances <c>DI</c> by the bytes-read count and zero-terminates
    /// the buffer.
    /// </summary>
    public void CirclesUnknown_1000_0DBC_10DBC()
    {
        // LES DI,[0x4c] (1000_0DBC / 0x10DBC)
        DI = UInt16[DS, 0x4C];
        ES = UInt16[DS, 0x4E];
        WritePaletteDataAddress(ES, DI);
        // MOV CX,ES (1000_0DC8 / 0x10DC8)
        CX = ES;
        // MOV AX,[0x50] (1000_0DCA / 0x10DCA)
        AX = UInt16[DS, 0x50];
        // SUB AX,CX (1000_0DCD / 0x10DCD)
        AX -= CX;
        // CMP AX,0x3a02 (1000_0DCF / 0x10DCF)
        Alu16.Sub(AX, 0x3A02);
        // CALL 0x1000:109a (1000_0DD4 / 0x10DD4)
        HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A();
        // ADD DI,CX (1000_0DD7 / 0x10DD7)
        DI += CX;
        // XOR AX,AX (1000_0DD9 / 0x10DD9)
        AX = 0;
        // STOSW ES:DI (1000_0DDB / 0x10DDB)
        UInt16[ES, DI] = AX;
        DI = (ushort)(DI + Direction16);
        // CLC  (1000_0DDC / 0x10DDC)
        CarryFlag = false;
        // RET  (1000_0DDD / 0x10DDD)

    }

    /// <summary>
    /// Top-level orchestrator for the circles animation phase. Stores the file handle, primes
    /// the HNM data buffer via <see cref="CirclesUnknown_1000_0DBC_10DBC"/>, loads the initial
    /// VGA palette, then runs <see cref="CircleMainLoop_1000_0CF4_10CF4"/> followed by the HNM
    /// per-frame loop calling <see cref="HNMUnknown_1000_0FEA_10FEA"/> until the remaining-frames
    /// counter at <c>DS:0x52</c> hits zero, and finally verifies the "LO" signature at offset 0xEE.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CirclesAnimation_1000_0DDE_10DDE()
    {
        // Read File
        UInt16[DS, 0x54] = AX;
        CirclesUnknown_1000_0DBC_10DBC();
        AX = UInt16[DS, 84];
        UInt16[DS, 0x52] = AX;
        DI = UInt16[DS, 76];
        ES = UInt16[DS, 78];
        WritePaletteDataAddress(ES, DI);
        CommonUnknown_1000_0EAD_10EAD();
        CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4();
        CommonUnknown_1000_0E4C_10E4C();
        CarryFlag = true;
        CommonUnknown_1000_0E49_10E49();
        CircleMainLoop_1000_0CF4_10CF4();
        CarryFlag = true;

        do
        {
            AX = UInt16[DS, 0x52];
            BP = 0xE46;
            HNMUnknown_1000_0FEA_10FEA();
            CarryFlag = true;
            Alu16.Sub(UInt16[DS, 0x52], 0);
        }
        while (!ZeroFlag);
        SI = 0xEE;
        AX = UInt16[DS, SI];
        SI += (ushort)Direction16;
        byte ah1 = AH;
        byte al1 = AL;
        AL = ah1;
        AH = al1;
        Alu16.Sub(AX, 0x4C4F);
        CarryFlag = false;

    }

    /// <summary>
    /// ASM fall-through wrapper: invokes <see cref="CirclesDrawStep_1000_0D22_10D22"/> and then
    /// continues straight into <see cref="CommonUnknown_1000_0E49_10E49"/> without an intervening
    /// return.
    /// </summary>
    public void HNMUnknown_1000_0E46_10E46()
    {
        // CALL 0x1000:0d22 (1000_0E46 / 0x10E46)
        CirclesDrawStep_1000_0D22_10D22();
        // Function call generated as ASM continues to next function entry point without return
        CommonUnknown_1000_0E49_10E49();
    }

    /// <summary>
    /// ASM fall-through wrapper: invokes <see cref="CommonUnknown_display_1000_0E59_10E59"/> and
    /// then continues straight into <see cref="CommonUnknown_1000_0E4C_10E4C"/>.
    /// </summary>
    public void CommonUnknown_1000_0E49_10E49()
    {
        // CALL 0x1000:0e59 (1000_0E49 / 0x10E49)
        CommonUnknown_display_1000_0E59_10E59();
        // Function call generated as ASM continues to next function entry point without return
        CommonUnknown_1000_0E4C_10E4C();
    }

    /// <summary>
    /// Advances the HNM/palette stream pointer via
    /// <see cref="UpdatePaletteDataAddress_1000_0E86_10E86"/>. When the new chunk reports a size
    /// of zero (ZeroFlag set on return), zeroes the remaining-frames counter at <c>DS:0x52</c>,
    /// which terminates the outer animation loop and allows the program to exit.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CommonUnknown_1000_0E4C_10E4C()
    {
        UpdatePaletteDataAddress_1000_0E86_10E86();
        if (!ZeroFlag)
        {
            return;
        }
        //Runs on program end
        //Without this line, the program doesn't exit.
        UInt16[DS, 0x52] = 0;

    }

    /// <summary>
    /// Reads the next HNM frame header from the current palette/HNM stream (flags into <c>DI</c>,
    /// length into <c>CX</c>, two command words into <c>DX</c> and <c>BX</c>). If the 0x200 flag
    /// bit is set the auxiliary buffer decoder <see cref="CommonUnknown_1000_0EBD_10EBD"/> is
    /// invoked first; then the main HNM bitstream decoder
    /// <see cref="CommonUnknown_1000_0B9A_10B9A"/> is called to draw the decoded pixels into
    /// the VGA framebuffer at <c>ES = 0xA000</c>.
    /// TODO: High level rewrite this first.
    /// </summary>
    public void CommonUnknown_display_1000_0E59_10E59()
    {
        Stack.Push16(DS);
        SI = PaletteDataAddress.Offset;
        DS = PaletteDataAddress.Segment;
        SI = Alu16.Add(SI, 2);
        AX = UInt16[DS, SI];
        SI += (ushort)Direction16;
        DI = AX;
        AX = UInt16[DS, SI];
        SI += (ushort)Direction16;
        CX = AX;
        CL = Alu8.Or(CL, CL);
        if (!ZeroFlag)
        {
            Alu16.And(DI, 0x200);
            if (!ZeroFlag)
            {
                CommonUnknown_1000_0EBD_10EBD();
            }
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            DX = AX;
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            BX = AX;
            AX = 0xA000;
            ES = AX;
            CommonUnknown_1000_0B9A_10B9A();
        }
        DS = Stack.Pop16();

    }

    /// <summary>
    /// Advances <see cref="PaletteDataAddress"/> by the chunk-size word stored at its current
    /// location, normalizing the result back to a seg:offset pair, and updates the cached
    /// chunk size via <see cref="CommonUnknown_1000_0EB2_10EB2"/> (sets <c>CX = size - 2</c>
    /// and <c>ZeroFlag</c>).
    /// </summary>
    public void UpdatePaletteDataAddress_1000_0E86_10E86()
    {
        SegmentedAddress pointer = PaletteDataAddress;
        ushort newSegmentOffset = (ushort)(UInt16[pointer] + pointer.Offset);
        ushort newSegment = (ushort)((newSegmentOffset >> 4) + pointer.Segment);
        ushort newOffset = (ushort)(newSegmentOffset & 0xF);
        WritePaletteDataAddress(newSegment, newOffset);
        CommonUnknown_1000_0EB2_10EB2(newSegment, newOffset);
    }

    /// <summary>
    /// Reads the chunk size at the current <see cref="PaletteDataAddress"/> via
    /// <see cref="CommonUnknown_1000_0EB2_10EB2"/>, leaving <c>CX = size - 2</c> and the
    /// <c>ZeroFlag</c> set when the original chunk size is zero.
    /// </summary>
    public void CommonUnknown_1000_0EAD_10EAD()
    {
        CommonUnknown_1000_0EB2_10EB2(PaletteDataAddress.Segment, PaletteDataAddress.Offset);
    }

    /// <summary>
    /// Reads the 16-bit chunk-size word at <c>segment:offset</c>, then sets <c>CX = value - 2</c>
    /// and the <c>ZeroFlag</c> to reflect whether the original word was zero (end-of-stream).
    /// </summary>
    public Action CommonUnknown_1000_0EB2_10EB2(ushort segment, ushort offset)
    {
        ushort value = UInt16[segment, offset];
        CX = (ushort)(value - 2);
        ZeroFlag = value == 0;
        return NearRet();
    }

    /// <summary>
    /// Decodes an HNM stream into an auxiliary buffer at <c>0x1131:0000</c>. Saves caller's
    /// <c>CX</c> and <c>DI</c>, redirects the destination to the auxiliary segment, invokes the
    /// inner decoder <see cref="CommonUnknown_1000_0EFE_10EFE"/>, then restores the registers.
    /// Called when the HNM frame header indicates flag bit 0x200.
    /// </summary>
    public void CommonUnknown_1000_0EBD_10EBD()
    {
        // AND DI,0xfdff (1000_0EBD / 0x10EBD)
        // DI &= 0xFDFF;
        DI = Alu16.And(DI, 0xFDFF);
        // PUSH CX (1000_0EC1 / 0x10EC1)
        Stack.Push16(CX);
        // PUSH DI (1000_0EC2 / 0x10EC2)
        Stack.Push16(DI);
        // MOV AX,0x1131 (1000_0EC3 / 0x10EC3)
        AX = 0x1131;
        // MOV ES,AX (1000_0EC6 / 0x10EC6)
        ES = AX;
        // MOV DI,0x0 (1000_0EC8 / 0x10EC8)
        DI = 0x0;
        // PUSH DI (1000_0ECB / 0x10ECB)
        Stack.Push16(DI);
        // PUSH ES (1000_0ECC / 0x10ECC)
        Stack.Push16(ES);
        // CALL 0x1000:0efe (1000_0ECD / 0x10ECD)
        CommonUnknown_1000_0EFE_10EFE();
        // POP DS (1000_0ED0 / 0x10ED0)
        DS = Stack.Pop16();
        // POP SI (1000_0ED1 / 0x10ED1)
        SI = Stack.Pop16();
        // POP DI (1000_0ED2 / 0x10ED2)
        DI = Stack.Pop16();
        // POP CX (1000_0ED3 / 0x10ED3)
        CX = Stack.Pop16();
        // RET  (1000_0ED4 / 0x10ED4)

    }

    /// <summary>
    /// Decoder prologue: saves caller <c>CX</c>, <c>DI</c> and <c>DS</c>, skips the 6-byte HNM
    /// frame header by adding 6 to <c>SI</c>, zeros the bit-buffer <c>BP</c>, then enters the
    /// main HNM bitstream decoder loop <see cref="CommonUnknownSplit_1000_0F30_10F30"/>.
    /// </summary>
    public void CommonUnknown_1000_0EFE_10EFE()
    {
        Stack.Push16(CX);
        Stack.Push16(DI);
        Stack.Push16(DS);
        SI += 6;
        BP = 0;
        CommonUnknownSplit_1000_0F30_10F30();
    }

    /// <summary>
    /// Core HNM bitstream decoder. Consumes a 16-bit prefix-code stream from <c>DS:SI</c>
    /// (re-filling the bit buffer <c>BP</c> from the stream when it empties) and writes the
    /// decoded byte run into <c>ES:DI</c>. Supports three encodings: literal bytes, short
    /// 2-bit length back-references with a one-byte near offset, and long back-references with
    /// a 13-bit far offset followed by an optional one-byte length. Terminates when the special
    /// end-of-stream code is reached, returning the number of bytes written in <c>CX</c>.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void CommonUnknownSplit_1000_0F30_10F30()
    {
        while (true)
        {
            BP = Alu16.Shr(BP, 1);
            if (!ZeroFlag)
            {
                if (!CarryFlag)
                {
                    goto label_4;
                }
            }
            else
            {
                goto label_3;
            }

        label_2:
            UInt8[ES, DI] = UInt8[DS, SI];
            SI += (ushort)Direction8;
            DI += (ushort)Direction8;
            continue;
        label_3:
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            BP = AX;
            CarryFlag = true;
            BP = Alu16.Rcr(BP, 1);
            if (CarryFlag)
            {
                goto label_2;
            }

        label_4:
            CX = 0;
            BP = Alu16.Shr(BP, 1);
            if (ZeroFlag)
            {
                AX = UInt16[DS, SI];
                SI += (ushort)Direction16;
                BP = AX;
                CarryFlag = true;
                BP = Alu16.Rcr(BP, 1);
            }
            if (!CarryFlag)
            {
                BP = Alu16.Shr(BP, 1);
                if (ZeroFlag)
                {
                    AX = UInt16[DS, SI];
                    SI += (ushort)Direction16;
                    BP = AX;
                    CarryFlag = true;
                    BP = Alu16.Rcr(BP, 1);
                }
                CX = Alu16.Rcl(CX, 1);
                BP = Alu16.Shr(BP, 1);
                if (ZeroFlag)
                {
                    AX = UInt16[DS, SI];
                    SI += (ushort)Direction16;
                    BP = AX;
                    CarryFlag = true;
                    BP = Alu16.Rcr(BP, 1);
                }
                CX = Alu16.Rcl(CX, 1);
                AL = UInt8[DS, SI];
                SI += (ushort)Direction8;
                AH = byte.MaxValue;
            }
            else
            {
                goto label_16;
            }
            label_12();
            void label_12()
            {
                AX = Alu16.Add(AX, DI);
                (SI, AX) = (AX, SI);
                BX = DS;
                DX = ES;
                DS = DX;
                ++CX;
                CX = Alu16.Inc(CX);
                while (CX != 0)
                {
                    CX--;
                    UInt8[ES, DI] = UInt8[DS, SI];
                    SI += (ushort)Direction8;
                    DI += (ushort)Direction8;
                }
                DS = BX;
                SI = AX;
            }
            continue;
        label_16:
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            CL = AL;
            AX >>= 1;
            AX >>= 1;
            AX = Alu16.Shr(AX, 1);
            AH |= 0xE0;
            CL = Alu8.And(CL, 7);
            if (ZeroFlag)
            {
                BX = AX;
                AL = UInt8[DS, SI];
                SI += (ushort)Direction8;
                CL = AL;
                AX = BX;
                CL = Alu8.Or(CL, CL);
                if (!ZeroFlag)
                {
                    label_12();
                }
                else
                {
                    break;
                }
            }
            else
            {
                label_12();
            }
        }
        CarryFlag = true;
        CX = DI;
        DS = Stack.Pop16();
        DI = Stack.Pop16();
        SP += 2;
        CX = Alu16.Sub(CX, DI);

    }

    /// <summary>
    /// Stores the current palette/HNM data segment and offset into <c>DS:[0x58]:[0x56]</c>,
    /// the two-word slot read back by <see cref="PaletteDataAddress"/>.
    /// </summary>
    public void WritePaletteDataAddress(ushort segment, ushort offset)
    {
        UInt16[DS, 0x58] = segment;
        UInt16[DS, 0x56] = offset;
    }
    private SegmentedAddress PaletteDataAddress => new(UInt16[DS, 0x58], UInt16[DS, 0x56]);
    private SegmentedAddress PaletteDataAddressPlusTwo => new(PaletteDataAddress.Segment, (ushort)(PaletteDataAddress.Offset + 2));
    /// <summary>
    /// Walks the linked list of <see cref="PaletteData"/> records that begins at
    /// <see cref="PaletteDataAddressPlusTwo"/> and loads each entry into the VGA DAC. Used to
    /// initialize the VGA palette before the HNM playback loop starts.
    /// </summary>
    public void CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4()
    {
        PaletteData? current = new(Memory, PaletteDataAddressPlusTwo);
        while (current != null)
        {
            current.LoadInVgaDac(State, Machine.VideoInt10Handler);
            current = current.Next;
        }
    }

    /// <summary>
    /// Renders one HNM frame via <see cref="HNMUnknown_1000_0E46_10E46"/> and then sleeps until
    /// the BIOS Data Area tick counter at <c>0000:046C</c> has advanced by ~5/8 of the per-frame
    /// tick budget, polling <see cref="CSharpOverrideHelper.CheckExternalEvents"/> in the wait
    /// loop. After waking, polls the keyboard via
    /// <see cref="CommonCheckForAnyKeyStroke_1000_1085_11085"/> for an early-exit request.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void HNMUnknown_1000_0FEA_10FEA()
    {
        InterruptFlag = true;
        Stack.Push16(AX);
        AX = 0;
        ES = AX;
        Stack.Push16(UInt16[ES, 0x46C]);
        HNMUnknown_1000_0E46_10E46();
        BX = Stack.Pop16();
        BP = Stack.Pop16();
        BP >>= 1;
        BP >>= 1;
        BP = Alu16.Shr(BP, 1);
        AX = BP;
        AX >>= 1;
        AX >>= 1;
        BP -= AX;
        //Wait for timer
        do
        {
            AX = 0;
            ES = AX;
            CheckExternalEvents(EntrySegmentAddress, 0x100F);
            //BDA 40:6C => current value of the Int1A counter
            AX = UInt16[ES, 0x46C];
            AX -= BX;
            Alu16.Sub(AX, BP);
        }
        while (CarryFlag);
        CommonCheckForAnyKeyStroke_1000_1085_11085();

    }

    /// <summary>
    /// String-scan helper. Reads bytes from <c>DS:DX</c> and walks <c>DI</c> forward; mirrors
    /// the layout of <see cref="CirclesUnknown_1000_105F_1105F"/> minus the explicit comparison
    /// against '.' (0x2E). Companion to that routine in the filename-handling code path.
    /// </summary>
    public void CirclesUnknown_1000_1019_11019()
    {
        DI = DX;
        while (true)
        {
            AL = UInt8[DS, DI];
            if (!ZeroFlag)
            {
                AL = Alu8.Or(AL, AL);
                return;
            }
        }
    }

    /// <summary>
    /// Scans the zero-terminated string at <c>DS:DX</c> for the first '.' (0x2E) byte and
    /// returns with <c>DI</c> pointing at it. Used by the entry point to find the extension
    /// position before appending ".HNM" to build the filename to open.
    /// </summary>
    public void CirclesUnknown_1000_105F_1105F()
    {
        // MOV DI,DX (1000_105F / 0x1105F)
        DI = DX;
        while (true)
        {
            AL = UInt8[DS, DI];
            // CMP AL,0x2e (1000_1063 / 0x11063)
            Alu8.Sub(AL, 0x2E);
            // OR AL,AL (1000_1067 / 0x11067)
            // AL |= AL;
            AL = Alu8.Or(AL, AL);
            // JZ 0x1000:106e (1000_1069 / 0x11069)
            if (ZeroFlag)
            {
                // JZ target is RET, inlining.
                // RET  (1000_106E / 0x1106E)
                return;
            }
            // INC DI (1000_106B / 0x1106B)
            DI = Alu16.Inc(DI);
            // JMP 0x1000:1061 (1000_106C / 0x1106C)
        }
    }

    /// <summary>
    /// Checks 288 times during the whole program runtime if any key from the keyboard was received.
    /// If any, exit to DOS immediatly.
    /// </summary>
    public void CommonCheckForAnyKeyStroke_1000_1085_11085()
    {
        // Bios Interrupt: GetKeystrokeStatus
        // MOV AH,0x1 (1000_1085 / 0x11085)
        AH = 0x1;
        // INT 0x16 (1000_1087 / 0x11087)
        Interrupt(0x16);
        // If not keystroke at all, return.
        // JZ 0x1000:1091 (1000_1089 / 0x11089)
        if (ZeroFlag)
        {
            // JZ target is RET, inlining.
            // RET  (1000_1091 / 0x11091)
            return;
        }
        // Bios Interrupt: GetKeystroke (aka Read Key)
        // XOR AH,AH (1000_108B / 0x1108B)
        AH = 0;
        // INT 0x16 (1000_108D / 0x1108D)
        Interrupt(0x16);
        // RET  (1000_1091 / 0x11091)

    }

    /// <summary>
    /// Reads up to 0x8000 bytes from the currently-open file handle into <c>ES:DI</c> via
    /// DOS Int21 ReadFile (AH=0x3F), leaving the bytes-actually-read count in <c>CX</c>, then
    /// closes the file handle via DOS Int21 CloseFile (AH=0x3E). Used after the last HNM chunk
    /// has been consumed.
    /// </summary>
    /// <remarks>First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation).</remarks>
    public void HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A()
    {
        Stack.Push16(DS);
        AX = ES;
        DS = AX;
        CX = 0x8000;
        DX = DI;
        // Read File
        AH = 0x3F;
        Interrupt(0x21);
        DS = Stack.Pop16();
        while (CarryFlag) ;
        CX = AX;
        CarryFlag = false;
        Stack.Push16(FlagRegister16);
        // Close File
        AH = 0x3E;
        Interrupt(0x21);
        FlagRegister16 = Stack.Pop16();
    }
}