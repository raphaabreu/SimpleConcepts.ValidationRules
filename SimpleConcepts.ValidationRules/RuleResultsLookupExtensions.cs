using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class RuleResultsLookupExtensions
    {
        public static IEnumerable<TElement> ValidKeys<TElement>(this ILookup<TElement, RuleResult> ruleResultsLookup)
        {
            return ruleResultsLookup
                .Where(r => r.Valid())
                .Select(r => r.Key);
        }

        public static IEnumerable<TElement> InvalidKeys<TElement>(this ILookup<TElement, RuleResult> validationResults)
        {
            return validationResults
                .Where(r => !r.Valid())
                .Select(r => r.Key);
        }
    }
}
