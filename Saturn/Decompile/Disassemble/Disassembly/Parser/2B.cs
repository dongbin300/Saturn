namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _2B
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (Util.GetHexString(DisassemblyHelper.opcode2))
            {
                case "C8":
                    DisassemblyHelper.SetDisassemblyString("SUB ECX, EAX");
                    break;

                default:
                    break;
            }
        }
    }
}
