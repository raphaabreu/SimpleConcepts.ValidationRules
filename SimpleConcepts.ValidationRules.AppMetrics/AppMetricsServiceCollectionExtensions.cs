using Microsoft.Extensions.DependencyInjection;

namespace SimpleConcepts.ValidationRules
{
    public static class AppMetricsServiceCollectionExtensions
    {
        public static IServiceCollection AddValidationMetrics(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<>), typeof(ValidationRuleMetricsHandler<>), ServiceLifetime.Singleton));
            services.Add(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<,>), typeof(ContextualizedValidationRuleMetricsHandler<,>), ServiceLifetime.Singleton));

            return services;
        }
    }
}
