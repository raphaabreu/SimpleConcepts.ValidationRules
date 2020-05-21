using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public static class ContextualizedValidationRulesAsyncExtensions
    {
        public static IEnumerable<IAsyncValidationRule<TElement, TContext>> WithDelegate<TElement, TContext>(
            this IEnumerable<IAsyncValidationRule<TElement, TContext>> source,
            Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, CancellationToken, ValueTask<IEnumerable<ValidationResult>>> applyRule)
        {
            return source.Select(rule => new DelegatedAsyncValidationRule<TElement, TContext>(rule, applyRule));
        }

        public static IAsyncValidationRule<TElement, TContext> GetFinalTarget<TElement, TContext>(this IAsyncValidationRule<TElement, TContext> source)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            while (source is IAsyncValidationRuleTargetAccess<TElement, TContext> access)
            {
                source = access.Target;
            }

            return source;
        }

        public static async ValueTask<IRuleResultsLookup<TElement>> ValidateAsync<TElement, TContext>(
            this IEnumerable<TElement> source, IEnumerable<IAsyncValidationRule<TElement, TContext>> rules, TContext context, CancellationToken cancellationToken = default)
        {
            // Copy to array to retain indexes.
            var sourceArray = source.ToArray();

            // If there are no elements there is nothing else to do.
            if (sourceArray.Length == 0)
            {
                return new RuleResultsLookup<TElement>(Array.Empty<KeyValuePair<TElement, IEnumerable<RuleResult>>>());
            }

            // Compute all rules in parallel.
            var validationTasks = rules
                .Select(rule => ExecuteRule(rule, source, context, cancellationToken))
                .ToArray();

            var ruleResults = new List<RuleResult[]>(validationTasks.Length);
            foreach (var task in validationTasks)
            {
                ruleResults.Add(await task);
            }

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<TElement, IEnumerable<RuleResult>>(element, ruleResults.Select(r => r[index])));

            return new RuleResultsLookup<TElement>(results);
        }

        private static async ValueTask<RuleResult[]> ExecuteRule<TElement, TContext>(
            IAsyncValidationRule<TElement, TContext> rule, IEnumerable<TElement> source, TContext context,
            CancellationToken cancellationToken)
        {
            var validationResult = await rule.ValidateAsync(source, context, cancellationToken);

            return validationResult
                .Select(result => new RuleResult(rule.GetType(), result))
                .ToArray();
        }
    }
}
