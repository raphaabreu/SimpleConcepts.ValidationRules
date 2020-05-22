using App.Metrics;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace SimpleConcepts.ValidationRules
{
    internal static class MetricsOptions
    {
        public static readonly MeterOptions RESULT = new MeterOptions
        {
            Name = "ValidationRules Result",
            MeasurementUnit = Unit.Events,
            RateUnit = TimeUnit.Seconds,
            Context = "application"
        };


        public static readonly TimerOptions EXECUTION_TIME = new TimerOptions
        {
            Name = "ValidationRules Execution Time",
            MeasurementUnit = Unit.Requests,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Seconds,
            Context = "application"
        };

        public static readonly MeterOptions EXCEPTION = new MeterOptions
        {
            Name = "ValidationRules Exception",
            MeasurementUnit = Unit.Events,
            RateUnit = TimeUnit.Seconds,
            Context = "application"
        };
    }
}
