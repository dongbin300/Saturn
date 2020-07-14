using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _00
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "00":
                    DisassemblyHelper.SetDisassemblyString("ADD BYTE PTR DS:[EAX], AL");
                    break;

                case "18":
                    DisassemblyHelper.SetDisassemblyString("ADD BYTE PTR DS:[EAX], BL");
                    break;

                case "1D":
                    DisassemblyHelper.SetDisassemblyString("ADD BYTE PTR DS:[{0}], BL", DisassemblyHelper.OperandType.LE32);
                    break;

                case "6C":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "B1":
                            DisassemblyHelper.SetDisassemblyString("ADD BYTE PTR DS:[ECX+ESI*4{0}], CH", DisassemblyHelper.OperandType.SH8);
                            break;

                        default:
                            break;
                    }
                    break;

                //case "6E":
                //    DisassemblyHelper.SetDisassemblyString("ADD BYTE PTR DS:[ESI{0}], CH", DisassemblyHelper.OperandType.SH8);
                //    break;

                case "74":
                    DisassemblyHelper.GetOpcode();

                    switch (GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "65":
                            DisassemblyHelper.SetDisassemblyString("ADD BYTE PTR SS:[EBP{0}], DH", DisassemblyHelper.OperandType.SH8);
                            break;

                        default:
                            break;
                    }
                    break;

                case "D8":
                    DisassemblyHelper.SetDisassemblyString("ADD AL, BL");
                    break;

                case "F4":
                    DisassemblyHelper.SetDisassemblyString("ADD AH, DH");
                    break;

                default:
                    break;
            }
        }
    }
}
