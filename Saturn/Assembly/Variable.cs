using System;
using System.Collections.Generic;
using System.Text;
using static Saturn.Assembly.IAssembly;
using static Saturn.Util;

namespace Saturn.Assembly
{
    public class Variable
    {
        public DataType DataType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public uint Address { get; set; }
        public string LEString => ToLittleEndianString();
        public string BEString => ToBigEndianString();

        public Variable(DataType dataType, string name, object value, uint address)
        {
            DataType = dataType;
            Name = name;
            Value = value;
            Address = address;
        }

        private string ToLittleEndianString()
        {
            return DataType switch
            {
                DataType.BYTE => GetHexString(Value),
                DataType.WORD => GetHexString(BitConverter.GetBytes((short)Value)),
                DataType.DWORD => GetHexString(BitConverter.GetBytes((int)Value)),
                DataType.QWORD => GetHexString(BitConverter.GetBytes((long)Value)),
                _ => null,
            };
        }

        private string ToBigEndianString()
        {
            return DataType switch
            {
                DataType.BYTE => GetHexString(Value),
                DataType.WORD => GetHexString(ReverseBytes(BitConverter.GetBytes((short)Value))),
                DataType.DWORD => GetHexString(ReverseBytes(BitConverter.GetBytes((int)Value))),
                DataType.QWORD => GetHexString(ReverseBytes(BitConverter.GetBytes((long)Value))),
                _ => null,
            };
        }
    }
}
