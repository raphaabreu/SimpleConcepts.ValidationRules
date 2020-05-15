using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleConcepts.ValidationRules
{
    internal class ValidationResultLookup<TElement> : ILookup<TElement, Validation>
    {
        private readonly IReadOnlyDictionary<TElement, IEnumerable<Validation>> _lookup;
        private readonly IEnumerable<IGrouping<TElement, Validation>> _enumerable;

        public ValidationResultLookup(IEnumerable<KeyValuePair<TElement, IEnumerable<Validation>>> results)
        {
            _lookup = results.ToDictionary(x => x.Key, x => x.Value);
            _enumerable = results.Select(x => (IGrouping<TElement, Validation>)new GroupingAdapter(x));
        }

        public IEnumerator<IGrouping<TElement, Validation>> GetEnumerator()
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

        public IEnumerable<Validation> this[TElement key] => _lookup[key];

        private struct GroupingAdapter : IGrouping<TElement, Validation>
        {
            private readonly KeyValuePair<TElement, IEnumerable<Validation>> _source;

            public GroupingAdapter(KeyValuePair<TElement, IEnumerable<Validation>> source)
            {
                _source = source;
            }

            public IEnumerator<Validation> GetEnumerator()
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
