using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IValidationRule<in T>
    {
        ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<T> items, CancellationToken cancellationToken);
    }

    public interface IValidationRule<in T, in TContext>
    {
        ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<T> items, TContext context, CancellationToken cancellationToken);
    }
}
