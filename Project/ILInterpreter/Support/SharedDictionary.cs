using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ILInterpreter.Support
{
    internal sealed class SharedDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {

        private readonly Dictionary<K, V> dict = new Dictionary<K, V>();

        public V this[K key]
        {
            get
            {
                lock (dict)
                {
                    return dict[key];
                }
            }
            set
            {
                lock (dict)
                {
                    dict[key] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (dict)
                {
                    return dict.Count;
                }
            }
        }

        public bool TryGetValue(K key, out V value)
        {
            lock (dict)
            {
                return dict.TryGetValue(key, out value);
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            List<KeyValuePair<K, V>> list;
            lock (dict)
            {
                list = dict.ToList();
            }
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
