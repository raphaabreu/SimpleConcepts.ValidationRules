﻿using System;
using System.Diagnostics;

namespace SimpleConcepts.ValidationRules
{
    [DebuggerDisplay("Error = {Result}", Name = "{RuleType.FullName}")]
    public class RuleResult
    {
        public Type RuleType { get; }
        public ValidationResult Result { get; }

        public RuleResult(Type ruleType, ValidationResult result)
        {
            RuleType = ruleType;
            Result = result;
        }

        public static bool operator ==(RuleResult left, RuleResult right)
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

        public static bool operator !=(RuleResult left, RuleResult right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is RuleResult rvr)
            {
                return Equals(rvr);
            }

            return false;
        }

        protected bool Equals(RuleResult other)
        {
            return other.RuleType == RuleType && other.Result == Result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RuleType != null ? RuleType.GetHashCode() : 0) * 397 + (Result != null ? Result.GetHashCode() : 0);
            }
        }
    }
}
