using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SimpleConcepts.ValidationRules.Tests.TestHandlers;
using SimpleConcepts.ValidationRules.Tests.TestRules;
using Xunit;

namespace SimpleConcepts.ValidationRules.Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public async Task ValidateAsync_WithValidatorOfTFromDependencyInjection_InvokesHandlersAndReturnsExpectedResult()
        {
            // Arrange
            var items = new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            var rules = new IValidationRule<int>[]
            {
                new GreaterThanRule(5),
                new GreaterThanRule(7),
                new LowerThanRule(10),
                new LowerThanRule(12)
            };

            var services = new ServiceCollection();

            services
                .AddValidator<int>()
                .AddScoped(p => rules[0])
                .AddScoped(p => rules[1])
                .AddScoped(p => rules[2])
                .AddScoped(p => rules[3])
                .AddSingleton<IValidationRuleHandler<int>>(p => new TestHandler<int>())
                .Add(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<>), typeof(TestHandler<>), ServiceLifetime.Singleton));

            var provider = services.BuildServiceProvider();
            var scope = provider.CreateScope();

            var handlers = scope.ServiceProvider.GetServices<IValidationRuleHandler<int>>();

            // Act
            var validator = scope.ServiceProvider.GetService<IValidator<int>>();
            var results = await validator.ValidateAsync(items);

            // Assert
            Assert.Equal(2, handlers.Count());
            Assert.All(handlers, h => Assert.Equal(4, ((TestHandler<int>)h).HandleCount));
            Assert.Equal(10, results.Count());
            Assert.Equal(new[] { "NOT_GREATER_THAN_5", "NOT_GREATER_THAN_7" }, results[4].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_5", "NOT_GREATER_THAN_7" }, results[5].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_7" }, results[6].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_7" }, results[7].ErrorCodes());
            Assert.True(results[8].Valid());
            Assert.True(results[9].Valid());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10" }, results[10].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10" }, results[11].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10", "NOT_LOWER_THAN_12" }, results[12].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10", "NOT_LOWER_THAN_12" }, results[13].ErrorCodes());
        }

        [Fact]
        public async Task ValidateAsync_WithValidatorOfTAndTContextFromDependencyInjection_InvokesHandlersAndReturnsExpectedResult()
        {
            // Arrange
            var items = new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            var rules = new IValidationRule<int, string>[]
            {
                new GreaterThanRule<string>(5),
                new GreaterThanRule<string>(7),
                new LowerThanRule<string>(10),
                new LowerThanRule<string>(12)
            };

            var services = new ServiceCollection();

            services
                .AddValidator<int, string>()
                .AddScoped(p => rules[0])
                .AddScoped(p => rules[1])
                .AddScoped(p => rules[2])
                .AddScoped(p => rules[3])
                .AddSingleton<IValidationRuleHandler<int, string>>(p => new ContextualizedTestHandler<int, string>())
                .Add(ServiceDescriptor.Describe(typeof(IValidationRuleHandler<,>), typeof(ContextualizedTestHandler<,>), ServiceLifetime.Singleton));

            var provider = services.BuildServiceProvider();
            var scope = provider.CreateScope();

            var handlers = scope.ServiceProvider.GetServices<IValidationRuleHandler<int, string>>();

            // Act
            var validator = scope.ServiceProvider.GetService<IValidator<int, string>>();
            var results = await validator.ValidateAsync(items, "test");

            // Assert
            Assert.Equal(2, handlers.Count());
            Assert.All(handlers, h => Assert.Equal(4, ((ContextualizedTestHandler<int, string>)h).HandleCount));
            Assert.Equal(10, results.Count());
            Assert.Equal(new[] { "NOT_GREATER_THAN_5", "NOT_GREATER_THAN_7" }, results[4].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_5", "NOT_GREATER_THAN_7" }, results[5].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_7" }, results[6].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_7" }, results[7].ErrorCodes());
            Assert.True(results[8].Valid());
            Assert.True(results[9].Valid());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10" }, results[10].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10" }, results[11].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10", "NOT_LOWER_THAN_12" }, results[12].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10", "NOT_LOWER_THAN_12" }, results[13].ErrorCodes());
        }

        [Fact]
        public void AddValidationRulesOfT_WithAssemblyScan_RegistersCorrectRulesAndHandlers()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddValidationRules<byte>(typeof(DependencyInjectionTests).Assembly, ServiceLifetime.Transient);

            // Assert
            Assert.Equal(3, services.Count);
            Assert.All(services, d => Assert.Equal(ServiceLifetime.Transient, d.Lifetime));
            Assert.Contains(services,
                d => d.ServiceType == typeof(IValidator<byte>) &&
                     d.ImplementationType == typeof(Validator<byte>));
            Assert.Contains(services,
                d => d.ServiceType == typeof(IValidationRuleHandler<byte>) &&
                     d.ImplementationType == typeof(ClosedHandler));
            Assert.Contains(services,
                d => d.ServiceType == typeof(IValidationRule<byte>) &&
                     d.ImplementationType == typeof(ClosedRule));
        }

        [Fact]
        public void AddValidationRulesOfTAndTContext_WithAssemblyScan_RegistersCorrectRulesAndHandlers()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddValidationRules<string, long>(typeof(DependencyInjectionTests).Assembly, ServiceLifetime.Transient);

            // Assert
            Assert.Equal(3, services.Count);
            Assert.All(services, d => Assert.Equal(ServiceLifetime.Transient, d.Lifetime));
            Assert.Contains(services,
                d => d.ServiceType == typeof(IValidator<string, long>) &&
                     d.ImplementationType == typeof(ContextualizedValidator<string, long>));
            Assert.Contains(services,
                d => d.ServiceType == typeof(IValidationRuleHandler<string, long>) &&
                     d.ImplementationType == typeof(ClosedHandler));
            Assert.Contains(services,
                d => d.ServiceType == typeof(IValidationRule<string, long>) &&
                     d.ImplementationType == typeof(ClosedRule));
        }
    }
}
