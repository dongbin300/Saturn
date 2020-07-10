using Saturn.Build;
using Saturn.Build.Assemble;
using System;
using System.Collections.Generic;
using static Saturn.Util;

namespace Saturn.PE
{
    public class IMAGE_OPTIONAL_HEADER
    {
        public enum WindowsSubsystem : ushort
        {
            IMAGE_SUBSYSTEM_UNKNOWN = 0x0000,
            IMAGE_SUBSYSTEM_NATIVE = 0x0001,
            IMAGE_SUBSYSTEM_WINDOWS_GUI = 0x0002,
            IMAGE_SUBSYSTEM_WINDOWS_CUI = 0x0003,
            IMAGE_SUBSYSTEM_OS2_CUI = 0x0005,
            IMAGE_SUBSYSTEM_POSIX_CUI = 0x0007,
            IMAGE_SUBSYSTEM_NATIVE_WINDOWS = 0x0008,
            IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 0x0009,
            IMAGE_SUBSYSTEM_EFI_APPLICATION = 0x000A,
            IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 0x000B,
            IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 0x000C,
            IMAGE_SUBSYSTEM_EFI_ROM = 0x000D,
            IMAGE_SUBSYSTEM_XBOX = 0x000E,
            IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION = 0x0010
        }

        [Flags]
        public enum DLLCharacteristics : ushort
        {
            IMAGE_DLLCHARACTERISTICS_HIGH_ENTROPY_VA = 0x0020,
            IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE = 0x0040,
            IMAGE_DLLCHARACTERISTICS_FORCE_INTEGRITY = 0x0080,
            IMAGE_DLLCHARACTERISTICS_NX_COMPAT = 0x0100,
            IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200,
            IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400,
            IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800,
            IMAGE_DLLCHARACTERISTICS_APPCONTAINER = 0x1000,
            IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
            IMAGE_DLLCHARACTERISTICS_GUARD_CF = 0x4000,
            IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000,
        }

        /* Standard fields */
        public BuildEnvironment.PEFormat Magic { get; set; }
        public byte MajorLinkerVersion { get; set; }
        public byte MinorLinkerVersion { get; set; }
        public uint SizeOfCode { get; set; }
        public uint SizeOfInitializedData { get; set; }
        public uint SizeOfUninitializedData { get; set; }
        public uint AddressOfEntryPoint { get; set; }
        public uint BaseOfCode { get; set; }
        public uint BaseOfData { get; set; } // PE32 only included

        /* Windows-specific fields */
        public uint ImageBase { get; set; }
        public ulong ImageBase64 { get; set; }
        public uint SectionAlignment { get; set; }
        public uint FileAlignment { get; set; }
        public ushort MajorOSVersion { get; set; }
        public ushort MinorOSVersion { get; set; }
        public ushort MajorImageVersion { get; set; }
        public ushort MinorImageVersion { get; set; }
        public ushort MajorSubsystemVersion { get; set; }
        public ushort MinorSubsystemVersion { get; set; }
        public uint Win32VersionValue { get; set; }
        public uint SizeOfImage { get; set; }
        public uint SizeOfHeaders { get; set; }
        public uint Checksum { get; set; }
        public WindowsSubsystem Subsystem { get; set; }
        public DLLCharacteristics _DLLCharacteristics { get; set; }
        public uint SizeOfStackReserve { get; set; }
        public ulong SizeOfStackReserve64 { get; set; }
        public uint SizeOfStackCommit { get; set; }
        public ulong SizeOfStackCommit64 { get; set; }
        public uint SizeOfHeapReserve { get; set; }
        public ulong SizeOfHeapReserve64 { get; set; }
        public uint SizeOfHeapCommit { get; set; }
        public ulong SizeOfHeapCommit64 { get; set; }
        public uint LoaderFlags { get; set; }
        public uint NumberOfDataDirectories { get; set; }
        public List<IMAGE_DATA_DIRECTORY> DataDirectories { get; set; }

        public IMAGE_OPTIONAL_HEADER()
        {
            Magic = BuildEnvironment.format;
            MajorLinkerVersion = HexStringToBytes("05")[0];
            MinorLinkerVersion = HexStringToBytes("0C")[0];
            SizeOfCode = AddressManager.TextSectionRawSize;
            SizeOfInitializedData = AddressManager.DataSectionRawSize;
            SizeOfUninitializedData = HexStringToUInt("00000000");
            AddressOfEntryPoint = AddressManager.EntryPointAddress;
            BaseOfCode = AddressManager.TextSectionRVA;
            SectionAlignment = AddressManager.SectionAlignment;
            FileAlignment = AddressManager.FileAlignment;
            MajorOSVersion = HexStringToUShort("0004");
            MinorOSVersion = HexStringToUShort("0000");
            MajorImageVersion = HexStringToUShort("0000");
            MinorImageVersion = HexStringToUShort("0000");
            MajorSubsystemVersion = HexStringToUShort("0004");
            MinorSubsystemVersion = HexStringToUShort("0000");
            Win32VersionValue = HexStringToUInt("00000000");
            SizeOfImage = AddressManager.ImageSize;
            SizeOfHeaders = HexStringToUInt("00000200");
            Checksum = HexStringToUInt("00000000");
            Subsystem = WindowsSubsystem.IMAGE_SUBSYSTEM_WINDOWS_GUI;
            _DLLCharacteristics = 0;
            LoaderFlags = HexStringToUInt("00000000");
            NumberOfDataDirectories = HexStringToUInt("00000010");

            DataDirectories = new List<IMAGE_DATA_DIRECTORY>();
            for (int i = 0; i < NumberOfDataDirectories; i++)
            {
                DataDirectories.Add(new IMAGE_DATA_DIRECTORY(HexStringToUInt("00000000"), HexStringToUInt("00000000")));
            }

            if (Magic == BuildEnvironment.PEFormat.PE32)
            {
                BaseOfData = AddressManager.DataSectionRVA;
                ImageBase = AddressManager.ImageBaseAddress;
                SizeOfStackReserve = HexStringToUInt("00100000");
                SizeOfStackCommit = HexStringToUInt("00001000");
                SizeOfHeapReserve = HexStringToUInt("00100000");
                SizeOfHeapCommit = HexStringToUInt("00001000");
            }
            else if (Magic == BuildEnvironment.PEFormat.PE32PLUS)
            {
                BaseOfData = 0;
                ImageBase64 = AddressManager.ImageBase64Address;
                SizeOfStackReserve64 = HexStringToULong("0000000000100000");
                SizeOfStackCommit64 = HexStringToULong("0000000000001000");
                SizeOfHeapReserve64 = HexStringToULong("0000000000100000");
                SizeOfHeapCommit64 = HexStringToULong("0000000000001000");
            }
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();

            ConcatenateBytes(ref bytes, (ushort)Magic);
            ConcatenateBytes(ref bytes, MajorLinkerVersion);
            ConcatenateBytes(ref bytes, MinorLinkerVersion);
            ConcatenateBytes(ref bytes, SizeOfCode);
            ConcatenateBytes(ref bytes, SizeOfInitializedData);
            ConcatenateBytes(ref bytes, SizeOfUninitializedData);
            ConcatenateBytes(ref bytes, AddressOfEntryPoint);
            ConcatenateBytes(ref bytes, BaseOfCode);
            if (Magic == BuildEnvironment.PEFormat.PE32)
            {
                ConcatenateBytes(ref bytes, BaseOfData);
                ConcatenateBytes(ref bytes, ImageBase);
            }
            else if (Magic == BuildEnvironment.PEFormat.PE32PLUS)
            {
                ConcatenateBytes(ref bytes, ImageBase64);
            }
            ConcatenateBytes(ref bytes, SectionAlignment);
            ConcatenateBytes(ref bytes, FileAlignment);
            ConcatenateBytes(ref bytes, MajorOSVersion);
            ConcatenateBytes(ref bytes, MinorOSVersion);
            ConcatenateBytes(ref bytes, MajorImageVersion);
            ConcatenateBytes(ref bytes, MinorImageVersion);
            ConcatenateBytes(ref bytes, MajorSubsystemVersion);
            ConcatenateBytes(ref bytes, MinorSubsystemVersion);
            ConcatenateBytes(ref bytes, Win32VersionValue);
            ConcatenateBytes(ref bytes, SizeOfImage);
            ConcatenateBytes(ref bytes, SizeOfHeaders);
            ConcatenateBytes(ref bytes, Checksum);
            ConcatenateBytes(ref bytes, (ushort)Subsystem);
            ConcatenateBytes(ref bytes, (ushort)_DLLCharacteristics);
            if (Magic == BuildEnvironment.PEFormat.PE32)
            {
                ConcatenateBytes(ref bytes, SizeOfStackReserve);
                ConcatenateBytes(ref bytes, SizeOfStackCommit);
                ConcatenateBytes(ref bytes, SizeOfHeapReserve);
                ConcatenateBytes(ref bytes, SizeOfHeapCommit);
            }
            else if (Magic == BuildEnvironment.PEFormat.PE32PLUS)
            {
                ConcatenateBytes(ref bytes, SizeOfStackReserve64);
                ConcatenateBytes(ref bytes, SizeOfStackCommit64);
                ConcatenateBytes(ref bytes, SizeOfHeapReserve64);
                ConcatenateBytes(ref bytes, SizeOfHeapCommit64);
            }
            ConcatenateBytes(ref bytes, LoaderFlags);
            ConcatenateBytes(ref bytes, NumberOfDataDirectories);

            foreach (IMAGE_DATA_DIRECTORY dir in DataDirectories)
            {
                ConcatenateBytes(ref bytes, dir.VirtualAddress);
                ConcatenateBytes(ref bytes, dir.Size);
            }

            return bytes.ToArray();
        }
    }
}
