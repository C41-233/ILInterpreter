using System.Collections.Generic;

namespace ILInterpreter.Support
{
    internal interface IListView<T> : IEnumerable<T>
    {

        int Count { get; }

        T this[int index] { get; }

    }
}
