using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public static class ValidationRulesAsyncExtensions
    {
        public static IEnumerable<IAsyncValidationRule<TElement>> WithDelegate<TElement>(
            this IEnumerable<IAsyncValidationRule<TElement>> source,
            Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, Task<IEnumerable<ValidationResult>>> applyRule)
        {
            return source.Select(rule => new DelegatedAsyncValidationRule<TElement>(rule, applyRule));
        }

        public static async Task<ILookup<TElement, RuleValidationResult>> ValidateAsync<TElement>(
            this IEnumerable<TElement> source, IEnumerable<IAsyncValidationRule<TElement>> rules)
        {
            // Copy to array to retain indexes.
            var sourceArray = source.ToArray();

            // If there are no elements there is nothing else to do.
            if (sourceArray.Length == 0)
            {
                return new ValidationResultLookup<TElement>(Array.Empty<KeyValuePair<TElement, IEnumerable<RuleValidationResult>>>());
            }

            // Compute all rules in parallel.
            var validationTasks = rules.Select(async rule =>
                (await rule.ValidateAsync(sourceArray)).Select(result => new RuleValidationResult(rule.GetType(), result)).ToArray()
            ).ToArray();

            var ruleResults = await Task.WhenAll(validationTasks);

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<TElement, IEnumerable<RuleValidationResult>>(element, ruleResults.Select(r => r[index])));

            return new ValidationResultLookup<TElement>(results);
        }
    }
}