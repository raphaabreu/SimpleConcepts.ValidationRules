using Microsoft.Extensions.DependencyInjection;

namespace SimpleConcepts.ValidationRules
{
    public static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddValidationLogging(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<>), typeof(ValidationRuleLoggingHandler<>), ServiceLifetime.Singleton));
            services.Add(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<,>), typeof(ContextualizedValidationRuleLoggingHandler<,>), ServiceLifetime.Singleton));

            return services;
        }
    }
}
