using System;

namespace PatternMatching
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Similar to Object.Equals, except it also takes advantage of IEquatable types
        /// if possible
        /// </summary>
        public static bool EquivalentTo<T>(this T objA, T objB)
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
