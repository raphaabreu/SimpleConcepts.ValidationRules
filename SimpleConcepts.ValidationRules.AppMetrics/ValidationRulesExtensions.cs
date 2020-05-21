using System.Collections.Generic;
using System.Linq;
using App.Metrics;

namespace SimpleConcepts.ValidationRules.AppMetrics
{
    public static class ValidationRulesExtensions
    {
        public static IEnumerable<IValidationRule<TElement>> WithMetrics<TElement>(this IEnumerable<IValidationRule<TElement>> source, IMetrics metrics)
        {
            return source.Select(r => new ValidationRuleMetricsDecorator<TElement>(r, metrics));
        }

        public static IEnumerable<IAsyncValidationRule<TElement>> WithMetrics<TElement>(this IEnumerable<IAsyncValidationRule<TElement>> source, IMetrics metrics)
        {
            return source.Select(r => new ValidationRuleMetricsDecorator<TElement>(r, metrics));
        }

        public static IEnumerable<IValidationRule<TElement, TContext>> WithMetrics<TElement, TContext>(this IEnumerable<IValidationRule<TElement, TContext>> source, IMetrics metrics)
        {
            return source.Select(r => new ContextualizedValidationRuleMetricsDecorator<TElement, TContext>(r, metrics));
        }

        public static IEnumerable<IAsyncValidationRule<TElement, TContext>> WithMetrics<TElement, TContext>(this IEnumerable<IAsyncValidationRule<TElement, TContext>> source, IMetrics metrics)
        {
            return source.Select(r => new ContextualizedValidationRuleMetricsDecorator<TElement, TContext>(r, metrics));
        }
    }
}
