namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _C6
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (Util.GetHexString(DisassemblyHelper.opcode2))
            {
                case "05":
                    DisassemblyHelper.SetDisassemblyString("MOV BYTE PTR DS:[{0}], {1}", DisassemblyHelper.OperandType.LE32, DisassemblyHelper.OperandType.LE8);
                    break;

                default:
                    break;
            }
        }
    }
}
