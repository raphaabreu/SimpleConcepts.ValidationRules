using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class ContextualizedValidationRulesExtensions
    {
        public static IEnumerable<IValidationRule<TElement, TContext>> WithDelegate<TElement, TContext>(
            this IEnumerable<IValidationRule<TElement, TContext>> source,
            Func<IValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, IEnumerable<ValidationResult>> applyRule)
        {
            return source.Select(rule => new DelegatedValidationRule<TElement, TContext>(rule, applyRule));
        }

        public static ILookup<TElement, RuleValidationResult> Validate<TElement, TContext>(
            this IEnumerable<TElement> source, IEnumerable<IValidationRule<TElement, TContext>> rules, TContext context)
        {
            // Copy to array to retain indexes.
            var sourceArray = source.ToArray();

            // If there are no elements there is nothing else to do.
            if (sourceArray.Length == 0)
            {
                return new ValidationResultLookup<TElement>(Array.Empty<KeyValuePair<TElement, IEnumerable<RuleValidationResult>>>());
            }

            // Compute all rules sequentially.
            var ruleResults = rules.Select(rule =>
                rule.Validate(sourceArray, context).Select(result => new RuleValidationResult(rule.GetType(), result)).ToArray()
            ).ToArray();

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<TElement, IEnumerable<RuleValidationResult>>(element, ruleResults.Select(r => r[index])));

            return new ValidationResultLookup<TElement>(results);
        }
    }
}