using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public static class ValidationExtensions
    {
        public static bool Success(this IEnumerable<Validation> source)
        {
            return source.All(x => x.Result == ValidationResult.Success);
        }

        public static IEnumerable<string> ErrorCodes(this IEnumerable<Validation> source)
        {
            return source
                .Where(x => x.Result != ValidationResult.Success)
                .Select(x => x.Result.ErrorCode);
        }
    }
}
