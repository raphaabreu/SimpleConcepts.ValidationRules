using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class FailingRule : IValidationRule<int>
    {
        public ValueTask<ValidationResult[]> ValidateAsync(int[] items, CancellationToken cancellationToken)
        {
            return new ValueTask<ValidationResult[]>(Task.FromException<ValidationResult[]>(new System.NotImplementedException()));
        }
    }

    public class FailingRule<TContext> : IValidationRule<int, TContext>
    {
        public ValueTask<ValidationResult[]> ValidateAsync(int[] items, TContext context, CancellationToken cancellationToken)
        {
            return new ValueTask<ValidationResult[]>(Task.FromException<ValidationResult[]>(new System.NotImplementedException()));
        }
    }
}
