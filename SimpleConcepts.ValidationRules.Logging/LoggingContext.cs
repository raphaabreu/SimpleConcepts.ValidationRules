using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.ValidationRules
{
    internal class LoggingContext<T> : IDisposable
    {
        private readonly ILogger _logger;
        private readonly Type _targetRuleType;
        private readonly int _itemCount;
        private readonly IDisposable _disposable;
        private Stopwatch _timer;

        public LoggingContext(ILoggerFactory loggerFactory, Type targetRuleType, int itemCount)
        {
            _logger = loggerFactory.CreateLogger(targetRuleType);
            _targetRuleType = targetRuleType;
            _itemCount = itemCount;
            _timer = new Stopwatch();

            _disposable = _logger.BeginScope("Validation rule {ValidationRuleName} with {ItemCount} {ItemType}", targetRuleType.Name, _itemCount, typeof(T).FullName);

            _logger.LogDebug("Validation rule {ValidationRuleName} starting with {ItemCount} items", _targetRuleType.Name, itemCount);
            _timer.Start();
        }

        public void Results(ValidationResult[] results)
        {
            var invalidCount = results.Count(r => r != ValidationResult.Valid);

            _timer.Stop();
            _logger.LogDebug("Validation rule {ValidationRuleName} finished in {ElapsedMilliseconds}ms with {InvalidItemCount} invalid items of {ItemCount}", _targetRuleType.Name, _timer.Elapsed.TotalMilliseconds, invalidCount, _itemCount);
        }

        public void Exception(Exception ex)
        {
            _timer.Stop();
            _logger.LogError(ex, "Validation rule {ValidationRuleName} failed in {ElapsedMilliseconds}ms", _targetRuleType.Name, _timer.Elapsed.TotalMilliseconds);
            _timer = null;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
