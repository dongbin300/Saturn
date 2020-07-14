using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _03
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "C2":
                    DisassemblyHelper.SetDisassemblyString("ADD EAX, EDX");
                    break;

                case "CA":
                    DisassemblyHelper.SetDisassemblyString("ADD ECX, EDX");
                    break;

                case "D1":
                    DisassemblyHelper.SetDisassemblyString("ADD EDX, ECX");
                    break;

                default:
                    break;
            }
        }
    }
}
