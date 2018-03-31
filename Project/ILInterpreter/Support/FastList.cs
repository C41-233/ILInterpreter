using System;
using System.Collections;
using System.Text;

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
