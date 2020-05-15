using System;
using Xunit;

namespace SimpleConcepts.ValidationRules.Tests
{
    public class ValidationResultTests
    {
        [Fact]
        public void Constructor_WithNull_Throws()
        {
            // Arrange
            // Act
            Action act = () => new ValidationResult(null);

            // Assert
            Assert.Throws<ArgumentNullException>("errorCode", act);
        }

        [Fact]
        public void Constructor_WithEmptySpaces_Throws()
        {
            // Arrange
            // Act
            Action act = () => new ValidationResult(" ");

            // Assert
            Assert.Throws<ArgumentNullException>("errorCode", act);
        }

        [Fact]
        public void OperatorEqual_WithSuccesses_ReturnsTrue()
        {
            // Arrange
            var left = ValidationResult.Valid;
            var right = ValidationResult.Valid;

            // Act
            var result = left == right;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorEqual_WithEqualErrorCodes_ReturnsTrue()
        {
            // Arrange
            var left = new ValidationResult("error123");
            var right = new ValidationResult("error123");

            // Act
            var result = left == right;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorEqual_WithSameInstance_ReturnsTrue()
        {
            // Arrange
            var left = new ValidationResult("error123");
            var right = left;

            // Act
            var result = left == right;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorEqual_WithDifferingErrorCodes_ReturnsFalse()
        {
            // Arrange
            var left = new ValidationResult("error123");
            var right = new ValidationResult("error321");

            // Act
            var result = left == right;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OperatorEqual_WithSuccessAndError_ReturnsFalse()
        {
            // Arrange
            var left = ValidationResult.Valid;
            var right = new ValidationResult("error321");

            // Act
            var result = left == right;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OperatorNotEqual_WithSuccessAndError_ReturnsTrue()
        {
            // Arrange
            var left = ValidationResult.Valid;
            var right = new ValidationResult("error321");

            // Act
            var result = left != right;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithSameInstance_ReturnsTrue()
        {
            // Arrange
            var left = new ValidationResult("error123");
            var right = left;

            // Act
            var result = left.Equals(right);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithDifferingErrorCodes_ReturnsFalse()
        {
            // Arrange
            var left = new ValidationResult("error123");
            var right = new ValidationResult("error321");

            // Act
            var result = left.Equals(right);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_WithSuccessAndError_ReturnsFalse()
        {
            // Arrange
            var left = new ValidationResult("error321");
            var right = ValidationResult.Valid;

            // Act
            var result = left.Equals(right);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_WithOtherType_ReturnsFalse()
        {
            // Arrange
            var left = new ValidationResult("error321");
            var right = "otherType";

            // Act
            var result = left.Equals(right);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_WithErrorCode_ReturnsErrorCodeHashCode()
        {
            // Arrange
            var validation = new ValidationResult("error123");

            // Act
            var result = validation.GetHashCode();

            // Assert
            Assert.Equal("error123".GetHashCode(), result);
        }
    }
}