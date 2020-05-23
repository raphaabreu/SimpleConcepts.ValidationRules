using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IValidationRuleHandler<in T>
    {
        ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, T[] items, ValidationRuleHandlerDelegate next, CancellationToken cancellationToken);
    }

    public interface IValidationRuleHandler<in T, in TContext>
    {
        ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, T[] items, TContext context, ValidationRuleHandlerDelegate next, CancellationToken cancellationToken);
    }
}
