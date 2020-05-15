using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public class DelegatedAsyncValidationRule<TElement, TContext> : IAsyncValidationRule<TElement, TContext>
    {
        private readonly IAsyncValidationRule<TElement, TContext> _target;
        private readonly Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, CancellationToken, Task<IEnumerable<ValidationResult>>> _validate;

        public DelegatedAsyncValidationRule(
            IAsyncValidationRule<TElement, TContext> target,
            Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, CancellationToken, Task<IEnumerable<ValidationResult>>> validate
        )
        {
            _target = target;
            _validate = validate;
        }

        public Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, TContext context, CancellationToken cancellationToken)
        {
            return _validate(_target, source, context, cancellationToken);
        }
    }

    public class DelegatedAsyncValidationRule<TElement> : IAsyncValidationRule<TElement>
    {
        private readonly IAsyncValidationRule<TElement> _target;
        private readonly Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, CancellationToken, Task<IEnumerable<ValidationResult>>> _validate;

        public DelegatedAsyncValidationRule(
            IAsyncValidationRule<TElement> target,
            Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, CancellationToken, Task<IEnumerable<ValidationResult>>> validate
        )
        {
            _target = target;
            _validate = validate;
        }

        public Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, CancellationToken cancellationToken)
        {
            return _validate(_target, source, cancellationToken);
        }
    }
}
