using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _F7
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "D0":
                    DisassemblyHelper.SetDisassemblyString("NOT EAX");
                    break;

                default:
                    break;
            }
        }
    }
}
