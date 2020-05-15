using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class RuleResultsExtensions
    {
        public static bool Valid(this IEnumerable<RuleResult> source)
        {
            return source.All(x => x.Result == ValidationResult.Valid);
        }

        public static IEnumerable<string> ErrorCodes(this IEnumerable<RuleResult> source)
        {
            return source
                .Where(x => x.Result != ValidationResult.Valid)
                .Select(x => x.Result.ErrorCode);
        }
    }
}
