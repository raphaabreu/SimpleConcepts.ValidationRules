using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestHandlers
{
    public class TestHandler<T> : IValidationRuleHandler<T>
    {
        public int HandleCount { get; set; }

        public ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<T> items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            HandleCount += 1;
            return next();
        }
    }
}
