using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IValidationRule<in T>
    {
        ValueTask<ValidationResult[]> ValidateAsync(T[] items, CancellationToken cancellationToken);
    }

    public interface IValidationRule<in T, in TContext>
    {
        ValueTask<ValidationResult[]> ValidateAsync(T[] items, TContext context, CancellationToken cancellationToken);
    }
}
