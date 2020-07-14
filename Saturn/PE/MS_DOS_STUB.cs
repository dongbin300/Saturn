using static Saturn.Util.StringUtil;

namespace Saturn.PE
{
    public class MS_DOS_STUB
    {
        public byte[] Data { get; set; }

        public MS_DOS_STUB()
        {
            Data = HexStringToBytes("0E 1F BA 0E 00 B4 09 CD 21 B8 01 4C CD 21 54 68 69 73 20 70 72 6F 67 72 61 6D 20 63 61 6E 6E 6F 74 20 62 65 20 72 75 6E 20 69 6E 20 44 4F 53 20 6D 6F 64 65 2E 0D 0D 0A 24 00 00 00 00 00 00 00 5D 17 1D DB 19 76 73 88 19 76 73 88 19 76 73 88 E5 56 61 88 18 76 73 88 52 69 63 68 19 76 73 88 00 00 00 00 00 00 00 00");
        }

        public byte[] ToBytes()
        {
            return Data;
        }
    }
}
