using PatternMatching.Tokens;

namespace PatternMatching
{
    public static class PatternMatchingExtensions
    {
        public static ValueToken<TInput, TResult> Match<TInput, TResult>(this TInput expression)
        {
            return new ValueToken<TInput, TResult>(expression);
        }

        public static ActionToken<T> Match<T>(this T expression)
        {
            return new ActionToken<T>(expression);
        }
    }
}
