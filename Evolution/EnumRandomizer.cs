using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution
{
    static class EnumRandomizer
    {
        static T Random<T>(this T e) where T: Enum
        {
            Array array = Enum.GetValues(typeof(T));
            Random r = new Random();
            return (T)array.GetValue(r.Next(array.Length));
        }
    }
}
