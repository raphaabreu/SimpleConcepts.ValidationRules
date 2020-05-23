using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.ValidationRules.AspNetTest
{
    public class ForecastValidationRule : IValidationRule<WeatherForecast>
    {
        public ValueTask<ValidationResult[]> ValidateAsync(WeatherForecast[] items, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            if (rnd.NextDouble() > 0.95)
            {
                throw new InvalidOperationException();
            }

            return new ValueTask<ValidationResult[]>(items
                .Select(x => rnd.NextDouble() > 0.2 ? ValidationResult.Valid : new ValidationResult("INVALID_FOREACAST"))
                .ToArray());
        }

    }

    public class DatedForecastValidationRule : IValidationRule<WeatherForecast, DateTime>
    {
        public ValueTask<ValidationResult[]> ValidateAsync(WeatherForecast[] items, DateTime context, CancellationToken cancellationToken)
        {
            var rnd = new Random();

            if (rnd.NextDouble() > 0.95)
            {
                throw new InvalidOperationException();
            }

            return new ValueTask<ValidationResult[]>(items
                .Select(x => rnd.NextDouble() > 0.2 ? ValidationResult.Valid : new ValidationResult("INVALID_FOREACAST"))
                .ToArray());
        }
    }

    public class ForecastValidationRuleHandler : IValidationRuleHandler<WeatherForecast>
    {
        private readonly ILogger<ForecastValidationRuleHandler> _logger;

        public ForecastValidationRuleHandler(ILogger<ForecastValidationRuleHandler> logger)
        {
            _logger = logger;
        }

        public ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, WeatherForecast[] items, ValidationRuleHandlerDelegate next,
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

        public ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, WeatherForecast[] items, DateTime context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling dated weather forecast rule.");

            return next();
        }
    }
}
