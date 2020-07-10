using Saturn.Assembly;
using Saturn.Build;
using static Saturn.Assembly.IAssembly;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;

namespace Saturn.Build.Assemble
{
    public class AddressManager
    {
        /* Optional Header */
        public static uint ImageBaseAddress = 0x00400000;
        public static ulong ImageBase64Address = 0x0000000000400000;
        public static uint SectionAlignment = 0x00001000;
        public static uint FileAlignment = 0x00000200;
        public static uint EntryPointAddress = 0x00001000;
        public static uint ImageSize = 0x00003000;

        /* Text Section */
        public static uint TextSectionSize; // 0x00001000
        public static uint TextSectionRVA; // 0x00001000
        public static uint TextSectionRawSize; // 0x00000200
        public static uint TextSectionRawPointer; // 0x00000200

        /* Data Section */
        public static uint DataSectionSize; // 0x00001000
        public static uint DataSectionRVA; // 0x00002000
        public static uint DataSectionRawSize; // 0x00000200
        public static uint DataSectionRawPointer; // 0x00000400

        public List<Variable> Variables;

        public uint DataSectionCurrentAddress;

        public AddressManager()
        {
            Variables = new List<Variable>();

            TextSectionSize = 0x00001000;
            DataSectionSize = 0x00001000;
            TextSectionRVA = EntryPointAddress;
            DataSectionRVA = TextSectionRVA + TextSectionSize;
            TextSectionRawSize = 0x00000200;
            DataSectionRawSize = 0x00000200;
            TextSectionRawPointer = 0x00000200;
            DataSectionRawPointer = TextSectionRawPointer + TextSectionRawSize;

            DataSectionCurrentAddress = (BuildEnvironment.format == BuildEnvironment.PEFormat.PE32 ? ImageBaseAddress : (uint)ImageBase64Address) + DataSectionRVA;
        }

        public AddressManager(uint textSectionRawSize, uint dataSectionRawSize)
        {
            Variables = new List<Variable>();

            TextSectionSize = 0x00001000;
            DataSectionSize = 0x00001000;
            TextSectionRVA = EntryPointAddress;
            DataSectionRVA = TextSectionRVA + TextSectionSize;
            TextSectionRawSize = textSectionRawSize;
            DataSectionRawSize = dataSectionRawSize;
            TextSectionRawPointer = 0x00000200;
            DataSectionRawPointer = TextSectionRawPointer + TextSectionRawSize;
            
            DataSectionCurrentAddress = (BuildEnvironment.format == BuildEnvironment.PEFormat.PE32 ? ImageBaseAddress : (uint)ImageBase64Address) + DataSectionRVA;
        }

        public void AddVariable(DataType dataType, string name, object value)
        {
            Variable variable = new Variable(dataType, name, value, DataSectionCurrentAddress);
            
            Variables.Add(variable);

            DataSectionCurrentAddress += dataType switch
            {
                DataType.BYTE => 1,
                DataType.WORD => 2,
                DataType.DWORD => 4,
                DataType.QWORD => 8,
                _ => 0
            };
        }

        public bool IsExistVariable(string variableName) => GetVariable(variableName) != null;

        public Variable GetVariable(string variableName) => Variables.Find(v => v.Name.Equals(variableName));

        public static uint GetSectionRawSize(byte[] bytes) => BitUtil.SufficientValue((uint)bytes.Length, 9);
    }
}
