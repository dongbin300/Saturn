using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _FF
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "04":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "00":
                            DisassemblyHelper.SetDisassemblyString("INC DWORD PTR DS:[EAX+EAX]");
                            break;

                        default:
                            break;
                    }
                    break;

                case "15":
                    DisassemblyHelper.SetDisassemblyString("CALL DWORD PTR DS:[{0}]", DisassemblyHelper.OperandType.LE32);
                    break;

                case "20":
                    DisassemblyHelper.SetDisassemblyString("JMP DWORD PTR DS:[EAX]");
                    break;

                case "64":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "00":
                            DisassemblyHelper.SetDisassemblyString("JMP DWORD PTR DS:[EAX+EAX{0}]", DisassemblyHelper.OperandType.SH8);
                            break;

                        default:
                            break;
                    }
                    break;

                case "74":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "39":
                            DisassemblyHelper.SetDisassemblyString("PUSH DWORD PTR DS:[ECX+EDI{0}]", DisassemblyHelper.OperandType.SH8);
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
