using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class GreaterThanRule : IAsyncValidationRule<int>, IValidationRule<int>
    {
        private readonly int _threshold;

        public GreaterThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source)
        {
            return Task.FromResult(Validate(source));
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source)
        {
            return source.Select(i => i > _threshold ? ValidationResult.Success : new ValidationResult($"NOT_GREATER_THAN_{_threshold}"));
        }
    }

    public class GreaterThanRule<TContext> : IAsyncValidationRule<int, TContext>, IValidationRule<int, TContext>
    {
        private readonly int _threshold;

        public GreaterThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, TContext context)
        {
            return Task.FromResult(Validate(source, context));
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source, TContext context)
        {
            return source.Select(i => i > _threshold ? ValidationResult.Success : new ValidationResult($"NOT_GREATER_THAN_{_threshold}"));
        }
    }

}
