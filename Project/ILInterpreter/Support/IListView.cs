using System.Collections.Generic;

namespace ILInterpreter.Support
{
    public interface IListView<T> : IEnumerable<T>
    {

        int Count { get; }

        T this[int index] { get; }

    }
}
