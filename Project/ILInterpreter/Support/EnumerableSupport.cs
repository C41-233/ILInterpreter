using ILInterpreter.Environment.TypeSystem;

namespace ILInterpreter.Support
{
    internal static class EnumerableSupport
    {

        public static bool Equals(IListView<ILType> a , ILType[] b)
        {
            if (a.Count != b.Length)
            {
                return false;
            }
            var count = a.Count;
            for (var i = 0; i < count; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}
