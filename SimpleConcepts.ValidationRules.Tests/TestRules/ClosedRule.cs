using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class ClosedRule : IValidationRule<byte>, IValidationRule<string, long>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<string> items, long context, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            return new ValueTask<IEnumerable<ValidationResult>>(items.Select(x => rnd.NextDouble() > 0.1 ? ValidationResult.Valid : new ValidationResult("TEST_ERROR")));
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<byte> items, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            return new ValueTask<IEnumerable<ValidationResult>>(items.Select(x => rnd.NextDouble() > 0.1 ? ValidationResult.Valid : new ValidationResult("TEST_ERROR")));
        }
    }
}
