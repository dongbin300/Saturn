using System;
using System.Runtime.Serialization;
using static Saturn.Util.ByteUtil;

namespace Saturn.PE
{
    public class SECTION
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public SECTION(string name, uint sectionSize, byte[] content)
        {
            Name = name;

            switch (name)
            {
                case ".text":
                    byte[] assemblyData = new byte[sectionSize];
                    ConcatenateBytes(ref assemblyData, 0, content);

                    byte[] assemblyEmptyData = new byte[sectionSize - content.Length];
                    Array.Clear(assemblyEmptyData, 0, assemblyEmptyData.Length);
                    ConcatenateBytes(ref assemblyData, content.Length, assemblyEmptyData);

                    Data = assemblyData;
                    break;

                case ".data":
                    byte[] variableData = new byte[sectionSize];
                    ConcatenateBytes(ref variableData, 0, content);

                    byte[] variableEmptyData = new byte[sectionSize - content.Length];
                    Array.Clear(variableEmptyData, 0, variableEmptyData.Length);
                    ConcatenateBytes(ref variableData, content.Length, variableEmptyData);

                    Data = variableData;
                    break;

                default:
                    break;
            }
        }

        public byte[] ToBytes()
        {
            return Data;
        }
    }
}
