using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using App.Metrics.Timer;

namespace SimpleConcepts.ValidationRules.AppMetrics
{
    internal static class MetricsExtensions
    {
        private static readonly string[] TIME_KEYS = { "rule", "ruleNamespace", "targetElement" };
        private static readonly string[] RESULT_KEYS = { "rule", "ruleNamespace", "targetElement", "result" };
        private static readonly string[] EXCEPTION_KEYS = new[] { "rule", "ruleNamespace", "targetElement", "exception" };

        public static TimerContext MarkTime<TElement>(this IMetrics metrics, Type ruleType)
        {
            return metrics.Measure.Timer.Time(MetricsOptions.EXECUTION_TIME,
                new MetricTags(TIME_KEYS, new[] { ruleType.Name, ruleType.Namespace, typeof(TElement).FullName }));
        }

        public static void MarkResults<TElement>(this IMetrics metrics, Type ruleType, IEnumerable<ValidationResult> results)
        {
            foreach (var result in results.GroupBy(r => r?.ErrorCode ?? "Valid"))
            {
                metrics.Measure.Meter.Mark(MetricsOptions.RESULT,
                    new MetricTags(RESULT_KEYS, new[] { ruleType.Name, ruleType.Namespace, typeof(TElement).FullName, result.Key }), result.Count());
            }
        }

        public static void MarkException<TElement>(this IMetrics metrics, Type ruleType, Exception exception, IEnumerable<TElement> source)
        {
            metrics.Measure.Meter.Mark(MetricsOptions.EXCEPTION,
                new MetricTags(EXCEPTION_KEYS, new[] { ruleType.Name, ruleType.Namespace, typeof(TElement).FullName, exception.GetType().FullName }), source.Count());
        }
    }
}
