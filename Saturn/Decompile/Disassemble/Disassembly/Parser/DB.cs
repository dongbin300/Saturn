using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _DB
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "E2":
                    DisassemblyHelper.SetDisassemblyString("FNCLEX");
                    break;

                default:
                    break;
            }
        }
    }
}
