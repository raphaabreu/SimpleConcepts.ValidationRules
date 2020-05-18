using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IAsyncValidationRule<in TElement>
    {
        ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, CancellationToken cancellationToken);
    }

    public interface IAsyncValidationRule<in TElement, in TContext>
    {
        ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, TContext context, CancellationToken cancellationToken);
    }
}
