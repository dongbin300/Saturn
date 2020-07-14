using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _65
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "78":
                    DisassemblyHelper.SetDisassemblyString("JS [{0}]", DisassemblyHelper.OperandType.J8);
                    break;

                default:
                    break;
            }
        }
    }
}
