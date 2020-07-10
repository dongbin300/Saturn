using System;
using System.Collections.Generic;
using System.Text;

namespace Saturn.Decompile.Disassemble
{
    public class PEFileDisassembly
    {
        public uint BaseAddress { get; set; }
        public int BassAddressInterval { get; set; }
        public byte[] Data { get; set; }
    }
}
