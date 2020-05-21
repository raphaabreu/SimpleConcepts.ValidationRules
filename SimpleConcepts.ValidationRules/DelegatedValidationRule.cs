using System;
using System.Collections.Generic;

namespace SimpleConcepts.ValidationRules
{
    public class DelegatedValidationRule<TElement, TContext> : IValidationRule<TElement, TContext>, IValidationRuleTargetAccess<TElement, TContext>
    {
        private readonly Func<IValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, IEnumerable<ValidationResult>> _validate;

        public IValidationRule<TElement, TContext> Target { get; }

        public DelegatedValidationRule(
            IValidationRule<TElement, TContext> target,
            Func<IValidationRule<TElement, TContext>, IEnumerable<TElement>, TContext, IEnumerable<ValidationResult>> validate
        )
        {
            Target = target;
            _validate = validate;
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source, TContext context)
        {
            return _validate(Target, source, context);
        }
    }

    public class DelegatedValidationRule<TElement> : IValidationRule<TElement>, IValidationRuleTargetAccess<TElement>
    {
        private readonly Func<IValidationRule<TElement>, IEnumerable<TElement>, IEnumerable<ValidationResult>> _validate;

        public IValidationRule<TElement> Target { get; }

        public DelegatedValidationRule(
            IValidationRule<TElement> target,
            Func<IValidationRule<TElement>, IEnumerable<TElement>, IEnumerable<ValidationResult>> validate
        )
        {
            Target = target;
            _validate = validate;
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source)
        {
            return _validate(Target, source);
        }
    }
}
