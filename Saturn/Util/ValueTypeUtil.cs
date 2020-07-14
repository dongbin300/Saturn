namespace Saturn.Util
{
    public class ValueTypeUtil
    {
        public static T Cast<T>(object input)
        {
            return (T)input;
        }

        public static T Convert<T>(object input)
        {
            return (T)System.Convert.ChangeType(input, typeof(T));
        }
    }
}
