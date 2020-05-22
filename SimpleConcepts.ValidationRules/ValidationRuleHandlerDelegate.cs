using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public delegate ValueTask<IEnumerable<ValidationResult>> ValidationRuleHandlerDelegate();
}
