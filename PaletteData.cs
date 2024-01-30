using Spice86.Core.Emulator.CPU;
using Spice86.Core.Emulator.InterruptHandlers.VGA;
using Spice86.Core.Emulator.Memory.ReaderWriter;
using Spice86.Core.Emulator.ReverseEngineer.DataStructure;
using Spice86.Core.Emulator.ReverseEngineer.DataStructure.Array;
using Spice86.Shared.Emulator.Memory;

namespace logo;

class PaletteData : MemoryBasedDataStructure
{
    public SegmentedAddress _address;

    public PaletteData(IByteReaderWriter byteReaderWriter, SegmentedAddress baseAddress): base(byteReaderWriter, baseAddress.ToPhysical())
    {
        _address = baseAddress;
    }

    public byte Index => UInt8[0];
    public byte Count => UInt8[1];
    public UInt8Array PaletteRgbData => GetUInt8Array(2, Count*3);
    public byte NextByte => UInt8[Size+1];
    public ushort Size => (ushort)(2 + Count*3);

    public PaletteData? Next => NextByte==0xFF?null:new PaletteData(this.ByteReaderWriter, new SegmentedAddress(_address.Segment, (ushort)(_address.Offset + Size)));
    public void LoadInVgaDac(State state, IVideoInt10Handler vgaBios)
    {
        state.ES = _address.Segment;
        // +2 because palette starts at offset 2 in the struct.
        // TODO: make UInt8Array know its base address.
        state.DX = (ushort)(_address.Offset + 2);
        state.CX = Count;
        state.BX = Index;
        // 12: write to DAC
        // TODO: expose the underlying methods.
        state.AX = 0x1012;
        vgaBios.SetPaletteRegisters();
    }
}