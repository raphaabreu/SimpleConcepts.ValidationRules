using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;

namespace SimpleConcepts.ValidationRules.AppMetrics
{
    internal class ContextualizedValidationRuleMetricsDecorator<TElement, TContext> : IValidationRule<TElement, TContext>, IAsyncValidationRule<TElement, TContext>, IValidationRuleTargetAccess<TElement, TContext>, IAsyncValidationRuleTargetAccess<TElement, TContext>
    {
        private readonly IValidationRule<TElement, TContext> _target;
        private readonly IMetrics _metrics;
        private readonly IAsyncValidationRule<TElement, TContext> _asyncTarget;

        public ContextualizedValidationRuleMetricsDecorator(IValidationRule<TElement, TContext> target, IMetrics metrics)
        {
            _target = target;
            _metrics = metrics;
        }

        public ContextualizedValidationRuleMetricsDecorator(IAsyncValidationRule<TElement, TContext> asyncTarget, IMetrics metrics)
        {
            _asyncTarget = asyncTarget;
            _metrics = metrics;
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source, TContext context)
        {
            var targetRuleType = _target.GetFinalTarget().GetType();

            try
            {
                IEnumerable<ValidationResult> results;

                using (_metrics.MarkTime<TElement>(targetRuleType))
                {
                    results = _target.Validate(source, context);
                }

                _metrics.MarkResults<TElement>(targetRuleType, results);

                return results;
            }
            catch (Exception ex)
            {
                _metrics.MarkException(targetRuleType, ex, source);

                throw;
            }
        }

        public async ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, TContext context, CancellationToken cancellationToken)
        {
            var targetRuleType = _target.GetFinalTarget().GetType();

            try
            {
                IEnumerable<ValidationResult> results;

                using (_metrics.MarkTime<TElement>(targetRuleType))
                {
                    results = await _asyncTarget.ValidateAsync(source, context, cancellationToken);
                }

                _metrics.MarkResults<TElement>(targetRuleType, results);

                return results;
            }
            catch (Exception ex)
            {
                _metrics.MarkException(targetRuleType, ex, source);

                throw;
            }
        }

        IValidationRule<TElement, TContext> IValidationRuleTargetAccess<TElement, TContext>.Target => _target;

        IAsyncValidationRule<TElement, TContext> IAsyncValidationRuleTargetAccess<TElement, TContext>.Target => _asyncTarget;
    }
}
