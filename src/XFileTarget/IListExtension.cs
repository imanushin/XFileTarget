using System.Collections;
using System.Collections.Generic;

namespace XFileTarget
{
    internal static class ListExtension
    {
        public static ListEnumerable<TValue> Enumerate<TValue>(this IList<TValue> list)
        {
            return new ListEnumerable<TValue>(list);
        }

        public struct ListEnumerable<TValue> : IEnumerable<TValue>
        {
            private readonly IList<TValue> _list;

            public ListEnumerable(IList<TValue> list)
            {
                _list = list;
            }

            public ListEnumerator<TValue> GetEnumerator()
            {
                return new ListEnumerator<TValue>(_list);
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return new ListEnumerator<TValue>(_list);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public struct ListEnumerator<TValue> : IEnumerator<TValue>
        {
            private readonly IList<TValue> _list;
            private readonly int _initialSize;
            private int _currentIndex;

            public ListEnumerator(IList<TValue> list)
            {
                _list = list;
                _currentIndex = -1;
                _initialSize = list.Count;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                _currentIndex++;

                return _currentIndex < _initialSize;
            }

            public void Reset()
            {
                _currentIndex = 0;
            }

            public TValue Current => _list[_currentIndex];

            object IEnumerator.Current => Current;
        }
    }
}