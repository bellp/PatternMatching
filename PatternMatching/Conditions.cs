using System;

namespace PatternMatching
{
    internal static class Conditions
    {
        public static bool InRange<T>(IComparable<T> fromInclusive, IComparable<T> toInclusive, T expression)
        {
            return fromInclusive.CompareTo(expression) <= 0 && toInclusive.CompareTo(expression) >= 0;
        }

        public static bool Equivalent<T>(T objA, T objB)
        {
            if (objA is IEquatable<T>)
            {
                var equatable = (IEquatable<T>)objA;
                return equatable.Equals(objB);
            }

            return Equals(objA, objB);
        }
    }
}
