namespace SimpleConcepts.ValidationRules
{
    public interface IValidationRuleTargetAccess<in TElement>
    {
        IValidationRule<TElement> Target { get; }
    }

    public interface IValidationRuleTargetAccess<in TElement, in TContext>
    {
        IValidationRule<TElement, TContext> Target { get; }
    }
}
