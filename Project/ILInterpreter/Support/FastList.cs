using System.Collections;

namespace ILInterpreter.Support
{
    internal sealed partial class FastList<T> : IListView<T>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
