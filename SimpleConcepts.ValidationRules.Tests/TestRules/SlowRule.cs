using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class SlowRule : IAsyncValidationRule<int>, IValidationRule<int>
    {
        public async Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));

            return source.Select(x => ValidationResult.Success);
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            return source.Select(x => ValidationResult.Success);
        }
    }

    public class SlowRule<TContext> : IAsyncValidationRule<int, TContext>, IValidationRule<int, TContext>
    {
        public async Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, TContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));

            return source.Select(x => ValidationResult.Success);
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source, TContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            return source.Select(x => ValidationResult.Success);
        }
    }
}