using System.Collections;

namespace ILInterpreter.Support
{
    internal sealed partial class FastList<T> : IListView<T>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Push(T value)
        {
            Add(value);
        }

        public T Pop()
        {
            var value = this[Count - 1];
            RemoveAt(Count-1);
            return value;
        }

    }
}
