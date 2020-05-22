using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using App.Metrics.Timer;

namespace SimpleConcepts.ValidationRules
{
    internal static class MetricsExtensions
    {
        private static readonly string[] TIME_KEYS = { "rule", "ruleNamespace", "item", "itemNamespace" };
        private static readonly string[] RESULT_KEYS = { "rule", "ruleNamespace", "item", "itemNamespace", "result" };
        private static readonly string[] EXCEPTION_KEYS = new[] { "rule", "ruleNamespace", "item", "itemNamespace", "exception" };

        public static TimerContext MarkTime<T>(this IMetrics metrics, Type ruleType)
        {
            return metrics.Measure.Timer.Time(MetricsOptions.EXECUTION_TIME,
                new MetricTags(TIME_KEYS, new[] { ruleType.Name, ruleType.Namespace, typeof(T).Name, typeof(T).FullName }));
        }

        public static void MarkResults<T>(this IMetrics metrics, Type ruleType, IEnumerable<ValidationResult> results)
        {
            foreach (var result in results.GroupBy(r => r?.ErrorCode ?? "Valid"))
            {
                metrics.Measure.Meter.Mark(MetricsOptions.RESULT,
                    new MetricTags(RESULT_KEYS, new[] { ruleType.Name, ruleType.Namespace, typeof(T).Name, typeof(T).FullName, result.Key }), result.Count());
            }
        }

        public static void MarkException<T>(this IMetrics metrics, Type ruleType, Exception exception, IEnumerable<T> items)
        {
            metrics.Measure.Meter.Mark(MetricsOptions.EXCEPTION,
                new MetricTags(EXCEPTION_KEYS, new[] { ruleType.Name, ruleType.Namespace, typeof(T).Name, typeof(T).FullName, exception.GetType().FullName }), items.Count());
        }
    }
}
