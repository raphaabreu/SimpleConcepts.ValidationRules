using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class LookupExtensions
    {
        public static IEnumerable<TElement> Valid<TElement>(this ILookup<TElement, Validation> validationResults)
        {
            return validationResults
                .Where(r => r.Success())
                .Select(r => r.Key);
        }

        public static IEnumerable<TElement> Invalid<TElement>(this ILookup<TElement, Validation> validationResults)
        {
            return validationResults
                .Where(r => r.Success())
                .Select(r => r.Key);
        }
    }
}
