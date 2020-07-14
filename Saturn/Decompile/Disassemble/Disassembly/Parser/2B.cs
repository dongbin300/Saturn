using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _2B
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
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
