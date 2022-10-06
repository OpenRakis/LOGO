namespace logo;

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

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_09B5_109B5(int loadOffset)
    {
        // MOV AH,AL (1000_09B5 / 0x109B5)
        AH = AL;
        // XOR SI,SI (1000_09B7 / 0x109B7)
        SI = 0;
        // MOV BX,word ptr ES:[BP + 0x0] (1000_09B9 / 0x109B9)
        BX = UInt16[ES, BP];
        do
        {
            // INC SI (1000_09BD / 0x109BD)
            SI = Alu.Inc16(SI);
            // JNZ 0x1000:09c1 (1000_09BE / 0x109BE)
            if (!ZeroFlag)
            {
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
                    // STC  (1000_09D6 / 0x109D6)
                    CarryFlag = true;
                    // RET  (1000_09D7 / 0x109D7)
                    return NearRet();
                }
            }
            // DEC SI (1000_09C0 / 0x109C0)
            SI = Alu.Dec16(SI);
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
        }
        while (CarryFlag);
        // RET  (1000_09D5 / 0x109D5)
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

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0A22_10A22(int loadOffset)
    {
        // CMP BX,0xc8 (1000_0A22 / 0x10A22)
        Alu.Sub16(BX, 0xC8);
        // JC 0x1000:0a2b (1000_0A26 / 0x10A26)
        if (CarryFlag)
        {
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
        }
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

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0A51_10A51(int loadOffset)
    {
        // PUSH DS (1000_0A51 / 0x10A51)
        Stack.Push16(DS);
        AX = 0xA000;
        DS = AX;
        ES = AX;
        // XOR SI,SI
        SI = 0;
        // XOR DI,DI
        DI = 0;
        CX = 0xFA00;
        ushort num1;
        do
        {
            AL = UInt8[DS, SI];
            SI += (ushort)Direction8;
            if (CarryFlag)
                AL = 0;
            UInt8[ES, DI] = AL;
            DI += (ushort)Direction8;
            num1 = --CX;
        }
        while (num1 != 0);
        DS = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0B9A_10B9A(int loadOffset)
    {
        if (loadOffset == 0)
        {
            goto label_60;
        }

    label_2:
        this.BP = this.DX;
        this.DI -= (ushort)(uint)this.BP;
        this.DI = this.Alu.Add16(this.DI, (ushort)320);
    label_3:
        do
        {
            do
            {
                this.AL = this.UInt8[this.DS, this.SI];
                this.SI += (ushort)(uint)this.Direction8;
                this.AL = this.Alu.Or8(this.AL, this.AL);
                if (!this.SignFlag)
                {
                    this.CX = this.AX;
                    this.CH = (byte)0;
                    ++this.CX;
                    this.BP = this.Alu.Sub16(this.BP, this.CX);
                    ushort num = 1;
                    do
                    {
                        this.AL = this.UInt8[this.DS, this.SI];
                        this.SI += (ushort)(uint)this.Direction8;
                        this.AL = this.Alu.Or8(this.AL, this.AL);
                        if (!this.ZeroFlag)
                        {
                            this.UInt8[this.ES, this.DI] = this.AL;
                            this.DI += (ushort)(uint)this.Direction8;
                            if (--this.CX == (ushort)0)
                            {
                                this.BP = this.Alu.Or16(this.BP, this.BP);
                                if (this.CarryFlag || this.ZeroFlag)
                                {
                                    this.BX = this.Alu.Dec16(this.BX);
                                    if (!this.ZeroFlag)
                                    {
                                        goto label_2;
                                    }
                                    else
                                    {
                                        goto label_9;
                                    }
                                }
                                else
                                {
                                    goto label_3;
                                }
                            }
                        }
                        else
                        {
                            this.DI = this.Alu.Inc16(this.DI);
                            num = --this.CX;
                        }
                    }
                    while (num != (ushort)0);
                    this.BP = this.Alu.Or16(this.BP, this.BP);
                    if (this.CarryFlag || this.ZeroFlag)
                    {
                        this.BX = this.Alu.Dec16(this.BX);
                        if (this.ZeroFlag)
                            goto label_9;
                        else
                            goto label_2;
                    }
                }
                else
                {
                    this.CX = (ushort)0x101;
                    this.AH = (byte)0;
                    this.CX -= (ushort)(uint)this.AX;
                    this.BP = this.Alu.Sub16(this.BP, this.CX);
                    this.AL = this.UInt8[this.DS, this.SI];
                    this.SI += (ushort)(uint)this.Direction8;
                    this.AL = this.Alu.Or8(this.AL, this.AL);
                    if (!this.ZeroFlag)
                    {
                        while (this.CX != (ushort)0)
                        {
                            --this.CX;
                            this.UInt8[this.ES, this.DI] = this.AL;
                            this.DI += (ushort)(uint)this.Direction8;
                        }
                        this.BP = this.Alu.Or16(this.BP, this.BP);
                    }
                    else
                    {
                        goto label_18;
                    }
                }
            }
            while (!this.CarryFlag && !this.ZeroFlag);
            this.BX = this.Alu.Dec16(this.BX);
            if (this.ZeroFlag)
            {
                goto label_9;
            }
            else
            {
                goto label_2;
            }

        label_18:
            this.DI = this.Alu.Add16(this.DI, this.CX);
            this.BP = this.Alu.Or16(this.BP, this.BP);
        }
        while (!this.CarryFlag && !this.ZeroFlag);
        this.BX = this.Alu.Dec16(this.BX);
        if (!this.ZeroFlag)
            goto label_2;
        label_9:
        this.DirectionFlag = false;
        this.UInt8[this.cs1, (ushort)0xA71] = (byte)0xC7;
        this.UInt8[this.cs1, (ushort)0xB2F] = (byte)0xC7;
        return this.FarRet();
    label_60:
        int num1 = (int)this.Alu.Sub8(this.CH, (byte)0xFE);
        if (this.CarryFlag)
            return this.FarRet();
        this.DI = this.Alu.Or16(this.DI, this.DI);
        if (!this.SignFlag)
        {
            this.BP = this.DI;
            this.BP = this.Alu.And16(this.BP, (ushort)511);
            this.AX = this.DI;
            this.NearCall(this.cs1, (ushort)0xBAF, new Func<int, Action>(this.unknown_1000_0A22_10A22));
            this.BX = this.CX;
            this.BH = (byte)0;
            int num2 = (int)this.Alu.Sub8(this.CH, byte.MaxValue);
            if (!this.ZeroFlag)
            {
                this.BP = this.Alu.Shr16(this.BP, 1);
                this.AX = this.DI;
                if (!this.CarryFlag)
                {
                    do
                    {
                        this.CX = this.BP;
                        this.DI = this.AX;
                        while (this.CX != (ushort)0)
                        {
                            --this.CX;
                            this.UInt16[this.ES, this.DI] = this.UInt16[this.DS, this.SI];
                            this.SI += (ushort)(uint)this.Direction16;
                            this.DI += (ushort)(uint)this.Direction16;
                        }
                        this.AX += (ushort)320;
                        this.BX = this.Alu.Dec16(this.BX);
                    }
                    while (!this.ZeroFlag);
                    return this.FarRet();
                }
                do
                {
                    this.CX = this.BP;
                    this.DI = this.AX;
                    while (this.CX != (ushort)0)
                    {
                        --this.CX;
                        this.UInt16[this.ES, this.DI] = this.UInt16[this.DS, this.SI];
                        this.SI += (ushort)(uint)this.Direction16;
                        this.DI += (ushort)(uint)this.Direction16;
                    }
                    this.UInt8[this.ES, this.DI] = this.UInt8[this.DS, this.SI];
                    this.SI += (ushort)(uint)this.Direction8;
                    this.DI += (ushort)(uint)this.Direction8;
                    this.AX += (ushort)320;
                    this.BX = this.Alu.Dec16(this.BX);
                }
                while (!this.ZeroFlag);
                return this.FarRet();
            }
            this.DX = this.DI;
        label_76:
            do
            {
                this.CX = this.BP;
                this.DI = this.DX;
                ushort num3 = 1;
                do
                {
                    this.AL = this.UInt8[this.DS, this.SI];
                    this.SI += (ushort)(uint)this.Direction8;
                    this.AL = this.Alu.Or8(this.AL, this.AL);
                    if (!this.ZeroFlag)
                    {
                        this.UInt8[this.ES, this.DI] = this.AL;
                        this.DI += (ushort)(uint)this.Direction8;
                        if (--this.CX == (ushort)0)
                        {
                            this.DX += (ushort)320;
                            this.BX = this.Alu.Dec16(this.BX);
                            if (this.ZeroFlag)
                                return this.FarRet();
                            goto label_76;
                        }
                    }
                    else
                    {
                        this.DI = this.Alu.Inc16(this.DI);
                        num3 = --this.CX;
                    }
                }
                while (num3 != (ushort)0);
                this.DX += (ushort)320;
                this.BX = this.Alu.Dec16(this.BX);
            }
            while (!this.ZeroFlag);
            return this.FarRet();
        }
        this.BP = this.DI;
        this.BP = this.Alu.And16(this.BP, (ushort)511);
        this.AX = this.DI;
        this.NearCall(this.cs1, (ushort)0xC05, new Func<int, Action>(this.unknown_1000_0A22_10A22));
        this.BX = this.CX;
        this.BH = (byte)0;
        int num4 = (int)this.Alu.And16(this.AX, (ushort)16384);
        if (this.ZeroFlag)
        {
            int num5 = (int)this.Alu.And16(this.AX, (ushort)8192);
            if (!this.ZeroFlag)
            {
                this.UInt8[this.cs1, (ushort)0xA71] = (byte)0xEF;
                this.UInt8[this.cs1, (ushort)0xB2F] = (byte)0xEF;
                this.AH = this.BL;
                this.AH = this.Alu.Dec8(this.AH);
                this.DH = this.AH;
                this.DL = (byte)0;
                this.AL = this.DL;
                this.DX >>= 1;
                this.DX >>= 1;
                this.DI += (ushort)(uint)this.AX;
                this.DI = this.Alu.Add16(this.DI, this.DX);
            }
            this.DX = this.BP;
            int num6 = (int)this.Alu.Sub8(this.CH, byte.MaxValue);
            if (!this.ZeroFlag)
            {
                while (true)
                {
                    do
                    {
                        this.AL = this.UInt8[this.DS, this.SI];
                        this.SI += (ushort)(uint)this.Direction8;
                        this.AL = this.Alu.Or8(this.AL, this.AL);
                        if (!this.SignFlag)
                        {
                            this.CX = this.AX;
                            this.CH = (byte)0;
                            ++this.CX;
                            this.BP = this.Alu.Sub16(this.BP, this.CX);
                            while (this.CX != (ushort)0)
                            {
                                --this.CX;
                                this.UInt8[this.ES, this.DI] = this.UInt8[this.DS, this.SI];
                                this.SI += (ushort)(uint)this.Direction8;
                                this.DI += (ushort)(uint)this.Direction8;
                            }
                            if (this.CarryFlag || this.ZeroFlag)
                            {
                                this.BX = this.Alu.Dec16(this.BX);
                                if (this.ZeroFlag)
                                {
                                    goto label_9;
                                }
                                else
                                {
                                    goto label_37;
                                }
                            }
                        }
                        else
                        {
                            this.CX = (ushort)0x101;
                            this.AH = (byte)0;
                            this.CX -= (ushort)(uint)this.AX;
                            this.BP = this.Alu.Sub16(this.BP, this.CX);
                            this.AL = this.UInt8[this.DS, this.SI];
                            this.SI += (ushort)(uint)this.Direction8;
                            while (this.CX != (ushort)0)
                            {
                                --this.CX;
                                this.UInt8[this.ES, this.DI] = this.AL;
                                this.DI += (ushort)(uint)this.Direction8;
                            }
                        }
                    }
                    while (!this.CarryFlag && !this.ZeroFlag);
                    goto label_48;
                label_37:
                    this.BP = this.DX;
                    this.DI -= (ushort)(uint)this.BP;
                    this.DI = this.Alu.Add16(this.DI, (ushort)320);
                    continue;
                label_48:
                    this.BX = this.Alu.Dec16(this.BX);
                    if (this.ZeroFlag)
                    {
                        goto label_9;
                    }
                    else
                    {
                        goto label_37;
                    }
                }
            }
            else
            {
                goto label_3;
            }
        }
        else
        {
            int num7 = (int)this.Alu.And16(this.AX, (ushort)8192);
            if (!this.ZeroFlag)
            {
                this.UInt8[this.cs1, (ushort)0xAD2] = (byte)0xEF;
                this.UInt8[this.cs1, (ushort)0xB61] = (byte)0xEF;
                this.AH = this.BL;
                this.AH = this.Alu.Dec8(this.AH);
                this.DH = this.AH;
                this.DL = (byte)0;
                this.AL = this.DL;
                this.DX >>= 1;
                this.DX >>= 1;
                this.DI += (ushort)(uint)this.AX;
                this.DI += (ushort)(uint)this.DX;
            }
            this.DI += (ushort)(uint)this.BP;
            this.DI = this.Alu.Dec16(this.DI);
            this.DirectionFlag = true;
            this.DX = this.BP;
            int num8 = (int)this.Alu.Sub8(this.CH, byte.MaxValue);
            if (this.ZeroFlag)
            {
            label_21:
                while (true)
                {
                    do
                    {
                        do
                        {
                            this.AL = this.UInt8[this.DS, this.SI];
                            this.SI = this.Alu.Inc16(this.SI);
                            this.AL = this.Alu.Or8(this.AL, this.AL);
                            if (!this.ZeroFlag)
                            {
                                this.CX = this.AX;
                                this.CH = (byte)0;
                                ++this.CX;
                                this.BP = this.Alu.Sub16(this.BP, this.CX);
                                ushort num9 = 1;
                                do
                                {
                                    this.AL = this.UInt8[this.DS, this.SI];
                                    this.SI = this.Alu.Inc16(this.SI);
                                    this.AL = this.Alu.Or8(this.AL, this.AL);
                                    if (!this.ZeroFlag)
                                    {
                                        this.UInt8[this.ES, this.DI] = this.AL;
                                        this.DI += (ushort)(uint)this.Direction8;
                                        if (--this.CX == (ushort)0)
                                        {
                                            this.BP = this.Alu.Or16(this.BP, this.BP);
                                            if (this.CarryFlag || this.ZeroFlag)
                                            {
                                                this.BX = this.Alu.Dec16(this.BX);
                                                if (this.ZeroFlag)
                                                {
                                                    goto label_9;
                                                }
                                                else
                                                {
                                                    goto label_20;
                                                }
                                            }
                                            else
                                            {
                                                goto label_21;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.DI = this.Alu.Dec16(this.DI);
                                        num9 = --this.CX;
                                    }
                                }
                                while (num9 != (ushort)0);
                                this.BP = this.Alu.Or16(this.BP, this.BP);
                                if (this.CarryFlag || this.ZeroFlag)
                                {
                                    this.BX = this.Alu.Dec16(this.BX);
                                    if (this.ZeroFlag)
                                    {
                                        goto label_9;
                                    }
                                    else
                                    {
                                        goto label_20;
                                    }
                                }
                            }
                            else
                            {
                                this.CX = (ushort)0x101;
                                this.AH = (byte)0;
                                this.CX -= (ushort)(uint)this.AX;
                                this.BP = this.Alu.Sub16(this.BP, this.CX);
                                this.AL = this.UInt8[this.DS, this.SI];
                                this.SI = this.Alu.Inc16(this.SI);
                                this.AL = this.Alu.Or8(this.AL, this.AL);
                                if (!this.ZeroFlag)
                                {
                                    while (this.CX != (ushort)0)
                                    {
                                        --this.CX;
                                        this.UInt8[this.ES, this.DI] = this.AL;
                                        this.DI += (ushort)(uint)this.Direction8;
                                    }
                                    this.BP = this.Alu.Or16(this.BP, this.BP);
                                }
                                else
                                {
                                    goto label_35;
                                }
                            }
                        }
                        while (!this.CarryFlag && !this.ZeroFlag);
                        this.BX = this.Alu.Dec16(this.BX);
                        if (this.ZeroFlag)
                        {
                            goto label_9;
                        }
                        else
                        {
                            goto label_20;
                        }

                    label_35:
                        this.DI = this.Alu.Sub16(this.DI, this.CX);
                        this.BP = this.Alu.Or16(this.BP, this.BP);
                    }
                    while (!this.CarryFlag && !this.ZeroFlag);
                    goto label_36;
                label_20:
                    this.BP = this.DX;
                    this.DI += (ushort)(uint)this.BP;
                    this.DI = this.Alu.Add16(this.DI, (ushort)320);
                    continue;
                label_36:
                    this.BX = this.Alu.Dec16(this.BX);
                    if (this.ZeroFlag)
                    {
                        goto label_9;
                    }
                    else
                    {
                        goto label_20;
                    }
                }
            }
            else
            {
                while (true)
                {
                    do
                    {
                        this.AL = this.UInt8[this.DS, this.SI];
                        this.SI = this.Alu.Inc16(this.SI);
                        this.AL = this.Alu.Or8(this.AL, this.AL);
                        if (!this.ZeroFlag)
                        {
                            this.CX = this.AX;
                            this.CH = (byte)0;
                            ++this.CX;
                            this.BP = this.Alu.Sub16(this.BP, this.CX);
                            ushort num10;
                            do
                            {
                                this.AL = this.UInt8[this.DS, this.SI];
                                this.SI = this.Alu.Inc16(this.SI);
                                this.UInt8[this.ES, this.DI] = this.AL;
                                this.DI += (ushort)(uint)this.Direction8;
                                num10 = --this.CX;
                            }
                            while (num10 != (ushort)0);
                            this.BP = this.Alu.Or16(this.BP, this.BP);
                            if (this.CarryFlag || this.ZeroFlag)
                            {
                                this.BX = this.Alu.Dec16(this.BX);
                                if (this.ZeroFlag)
                                {
                                    goto label_9;
                                }
                                else
                                {
                                    goto label_49;
                                }
                            }
                        }
                        else
                        {
                            this.CX = 0x101;
                            this.AH = 0;
                            this.CX -= (ushort)(uint)this.AX;
                            this.BP = this.Alu.Sub16(this.BP, this.CX);
                            this.AL = this.UInt8[this.DS, this.SI];
                            this.SI = this.Alu.Inc16(this.SI);
                            while (this.CX != (ushort)0)
                            {
                                --this.CX;
                                this.UInt8[this.ES, this.DI] = this.AL;
                                this.DI += (ushort)(uint)this.Direction8;
                            }
                            this.BP = this.Alu.Or16(this.BP, this.BP);
                        }
                    }
                    while (!this.CarryFlag && !this.ZeroFlag);
                    goto label_59;
                label_49:
                    this.BP = this.DX;
                    this.DI += (ushort)(uint)this.BP;
                    this.DI = this.Alu.Add16(this.DI, (ushort)320);
                    continue;
                label_59:
                    this.BX = this.Alu.Dec16(this.BX);
                    if (this.ZeroFlag)
                    {
                        goto label_9;
                    }
                    else
                    {
                        goto label_49;
                    }
                }
            }
        }
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0C72_10C72(int loadOffset)
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
    public virtual Action unknown_1000_0CF4_10CF4(int loadOffset)
    {
        NearCall(cs1, 0xCF7, new Func<int, Action>(unknown_1000_0C72_10C72));
        SI = 0xCBC;
        AX = UInt16[cs1, SI];
        UInt16[cs1, 0xCB6] = AX;
        UInt16[cs1, 0xCBA] = SI;
        AX = 0;
        UInt16[cs1, 0xCB0] = AX;
        UInt16[cs1, 0xCB2] = AX;
        UInt16[cs1, 0xCB4] = AX;
        CX = 0xFB;
        ushort num;
        do
        {
            Stack.Push16(CX);
            NearCall(cs1, 0xD1B, new Func<int, Action>(unknown_1000_0D22_10D22));
            CX = Stack.Pop16();
            NearCall(cs1, 0xD1F, new Func<int, Action>(unknown_1000_1085_11085));
            num = --CX;
        }
        while (num != 0 && ZeroFlag);
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0D22_10D22(int loadOffset)
    {
        Stack.Push16(DS);
        Stack.Push16(ES);
        Stack.Push16(cs1);
        Stack.Push16(cs1);
        DS = Stack.Pop16();
        ES = Stack.Pop16();
        int num1 = Alu.Sub16(UInt16[cs1, 0xCB6], 0);
        if (!SignFlag)
        {
            DI = 0x160;
            SI = DI;
            CX = 0xF0;
            DX = UInt16[cs1, 0xCB8];
            AX = DX;
            AX <<= 1;
            AX += DX;
            SI += AX;
            CX = Alu.Sub16(CX, AX);
            while (CX != 0)
            {
                --CX;
                UInt8[ES, DI] = UInt8[DS, SI];
                SI += (ushort)Direction8;
                DI += (ushort)Direction8;
            }
            CX = DX;
            ushort num2;
            do
            {
                NearCall(cs1, 0xD4E, new Func<int, Action>(unknown_1000_0D5F_10D5F));
                num2 = --CX;
            }
            while (num2 != 0);
            DX = 0x160;
            BX = 0x50;
            CX = 0x50;
            NearCall(cs1, 0xD5C, new Func<int, Action>(unknown_1000_09D8_109D8));
        }
        ES = Stack.Pop16();
        DS = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0D5F_10D5F(int loadOffset)
    {
        SI = UInt16[cs1, 0xCBA];
        UInt16[cs1, 0xCB6] = Alu.Dec16(UInt16[cs1, 0xCB6]);
        if (ZeroFlag)
        {
            SI = Alu.Add16(SI, 8);
            UInt16[cs1, 0xCBA] = SI;
            AX = UInt16[DS, SI];
            UInt16[cs1, 0xCB6] = AX;
        }
        AX = UInt16[DS, (ushort)(SI + 2U)];
        AX = Alu.Add16(AX, UInt16[cs1, 0xCB0]);
        UInt16[cs1, 0xCB0] = AX;
        AL = Alu.Shl8(AL, 1);
        AH = Alu.Adc8(AH, 0);
        AL = AH;
        AL = Alu.And8(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        AX = UInt16[DS, (ushort)(SI + 0x4)];
        AX = Alu.Add16(AX, UInt16[cs1, 0xCB2]);
        UInt16[cs1, 0xCB2] = AX;
        AL = Alu.Shl8(AL, 1);
        AH = Alu.Adc8(AH, 0);
        AL = AH;
        AL = Alu.And8(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
        AX = UInt16[DS, (ushort)(SI + 0x6)];
        AX = Alu.Add16(AX, UInt16[cs1, 0xCB4]);
        UInt16[cs1, 0xCB4] = AX;
        AL = Alu.Shl8(AL, 1);
        AH = Alu.Adc8(AH, 0);
        AL = AH;
        AL = Alu.And8(AL, 0x3F);
        UInt8[ES, DI] = AL;
        DI += (ushort)Direction8;
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

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0DDE_10DDE(int loadOffset)
    {
        UInt16[DS, 0x54] = AX;
        NearCall(cs1, 0xDE4, new Func<int, Action>(unknown_1000_0DBC_10DBC));
        if (CarryFlag)
            return NearRet();
        do
        {
            NearCall(cs1, 0xDE9, new Func<int, Action>(unknown_1000_0A3A_10A3A));
            AX = UInt16[DS, 84];
            UInt16[DS, 0x52] = AX;
            DI = UInt16[DS, 76];
            ES = UInt16[DS, 78];
            UInt16[DS, 0x56] = DI;
            UInt16[DS, 0x58] = ES;
            NearCall(cs1, 0xDFE, new Func<int, Action>(unknown_1000_0EAD_10EAD));
            if (ZeroFlag)
                return NearRet();
            NearCall(cs1, 0xE03, new Func<int, Action>(unknown_1000_0FA4_10FA4));
            NearCall(cs1, 0xE06, new Func<int, Action>(unknown_1000_0E4C_10E4C));
            CarryFlag = true;
            if (ZeroFlag)
                return NearRet();
            NearCall(cs1, 0xE0C, new Func<int, Action>(unknown_1000_0E49_10E49));
            NearCall(cs1, 0xE0F, new Func<int, Action>(unknown_1000_0CF4_10CF4));
            CarryFlag = true;
            if (!ZeroFlag)
                return NearRet();
            do
            {
                AX = UInt16[DS, 0x52];
                BP = 0xE46;
                NearCall(cs1, 0xE1B, new Func<int, Action>(unknown_1000_0FEA_10FEA));
                CarryFlag = true;
                if (!ZeroFlag)
                    return NearRet();
                int num = Alu.Sub16(UInt16[DS, 82], 0);
            }
            while (!ZeroFlag);
            SI = 0xEE;
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            NearCall(cs1, 0xE2C, new Func<int, Action>(unknown_1000_11BD_111BD));
            byte ah1 = AH;
            byte al1 = AL;
            byte num1;
            AL = num1 = ah1;
            AH = num1 = al1;
            NearCall(cs1, 0xE31, new Func<int, Action>(unknown_1000_11BD_111BD));
            int num2 = Alu.Sub16(AX, 0x4C4F);
            if (ZeroFlag)
            {
                AX = UInt16[DS, SI];
                SI += (ushort)Direction16;
                NearCall(cs1, 0xE3A, new Func<int, Action>(unknown_1000_11BD_111BD));
                byte ah2 = AH;
                byte al2 = AL;
                AL = num1 = ah2;
                AH = num1 = al2;
                NearCall(cs1, 0xE3F, new Func<int, Action>(unknown_1000_11BD_111BD));
                int num3 = Alu.Sub16(AX, 0x4F50);
            }
            else
            {
                break;
            }
        }
        while (ZeroFlag);
        CarryFlag = false;
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

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0E4C_10E4C(int loadOffset)
    {
        NearCall(cs1, 0xE4F, new Func<int, Action>(unknown_1000_0E86_10E86));
        if (!ZeroFlag)
        {
            return NearRet();
        }
        UInt16[DS, 0x52] = 0;
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0E59_10E59(int loadOffset)
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
            int num = Alu.And16(DI, 0x200);
            if (!ZeroFlag)
            {
                NearCall(cs1, 0xE74, new Func<int, Action>(unknown_1000_0EBD_10EBD));
            }
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            DX = AX;
            AX = UInt16[DS, SI];
            SI += (ushort)Direction16;
            BX = AX;
            AX = 0xA000;
            ES = AX;
            FarCall(cs1, 0xE84, new Func<int, Action>(unknown_1000_0B9A_10B9A));
        }
        DS = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0E86_10E86(int loadOffset)
    {
        Stack.Push16(SI);
        SI = UInt16[DS, 0x56];
        ES = UInt16[DS, 0x58];
        AX = UInt16[ES, SI];
        SI = Alu.Add16(SI, AX);
        AX = SI;
        AX >>= 1;
        AX >>= 1;
        AX >>= 1;
        AX = Alu.Shr16(AX, 1);
        CX = ES;
        AX = Alu.Add16(AX, CX);
        ES = AX;
        SI = Alu.And16(SI, 0xF);
        UInt16[DS, 0x56] = SI;
        UInt16[DS, 0x58] = ES;
        if (JumpDispatcher?.Jump(split_1000_0EB2_10EB2, 0) is true)
        {
            return JumpDispatcher?.JumpAsmReturn;
        }
        return JumpDispatcher?.JumpAsmReturn;
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
        Stack.Push16(CX);
        Stack.Push16(DI);
        Stack.Push16(DS);
        SI += 6;
        BP = 0;
        if (JumpDispatcher?.Jump(split_1000_0F30_10F30, 0) is true)
        {
            return JumpDispatcher?.JumpAsmReturn;
        }
        return JumpDispatcher?.JumpAsmReturn;
        //throw FailAsUntested("External goto not supported for this function.");
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action split_1000_0F30_10F30(int loadOffset)
    {
        while (true)
        {
            BP = Alu.Shr16(BP, 1);
            if (!ZeroFlag)
            {
                if (!CarryFlag)
                    goto label_4;
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
                goto label_2;
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

        label_12:
            AX = Alu.Add16(AX, DI);
            (SI, AX) = (AX, SI);
            BX = DS;
            DX = ES;
            DS = DX;
            ++CX;
            CX = Alu.Inc16(CX);
            while (CX != 0)
            {
                --CX;
                UInt8[ES, DI] = UInt8[DS, SI];
                SI += (ushort)Direction8;
                DI += (ushort)Direction8;
            }
            DS = BX;
            SI = AX;
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
                    goto label_12;
                }
                else
                {
                    break;
                }
            }
            else
            {
                goto label_12;
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
    public virtual Action unknown_1000_0FA4_10FA4(int loadOffset)
    {
        SI = UInt16[DS, 0x56];
        ES = UInt16[DS, 0x58];
        SI = Alu.Add16(SI, 2);
        while (true)
        {
            AX = UInt16[ES, SI];
            SI += (ushort)Direction16;
            int num = Alu.Sub8(AL, byte.MaxValue);
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
                AX = 0x1012;
                Interrupt(0x10);
                NearCall(cs1, 0xFC9, unknown_1000_0FCC_10FCC);
            }
            else
            {
                break;
            }
        }
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0FCC_10FCC(int loadOffset)
    {
        Stack.Push16(SI);
        Stack.Push16(DI);
        SI = DX;
        DI = 0x70;
        DI += BX;
        DI += BX;
        DI = Alu.Add16(DI, BX);
        AX = CX;
        CX <<= 1;
        CX = Alu.Add16(CX, AX);
        ushort num;
        do
        {
            AL = UInt8[ES, SI];
            SI += (ushort)Direction8;
            UInt8[cs1, DI] = AL;
            DI = Alu.Inc16(DI);
            num = --CX;
        }
        while (num != 0);
        DI = Stack.Pop16();
        SI = Stack.Pop16();
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_0FEA_10FEA(int loadOffset)
    {
        InterruptFlag = true;
        Stack.Push16(AX);
        AX = 0;
        ES = AX;
        Stack.Push16(UInt16[ES, 0x46C]);
        uint bp = BP;
        if (bp != 0xE46)
            throw FailAsUntested("Error: Function not registered at address " + ConvertUtils.ToHex32WithoutX(bp));
        NearCall(cs1, 0xFF7, new Func<int, Action>(unknown_1000_0E46_10E46));
        BX = Stack.Pop16();
        BP = Stack.Pop16();
        BP >>= 1;
        BP >>= 1;
        BP = Alu.Shr16(BP, 1);
        AX = BP;
        AX >>= 1;
        AX >>= 1;
        BP -= AX;
        do
        {
            AX = 0;
            ES = AX;
            CheckExternalEvents(cs1, 0x100F);
            AX = UInt16[ES, 0x46C];
            AX -= BX;
            int num = Alu.Sub16(AX, BP);
        }
        while (CarryFlag);
        NearCall(cs1, 0x1018, new Func<int, Action>(unknown_1000_1085_11085));
        return NearRet();
    }

    /// <summary>
    /// First pass rewrite done by the .NET Roslyn compiler (ReadyToRun pre-compilation)
    /// </summary>
    public virtual Action unknown_1000_1019_11019(int loadOffset)
    {
        DI = DX;
        while (true)
        {
            AL = UInt8[DS, DI];
            if (!ZeroFlag)
            {
                AL = Alu.Or8(AL, AL);
                if (!ZeroFlag)
                {
                    DI = Alu.Inc16(DI);
                }
                else
                {
                    return NearRet();
                }
            }
            else
            {
                break;
            }
        }
        return NearRet();
    }

    public virtual Action unknown_1000_105F_1105F(int loadOffset)
    {
        // MOV DI,DX (1000_105F / 0x1105F)
        DI = DX;
        while (true)
        {
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
        }
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