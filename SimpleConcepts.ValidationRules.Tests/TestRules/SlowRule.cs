using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class SlowRule : IValidationRule<int>
    {
        public async ValueTask<ValidationResult[]> ValidateAsync(int[] items, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return items
                .Select(x => ValidationResult.Valid)
                .ToArray();
        }
    }

    public class SlowRule<TContext> : IValidationRule<int, TContext>
    {
        public async ValueTask<ValidationResult[]> ValidateAsync(int[] items, TContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return items
                .Select(x => ValidationResult.Valid)
                .ToArray();
        }
    }
}
