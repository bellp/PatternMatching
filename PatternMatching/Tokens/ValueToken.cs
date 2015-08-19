using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternMatching.Tokens
{
    public class ValueToken<TExpression, TResult>
    {
        private readonly TExpression _expression;
        private readonly bool _conditionMet;
        private readonly TResult _result;

        internal ValueToken(TExpression expression)
        {
            _expression = expression;
        }

        private ValueToken(TExpression expression, TResult result, bool conditionMet)
        {
            _expression = expression;
            _result = result;
            _conditionMet = conditionMet;
        }

        public ValueToken<TExpression, TResult> WithRange(IComparable<TExpression> fromInclusive, IComparable<TExpression> toExclusive, TResult resultExpression)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = fromInclusive.CompareTo(_expression) <= 0 && toExclusive.CompareTo(_expression) > 0;
            return new ValueToken<TExpression, TResult>(_expression, resultExpression, conditionMet);
        }

        public ValueToken<TExpression, TResult> WithType<TType>(TResult resultExpression) where TType : TExpression
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression is TType;
            return new ValueToken<TExpression, TResult>(_expression, resultExpression, conditionMet);
        }

        public ValueToken<TExpression, TResult> WithType<TType>(Func<TType, TResult> selector) where TType : TExpression
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression is TType;

            if (conditionMet)
            {
                return new ValueToken<TExpression, TResult>(_expression, selector((TType) _expression), true);
            }

            return this;
        }

        public virtual ValueToken<TExpression, TResult> With(IEnumerable<TExpression> collection, TResult resultExpression)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = collection.Contains(_expression);
            return new ValueToken<TExpression, TResult>(_expression, resultExpression, conditionMet);
        }

        public ValueToken<TExpression, TResult> With(TExpression patternExpression, TResult resultExpression)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression.EquivalentTo(patternExpression);
            return new ValueToken<TExpression, TResult>(_expression, resultExpression, conditionMet);
        }

        public ValueToken<TExpression, TResult> With(TExpression expression, Func<TExpression, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression.EquivalentTo(expression);
            return new ValueToken<TExpression, TResult>(_expression, selector(_expression), conditionMet);
        }

        public ValueToken<TExpression, TResult> With(Predicate<TExpression> predicate, TResult resultExpression)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = _expression != null && predicate(_expression);
            return new ValueToken<TExpression, TResult>(_expression, resultExpression, conditionMet);
        }

        public ValueToken<TExpression, TResult> WithNull(TResult resultExpression)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = (_expression == null);
            return new ValueToken<TExpression, TResult>(_expression, resultExpression, conditionMet);
        }

        public TResult Finally(TResult alternative)
        {
            if (_conditionMet)
            {
                return _result;
            }

            return alternative;
        }
    }
}