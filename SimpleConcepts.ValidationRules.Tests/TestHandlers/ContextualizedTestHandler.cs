using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestHandlers
{
    public class ContextualizedTestHandler<T, TContext> : IValidationRuleHandler<T, TContext>
    {
        public int HandleCount { get; set; }

        public ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<T> items, TContext context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            HandleCount += 1;
            return next();
        }
    }
}
