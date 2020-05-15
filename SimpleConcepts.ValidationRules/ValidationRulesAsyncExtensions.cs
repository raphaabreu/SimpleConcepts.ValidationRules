﻿using System;
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
            Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, CancellationToken, Task<IEnumerable<ValidationResult>>> applyRule)
        {
            return source.Select(rule => new DelegatedAsyncValidationRule<TElement>(rule, applyRule));
        }

        public static async Task<ILookup<TElement, Validation>> ValidateAsync<TElement>(
            this IEnumerable<TElement> source, IEnumerable<IAsyncValidationRule<TElement>> rules, CancellationToken cancellationToken = default)
        {
            // Copy to array to retain indexes.
            var sourceArray = source.ToArray();

            // If there are no elements there is nothing else to do.
            if (sourceArray.Length == 0)
            {
                return new ValidationResultLookup<TElement>(Array.Empty<KeyValuePair<TElement, IEnumerable<Validation>>>());
            }

            // Compute all rules in parallel.
            var validationTasks = rules.Select(async rule =>
                (await rule.ValidateAsync(sourceArray, cancellationToken)).Select(result => new Validation(rule.GetType(), result)).ToArray()
            ).ToArray();

            var ruleResults = await Task.WhenAll(validationTasks);

            // Aggregate all results by element.
            var results = sourceArray
                .Select((element, index) => new KeyValuePair<TElement, IEnumerable<Validation>>(element, ruleResults.Select(r => r[index])));

            return new ValidationResultLookup<TElement>(results);
        }
    }
}
