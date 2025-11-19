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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
        if (!ZeroFlag) {
            Memory.SetZeroTerminatedString(MemoryUtils.ToPhysicalAddress(DS,DI), ".HNM", 5);
        }
        // Open file handle (LOGO.HNM)
        Machine.Dos.DosInt21Handler.OpenFileorDevice(false);
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
        Machine.Dos.DosInt21Handler.QuitWithExitCode();;
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

    public void CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8()
    {
        int colors = CX;
        ushort colorOffset = DX;
        Thread.Sleep(1000/60);
        IVideoState videoState = Machine.VgaRegisters;
        videoState.DacRegisters.IndexRegisterWriteMode = BL;
        for (int i = 0; i < colors * 3; i++)
        {
            videoState.DacRegisters.DataRegister = UInt8[DS, (ushort)(colorOffset + i)];
        }
        
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public void ConvertLineNumberToArrayIndex_1000_0A22_10A22()
    {
        DI = (ushort)(BX * 320 + DX);
        
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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

    public void HNMUnknown_1000_0E46_10E46()
    {
        // CALL 0x1000:0d22 (1000_0E46 / 0x10E46)
        CirclesDrawStep_1000_0D22_10D22();
        // Function call generated as ASM continues to next function entry point without return
        CommonUnknown_1000_0E49_10E49();
    }

    public void CommonUnknown_1000_0E49_10E49()
    {
        // CALL 0x1000:0e59 (1000_0E49 / 0x10E49)
        CommonUnknown_display_1000_0E59_10E59();
        // Function call generated as ASM continues to next function entry point without return
        CommonUnknown_1000_0E4C_10E4C();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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

    public void UpdatePaletteDataAddress_1000_0E86_10E86()
    {
        SegmentedAddress pointer = PaletteDataAddress;
        ushort newSegmentOffset = (ushort)(UInt16[pointer] + pointer.Offset);
        ushort newSegment = (ushort)((newSegmentOffset >> 4) + pointer.Segment);
        ushort newOffset = (ushort)(newSegmentOffset & 0xF);
        WritePaletteDataAddress(newSegment, newOffset);
        CommonUnknown_1000_0EB2_10EB2(newSegment, newOffset);
    }

    public void CommonUnknown_1000_0EAD_10EAD()
    {
        CommonUnknown_1000_0EB2_10EB2(PaletteDataAddress.Segment, PaletteDataAddress.Offset);
    }

    public Action CommonUnknown_1000_0EB2_10EB2(ushort segment, ushort offset)
    {
        ushort value = UInt16[segment, offset];
        CX = (ushort)(value - 2);
        ZeroFlag = value == 0;
        return NearRet();
    }

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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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

    public void WritePaletteDataAddress(ushort segment, ushort offset)
    {
        UInt16[DS, 0x58] = segment;
        UInt16[DS, 0x56] = offset;
    }
    private SegmentedAddress PaletteDataAddress => new(UInt16[DS, 0x58], UInt16[DS, 0x56]);
    private SegmentedAddress PaletteDataAddressPlusTwo => new(PaletteDataAddress.Segment, (ushort)(PaletteDataAddress.Offset + 2));
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
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