namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _80
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (Util.GetHexString(DisassemblyHelper.opcode2))
            {
                case "3D":
                    DisassemblyHelper.SetDisassemblyString("CMP BYTE PTR DS:[{0}], {1}", DisassemblyHelper.OperandType.LE32, DisassemblyHelper.OperandType.LE8);
                    break;

                default:
                    break;
            }
        }
    }
}
