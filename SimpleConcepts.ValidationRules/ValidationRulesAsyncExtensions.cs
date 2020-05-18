using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public static class ValidationRulesAsyncExtensions
    {
        public static IEnumerable<IAsyncValidationRule<TElement>> WithDelegate<TElement>(
            this IEnumerable<IAsyncValidationRule<TElement>> source,
            Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, CancellationToken, ValueTask<IEnumerable<ValidationResult>>> applyRule)
        {
            return source.Select(rule => new DelegatedAsyncValidationRule<TElement>(rule, applyRule));
        }

        public static async ValueTask<IRuleResultsLookup<TElement>> ValidateAsync<TElement>(
            this IEnumerable<TElement> source, IEnumerable<IAsyncValidationRule<TElement>> rules, CancellationToken cancellationToken = default)
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
                .Select(rule => ExecuteRule(rule, source, cancellationToken))
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

        private static async ValueTask<RuleResult[]> ExecuteRule<TElement>(
            IAsyncValidationRule<TElement> rule, IEnumerable<TElement> source,
            CancellationToken cancellationToken)
        {
            var validationResult = await rule.ValidateAsync(source, cancellationToken);

            return validationResult
                .Select(result => new RuleResult(rule.GetType(), result))
                .ToArray();
        }
    }
}
