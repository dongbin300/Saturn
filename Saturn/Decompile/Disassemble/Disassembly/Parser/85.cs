﻿using static Saturn.Util.StringUtil;

namespace Saturn.Decompile.Disassemble.Disassembly.Parser
{
    public class _85
    {
        public static void Parse()
        {
            DisassemblyHelper.GetOpcode();

            switch (GetHexString(DisassemblyHelper.opcode2))
            {
                case "00":
                    DisassemblyHelper.SetDisassemblyString("TEST DWORD PTR DS:[EAX], EAX");
                    break;

                case "C0":
                    DisassemblyHelper.SetDisassemblyString("TEST EAX, EAX");
                    break;

                case "D2":
                    DisassemblyHelper.SetDisassemblyString("TEST EDX, EDX");
                    break;

                case "DB":
                    DisassemblyHelper.SetDisassemblyString("TEST EBX, EBX");
                    break;

                case "F6":
                    DisassemblyHelper.SetDisassemblyString("TEST ESI, ESI");
                    break;

                default:
                    break;
            }
        }
    }
}
