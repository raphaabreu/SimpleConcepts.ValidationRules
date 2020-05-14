using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public interface IAsyncValidationRule<in TElement>
    {
        Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source);
    }

    public interface IAsyncValidationRule<in TElement, in TContext>
    {
        Task<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, TContext context);
    }
}