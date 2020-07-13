using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SimplePerformanceChecker;
using Saturn;
using System.Runtime.CompilerServices;
using static Saturn.Util;
using System.Text.RegularExpressions;

namespace PETest
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 100000;

            ;

            for(int i=0; i<count; i++)
            {
                Method1();
            }

            ;

            for (int i = 0; i < count; i++)
            {
                Method2();
            }

            ;
        }

        static void Method1()
        {
            string str = "7891623761724";
            decimal.TryParse(str, out decimal _);
        }

        static void Method2()
        {
            string str = "7891623761724";
            Regex.IsMatch(str, @"^[0-9]+$");
        }
    }
}
