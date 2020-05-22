using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleConcepts.ValidationRules
{
    public static class DependencyInjectionServiceCollectionExtensions
    {
        public static IServiceCollection AddValidator<T>(this IServiceCollection services)
        {
            return services.AddValidator<T>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddValidator<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.TryAdd(ServiceDescriptor.Describe(typeof(IValidator<T>), typeof(Validator<T>), lifetime));

            return services;
        }

        public static IServiceCollection AddValidationRules<T>(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime)
        {
            var ruleTypes = assembly.ExportedTypes.Where(t => typeof(IValidationRule<T>).IsAssignableFrom(t));
            foreach (var ruleType in ruleTypes)
            {
                services.TryAddEnumerable(ServiceDescriptor.Describe(typeof(IValidationRule<T>), ruleType, lifetime));
            }

            var handlerTypes = assembly.ExportedTypes.Where(t => typeof(IValidationRuleHandler<T>).IsAssignableFrom(t));
            foreach (var handlerType in handlerTypes)
            {
                services.TryAddEnumerable(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<T>), handlerType, lifetime));
            }

            return services.AddValidator<T>(lifetime);
        }

        public static IServiceCollection AddValidator<T, TContext>(this IServiceCollection services)
        {
            return services.AddValidator<T, TContext>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddValidator<T, TContext>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.TryAdd(ServiceDescriptor.Describe(typeof(IValidator<T, TContext>), typeof(ContextualizedValidator<T, TContext>), lifetime));

            return services;
        }

        public static IServiceCollection AddValidationRules<T, TContext>(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime)
        {
            var ruleTypes = assembly.ExportedTypes.Where(t => typeof(IValidationRule<T, TContext>).IsAssignableFrom(t));
            foreach (var ruleType in ruleTypes)
            {
                services.TryAddEnumerable(ServiceDescriptor.Describe(typeof(IValidationRule<T, TContext>), ruleType, lifetime));
            }

            var handlerTypes = assembly.ExportedTypes.Where(t => typeof(IValidationRuleHandler<T, TContext>).IsAssignableFrom(t));
            foreach (var handlerType in handlerTypes)
            {
                services.TryAddEnumerable(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<T, TContext>), handlerType, lifetime));
            }

            return services.AddValidator<T, TContext>(lifetime);
        }
    }
}
