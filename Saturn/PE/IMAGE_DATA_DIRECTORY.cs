using System;
using System.Collections.Generic;
using System.Text;

namespace Saturn.PE
{
    public class IMAGE_DATA_DIRECTORY
    {
        public uint VirtualAddress { get; set; }
        public uint Size { get; set; }

        public IMAGE_DATA_DIRECTORY(uint virtualAddress, uint size)
        {
            VirtualAddress = virtualAddress;
            Size = size;
        }
    }
}
