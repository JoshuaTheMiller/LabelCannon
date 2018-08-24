using System;
using System.Collections.Generic;

namespace Service
{
    public sealed class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> comparerFunction;

        public DelegateEqualityComparer(Func<T, T, bool> comparerFunction)
        {
            this.comparerFunction = comparerFunction;
        }

        public bool Equals(T x, T y)
        {
            return comparerFunction(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}