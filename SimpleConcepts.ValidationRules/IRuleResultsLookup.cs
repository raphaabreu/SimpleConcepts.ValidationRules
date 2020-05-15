using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public interface IRuleResultsLookup<TElement> : ILookup<TElement, RuleResult>
    {

    }
}
