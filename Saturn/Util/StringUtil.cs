using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
            if(str.StartsWith("0X") || str.EndsWith("H")) // hexa-decimal
            {
                return Regex.IsMatch(str.Replace("0X", "").Replace("H", ""), @"^[A-F0-9]+$");
            }
            else // decimal
            {
                return IsNumericString(str);
            }
        }
    }
}
