using System.Collections.Generic;

namespace SimpleConcepts.ValidationRules
{
    public interface IValidationRule<in TElement>
    {
        IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source);
    }

    public interface IValidationRule<in TElement, in TContext>
    {
        IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source, TContext context);
    }
}