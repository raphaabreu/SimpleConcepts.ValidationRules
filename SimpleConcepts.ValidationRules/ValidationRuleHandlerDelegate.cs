using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public delegate ValueTask<ValidationResult[]> ValidationRuleHandlerDelegate();
}
