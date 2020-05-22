using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;

namespace SimpleConcepts.ValidationRules
{
    public class ValidationRuleMetricsHandler<T> : IValidationRuleHandler<T>
    {
        private readonly IMetrics _metrics;

        public ValidationRuleMetricsHandler(IMetrics metrics)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        public async ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<T> items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<ValidationResult> results;

                using (_metrics.MarkTime<T>(targetRuleType))
                {
                    results = await next();
                }

                _metrics.MarkResults<T>(targetRuleType, results);

                return results;
            }
            catch (Exception ex)
            {
                _metrics.MarkException(targetRuleType, ex, items);

                throw;
            }
        }
    }
}
