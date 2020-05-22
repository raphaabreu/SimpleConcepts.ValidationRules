using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class ClosedRule : IValidationRule<byte>, IValidationRule<string, long>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<string> items, long context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<byte> items, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
