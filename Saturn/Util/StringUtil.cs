using System;
using System.Text;
using System.Text.RegularExpressions;
using static Saturn.Util.ByteUtil;

namespace Saturn.Util
{
    public class StringUtil
    {
        public static bool IsNumericString(string str)
        {
            return decimal.TryParse(str, out decimal _);
        }

        public static bool IsHexaDecimalString(string str)
        {
            if(str.StartsWith("0X") || str.EndsWith("H")) // hexa-decimal
            {
                return Regex.IsMatch(str.Replace("0X", "").Replace("H", ""), @"^[A-F0-9]+$");
            }
            else // decimal
            {
                return IsNumericString(str);
            }
        }

        /// <summary>
        /// Big Endian
        /// (byte)15 -> "0F"
        /// (int)15 -> "0000000F"
        /// </summary>
        /// <param name="input">input type: byte, byte[], short, ushort, int, uint, long, ulong</param>
        /// <param name="autoSize"></param>
        /// <returns></returns>
        public static string GetHexString(object input, bool autoSize = false)
        {
            char[] ch;
            int len;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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
    }
}
