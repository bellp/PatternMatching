using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternMatching.Tokens
{
    public class ActionToken<TExpression>
    {
        private readonly TExpression _expression;
        private readonly bool _conditionMet;
        private readonly bool _deferAction;

        internal ActionToken(TExpression expression)
        {
            _expression = expression;
        }

        private ActionToken(TExpression expression, bool conditionMet, bool deferAction)
        {
            _expression = expression;
            _conditionMet = conditionMet;
            _deferAction = deferAction;
        }

        private ActionToken(TExpression expression, bool conditionMet)
        {
            _expression = expression;
            _conditionMet = conditionMet;
        }

        public ActionToken<TExpression> WithRange(IComparable<TExpression> fromInclusive, IComparable<TExpression> toInclusive)
        {
            return CreateToken(() => Conditions.InRange(fromInclusive, toInclusive, _expression));
        }

        public ActionToken<TExpression> WithRange(IComparable<TExpression> fromInclusive, IComparable<TExpression> toInclusive, Action<TExpression> action)
        {
            return CreateToken(() => action(_expression), () => Conditions.InRange(fromInclusive, toInclusive, _expression));
        }

        public ActionToken<TExpression> WithType<T>() where T : TExpression
        {
            return CreateToken(() => _expression is T);
        }

        public ActionToken<TExpression> WithType<T>(Action<T> action) where T : TExpression
        {
            return CreateToken(() => action((T)_expression), () => _expression is T);
        }

        public ActionToken<TExpression> With(TExpression expression)
        {
            return CreateToken(() => _expression.EquivalentTo(expression));
        }

        public ActionToken<TExpression> With(TExpression expression, Action<TExpression> action)
        {
            return CreateToken(() => action(_expression), () => _expression.EquivalentTo(expression));
        }

        public ActionToken<TExpression> With(Predicate<TExpression> predicate, Action<TExpression> action)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return CreateToken(() => action(_expression), () => predicate(_expression));
        }

        public ActionToken<TExpression> With(IEnumerable<TExpression> collection, Action<TExpression> action)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return CreateToken(() => action(_expression), () => collection.Contains(_expression));
        }

        public ActionToken<TExpression> WithNull()
        {
            return CreateToken(() => _expression == null);
        }

        public ActionToken<TExpression> WithNull(Action action)
        {
            return CreateToken(action, () => _expression == null);
        }

        public void Finally(Action<TExpression> action)
        {
            if (!_conditionMet || _deferAction)
            {
                action(_expression);
            }
        }

        private ActionToken<TExpression> CreateToken(Func<bool> condition)
        {
            if (_conditionMet)
            {
                return this;
            }

            bool conditionMet = condition();
            return new ActionToken<TExpression>(_expression, conditionMet, conditionMet);
        }

        private ActionToken<TExpression> CreateToken(Action action, Func<bool> condition)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (_conditionMet)
            {
                if (_deferAction)
                {
                    action();
                    return new ActionToken<TExpression>(_expression, true, false);
                }

                return this;
            }

            bool conditionMet = condition();

            if (conditionMet)
            {
                action();
            }

            return new ActionToken<TExpression>(_expression, conditionMet);
        }
    }
}