namespace Saturn.Util
{
    public class BitUtil
    {
        public static uint SufficientValue(uint value, int digit)
        {
            return ((value >> digit) + 1) << digit;
        }

        public static uint InsufficientValue(uint value, int digit)
        {
            return (value >> digit) << digit;
        }
    }
}
