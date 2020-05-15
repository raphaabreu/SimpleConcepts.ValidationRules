using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class ValidationRulesExtensions
    {
        public static IEnumerable<IValidationRule<TElement>> WithDelegate<TElement>(
            this IEnumerable<IValidationRule<TElement>> source,
            Func<IValidationRule<TElement>, IEnumerable<TElement>, IEnumerable<ValidationResult>> applyRule)
        {
            return source.Select(rule => new DelegatedValidationRule<TElement>(rule, applyRule));
        }

        public static IRuleResultsLookup<TElement> Validate<TElement>(
            this IEnumerable<TElement> source, IEnumerable<IValidationRule<TElement>> rules)
        {
            // Copy to array to retain indexes.
            var sourceArray = source.ToArray();

            // If there are no elements there is nothing else to do.
            if (sourceArray.Length == 0)
            {
                return new RuleResultsLookup<TElement>(Array.Empty<KeyValuePair<TElement, IEnumerable<RuleResult>>>());
            }

            // Compute all rules sequentially.
            var ruleResults = rules.Select(rule =>
                rule.Validate(sourceArray).Select(result => new RuleResult(rule.GetType(), result)).ToArray()
            ).ToArray();

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<TElement, IEnumerable<RuleResult>>(element, ruleResults.Select(r => r[index])));

            return new RuleResultsLookup<TElement>(results);
        }

    }
}
