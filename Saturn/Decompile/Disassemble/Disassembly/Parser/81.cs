using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _81
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "3C":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "10":
                            DisassemblyHelper.SetDisassemblyString("CMP DWORD PTR DS:[EAX+EDX], {0}", DisassemblyHelper.OperandType.LE32);
                            break;

                        default:
                            break;
                    }
                    break;

                case "3E":
                    DisassemblyHelper.SetDisassemblyString("CMP DWORD PTR DS:[ESI], {0}", DisassemblyHelper.OperandType.LE32);
                    break;

                case "7C":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "02":
                            DisassemblyHelper.SetDisassemblyString("CMP DWORD PTR DS:[EDX+EAX{0}], {1}", DisassemblyHelper.OperandType.SH8, DisassemblyHelper.OperandType.LE32);
                            break;

                        case "30":
                            DisassemblyHelper.SetDisassemblyString("CMP DWORD PTR DS:[EAX+ESI{0}], {1}", DisassemblyHelper.OperandType.SH8, DisassemblyHelper.OperandType.LE32);
                            break;

                        default:
                            break;
                    }
                    break;

                case "7E":
                    DisassemblyHelper.SetDisassemblyString("CMP DWORD PTR DS:[ESI{0}], {1}", DisassemblyHelper.OperandType.SH8, DisassemblyHelper.OperandType.LE32);
                    break;

                case "C4":
                    DisassemblyHelper.SetDisassemblyString("ADD ESP, {0}", DisassemblyHelper.OperandType.LE32);
                    break;

                case "EC":
                    DisassemblyHelper.SetDisassemblyString("SUB ESP, {0}", DisassemblyHelper.OperandType.LE32);
                    break;

                default:
                    break;
            }
        }
    }
}
