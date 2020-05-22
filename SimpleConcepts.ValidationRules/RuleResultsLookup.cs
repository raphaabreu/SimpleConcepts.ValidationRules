using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    public class RuleResultsLookup<T> : IRuleResultsLookup<T>
    {
        private readonly IReadOnlyDictionary<T, IEnumerable<RuleResult>> _lookup;
        private readonly IEnumerable<IGrouping<T, RuleResult>> _enumerable;

        public RuleResultsLookup(IEnumerable<KeyValuePair<T, IEnumerable<RuleResult>>> results)
        {
            _lookup = results.ToDictionary(x => x.Key, x => x.Value);
            _enumerable = results.Select(x => (IGrouping<T, RuleResult>)new GroupingAdapter(x));
        }

        public IEnumerator<IGrouping<T, RuleResult>> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T key)
        {
            return _lookup.ContainsKey(key);
        }

        public int Count => _lookup.Count;

        public IEnumerable<RuleResult> this[T key] => _lookup[key];

        [DebuggerDisplay("Valid = {Valid}", Name = "{Key}")]
        private struct GroupingAdapter : IGrouping<T, RuleResult>
        {
            private readonly KeyValuePair<T, IEnumerable<RuleResult>> _source;

            public GroupingAdapter(KeyValuePair<T, IEnumerable<RuleResult>> source)
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

            public bool Valid()
            {
                return _source.Value.Valid();
            }

            public T Key => _source.Key;
        }
    }
}
