using System;
using System.Collections.Generic;
using static Saturn.Util.ByteUtil;
using static Saturn.Util.StringUtil;

namespace Saturn.MachineCode
{
    public class AssemblyHex
    {
        /// <summary>
        /// Flexible size
        /// Max 4 Bytes
        /// </summary>
        public byte[] Opcode { get; set; }

        /// <summary>
        /// Fixed Size
        /// Max 4 Bytes
        /// </summary>
        public byte[] Operand1 { get; set; }

        /// <summary>
        /// Fixed Size
        /// Max 4 Bytes
        /// </summary>
        public byte[] Operand2 { get; set; }

        public string String => ToString();
        public byte[] Bytes => ToBytes();

        /// <summary>
        /// If opcode starts with 0x00 byte, must set opcodeSize
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="operand1"></param>
        /// <param name="operand2"></param>
        /// <param name="opcodeSize"></param>
        public AssemblyHex(object opcode, object operand1 = null, object operand2 = null, int opcodeSize = 0)
        {
            Opcode = opcodeSize != 0 ?
                 opcode switch
                 {
                     null => throw new NullReferenceException("Opcode"),
                     _ => opcode.GetType().Name switch
                     {
                         string s when s.Equals("int") | s.Equals("Int32") => ReverseBytes(GetBytes(BitConverter.GetBytes((int)opcode), 0, opcodeSize)),
                         string s when s.Equals("uint") | s.Equals("UInt32") => ReverseBytes(GetBytes(BitConverter.GetBytes((uint)opcode), 0, opcodeSize)),
                         _ => null
                     }
                 }
                 :
                opcode switch
                {
                    null => throw new NullReferenceException("Opcode"),
                    _ => opcode.GetType().Name switch
                    {
                        string s when s.Equals("byte") | s.Equals("Byte") => new byte[] { (byte)opcode },
                        string s when s.Equals("byte[]") | s.Equals("Byte[]") => (byte[])opcode,
                        string s when s.Equals("short") | s.Equals("Int16") => TrimBytes(ReverseBytes(BitConverter.GetBytes((short)opcode))),
                        string s when s.Equals("ushort") | s.Equals("UInt16") => TrimBytes(ReverseBytes(BitConverter.GetBytes((ushort)opcode))),
                        string s when s.Equals("int") | s.Equals("Int32") => TrimBytes(ReverseBytes(BitConverter.GetBytes((int)opcode))),
                        string s when s.Equals("uint") | s.Equals("UInt32") => TrimBytes(ReverseBytes(BitConverter.GetBytes((uint)opcode))),
                        string s when s.Equals("long") | s.Equals("Int64") => TrimBytes(ReverseBytes(BitConverter.GetBytes((long)opcode))),
                        string s when s.Equals("ulong") | s.Equals("UInt64") => TrimBytes(ReverseBytes(BitConverter.GetBytes((ulong)opcode))),
                        _ => null
                    }
                };

            Operand1 = operand1 switch
            {
                null => null,
                _ => operand1.GetType().Name switch
                {
                    string s when s.Equals("byte") | s.Equals("Byte") => new byte[] { (byte)operand1 },
                    string s when s.Equals("byte[]") | s.Equals("Byte[]") => (byte[])operand1,
                    string s when s.Equals("short") | s.Equals("Int16") => BitConverter.GetBytes((short)operand1),
                    string s when s.Equals("ushort") | s.Equals("UInt16") => BitConverter.GetBytes((ushort)operand1),
                    string s when s.Equals("int") | s.Equals("Int32") => BitConverter.GetBytes((int)operand1),
                    string s when s.Equals("uint") | s.Equals("UInt32") => BitConverter.GetBytes((uint)operand1),
                    string s when s.Equals("long") | s.Equals("Int64") => BitConverter.GetBytes((long)operand1),
                    string s when s.Equals("ulong") | s.Equals("UInt64") => BitConverter.GetBytes((ulong)operand1),
                    _ => null
                }
            };

            Operand2 = operand2 switch
            {
                null => null,
                _ => operand2.GetType().Name switch
                {
                    string s when s.Equals("byte") | s.Equals("Byte") => new byte[] { (byte)operand2 },
                    string s when s.Equals("byte[]") | s.Equals("Byte[]") => (byte[])operand2,
                    string s when s.Equals("short") | s.Equals("Int16") => BitConverter.GetBytes((short)operand2),
                    string s when s.Equals("ushort") | s.Equals("UInt16") => BitConverter.GetBytes((ushort)operand2),
                    string s when s.Equals("int") | s.Equals("Int32") => BitConverter.GetBytes((int)operand2),
                    string s when s.Equals("uint") | s.Equals("UInt32") => BitConverter.GetBytes((uint)operand2),
                    string s when s.Equals("long") | s.Equals("Int64") => BitConverter.GetBytes((long)operand2),
                    string s when s.Equals("ulong") | s.Equals("UInt64") => BitConverter.GetBytes((ulong)operand2),
                    _ => null
                }
            };
        }

        new string ToString()
        {
            if (Opcode == null)
            {
                throw new NullReferenceException("Opcode");
            }

            if (Operand1 == null)
            {
                return GetHexString(Opcode);
            }

            if (Operand2 == null)
            {
                return GetHexString(Opcode) + GetHexString(Operand1);
            }

            return GetHexString(Opcode) + GetHexString(Operand1) + GetHexString(Operand2);
        }

        byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();

            if (Opcode == null)
            {
                throw new NullReferenceException("Opcode");
            }

            if (Operand1 == null)
            {
                return Opcode;
            }

            if (Operand2 == null)
            {
                ConcatenateBytes(ref bytes, Opcode);
                ConcatenateBytes(ref bytes, Operand1);

                return bytes.ToArray();
            }

            ConcatenateBytes(ref bytes, Opcode);
            ConcatenateBytes(ref bytes, Operand1);
            ConcatenateBytes(ref bytes, Operand2);

            return bytes.ToArray();
        }
    }
}
