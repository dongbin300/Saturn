﻿using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _26
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "1C":
                    DisassemblyHelper.SetDisassemblyString("SBB AL, {0}", DisassemblyHelper.OperandType.LE8);
                    break;

                default:
                    break;
            }
        }
    }
}
