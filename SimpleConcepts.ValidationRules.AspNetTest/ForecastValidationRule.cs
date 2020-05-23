using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.ValidationRules.AspNetTest
{
    public class ForecastValidationRule : IValidationRule<WeatherForecast>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<WeatherForecast> items, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            if (rnd.NextDouble() > 0.95)
            {
                throw new InvalidOperationException();
            }

            return new ValueTask<IEnumerable<ValidationResult>>(items.Select(x => rnd.NextDouble() > 0.2 ? ValidationResult.Valid : new ValidationResult("INVALID_FOREACAST")));
        }

    }

    public class DatedForecastValidationRule : IValidationRule<WeatherForecast, DateTime>
    {
        public ValueTask<IEnumerable<ValidationResult>> ValidateAsync(IEnumerable<WeatherForecast> items, DateTime context, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            if (rnd.NextDouble() > 0.95)
            {
                throw new InvalidOperationException();
            }

            return new ValueTask<IEnumerable<ValidationResult>>(items.Select(x => rnd.NextDouble() > 0.2 ? ValidationResult.Valid : new ValidationResult("INVALID_FOREACAST")));
        }
    }

    public class ForecastValidationRuleHandler : IValidationRuleHandler<WeatherForecast>
    {
        private readonly ILogger<ForecastValidationRuleHandler> _logger;

        public ForecastValidationRuleHandler(ILogger<ForecastValidationRuleHandler> logger)
        {
            _logger = logger;
        }

        public ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<WeatherForecast> items, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling weather forecast rule.");

            return next();
        }
    }

    public class DatedForecastValidationRuleHandler : IValidationRuleHandler<WeatherForecast, DateTime>
    {
        private readonly ILogger<DatedForecastValidationRuleHandler> _logger;

        public DatedForecastValidationRuleHandler(ILogger<DatedForecastValidationRuleHandler> logger)
        {
            _logger = logger;
        }

        public ValueTask<IEnumerable<ValidationResult>> HandleAsync(Type targetRuleType, IEnumerable<WeatherForecast> items, DateTime context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling dated weather forecast rule.");

            return next();
        }
    }
}
