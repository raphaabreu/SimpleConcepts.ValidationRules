using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public class DelegatedAsyncValidationRule<TElement, TContext> : IAsyncValidationRule<TElement, TContext>, IAsyncValidationRuleTargetAccess<TElement, TContext>
    {
        private readonly Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, CancellationToken, ValueTask<IEnumerable<ValidationResult>>> _validate;

        public IAsyncValidationRule<TElement, TContext> Target { get; }

        public DelegatedAsyncValidationRule(
            IAsyncValidationRule<TElement, TContext> target,
            Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, CancellationToken, ValueTask<IEnumerable<ValidationResult>>> validate
        )
        {
            Target = target;
            _validate = validate;
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, TContext context, CancellationToken cancellationToken)
        {
            return _validate(Target, source, context, cancellationToken);
        }
    }

    public class DelegatedAsyncValidationRule<TElement> : IAsyncValidationRule<TElement>, IAsyncValidationRuleTargetAccess<TElement>
    {
        private readonly Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, CancellationToken, ValueTask<IEnumerable<ValidationResult>>> _validate;

        public IAsyncValidationRule<TElement> Target { get; }

        public DelegatedAsyncValidationRule(
            IAsyncValidationRule<TElement> target,
            Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, CancellationToken, ValueTask<IEnumerable<ValidationResult>>> validate
        )
        {
            Target = target;
            _validate = validate;
        }

        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, CancellationToken cancellationToken)
        {
            return _validate(Target, source, cancellationToken);
        }
    }
}
