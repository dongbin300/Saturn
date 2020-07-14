using static Saturn.Util.StringUtil;
using static Saturn.Util.ByteUtil;

namespace Saturn.PE
{
    public class IMAGE_DOS_HEADER
    {
        public ushort Signature { get; set; }
        public ushort BytesOnLastPageOfFile { get; set; }
        public ushort PagesInFile { get; set; }
        public ushort Relocations { get; set; }
        public ushort SizeOfHeaderInParagraphs { get; set; }
        public ushort MinimumExtraParagraphs { get; set; }
        public ushort MaximumExtraParagraphs { get; set; }
        public ushort InitialSS { get; set; }
        public ushort InitialSP { get; set; }
        public ushort Checksum { get; set; }
        public ushort InitialIP { get; set; }
        public ushort InitialCS { get; set; }
        public ushort OffsetToRelocationTable { get; set; }
        public ushort OverlayNumber { get; set; }
        public ushort OEMIdentifier { get; set; }
        public ushort OEMInformation { get; set; }
        public uint OffsetToNewEXEHeader { get; set; }

        public IMAGE_DOS_HEADER()
        {
            Signature = HexStringToUShort("5A4D");
            BytesOnLastPageOfFile = HexStringToUShort("0090");//
            PagesInFile = HexStringToUShort("0003");//
            Relocations = HexStringToUShort("0000");//
            SizeOfHeaderInParagraphs = HexStringToUShort("0004");//
            MinimumExtraParagraphs = HexStringToUShort("0000");//
            MaximumExtraParagraphs = HexStringToUShort("FFFF");//
            InitialSS = HexStringToUShort("0000");//
            InitialSP = HexStringToUShort("00B8");//
            Checksum = HexStringToUShort("0000");//
            InitialIP = HexStringToUShort("0000");//
            InitialCS = HexStringToUShort("0000");//
            OffsetToRelocationTable = HexStringToUShort("0040");//
            OverlayNumber = HexStringToUShort("0000");//

            // Reserved 8 Bytes

            OEMIdentifier = HexStringToUShort("0000");//
            OEMInformation = HexStringToUShort("0000");//

            // Reserved 20 Bytes

            OffsetToNewEXEHeader = HexStringToUInt("000000A8");//
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[64];

            ConcatenateBytes(ref bytes, 0, Signature);
            ConcatenateBytes(ref bytes, 2, BytesOnLastPageOfFile);
            ConcatenateBytes(ref bytes, 4, PagesInFile);
            ConcatenateBytes(ref bytes, 6, Relocations);
            ConcatenateBytes(ref bytes, 8, SizeOfHeaderInParagraphs);
            ConcatenateBytes(ref bytes, 10, MinimumExtraParagraphs);
            ConcatenateBytes(ref bytes, 12, MaximumExtraParagraphs);
            ConcatenateBytes(ref bytes, 14, InitialSS);
            ConcatenateBytes(ref bytes, 16, InitialSP);
            ConcatenateBytes(ref bytes, 18, Checksum);
            ConcatenateBytes(ref bytes, 20, InitialIP);
            ConcatenateBytes(ref bytes, 22, InitialCS);
            ConcatenateBytes(ref bytes, 24, OffsetToRelocationTable);
            ConcatenateBytes(ref bytes, 26, OverlayNumber);
            ConcatenateHexStringBytes(ref bytes, 28, "00 00 00 00 00 00 00 00");
            ConcatenateBytes(ref bytes, 36, OEMIdentifier);
            ConcatenateBytes(ref bytes, 38, OEMInformation);
            ConcatenateHexStringBytes(ref bytes, 40, "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            ConcatenateBytes(ref bytes, 60, OffsetToNewEXEHeader);

            return bytes;
        }
    }
}
