using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IValidationRuleHandler<in T>
    {
        ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<T> items, ValidationRuleHandlerDelegate next, CancellationToken cancellationToken);
    }

    public interface IValidationRuleHandler<in T, in TContext>
    {
        ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<T> items, TContext context, ValidationRuleHandlerDelegate next, CancellationToken cancellationToken);
    }
}
