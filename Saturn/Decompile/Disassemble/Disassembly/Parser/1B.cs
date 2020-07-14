using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _1B
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "C0":
                    DisassemblyHelper.SetDisassemblyString("SBB EAX, EAX");
                    break;

                default:
                    break;
            }
        }
    }
}
