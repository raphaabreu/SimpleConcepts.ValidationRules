using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestHandlers
{
    public class ClosedHandler : IValidationRuleHandler<byte>, IValidationRuleHandler<string, long>
    {
        public async ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<string> items, long context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            return await next();
        }

        public async ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<byte> items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            return await next();
        }
    }
}
