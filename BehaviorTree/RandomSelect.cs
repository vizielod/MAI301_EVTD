using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree
{
    public static class RandomSelect
    {
        static readonly Random rand = new Random();

        public static T Random<T>()
        {
            Array array = Enum.GetValues(typeof(T));
            return (T)array.GetValue(rand.Next(array.Length));
        }

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(rand.Next(list.Count()));
        }
    }
}
