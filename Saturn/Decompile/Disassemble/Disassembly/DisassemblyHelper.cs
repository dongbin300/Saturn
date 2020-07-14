using Saturn.PE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using static Saturn.Util.StringUtil;
using static Saturn.Util.ByteUtil;

namespace Saturn.Decompile.Disassemble.Disassembly
{
    public class DisassemblyHelper
    {
        /// <summary>
        /// Little Endian 8
        /// Little Endian 16
        /// Little Endian 32
        /// Signed Hex
        /// Jump Opcode
        /// </summary>
        public enum OperandType
        {
            None,
            LE8,
            LE16,
            LE32,
            SH8,
            SH16,
            SH32,
            J8,
            J16,
            J32
        }

        public static byte[] data = null;
        public static int interval = 0;
        public static int i = 0;
        public static byte[] opcode1 = null;
        public static byte[] opcode2 = null;
        public static byte[] opcode3 = null;
        public static byte[] opcode4 = null;
        public static byte[] operand1 = null;
        public static byte[] operand2 = null;
        public static byte[] operand3 = null;
        public static byte[] operand4 = null;
        public static StringBuilder disassembly = new StringBuilder();
        public static uint currentAddress = 0;

        public static int success = 0;
        public static int fail = 0;

        public static List<Instruction> MakeInstructionTable(PEFileDisassembly peFileDisassembly)
        {
            try
            {
                List<Instruction> instructions = new List<Instruction>();

                var baseAddress = peFileDisassembly.BaseAddress;
                data = peFileDisassembly.Data;
                interval = peFileDisassembly.BassAddressInterval;

                i = 0;

                while (i < data.Length)
                {
                    // initialize
                    opcode1 = opcode2 = opcode3 = opcode4 = null;
                    operand1 = operand2 = operand3 = operand4 = null;
                    disassembly.Clear();

                    currentAddress = (uint)(baseAddress + i);
                    GetOpcode();

                    // parse hex
                    try
                    {
                        Type t = Type.GetType("Saturn.Decompile.Disassemble.Disassembly.Parser._" + GetHexString(opcode1));
                        MethodInfo method = t.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
                        method.Invoke(null, null);
                    }
                    catch
                    {

                    }

                    // arrange opcode & operand and make hex bytes
                    byte[] OPCODE =
                        opcode2 == null ? opcode1.ToArray() :
                        opcode3 == null ? opcode1.Concat(opcode2).ToArray() :
                        opcode4 == null ? opcode1.Concat(opcode2).Concat(opcode3).ToArray() :
                        opcode1.Concat(opcode2).Concat(opcode3).Concat(opcode4).ToArray();

                    byte[] OPERAND =
                        operand1 == null ? null :
                        operand2 == null ? operand1.ToArray() :
                        operand3 == null ? operand1.Concat(operand2).ToArray() :
                        operand4 == null ? operand1.Concat(operand2).Concat(operand3).ToArray() :
                        operand1.Concat(operand2).Concat(operand3).Concat(operand4).ToArray();

                    byte[] hexBytes =
                        OPERAND == null ? OPCODE :
                        OPCODE.Concat(OPERAND).ToArray();

                    // make instructions
                    instructions.Add(new Instruction(
                        currentAddress, hexBytes, disassembly.ToString().ToLower()
                        ));

                    // check progress
                    if (string.IsNullOrEmpty(disassembly.ToString()))
                    {
                        fail++;
                    }
                    else
                    {
                        success++;
                    }
                }

                return instructions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void GetOpcode()
        {
            if (opcode1 == null)
            {
                opcode1 = new byte[] { data[i++] };
            }
            else if (opcode2 == null)
            {
                opcode2 = new byte[] { data[i++] };
            }
            else if (opcode3 == null)
            {
                opcode3 = new byte[] { data[i++] };
            }
            else if (opcode4 == null)
            {
                opcode4 = new byte[] { data[i++] };
            }
        }

        public static void GetOperand(int count)
        {
            switch (count)
            {
                case 1:
                    operand1 = GetBytes(data, i, 4);
                    i += 4;
                    break;
                case 2:
                    operand1 = GetBytes(data, i, 4);
                    operand2 = GetBytes(data, i + 4, 4);
                    i += 8;
                    break;
                default:
                    break;
            }
        }

        public static void GetOperand16(int count)
        {
            switch (count)
            {
                case 1:
                    operand1 = GetBytes(data, i, 2);
                    i += 2;
                    break;
                case 2:
                    operand1 = GetBytes(data, i, 2);
                    operand2 = GetBytes(data, i + 2, 2);
                    i += 4;
                    break;
                default:
                    break;
            }
        }

        public static void GetOperand8(int count)
        {
            switch (count)
            {
                case 1:
                    operand1 = GetBytes(data, i, 1);
                    i++;
                    break;
                case 2:
                    operand1 = GetBytes(data, i, 1);
                    operand2 = GetBytes(data, i + 1, 1);
                    i += 2;
                    break;
                default:
                    break;
            }
        }

        public static void SetDisassemblyString(string assembly,
            OperandType operand1Type = OperandType.None,
            OperandType operand2Type = OperandType.None,
            OperandType operand3Type = OperandType.None,
            OperandType operand4Type = OperandType.None)
        {
            if (operand1Type == OperandType.None)
            {
                disassembly.Append(assembly);
                return;
            }

            GetOperandsFormat(operand1Type, operand2Type, operand3Type, operand4Type);

            if(operand2Type == OperandType.None)
            {
                disassembly.AppendFormat(assembly, SetOperand(operand1, operand1Type));
                return;
            }

            if (operand3Type == OperandType.None)
            {
                disassembly.AppendFormat(assembly, SetOperand(operand1, operand1Type), SetOperand(operand2, operand2Type));
                return;
            }

            if (operand4Type == OperandType.None)
            {
                disassembly.AppendFormat(assembly, SetOperand(operand1, operand1Type), SetOperand(operand2, operand2Type), SetOperand(operand3, operand3Type));
                return;
            }

            disassembly.AppendFormat(assembly, SetOperand(operand1, operand1Type), SetOperand(operand2, operand2Type), SetOperand(operand3, operand3Type), SetOperand(operand4, operand4Type));
        }

        /// <summary>
        /// 오퍼랜드 출력
        /// 1. 1바이트 출력(l-e) : 변환없이 바로 Hex String
        /// 2. 2바이트 출력(l-e) : Reverse 후 Hex String
        /// 3. 4바이트 출력(l-e) : Reverse 후 Hex String
        /// 4. SignedHexString 출력 : Util.UnsignedToSignedHexString
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static string SetOperand(byte[] operand, OperandType type)
        {
            switch (type)
            {
                case OperandType.SH8:
                case OperandType.SH16:
                case OperandType.SH32:
                    return UnsignedToSignedHexString(operand);
                case OperandType.LE8:
                    return GetHexString(operand);
                case OperandType.LE16:
                case OperandType.LE32:
                    return GetHexString(ReverseBytes(operand));
                case OperandType.J8:
                    return GetHexString(ReverseBytes(BitConverter.GetBytes((byte)(currentAddress + operand[0] + GetOpcodeCount() + 1))));
                case OperandType.J16:
                    return GetHexString(ReverseBytes(BitConverter.GetBytes((ushort)(currentAddress + BitConverter.ToUInt32(operand) + GetOpcodeCount() + 2))));
                case OperandType.J32:
                    return GetHexString(ReverseBytes(BitConverter.GetBytes((uint)(currentAddress + BitConverter.ToUInt32(operand) + GetOpcodeCount() + 4))));
                default:
                    return string.Empty;
            }
        }

        public static void GetOperand(OperandType type, int operandNumber)
        {
            int byteSize = 0;

            switch (type)
            {
                case OperandType.LE8:
                case OperandType.SH8:
                case OperandType.J8:
                    byteSize = 1;
                    break;

                case OperandType.LE16:
                case OperandType.SH16:
                case OperandType.J16:
                    byteSize = 2;
                    break;

                case OperandType.LE32:
                case OperandType.SH32:
                case OperandType.J32:
                    byteSize = 4;
                    break;
            }

            var operand = GetBytes(data, i, byteSize);
            switch (operandNumber)
            {
                case 1:
                    operand1 = operand;
                    break;
                case 2:
                    operand2 = operand;
                    break;
                case 3:
                    operand3 = operand;
                    break;
                case 4:
                    operand4 = operand;
                    break;
                default:
                    break;
            }
            i += byteSize;
        }

        public static void GetOperands(OperandType type, int count = 1)
        {
            switch (type)
            {
                case OperandType.LE8:
                case OperandType.SH8:
                case OperandType.J8:
                    switch (count)
                    {
                        case 1:
                            operand1 = GetBytes(data, i, 1);
                            i++;
                            break;
                        case 2:
                            operand1 = GetBytes(data, i, 1);
                            operand2 = GetBytes(data, i + 1, 1);
                            i += 2;
                            break;
                        case 3:
                            operand1 = GetBytes(data, i, 1);
                            operand2 = GetBytes(data, i + 1, 1);
                            operand3 = GetBytes(data, i + 2, 1);
                            i += 3;
                            break;
                        case 4:
                            operand1 = GetBytes(data, i, 1);
                            operand2 = GetBytes(data, i + 1, 1);
                            operand3 = GetBytes(data, i + 2, 1);
                            operand4 = GetBytes(data, i + 3, 1);
                            i += 4;
                            break;
                        default:
                            break;
                    }
                    break;

                case OperandType.LE16:
                case OperandType.SH16:
                case OperandType.J16:
                    switch (count)
                    {
                        case 1:
                            operand1 = GetBytes(data, i, 2);
                            i += 2;
                            break;
                        case 2:
                            operand1 = GetBytes(data, i, 2);
                            operand2 = GetBytes(data, i + 2, 2);
                            i += 4;
                            break;
                        case 3:
                            operand1 = GetBytes(data, i, 2);
                            operand2 = GetBytes(data, i + 2, 2);
                            operand3 = GetBytes(data, i + 4, 2);
                            i += 6;
                            break;
                        case 4:
                            operand1 = GetBytes(data, i, 2);
                            operand2 = GetBytes(data, i + 2, 2);
                            operand3 = GetBytes(data, i + 4, 2);
                            operand4 = GetBytes(data, i + 6, 2);
                            i += 8;
                            break;
                        default:
                            break;
                    }
                    break;

                case OperandType.LE32:
                case OperandType.SH32:
                case OperandType.J32:
                    switch (count)
                    {
                        case 1:
                            operand1 = GetBytes(data, i, 4);
                            i += 4;
                            break;
                        case 2:
                            operand1 = GetBytes(data, i, 4);
                            operand2 = GetBytes(data, i + 4, 4);
                            i += 8;
                            break;
                        case 3:
                            operand1 = GetBytes(data, i, 4);
                            operand2 = GetBytes(data, i + 4, 4);
                            operand3 = GetBytes(data, i + 8, 4);
                            i += 12;
                            break;
                        case 4:
                            operand1 = GetBytes(data, i, 4);
                            operand2 = GetBytes(data, i + 4, 4);
                            operand3 = GetBytes(data, i + 8, 4);
                            operand4 = GetBytes(data, i + 12, 4);
                            i += 16;
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        public static void GetOperandsFormat(
            OperandType operand1Type = OperandType.None,
            OperandType operand2Type = OperandType.None,
            OperandType operand3Type = OperandType.None,
            OperandType operand4Type = OperandType.None)
        {
            if (operand1Type == OperandType.None)
            {
                return;
            }

            GetOperand(operand1Type, 1);

            if (operand2Type == OperandType.None)
            {
                return;
            }

            GetOperand(operand2Type, 2);

            if (operand3Type == OperandType.None)
            {
                return;
            }

            GetOperand(operand3Type, 3);

            if (operand4Type == OperandType.None)
            {
                return;
            }

            GetOperand(operand4Type, 4);
        }

        public static void GetOperandFormat(int size1 = 0, int size2 = 0, int size3 = 0, int size4 = 0)
        {
            if (size1 != 0)
            {
                operand1 = GetBytes(data, i, size1);
                i += size1;
            }
            if (size2 != 0)
            {
                operand2 = GetBytes(data, i, size2);
                i += size2;
            }
            if (size3 != 0)
            {
                operand3 = GetBytes(data, i, size3);
                i += size3;
            }
            if (size4 != 0)
            {
                operand4 = GetBytes(data, i, size4);
                i += size4;
            }
        }

        public static int GetOpcodeCount()
        {
            if (opcode1 == null)
                return 0;
            else if (opcode2 == null)
                return 1;
            else if (opcode3 == null)
                return 2;
            else if (opcode4 == null)
                return 3;
            else
                return 4;
        }
    }
}
