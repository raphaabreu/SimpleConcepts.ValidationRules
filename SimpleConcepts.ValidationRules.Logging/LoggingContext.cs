using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.ValidationRules
{
    internal class LoggingContext<T> : IDisposable
    {
        private readonly ILogger _logger;
        private readonly Type _targetRuleType;
        private readonly IDisposable _disposable;
        private Stopwatch _timer;

        public LoggingContext(ILoggerFactory loggerFactory, Type targetRuleType, int itemCount)
        {
            _logger = loggerFactory.CreateLogger(targetRuleType);
            _targetRuleType = targetRuleType;
            _timer = new Stopwatch();

            _disposable = _logger.BeginScope("Validation rule {ValidationRuleName} with {ItemCount} {ItemType}", targetRuleType.Name, itemCount, typeof(T).FullName);

            _logger.LogDebug("Validation rule {ValidationRuleName} starting with {ItemCount} items", itemCount, typeof(T).FullName, _targetRuleType.Name);
            _timer.Start();
        }

        public void Exception(Exception ex)
        {
            _timer.Stop();
            _logger.LogError(ex, "Validation rule {ValidationRuleName} failed in {ElapsedMilliseconds}ms", _targetRuleType.Name, _timer.Elapsed.TotalMilliseconds);
            _timer = null;
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _logger.LogDebug("Validation rule {ValidationRuleName} finished in {ElapsedMilliseconds}ms", _targetRuleType.Name, _timer.Elapsed.TotalMilliseconds);
            }

            _disposable.Dispose();
        }
    }
}
