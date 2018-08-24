using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, Func<T, T, bool> comparerFunction)
        {
            return enumerable.Distinct(new DelegateEqualityComparer<T>(comparerFunction)).ToList();
        }
    }
}
