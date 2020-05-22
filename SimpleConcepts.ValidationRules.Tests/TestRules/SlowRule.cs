using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class SlowRule : IValidationRule<int>
    {
        public async ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> items, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return items.Select(x => ValidationResult.Valid);
        }
    }

    public class SlowRule<TContext> : IValidationRule<int, TContext>
    {
        public async ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> items, TContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return items.Select(x => ValidationResult.Valid);
        }
    }
}
