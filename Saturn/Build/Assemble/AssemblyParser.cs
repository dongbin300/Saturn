using Saturn.Assembly;
using System;
using System.Collections.Generic;
using System.Linq;
using static Saturn.Assembly.IAssembly;
using static Saturn.Util;

namespace Saturn.Build.Assemble
{
    public class AssemblyParser
    {
        public static (List<Instruction>, List<Variable>) Parse(string assemblyText, uint textSectionRawSize = 0x00000000, uint dataSectionRawSize = 0x00000000)
        {
            List<Instruction> instructions = new List<Instruction>();
            AddressManager addressManager = textSectionRawSize == 0 && dataSectionRawSize == 0 ? new AddressManager() : new AddressManager(textSectionRawSize, dataSectionRawSize);
            int lineNumber = 0;

            try
            {
                string[] instructionTexts = assemblyText.ToUpper().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string instructionText in instructionTexts)
                {
                    lineNumber++;

                    string instructionTextExcludeComment = instructionText.Split(';')[0];

                    if (instructionTextExcludeComment == string.Empty)
                    {
                        continue;
                    }

                    int firstSpaceIndex = instructionTextExcludeComment.IndexOf(' ');

                    if (firstSpaceIndex == -1) // Opcode + 0 Operand
                    {
                        OpcodeType opcode = (OpcodeType)ParseHead(instructionTextExcludeComment);
                        instructions.Add(new Instruction(opcode));
                    }
                    else
                    {
                        string headText = instructionTextExcludeComment.Substring(0, firstSpaceIndex).Trim();
                        object head = ParseHead(headText);

                        if (head is OpcodeType opcode)
                        {
                            string operandText = instructionTextExcludeComment.Substring(firstSpaceIndex + 1).Trim();
                            string[] operandTexts = operandText.Split(',').Select(p => p.Trim()).ToArray();

                            if (operandTexts.Length == 1) // Opcode + 1 Operand
                            {
                                object operand1 = ParseOperand(operandTexts[0], addressManager, opcode);
                                instructions.Add(new Instruction(opcode, operand1));
                            }
                            else if (operandTexts.Length == 2) // Opcode + 2 Operands
                            {
                                object operand1 = ParseOperand(operandTexts[0], addressManager, null, opcode);
                                object operand2 = ParseOperand(operandTexts[1], addressManager, operand1, opcode);
                                instructions.Add(new Instruction(opcode, operand1, operand2));
                            }
                        }

                        else if (head is DataType dataType)
                        {
                            string variableText = instructionTextExcludeComment.Substring(firstSpaceIndex + 1).Trim();
                            string[] variableTexts = variableText.Split(',').Select(p => p.Trim()).ToArray();

                            if (variableTexts.Length == 1) // DataType + Variable Name
                            {
                                string variableName = variableTexts[0];

                                if (addressManager.IsExistVariable(variableName))
                                {
                                    throw new Exception("[VariableNameOverlap] variable already exists.");
                                }

                                addressManager.AddVariable(dataType, variableName, 0);
                            }
                            else if (variableTexts.Length == 2) // DataType + Variable Name + Initial Value
                            {
                                string variableName = variableTexts[0];

                                if (addressManager.IsExistVariable(variableName))
                                {
                                    throw new Exception("[VariableNameOverlap] variable already exists.");
                                }

                                // Exception handling and add variable according to data type
                                switch (dataType)
                                {
                                    case DataType.BYTE:
                                        byte byteResult = ConvertToByte(variableTexts[1]);
                                        addressManager.AddVariable(dataType, variableName, byteResult);
                                        break;

                                    case DataType.WORD:
                                        ushort wordResult = ConvertToWord(variableTexts[1]);
                                        addressManager.AddVariable(dataType, variableName, wordResult);
                                        break;

                                    case DataType.DWORD:
                                        uint dwordResult = ConvertToDword(variableTexts[1]);
                                        addressManager.AddVariable(dataType, variableName, dwordResult);
                                        break;

                                    case DataType.QWORD:
                                        ulong qwordResult = ConvertToQword(variableTexts[1]);
                                        addressManager.AddVariable(dataType, variableName, qwordResult);
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ", line:" + lineNumber);
            }

            return (instructions, addressManager.Variables);
        }

        /// <summary>
        /// Parse Head Section
        /// i) Opcode
        ///     -ADD, MUL, RET, INC, PUSH, POP, MOV, ...
        /// ii) Data Type
        ///     -BYTE, WORD, DWORD, QWORD
        /// </summary>
        /// <param name="headText"></param>
        /// <returns></returns>
        static object ParseHead(string headText)
        {
            if (Enum.TryParse(typeof(OpcodeType), headText, out object result))
            {
                return result;
            }

            if (Enum.TryParse(typeof(DataType), headText, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Parse Operand
        /// i) Register-X Type
        ///     -R8, R16, R32, R64
        /// ii) Pointer-X Type
        ///     -PTR8, PTR16, PTR32, PTR64
        /// iii) Constant Pointer Type
        ///     -PTRC
        /// iv) Segment Type
        ///     -S
        /// v) Variable Type
        ///     -num1, num2, ...
        /// vi) Constant Value Type
        ///     -11, 12, 13, 14, ...
        /// </summary>
        /// <param name="operandText"></param>
        /// <returns></returns>
        static object ParseOperand(string operandText, AddressManager addressManager, object preArgument = null, OpcodeType opcode = OpcodeType.RET)
        {
            string[] operandTexts = operandText.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            if (operandTexts.Length == 1) // not PTR operand
            {
                // 175, 0x1034, ...
                if (StringUtil.IsHexaDecimalString(operandTexts[0]))
                {
                    // *) Special Case
                    switch(opcode)
                    {
                        case OpcodeType.IN:
                        case OpcodeType.OUT:
                        case OpcodeType.RCL:
                        case OpcodeType.RCR:
                        case OpcodeType.ROL:
                        case OpcodeType.ROR:
                        case OpcodeType.SHL:
                        case OpcodeType.SHR:
                            return ConvertToByte(operandTexts[0]);

                        default:
                            break;
                    }

                    // vi) Constant Value Type
                    if (preArgument is PTRC ptrc)
                    {
                        switch (ptrc.DataType)
                        {
                            case DataType.BYTE:
                                return ConvertToByte(operandTexts[0]);

                            case DataType.WORD:
                                return ConvertToWord(operandTexts[0]);

                            case DataType.DWORD:
                                return ConvertToDword(operandTexts[0]);

                            case DataType.QWORD:
                                return ConvertToQword(operandTexts[0]);

                            default:
                                break;
                        }
                    }

                    if(preArgument is PTR8 || preArgument is R8)
                    {
                        return ConvertToByte(operandTexts[0]);
                    }
                    if (preArgument is PTR16 || preArgument is R16)
                    {
                        return ConvertToWord(operandTexts[0]);
                    }
                    if (preArgument is PTR32 || preArgument is R32)
                    {
                        return ConvertToDword(operandTexts[0]);
                    }
                    if (preArgument is PTR64 || preArgument is R64)
                    {
                        return ConvertToQword(operandTexts[0]);
                    }

                    if(preArgument is OpcodeType opcodeType)
                    {
                        switch (opcodeType)
                        {
                            case OpcodeType.PUSH:
                            case OpcodeType.JO:
                            case OpcodeType.JNO:
                            case OpcodeType.JB:
                            case OpcodeType.JAE:
                            case OpcodeType.JE:
                            case OpcodeType.JNE:
                            case OpcodeType.JBE:
                            case OpcodeType.JA:
                            case OpcodeType.JS:
                            case OpcodeType.JNS:
                            case OpcodeType.JP:
                            case OpcodeType.JNP:
                            case OpcodeType.JL:
                            case OpcodeType.JGE:
                            case OpcodeType.JLE:
                            case OpcodeType.JG:
                            case OpcodeType.INT:
                            case OpcodeType.AAM:
                            case OpcodeType.AAD:
                            case OpcodeType.LOOPNE:
                            case OpcodeType.LOOPE:
                            case OpcodeType.LOOP:
                            case OpcodeType.JECXZ:
                            case OpcodeType.JMP:
                                return ConvertToByte(operandTexts[0]);

                            default:
                                break;
                        }
                    }
                }
                else
                {
                    // i) Register-X Type
                    if (Enum.TryParse(typeof(R8), operandTexts[0], out object result))
                        return result;

                    if (Enum.TryParse(typeof(R16), operandTexts[0], out result))
                        return result;

                    if (Enum.TryParse(typeof(R32), operandTexts[0], out result))
                        return result;

                    if (Enum.TryParse(typeof(R64), operandTexts[0], out result))
                        return result;

                    // iv) Segment Type
                    if (Enum.TryParse(typeof(S), operandTexts[0], out result))
                        return result;

                    // v) Variable Type
                    if (addressManager.IsExistVariable(operandTexts[0]))
                    {
                        Variable variable = addressManager.GetVariable(operandTexts[0]);

                        return new PTRC(variable.DataType, variable.Address);
                    }
                    else
                    {
                        throw new Exception($"[VariableGetError] variable doesn't exist.\n->{operandTexts[0]}");
                    }
                }
            }
            else if (operandTexts.Length == 2) // PTR operand
            {
                // Remove 'PTR' Keyword
                operandTexts[0] = operandTexts[0].Replace("PTR", "").Trim();

                // iii) Constant Pointer Type
                if (operandTexts[1][0] >= '0' && operandTexts[1][0] <= '9')
                {
                    var dataType = (DataType)Enum.Parse(typeof(DataType), operandTexts[0]);
                    return new PTRC(dataType, HexStringToUInt(operandTexts[1]));
                }
                // ii) Pointer-X Type
                else
                {
                    try
                    {
                        return (operandTexts[0]) switch
                        {
                            "BYTE" => Enum.Parse(typeof(PTR8), operandTexts[1]),
                            "WORD" => Enum.Parse(typeof(PTR16), operandTexts[1]),
                            "DWORD" => Enum.Parse(typeof(PTR32), operandTexts[1]),
                            "FWORD" => Enum.Parse(typeof(PTR48), operandTexts[1]),
                            "QWORD" => Enum.Parse(typeof(PTR64), operandTexts[1]),
                            _ => null,
                        };
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        static byte ConvertToByte(string numericString)
        {
            byte result;

            if (numericString.StartsWith("0X"))
            {
                result = Convert.ToByte(numericString, 16);
            }
            else if (!byte.TryParse(numericString, out result))
            {
                throw new Exception("[VariableParse] value range exceeded.");
            }

            return result;
        }

        static ushort ConvertToWord(string numericString)
        {
            ushort result;

            if (numericString.StartsWith("0X"))
            {
                result = Convert.ToUInt16(numericString, 16);
            }
            else if (!ushort.TryParse(numericString, out result))
            {
                throw new Exception("[VariableParse] value range exceeded.");
            }
            return result;
        }

        static uint ConvertToDword(string numericString)
        {
            uint result;

            if (numericString.StartsWith("0X"))
            {
                result = Convert.ToUInt32(numericString, 16);
            }
            else if (!uint.TryParse(numericString, out result))
            {
                throw new Exception("[VariableParse] value range exceeded.");
            }
            return result;
        }

        static ulong ConvertToQword(string numericString)
        {
            ulong result;

            if (numericString.StartsWith("0X"))
            {
                result = Convert.ToUInt64(numericString, 16);
            }
            else if (!ulong.TryParse(numericString, out result))
            {
                throw new Exception("[VariableParse] value range exceeded.");
            }
            return result;
        }
    }
}
