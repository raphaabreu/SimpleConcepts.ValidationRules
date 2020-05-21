namespace SimpleConcepts.ValidationRules
{
    public interface IAsyncValidationRuleTargetAccess<in TElement>
    {
        IAsyncValidationRule<TElement> Target { get; }
    }

    public interface IAsyncValidationRuleTargetAccess<in TElement, in TContext>
    {
        IAsyncValidationRule<TElement, TContext> Target { get; }
    }
}
