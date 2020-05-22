using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public interface IRuleResultsLookup<T> : ILookup<T, RuleResult>
    {

    }
}
