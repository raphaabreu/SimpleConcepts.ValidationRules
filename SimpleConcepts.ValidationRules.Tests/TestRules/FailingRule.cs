using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class FailingRule : IValidationRule<int>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> items, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(Task.FromException<IEnumerable<ValidationResult>>(new System.NotImplementedException()));
        }
    }

    public class FailingRule<TContext> : IValidationRule<int, TContext>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> items, TContext context, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(Task.FromException<IEnumerable<ValidationResult>>(new System.NotImplementedException()));
        }
    }
}
