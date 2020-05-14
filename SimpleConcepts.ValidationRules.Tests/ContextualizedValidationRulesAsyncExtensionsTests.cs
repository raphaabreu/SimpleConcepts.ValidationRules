﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SimpleConcepts.ValidationRules.Tests.TestRules;
using Xunit;

namespace SimpleConcepts.ValidationRules.Tests
{
    public class ContextualizedValidationRulesAsyncExtensionsTests
    {
        [Fact]
        public async Task ValidateAsync_WithNoElements_DoesNotInvokeRules()
        {
            // Arrange
            var rules = new[] { new FailingRule<string>() };
            var elements = Array.Empty<int>();

            // Act
            var result = await elements.ValidateAsync(rules, "context");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ValidateAsync_WithSlowRule_InvokeAllInParallel()
        {
            // Arrange
            var rules = new[] { new SlowRule<string>(), new SlowRule<string>(), new SlowRule<string>(), new SlowRule<string>() };
            var elements = new[] { 1, 2, 3, 4 };
            var stopWatch = new Stopwatch();

            // Act
            stopWatch.Start();
            var results = await elements.ValidateAsync(rules, "context");
            stopWatch.Stop();

            // Assert
            Assert.Equal(16, results.SelectMany(x => x).Count());
            Assert.True(stopWatch.Elapsed < TimeSpan.FromSeconds(0.6));
        }

        [Fact]
        public async Task ValidateAsync_WithMixedResults_MergeResults()
        {
            // Arrange
            var rules = new IAsyncValidationRule<int, string>[]
            {
                new GreaterThanRule<string>(5),
                new GreaterThanRule<string>(7),
                new LowerThanRule<string>(10),
                new LowerThanRule<string>(12)
            };
            var elements = new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

            // Act
            var results = await elements.ValidateAsync(rules, "context");

            // Assert
            Assert.Equal(10, results.Count());
            Assert.Equal(new[] { "NOT_GREATER_THAN_5", "NOT_GREATER_THAN_7" }, results[4].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_5", "NOT_GREATER_THAN_7" }, results[5].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_7" }, results[6].ErrorCodes());
            Assert.Equal(new[] { "NOT_GREATER_THAN_7" }, results[7].ErrorCodes());
            Assert.True(results[8].Success());
            Assert.True(results[9].Success());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10" }, results[10].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10" }, results[11].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10", "NOT_LOWER_THAN_12" }, results[12].ErrorCodes());
            Assert.Equal(new[] { "NOT_LOWER_THAN_10", "NOT_LOWER_THAN_12" }, results[13].ErrorCodes());
        }

        [Fact]
        public async Task ValidateAsync_WithDelegatedRule_DelegateGetsCalled()
        {
            // Arrange
            var rules = new IAsyncValidationRule<int, string>[]
            {
                new GreaterThanRule<string>(5),
            };
            var elements = new[] { 1 };
            var called = false;
            var delegated = rules.WithDelegate((rule, e, c) =>
            {
                called = true;
                return rule.ValidateAsync(e, c);
            });

            // Act
            await elements.ValidateAsync(delegated, "context");

            // Assert
            Assert.True(called);
        }
    }
}