using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class LowerThanRule : IAsyncValidationRule<int>, IValidationRule<int>
    {
        private readonly int _threshold;

        public LowerThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(Validate(source));
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source)
        {
            return source.Select(i => i < _threshold ? ValidationResult.Valid : new ValidationResult($"NOT_LOWER_THAN_{_threshold}"));
        }
    }

    public class LowerThanRule<TContext> : IAsyncValidationRule<int, TContext>, IValidationRule<int, TContext>
    {
        private readonly int _threshold;

        public LowerThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, TContext context, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(Validate(source, context));
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source, TContext context)
        {
            return source.Select(i => i < _threshold ? ValidationResult.Valid : new ValidationResult($"NOT_LOWER_THAN_{_threshold}"));
        }
    }
}
