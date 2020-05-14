using System;
using System.Diagnostics;

namespace SimpleConcepts.ValidationRules
{
    [DebuggerDisplay("Error = {Result}", Name = "{RuleType.FullName}")]
    public struct RuleValidationResult
    {
        public Type RuleType { get; }
        public ValidationResult Result { get; }

        public RuleValidationResult(Type ruleType, ValidationResult result)
        {
            RuleType = ruleType;
            Result = result;
        }

        public override bool Equals(object obj)
        {
            if (obj is RuleValidationResult rvr)
            {
                return rvr.RuleType == RuleType && rvr.Result == Result;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RuleType != null ? RuleType.GetHashCode() : 0) * 397 + (Result != null ? Result.GetHashCode() : 0);
            }
        }

        public static bool operator ==(RuleValidationResult left, RuleValidationResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RuleValidationResult left, RuleValidationResult right)
        {
            return !(left == right);
        }
    }
}