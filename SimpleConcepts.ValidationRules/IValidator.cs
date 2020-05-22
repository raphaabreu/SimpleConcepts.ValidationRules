using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IValidator<T>
    {
        ValueTask<IRuleResultsLookup<T>> ValidateAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);
    }

    public interface IValidator<T, in TContext>
    {
        ValueTask<IRuleResultsLookup<T>> ValidateAsync(IEnumerable<T> items, TContext context, CancellationToken cancellationToken = default);
    }
}
