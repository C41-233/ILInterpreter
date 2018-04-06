using System;
using System.Linq;
using System.Text;

namespace ILInterpreter.Environment.TypeSystem
{
    internal static class TypeSupport
    {

        public static Type GetType(string fullname)
        {
            var type = Type.GetType(fullname);
            if (type != null)
            {
                return type;
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                type = assembly.GetType(fullname);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        public static string GetArrayString(int rank)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            for (var i = 0; i < rank - 1; i++)
            {
                sb.Append(',');
            }
            sb.Append(']');
            return sb.ToString();
        }

    }
}
