using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public class RuleResultsLookup<TElement> : IRuleResultsLookup<TElement>
    {
        private readonly IReadOnlyDictionary<TElement, IEnumerable<RuleResult>> _lookup;
        private readonly IEnumerable<IGrouping<TElement, RuleResult>> _enumerable;

        public RuleResultsLookup(IEnumerable<KeyValuePair<TElement, IEnumerable<RuleResult>>> results)
        {
            _lookup = results.ToDictionary(x => x.Key, x => x.Value);
            _enumerable = results.Select(x => (IGrouping<TElement, RuleResult>)new GroupingAdapter(x));
        }

        public IEnumerator<IGrouping<TElement, RuleResult>> GetEnumerator()
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

        public IEnumerable<RuleResult> this[TElement key] => _lookup[key];

        private struct GroupingAdapter : IGrouping<TElement, RuleResult>
        {
            private readonly KeyValuePair<TElement, IEnumerable<RuleResult>> _source;

            public GroupingAdapter(KeyValuePair<TElement, IEnumerable<RuleResult>> source)
            {
                _source = source;
            }

            public IEnumerator<RuleResult> GetEnumerator()
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
