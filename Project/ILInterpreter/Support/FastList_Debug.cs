using System.Collections.Generic;

namespace ILInterpreter.Support
{
    internal sealed partial class FastList<T>
    {

        private readonly List<T> list;

        public FastList()
        {
            list = new List<T>();
        }

        public FastList(int capacity)
        {
            list = new List<T>(capacity);
        }

        public void Add(T element)
        {
            list.Add(element);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
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

        public T[] ToArray()
        {
            return list.ToArray();
        }

        public void Trim()
        {
            list.TrimExcess();
        }

        public void Clear()
        {
            list.Clear();
        }
    }

}
