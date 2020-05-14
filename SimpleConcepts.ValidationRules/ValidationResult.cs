using System;
using System.Diagnostics;

namespace SimpleConcepts.ValidationRules
{
    [DebuggerDisplay("{ErrorCode}")]
    public class ValidationResult
    {
        public static ValidationResult Success => null;

        public string ErrorCode { get; }

        public ValidationResult(string errorCode)
        {
            if (string.IsNullOrWhiteSpace(errorCode))
            {
                throw new ArgumentNullException(nameof(errorCode));
            }

            ErrorCode = errorCode;
        }

        public static bool operator ==(ValidationResult left, ValidationResult right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (left is null != right is null)
            {
                return false;
            }

            return left != null && left.Equals(right);
        }

        public static bool operator !=(ValidationResult left, ValidationResult right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is ValidationResult v)
            {
                return Equals(v);
            }

            return false;
        }

        protected bool Equals(ValidationResult other)
        {
            return ErrorCode == other.ErrorCode;
        }

        public override int GetHashCode()
        {
            return ErrorCode.GetHashCode();
        }
    }
}