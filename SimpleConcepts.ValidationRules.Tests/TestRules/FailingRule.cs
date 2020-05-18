﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules.Tests.TestRules
{
    public class FailingRule : IAsyncValidationRule<int>, IValidationRule<int>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(Task.FromException<IEnumerable<ValidationResult>>(new System.NotImplementedException()));
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source)
        {
            throw new System.NotImplementedException();
        }
    }

    public class FailingRule<TContext> : IAsyncValidationRule<int, TContext>, IValidationRule<int, TContext>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<int> source, TContext context, CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<ValidationResult>>(Task.FromException<IEnumerable<ValidationResult>>(new System.NotImplementedException()));
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<int> source, TContext context)
        {
            throw new System.NotImplementedException();
        }
    }

}
