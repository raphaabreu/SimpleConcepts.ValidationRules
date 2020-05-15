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
            Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, CancellationToken, Task<IEnumerable<ValidationResult>>> applyRule)
        {
            return source.Select(rule => new DelegatedAsyncValidationRule<TElement, TContext>(rule, applyRule));
        }

        public static async Task<IRuleResultsLookup<TElement>> ValidateAsync<TElement, TContext>(
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
            var validationTasks = rules.Select(async rule =>
                (await rule.ValidateAsync(sourceArray, context, cancellationToken)).Select(result => new RuleResult(rule.GetType(), result)).ToArray()
            ).ToArray();

            var ruleResults = await Task.WhenAll(validationTasks);

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<TElement, IEnumerable<RuleResult>>(element, ruleResults.Select(r => r[index])));

            return new RuleResultsLookup<TElement>(results);
        }
    }
}
