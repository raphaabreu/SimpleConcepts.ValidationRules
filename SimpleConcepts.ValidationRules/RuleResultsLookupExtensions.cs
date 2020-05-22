using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class RuleResultsLookupExtensions
    {
        public static IEnumerable<T> ValidKeys<T>(this ILookup<T, RuleResult> ruleResultsLookup)
        {
            return ruleResultsLookup
                .Where(r => r.Valid())
                .Select(r => r.Key);
        }

        public static IEnumerable<T> InvalidKeys<T>(this ILookup<T, RuleResult> validationResults)
        {
            return validationResults
                .Where(r => !r.Valid())
                .Select(r => r.Key);
        }
    }
}
