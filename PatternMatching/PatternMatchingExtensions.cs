using PatternMatching.Tokens;

namespace PatternMatching
{
    public static class PatternMatchingExtensions
    {
        /// <summary>
        /// Matches an expression to a value
        /// </summary>
        public static ValueToken<TSource, TResult> Match<TSource, TResult>(this TSource expression)
        {
            return new ValueToken<TSource, TResult>(expression);
        }

        /// <summary>
        /// Matches an expression to an action
        /// </summary>
        public static ActionToken<T> Match<T>(this T expression)
        {
            return new ActionToken<T>(expression);
        }
    }
}
