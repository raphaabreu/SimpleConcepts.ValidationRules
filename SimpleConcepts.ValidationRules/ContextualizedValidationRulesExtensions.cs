using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public static class ContextualizedValidationRulesExtensions
    {
        public static async ValueTask<IRuleResultsLookup<T>> ValidateAsync<T, TContext>(
            this IEnumerable<T> source, IEnumerable<IValidationRule<T, TContext>> rules, TContext context, CancellationToken cancellationToken = default)
        {
            // Copy to array to retain indexes.
            var sourceArray = source.ToArray();

            // If there are no elements there is nothing else to do.
            if (sourceArray.Length == 0)
            {
                return new RuleResultsLookup<T>(Array.Empty<KeyValuePair<T, IEnumerable<RuleResult>>>());
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
                .Select((item, index) => new KeyValuePair<T, IEnumerable<RuleResult>>(item, ruleResults.Select(r => r[index])));

            return new RuleResultsLookup<T>(results);
        }

        private static async ValueTask<RuleResult[]> ExecuteRule<TElement, TContext>(
            IValidationRule<TElement, TContext> rule, IEnumerable<TElement> source, TContext context,
            CancellationToken cancellationToken)
        {
            var validationResult = await rule.ValidateAsync(source, context, cancellationToken);

            return validationResult
                .Select(result => new RuleResult(rule.GetType(), result))
                .ToArray();
        }
    }
}
