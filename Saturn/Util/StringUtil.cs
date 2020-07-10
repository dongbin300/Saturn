using System;
using System.Collections.Generic;
using System.Text;

namespace Saturn
{
    public class StringUtil
    {
        public static bool IsNumericString(string str)
        {
            return decimal.TryParse(str, out decimal _);
        }

        public static bool IsHexaDecimalString(string str)
        {
            str = str.Replace("0X", "").Replace("H", "");
            return IsNumericString(str);
        }
    }
}
