using System;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _23
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (Util.GetHexString(DisassemblyHelper.opcode2))
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
