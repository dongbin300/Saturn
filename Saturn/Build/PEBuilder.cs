using Saturn.PE;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Saturn.Build
{
    public class PEBuilder
    {
        private IMAGE_DOS_HEADER imageDosHeader;
        private MS_DOS_STUB msDosStub;
        private IMAGE_NT_HEADER imageNTHeader;
        private List<IMAGE_SECTION_HEADER> imageSectionHeaders;
        private List<SECTION> sections;

        private List<byte> bytes = new List<byte>();

        public PEBuilder(byte[] assemblyBytes, byte[] dataBytes)
        {
            imageDosHeader = new IMAGE_DOS_HEADER();
            msDosStub = new MS_DOS_STUB();
            imageNTHeader = new IMAGE_NT_HEADER();
            imageSectionHeaders = new List<IMAGE_SECTION_HEADER>();
            imageSectionHeaders.Add(new IMAGE_SECTION_HEADER(".text", (uint)assemblyBytes.Length));
            imageSectionHeaders.Add(new IMAGE_SECTION_HEADER(".data", (uint)dataBytes.Length));
            sections = new List<SECTION>();
            sections.Add(new SECTION(".text", imageSectionHeaders.Find(h => h.NameByString.Equals(".text")).SizeOfRawData, assemblyBytes));
            sections.Add(new SECTION(".data", imageSectionHeaders.Find(h => h.NameByString.Equals(".data")).SizeOfRawData, dataBytes));

            Serialize();
        }

        private void Serialize()
        {
            var imageDosHeaderBytes = imageDosHeader.ToBytes();
            bytes.AddRange(imageDosHeaderBytes.ToList());

            var msDosStubBytes = msDosStub.ToBytes();
            bytes.AddRange(msDosStubBytes.ToList());

            var imageNTHeaderBytes = imageNTHeader.ToBytes();
            bytes.AddRange(imageNTHeaderBytes.ToList());

            foreach(IMAGE_SECTION_HEADER header in imageSectionHeaders)
            {
                var imageSectionHeaderBytes = header.ToBytes();
                bytes.AddRange(imageSectionHeaderBytes.ToList());
            }

            bytes.AddRange(new List<byte>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            foreach(SECTION section in sections)
            {
                var sectionBytes = section.ToBytes();
                bytes.AddRange(sectionBytes.ToList());
            }
        }
        
        public void WriteFile(Stream stream)
        {
            var bytesArray = bytes.ToArray();
            stream.Write(bytesArray, 0, bytesArray.Length);
        }
    }
}
