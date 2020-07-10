using System;
using static Saturn.Util;

namespace Saturn.PE
{
    public class IMAGE_FILE_HEADER
    {
        public enum MachineType : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0000,
            IMAGE_FILE_MACHINE_AM33 = 0x01D3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x01C0,
            IMAGE_FILE_MACHINE_ARM64 = 0xAA64,
            IMAGE_FILE_MACHINE_ARMNT = 0x01C4,
            IMAGE_FILE_MACHINE_EBC = 0x0EBC,
            IMAGE_FILE_MACHINE_I386 = 0x014C,
            IMAGE_FILE_MACHINE_IA64 = 0x0200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x0266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x0366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x0466,
            IMAGE_FILE_MACHINE_POWERPC = 0x01F0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x01F1,
            IMAGE_FILE_MACHINE_R4000 = 0x0166,
            IMAGE_FILE_MACHINE_RISCV32 = 0x5032,
            IMAGE_FILE_MACHINE_RISCV64 = 0x5064,
            IMAGE_FILE_MACHINE_RISCV128 = 0x5128,
            IMAGE_FILE_MACHINE_SH3 = 0x01A2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x01A3,
            IMAGE_FILE_MACHINE_SH4 = 0x01A6,
            IMAGE_FILE_MACHINE_SH5 = 0x01A8,
            IMAGE_FILE_MACHINE_THUMB = 0x01C2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x0169,
        }

        [Flags]
        public enum Characteristics : ushort
        {
            IMAGE_FILE_RELOCS_STRIPPED = 0x0001,
            IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002,
            IMAGE_FILE_LINE_NUMS_STRIPPED = 0x0004,
            IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x0008,
            IMAGE_FILE_AGGRESSIVE_WS_TRIM = 0x0010,
            IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020,
            IMAGE_FILE_BYTES_REVERSED_LO = 0x0080,
            IMAGE_FILE_32BIT_MACHINE = 0x0100,
            IMAGE_FILE_DEBUG_STRIPPED = 0x0200,
            IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 0x0400,
            IMAGE_FILE_NET_RUN_FROM_SWAP = 0x0800,
            IMAGE_FILE_SYSTEM = 0x1000,
            IMAGE_FILE_DLL = 0x2000,
            IMAGE_FILE_UP_SYSTEM_ONLY = 0x4000,
            IMAGE_FILE_BYTES_REVERSED_HI = 0x8000
        }

        public MachineType Machine { get; set; }
        public ushort NumberOfSections { get; set; }
        public int TimeDateStamp { get; set; }
        public uint PointerToSymbolTable { get; set; }
        public uint NumberOfSymbols { get; set; }
        public ushort SizeOfOptionalHeader { get; set; }
        public Characteristics _Characteristics { get; set; }

        public IMAGE_FILE_HEADER()
        {
            Machine = MachineType.IMAGE_FILE_MACHINE_I386;
            NumberOfSections = HexStringToUShort("0002");
            TimeDateStamp = (int)((long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds & 0xFFFFFFFF);
            PointerToSymbolTable = HexStringToUInt("00000000");
            NumberOfSymbols = HexStringToUInt("00000000");
            SizeOfOptionalHeader = HexStringToUShort("00E0");
            _Characteristics = Characteristics.IMAGE_FILE_RELOCS_STRIPPED | Characteristics.IMAGE_FILE_EXECUTABLE_IMAGE | Characteristics.IMAGE_FILE_LINE_NUMS_STRIPPED |
            Characteristics.IMAGE_FILE_LOCAL_SYMS_STRIPPED |
            Characteristics.IMAGE_FILE_32BIT_MACHINE;
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[20];

            ConcatenateBytes(ref bytes, 0, (ushort)Machine);
            ConcatenateBytes(ref bytes, 2, NumberOfSections);
            ConcatenateBytes(ref bytes, 4, TimeDateStamp);
            ConcatenateBytes(ref bytes, 8, PointerToSymbolTable);
            ConcatenateBytes(ref bytes, 12, NumberOfSymbols);
            ConcatenateBytes(ref bytes, 16, SizeOfOptionalHeader);
            ConcatenateBytes(ref bytes, 18, (ushort)_Characteristics);

            return bytes;
        }
    }
}
