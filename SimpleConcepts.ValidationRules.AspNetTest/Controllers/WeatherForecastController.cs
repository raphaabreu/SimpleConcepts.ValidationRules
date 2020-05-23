using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.ValidationRules.AspNetTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IValidator<WeatherForecast> _validator;
        private readonly IValidator<WeatherForecast, DateTime> _datedValidator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IValidator<WeatherForecast> validator, IValidator<WeatherForecast, DateTime> datedValidator)
        {
            _logger = logger;
            _validator = validator;
            _datedValidator = datedValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var rng = new Random();

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            IRuleResultsLookup<WeatherForecast> validationResults;
            if (rng.NextDouble() > 0.5)
            {
                validationResults = await _validator.ValidateAsync(forecasts);
            }
            else
            {
                validationResults = await _datedValidator.ValidateAsync(forecasts, DateTime.UtcNow);
            }

            return validationResults.ValidItems();
        }
    }
}
