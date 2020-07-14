using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _8D
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "15":
                    DisassemblyHelper.SetDisassemblyString("LEA EDX, DWORD PTR DS:[{0}]", DisassemblyHelper.OperandType.LE32);
                    break;

                case "45":
                    DisassemblyHelper.SetDisassemblyString("LEA EAX, DWORD PTR SS:[EBP{0}]", DisassemblyHelper.OperandType.SH8);
                    break;

                case "4C":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "24":
                            DisassemblyHelper.SetDisassemblyString("LEA ECX, DWORD PTR SS:[ESP{0}]", DisassemblyHelper.OperandType.SH8);
                            break;

                        default:
                            break;
                    }
                    break;

                case "85":
                    DisassemblyHelper.SetDisassemblyString("LEA EAX, DWORD PTR SS:[EBP{0}]", DisassemblyHelper.OperandType.SH32);
                    break;

                case "8C":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "05":
                            DisassemblyHelper.SetDisassemblyString("LEA ECX, DWORD PTR SS:[EBP+EAX{0}]", DisassemblyHelper.OperandType.SH32);
                            break;

                        default:
                            break;
                    }
                    break;

                case "8D":
                    DisassemblyHelper.SetDisassemblyString("LEA ECX, DWORD PTR SS:[EBP{0}]", DisassemblyHelper.OperandType.SH32);
                    break;

                case "94":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "0D":
                            DisassemblyHelper.SetDisassemblyString("LEA EDX, DWORD PTR SS:[EBP+ECX{0}]", DisassemblyHelper.OperandType.SH32);
                            break;

                        default:
                            break;
                    }
                    break;

                case "9B":
                    DisassemblyHelper.SetDisassemblyString("LEA EBX, DWORD PTR DS:[EBX{0}]", DisassemblyHelper.OperandType.SH32);
                    break;

                case "BD":
                    DisassemblyHelper.SetDisassemblyString("LEA EDI, DWORD PTR SS:[EBP{0}]", DisassemblyHelper.OperandType.SH32);
                    break;

                default:
                    break;
            }
        }
    }
}
