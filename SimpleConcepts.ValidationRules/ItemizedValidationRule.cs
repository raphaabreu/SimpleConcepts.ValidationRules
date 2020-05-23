using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public abstract class ItemizedValidationRule<T> : IValidationRule<T>
    {
        public async ValueTask<ValidationResult[]> ValidateAsync(T[] items, CancellationToken cancellationToken)
        {
            var validationTasks = items
                .Select(item => ValidateItemAsync(item, cancellationToken))
                .ToArray();
            var results = new ValidationResult[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                results[i] = await validationTasks[i];
            }

            return results;
        }

        protected abstract ValueTask<ValidationResult> ValidateItemAsync(T item, CancellationToken cancellationToken);
    }

    public abstract class ItemizedValidationRule<T, TContext> : IValidationRule<T, TContext>
    {
        public async ValueTask<ValidationResult[]> ValidateAsync(T[] items, TContext context, CancellationToken cancellationToken)
        {
            var validationTasks = items
                .Select(item => ValidateItemAsync(item, context, cancellationToken))
                .ToArray();
            var results = new ValidationResult[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                results[i] = await validationTasks[i];
            }

            return results;
        }

        protected abstract ValueTask<ValidationResult> ValidateItemAsync(T item, TContext context, CancellationToken cancellationToken);
    }

}
