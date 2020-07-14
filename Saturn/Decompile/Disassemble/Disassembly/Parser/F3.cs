using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _F3
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "AA":
                    DisassemblyHelper.SetDisassemblyString("REP STOSB");
                    break;

                case "AB":
                    DisassemblyHelper.SetDisassemblyString("REP STOSD");
                    break;

                default:
                    break;
            }
        }
    }
}
