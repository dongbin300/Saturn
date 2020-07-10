namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _89
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (Util.GetHexString(DisassemblyHelper.opcode2))
            {
                case "04":
                    DisassemblyHelper.GetOpcode();

                    switch (Util.GetHexString(DisassemblyHelper.opcode3))
                    {
                        case "24":
                            Disassembly.DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR SS:[ESP], EAX");
                            break;

                        default:
                            break;
                    }
                    break;

                case "32":
                    DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR DS:[EDX], ESI");
                    break;

                case "45":
                    DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR SS:[EBP{0}], EAX", DisassemblyHelper.OperandType.SH8);
                    break;

                case "46":
                    DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR DS:[ESI{0}], EAX", DisassemblyHelper.OperandType.SH8);
                    break;

                case "4D":
                    DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR SS:[EBP{0}], ECX", DisassemblyHelper.OperandType.SH8);
                    break;

                case "5E":
                    DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR DS:[ESI{0}], EBX", DisassemblyHelper.OperandType.SH8);
                    break;

                case "85":
                    DisassemblyHelper.SetDisassemblyString("MOV DWORD PTR SS:[EBP{0}], EAX", DisassemblyHelper.OperandType.SH32);
                    break;

                default:
                    break;
            }
        }
    }
}
