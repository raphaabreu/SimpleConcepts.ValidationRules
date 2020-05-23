using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestHandlers
{
    public class ClosedHandler : IValidationRuleHandler<byte>, IValidationRuleHandler<string, long>
    {
        public async ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, string[] items, long context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            return await next();
        }

        public async ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, byte[] items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            return await next();
        }
    }
}
