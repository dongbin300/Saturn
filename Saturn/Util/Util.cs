using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Saturn
{
    public class Util
    {
        /// <summary>
        /// Big Endian
        /// </summary>
        /// <param name="input"></param>
        /// <param name="autoSize"></param>
        /// <returns></returns>
        public static string GetHexString(object input, bool autoSize = false)
        {
            int len = 0;
            char[] ch = null;
            Type t = input.GetType();

            switch (t.Name)
            {
                case "byte":
                case "Byte":
                    len = 2;
                    ch = new char[len--];
                    for (int i = len; i >= 0; i--)
                    {
                        ch[len - i] = ByteToHexChar((byte)((uint)((byte)input >> 4 * i) & 15));
                    }
                    break;

                case "byte[]":
                case "Byte[]":
                    var byteLength = ((byte[])input).Length;
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < byteLength; i++)
                    {
                        builder.Append(GetHexString(((byte[])input)[i]));
                    }
                    return builder.ToString();

                case "short":
                case "Int16":
                    len = 4;
                    ch = new char[len--];
                    if (autoSize)
                    {
                        return GetHexString(TrimBytes(ReverseBytes(BitConverter.GetBytes((short)input))));
                    }
                    else
                    {
                        for (int i = len; i >= 0; i--)
                        {
                            ch[len - i] = ByteToHexChar((byte)((uint)((short)input >> 4 * i) & 15));
                        }
                    }
                    break;

                case "ushort":
                case "UInt16":
                    len = 4;
                    ch = new char[len--];
                    if (autoSize)
                    {
                        return GetHexString(TrimBytes(ReverseBytes(BitConverter.GetBytes((ushort)input))));
                    }
                    else
                    {
                        for (int i = len; i >= 0; i--)
                        {
                            ch[len - i] = ByteToHexChar((byte)((uint)((ushort)input >> 4 * i) & 15));
                        }
                    }
                    break;

                case "int":
                case "Int32":
                    len = 8;
                    ch = new char[len--];
                    if (autoSize)
                    {
                        return GetHexString(TrimBytes(ReverseBytes(BitConverter.GetBytes((int)input))));
                    }
                    else
                    {
                        for (int i = len; i >= 0; i--)
                        {
                            ch[len - i] = ByteToHexChar((byte)((uint)((int)input >> 4 * i) & 15));
                        }
                    }
                    break;

                case "uint":
                case "UInt32":
                    len = 8;
                    ch = new char[len--];
                    if (autoSize)
                    {
                        return GetHexString(TrimBytes(ReverseBytes(BitConverter.GetBytes((uint)input))));
                    }
                    else
                    {
                        for (int i = len; i >= 0; i--)
                        {
                            ch[len - i] = ByteToHexChar((byte)(((uint)input >> 4 * i) & 15));
                        }
                    }
                    break;

                case "long":
                case "Int64":
                    len = 16;
                    ch = new char[len--];
                    if (autoSize)
                    {
                        return GetHexString(TrimBytes(ReverseBytes(BitConverter.GetBytes((long)input))));
                    }
                    else
                    {
                        for (int i = len; i >= 0; i--)
                        {
                            ch[len - i] = ByteToHexChar((byte)((uint)((long)input >> 4 * i) & 15));
                        }
                    }
                    break;

                case "ulong":
                case "UInt64":
                    len = 16;
                    ch = new char[len--];
                    if (autoSize)
                    {
                        return GetHexString(TrimBytes(ReverseBytes(BitConverter.GetBytes((ulong)input))));
                    }
                    else
                    {
                        for (int i = len; i >= 0; i--)
                        {
                            ch[len - i] = ByteToHexChar((byte)((uint)((ulong)input >> 4 * i) & 15));
                        }
                    }
                    break;

                default:
                    return string.Empty;
            }

            return new string(ch);
        }

        public static byte[] GetBytes(byte[] data, int startIndex, int count)
        {
            byte[] data2 = new byte[count];
            Buffer.BlockCopy(data, startIndex, data2, 0, count);

            return data2;
        }

        public static byte[] ReverseBytes(byte[] data)
        {
            byte[] data2 = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                data2[i] = data[data.Length - i - 1];
            }

            return data2;
        }

        public static short ReverseBytes(short data)
        {
            return BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(data)));
        }

        public static ushort ReverseBytes(ushort data)
        {
            return BitConverter.ToUInt16(ReverseBytes(BitConverter.GetBytes(data)));
        }

        public static int ReverseBytes(int data)
        {
            return BitConverter.ToInt32(ReverseBytes(BitConverter.GetBytes(data)));
        }

        public static uint ReverseBytes(uint data)
        {
            return BitConverter.ToUInt32(ReverseBytes(BitConverter.GetBytes(data)));
        }

        public static long ReverseBytes(long data)
        {
            return BitConverter.ToInt64(ReverseBytes(BitConverter.GetBytes(data)));
        }

        public static ulong ReverseBytes(ulong data)
        {
            return BitConverter.ToUInt64(ReverseBytes(BitConverter.GetBytes(data)));
        }

        public static int IndexOfBytes(byte[] data, byte[] searchBytes)
        {
            if (searchBytes.Length > data.Length)
            {
                return -1;
            }

            for (int i = 0; i < data.Length - searchBytes.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    if (data[i + j] != searchBytes[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        private static char ByteToHexChar(byte b)
        {
            return b < 10 ? (char)(b + 48) : (char)(b + 55);
        }

        public static byte HexCharToByte(char c)
        {
            return c < 65 ? (byte)(c - 48) : (byte)(c - 55);
        }

        public static string UnsignedToSignedHexString(byte[] bytes)
        {
            switch (bytes.Length)
            {
                case 1:
                    sbyte int8 = (sbyte)bytes[0];
                    return (int8 >= 0 ? "+" : "-") + GetHexString(Math.Abs(int8));

                case 2:
                    short int16 = BitConverter.ToInt16(bytes);
                    return (int16 >= 0 ? "+" : "-") + GetHexString(Math.Abs(int16));

                case 4:
                    int int32 = BitConverter.ToInt32(bytes);
                    return (int32 >= 0 ? "+" : "-") + GetHexString(Math.Abs(int32));

                default:
                    return string.Empty;
            }
        }

        public static byte[] HexStringToBytes(string str)
        {
            string str2 = str.ToUpper().Replace(" ", "");
            int length = str2.Length;

            if (length % 2 == 1)
            {
                str2 = "0" + str2;
                length++;
            }

            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = (byte)((byte)(HexCharToByte(str2[i]) << 4) + HexCharToByte(str2[i + 1]));
            }

            return bytes;
        }

        public static short HexStringToShort(string str, bool isLittleEndian = false)
        {
            return BitConverter.ToInt16(isLittleEndian ? HexStringToBytes(str) : ReverseBytes(HexStringToBytes(str)));
        }

        public static ushort HexStringToUShort(string str, bool isLittleEndian = false)
        {
            return BitConverter.ToUInt16(isLittleEndian ? HexStringToBytes(str) : ReverseBytes(HexStringToBytes(str)));
        }

        public static int HexStringToInt(string str, bool isLittleEndian = false)
        {
            string str2 = new string('0', 8 - str.Length) + str;

            return BitConverter.ToInt32(isLittleEndian ? HexStringToBytes(str2) : ReverseBytes(HexStringToBytes(str2)));
        }

        public static uint HexStringToUInt(string str, bool isLittleEndian = false)
        {
            return BitConverter.ToUInt32(isLittleEndian ? HexStringToBytes(str) : ReverseBytes(HexStringToBytes(str)));
        }

        public static long HexStringToLong(string str, bool isLittleEndian = false)
        {
            return BitConverter.ToInt64(isLittleEndian ? HexStringToBytes(str) : ReverseBytes(HexStringToBytes(str)));
        }

        public static ulong HexStringToULong(string str, bool isLittleEndian = false)
        {
            return BitConverter.ToUInt64(isLittleEndian ? HexStringToBytes(str) : ReverseBytes(HexStringToBytes(str)));
        }

        public static void ConcatenateHexStringBytes(ref byte[] bytes, int index, string target)
        {
            var targetBytes = HexStringToBytes(target);

            Buffer.BlockCopy(targetBytes, 0, bytes, index, targetBytes.Length);
        }

        public static void ConcatenateHexStringBytes(ref List<byte> bytes, string target)
        {
            var targetBytes = HexStringToBytes(target);

            bytes.AddRange(targetBytes.ToList());
        }

        public static void ConcatenateBytes(ref byte[] bytes, int index, object target, bool isLittleEndian = false)
        {
            byte[] targetBytes = null;
            Type t = target.GetType();

            //var convertedTarget = typeof(ValueTypeUtil).GetMethod("Convert", BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(t).Invoke(null, new object[] { target });

            switch (t.Name)
            {
                case "byte":
                case "Byte":
                    targetBytes = new byte[] { (byte)target };
                    break;
                case "byte[]":
                case "Byte[]":
                    targetBytes = (byte[])target;
                    break;
                case "short":
                case "Int16":
                    targetBytes = BitConverter.GetBytes((short)target);
                    break;
                case "ushort":
                case "UInt16":
                    targetBytes = BitConverter.GetBytes((ushort)target);
                    break;
                case "int":
                case "Int32":
                    targetBytes = BitConverter.GetBytes((int)target);
                    break;
                case "uint":
                case "UInt32":
                    targetBytes = BitConverter.GetBytes((uint)target);
                    break;
                case "long":
                case "Int64":
                    targetBytes = BitConverter.GetBytes((long)target);
                    break;
                case "ulong":
                case "UInt64":
                    targetBytes = BitConverter.GetBytes((ulong)target);
                    break;
                default:
                    break;
            }

            if (isLittleEndian)
            {
                targetBytes = ReverseBytes(targetBytes);
            }

            Buffer.BlockCopy(targetBytes, 0, bytes, index, targetBytes.Length);
        }

        public static void ConcatenateBytes(ref List<byte> bytes, object target, bool isLittleEndian = false)
        {
            byte[] targetBytes = null;
            Type t = target.GetType();

            switch (t.Name)
            {
                case "byte":
                case "Byte":
                    targetBytes = new byte[] { (byte)target };
                    break;
                case "byte[]":
                case "Byte[]":
                    targetBytes = (byte[])target;
                    break;
                case "short":
                case "Int16":
                    targetBytes = BitConverter.GetBytes((short)target);
                    break;
                case "ushort":
                case "UInt16":
                    targetBytes = BitConverter.GetBytes((ushort)target);
                    break;
                case "int":
                case "Int32":
                    targetBytes = BitConverter.GetBytes((int)target);
                    break;
                case "uint":
                case "UInt32":
                    targetBytes = BitConverter.GetBytes((uint)target);
                    break;
                case "long":
                case "Int64":
                    targetBytes = BitConverter.GetBytes((long)target);
                    break;
                case "ulong":
                case "UInt64":
                    targetBytes = BitConverter.GetBytes((ulong)target);
                    break;
                default:
                    break;
            }

            if (isLittleEndian)
            {
                targetBytes = ReverseBytes(targetBytes);
            }

            bytes.AddRange(targetBytes.ToList());
        }

        /// <summary>
        /// only trim start
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] TrimBytes(byte[] data)
        {
            while(data[0] == 0)
            {
                data = GetBytes(data, 1, data.Length - 1);
            }

            return data;
        }
    }
}
