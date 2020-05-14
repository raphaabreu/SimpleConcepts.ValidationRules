using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    internal class ValidationResultLookup<TElement> : ILookup<TElement, RuleValidationResult>
    {
        private readonly IReadOnlyDictionary<TElement, IEnumerable<RuleValidationResult>> _lookup;
        private readonly IEnumerable<IGrouping<TElement, RuleValidationResult>> _enumerable;

        public ValidationResultLookup(IEnumerable<KeyValuePair<TElement, IEnumerable<RuleValidationResult>>> results)
        {
            _lookup = results.ToDictionary(x => x.Key, x => x.Value);
            _enumerable = results.Select(x => (IGrouping<TElement, RuleValidationResult>)new GroupingAdapter(x));
        }

        public IEnumerator<IGrouping<TElement, RuleValidationResult>> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(TElement key)
        {
            return _lookup.ContainsKey(key);
        }

        public int Count => _lookup.Count;

        public IEnumerable<RuleValidationResult> this[TElement key] => _lookup[key];

        private struct GroupingAdapter : IGrouping<TElement, RuleValidationResult>
        {
            private readonly KeyValuePair<TElement, IEnumerable<RuleValidationResult>> _source;

            public GroupingAdapter(KeyValuePair<TElement, IEnumerable<RuleValidationResult>> source)
            {
                _source = source;
            }

            public IEnumerator<RuleValidationResult> GetEnumerator()
            {
                return _source.Value.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public TElement Key => _source.Key;
        }
    }
}