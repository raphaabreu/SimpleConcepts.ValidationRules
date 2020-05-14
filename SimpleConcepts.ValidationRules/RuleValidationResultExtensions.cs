using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class RuleValidationResultExtensions
    {
        public static bool Success(this IEnumerable<RuleValidationResult> source)
        {
            return source.All(x => x.Result == ValidationResult.Success);
        }

        public static IEnumerable<string> ErrorCodes(this IEnumerable<RuleValidationResult> source)
        {
            return source
                .Where(x => x.Result != ValidationResult.Success)
                .Select(x => x.Result.ErrorCode);
        }
    }
}