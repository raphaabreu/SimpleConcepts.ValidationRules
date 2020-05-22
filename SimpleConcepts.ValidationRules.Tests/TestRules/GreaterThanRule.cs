using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class GreaterThanRule : IValidationRule<int>
    {
        private readonly int _threshold;

        public GreaterThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> items, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(items.Select(i => i > _threshold ? ValidationResult.Valid : new ValidationResult($"NOT_GREATER_THAN_{_threshold}")));
        }
    }

    public class GreaterThanRule<TContext> : IValidationRule<int, TContext>
    {
        private readonly int _threshold;

        public GreaterThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> items, TContext context, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(items.Select(i => i > _threshold ? ValidationResult.Valid : new ValidationResult($"NOT_GREATER_THAN_{_threshold}")));
        }
    }
}
