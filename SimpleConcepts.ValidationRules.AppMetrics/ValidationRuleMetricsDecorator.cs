using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;

namespace SimpleConcepts.ValidationRules.AppMetrics
{
    internal class ValidationRuleMetricsDecorator<TElement> : IValidationRule<TElement>, IAsyncValidationRule<TElement>, IValidationRuleTargetAccess<TElement>, IAsyncValidationRuleTargetAccess<TElement>
    {
        private readonly IValidationRule<TElement> _target;
        private readonly IMetrics _metrics;
        private readonly IAsyncValidationRule<TElement> _asyncTarget;

        public ValidationRuleMetricsDecorator(IValidationRule<TElement> target, IMetrics metrics)
        {
            _target = target;
            _metrics = metrics;
        }

        public ValidationRuleMetricsDecorator(IAsyncValidationRule<TElement> asyncTarget, IMetrics metrics)
        {
            _asyncTarget = asyncTarget;
            _metrics = metrics;
        }

        public IEnumerable<ValidationResult> Validate(IEnumerable<TElement> source)
        {
            var targetRuleType = _target.GetFinalTarget().GetType();

            try
            {
                IEnumerable<ValidationResult> results;

                using (_metrics.MarkTime<TElement>(targetRuleType))
                {
                    results = _target.Validate(source);
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

        public async ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<TElement> source, CancellationToken cancellationToken)
        {
            var targetRuleType = _target.GetFinalTarget().GetType();

            try
            {
                IEnumerable<ValidationResult> results;

                using (_metrics.MarkTime<TElement>(targetRuleType))
                {
                    results = await _asyncTarget.ValidateAsync(source, cancellationToken);
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

        IValidationRule<TElement> IValidationRuleTargetAccess<TElement>.Target => _target;

        IAsyncValidationRule<TElement> IAsyncValidationRuleTargetAccess<TElement>.Target => _asyncTarget;
    }
}
