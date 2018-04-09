using ILInterpreter.Environment.TypeSystem;

namespace ILInterpreter.Support
{
    internal static class CompareSupport
    {

        public static bool Equals(IListView<ILType> a , ILType[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null && b.Length != 0)
            {
                return false;
            }
            if (b == null && a.Count != 0)
            {
                return false;
            }
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

        public static bool Equals(ILType[] a, IListView<ILType> b)
        {
            return Equals(b, a);
        }

    }
}
