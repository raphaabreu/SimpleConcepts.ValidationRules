using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestHandlers
{
    public class TestHandler<T> : IValidationRuleHandler<T>
    {
        public int HandleCount { get; set; }

        public ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, T[] items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            HandleCount += 1;
            return next();
        }
    }
}
