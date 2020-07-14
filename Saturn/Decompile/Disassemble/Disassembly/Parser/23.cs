using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _23
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "C8":
                    DisassemblyHelper.SetDisassemblyString("AND ECX, EAX");
                    break;

                default:
                    break;
            }
        }
    }
}
