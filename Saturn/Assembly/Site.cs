using System;
using System.Collections.Generic;
using System.Text;

namespace Saturn.Assembly
{
    public class Site
    {
        public string Name { get; set; }
        public uint Address { get; set; }

        public Site(string name, uint address)
        {
            Name = name;
            Address = address;
        }
    }
}
