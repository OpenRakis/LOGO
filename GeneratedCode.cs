using System.Diagnostics;

namespace logo;

/// <summary>
/// This is the resulting conversion by hand to high level code.
/// Work in progress.
/// </summary>
public partial class GeneratedOverrides : CSharpOverrideHelper
{
    /// <summary>
    /// Observed entry segment address at generation time
    /// </summary>
    private const ushort EntrySegmentAddress = 0x1000;

    public GeneratedOverrides(Dictionary<SegmentedAddress, FunctionInformation> functionInformations, Machine machine, ushort entrySegment = 0x1000) : base(functionInformations, machine)
    {
        //Do not set EntrySegmentAddress to something else if the program is not relocatable.

        DefineGeneratedCodeOverrides();
        DetectCodeRewrites();
        SetProvidedInterruptHandlersAsOverridden();
    }

    public Dictionary<SegmentedAddress, FunctionInformation> FunctionInformations => _functionInformations;

    public void DefineGeneratedCodeOverrides()
    {
        // 0x1000
        DefineFunction(EntrySegmentAddress, 0x0, EntryPoint_OpenLogoHnmFileAndRun_1000_0000_10000, false);
        DefineFunction(EntrySegmentAddress, 0x970, SetVideoMode_1000_0970_10970, false);
        DefineFunction(EntrySegmentAddress, 0x9B5, Nop_1000_09B5_109B5, false);
        DefineFunction(EntrySegmentAddress, 0x9D8, CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8, false);
        DefineFunction(EntrySegmentAddress, 0xA22, CommonUnknown_1000_0A22_10A22, false);
        DefineFunction(EntrySegmentAddress, 0xA3A, Nop_1000_0A3A_10A3A, false);
        DefineFunction(EntrySegmentAddress, 0xA51, Nop_1000_0A51_10A51, false);
        DefineFunction(EntrySegmentAddress, 0xB9A, CommonUnknown_1000_0B9A_10B9A, false);
        DefineFunction(EntrySegmentAddress, 0xC72, CirclesUnknown_display_1000_0C72_10C72, false);
        DefineFunction(EntrySegmentAddress, 0xCF4, CircleUunknown_1000_0CF4_10CF4, false);
        DefineFunction(EntrySegmentAddress, 0xD22, CommonChangeVgaPalette_1000_0D22_10D22, false);
        DefineFunction(EntrySegmentAddress, 0xD5F, CommonComputeNextVgaPalette_1000_0D5F_10D5F, false);
        DefineFunction(EntrySegmentAddress, 0xDBC, CirclesUnknown_1000_0DBC_10DBC, false);
        DefineFunction(EntrySegmentAddress, 0xDDE, CirclesAnimation_1000_0DDE_10DDE, false);
        DefineFunction(EntrySegmentAddress, 0xE46, HNMUnknown_1000_0E46_10E46, false);
        DefineFunction(EntrySegmentAddress, 0xE49, CommonUnknown_1000_0E49_10E49, false);
        DefineFunction(EntrySegmentAddress, 0xE4C, CommonUnknown_1000_0E4C_10E4C, false);
        DefineFunction(EntrySegmentAddress, 0xE59, CommonUnknown_display_1000_0E59_10E59, false);
        DefineFunction(EntrySegmentAddress, 0xE86, CommonUnknown_1000_0E86_10E86, false);
        DefineFunction(EntrySegmentAddress, 0xEAD, CommonUnknown_1000_0EAD_10EAD, false);
        DefineFunction(EntrySegmentAddress, 0xEBD, CommonUnknown_1000_0EBD_10EBD, false);
        DefineFunction(EntrySegmentAddress, 0xEFE, CommonUnknown_1000_0EFE_10EFE, false);
        DefineFunction(EntrySegmentAddress, 0xF30, CommonUnknownSplit_1000_0F30_10F30, false);
        DefineFunction(EntrySegmentAddress, 0xFA4, CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4, false);
        DefineFunction(EntrySegmentAddress, 0xFCC, nop_1000_0FCC_10FCC, false);
        DefineFunction(EntrySegmentAddress, 0xFEA, HNMUnknown_1000_0FEA_10FEA, false);
        DefineFunction(EntrySegmentAddress, 0x1019, CirclesUnknown_1000_1019_11019, false);
        DefineFunction(EntrySegmentAddress, 0x105F, CirclesUnknown_1000_105F_1105F, false);
        DefineFunction(EntrySegmentAddress, 0x1085, CommonCheckForAnyKeyStroke_1000_1085_11085, false);
        DefineFunction(EntrySegmentAddress, 0x109A, HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A, false);
        DefineFunction(EntrySegmentAddress, 0x10F4, Nop_1000_10F4_110F4, false);
        DefineFunction(EntrySegmentAddress, 0x11BD, Nop_1000_11BD_111BD, false);
    }

    public void DetectCodeRewrites()
    {
        DefineExecutableArea(0x10000, 0x10090);
        DefineExecutableArea(0x10970, 0x10CAF);
        DefineExecutableArea(0x10CF4, 0x10ED4);
        DefineExecutableArea(0x10EFE, 0x10F07);
        DefineExecutableArea(0x10F30, 0x1103A);
        DefineExecutableArea(0x1105F, 0x1106E);
        DefineExecutableArea(0x11085, 0x11091);
        DefineExecutableArea(0x1109A, 0x110C1);
        DefineExecutableArea(0x110F4, 0x111C7);
        DefineExecutableArea(0xF0008, 0xF000B);
        DefineExecutableArea(0xF0014, 0xF0017);
        DefineExecutableArea(0xF0020, 0xF0023);
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action EntryPoint_OpenLogoHnmFileAndRun_1000_0000_10000(int loadOffset)
    {
        AX = 0x111C;
        DS = AX;
        AX = UInt16[ES, 2];
        UInt16[DS, 0x50] = AX;
        BX = 8;
        Nop_1000_10F4_110F4(0);
        AX = 0xC;
        SI = 0x8E;
        Alu.Sub8(UInt8[DS, SI], 0);
        UInt16[DS, 0x52] = AX;
        DX = 0x5A;
        CirclesUnknown_1000_105F_1105F(0);
        Alu.Sub8(UInt8[DS, DI], 46);
        if (!ZeroFlag)
        {
            UInt16[DS, DI] = 0x482E;
            UInt16[DS, (ushort)(DI + 2U)] = 0x4D4E;
            UInt8[DS, (ushort)(DI + 4U)] = 0;
        }
        // Open file handle (LOGO.HNM)
        AX = 0x3D00;
        Interrupt(0x21);
        DX = 0x2E;
        if (!CarryFlag)
        {
            Stack.Push16(AX);
            SetVideoMode_1000_0970_10970(0);
            CirclesUnknown_1000_1019_11019(0);
            BX = Stack.Pop16();
            AX = UInt16[DS, 0x52];
            CirclesAnimation_1000_0DDE_10DDE(0);
            Nop_1000_0A51_10A51(0);
            DX = 0;
            AL = 0;
            // End of LOGO.EXE
            // Quit With Exit Code (Cpu.IsRunning set to false)
            AH = 0x4C;
            Interrupt(0x21);
        }
        // End of LOGO.EXE
        // Print string: "\u0006<zw\u0002"
        AH = 0x9;
        Interrupt(0x21);
        // End of LOGO.EXE
        // Quit to DOS
        AL = 0;
        AH = 0x4C;
        Interrupt(0x21);
        CheckExternalEvents(EntrySegmentAddress, 0x6D);
        throw new NotImplementedException("You should not be here...");
    }

    /// <summary>
    /// Sets Video mode VGA to 0x13
    /// </summary>
    public virtual Action SetVideoMode_1000_0970_10970(int loadOffset)
    {
        Machine.VgaCard.SetVideoModeValue(0x13);
        return NearRet();
    }

    public virtual Action Nop_1000_09B5_109B5(int loadOffset)
    {
        return NearRet();
    }

    public virtual Action CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8(int loadOffset)
    {
        int colors = CX;
        ushort colorOffset = DX;
        Machine.VgaCard.UpdateScreen();
        Thread.Sleep(15);
        Machine.VgaCard.SetVgaWriteIndex(BL);
        for (int i = 0; i < colors * 3; i++)
        {
            CheckExternalEvents(EntrySegmentAddress, 0x09D9);
            Machine.VgaCard.RgbDataWrite(UInt8[DS, (ushort)(colorOffset + i)]);
        }
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CommonUnknown_1000_0A22_10A22(int loadOffset)
    {
        // CMP BX,0xc8 (1000_0A22 / 0x10A22)
        Alu.Sub16(BX, 0xC8);
        // XCHG BL,BH (1000_0A2B / 0x10A2B)
        (BH, BL) = (BL, BH);
        // MOV DI,BX (1000_0A2D / 0x10A2D)
        DI = BX;
        // SHR DI,0x1 (1000_0A2F / 0x10A2F)
        DI >>= 0x1;
        // SHR DI,0x1 (1000_0A31 / 0x10A31)
        DI >>= 0x1;
        // ADD DI,BX (1000_0A33 / 0x10A33)
        // DI += BX;
        DI = Alu.Add16(DI, BX);
        // ADD DI,DX (1000_0A37 / 0x10A37)
        // DI += DX;
        DI = Alu.Add16(DI, DX);
        // RET  (1000_0A39 / 0x10A39)
        return NearRet();
    }

    public virtual Action Nop_1000_0A3A_10A3A(int loadOffset)
    {
        return NearRet();
    }

    public virtual Action Nop_1000_0A51_10A51(int loadOffset)
    {
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CommonUnknown_1000_0B9A_10B9A(int loadOffset)
    {
        Alu.Sub8(CH, 0xFE);
        DI = Alu.Or16(DI, DI);
        if (!SignFlag)
        {
            BP = DI;
            BP = Alu.And16(BP, 511);
            AX = DI;
            CommonUnknown_1000_0A22_10A22(0);
            BX = CX;
            BH = 0;
            Alu.Sub8(CH, byte.MaxValue);
            if (!ZeroFlag)
            {
                BP = Alu.Shr16(BP, 1);
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
                        BX = Alu.Dec16(BX);
                    }
                    while (!ZeroFlag);
                    return FarRet();
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
                    AL = Alu.Or8(AL, AL);
                    if (!ZeroFlag)
                    {
                        UInt8[ES, DI] = AL;
                        DI += (ushort)(uint)Direction8;
                        if (--CX == 0)
                        {
                            DX += 320;
                            BX = Alu.Dec16(BX);
                            if (ZeroFlag)
                                return FarRet();
                            goto label_76;
                        }
                    }
                    else
                    {
                        DI = Alu.Inc16(DI);
                        num3 = --CX;
                    }
                }
                while (num3 != 0);
                DX += 320;
                BX = Alu.Dec16(BX);
            }
            while (!ZeroFlag);
            return FarRet();
        }
        BP = DI;
        BP = Alu.And16(BP, 511);
        AX = DI;
        CommonUnknown_1000_0A22_10A22(0);
        BX = CX;
        BH = 0;
        Alu.And16(AX, 16384);
    label_3:
        do
        {
            AL = UInt8[DS, SI];
            SI += (ushort)(uint)Direction8;
            AL = Alu.Or8(AL, AL);
            if (!SignFlag)
            {
                CX = AX;
                CH = 0;
                ++CX;
                BP = Alu.Sub16(BP, CX);
                while (true)
                {
                    AL = UInt8[DS, SI];
                    SI += (ushort)(uint)Direction8;
                    AL = Alu.Or8(AL, AL);
                    if (!ZeroFlag)
                    {
                        UInt8[ES, DI] = AL;
                        DI += (ushort)(uint)Direction8;
                        if (--CX == 0)
                        {
                            BP = Alu.Or16(BP, BP);
                            if (ZeroFlag)
                            {
                                BX = Alu.Dec16(BX);
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
                BP = Alu.Sub16(BP, CX);
                AL = UInt8[DS, SI];
                SI += (ushort)(uint)Direction8;
                AL = Alu.Or8(AL, AL);
                goto label_18;
            }
        label_18:
            DI = Alu.Add16(DI, CX);
            BP = Alu.Or16(BP, BP);
        }
        while (!CarryFlag && !ZeroFlag);
        BX = Alu.Dec16(BX);
        UInt8[EntrySegmentAddress, 0xA71] = 0xC7;
        UInt8[EntrySegmentAddress, 0xB2F] = 0xC7;
        return FarRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CirclesUnknown_display_1000_0C72_10C72(int loadOffset)
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
                DI = Alu.Sub16(DI, 2);
                UInt16[DS, DI] = AX;
                num = --CX;
            }
            while (num != 0);
            SI += 0xA0;
            DI += 0x1E0;
            DX = Alu.Dec16(DX);
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
            DX = Alu.Dec16(DX);
        }
        while (!ZeroFlag);
        DS = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CircleUunknown_1000_0CF4_10CF4(int loadOffset)
    {
        CirclesUnknown_display_1000_0C72_10C72(0);
        SI = 0xCBC;
        AX = UInt16[EntrySegmentAddress, SI];
        UInt16[EntrySegmentAddress, 0xCB6] = AX;
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
            CommonChangeVgaPalette_1000_0D22_10D22(0);
            CX = Stack.Pop16();
            CommonCheckForAnyKeyStroke_1000_1085_11085(0);
            num = --CX;
        }
        while (num != 0 && ZeroFlag);
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CommonChangeVgaPalette_1000_0D22_10D22(int loadOffset)
    {
        Stack.Push16(DS);
        Stack.Push16(ES);
        Stack.Push16(EntrySegmentAddress);
        Stack.Push16(EntrySegmentAddress);
        DS = Stack.Pop16();
        ES = Stack.Pop16();
        Alu.Sub16(UInt16[EntrySegmentAddress, 0xCB6], 0);
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
            CX = Alu.Sub16(CX, AX);
            while (CX != 0)
            {
                CX--;
                UInt8[ES, DI] = UInt8[DS, SI];
                SI += (ushort)Direction8;
                DI += (ushort)Direction8;
            }
            CX = DX;
            CommonComputeNextVgaPalette_1000_0D5F_10D5F(0);
            DX = 0x160;
            BX = 0x50;
            CX = 0x50;
            CommonCirclesWaitFrameAndWriteNextPaletteData_1000_09D8_109D8(0);
        }
        ES = Stack.Pop16();
        DS = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CommonComputeNextVgaPalette_1000_0D5F_10D5F(int loadOffset)
    {
        SI = UInt16[EntrySegmentAddress, 0xCBA];
        UInt16[EntrySegmentAddress, 0xCB6] = Alu.Dec16(UInt16[EntrySegmentAddress, 0xCB6]);
        if (ZeroFlag)
        {
            SI = Alu.Add16(SI, 8);
            UInt16[EntrySegmentAddress, 0xCBA] = SI;
            AX = UInt16[DS, SI];
            UInt16[EntrySegmentAddress, 0xCB6] = AX;
        }
        AX = UInt16[DS, (ushort)(SI + 2U)];
        AX = Alu.Add16(AX, UInt16[EntrySegmentAddress, 0xCB0]);
        UInt16[EntrySegmentAddress, 0xCB0] = AX;
        AL = Alu.Shl8(AL, 1);
        AH = Alu.Adc8(AH, 0);
        AL = AH;
        AL = Alu.And8(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        AX = UInt16[DS, (ushort)(SI + 0x4)];
        AX = Alu.Add16(AX, UInt16[EntrySegmentAddress, 0xCB2]);
        UInt16[EntrySegmentAddress, 0xCB2] = AX;
        AL = Alu.Shl8(AL, 1);
        AH = Alu.Adc8(AH, 0);
        AL = AH;
        AL = Alu.And8(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        AX = UInt16[DS, (ushort)(SI + 0x6)];
        AX = Alu.Add16(AX, UInt16[EntrySegmentAddress, 0xCB4]);
        UInt16[EntrySegmentAddress, 0xCB4] = AX;
        AL = Alu.Shl8(AL, 1);
        AH = Alu.Adc8(AH, 0);
        AL = AH;
        AL = Alu.And8(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        return NearRet();
    }

    public virtual Action CirclesUnknown_1000_0DBC_10DBC(int loadOffset)
    {
        // LES DI,[0x4c] (1000_0DBC / 0x10DBC)
        DI = UInt16[DS, 0x4C];
        ES = UInt16[DS, 0x4E];
        // MOV word ptr [0x56],DI (1000_0DC0 / 0x10DC0)
        UInt16[DS, 0x56] = DI;
        // MOV word ptr [0x58],ES (1000_0DC4 / 0x10DC4)
        UInt16[DS, 0x58] = ES;
        // MOV CX,ES (1000_0DC8 / 0x10DC8)
        CX = ES;
        // MOV AX,[0x50] (1000_0DCA / 0x10DCA)
        AX = UInt16[DS, 0x50];
        // SUB AX,CX (1000_0DCD / 0x10DCD)
        AX -= CX;
        // CMP AX,0x3a02 (1000_0DCF / 0x10DCF)
        Alu.Sub16(AX, 0x3A02);
        // CALL 0x1000:109a (1000_0DD4 / 0x10DD4)
        HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A(0);
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
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CirclesAnimation_1000_0DDE_10DDE(int loadOffset)
    {
        // Read File
        UInt16[DS, 0x54] = AX;
        CirclesUnknown_1000_0DBC_10DBC(0);
        Nop_1000_0A3A_10A3A(0);
        AX = UInt16[DS, 84];
        UInt16[DS, 0x52] = AX;
        DI = UInt16[DS, 76];
        ES = UInt16[DS, 78];
        UInt16[DS, 0x56] = DI;
        UInt16[DS, 0x58] = ES;
        CommonUnknown_1000_0EAD_10EAD(0);
        CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4(0);
        CommonUnknown_1000_0E4C_10E4C(0);
        CarryFlag = true;
        CommonUnknown_1000_0E49_10E49(0);
        CircleUunknown_1000_0CF4_10CF4(0);
        CarryFlag = true;

        do
        {
            AX = UInt16[DS, 0x52];
            BP = 0xE46;
            HNMUnknown_1000_0FEA_10FEA(0);
            CarryFlag = true;
            Alu.Sub16(UInt16[DS, 82], 0);
        }
        while (!ZeroFlag);
        SI = 0xEE;
        AX = UInt16[DS, SI];
        SI += (ushort)Direction16;
        Nop_1000_11BD_111BD(0);
        byte ah1 = AH;
        byte al1 = AL;
        AL = ah1;
        AH = al1;
        Nop_1000_11BD_111BD(0);
        Alu.Sub16(AX, 0x4C4F);
        CarryFlag = false;
        return NearRet();
    }

    public virtual Action HNMUnknown_1000_0E46_10E46(int loadOffset)
    {
        // CALL 0x1000:0d22 (1000_0E46 / 0x10E46)
        CommonChangeVgaPalette_1000_0D22_10D22(0);
        // Function call generated as ASM continues to next function entry point without return
        return CommonUnknown_1000_0E49_10E49(0);
    }

    public virtual Action CommonUnknown_1000_0E49_10E49(int loadOffset)
    {
        // CALL 0x1000:0e59 (1000_0E49 / 0x10E49)
        CommonUnknown_display_1000_0E59_10E59(0);
        // Function call generated as ASM continues to next function entry point without return
        return CommonUnknown_1000_0E4C_10E4C(0);
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CommonUnknown_1000_0E4C_10E4C(int loadOffset)
    {
        CommonUnknown_1000_0E86_10E86(0);
        if (!ZeroFlag)
        {
            return NearRet();
        }
        UInt16[DS, 0x52] = 0;
        return NearRet();
    }

    /// <summary>
    /// TODO: High level rewrite this first.
    /// </summary>
    public virtual Action CommonUnknown_display_1000_0E59_10E59(int loadOffset)
    {
        Stack.Push16(DS);
        SI = UInt16[DS, 0x56];
        DS = UInt16[DS, 0x58];
        SI = Alu.Add16(SI, 2);
        AX = UInt16[DS, SI];
        SI += (ushort)Direction16;
        DI = AX;
        AX = UInt16[DS, SI];
        SI += (ushort)Direction16;
        CX = AX;
        CL = Alu.Or8(CL, CL);
        if (!ZeroFlag)
        {
            Alu.And16(DI, 0x200);
            if (!ZeroFlag)
            {
                CommonUnknown_1000_0EBD_10EBD(0);
            }
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            DX = AX;
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            BX = AX;
            AX = 0xA000;
            ES = AX;
            CommonUnknown_1000_0B9A_10B9A(0);
        }
        DS = Stack.Pop16();
        return NearRet();
    }

    public virtual Action CommonUnknown_1000_0E86_10E86(int loadOffset)
    {
        SegmentedAddress pointer = new SegmentedAddress(UInt16[DS, 0x58], UInt16[DS, 0x56]);
        ushort newSegmentOffset = (ushort)(UInt16[pointer.ToPhysical()] + pointer.Offset);
        ushort newSegment = (ushort)((newSegmentOffset >> 4) + pointer.Segment);
        ushort newOffset = (ushort)(newSegmentOffset & 0xF);
        UInt16[DS, 0x56] = newOffset;
        UInt16[DS, 0x58] = newSegment;
        return CommonUnknown_1000_0EB2_10EB2(newSegment, newOffset);
    }

    public virtual Action CommonUnknown_1000_0EAD_10EAD(int loadOffset)
    {
        return CommonUnknown_1000_0EB2_10EB2(UInt16[DS, 0x58], UInt16[DS, 0x56]);
    }

    public Action CommonUnknown_1000_0EB2_10EB2(ushort segment, ushort offset)
    {
        ushort value = UInt16[segment, offset];
        CX = (ushort)(value - 2);
        ZeroFlag = value == 0;
        return NearRet(0);
    }

    public virtual Action CommonUnknown_1000_0EBD_10EBD(int loadOffset)
    {
        // AND DI,0xfdff (1000_0EBD / 0x10EBD)
        // DI &= 0xFDFF;
        DI = Alu.And16(DI, 0xFDFF);
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
        CommonUnknown_1000_0EFE_10EFE(0);
        // POP DS (1000_0ED0 / 0x10ED0)
        DS = Stack.Pop16();
        // POP SI (1000_0ED1 / 0x10ED1)
        SI = Stack.Pop16();
        // POP DI (1000_0ED2 / 0x10ED2)
        DI = Stack.Pop16();
        // POP CX (1000_0ED3 / 0x10ED3)
        CX = Stack.Pop16();
        // RET  (1000_0ED4 / 0x10ED4)
        return NearRet();
    }

    public virtual Action CommonUnknown_1000_0EFE_10EFE(int loadOffset)
    {
        Stack.Push16(CX);
        Stack.Push16(DI);
        Stack.Push16(DS);
        SI += 6;
        BP = 0;
        JumpDispatcher?.Jump(CommonUnknownSplit_1000_0F30_10F30, 0);
        return JumpDispatcher?.JumpAsmReturn;
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CommonUnknownSplit_1000_0F30_10F30(int loadOffset)
    {
        while (true)
        {
            BP = Alu.Shr16(BP, 1);
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
            BP = Alu.Rcr16(BP, 1);
            if (CarryFlag)
            {
                goto label_2;
            }

        label_4:
            CX = 0;
            BP = Alu.Shr16(BP, 1);
            if (ZeroFlag)
            {
                AX = UInt16[DS, SI];
                SI += (ushort)Direction16;
                BP = AX;
                CarryFlag = true;
                BP = Alu.Rcr16(BP, 1);
            }
            if (!CarryFlag)
            {
                BP = Alu.Shr16(BP, 1);
                if (ZeroFlag)
                {
                    AX = UInt16[DS, SI];
                    SI += (ushort)Direction16;
                    BP = AX;
                    CarryFlag = true;
                    BP = Alu.Rcr16(BP, 1);
                }
                CX = Alu.Rcl16(CX, 1);
                BP = Alu.Shr16(BP, 1);
                if (ZeroFlag)
                {
                    AX = UInt16[DS, SI];
                    SI += (ushort)Direction16;
                    BP = AX;
                    CarryFlag = true;
                    BP = Alu.Rcr16(BP, 1);
                }
                CX = Alu.Rcl16(CX, 1);
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
                AX = Alu.Add16(AX, DI);
                (SI, AX) = (AX, SI);
                BX = DS;
                DX = ES;
                DS = DX;
                ++CX;
                CX = Alu.Inc16(CX);
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
            AX = Alu.Shr16(AX, 1);
            AH |= 0xE0;
            CL = Alu.And8(CL, 7);
            if (ZeroFlag)
            {
                BX = AX;
                AL = UInt8[DS, SI];
                SI += (ushort)Direction8;
                CL = AL;
                AX = BX;
                CL = Alu.Or8(CL, CL);
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
        CX = Alu.Sub16(CX, DI);
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action CirclesChangeVgaPaletteLoop_1000_0FA4_10FA4(int loadOffset)
    {
        SI = UInt16[DS, 0x56];
        ES = UInt16[DS, 0x58];
        SI = Alu.Add16(SI, 2);
        while (true)
        {
            AX = UInt16[ES, SI];
            SI += (ushort)Direction16;
            Alu.Sub8(AL, byte.MaxValue);
            if (!ZeroFlag)
            {
                CX = 0;
                BX = 0;
                BL = AL;
                CL = AH;
                DX = SI;
                SI += CX;
                SI += CX;
                SI = Alu.Add16(SI, CX);
                // Set VGA Palette
                AX = 0x1012;
                Interrupt(0x10);
                nop_1000_0FCC_10FCC(0);
            }
            else
            {
                break;
            }
        }
        return NearRet();
    }

    public virtual Action nop_1000_0FCC_10FCC(int loadOffset)
    {
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action HNMUnknown_1000_0FEA_10FEA(int loadOffset)
    {
        InterruptFlag = true;
        Stack.Push16(AX);
        AX = 0;
        ES = AX;
        Stack.Push16(UInt16[ES, 0x46C]);
        HNMUnknown_1000_0E46_10E46(0);
        BX = Stack.Pop16();
        BP = Stack.Pop16();
        BP >>= 1;
        BP >>= 1;
        BP = Alu.Shr16(BP, 1);
        AX = BP;
        AX >>= 1;
        AX >>= 1;
        BP -= AX;
        //Seems to be a delay loop
        do
        {
            AX = 0;
            ES = AX;
            CheckExternalEvents(EntrySegmentAddress, 0x100F);
            AX = UInt16[ES, 0x46C];
            AX -= BX;
            Alu.Sub16(AX, BP);
        }
        while (CarryFlag);
        CommonCheckForAnyKeyStroke_1000_1085_11085(0);
        return NearRet();
    }

    public virtual Action CirclesUnknown_1000_1019_11019(int loadOffset)
    {
        DI = DX;
        while (true)
        {
            AL = UInt8[DS, DI];
            if (!ZeroFlag)
            {
                AL = Alu.Or8(AL, AL);
                return NearRet();
            }
        }
    }

    public virtual Action CirclesUnknown_1000_105F_1105F(int loadOffset)
    {
        // MOV DI,DX (1000_105F / 0x1105F)
        DI = DX;
        while (true)
        {
            AL = UInt8[DS, DI];
            // CMP AL,0x2e (1000_1063 / 0x11063)
            Alu.Sub8(AL, 0x2E);
            // OR AL,AL (1000_1067 / 0x11067)
            // AL |= AL;
            AL = Alu.Or8(AL, AL);
            // JZ 0x1000:106e (1000_1069 / 0x11069)
            if (ZeroFlag)
            {
                // JZ target is RET, inlining.
                // RET  (1000_106E / 0x1106E)
                return NearRet();
            }
            // INC DI (1000_106B / 0x1106B)
            DI = Alu.Inc16(DI);
            // JMP 0x1000:1061 (1000_106C / 0x1106C)
        }
    }

    /// <summary>
    /// Checks 288 times during the whole program runtime if any key from the keyboard was received.
    /// If any, exit to DOS immediatly.
    /// </summary>
    public virtual Action CommonCheckForAnyKeyStroke_1000_1085_11085(int loadOffset)
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
            return NearRet();
        }
        // Bios Interrupt: GetKeystroke (aka Read Key)
        // XOR AH,AH (1000_108B / 0x1108B)
        AH = 0;
        // INT 0x16 (1000_108D / 0x1108D)
        Interrupt(0x16);
        // RET  (1000_1091 / 0x11091)
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action HNMReadFile_AdvancePointer_CloseFile_1000_109A_1109A(int loadOffset)
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
        return NearRet();
    }

    public virtual Action Nop_1000_10F4_110F4(int loadOffset)
    {
        return NearRet();
    }

    public virtual Action Nop_1000_11BD_111BD(int loadOffset)
    {
        return NearRet();
    }
}