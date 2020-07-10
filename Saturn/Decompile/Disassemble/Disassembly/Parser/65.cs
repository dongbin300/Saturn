using System;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _65
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (Util.GetHexString(DisassemblyHelper.opcode2))
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
