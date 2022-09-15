
namespace Stubs;
using System;
using System.Linq;
using System.Collections.Generic;

using Spice86.Core.Emulator.Function;
using Spice86.Core.Emulator.VM;
using Spice86.Core.Emulator.Memory;
using Spice86.Core.Emulator.ReverseEngineer;

///<summary>
/// Getters and setters for what could be global variables, split per segment register. 0 addresses in total.
/// Observed values for segments:
/// DS:0x1080,0x1143,0x111C,0x215C,0x21F9,0x225A,0x2260,0x22AA,0x22F9,0x235A,0x23C7,0x2441,0x24C3,0x24FC,0x253D,0x253E,0x253F,0x2540,0x25B7,0x25B8,0x25BA,0x25BB,0x25BC,0x25BD,0x25BE,0x25BFCS:0x1143,0x1000ES:0xFF0,0x40

/// Number of globals:
/// 50
/// Globals content:
/// 
            // Accessors for values accessed via register DS
            public class GlobalsOnDs : MemoryBasedDataStructureWithDsBaseAddress {
                public GlobalsOnDs(Machine machine) : base(machine) {
                    
                }

                // Getters and Setters for address 0x1080:0xA/0x1080A.
    // Was accessed via the following registers: DS
    public int GetWord16_1080_000A() {
        return GetUint16(0xA);
    }
    
    // Operation not registered by running code
    public void SetWord16_1080_000A(int value) {
        SetUint16(0xA, (ushort)value);
    }
    
// Getters and Setters for address 0x1080:0xC/0x1080C.
    // Was accessed via the following registers: DS
    public int GetWord16_1080_000C() {
        return GetUint16(0xC);
    }
    
    // Operation not registered by running code
    public void SetWord16_1080_000C(int value) {
        SetUint16(0xC, (ushort)value);
    }
    
// Getters and Setters for address 0x111C:0x4C/0x1120C.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_111C_004C() {
        return new SegmentedAddress(GetUint16((ushort)0x4C + 2), GetUint16((ushort)0x4C));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_111C_004C(SegmentedAddress value) {
        SetUint16((ushort)0x4C + 2, (ushort)value.Segment);
        SetUint16((ushort)0x4C, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x111C:0x50/0x11210.
    // Was accessed via the following registers: DS
    public int GetWord16_111C_0050() {
        return GetUint16(0x50);
    }
    
    // Was accessed via the following registers: DS
    public void SetWord16_111C_0050(int value) {
        SetUint16(0x50, (ushort)value);
    }
    
// Getters and Setters for address 0x111C:0x52/0x11212.
    // Was accessed via the following registers: DS
    public int GetWord16_111C_0052() {
        return GetUint16(0x52);
    }
    
    // Was accessed via the following registers: DS
    public void SetWord16_111C_0052(int value) {
        SetUint16(0x52, (ushort)value);
    }
    
// Getters and Setters for address 0x111C:0x54/0x11214.
    // Was accessed via the following registers: DS
    public int GetWord16_111C_0054() {
        return GetUint16(0x54);
    }
    
    // Was accessed via the following registers: DS
    public void SetWord16_111C_0054(int value) {
        SetUint16(0x54, (ushort)value);
    }
    
// Getters and Setters for address 0x111C:0x56/0x11216.
    // Operation not registered by running code
    public int GetWord16_111C_0056() {
        return GetUint16(0x56);
    }
    
    // Was accessed via the following registers: DS
    public void SetWord16_111C_0056(int value) {
        SetUint16(0x56, (ushort)value);
    }
    
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_111C_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_111C_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x111C:0x58/0x11218.
    // Operation not registered by running code
    public int GetWord16_111C_0058() {
        return GetUint16(0x58);
    }
    
    // Was accessed via the following registers: DS
    public void SetWord16_111C_0058(int value) {
        SetUint16(0x58, (ushort)value);
    }
    
// Getters and Setters for address 0x1143:0x2/0x11432.
    // Operation not registered by running code
    public int GetWord16_1143_0002() {
        return GetUint16(0x2);
    }
    
    // Was accessed via the following registers: DS
    public void SetWord16_1143_0002(int value) {
        SetUint16(0x2, (ushort)value);
    }
    
// Getters and Setters for address 0x1143:0x4/0x11434.
    // Was accessed via the following registers: DS
    public int GetWord16_1143_0004() {
        return GetUint16(0x4);
    }
    
    // Operation not registered by running code
    public void SetWord16_1143_0004(int value) {
        SetUint16(0x4, (ushort)value);
    }
    
// Getters and Setters for address 0x1143:0x6/0x11436.
    // Was accessed via the following registers: DS
    public int GetWord16_1143_0006() {
        return GetUint16(0x6);
    }
    
    // Operation not registered by running code
    public void SetWord16_1143_0006(int value) {
        SetUint16(0x6, (ushort)value);
    }
    
// Getters and Setters for address 0x215C:0x56/0x21616.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_215C_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_215C_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x21F9:0x56/0x21FE6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_21F9_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_21F9_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x225A:0x56/0x225F6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_225A_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_225A_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x2260:0x56/0x22656.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_2260_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_2260_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x22AA:0x56/0x22AF6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_22AA_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_22AA_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x22F9:0x56/0x22FE6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_22F9_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_22F9_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x235A:0x56/0x235F6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_235A_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_235A_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x23C7:0x56/0x23CC6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_23C7_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_23C7_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x2441:0x56/0x24466.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_2441_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_2441_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x24C3:0x56/0x24C86.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_24C3_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_24C3_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x24FC:0x56/0x25016.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_24FC_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_24FC_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x253D:0x56/0x25426.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_253D_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_253D_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x253E:0x56/0x25436.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_253E_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_253E_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x253F:0x56/0x25446.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_253F_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_253F_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x2540:0x56/0x25456.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_2540_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_2540_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25B7:0x56/0x25BC6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25B7_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25B7_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25B8:0x56/0x25BD6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25B8_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25B8_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25BA:0x56/0x25BF6.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25BA_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25BA_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25BB:0x56/0x25C06.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25BB_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25BB_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25BC:0x56/0x25C16.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25BC_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25BC_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25BD:0x56/0x25C26.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25BD_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25BD_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25BE:0x56/0x25C36.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25BE_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25BE_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    
// Getters and Setters for address 0x25BF:0x56/0x25C46.
    // Was accessed via the following registers: DS
    public SegmentedAddress GetPtrDword32Ptr_25BF_0056() {
        return new SegmentedAddress(GetUint16((ushort)0x56 + 2), GetUint16((ushort)0x56));
    }
    
    // Operation not registered by running code
    public void SetPtrDword32Ptr_25BF_0056(SegmentedAddress value) {
        SetUint16((ushort)0x56 + 2, (ushort)value.Segment);
        SetUint16((ushort)0x56, (ushort)value.Offset);
    }
    

;
                }
            
            public class GlobalsOnCsSegment0x1143 : MemoryBasedDataStructureWithBaseAddress {
                public GlobalsOnCsSegment0x1143(Machine machine) : base(machine.Memory, 0x1143 * 0x10) {
                  
                }
                // Getters and Setters for address 0x1143:0x8/0x11438.
    // Was accessed via the following registers: CS
    public int GetWord16_1143_0008() {
        return GetUint16(0x8);
    }
    
    // Operation not registered by running code
    public void SetWord16_1143_0008(int value) {
        SetUint16(0x8, (ushort)value);
    }
    


            }
            
            public class GlobalsOnCsSegment0x1000 : MemoryBasedDataStructureWithBaseAddress {
                public GlobalsOnCsSegment0x1000(Machine machine) : base(machine.Memory, 0x1000 * 0x10) {
                  
                }
                // Getters and Setters for address 0x1000:0x6B/0x1006B.
    // Was accessed via the following registers: CS
    public int GetByte8_1000_006B() {
        return GetUint8(0x6B);
    }
    
    // Was accessed via the following registers: CS
    public void SetByte8_1000_006B(int value) {
        SetUint8(0x6B, (byte)value);
    }
    
// Getters and Setters for address 0x1000:0x6C/0x1006C.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_006C() {
        return GetUint16(0x6C);
    }
    
    // Was accessed via the following registers: CS
    public void SetWord16_1000_006C(int value) {
        SetUint16(0x6C, (ushort)value);
    }
    
// Getters and Setters for address 0x1000:0x6E/0x1006E.
    // Was accessed via the following registers: CS
    public int GetByte8_1000_006E() {
        return GetUint8(0x6E);
    }
    
    // Was accessed via the following registers: CS
    public void SetByte8_1000_006E(int value) {
        SetUint8(0x6E, (byte)value);
    }
    
// Getters and Setters for address 0x1000:0x6F/0x1006F.
    // Was accessed via the following registers: CS
    public int GetByte8_1000_006F() {
        return GetUint8(0x6F);
    }
    
    // Was accessed via the following registers: CS
    public void SetByte8_1000_006F(int value) {
        SetUint8(0x6F, (byte)value);
    }
    
// Getters and Setters for address 0x1000:0xA71/0x10A71.
    // Operation not registered by running code
    public int GetByte8_1000_0A71() {
        return GetUint8(0xA71);
    }
    
    // Was accessed via the following registers: CS
    public void SetByte8_1000_0A71(int value) {
        SetUint8(0xA71, (byte)value);
    }
    
// Getters and Setters for address 0x1000:0xB2F/0x10B2F.
    // Operation not registered by running code
    public int GetByte8_1000_0B2F() {
        return GetUint8(0xB2F);
    }
    
    // Was accessed via the following registers: CS
    public void SetByte8_1000_0B2F(int value) {
        SetUint8(0xB2F, (byte)value);
    }
    
// Getters and Setters for address 0x1000:0xCB0/0x10CB0.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_0CB0() {
        return GetUint16(0xCB0);
    }
    
    // Was accessed via the following registers: CS
    public void SetWord16_1000_0CB0(int value) {
        SetUint16(0xCB0, (ushort)value);
    }
    
// Getters and Setters for address 0x1000:0xCB2/0x10CB2.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_0CB2() {
        return GetUint16(0xCB2);
    }
    
    // Was accessed via the following registers: CS
    public void SetWord16_1000_0CB2(int value) {
        SetUint16(0xCB2, (ushort)value);
    }
    
// Getters and Setters for address 0x1000:0xCB4/0x10CB4.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_0CB4() {
        return GetUint16(0xCB4);
    }
    
    // Was accessed via the following registers: CS
    public void SetWord16_1000_0CB4(int value) {
        SetUint16(0xCB4, (ushort)value);
    }
    
// Getters and Setters for address 0x1000:0xCB6/0x10CB6.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_0CB6() {
        return GetUint16(0xCB6);
    }
    
    // Was accessed via the following registers: CS
    public void SetWord16_1000_0CB6(int value) {
        SetUint16(0xCB6, (ushort)value);
    }
    
// Getters and Setters for address 0x1000:0xCB8/0x10CB8.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_0CB8() {
        return GetUint16(0xCB8);
    }
    
    // Operation not registered by running code
    public void SetWord16_1000_0CB8(int value) {
        SetUint16(0xCB8, (ushort)value);
    }
    
// Getters and Setters for address 0x1000:0xCBA/0x10CBA.
    // Was accessed via the following registers: CS
    public int GetWord16_1000_0CBA() {
        return GetUint16(0xCBA);
    }
    
    // Was accessed via the following registers: CS
    public void SetWord16_1000_0CBA(int value) {
        SetUint16(0xCBA, (ushort)value);
    }
    


            }
            

            // Accessors for values accessed via register ES
            public class GlobalsOnEs : MemoryBasedDataStructureWithEsBaseAddress {
                public GlobalsOnEs(Machine machine) : base(machine) {
                    
                }

                // Getters and Setters for address 0x40:0x63/0x463.
    // Was accessed via the following registers: ES
    public int GetWord16_0040_0063() {
        return GetUint16(0x63);
    }
    
    // Operation not registered by running code
    public void SetWord16_0040_0063(int value) {
        SetUint16(0x63, (ushort)value);
    }
    
// Getters and Setters for address 0x40:0x6C/0x46C.
    // Was accessed via the following registers: ES
    public int GetWord16_0040_006C() {
        return GetUint16(0x6C);
    }
    
    // Operation not registered by running code
    public void SetWord16_0040_006C(int value) {
        SetUint16(0x6C, (ushort)value);
    }
    
// Getters and Setters for address 0xFF0:0x2/0xFF02.
    // Was accessed via the following registers: ES
    public int GetWord16_0FF0_0002() {
        return GetUint16(0x2);
    }
    
    // Operation not registered by running code
    public void SetWord16_0FF0_0002(int value) {
        SetUint16(0x2, (ushort)value);
    }
    

;
                }
            

/// <summary>
/// Stubs for overrides
/// </summary>
public class Stubs : CSharpOverrideHelper {
    public Stubs(Dictionary<SegmentedAddress, FunctionInformation> functionInformations, Machine machine) : base(functionInformations, machine) {
        
    }

  // Not providing stub for entry. Reason: Function has no return


    // defineFunction(0x1000, 0x10F4, "unknown", this.unknown_1000_10F4_110F4);
    public Action unknown_1000_10F4_110F4() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0x105F, "unknown", this.unknown_1000_105F_1105F);
    public Action unknown_1000_105F_1105F() {
        
   
        return NearRet();
    }

    // defineFunction(0xF000, 0x20, "interrupt_handler_0x21", this.interrupt_handler_0x21_F000_0020_F0020);
    public Action interrupt_handler_0x21_F000_0020_F0020() {
        
   
        return InterruptRet();
    }

    // defineFunction(0x1000, 0x970, "unknown", this.unknown_1000_0970_10970);
    public Action unknown_1000_0970_10970() {
        // unknown_1000_09B5_109B5()// interrupt_handler_0x10_F000_0008_F0008()

        return NearRet();
    }

    // defineFunction(0xF000, 0x8, "interrupt_handler_0x10", this.interrupt_handler_0x10_F000_0008_F0008);
    public Action interrupt_handler_0x10_F000_0008_F0008() {
        
   
        return InterruptRet();
    }

    // defineFunction(0x1000, 0x9B5, "unknown", this.unknown_1000_09B5_109B5);
    public Action unknown_1000_09B5_109B5() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0x1019, "unknown", this.unknown_1000_1019_11019);
    public Action unknown_1000_1019_11019() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xDDE, "unknown", this.unknown_1000_0DDE_10DDE);
    public Action unknown_1000_0DDE_10DDE() {
        // unknown_1000_0A3A_10A3A()// unknown_1000_0CF4_10CF4()// unknown_1000_0DBC_10DBC()// unknown_1000_0E49_10E49()// unknown_1000_0E4C_10E4C()// unknown_1000_0EAD_10EAD()// unknown_1000_0FA4_10FA4()// unknown_1000_0FEA_10FEA()// unknown_1000_11BD_111BD()

        return NearRet();
    }

    // defineFunction(0x1000, 0xDBC, "unknown", this.unknown_1000_0DBC_10DBC);
    public Action unknown_1000_0DBC_10DBC() {
        // unknown_1000_109A_1109A()

        return NearRet();
    }

    // defineFunction(0x1000, 0x109A, "unknown", this.unknown_1000_109A_1109A);
    public Action unknown_1000_109A_1109A() {
        // interrupt_handler_0x21_F000_0020_F0020()

        return NearRet();
    }

    // defineFunction(0x1000, 0xA3A, "unknown", this.unknown_1000_0A3A_10A3A);
    public Action unknown_1000_0A3A_10A3A() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xEAD, "unknown", this.unknown_1000_0EAD_10EAD);
    public Action unknown_1000_0EAD_10EAD() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xFA4, "unknown", this.unknown_1000_0FA4_10FA4);
    public Action unknown_1000_0FA4_10FA4() {
        // unknown_1000_0FCC_10FCC()// interrupt_handler_0x10_F000_0008_F0008()

        return NearRet();
    }

    // defineFunction(0x1000, 0xFCC, "unknown", this.unknown_1000_0FCC_10FCC);
    public Action unknown_1000_0FCC_10FCC() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xE4C, "unknown", this.unknown_1000_0E4C_10E4C);
    public Action unknown_1000_0E4C_10E4C() {
        // unknown_1000_0E86_10E86()

        return NearRet();
    }

    // defineFunction(0x1000, 0xE86, "unknown", this.unknown_1000_0E86_10E86);
    public Action unknown_1000_0E86_10E86() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xE49, "unknown", this.unknown_1000_0E49_10E49);
    public Action unknown_1000_0E49_10E49() {
        // unknown_1000_0E59_10E59()// unknown_1000_0E86_10E86()

        return NearRet();
    }

    // defineFunction(0x1000, 0xE59, "unknown", this.unknown_1000_0E59_10E59);
    public Action unknown_1000_0E59_10E59() {
        // unknown_1000_0B9A_10B9A()// unknown_1000_0EBD_10EBD()

        return NearRet();
    }

    // defineFunction(0x1000, 0xEBD, "unknown", this.unknown_1000_0EBD_10EBD);
    public Action unknown_1000_0EBD_10EBD() {
        // unknown_1000_0EFE_10EFE()

        return NearRet();
    }

    // defineFunction(0x1000, 0xEFE, "unknown", this.unknown_1000_0EFE_10EFE);
    public Action unknown_1000_0EFE_10EFE() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xB9A, "unknown", this.unknown_1000_0B9A_10B9A);
    public Action unknown_1000_0B9A_10B9A() {
        // unknown_1000_0A22_10A22()

        return FarRet();
    }

    // defineFunction(0x1000, 0xA22, "unknown", this.unknown_1000_0A22_10A22);
    public Action unknown_1000_0A22_10A22() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xCF4, "unknown", this.unknown_1000_0CF4_10CF4);
    public Action unknown_1000_0CF4_10CF4() {
        // unknown_1000_0C72_10C72()// unknown_1000_0D22_10D22()// unknown_1000_1085_11085()

        return NearRet();
    }

    // defineFunction(0x1000, 0xC72, "unknown", this.unknown_1000_0C72_10C72);
    public Action unknown_1000_0C72_10C72() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xD22, "unknown", this.unknown_1000_0D22_10D22);
    public Action unknown_1000_0D22_10D22() {
        // unknown_1000_09D8_109D8()// unknown_1000_0D5F_10D5F()

        return NearRet();
    }

    // defineFunction(0x1000, 0xD5F, "unknown", this.unknown_1000_0D5F_10D5F);
    public Action unknown_1000_0D5F_10D5F() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0x9D8, "unknown", this.unknown_1000_09D8_109D8);
    public Action unknown_1000_09D8_109D8() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0x1085, "unknown", this.unknown_1000_1085_11085);
    public Action unknown_1000_1085_11085() {
        // interrupt_handler_0x16_F000_0014_F0014()

        return NearRet();
    }

    // defineFunction(0xF000, 0x14, "interrupt_handler_0x16", this.interrupt_handler_0x16_F000_0014_F0014);
    public Action interrupt_handler_0x16_F000_0014_F0014() {
        
   
        return InterruptRet();
    }

    // defineFunction(0x1000, 0xFEA, "unknown", this.unknown_1000_0FEA_10FEA);
    public Action unknown_1000_0FEA_10FEA() {
        // unknown_1000_0E46_10E46()// unknown_1000_1085_11085()

        return NearRet();
    }

    // defineFunction(0x1000, 0xE46, "unknown", this.unknown_1000_0E46_10E46);
    public Action unknown_1000_0E46_10E46() {
        // unknown_1000_0D22_10D22()// unknown_1000_0E59_10E59()// unknown_1000_0E86_10E86()

        return NearRet();
    }

    // defineFunction(0x1000, 0x11BD, "unknown", this.unknown_1000_11BD_111BD);
    public Action unknown_1000_11BD_111BD() {
        
   
        return NearRet();
    }

    // defineFunction(0x1000, 0xA51, "unknown", this.unknown_1000_0A51_10A51);
    public Action unknown_1000_0A51_10A51() {
        
   
        return NearRet();
    }
}
