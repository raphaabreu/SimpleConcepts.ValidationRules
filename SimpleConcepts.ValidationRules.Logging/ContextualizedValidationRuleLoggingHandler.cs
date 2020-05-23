using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleConcepts.ValidationRules
{
    public class ContextualizedValidationRuleLoggingHandler<T, TContext> : IValidationRuleHandler<T, TContext>
    {
        private readonly ILoggerFactory _loggerFactory;

        public ContextualizedValidationRuleLoggingHandler(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async ValueTask<ValidationResult[]> HandleAsync(Type targetRuleType, T[] items, TContext context, ValidationRuleHandlerDelegate next,
            CancellationToken cancellationToken)
        {
            using (var loggingContext = new LoggingContext<T>(_loggerFactory, targetRuleType, items.Length))
            {
                try
                {
                    var results = await next();

                    loggingContext.Results(results);

                    return results;
                }
                catch (Exception ex)
                {
                    loggingContext.Exception(ex);
                    throw;
                }
            }
        }
    }
}
