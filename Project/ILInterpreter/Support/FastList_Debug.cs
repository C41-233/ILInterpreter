using System.Collections;
using System.Collections.Generic;

namespace ILInterpreter.Support
{
    internal sealed class FastList<T> : IEnumerable<T>
    {

        private readonly List<T> list = new List<T>();

        public void Add(T element)
        {
            list.Add(element);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public T this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T[] ToArray()
        {
            return list.ToArray();
        }

    }

}
