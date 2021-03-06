﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class LowerThanRule : IValidationRule<int>
    {
        private readonly int _threshold;

        public LowerThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public ValueTask<ValidationResult[]> ValidateAsync(int[] items, CancellationToken cancellationToken)
        {
            return new ValueTask<ValidationResult[]>(items
                .Select(i => i < _threshold ? ValidationResult.Valid : new ValidationResult($"NOT_LOWER_THAN_{_threshold}"))
                .ToArray());
        }
    }

    public class LowerThanRule<TContext> : IValidationRule<int, TContext>
    {
        private readonly int _threshold;

        public LowerThanRule(int threshold)
        {
            _threshold = threshold;
        }

        public ValueTask<ValidationResult[]> ValidateAsync(int[] items, TContext context, CancellationToken cancellationToken)
        {
            return new ValueTask<ValidationResult[]>(items
                .Select(i => i < _threshold ? ValidationResult.Valid : new ValidationResult($"NOT_LOWER_THAN_{_threshold}"))
                .ToArray());
        }
    }
}
