using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Saturn
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
