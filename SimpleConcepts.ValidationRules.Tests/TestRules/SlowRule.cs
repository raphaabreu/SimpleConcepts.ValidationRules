using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class SlowRule : IAsyncValidationRule<int>, IValidationRule<int>
    {
        public async ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return source.Select(x => ValidationResult.Valid);
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            return source.Select(x => ValidationResult.Valid);
        }
    }

    public class SlowRule<TContext> : IAsyncValidationRule<int, TContext>, IValidationRule<int, TContext>
    {
        public async ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, TContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);

            return source.Select(x => ValidationResult.Valid);
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source, TContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            return source.Select(x => ValidationResult.Valid);
        }
    }
}
