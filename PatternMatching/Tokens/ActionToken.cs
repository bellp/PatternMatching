using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternMatching.Tokens
{
    public class ActionToken<TExpression>
    {
        private readonly TExpression _expression;
        private readonly bool _conditionMet;

        internal ActionToken(TExpression expression)
        {
            _expression = expression;
        }

        private ActionToken(TExpression expression, bool conditionMet)
        {
            _expression = expression;
            _conditionMet = conditionMet;
        }

        public ActionToken<TExpression> WithRange(IComparable<TExpression> fromInclusive, IComparable<TExpression> toExclusive, Action<TExpression> action)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = fromInclusive.CompareTo(_expression) <= 0 && toExclusive.CompareTo(_expression) > 0;
            return GenerateToken(action, conditionMet);
        }

        public ActionToken<TExpression> WithType<T>(Action<T> action) where T : TExpression
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression is T;

            if (conditionMet)
            {
                action((T)_expression);
            }

            return new ActionToken<TExpression>(_expression, conditionMet);
        }

        public ActionToken<TExpression> With(TExpression expression, Action<TExpression> action)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression.EquivalentTo(expression);
            return GenerateToken(action, conditionMet);
        }

        public ActionToken<TExpression> With(Predicate<TExpression> predicate, Action<TExpression> action)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = predicate(_expression);
            return GenerateToken(action, conditionMet);
        }

        public ActionToken<TExpression> With(IEnumerable<TExpression> collection, Action<TExpression> action)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = collection.Contains(_expression);
            return GenerateToken(action, conditionMet);
        }

        public ActionToken<TExpression> WithNull(Action action)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression == null;

            if (conditionMet)
            {
                action();
            }

            return new ActionToken<TExpression>(_expression, conditionMet);
        }

        public void Finally(Action<TExpression> action)
        {
            if (!_conditionMet)
            {
                action(_expression);
            }
        }

        private ActionToken<TExpression> GenerateToken(Action<TExpression> action, bool conditionMet)
        {
            if (conditionMet)
            {
                action(_expression);
            }

            return new ActionToken<TExpression>(_expression, conditionMet);
        }
    }
}