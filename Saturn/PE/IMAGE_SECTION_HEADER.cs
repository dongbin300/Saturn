using Saturn.Build.Assemble;
using System;
using System.Collections.Generic;
using static Saturn.Util.StringUtil;
using static Saturn.Util.ByteUtil;

namespace Saturn.PE
{
    public class IMAGE_SECTION_HEADER
    {
        [Flags]
        public enum SectionFlags : uint
        {
            IMAGE_SCN_TYPE_NO_PAD = 0x00000008,
            IMAGE_SCN_CNT_CODE = 0x00000020,
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,
            IMAGE_SCN_LNK_OTHER = 0x00000100,
            IMAGE_SCN_LNK_INFO = 0x00000200,
            IMAGE_SCN_LNK_REMOVE = 0x00000800,
            IMAGE_SCN_LNK_COMDAT = 0x00001000,
            IMAGE_SCN_GPREL = 0x00008000,
            IMAGE_SCN_MEM_PURGEABLE = 0x00020000,
            IMAGE_SCN_MEM_16BIT = 0x00020000,
            IMAGE_SCN_MEM_LOCKED = 0x00040000,
            IMAGE_SCN_MEM_PRELOAD = 0x00080000,
            IMAGE_SCN_ALIGN_1BYTES = 0x00100000,
            IMAGE_SCN_ALIGN_2BYTES = 0x00200000,
            IMAGE_SCN_ALIGN_4BYTES = 0x00300000,
            IMAGE_SCN_ALIGN_8BYTES = 0x00400000,
            IMAGE_SCN_ALIGN_16BYTES = 0x00500000,
            IMAGE_SCN_ALIGN_32BYTES = 0x00600000,
            IMAGE_SCN_ALIGN_64BYTES = 0x00700000,
            IMAGE_SCN_ALIGN_128BYTES = 0x00800000,
            IMAGE_SCN_ALIGN_256BYTES = 0x00900000,
            IMAGE_SCN_ALIGN_512BYTES = 0x00A00000,
            IMAGE_SCN_ALIGN_1024BYTES = 0x00B00000,
            IMAGE_SCN_ALIGN_2048BYTES = 0x00C00000,
            IMAGE_SCN_ALIGN_4096BYTES = 0x00D00000,
            IMAGE_SCN_ALIGN_8192BYTES = 0x00E00000,
            IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,
            IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,
            IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,
            IMAGE_SCN_MEM_NOT_PAGED = 0x08000000,
            IMAGE_SCN_MEM_SHARED = 0x10000000,
            IMAGE_SCN_MEM_EXECUTE = 0x20000000,
            IMAGE_SCN_MEM_READ = 0x40000000,
            IMAGE_SCN_MEM_WRITE = 0x80000000,
        }

        public string NameByString { get; set; }
        public ulong Name { get; set; }

        /// <summary>
        /// 어셈블리 코드의 크기
        /// </summary>
        public uint VirtualSize { get; set; }

        /// <summary>
        /// 실제 메모리 상에서의 상대주소
        /// section마다 기본값은 0x1000으로 잡지만
        /// 크기가 클 경우 그 이상으로 잡아야함
        /// </summary>
        public uint RVA { get; set; }

        /// <summary>
        /// Section의 크기
        /// </summary>
        public uint SizeOfRawData { get; set; }

        /// <summary>
        /// Section의 시작주소
        /// </summary>
        public uint PointerToRawData { get; set; }

        public uint PointerToRelocations { get; set; }
        public uint PointerToLineNumbers { get; set; }
        public ushort NumberOfRelocations { get; set; }
        public ushort NumberOfLineNumbers { get; set; }
        public SectionFlags Characteristics { get; set; }

        public IMAGE_SECTION_HEADER(string name, uint contentBytesLength = 0)
        {
            NameByString = name;

            switch (name)
            {
                case ".text":
                    InitializeText(contentBytesLength);
                    break;

                case ".data":
                    InitializeData(contentBytesLength);
                    break;

                default:
                    break;
            }
        }

        private void InitializeText(uint assemblyBytesLength)
        {
            Name = HexStringToULong("2E74657874000000"); // .text
            VirtualSize = assemblyBytesLength;
            RVA = AddressManager.TextSectionRVA;
            SizeOfRawData = AddressManager.TextSectionRawSize;
            PointerToRawData = AddressManager.TextSectionRawPointer;
            PointerToRelocations = HexStringToUInt("00000000");
            PointerToLineNumbers = HexStringToUInt("00000000");
            NumberOfRelocations = HexStringToUShort("0000");
            NumberOfLineNumbers = HexStringToUShort("0000");
            Characteristics = SectionFlags.IMAGE_SCN_CNT_CODE | SectionFlags.IMAGE_SCN_MEM_EXECUTE | SectionFlags.IMAGE_SCN_MEM_READ;
        }

        private void InitializeData(uint dataBytesLength)
        {
            Name = HexStringToULong("2E64617461000000"); // .data
            VirtualSize = dataBytesLength;
            RVA = AddressManager.DataSectionRVA;
            SizeOfRawData = AddressManager.DataSectionRawSize;
            PointerToRawData = AddressManager.DataSectionRawPointer;
            PointerToRelocations = HexStringToUInt("00000000");
            PointerToLineNumbers = HexStringToUInt("00000000");
            NumberOfRelocations = HexStringToUShort("0000");
            NumberOfLineNumbers = HexStringToUShort("0000");
            Characteristics = SectionFlags.IMAGE_SCN_CNT_INITIALIZED_DATA | SectionFlags.IMAGE_SCN_MEM_READ | SectionFlags.IMAGE_SCN_MEM_WRITE;
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();

            ConcatenateBytes(ref bytes, Name, true);
            ConcatenateBytes(ref bytes, VirtualSize);
            ConcatenateBytes(ref bytes, RVA);
            ConcatenateBytes(ref bytes, SizeOfRawData);
            ConcatenateBytes(ref bytes, PointerToRawData);
            ConcatenateBytes(ref bytes, PointerToRelocations);
            ConcatenateBytes(ref bytes, PointerToLineNumbers);
            ConcatenateBytes(ref bytes, NumberOfRelocations);
            ConcatenateBytes(ref bytes, NumberOfLineNumbers);
            ConcatenateBytes(ref bytes, (uint)Characteristics);

            return bytes.ToArray();
        }
    }
}
