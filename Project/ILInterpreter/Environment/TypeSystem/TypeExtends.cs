using System;
using System.Linq;

namespace ILInterpreter.Environment.TypeSystem
{
    internal static class TypeExtends
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

    }
}
