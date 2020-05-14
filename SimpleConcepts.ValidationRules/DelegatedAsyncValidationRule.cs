using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public class DelegatedAsyncValidationRule<TElement, TContext> : IAsyncValidationRule<TElement, TContext>
    {
        private readonly IAsyncValidationRule<TElement, TContext> _target;
        private readonly Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, Task<IEnumerable<ValidationResult>>> _validate;

        public DelegatedAsyncValidationRule(
            IAsyncValidationRule<TElement, TContext> target,
            Func<IAsyncValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, Task<IEnumerable<ValidationResult>>> validate
        )
        {
            _target = target;
            _validate = validate;
        }

        public Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, TContext context)
        {
            return _validate(_target, source, context);
        }
    }

    public class DelegatedAsyncValidationRule<TElement> : IAsyncValidationRule<TElement>
    {
        private readonly IAsyncValidationRule<TElement> _target;
        private readonly Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, Task<IEnumerable<ValidationResult>>> _validate;

        public DelegatedAsyncValidationRule(
            IAsyncValidationRule<TElement> target,
            Func<IAsyncValidationRule<TElement>, IEnumerable<TElement>, Task<IEnumerable<ValidationResult>>> validate
        )
        {
            _target = target;
            _validate = validate;
        }

        public Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source)
        {
            return _validate(_target, source);
        }
    }
}