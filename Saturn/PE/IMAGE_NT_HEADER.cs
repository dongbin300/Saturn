using System;
using System.Collections.Generic;
using System.Linq;
using static Saturn.Util;

namespace Saturn.PE
{
    public class IMAGE_NT_HEADER
    {
        public uint Signature { get; set; }
        public IMAGE_FILE_HEADER ImageFileHeader { get; set; }
        public IMAGE_OPTIONAL_HEADER ImageOptionalHeader { get; set; }

        public IMAGE_NT_HEADER()
        {
            Signature = HexStringToUInt("00004550");
            ImageFileHeader = new IMAGE_FILE_HEADER();
            ImageOptionalHeader = new IMAGE_OPTIONAL_HEADER();
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();

            var signatureBytes = BitConverter.GetBytes(Signature);
            bytes.AddRange(signatureBytes.ToList());

            var imageFileHeaderBytes = ImageFileHeader.ToBytes();
            bytes.AddRange(imageFileHeaderBytes.ToList());

            var imageOptionalHeaderBytes = ImageOptionalHeader.ToBytes();
            bytes.AddRange(imageOptionalHeaderBytes.ToList());

            return bytes.ToArray();
        }
    }
}
