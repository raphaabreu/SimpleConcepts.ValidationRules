using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class ClosedRule : IValidationRule<byte>, IValidationRule<string, long>
    {
        public ValueTask<ValidationResult[]> ValidateAsync(string[] items, long context, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            return new ValueTask<ValidationResult[]>(items.Select(x => rnd.NextDouble() > 0.1 ? ValidationResult.Valid : new ValidationResult("TEST_ERROR")).ToArray());
        }

        public ValueTask<ValidationResult[]> ValidateAsync(byte[] items, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            return new ValueTask<ValidationResult[]>(items.Select(x => rnd.NextDouble() > 0.1 ? ValidationResult.Valid : new ValidationResult("TEST_ERROR")).ToArray());
        }
    }
}
