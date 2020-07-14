using System;
using System.Collections.Generic;
using System.Linq;
using static Saturn.Util.StringUtil;

namespace Saturn.Util
{
    public class ByteUtil
    {
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

        public static char ByteToHexChar(byte b)
        {
            return b < 10 ? (char)(b + 48) : (char)(b + 55);
        }

        public static byte HexCharToByte(char c)
        {
            return c < 65 ? (byte)(c - 48) : (byte)(c - 55);
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
            while (data[0] == 0)
            {
                data = GetBytes(data, 1, data.Length - 1);
            }

            return data;
        }
    }
}
