using System.Collections;
using System.Collections.Generic;
using System.Text;

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

        public string Join(string seperate, string start, string end)
        {
            var sb = new StringBuilder();
            sb.Append(start);
            for (var i = 0; i < Count; i++)
            {
                sb.Append(this[i]);
                if (i != Count - 1)
                {
                    sb.Append(seperate);
                }
            }
            sb.Append(end);
            return sb.ToString();
        }

    }

}
