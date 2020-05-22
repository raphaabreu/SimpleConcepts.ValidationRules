using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestHandlers
{
    public class ClosedHandler : IValidationRuleHandler<byte>, IValidationRuleHandler<string, long>
    {
        public ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<string> items, long context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<byte> items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
