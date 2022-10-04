﻿namespace logo;

public partial class GeneratedOverrides : CSharpOverrideHelper
{
    protected ushort cs1; // 0x1000
    protected ushort cs2; // 0xF000

    public GeneratedOverrides(Dictionary<SegmentedAddress, FunctionInformation> functionInformations, Machine machine, ushort entrySegment = 0x1000) : base(functionInformations, machine)
    {
        // Observed cs1 address at generation time is 0x1000. Do not set entrySegment to something else if the program is not relocatable.
        this.cs1 = (ushort)(entrySegment + 0x0);
        this.cs2 = (ushort)(entrySegment + 0xE000);

        DefineGeneratedCodeOverrides();
        DetectCodeRewrites();
        SetProvidedInterruptHandlersAsOverridden();
    }

    public Dictionary<SegmentedAddress, FunctionInformation> FunctionInformations => _functionInformations;

    public void DefineGeneratedCodeOverrides()
    {
        // 0x1000
        DefineFunction(cs1, 0x0, entry_1000_0000_10000, false);
        DefineFunction(cs1, 0x970, unknown_1000_0970_10970, false);
        DefineFunction(cs1, 0x9B5, unknown_1000_09B5_109B5, false);
        DefineFunction(cs1, 0x9D8, unknown_1000_09D8_109D8, false);
        DefineFunction(cs1, 0xA22, unknown_1000_0A22_10A22, false);
        DefineFunction(cs1, 0xA3A, unknown_1000_0A3A_10A3A, false);
        DefineFunction(cs1, 0xA51, unknown_1000_0A51_10A51, false);
        DefineFunction(cs1, 0xB9A, unknown_1000_0B9A_10B9A, false);
        DefineFunction(cs1, 0xC72, unknown_1000_0C72_10C72, false);
        DefineFunction(cs1, 0xCF4, unknown_1000_0CF4_10CF4, false);
        DefineFunction(cs1, 0xD22, unknown_1000_0D22_10D22, false);
        DefineFunction(cs1, 0xD5F, unknown_1000_0D5F_10D5F, false);
        DefineFunction(cs1, 0xDBC, unknown_1000_0DBC_10DBC, false);
        DefineFunction(cs1, 0xDDE, unknown_1000_0DDE_10DDE, false);
        DefineFunction(cs1, 0xE46, unknown_1000_0E46_10E46, false);
        DefineFunction(cs1, 0xE49, unknown_1000_0E49_10E49, false);
        DefineFunction(cs1, 0xE4C, unknown_1000_0E4C_10E4C, false);
        DefineFunction(cs1, 0xE59, unknown_1000_0E59_10E59, false);
        DefineFunction(cs1, 0xE86, unknown_1000_0E86_10E86, false);
        DefineFunction(cs1, 0xEAD, unknown_1000_0EAD_10EAD, false);
        DefineFunction(cs1, 0xEB2, split_1000_0EB2_10EB2, false);
        DefineFunction(cs1, 0xEBD, unknown_1000_0EBD_10EBD, false);
        DefineFunction(cs1, 0xEFE, unknown_1000_0EFE_10EFE, false);
        DefineFunction(cs1, 0xF30, split_1000_0F30_10F30, false);
        DefineFunction(cs1, 0xFA4, unknown_1000_0FA4_10FA4, false);
        DefineFunction(cs1, 0xFCC, unknown_1000_0FCC_10FCC, false);
        DefineFunction(cs1, 0xFEA, unknown_1000_0FEA_10FEA, false);
        DefineFunction(cs1, 0x1019, unknown_1000_1019_11019, false);
        DefineFunction(cs1, 0x105F, unknown_1000_105F_1105F, false);
        DefineFunction(cs1, 0x1085, unknown_1000_1085_11085, false);
        DefineFunction(cs1, 0x109A, unknown_1000_109A_1109A, false);
        DefineFunction(cs1, 0x10F4, unknown_1000_10F4_110F4, false);
        DefineFunction(cs1, 0x11BD, unknown_1000_11BD_111BD, false);
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
    public virtual Action entry_1000_0000_10000(int loadOffset)
    {
        AX = 0x111C;
        DS = AX;
        AX = UInt16[ES, 2];
        UInt16[DS, 0x50] = AX;
        BX = 8;
        NearCall(cs1, 0x12, new Func<int, Action>(unknown_1000_10F4_110F4));
        AX = 0xC;
        SI = 0x8E;
        int num1 = Alu.Sub8(UInt8[DS, SI], 0);
        int num2 = ZeroFlag ? 1 : 0;
        UInt16[DS, 0x52] = AX;
        DX = 0x5A;
        NearCall(cs1, 0x2C, new Func<int, Action>(unknown_1000_105F_1105F));
        int num3 = Alu.Sub8(UInt8[DS, DI], 46);
        if (!ZeroFlag)
        {
            UInt16[DS, DI] = 0x482E;
            UInt16[DS, (ushort)(DI + 2U)] = 0x4D4E;
            UInt8[DS, (ushort)(DI + 4U)] = 0;
        }
        AX = 0x3D00;
        Interrupt(0x21);
        DX = 0x2E;
        if (!CarryFlag)
        {
            Stack.Push16(AX);
            NearCall(cs1, 0x4C, new Func<int, Action>(unknown_1000_0970_10970));
            NearCall(cs1, 0x4F, new Func<int, Action>(unknown_1000_1019_11019));
            BX = Stack.Pop16();
            AX = UInt16[DS, 0x52];
            NearCall(cs1, 0x56, new Func<int, Action>(unknown_1000_0DDE_10DDE));
            NearCall(cs1, 0x59, new Func<int, Action>(unknown_1000_0A51_10A51));
            DX = 0;
            AL = 0;
            AH = 0x4C;
            Interrupt(0x21);
        }
        AH = 0x9;
        Interrupt(0x21);
        AL = 0;
        AH = 0x4C;
        Interrupt(0x21);
        CheckExternalEvents(cs1, 0x6D);
        if (OverflowFlag)
            throw FailAsUntested("Would have been a goto but label label_1000_0047_10047 does not exist because no instruction was found there that belongs to a function.");
        DI += DI;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] += AL;
        UInt8[DS, (ushort)(BX + SI)] = Alu.Add8(UInt8[DS, (ushort)(BX + SI)], AL);
        throw FailAsUntested("Function does not end with return and no instruction after the body ...");
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0970_10970(int loadOffset)
    {
        AX = 0x13;
        Interrupt(0x10);
        Stack.Push16(FlagRegister16);
        InterruptFlag = true;
        AX = 0x40;
        ES = AX;
        DX = UInt16[ES, 0x63];
        DL = Alu.Add8(DL, 6);
        UInt16[cs1, 0x6C] = DX;
        BP = 0x6C;
        CheckExternalEvents(cs1, 0x98C);
        AL = Cpu.In8(DX);
        AL = Alu.And8(AL, 8);
        NearCall(cs1, 0x992, new Func<int, Action>(unknown_1000_09B5_109B5));
        if (CarryFlag)
        {
            NearCall(cs1, 0x997, new Func<int, Action>(unknown_1000_09B5_109B5));
            if (CarryFlag)
            {
                DI = SI;
                UInt8[cs1, 0x6F] = AH;
                NearCall(cs1, 0x9A3, new Func<int, Action>(unknown_1000_09B5_109B5));
                if (CarryFlag)
                {
                    int num = Alu.Sub16(SI, DI);
                    UInt8[cs1, 0x6E] = (byte)~UInt8[cs1, 0x6E];
                    if (CarryFlag)
                    {
                        UInt8[cs1, 0x6F] = AH;
                    }
                }
            }
        }
        FlagRegister16 = Stack.Pop16();
        return NearRet();
    }

    public virtual Action unknown_1000_09B5_109B5(int loadOffset)
    {
        // MOV AH,AL (1000_09B5 / 0x109B5)
        AH = AL;
        // XOR SI,SI (1000_09B7 / 0x109B7)
        SI = 0;
        // MOV BX,word ptr ES:[BP + 0x0] (1000_09B9 / 0x109B9)
        BX = UInt16[ES, BP];
    label_1000_09BD_109BD:
        // INC SI (1000_09BD / 0x109BD)
        SI = Alu.Inc16(SI);
        // JNZ 0x1000:09c1 (1000_09BE / 0x109BE)
        if (!ZeroFlag)
        {
            goto label_1000_09C1_109C1;
        }
        // DEC SI (1000_09C0 / 0x109C0)
        SI = Alu.Dec16(SI);
    label_1000_09C1_109C1:
        // IN AL,DX (1000_09C1 / 0x109C1)
        CheckExternalEvents(cs1, 0x9c2);
        AL = Cpu.In8(DX);
        // AND AL,0x8 (1000_09C2 / 0x109C2)
        AL &= 0x8;
        // CMP AL,AH (1000_09C4 / 0x109C4)
        Alu.Sub8(AL, AH);
        // JNZ 0x1000:09d6 (1000_09C6 / 0x109C6)
        if (!ZeroFlag)
        {
            goto label_1000_09D6_109D6;
        }
        // PUSH AX (1000_09C8 / 0x109C8)
        Stack.Push16(AX);
        // MOV AX,word ptr ES:[BP + 0x0] (1000_09C9 / 0x109C9)
        AX = UInt16[ES, BP];
        // SUB AX,BX (1000_09CD / 0x109CD)
        AX -= BX;
        // CMP AX,0x64 (1000_09CF / 0x109CF)
        Alu.Sub16(AX, 0x64);
        // POP AX (1000_09D2 / 0x109D2)
        AX = Stack.Pop16();
        // JC 0x1000:09bd (1000_09D3 / 0x109D3)
        if (CarryFlag)
        {
            goto label_1000_09BD_109BD;
        }
        // RET  (1000_09D5 / 0x109D5)
        return NearRet();
    label_1000_09D6_109D6:
        // STC  (1000_09D6 / 0x109D6)
        CarryFlag = true;
        // RET  (1000_09D7 / 0x109D7)
        return NearRet();
    }

    public virtual Action unknown_1000_09D8_109D8(int loadOffset)
    {
        // PUSH SI (1000_09D8 / 0x109D8)
        Stack.Push16(SI);
        // PUSH DS (1000_09D9 / 0x109D9)
        Stack.Push16(DS);
        // PUSH ES (1000_09DA / 0x109DA)
        Stack.Push16(ES);
        // POP DS (1000_09DB / 0x109DB)
        DS = Stack.Pop16();
        // MOV SI,DX (1000_09DC / 0x109DC)
        SI = DX;
        // PUSHF  (1000_09DE / 0x109DE)
        Stack.Push16(FlagRegister16);
        // CMP byte ptr CS:[0x6e],0x0 (1000_09DF / 0x109DF)
        Alu.Sub8(UInt8[cs1, 0x6E], 0x0);
        // JZ 0x1000:09f6 (1000_09E5 / 0x109E5)
        if (ZeroFlag)
        {
            goto label_1000_09F6_109F6;
        }
        // MOV DX,word ptr CS:[0x6c] (1000_09E7 / 0x109E7)
        DX = UInt16[cs1, 0x6C];
    label_1000_09EC_109EC:
        CheckExternalEvents(cs1, 0x09EC);
        // IN AL,DX (1000_09EC / 0x109EC)
        AL = Cpu.In8(DX);
        // AND AL,0x8 (1000_09ED / 0x109ED)
        AL &= 0x8;
        // CMP AL,byte ptr CS:[0x6f] (1000_09EF / 0x109EF)
        Alu.Sub8(AL, UInt8[cs1, 0x6F]);
        // JNZ 0x1000:09ec (1000_09F4 / 0x109F4)
        if (!ZeroFlag)
        {
            goto label_1000_09EC_109EC;
        }
    label_1000_09F6_109F6:
        // CLI  (1000_09F6 / 0x109F6)
        InterruptFlag = false;
        // MOV DX,0x3c8 (1000_09F7 / 0x109F7)
        DX = 0x3C8;
        // MOV AL,BL (1000_09FA / 0x109FA)
        AL = BL;
        // OUT DX,AL (1000_09FC / 0x109FC)
        Cpu.Out8(DX, AL);
        // JMP 0x1000:09ff (1000_09FD / 0x109FD)
        // JMP target is JMP, inlining.
        // JMP 0x1000:0a01 (1000_09FF / 0x109FF)
        // JMP target is JMP, inlining.
        // JMP 0x1000:0a03 (1000_0A01 / 0x10A01)
        // JMP target is JMP, inlining.
        // JMP 0x1000:0a05 (1000_0A03 / 0x10A03)
        goto label_1000_0A05_10A05;
    label_1000_0A05_10A05:
        // INC DX (1000_0A05 / 0x10A05)
        DX = Alu.Inc16(DX);
        // MOV AX,CX (1000_0A06 / 0x10A06)
        AX = CX;
        // ADD CX,CX (1000_0A08 / 0x10A08)
        CX += CX;
        // ADD CX,AX (1000_0A0A / 0x10A0A)
        CX += AX;
        // CMP byte ptr CS:[0x6b],0x0 (1000_0A0C / 0x10A0C)
        Alu.Sub8(UInt8[cs1, 0x6B], 0x0);
        // JZ 0x1000:0a1a (1000_0A12 / 0x10A12)
        if (ZeroFlag)
        {
            goto label_1000_0A1A_10A1A;
        }
        // REP
        while (CX != 0)
        {
            CX--;
            // OUTSB DX,SI (1000_0A14 / 0x10A14)
            Cpu.Out8(DX, UInt8[DS, SI]);
            SI = (ushort)(SI + Direction8);
        }
        // POPF  (1000_0A16 / 0x10A16)
        FlagRegister16 = Stack.Pop16();
        // POP DS (1000_0A17 / 0x10A17)
        DS = Stack.Pop16();
        // POP SI (1000_0A18 / 0x10A18)
        SI = Stack.Pop16();
        // RET  (1000_0A19 / 0x10A19)
        return NearRet();
    label_1000_0A1A_10A1A:
        // LODSB SI (1000_0A1A / 0x10A1A)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // OUT DX,AL (1000_0A1B / 0x10A1B)
        Cpu.Out8(DX, AL);
        // LOOP 0x1000:0a1a (1000_0A1C / 0x10A1C)
        if (--CX != 0)
        {
            goto label_1000_0A1A_10A1A;
        }
        // POPF  (1000_0A1E / 0x10A1E)
        FlagRegister16 = Stack.Pop16();
        // POP DS (1000_0A1F / 0x10A1F)
        DS = Stack.Pop16();
        // POP SI (1000_0A20 / 0x10A20)
        SI = Stack.Pop16();
        // RET  (1000_0A21 / 0x10A21)
        return NearRet();
    }

    public virtual Action unknown_1000_0A22_10A22(int loadOffset)
    {
        // CMP BX,0xc8 (1000_0A22 / 0x10A22)
        Alu.Sub16(BX, 0xC8);
        // JC 0x1000:0a2b (1000_0A26 / 0x10A26)
        if (CarryFlag)
        {
            goto label_1000_0A2B_10A2B;
        }
        // MOV BX,0xc7 (1000_0A28 / 0x10A28)
        BX = 0xC7;
    label_1000_0A2B_10A2B:
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
        // XCHG BL,BH (1000_0A35 / 0x10A35)
        (BH, BL) = (BL, BH);
        // ADD DI,DX (1000_0A37 / 0x10A37)
        // DI += DX;
        DI = Alu.Add16(DI, DX);
        // RET  (1000_0A39 / 0x10A39)
        return NearRet();
    }

    public virtual Action unknown_1000_0A3A_10A3A(int loadOffset)
    {
        // PUSH AX (1000_0A3A / 0x10A3A)
        Stack.Push16(AX);
        // PUSH CX (1000_0A3B / 0x10A3B)
        Stack.Push16(CX);
        // PUSH DI (1000_0A3C / 0x10A3C)
        Stack.Push16(DI);
        // PUSH ES (1000_0A3D / 0x10A3D)
        Stack.Push16(ES);
        // MOV AX,0xa000 (1000_0A3E / 0x10A3E)
        AX = 0xA000;
        // MOV ES,AX (1000_0A41 / 0x10A41)
        ES = AX;
        // XOR DI,DI (1000_0A43 / 0x10A43)
        DI = 0;
        // MOV CX,0x7d00 (1000_0A45 / 0x10A45)
        CX = 0x7D00;
        // XOR AX,AX (1000_0A48 / 0x10A48)
        AX = 0;
        // REP
        while (CX != 0)
        {
            CX--;
            // STOSW ES:DI (1000_0A4A / 0x10A4A)
            UInt16[ES, DI] = AX;
            DI = (ushort)(DI + Direction16);
        }
        // POP ES (1000_0A4C / 0x10A4C)
        ES = Stack.Pop16();
        // POP DI (1000_0A4D / 0x10A4D)
        DI = Stack.Pop16();
        // POP CX (1000_0A4E / 0x10A4E)
        CX = Stack.Pop16();
        // POP AX (1000_0A4F / 0x10A4F)
        AX = Stack.Pop16();
        // RET  (1000_0A50 / 0x10A50)
        return NearRet();
    }

    public virtual Action unknown_1000_0A51_10A51(int loadOffset)
    {
        // PUSH DS (1000_0A51 / 0x10A51)
        Stack.Push16(DS);
        // MOV AX,0xa000 (1000_0A52 / 0x10A52)
        AX = 0xA000;
        // MOV DS,AX (1000_0A55 / 0x10A55)
        DS = AX;
        // MOV ES,AX (1000_0A57 / 0x10A57)
        ES = AX;
        // XOR SI,SI (1000_0A59 / 0x10A59)
        SI = 0;
        // XOR DI,DI (1000_0A5B / 0x10A5B)
        DI = 0;
        // MOV CX,0xfa00 (1000_0A5D / 0x10A5D)
        CX = 0xFA00;
    label_1000_0A60_10A60:
        // LODSB SI (1000_0A60 / 0x10A60)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // CMP AL,0xa0 (1000_0A61 / 0x10A61)
        Alu.Sub8(AL, 0xA0);
        // JNC 0x1000:0a67 (1000_0A63 / 0x10A63)
        if (!CarryFlag)
        {
            goto label_1000_0A67_10A67;
        }
        // XOR AL,AL (1000_0A65 / 0x10A65)
        AL = 0;
    label_1000_0A67_10A67:
        // STOSB ES:DI (1000_0A67 / 0x10A67)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // LOOP 0x1000:0a60 (1000_0A68 / 0x10A68)
        if (--CX != 0)
        {
            goto label_1000_0A60_10A60;
        }
        // POP DS (1000_0A6A / 0x10A6A)
        DS = Stack.Pop16();
        // RET  (1000_0A6B / 0x10A6B)
        return NearRet();
    }

    public virtual Action unknown_1000_0B9A_10B9A(int loadOffset)
    {
        switch (loadOffset)
        {
            case 0xA6C: break; // Instructions before entry targeted by 0x10AC9, 0x10A8D
            case 0: goto entry;
        }
    label_1000_0A6C_10A6C:
        // MOV BP,DX (1000_0A6C / 0x10A6C)
        BP = DX;
        // SUB DI,BP (1000_0A6E / 0x10A6E)
        DI -= BP;
        // ADD DI,0x140 (1000_0A70 / 0x10A70)
        // DI += 0x140;
        DI = Alu.Add16(DI, 0x140);
    label_1000_0A74_10A74:
        // LODSB SI (1000_0A74 / 0x10A74)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // OR AL,AL (1000_0A75 / 0x10A75)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JS 0x1000:0aa9 (1000_0A77 / 0x10A77)
        if (SignFlag)
        {
            goto label_1000_0AA9_10AA9;
        }
        // MOV CX,AX (1000_0A79 / 0x10A79)
        CX = AX;
        // XOR CH,CH (1000_0A7B / 0x10A7B)
        CH = 0;
        // INC CX (1000_0A7D / 0x10A7D)
        CX++;
        // SUB BP,CX (1000_0A7E / 0x10A7E)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
    label_1000_0A80_10A80:
        // LODSB SI (1000_0A80 / 0x10A80)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // OR AL,AL (1000_0A81 / 0x10A81)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0a9d (1000_0A83 / 0x10A83)
        if (ZeroFlag)
        {
            goto label_1000_0A9D_10A9D;
        }
        // STOSB ES:DI (1000_0A85 / 0x10A85)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // LOOP 0x1000:0a80 (1000_0A86 / 0x10A86)
        if (--CX != 0)
        {
            goto label_1000_0A80_10A80;
        }
        // OR BP,BP (1000_0A88 / 0x10A88)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0a74 (1000_0A8A / 0x10A8A)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0A74_10A74;
        }
        // DEC BX (1000_0A8C / 0x10A8C)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0a6c (1000_0A8D / 0x10A8D)
        if (!ZeroFlag)
        {
            goto label_1000_0A6C_10A6C;
        }
    label_1000_0A8F_10A8F:
        // CLD  (1000_0A8F / 0x10A8F)
        DirectionFlag = false;
        // MOV byte ptr CS:[0xa71],0xc7 (1000_0A90 / 0x10A90)
        UInt8[cs1, 0xA71] = 0xC7;
        // MOV byte ptr CS:[0xb2f],0xc7 (1000_0A96 / 0x10A96)
        UInt8[cs1, 0xB2F] = 0xC7;
        // RETF  (1000_0A9C / 0x10A9C)
        return FarRet();
    label_1000_0A9D_10A9D:
        // INC DI (1000_0A9D / 0x10A9D)
        DI = Alu.Inc16(DI);
        // LOOP 0x1000:0a80 (1000_0A9E / 0x10A9E)
        if (--CX != 0)
        {
            goto label_1000_0A80_10A80;
        }
        // OR BP,BP (1000_0AA0 / 0x10AA0)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0a74 (1000_0AA2 / 0x10AA2)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0A74_10A74;
        }
        // DEC BX (1000_0AA4 / 0x10AA4)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0a6c (1000_0AA5 / 0x10AA5)
        if (!ZeroFlag)
        {
            goto label_1000_0A6C_10A6C;
        }
        // JMP 0x1000:0a8f (1000_0AA7 / 0x10AA7)
        goto label_1000_0A8F_10A8F;
    label_1000_0AA9_10AA9:
        // MOV CX,0x101 (1000_0AA9 / 0x10AA9)
        CX = 0x101;
        // XOR AH,AH (1000_0AAC / 0x10AAC)
        AH = 0;
        // SUB CX,AX (1000_0AAE / 0x10AAE)
        CX -= AX;
        // SUB BP,CX (1000_0AB0 / 0x10AB0)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
        // LODSB SI (1000_0AB2 / 0x10AB2)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // OR AL,AL (1000_0AB3 / 0x10AB3)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0ac2 (1000_0AB5 / 0x10AB5)
        if (ZeroFlag)
        {
            goto label_1000_0AC2_10AC2;
        }
        // REP
        while (CX != 0)
        {
            CX--;
            // STOSB ES:DI (1000_0AB7 / 0x10AB7)
            UInt8[ES, DI] = AL;
            DI = (ushort)(DI + Direction8);
        }
        // OR BP,BP (1000_0AB9 / 0x10AB9)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0a74 (1000_0ABB / 0x10ABB)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0A74_10A74;
        }
        // DEC BX (1000_0ABD / 0x10ABD)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0a6c (1000_0ABE / 0x10ABE)
        if (!ZeroFlag)
        {
            goto label_1000_0A6C_10A6C;
        }
        // JMP 0x1000:0a8f (1000_0AC0 / 0x10AC0)
        goto label_1000_0A8F_10A8F;
    label_1000_0AC2_10AC2:
        // ADD DI,CX (1000_0AC2 / 0x10AC2)
        // DI += CX;
        DI = Alu.Add16(DI, CX);
        // OR BP,BP (1000_0AC4 / 0x10AC4)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0a74 (1000_0AC6 / 0x10AC6)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0A74_10A74;
        }
        // DEC BX (1000_0AC8 / 0x10AC8)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0a6c (1000_0AC9 / 0x10AC9)
        if (!ZeroFlag)
        {
            goto label_1000_0A6C_10A6C;
        }
        // JMP 0x1000:0a8f (1000_0ACB / 0x10ACB)
        goto label_1000_0A8F_10A8F;
    label_1000_0ACD_10ACD:
        // MOV BP,DX (1000_0ACD / 0x10ACD)
        BP = DX;
        // ADD DI,BP (1000_0ACF / 0x10ACF)
        DI += BP;
        // ADD DI,0x140 (1000_0AD1 / 0x10AD1)
        // DI += 0x140;
        DI = Alu.Add16(DI, 0x140);
    label_1000_0AD5_10AD5:
        // MOV AL,byte ptr [SI] (1000_0AD5 / 0x10AD5)
        AL = UInt8[DS, SI];
        // INC SI (1000_0AD7 / 0x10AD7)
        SI = Alu.Inc16(SI);
        // OR AL,AL (1000_0AD8 / 0x10AD8)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0b02 (1000_0ADA / 0x10ADA)
        if (ZeroFlag)
        {
            goto label_1000_0B02_10B02;
        }
        // MOV CX,AX (1000_0ADC / 0x10ADC)
        CX = AX;
        // XOR CH,CH (1000_0ADE / 0x10ADE)
        CH = 0;
        // INC CX (1000_0AE0 / 0x10AE0)
        CX++;
        // SUB BP,CX (1000_0AE1 / 0x10AE1)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
    label_1000_0AE3_10AE3:
        // MOV AL,byte ptr [SI] (1000_0AE3 / 0x10AE3)
        AL = UInt8[DS, SI];
        // INC SI (1000_0AE5 / 0x10AE5)
        SI = Alu.Inc16(SI);
        // OR AL,AL (1000_0AE6 / 0x10AE6)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0af6 (1000_0AE8 / 0x10AE8)
        if (ZeroFlag)
        {
            goto label_1000_0AF6_10AF6;
        }
        // STOSB ES:DI (1000_0AEA / 0x10AEA)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // LOOP 0x1000:0ae3 (1000_0AEB / 0x10AEB)
        if (--CX != 0)
        {
            goto label_1000_0AE3_10AE3;
        }
        // OR BP,BP (1000_0AED / 0x10AED)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0ad5 (1000_0AEF / 0x10AEF)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0AD5_10AD5;
        }
        // DEC BX (1000_0AF1 / 0x10AF1)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0acd (1000_0AF2 / 0x10AF2)
        if (!ZeroFlag)
        {
            goto label_1000_0ACD_10ACD;
        }
        // JMP 0x1000:0a8f (1000_0AF4 / 0x10AF4)
        goto label_1000_0A8F_10A8F;
    label_1000_0AF6_10AF6:
        // DEC DI (1000_0AF6 / 0x10AF6)
        DI = Alu.Dec16(DI);
        // LOOP 0x1000:0ae3 (1000_0AF7 / 0x10AF7)
        if (--CX != 0)
        {
            goto label_1000_0AE3_10AE3;
        }
        // OR BP,BP (1000_0AF9 / 0x10AF9)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0ad5 (1000_0AFB / 0x10AFB)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0AD5_10AD5;
        }
        // DEC BX (1000_0AFD / 0x10AFD)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0acd (1000_0AFE / 0x10AFE)
        if (!ZeroFlag)
        {
            goto label_1000_0ACD_10ACD;
        }
        // JMP 0x1000:0a8f (1000_0B00 / 0x10B00)
        goto label_1000_0A8F_10A8F;
    label_1000_0B02_10B02:
        // MOV CX,0x101 (1000_0B02 / 0x10B02)
        CX = 0x101;
        // XOR AH,AH (1000_0B05 / 0x10B05)
        AH = 0;
        // SUB CX,AX (1000_0B07 / 0x10B07)
        CX -= AX;
        // SUB BP,CX (1000_0B09 / 0x10B09)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
        // MOV AL,byte ptr [SI] (1000_0B0B / 0x10B0B)
        AL = UInt8[DS, SI];
        // INC SI (1000_0B0D / 0x10B0D)
        SI = Alu.Inc16(SI);
        // OR AL,AL (1000_0B0E / 0x10B0E)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0b1e (1000_0B10 / 0x10B10)
        if (ZeroFlag)
        {
            goto label_1000_0B1E_10B1E;
        }
        // REP
        while (CX != 0)
        {
            CX--;
            // STOSB ES:DI (1000_0B12 / 0x10B12)
            UInt8[ES, DI] = AL;
            DI = (ushort)(DI + Direction8);
        }
        // OR BP,BP (1000_0B14 / 0x10B14)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0ad5 (1000_0B16 / 0x10B16)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0AD5_10AD5;
        }
        // DEC BX (1000_0B18 / 0x10B18)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0acd (1000_0B19 / 0x10B19)
        if (!ZeroFlag)
        {
            goto label_1000_0ACD_10ACD;
        }
        // JMP 0x1000:0a8f (1000_0B1B / 0x10B1B)
        goto label_1000_0A8F_10A8F;
    label_1000_0B1E_10B1E:
        // SUB DI,CX (1000_0B1E / 0x10B1E)
        // DI -= CX;
        DI = Alu.Sub16(DI, CX);
        // OR BP,BP (1000_0B20 / 0x10B20)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0ad5 (1000_0B22 / 0x10B22)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0AD5_10AD5;
        }
        // DEC BX (1000_0B24 / 0x10B24)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0acd (1000_0B25 / 0x10B25)
        if (!ZeroFlag)
        {
            goto label_1000_0ACD_10ACD;
        }
        // JMP 0x1000:0a8f (1000_0B27 / 0x10B27)
        goto label_1000_0A8F_10A8F;
    label_1000_0B2A_10B2A:
        // MOV BP,DX (1000_0B2A / 0x10B2A)
        BP = DX;
        // SUB DI,BP (1000_0B2C / 0x10B2C)
        DI -= BP;
        // ADD DI,0x140 (1000_0B2E / 0x10B2E)
        // DI += 0x140;
        DI = Alu.Add16(DI, 0x140);
    label_1000_0B32_10B32:
        // LODSB SI (1000_0B32 / 0x10B32)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // OR AL,AL (1000_0B33 / 0x10B33)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JS 0x1000:0b48 (1000_0B35 / 0x10B35)
        if (SignFlag)
        {
            goto label_1000_0B48_10B48;
        }
        // MOV CX,AX (1000_0B37 / 0x10B37)
        CX = AX;
        // XOR CH,CH (1000_0B39 / 0x10B39)
        CH = 0;
        // INC CX (1000_0B3B / 0x10B3B)
        CX++;
        // SUB BP,CX (1000_0B3C / 0x10B3C)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
        // REP
        while (CX != 0)
        {
            CX--;
            // MOVSB ES:DI,SI (1000_0B3E / 0x10B3E)
            UInt8[ES, DI] = UInt8[DS, SI];
            SI = (ushort)(SI + Direction8);
            DI = (ushort)(DI + Direction8);
        }
        // JA 0x1000:0b32 (1000_0B40 / 0x10B40)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0B32_10B32;
        }
        // DEC BX (1000_0B42 / 0x10B42)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0b2a (1000_0B43 / 0x10B43)
        if (!ZeroFlag)
        {
            goto label_1000_0B2A_10B2A;
        }
        // JMP 0x1000:0a8f (1000_0B45 / 0x10B45)
        goto label_1000_0A8F_10A8F;
    label_1000_0B48_10B48:
        // MOV CX,0x101 (1000_0B48 / 0x10B48)
        CX = 0x101;
        // XOR AH,AH (1000_0B4B / 0x10B4B)
        AH = 0;
        // SUB CX,AX (1000_0B4D / 0x10B4D)
        CX -= AX;
        // SUB BP,CX (1000_0B4F / 0x10B4F)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
        // LODSB SI (1000_0B51 / 0x10B51)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // REP
        while (CX != 0)
        {
            CX--;
            // STOSB ES:DI (1000_0B52 / 0x10B52)
            UInt8[ES, DI] = AL;
            DI = (ushort)(DI + Direction8);
        }
        // JA 0x1000:0b32 (1000_0B54 / 0x10B54)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0B32_10B32;
        }
        // DEC BX (1000_0B56 / 0x10B56)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0b2a (1000_0B57 / 0x10B57)
        if (!ZeroFlag)
        {
            goto label_1000_0B2A_10B2A;
        }
        // JMP 0x1000:0a8f (1000_0B59 / 0x10B59)
        goto label_1000_0A8F_10A8F;
    label_1000_0B5C_10B5C:
        // MOV BP,DX (1000_0B5C / 0x10B5C)
        BP = DX;
        // ADD DI,BP (1000_0B5E / 0x10B5E)
        DI += BP;
        // ADD DI,0x140 (1000_0B60 / 0x10B60)
        // DI += 0x140;
        DI = Alu.Add16(DI, 0x140);
    label_1000_0B64_10B64:
        // MOV AL,byte ptr [SI] (1000_0B64 / 0x10B64)
        AL = UInt8[DS, SI];
        // INC SI (1000_0B66 / 0x10B66)
        SI = Alu.Inc16(SI);
        // OR AL,AL (1000_0B67 / 0x10B67)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0b82 (1000_0B69 / 0x10B69)
        if (ZeroFlag)
        {
            goto label_1000_0B82_10B82;
        }
        // MOV CX,AX (1000_0B6B / 0x10B6B)
        CX = AX;
        // XOR CH,CH (1000_0B6D / 0x10B6D)
        CH = 0;
        // INC CX (1000_0B6F / 0x10B6F)
        CX++;
        // SUB BP,CX (1000_0B70 / 0x10B70)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
    label_1000_0B72_10B72:
        // MOV AL,byte ptr [SI] (1000_0B72 / 0x10B72)
        AL = UInt8[DS, SI];
        // INC SI (1000_0B74 / 0x10B74)
        SI = Alu.Inc16(SI);
        // STOSB ES:DI (1000_0B75 / 0x10B75)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // LOOP 0x1000:0b72 (1000_0B76 / 0x10B76)
        if (--CX != 0)
        {
            goto label_1000_0B72_10B72;
        }
        // OR BP,BP (1000_0B78 / 0x10B78)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0b64 (1000_0B7A / 0x10B7A)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0B64_10B64;
        }
        // DEC BX (1000_0B7C / 0x10B7C)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0b5c (1000_0B7D / 0x10B7D)
        if (!ZeroFlag)
        {
            goto label_1000_0B5C_10B5C;
        }
        // JMP 0x1000:0a8f (1000_0B7F / 0x10B7F)
        goto label_1000_0A8F_10A8F;
    label_1000_0B82_10B82:
        // MOV CX,0x101 (1000_0B82 / 0x10B82)
        CX = 0x101;
        // XOR AH,AH (1000_0B85 / 0x10B85)
        AH = 0;
        // SUB CX,AX (1000_0B87 / 0x10B87)
        CX -= AX;
        // SUB BP,CX (1000_0B89 / 0x10B89)
        // BP -= CX;
        BP = Alu.Sub16(BP, CX);
        // MOV AL,byte ptr [SI] (1000_0B8B / 0x10B8B)
        AL = UInt8[DS, SI];
        // INC SI (1000_0B8D / 0x10B8D)
        SI = Alu.Inc16(SI);
        // REP
        while (CX != 0)
        {
            CX--;
            // STOSB ES:DI (1000_0B8E / 0x10B8E)
            UInt8[ES, DI] = AL;
            DI = (ushort)(DI + Direction8);
        }
        // OR BP,BP (1000_0B90 / 0x10B90)
        // BP |= BP;
        BP = Alu.Or16(BP, BP);
        // JA 0x1000:0b64 (1000_0B92 / 0x10B92)
        if (!CarryFlag && !ZeroFlag)
        {
            goto label_1000_0B64_10B64;
        }
        // DEC BX (1000_0B94 / 0x10B94)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0b5c (1000_0B95 / 0x10B95)
        if (!ZeroFlag)
        {
            goto label_1000_0B5C_10B5C;
        }
        // JMP 0x1000:0a8f (1000_0B97 / 0x10B97)
        goto label_1000_0A8F_10A8F;
    entry:
        // CMP CH,0xfe (1000_0B9A / 0x10B9A)
        Alu.Sub8(CH, 0xFE);
        // JNC 0x1000:0ba0 (1000_0B9D / 0x10B9D)
        if (!CarryFlag)
        {
            goto label_1000_0BA0_10BA0;
        }
        // RETF  (1000_0B9F / 0x10B9F)
        return FarRet();
    label_1000_0BA0_10BA0:
        // OR DI,DI (1000_0BA0 / 0x10BA0)
        // DI |= DI;
        DI = Alu.Or16(DI, DI);
        // JS 0x1000:0bfa (1000_0BA2 / 0x10BA2)
        if (SignFlag)
        {
            goto label_1000_0BFA_10BFA;
        }
        // MOV BP,DI (1000_0BA4 / 0x10BA4)
        BP = DI;
        // AND BP,0x1ff (1000_0BA6 / 0x10BA6)
        // BP &= 0x1FF;
        BP = Alu.And16(BP, 0x1FF);
        // MOV AX,DI (1000_0BAA / 0x10BAA)
        AX = DI;
        // CALL 0x1000:0a22 (1000_0BAC / 0x10BAC)
        NearCall(cs1, 0xBAF, unknown_1000_0A22_10A22);
        // MOV BX,CX (1000_0BAF / 0x10BAF)
        BX = CX;
        // XOR BH,BH (1000_0BB1 / 0x10BB1)
        BH = 0;
        // CMP CH,0xff (1000_0BB3 / 0x10BB3)
        Alu.Sub8(CH, 0xFF);
        // JZ 0x1000:0bd9 (1000_0BB6 / 0x10BB6)
        if (ZeroFlag)
        {
            goto label_1000_0BD9_10BD9;
        }
        // SHR BP,0x1 (1000_0BB8 / 0x10BB8)
        // BP >>= 0x1;
        BP = Alu.Shr16(BP, 0x1);
        // MOV AX,DI (1000_0BBA / 0x10BBA)
        AX = DI;
        // JC 0x1000:0bcb (1000_0BBC / 0x10BBC)
        if (CarryFlag)
        {
            goto label_1000_0BCB_10BCB;
        }
    label_1000_0BBE_10BBE:
        // MOV CX,BP (1000_0BBE / 0x10BBE)
        CX = BP;
        // MOV DI,AX (1000_0BC0 / 0x10BC0)
        DI = AX;
        // REP
        while (CX != 0)
        {
            CX--;
            // MOVSW ES:DI,SI (1000_0BC2 / 0x10BC2)
            UInt16[ES, DI] = UInt16[DS, SI];
            SI = (ushort)(SI + Direction16);
            DI = (ushort)(DI + Direction16);
        }
        // ADD AX,0x140 (1000_0BC4 / 0x10BC4)
        AX += 0x140;
        // DEC BX (1000_0BC7 / 0x10BC7)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0bbe (1000_0BC8 / 0x10BC8)
        if (!ZeroFlag)
        {
            goto label_1000_0BBE_10BBE;
        }
        // RETF  (1000_0BCA / 0x10BCA)
        return FarRet();
    label_1000_0BCB_10BCB:
        // MOV CX,BP (1000_0BCB / 0x10BCB)
        CX = BP;
        // MOV DI,AX (1000_0BCD / 0x10BCD)
        DI = AX;
        // REP
        while (CX != 0)
        {
            CX--;
            // MOVSW ES:DI,SI (1000_0BCF / 0x10BCF)
            UInt16[ES, DI] = UInt16[DS, SI];
            SI = (ushort)(SI + Direction16);
            DI = (ushort)(DI + Direction16);
        }
        // MOVSB ES:DI,SI (1000_0BD1 / 0x10BD1)
        UInt8[ES, DI] = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        DI = (ushort)(DI + Direction8);
        // ADD AX,0x140 (1000_0BD2 / 0x10BD2)
        AX += 0x140;
        // DEC BX (1000_0BD5 / 0x10BD5)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0bcb (1000_0BD6 / 0x10BD6)
        if (!ZeroFlag)
        {
            goto label_1000_0BCB_10BCB;
        }
        // RETF  (1000_0BD8 / 0x10BD8)
        return FarRet();
    label_1000_0BD9_10BD9:
        // MOV DX,DI (1000_0BD9 / 0x10BD9)
        DX = DI;
    label_1000_0BDB_10BDB:
        // MOV CX,BP (1000_0BDB / 0x10BDB)
        CX = BP;
        // MOV DI,DX (1000_0BDD / 0x10BDD)
        DI = DX;
    label_1000_0BDF_10BDF:
        // LODSB SI (1000_0BDF / 0x10BDF)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // OR AL,AL (1000_0BE0 / 0x10BE0)
        // AL |= AL;
        AL = Alu.Or8(AL, AL);
        // JZ 0x1000:0bef (1000_0BE2 / 0x10BE2)
        if (ZeroFlag)
        {
            goto label_1000_0BEF_10BEF;
        }
        // STOSB ES:DI (1000_0BE4 / 0x10BE4)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // LOOP 0x1000:0bdf (1000_0BE5 / 0x10BE5)
        if (--CX != 0)
        {
            goto label_1000_0BDF_10BDF;
        }
        // ADD DX,0x140 (1000_0BE7 / 0x10BE7)
        DX += 0x140;
        // DEC BX (1000_0BEB / 0x10BEB)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0bdb (1000_0BEC / 0x10BEC)
        if (!ZeroFlag)
        {
            goto label_1000_0BDB_10BDB;
        }
        // RETF  (1000_0BEE / 0x10BEE)
        return FarRet();
    label_1000_0BEF_10BEF:
        // INC DI (1000_0BEF / 0x10BEF)
        DI = Alu.Inc16(DI);
        // LOOP 0x1000:0bdf (1000_0BF0 / 0x10BF0)
        if (--CX != 0)
        {
            goto label_1000_0BDF_10BDF;
        }
        // ADD DX,0x140 (1000_0BF2 / 0x10BF2)
        DX += 0x140;
        // DEC BX (1000_0BF6 / 0x10BF6)
        BX = Alu.Dec16(BX);
        // JNZ 0x1000:0bdb (1000_0BF7 / 0x10BF7)
        if (!ZeroFlag)
        {
            goto label_1000_0BDB_10BDB;
        }
        // RETF  (1000_0BF9 / 0x10BF9)
        return FarRet();
    label_1000_0BFA_10BFA:
        // MOV BP,DI (1000_0BFA / 0x10BFA)
        BP = DI;
        // AND BP,0x1ff (1000_0BFC / 0x10BFC)
        // BP &= 0x1FF;
        BP = Alu.And16(BP, 0x1FF);
        // MOV AX,DI (1000_0C00 / 0x10C00)
        AX = DI;
        // CALL 0x1000:0a22 (1000_0C02 / 0x10C02)
        NearCall(cs1, 0xC05, unknown_1000_0A22_10A22);
        // MOV BX,CX (1000_0C05 / 0x10C05)
        BX = CX;
        // XOR BH,BH (1000_0C07 / 0x10C07)
        BH = 0;
        // TEST AX,0x4000 (1000_0C09 / 0x10C09)
        Alu.And16(AX, 0x4000);
        // JNZ 0x1000:0c3e (1000_0C0C / 0x10C0C)
        if (!ZeroFlag)
        {
            goto label_1000_0C3E_10C3E;
        }
        // TEST AX,0x2000 (1000_0C0E / 0x10C0E)
        Alu.And16(AX, 0x2000);
        // JZ 0x1000:0c31 (1000_0C11 / 0x10C11)
        if (ZeroFlag)
        {
            goto label_1000_0C31_10C31;
        }
        // MOV byte ptr CS:[0xa71],0xef (1000_0C13 / 0x10C13)
        UInt8[cs1, 0xA71] = 0xEF;
        // MOV byte ptr CS:[0xb2f],0xef (1000_0C19 / 0x10C19)
        UInt8[cs1, 0xB2F] = 0xEF;
        // MOV AH,BL (1000_0C1F / 0x10C1F)
        AH = BL;
        // DEC AH (1000_0C21 / 0x10C21)
        AH = Alu.Dec8(AH);
        // MOV DH,AH (1000_0C23 / 0x10C23)
        DH = AH;
        // XOR DL,DL (1000_0C25 / 0x10C25)
        DL = 0;
        // MOV AL,DL (1000_0C27 / 0x10C27)
        AL = DL;
        // SHR DX,0x1 (1000_0C29 / 0x10C29)
        DX >>= 0x1;
        // SHR DX,0x1 (1000_0C2B / 0x10C2B)
        DX >>= 0x1;
        // ADD DI,AX (1000_0C2D / 0x10C2D)
        DI += AX;
        // ADD DI,DX (1000_0C2F / 0x10C2F)
        // DI += DX;
        DI = Alu.Add16(DI, DX);
    label_1000_0C31_10C31:
        // MOV DX,BP (1000_0C31 / 0x10C31)
        DX = BP;
        // CMP CH,0xff (1000_0C33 / 0x10C33)
        Alu.Sub8(CH, 0xFF);
        // JZ 0x1000:0c3b (1000_0C36 / 0x10C36)
        if (ZeroFlag)
        {
            // JZ target is JMP, inlining.
            // JMP 0x1000:0a74 (1000_0C3B / 0x10C3B)
            goto label_1000_0A74_10A74;
        }
        // JMP 0x1000:0b32 (1000_0C38 / 0x10C38)
        goto label_1000_0B32_10B32;
    label_1000_0C3E_10C3E:
        // TEST AX,0x2000 (1000_0C3E / 0x10C3E)
        Alu.And16(AX, 0x2000);
        // JZ 0x1000:0c61 (1000_0C41 / 0x10C41)
        if (ZeroFlag)
        {
            goto label_1000_0C61_10C61;
        }
        // MOV byte ptr CS:[0xad2],0xef (1000_0C43 / 0x10C43)
        UInt8[cs1, 0xAD2] = 0xEF;
        // MOV byte ptr CS:[0xb61],0xef (1000_0C49 / 0x10C49)
        UInt8[cs1, 0xB61] = 0xEF;
        // MOV AH,BL (1000_0C4F / 0x10C4F)
        AH = BL;
        // DEC AH (1000_0C51 / 0x10C51)
        AH = Alu.Dec8(AH);
        // MOV DH,AH (1000_0C53 / 0x10C53)
        DH = AH;
        // XOR DL,DL (1000_0C55 / 0x10C55)
        DL = 0;
        // MOV AL,DL (1000_0C57 / 0x10C57)
        AL = DL;
        // SHR DX,0x1 (1000_0C59 / 0x10C59)
        DX >>= 0x1;
        // SHR DX,0x1 (1000_0C5B / 0x10C5B)
        DX >>= 0x1;
        // ADD DI,AX (1000_0C5D / 0x10C5D)
        DI += AX;
        // ADD DI,DX (1000_0C5F / 0x10C5F)
        DI += DX;
    label_1000_0C61_10C61:
        // ADD DI,BP (1000_0C61 / 0x10C61)
        DI += BP;
        // DEC DI (1000_0C63 / 0x10C63)
        DI = Alu.Dec16(DI);
        // STD  (1000_0C64 / 0x10C64)
        DirectionFlag = true;
        // MOV DX,BP (1000_0C65 / 0x10C65)
        DX = BP;
        // CMP CH,0xff (1000_0C67 / 0x10C67)
        Alu.Sub8(CH, 0xFF);
        // JZ 0x1000:0c6f (1000_0C6A / 0x10C6A)
        if (ZeroFlag)
        {
            // JZ target is JMP, inlining.
            // JMP 0x1000:0ad5 (1000_0C6F / 0x10C6F)
            goto label_1000_0AD5_10AD5;
        }
        // JMP 0x1000:0b64 (1000_0C6C / 0x10C6C)
        goto label_1000_0B64_10B64;
    }

    public virtual Action unknown_1000_0C72_10C72(int loadOffset)
    {
        // PUSH DS (1000_0C72 / 0x10C72)
        Stack.Push16(DS);
        // MOV AX,0xa000 (1000_0C73 / 0x10C73)
        AX = 0xA000;
        // MOV ES,AX (1000_0C76 / 0x10C76)
        ES = AX;
        // MOV DS,AX (1000_0C78 / 0x10C78)
        DS = AX;
        // XOR SI,SI (1000_0C7A / 0x10C7A)
        SI = 0;
        // MOV DI,0x140 (1000_0C7C / 0x10C7C)
        DI = 0x140;
        // MOV DX,0x64 (1000_0C7F / 0x10C7F)
        DX = 0x64;
    label_1000_0C82_10C82:
        // MOV CX,0x50 (1000_0C82 / 0x10C82)
        CX = 0x50;
    label_1000_0C85_10C85:
        // LODSW SI (1000_0C85 / 0x10C85)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // XCHG AH,AL (1000_0C86 / 0x10C86)
        (AL, AH) = (AH, AL);
        // SUB DI,0x2 (1000_0C88 / 0x10C88)
        // DI -= 0x2;
        DI = Alu.Sub16(DI, 0x2);
        // MOV word ptr [DI],AX (1000_0C8B / 0x10C8B)
        UInt16[DS, DI] = AX;
        // LOOP 0x1000:0c85 (1000_0C8D / 0x10C8D)
        if (--CX != 0)
        {
            goto label_1000_0C85_10C85;
        }
        // ADD SI,0xa0 (1000_0C8F / 0x10C8F)
        SI += 0xA0;
        // ADD DI,0x1e0 (1000_0C93 / 0x10C93)
        DI += 0x1E0;
        // DEC DX (1000_0C97 / 0x10C97)
        DX = Alu.Dec16(DX);
        // JNZ 0x1000:0c82 (1000_0C98 / 0x10C98)
        if (!ZeroFlag)
        {
            goto label_1000_0C82_10C82;
        }
        // XOR SI,SI (1000_0C9A / 0x10C9A)
        SI = 0;
        // MOV DI,0xf8c0 (1000_0C9C / 0x10C9C)
        DI = 0xF8C0;
        // MOV DX,0x64 (1000_0C9F / 0x10C9F)
        DX = 0x64;
    label_1000_0CA2_10CA2:
        // MOV CX,0xa0 (1000_0CA2 / 0x10CA2)
        CX = 0xA0;
        // REP
        while (CX != 0)
        {
            CX--;
            // MOVSW ES:DI,SI (1000_0CA5 / 0x10CA5)
            UInt16[ES, DI] = UInt16[DS, SI];
            SI = (ushort)(SI + Direction16);
            DI = (ushort)(DI + Direction16);
        }
        // SUB DI,0x280 (1000_0CA7 / 0x10CA7)
        DI -= 0x280;
        // DEC DX (1000_0CAB / 0x10CAB)
        DX = Alu.Dec16(DX);
        // JNZ 0x1000:0ca2 (1000_0CAC / 0x10CAC)
        if (!ZeroFlag)
        {
            goto label_1000_0CA2_10CA2;
        }
        // POP DS (1000_0CAE / 0x10CAE)
        DS = Stack.Pop16();
        // RET  (1000_0CAF / 0x10CAF)
        return NearRet();
    }

    public virtual Action unknown_1000_0CF4_10CF4(int loadOffset)
    {
        // CALL 0x1000:0c72 (1000_0CF4 / 0x10CF4)
        NearCall(cs1, 0xCF7, unknown_1000_0C72_10C72);
        // MOV SI,0xcbc (1000_0CF7 / 0x10CF7)
        SI = 0xCBC;
        // MOV AX,word ptr CS:[SI] (1000_0CFA / 0x10CFA)
        AX = UInt16[cs1, SI];
        // MOV CS:[0xcb6],AX (1000_0CFD / 0x10CFD)
        UInt16[cs1, 0xCB6] = AX;
        // MOV word ptr CS:[0xcba],SI (1000_0D01 / 0x10D01)
        UInt16[cs1, 0xCBA] = SI;
        // XOR AX,AX (1000_0D06 / 0x10D06)
        AX = 0;
        // MOV CS:[0xcb0],AX (1000_0D08 / 0x10D08)
        UInt16[cs1, 0xCB0] = AX;
        // MOV CS:[0xcb2],AX (1000_0D0C / 0x10D0C)
        UInt16[cs1, 0xCB2] = AX;
        // MOV CS:[0xcb4],AX (1000_0D10 / 0x10D10)
        UInt16[cs1, 0xCB4] = AX;
        // MOV CX,0xfb (1000_0D14 / 0x10D14)
        CX = 0xFB;
    label_1000_0D17_10D17:
        // PUSH CX (1000_0D17 / 0x10D17)
        Stack.Push16(CX);
        // CALL 0x1000:0d22 (1000_0D18 / 0x10D18)
        NearCall(cs1, 0xD1B, unknown_1000_0D22_10D22);
        // POP CX (1000_0D1B / 0x10D1B)
        CX = Stack.Pop16();
        // CALL 0x1000:1085 (1000_0D1C / 0x10D1C)
        NearCall(cs1, 0xD1F, unknown_1000_1085_11085);
        // LOOPZ 0x1000:0d17 (1000_0D1F / 0x10D1F)
        if (--CX != 0 && ZeroFlag)
        {
            goto label_1000_0D17_10D17;
        }
        // RET  (1000_0D21 / 0x10D21)
        return NearRet();
    }

    public virtual Action unknown_1000_0D22_10D22(int loadOffset)
    {
        // PUSH DS (1000_0D22 / 0x10D22)
        Stack.Push16(DS);
        // PUSH ES (1000_0D23 / 0x10D23)
        Stack.Push16(ES);
        // PUSH CS (1000_0D24 / 0x10D24)
        Stack.Push16(cs1);
        // PUSH CS (1000_0D25 / 0x10D25)
        Stack.Push16(cs1);
        // POP DS (1000_0D26 / 0x10D26)
        DS = Stack.Pop16();
        // POP ES (1000_0D27 / 0x10D27)
        ES = Stack.Pop16();
        // CMP word ptr CS:[0xcb6],0x0 (1000_0D28 / 0x10D28)
        Alu.Sub16(UInt16[cs1, 0xCB6], 0x0);
        // JS 0x1000:0d5c (1000_0D2E / 0x10D2E)
        if (SignFlag)
        {
            goto label_1000_0D5C_10D5C;
        }
        // MOV DI,0x160 (1000_0D30 / 0x10D30)
        DI = 0x160;
        // MOV SI,DI (1000_0D33 / 0x10D33)
        SI = DI;
        // MOV CX,0xf0 (1000_0D35 / 0x10D35)
        CX = 0xF0;
        // MOV DX,word ptr CS:[0xcb8] (1000_0D38 / 0x10D38)
        DX = UInt16[cs1, 0xCB8];
        // MOV AX,DX (1000_0D3D / 0x10D3D)
        AX = DX;
        // SHL AX,0x1 (1000_0D3F / 0x10D3F)
        AX <<= 0x1;
        // ADD AX,DX (1000_0D41 / 0x10D41)
        AX += DX;
        // ADD SI,AX (1000_0D43 / 0x10D43)
        SI += AX;
        // SUB CX,AX (1000_0D45 / 0x10D45)
        // CX -= AX;
        CX = Alu.Sub16(CX, AX);
        // REP
        while (CX != 0)
        {
            CX--;
            // MOVSB ES:DI,SI (1000_0D47 / 0x10D47)
            UInt8[ES, DI] = UInt8[DS, SI];
            SI = (ushort)(SI + Direction8);
            DI = (ushort)(DI + Direction8);
        }
        // MOV CX,DX (1000_0D49 / 0x10D49)
        CX = DX;
    label_1000_0D4B_10D4B:
        // CALL 0x1000:0d5f (1000_0D4B / 0x10D4B)
        NearCall(cs1, 0xD4E, unknown_1000_0D5F_10D5F);
        // LOOP 0x1000:0d4b (1000_0D4E / 0x10D4E)
        if (--CX != 0)
        {
            goto label_1000_0D4B_10D4B;
        }
        // MOV DX,0x160 (1000_0D50 / 0x10D50)
        DX = 0x160;
        // MOV BX,0x50 (1000_0D53 / 0x10D53)
        BX = 0x50;
        // MOV CX,0x50 (1000_0D56 / 0x10D56)
        CX = 0x50;
        // CALL 0x1000:09d8 (1000_0D59 / 0x10D59)
        NearCall(cs1, 0xD5C, unknown_1000_09D8_109D8);
    label_1000_0D5C_10D5C:
        // POP ES (1000_0D5C / 0x10D5C)
        ES = Stack.Pop16();
        // POP DS (1000_0D5D / 0x10D5D)
        DS = Stack.Pop16();
        // RET  (1000_0D5E / 0x10D5E)
        return NearRet();
    }

    public virtual Action unknown_1000_0D5F_10D5F(int loadOffset)
    {
        // MOV SI,word ptr CS:[0xcba] (1000_0D5F / 0x10D5F)
        SI = UInt16[cs1, 0xCBA];
        // DEC word ptr CS:[0xcb6] (1000_0D64 / 0x10D64)
        UInt16[cs1, 0xCB6] = Alu.Dec16(UInt16[cs1, 0xCB6]);
        // JNZ 0x1000:0d79 (1000_0D69 / 0x10D69)
        if (!ZeroFlag)
        {
            goto label_1000_0D79_10D79;
        }
        // ADD SI,0x8 (1000_0D6B / 0x10D6B)
        // SI += 0x8;
        SI = Alu.Add16(SI, 0x8);
        // MOV word ptr CS:[0xcba],SI (1000_0D6E / 0x10D6E)
        UInt16[cs1, 0xCBA] = SI;
        // MOV AX,word ptr [SI] (1000_0D73 / 0x10D73)
        AX = UInt16[DS, SI];
        // MOV CS:[0xcb6],AX (1000_0D75 / 0x10D75)
        UInt16[cs1, 0xCB6] = AX;
    label_1000_0D79_10D79:
        // MOV AX,word ptr [SI + 0x2] (1000_0D79 / 0x10D79)
        AX = UInt16[DS, (ushort)(SI + 0x2)];
        // ADD AX,word ptr CS:[0xcb0] (1000_0D7C / 0x10D7C)
        // AX += UInt16[cs1, 0xCB0];
        AX = Alu.Add16(AX, UInt16[cs1, 0xCB0]);
        // MOV CS:[0xcb0],AX (1000_0D81 / 0x10D81)
        UInt16[cs1, 0xCB0] = AX;
        // SHL AL,0x1 (1000_0D85 / 0x10D85)
        // AL <<= 0x1;
        AL = Alu.Shl8(AL, 0x1);
        // ADC AH,0x0 (1000_0D87 / 0x10D87)
        AH = Alu.Adc8(AH, 0x0);
        // MOV AL,AH (1000_0D8A / 0x10D8A)
        AL = AH;
        // AND AL,0x3f (1000_0D8C / 0x10D8C)
        // AL &= 0x3F;
        AL = Alu.And8(AL, 0x3F);
        // STOSB ES:DI (1000_0D8E / 0x10D8E)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // MOV AX,word ptr [SI + 0x4] (1000_0D8F / 0x10D8F)
        AX = UInt16[DS, (ushort)(SI + 0x4)];
        // ADD AX,word ptr CS:[0xcb2] (1000_0D92 / 0x10D92)
        // AX += UInt16[cs1, 0xCB2];
        AX = Alu.Add16(AX, UInt16[cs1, 0xCB2]);
        // MOV CS:[0xcb2],AX (1000_0D97 / 0x10D97)
        UInt16[cs1, 0xCB2] = AX;
        // SHL AL,0x1 (1000_0D9B / 0x10D9B)
        // AL <<= 0x1;
        AL = Alu.Shl8(AL, 0x1);
        // ADC AH,0x0 (1000_0D9D / 0x10D9D)
        AH = Alu.Adc8(AH, 0x0);
        // MOV AL,AH (1000_0DA0 / 0x10DA0)
        AL = AH;
        // AND AL,0x3f (1000_0DA2 / 0x10DA2)
        // AL &= 0x3F;
        AL = Alu.And8(AL, 0x3F);
        // STOSB ES:DI (1000_0DA4 / 0x10DA4)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // MOV AX,word ptr [SI + 0x6] (1000_0DA5 / 0x10DA5)
        AX = UInt16[DS, (ushort)(SI + 0x6)];
        // ADD AX,word ptr CS:[0xcb4] (1000_0DA8 / 0x10DA8)
        // AX += UInt16[cs1, 0xCB4];
        AX = Alu.Add16(AX, UInt16[cs1, 0xCB4]);
        // MOV CS:[0xcb4],AX (1000_0DAD / 0x10DAD)
        UInt16[cs1, 0xCB4] = AX;
        // SHL AL,0x1 (1000_0DB1 / 0x10DB1)
        // AL <<= 0x1;
        AL = Alu.Shl8(AL, 0x1);
        // ADC AH,0x0 (1000_0DB3 / 0x10DB3)
        AH = Alu.Adc8(AH, 0x0);
        // MOV AL,AH (1000_0DB6 / 0x10DB6)
        AL = AH;
        // AND AL,0x3f (1000_0DB8 / 0x10DB8)
        // AL &= 0x3F;
        AL = Alu.And8(AL, 0x3F);
        // STOSB ES:DI (1000_0DBA / 0x10DBA)
        UInt8[ES, DI] = AL;
        DI = (ushort)(DI + Direction8);
        // RET  (1000_0DBB / 0x10DBB)
        return NearRet();
    }

    public virtual Action unknown_1000_0DBC_10DBC(int loadOffset)
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
        // JC 0x1000:0ddd (1000_0DD2 / 0x10DD2)
        if (CarryFlag)
        {
            // JC target is RET, inlining.
            // RET  (1000_0DDD / 0x10DDD)
            return NearRet();
        }
        // CALL 0x1000:109a (1000_0DD4 / 0x10DD4)
        NearCall(cs1, 0xDD7, unknown_1000_109A_1109A);
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

    public virtual Action unknown_1000_0DDE_10DDE(int loadOffset)
    {
        // MOV [0x54],AX (1000_0DDE / 0x10DDE)
        UInt16[DS, 0x54] = AX;
        // CALL 0x1000:0dbc (1000_0DE1 / 0x10DE1)
        NearCall(cs1, 0xDE4, unknown_1000_0DBC_10DBC);
        // JC 0x1000:0e45 (1000_0DE4 / 0x10DE4)
        if (CarryFlag)
        {
            // JC target is RET, inlining.
            // RET  (1000_0E45 / 0x10E45)
            return NearRet();
        }
    label_1000_0DE6_10DE6:
        // CALL 0x1000:0a3a (1000_0DE6 / 0x10DE6)
        NearCall(cs1, 0xDE9, unknown_1000_0A3A_10A3A);
        // MOV AX,[0x54] (1000_0DE9 / 0x10DE9)
        AX = UInt16[DS, 0x54];
        // MOV [0x52],AX (1000_0DEC / 0x10DEC)
        UInt16[DS, 0x52] = AX;
        // LES DI,[0x4c] (1000_0DEF / 0x10DEF)
        DI = UInt16[DS, 0x4C];
        ES = UInt16[DS, 0x4E];
        // MOV word ptr [0x56],DI (1000_0DF3 / 0x10DF3)
        UInt16[DS, 0x56] = DI;
        // MOV word ptr [0x58],ES (1000_0DF7 / 0x10DF7)
        UInt16[DS, 0x58] = ES;
        // CALL 0x1000:0ead (1000_0DFB / 0x10DFB)
        NearCall(cs1, 0xDFE, unknown_1000_0EAD_10EAD);
        // JZ 0x1000:0e45 (1000_0DFE / 0x10DFE)
        if (ZeroFlag)
        {
            // JZ target is RET, inlining.
            // RET  (1000_0E45 / 0x10E45)
            return NearRet();
        }
        // CALL 0x1000:0fa4 (1000_0E00 / 0x10E00)
        NearCall(cs1, 0xE03, unknown_1000_0FA4_10FA4);
        // CALL 0x1000:0e4c (1000_0E03 / 0x10E03)
        NearCall(cs1, 0xE06, unknown_1000_0E4C_10E4C);
        // STC  (1000_0E06 / 0x10E06)
        CarryFlag = true;
        // JZ 0x1000:0e45 (1000_0E07 / 0x10E07)
        if (ZeroFlag)
        {
            // JZ target is RET, inlining.
            // RET  (1000_0E45 / 0x10E45)
            return NearRet();
        }
        // CALL 0x1000:0e49 (1000_0E09 / 0x10E09)
        NearCall(cs1, 0xE0C, unknown_1000_0E49_10E49);
        // CALL 0x1000:0cf4 (1000_0E0C / 0x10E0C)
        NearCall(cs1, 0xE0F, unknown_1000_0CF4_10CF4);
        // STC  (1000_0E0F / 0x10E0F)
        CarryFlag = true;
        // JNZ 0x1000:0e45 (1000_0E10 / 0x10E10)
        if (!ZeroFlag)
        {
            // JNZ target is RET, inlining.
            // RET  (1000_0E45 / 0x10E45)
            return NearRet();
        }
    label_1000_0E12_10E12:
        // MOV AX,[0x52] (1000_0E12 / 0x10E12)
        AX = UInt16[DS, 0x52];
        // MOV BP,0xe46 (1000_0E15 / 0x10E15)
        BP = 0xE46;
        // CALL 0x1000:0fea (1000_0E18 / 0x10E18)
        NearCall(cs1, 0xE1B, unknown_1000_0FEA_10FEA);
        // STC  (1000_0E1B / 0x10E1B)
        CarryFlag = true;
        // JNZ 0x1000:0e45 (1000_0E1C / 0x10E1C)
        if (!ZeroFlag)
        {
            // JNZ target is RET, inlining.
            // RET  (1000_0E45 / 0x10E45)
            return NearRet();
        }
        // CMP word ptr [0x52],0x0 (1000_0E1E / 0x10E1E)
        Alu.Sub16(UInt16[DS, 0x52], 0x0);
        // JNZ 0x1000:0e12 (1000_0E23 / 0x10E23)
        if (!ZeroFlag)
        {
            goto label_1000_0E12_10E12;
        }
        // MOV SI,0xee (1000_0E25 / 0x10E25)
        SI = 0xEE;
        // LODSW SI (1000_0E28 / 0x10E28)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // CALL 0x1000:11bd (1000_0E29 / 0x10E29)
        NearCall(cs1, 0xE2C, unknown_1000_11BD_111BD);
        // XCHG AH,AL (1000_0E2C / 0x10E2C)
        (AL, AH) = (AH, AL);
        // CALL 0x1000:11bd (1000_0E2E / 0x10E2E)
        NearCall(cs1, 0xE31, unknown_1000_11BD_111BD);
        // CMP AX,0x4c4f (1000_0E31 / 0x10E31)
        Alu.Sub16(AX, 0x4C4F);
        // JNZ 0x1000:0e44 (1000_0E34 / 0x10E34)
        if (!ZeroFlag)
        {
            goto label_1000_0E44_10E44;
        }
        // LODSW SI (1000_0E36 / 0x10E36)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // CALL 0x1000:11bd (1000_0E37 / 0x10E37)
        NearCall(cs1, 0xE3A, unknown_1000_11BD_111BD);
        // XCHG AH,AL (1000_0E3A / 0x10E3A)
        (AL, AH) = (AH, AL);
        // CALL 0x1000:11bd (1000_0E3C / 0x10E3C)
        NearCall(cs1, 0xE3F, unknown_1000_11BD_111BD);
        // CMP AX,0x4f50 (1000_0E3F / 0x10E3F)
        Alu.Sub16(AX, 0x4F50);
        // JZ 0x1000:0de6 (1000_0E42 / 0x10E42)
        if (ZeroFlag)
        {
            goto label_1000_0DE6_10DE6;
        }
    label_1000_0E44_10E44:
        // CLC  (1000_0E44 / 0x10E44)
        CarryFlag = false;
        // RET  (1000_0E45 / 0x10E45)
        return NearRet();
    }

    public virtual Action unknown_1000_0E46_10E46(int loadOffset)
    {
        // CALL 0x1000:0d22 (1000_0E46 / 0x10E46)
        NearCall(cs1, 0xE49, unknown_1000_0D22_10D22);
        // Function call generated as ASM continues to next function entry point without return
        return unknown_1000_0E49_10E49(0);
    }

    public virtual Action unknown_1000_0E49_10E49(int loadOffset)
    {
        // CALL 0x1000:0e59 (1000_0E49 / 0x10E49)
        NearCall(cs1, 0xE4C, unknown_1000_0E59_10E59);
        // Function call generated as ASM continues to next function entry point without return
        return unknown_1000_0E4C_10E4C(0);
    }

    public virtual Action unknown_1000_0E4C_10E4C(int loadOffset)
    {
        // CALL 0x1000:0e86 (1000_0E4C / 0x10E4C)
        NearCall(cs1, 0xE4F, unknown_1000_0E86_10E86);
        // JZ 0x1000:0e52 (1000_0E4F / 0x10E4F)
        if (ZeroFlag)
        {
            goto label_1000_0E52_10E52;
        }
        // RET  (1000_0E51 / 0x10E51)
        return NearRet();
    label_1000_0E52_10E52:
        // MOV word ptr [0x52],0x0 (1000_0E52 / 0x10E52)
        UInt16[DS, 0x52] = 0x0;
        // RET  (1000_0E58 / 0x10E58)
        return NearRet();
    }

    public virtual Action unknown_1000_0E59_10E59(int loadOffset)
    {
        // PUSH DS (1000_0E59 / 0x10E59)
        Stack.Push16(DS);
        // LDS SI,[0x56] (1000_0E5A / 0x10E5A)
        SI = UInt16[DS, 0x56];
        DS = UInt16[DS, 0x58];
        // ADD SI,0x2 (1000_0E5E / 0x10E5E)
        // SI += 0x2;
        SI = Alu.Add16(SI, 0x2);
        // LODSW SI (1000_0E61 / 0x10E61)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV DI,AX (1000_0E62 / 0x10E62)
        DI = AX;
        // LODSW SI (1000_0E64 / 0x10E64)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV CX,AX (1000_0E65 / 0x10E65)
        CX = AX;
        // OR CL,CL (1000_0E67 / 0x10E67)
        // CL |= CL;
        CL = Alu.Or8(CL, CL);
        // JZ 0x1000:0e84 (1000_0E69 / 0x10E69)
        if (ZeroFlag)
        {
            goto label_1000_0E84_10E84;
        }
        // TEST DI,0x200 (1000_0E6B / 0x10E6B)
        Alu.And16(DI, 0x200);
        // JZ 0x1000:0e74 (1000_0E6F / 0x10E6F)
        if (ZeroFlag)
        {
            goto label_1000_0E74_10E74;
        }
        // CALL 0x1000:0ebd (1000_0E71 / 0x10E71)
        NearCall(cs1, 0xE74, unknown_1000_0EBD_10EBD);
    label_1000_0E74_10E74:
        // LODSW SI (1000_0E74 / 0x10E74)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV DX,AX (1000_0E75 / 0x10E75)
        DX = AX;
        // LODSW SI (1000_0E77 / 0x10E77)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV BX,AX (1000_0E78 / 0x10E78)
        BX = AX;
        // MOV AX,0xa000 (1000_0E7A / 0x10E7A)
        AX = 0xA000;
        // MOV ES,AX (1000_0E7D / 0x10E7D)
        ES = AX;
        // CALLF 0x1000:0b9a (1000_0E7F / 0x10E7F)
        FarCall(cs1, 0xE84, unknown_1000_0B9A_10B9A);
    label_1000_0E84_10E84:
        // POP DS (1000_0E84 / 0x10E84)
        DS = Stack.Pop16();
        // RET  (1000_0E85 / 0x10E85)
        return NearRet();
    }

    public virtual Action unknown_1000_0E86_10E86(int loadOffset)
    {
    entrydispatcher:
        if (loadOffset != 0)
        {
            throw FailAsUntested("External goto not supported for this function.");
        }
        // PUSH SI (1000_0E86 / 0x10E86)
        Stack.Push16(SI);
        // LES SI,[0x56] (1000_0E87 / 0x10E87)
        SI = UInt16[DS, 0x56];
        ES = UInt16[DS, 0x58];
        // MOV AX,word ptr ES:[SI] (1000_0E8B / 0x10E8B)
        AX = UInt16[ES, SI];
        // ADD SI,AX (1000_0E8E / 0x10E8E)
        // SI += AX;
        SI = Alu.Add16(SI, AX);
        // MOV AX,SI (1000_0E90 / 0x10E90)
        AX = SI;
        // SHR AX,0x1 (1000_0E92 / 0x10E92)
        AX >>= 0x1;
        // SHR AX,0x1 (1000_0E94 / 0x10E94)
        AX >>= 0x1;
        // SHR AX,0x1 (1000_0E96 / 0x10E96)
        AX >>= 0x1;
        // SHR AX,0x1 (1000_0E98 / 0x10E98)
        // AX >>= 0x1;
        AX = Alu.Shr16(AX, 0x1);
        // MOV CX,ES (1000_0E9A / 0x10E9A)
        CX = ES;
        // ADD AX,CX (1000_0E9C / 0x10E9C)
        // AX += CX;
        AX = Alu.Add16(AX, CX);
        // MOV ES,AX (1000_0E9E / 0x10E9E)
        ES = AX;
        // AND SI,0xf (1000_0EA0 / 0x10EA0)
        // SI &= 0xF;
        SI = Alu.And16(SI, 0xF);
        // MOV word ptr [0x56],SI (1000_0EA3 / 0x10EA3)
        UInt16[DS, 0x56] = SI;
        // MOV word ptr [0x58],ES (1000_0EA7 / 0x10EA7)
        UInt16[DS, 0x58] = ES;
        // JMP 0x1000:0eb2 (1000_0EAB / 0x10EAB)
        // Jump converted to entry function call
        if (JumpDispatcher.Jump(split_1000_0EB2_10EB2, 0))
        {
            loadOffset = JumpDispatcher.NextEntryAddress;
            goto entrydispatcher;
        }
        return JumpDispatcher.JumpAsmReturn!;
    }

    public virtual Action unknown_1000_0EAD_10EAD(int loadOffset)
    {
        // PUSH SI (1000_0EAD / 0x10EAD)
        Stack.Push16(SI);
        // LES SI,[0x56] (1000_0EAE / 0x10EAE)
        SI = UInt16[DS, 0x56];
        ES = UInt16[DS, 0x58];
        // Function call generated as ASM continues to next function entry point without return
        return split_1000_0EB2_10EB2(0);
    }

    public virtual Action split_1000_0EB2_10EB2(int loadOffset)
    {
        // LODSW ES:SI (1000_0EB2 / 0x10EB2)
        AX = UInt16[ES, SI];
        SI = (ushort)(SI + Direction16);
        // MOV CX,AX (1000_0EB4 / 0x10EB4)
        CX = AX;
        // SUB CX,0x2 (1000_0EB6 / 0x10EB6)
        // CX -= 0x2;
        CX = Alu.Sub16(CX, 0x2);
        // POP SI (1000_0EB9 / 0x10EB9)
        SI = Stack.Pop16();
        // OR AX,AX (1000_0EBA / 0x10EBA)
        // AX |= AX;
        AX = Alu.Or16(AX, AX);
        // RET  (1000_0EBC / 0x10EBC)
        return NearRet();
    }

    public virtual Action unknown_1000_0EBD_10EBD(int loadOffset)
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
        NearCall(cs1, 0xED0, unknown_1000_0EFE_10EFE);
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

    public virtual Action unknown_1000_0EFE_10EFE(int loadOffset)
    {
    entrydispatcher:
        if (loadOffset != 0)
        {
            throw FailAsUntested("External goto not supported for this function.");
        }
        // PUSH CX (1000_0EFE / 0x10EFE)
        Stack.Push16(CX);
        // PUSH DI (1000_0EFF / 0x10EFF)
        Stack.Push16(DI);
        // PUSH DS (1000_0F00 / 0x10F00)
        Stack.Push16(DS);
        // ADD SI,0x6 (1000_0F01 / 0x10F01)
        SI += 0x6;
        // XOR BP,BP (1000_0F04 / 0x10F04)
        BP = 0;
        // JMP 0x1000:0f30 (1000_0F06 / 0x10F06)
        // Jump converted to entry function call
        if (JumpDispatcher.Jump(split_1000_0F30_10F30, 0))
        {
            loadOffset = JumpDispatcher.NextEntryAddress;
            goto entrydispatcher;
        }
        return JumpDispatcher.JumpAsmReturn!;
    }

    public virtual Action split_1000_0F30_10F30(int loadOffset)
    {
    label_1000_0F30_10F30:
        // SHR BP,0x1 (1000_0F30 / 0x10F30)
        // BP >>= 0x1;
        BP = Alu.Shr16(BP, 0x1);
        // JZ 0x1000:0f39 (1000_0F32 / 0x10F32)
        if (ZeroFlag)
        {
            goto label_1000_0F39_10F39;
        }
        // JNC 0x1000:0f41 (1000_0F34 / 0x10F34)
        if (!CarryFlag)
        {
            goto label_1000_0F41_10F41;
        }
    label_1000_0F36_10F36:
        // MOVSB ES:DI,SI (1000_0F36 / 0x10F36)
        UInt8[ES, DI] = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        DI = (ushort)(DI + Direction8);
        // JMP 0x1000:0f30 (1000_0F37 / 0x10F37)
        goto label_1000_0F30_10F30;
    label_1000_0F39_10F39:
        // LODSW SI (1000_0F39 / 0x10F39)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV BP,AX (1000_0F3A / 0x10F3A)
        BP = AX;
        // STC  (1000_0F3C / 0x10F3C)
        CarryFlag = true;
        // RCR BP,0x1 (1000_0F3D / 0x10F3D)
        BP = Alu.Rcr16(BP, 0x1);
        // JC 0x1000:0f36 (1000_0F3F / 0x10F3F)
        if (CarryFlag)
        {
            goto label_1000_0F36_10F36;
        }
    label_1000_0F41_10F41:
        // XOR CX,CX (1000_0F41 / 0x10F41)
        CX = 0;
        // SHR BP,0x1 (1000_0F43 / 0x10F43)
        // BP >>= 0x1;
        BP = Alu.Shr16(BP, 0x1);
        // JNZ 0x1000:0f4d (1000_0F45 / 0x10F45)
        if (!ZeroFlag)
        {
            goto label_1000_0F4D_10F4D;
        }
        // LODSW SI (1000_0F47 / 0x10F47)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV BP,AX (1000_0F48 / 0x10F48)
        BP = AX;
        // STC  (1000_0F4A / 0x10F4A)
        CarryFlag = true;
        // RCR BP,0x1 (1000_0F4B / 0x10F4B)
        BP = Alu.Rcr16(BP, 0x1);
    label_1000_0F4D_10F4D:
        // JC 0x1000:0f7d (1000_0F4D / 0x10F4D)
        if (CarryFlag)
        {
            goto label_1000_0F7D_10F7D;
        }
        // SHR BP,0x1 (1000_0F4F / 0x10F4F)
        // BP >>= 0x1;
        BP = Alu.Shr16(BP, 0x1);
        // JNZ 0x1000:0f59 (1000_0F51 / 0x10F51)
        if (!ZeroFlag)
        {
            goto label_1000_0F59_10F59;
        }
        // LODSW SI (1000_0F53 / 0x10F53)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV BP,AX (1000_0F54 / 0x10F54)
        BP = AX;
        // STC  (1000_0F56 / 0x10F56)
        CarryFlag = true;
        // RCR BP,0x1 (1000_0F57 / 0x10F57)
        BP = Alu.Rcr16(BP, 0x1);
    label_1000_0F59_10F59:
        // RCL CX,0x1 (1000_0F59 / 0x10F59)
        CX = Alu.Rcl16(CX, 0x1);
        // SHR BP,0x1 (1000_0F5B / 0x10F5B)
        // BP >>= 0x1;
        BP = Alu.Shr16(BP, 0x1);
        // JNZ 0x1000:0f65 (1000_0F5D / 0x10F5D)
        if (!ZeroFlag)
        {
            goto label_1000_0F65_10F65;
        }
        // LODSW SI (1000_0F5F / 0x10F5F)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV BP,AX (1000_0F60 / 0x10F60)
        BP = AX;
        // STC  (1000_0F62 / 0x10F62)
        CarryFlag = true;
        // RCR BP,0x1 (1000_0F63 / 0x10F63)
        BP = Alu.Rcr16(BP, 0x1);
    label_1000_0F65_10F65:
        // RCL CX,0x1 (1000_0F65 / 0x10F65)
        CX = Alu.Rcl16(CX, 0x1);
        // LODSB SI (1000_0F67 / 0x10F67)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // MOV AH,0xff (1000_0F68 / 0x10F68)
        AH = 0xFF;
    label_1000_0F6A_10F6A:
        // ADD AX,DI (1000_0F6A / 0x10F6A)
        // AX += DI;
        AX = Alu.Add16(AX, DI);
        // XCHG AX,SI (1000_0F6C / 0x10F6C)
        (SI, AX) = (AX, SI);
        // MOV BX,DS (1000_0F6D / 0x10F6D)
        BX = DS;
        // MOV DX,ES (1000_0F6F / 0x10F6F)
        DX = ES;
        // MOV DS,DX (1000_0F71 / 0x10F71)
        DS = DX;
        // INC CX (1000_0F73 / 0x10F73)
        CX++;
        // INC CX (1000_0F74 / 0x10F74)
        CX = Alu.Inc16(CX);
        // REP
        while (CX != 0)
        {
            CX--;
            // MOVSB ES:DI,SI (1000_0F75 / 0x10F75)
            UInt8[ES, DI] = UInt8[DS, SI];
            SI = (ushort)(SI + Direction8);
            DI = (ushort)(DI + Direction8);
        }
        // MOV DS,BX (1000_0F77 / 0x10F77)
        DS = BX;
        // MOV SI,AX (1000_0F79 / 0x10F79)
        SI = AX;
        // JMP 0x1000:0f30 (1000_0F7B / 0x10F7B)
        goto label_1000_0F30_10F30;
    label_1000_0F7D_10F7D:
        // LODSW SI (1000_0F7D / 0x10F7D)
        AX = UInt16[DS, SI];
        SI = (ushort)(SI + Direction16);
        // MOV CL,AL (1000_0F7E / 0x10F7E)
        CL = AL;
        // SHR AX,0x1 (1000_0F80 / 0x10F80)
        AX >>= 0x1;
        // SHR AX,0x1 (1000_0F82 / 0x10F82)
        AX >>= 0x1;
        // SHR AX,0x1 (1000_0F84 / 0x10F84)
        // AX >>= 0x1;
        AX = Alu.Shr16(AX, 0x1);
        // OR AH,0xe0 (1000_0F86 / 0x10F86)
        AH |= 0xE0;
        // AND CL,0x7 (1000_0F89 / 0x10F89)
        // CL &= 0x7;
        CL = Alu.And8(CL, 0x7);
        // JNZ 0x1000:0f6a (1000_0F8C / 0x10F8C)
        if (!ZeroFlag)
        {
            goto label_1000_0F6A_10F6A;
        }
        // MOV BX,AX (1000_0F8E / 0x10F8E)
        BX = AX;
        // LODSB SI (1000_0F90 / 0x10F90)
        AL = UInt8[DS, SI];
        SI = (ushort)(SI + Direction8);
        // MOV CL,AL (1000_0F91 / 0x10F91)
        CL = AL;
        // MOV AX,BX (1000_0F93 / 0x10F93)
        AX = BX;
        // OR CL,CL (1000_0F95 / 0x10F95)
        // CL |= CL;
        CL = Alu.Or8(CL, CL);
        // JNZ 0x1000:0f6a (1000_0F97 / 0x10F97)
        if (!ZeroFlag)
        {
            goto label_1000_0F6A_10F6A;
        }
        // STC  (1000_0F99 / 0x10F99)
        CarryFlag = true;
        // MOV CX,DI (1000_0F9A / 0x10F9A)
        CX = DI;
        // POP DS (1000_0F9C / 0x10F9C)
        DS = Stack.Pop16();
        // POP DI (1000_0F9D / 0x10F9D)
        DI = Stack.Pop16();
        // ADD SP,0x2 (1000_0F9E / 0x10F9E)
        SP += 0x2;
        // SUB CX,DI (1000_0FA1 / 0x10FA1)
        // CX -= DI;
        CX = Alu.Sub16(CX, DI);
        // RET  (1000_0FA3 / 0x10FA3)
        return NearRet();
    }

    public virtual Action unknown_1000_0FA4_10FA4(int loadOffset)
    {
        // LES SI,[0x56] (1000_0FA4 / 0x10FA4)
        SI = UInt16[DS, 0x56];
        ES = UInt16[DS, 0x58];
        // ADD SI,0x2 (1000_0FA8 / 0x10FA8)
        // SI += 0x2;
        SI = Alu.Add16(SI, 0x2);
    label_1000_0FAB_10FAB:
        // LODSW ES:SI (1000_0FAB / 0x10FAB)
        AX = UInt16[ES, SI];
        SI = (ushort)(SI + Direction16);
        // CMP AL,0xff (1000_0FAD / 0x10FAD)
        Alu.Sub8(AL, 0xFF);
        // JZ 0x1000:0fcb (1000_0FAF / 0x10FAF)
        if (ZeroFlag)
        {
            // JZ target is RET, inlining.
            // RET  (1000_0FCB / 0x10FCB)
            return NearRet();
        }
        // XOR CX,CX (1000_0FB1 / 0x10FB1)
        CX = 0;
        // XOR BX,BX (1000_0FB3 / 0x10FB3)
        BX = 0;
        // MOV BL,AL (1000_0FB5 / 0x10FB5)
        BL = AL;
        // MOV CL,AH (1000_0FB7 / 0x10FB7)
        CL = AH;
        // MOV DX,SI (1000_0FB9 / 0x10FB9)
        DX = SI;
        // ADD SI,CX (1000_0FBB / 0x10FBB)
        SI += CX;
        // ADD SI,CX (1000_0FBD / 0x10FBD)
        SI += CX;
        // ADD SI,CX (1000_0FBF / 0x10FBF)
        // SI += CX;
        SI = Alu.Add16(SI, CX);
        // MOV AX,0x1012 (1000_0FC1 / 0x10FC1)
        AX = 0x1012;
        // INT 0x10 (1000_0FC4 / 0x10FC4)
        Interrupt(0x10);
        // CALL 0x1000:0fcc (1000_0FC6 / 0x10FC6)
        NearCall(cs1, 0xFC9, unknown_1000_0FCC_10FCC);
        // JMP 0x1000:0fab (1000_0FC9 / 0x10FC9)
        goto label_1000_0FAB_10FAB;
    }

    public virtual Action unknown_1000_0FCC_10FCC(int loadOffset)
    {
        // PUSH SI (1000_0FCC / 0x10FCC)
        Stack.Push16(SI);
        // PUSH DI (1000_0FCD / 0x10FCD)
        Stack.Push16(DI);
        // MOV SI,DX (1000_0FCE / 0x10FCE)
        SI = DX;
        // MOV DI,0x70 (1000_0FD0 / 0x10FD0)
        DI = 0x70;
        // ADD DI,BX (1000_0FD3 / 0x10FD3)
        DI += BX;
        // ADD DI,BX (1000_0FD5 / 0x10FD5)
        DI += BX;
        // ADD DI,BX (1000_0FD7 / 0x10FD7)
        // DI += BX;
        DI = Alu.Add16(DI, BX);
        // MOV AX,CX (1000_0FD9 / 0x10FD9)
        AX = CX;
        // SHL CX,0x1 (1000_0FDB / 0x10FDB)
        CX <<= 0x1;
        // ADD CX,AX (1000_0FDD / 0x10FDD)
        // CX += AX;
        CX = Alu.Add16(CX, AX);
    label_1000_0FDF_10FDF:
        // LODSB ES:SI (1000_0FDF / 0x10FDF)
        AL = UInt8[ES, SI];
        SI = (ushort)(SI + Direction8);
        // MOV byte ptr CS:[DI],AL (1000_0FE1 / 0x10FE1)
        UInt8[cs1, DI] = AL;
        // INC DI (1000_0FE4 / 0x10FE4)
        DI = Alu.Inc16(DI);
        // LOOP 0x1000:0fdf (1000_0FE5 / 0x10FE5)
        if (--CX != 0)
        {
            goto label_1000_0FDF_10FDF;
        }
        // POP DI (1000_0FE7 / 0x10FE7)
        DI = Stack.Pop16();
        // POP SI (1000_0FE8 / 0x10FE8)
        SI = Stack.Pop16();
        // RET  (1000_0FE9 / 0x10FE9)
        return NearRet();
    }

    public virtual Action unknown_1000_0FEA_10FEA(int loadOffset)
    {
        // STI  (1000_0FEA / 0x10FEA)
        InterruptFlag = true;
        // PUSH AX (1000_0FEB / 0x10FEB)
        Stack.Push16(AX);
        // XOR AX,AX (1000_0FEC / 0x10FEC)
        AX = 0;
        // MOV ES,AX (1000_0FEE / 0x10FEE)
        ES = AX;
        // PUSH word ptr ES:[0x46c] (1000_0FF0 / 0x10FF0)
        Stack.Push16(UInt16[ES, 0x46C]);
        // CALL BP (1000_0FF5 / 0x10FF5)
        // Indirect call to BP, generating possible targets from emulator records
        uint targetAddress_1000_0FF5 = (uint)(BP);
        switch (targetAddress_1000_0FF5)
        {
            case 0xE46: NearCall(cs1, 0xFF7, unknown_1000_0E46_10E46); break;
            default:
                throw FailAsUntested("Error: Function not registered at address " + ConvertUtils.ToHex32WithoutX(targetAddress_1000_0FF5));
        }
        // POP BX (1000_0FF7 / 0x10FF7)
        BX = Stack.Pop16();
        // POP BP (1000_0FF8 / 0x10FF8)
        BP = Stack.Pop16();
        // SHR BP,0x1 (1000_0FF9 / 0x10FF9)
        BP >>= 0x1;
        // SHR BP,0x1 (1000_0FFB / 0x10FFB)
        BP >>= 0x1;
        // SHR BP,0x1 (1000_0FFD / 0x10FFD)
        // BP >>= 0x1;
        BP = Alu.Shr16(BP, 0x1);
        // MOV AX,BP (1000_0FFF / 0x10FFF)
        AX = BP;
        // SHR AX,0x1 (1000_1001 / 0x11001)
        AX >>= 0x1;
        // SHR AX,0x1 (1000_1003 / 0x11003)
        AX >>= 0x1;
        // SUB BP,AX (1000_1005 / 0x11005)
        BP -= AX;
    label_1000_1007_11007:
        // XOR AX,AX (1000_1007 / 0x11007)
        AX = 0;
        // MOV ES,AX (1000_1009 / 0x11009)
        ES = AX;
        CheckExternalEvents(cs1, 0x100F);
        // MOV AX,ES:[0x46c] (1000_100B / 0x1100B)
        AX = UInt16[ES, 0x46C];
        // SUB AX,BX (1000_100F / 0x1100F)
        AX -= BX;
        // CMP AX,BP (1000_1011 / 0x11011)
        Alu.Sub16(AX, BP);
        // JC 0x1000:1007 (1000_1013 / 0x11013)
        if (CarryFlag)
        {
            goto label_1000_1007_11007;
        }
        // CALL 0x1000:1085 (1000_1015 / 0x11015)
        NearCall(cs1, 0x1018, unknown_1000_1085_11085);
        // RET  (1000_1018 / 0x11018)
        return NearRet();
    }

    public virtual Action unknown_1000_1019_11019(int loadOffset)
    {
        // PUSHF  (1000_1019 / 0x11019)
        Stack.Push16(FlagRegister16);
        // PUSHF  (1000_101A / 0x1101A)
        Stack.Push16(FlagRegister16);
        // XOR AX,AX (1000_101B / 0x1101B)
        AX = 0;
        // PUSH AX (1000_101D / 0x1101D)
        Stack.Push16(AX);
        // POPF  (1000_101E / 0x1101E)
        FlagRegister16 = Stack.Pop16();
        // PUSHF  (1000_101F / 0x1101F)
        Stack.Push16(FlagRegister16);
        // POP AX (1000_1020 / 0x11020)
        AX = Stack.Pop16();
        // POPF  (1000_1021 / 0x11021)
        FlagRegister16 = Stack.Pop16();
        // AND AX,0xf000 (1000_1022 / 0x11022)
        AX &= 0xF000;
        // CMP AX,0xf000 (1000_1025 / 0x11025)
        Alu.Sub16(AX, 0xF000);
        // JZ 0x1000:1039 (1000_1028 / 0x11028)
        if (ZeroFlag)
        {
            goto label_1000_1039_11039;
        }
        // MOV AX,0x7000 (1000_102A / 0x1102A)
        AX = 0x7000;
        // PUSH AX (1000_102D / 0x1102D)
        Stack.Push16(AX);
        // POPF  (1000_102E / 0x1102E)
        FlagRegister16 = Stack.Pop16();
        // PUSHF  (1000_102F / 0x1102F)
        Stack.Push16(FlagRegister16);
        // POP AX (1000_1030 / 0x11030)
        AX = Stack.Pop16();
        // AND AX,0x7000 (1000_1031 / 0x11031)
        // AX &= 0x7000;
        AX = Alu.And16(AX, 0x7000);
        // MOV byte ptr CS:[0x6b],AH (1000_1034 / 0x11034)
        UInt8[cs1, 0x6B] = AH;
    label_1000_1039_11039:
        // POPF  (1000_1039 / 0x11039)
        FlagRegister16 = Stack.Pop16();
        // RET  (1000_103A / 0x1103A)
        return NearRet();
    }

    public virtual Action unknown_1000_105F_1105F(int loadOffset)
    {
        // MOV DI,DX (1000_105F / 0x1105F)
        DI = DX;
    label_1000_1061_11061:
        // MOV AL,byte ptr [DI] (1000_1061 / 0x11061)
        AL = UInt8[DS, DI];
        // CMP AL,0x2e (1000_1063 / 0x11063)
        Alu.Sub8(AL, 0x2E);
        // JZ 0x1000:106e (1000_1065 / 0x11065)
        if (ZeroFlag)
        {
            // JZ target is RET, inlining.
            // RET  (1000_106E / 0x1106E)
            return NearRet();
        }
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
        goto label_1000_1061_11061;
        // RET  (1000_106E / 0x1106E)
    }

    public virtual Action unknown_1000_1085_11085(int loadOffset)
    {
        // MOV AH,0x1 (1000_1085 / 0x11085)
        AH = 0x1;
        // INT 0x16 (1000_1087 / 0x11087)
        Interrupt(0x16);
        // JZ 0x1000:1091 (1000_1089 / 0x11089)
        if (ZeroFlag)
        {
            // JZ target is RET, inlining.
            // RET  (1000_1091 / 0x11091)
            return NearRet();
        }
        // XOR AH,AH (1000_108B / 0x1108B)
        AH = 0;
        // INT 0x16 (1000_108D / 0x1108D)
        Interrupt(0x16);
        // OR AX,AX (1000_108F / 0x1108F)
        // AX |= AX;
        AX = Alu.Or16(AX, AX);
        // RET  (1000_1091 / 0x11091)
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_109A_1109A(int loadOffset)
    {
        while (true)
        {
            Stack.Push16(DS);
            AX = ES;
            DS = AX;
            CX = 0x8000;
            DX = DI;
            AH = 0x3F;
            Interrupt(0x21);
            DS = Stack.Pop16();
            if (!CarryFlag)
            {
                if (CarryFlag || ZeroFlag)
                {
                    AX = ES;
                    AX = Alu.Add16(AX, 0x800);
                    ES = AX;
                }
                else
                {
                    break;
                }
            }
            else
            {
                goto label_4;
            }
        }
        CX = AX;
        CarryFlag = false;
    label_4:
        Stack.Push16(FlagRegister16);
        AH = 0x3E;
        Interrupt(0x21);
        FlagRegister16 = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_10F4_110F4(int loadOffset)
    {
        Stack.Push16(DS);
        Stack.Push16(ES);
        DS = Stack.Pop16();
        ES = Stack.Pop16();
        DirectionFlag = false;
        SI = 0x80;
        AL = UInt8[DS, SI];
        SI += (ushort)Direction8;
        CX = 0;
        CL = AL;
        if (CX != 0)
        {
            ushort num1;
            do
            {
                DI = UInt16[ES, BX];
                ++BX;
                BX = Alu.Inc16(BX);
                DI = Alu.Or16(DI, DI);
                if (!ZeroFlag)
                {
                    ushort num2;
                    do
                    {
                        if (ZeroFlag)
                        {
                            SI = Alu.Inc16(SI);
                            num2 = --CX;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (num2 != 0);
                    ushort num4;
                    do
                    {
                        AL = UInt8[DS, SI];
                        SI += (ushort)Direction8;
                        if (!ZeroFlag)
                        {
                            UInt8[ES, DI] = AL;
                            DI += (ushort)Direction8;
                            num4 = --CX;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (num4 != 0);
                    AL = 0;
                    UInt8[ES, DI] = AL;
                    DI += (ushort)Direction8;
                    if (CX != 0)
                    {
                        num1 = --CX;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            while (num1 != 0);
        }
        AX = 0x111C;
        DS = AX;
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_11BD_111BD(int loadOffset)
    {
        if (CarryFlag || (!CarryFlag && !ZeroFlag))
        {
            return NearRet();
        }
        AL = Alu.And8(AL, 0xDF);
        return NearRet();
    }
}