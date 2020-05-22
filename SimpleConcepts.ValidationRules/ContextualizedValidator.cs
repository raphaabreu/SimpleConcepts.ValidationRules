using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleConcepts.ValidationRules
{
    public class ContextualizedValidator<T, TContext> : IValidator<T, TContext>
    {
        private readonly IEnumerable<IValidationRule<T, TContext>> _rules;
        private readonly IEnumerable<IValidationRuleHandler<T, TContext>> _handlers;

        public ContextualizedValidator(IEnumerable<IValidationRule<T, TContext>> rules, IEnumerable<IValidationRuleHandler<T, TContext>> handlers)
        {
            _rules = rules;
            _handlers = handlers;
        }

        public async ValueTask<IRuleResultsLookup<T>> ValidateAsync(IEnumerable<T> items, TContext context, CancellationToken cancellationToken = default)
        {
            // Copy to array to retain indexes.
            var itemsArray = items.ToArray();

            // If there are no elements there is nothing else to do.
            if (itemsArray.Length == 0)
            {
                return new RuleResultsLookup<T>(Array.Empty<KeyValuePair<T, IEnumerable<RuleResult>>>());
            }

            // Compute all rules in parallel.
            var validationTasks = _rules
                .Select(rule => HandleAsync(rule, items, context, cancellationToken))
                .ToArray();

            var ruleResults = new List<RuleResult[]>(validationTasks.Length);
            foreach (var task in validationTasks)
            {
                ruleResults.Add(await task);
            }

            // Aggregate all results by item.
            var results = itemsArray
                .Select((item, index) => new KeyValuePair<T, IEnumerable<RuleResult>>(item, ruleResults.Select(r => r[index])));

            return new RuleResultsLookup<T>(results);
        }

        private async ValueTask<RuleResult[]> HandleAsync(IValidationRule<T, TContext> rule, IEnumerable<T> items, TContext context, CancellationToken cancellationToken)
        {
            // Creates final handler that executes the rule itself.
            async ValueTask<IEnumerable<ValidationResult>> finalHandler()
            {
                return (await rule.ValidateAsync(items, context, cancellationToken)).ToArray();
            }

            // Creates the handler chain by reversing the original list of handlers then aggregating them with the final handler as the seed.
            var handlerChain = _handlers
                .Reverse()
                .Aggregate((ValidationRuleHandlerDelegate)finalHandler,
                    (next, handler) => () => handler.HandleAsync(rule.GetType(), items, context, next, cancellationToken));

            var validationResult = await handlerChain();

            return validationResult
                .Select(result => new RuleResult(rule.GetType(), result))
                .ToArray();
        }
    }
}
