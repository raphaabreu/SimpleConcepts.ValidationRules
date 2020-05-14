using System;
using System.Collections.Generic;

namespace SimpleConcepts.ValidationRules
{
    public class DelegatedValidationRule<TElement, TContext> : IValidationRule<TElement, TContext>
    {
        private readonly IValidationRule<TElement, TContext> _target;
        private readonly Func<IValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, IEnumerable<ValidationResult>> _validate;

        public DelegatedValidationRule(
            IValidationRule<TElement, TContext> target,
            Func<IValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, IEnumerable<ValidationResult>> validate
        )
        {
            _target = target;
            _validate = validate;
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source, TContext context)
        {
            return _validate(_target, source, context);
        }
    }

    public class DelegatedValidationRule<TElement> : IValidationRule<TElement>
    {
        private readonly IValidationRule<TElement> _target;
        private readonly Func<IValidationRule<TElement>, IEnumerable<TElement>, IEnumerable<ValidationResult>> _validate;

        public DelegatedValidationRule(
            IValidationRule<TElement> target,
            Func<IValidationRule<TElement>, IEnumerable<TElement>, IEnumerable<ValidationResult>> validate
        )
        {
            _target = target;
            _validate = validate;
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source)
        {
            return _validate(_target, source);
        }
    }
}