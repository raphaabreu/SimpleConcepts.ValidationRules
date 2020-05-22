using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public static class ValidationRulesExtensions
    {
        public static async ValueTask<IRuleResultsLookup<T>> ValidateAsync<T>(
            this IEnumerable<T> source, IEnumerable<IValidationRule<T>> rules, CancellationToken cancellationToken = default)
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
                .Select(rule => ExecuteRule(rule, source, cancellationToken))
                .ToArray();

            var ruleResults = new List<RuleResult[]>(validationTasks.Length);
            foreach (var task in validationTasks)
            {
                ruleResults.Add(await task);
            }

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<T, IEnumerable<RuleResult>>(element, ruleResults.Select(r => r[index])));

            return new RuleResultsLookup<T>(results);
        }

        private static async ValueTask<RuleResult[]> ExecuteRule<T>(
            IValidationRule<T> rule, IEnumerable<T> source,
            CancellationToken cancellationToken)
        {
            var validationResult = await rule.ValidateAsync(source, cancellationToken);

            return validationResult
                .Select(result => new RuleResult(rule.GetType(), result))
                .ToArray();
        }
    }
}
