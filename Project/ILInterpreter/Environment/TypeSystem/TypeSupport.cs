using System;
using System.Linq;
using System.Text;
using Mono.Cecil;

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

        public static string GetTypeAssemblyQualifiedName(this TypeReference reference)
        {
            return GetTypeName(reference, true);
        }

        private static string GetTypeName(TypeReference reference, bool assembly)
        {
            var sb = new StringBuilder();

            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (reference is PointerType)
            {
                var pointerType = (PointerType) reference;
                var element = pointerType.GetElementType();
                sb.Append(GetTypeName(element, false));
                sb.Append('*');
            }
            else if (reference is ByReferenceType)
            {
                var refType = (ByReferenceType) reference;
                var element = refType.GetElementType();
                sb.Append(GetTypeName(element, false));
                sb.Append('&');
            }
            else if (reference is ArrayType)
            {
                var arrayType = (ArrayType) reference;
                var element = arrayType.GetElementType();
                sb.Append(GetTypeName(element, false));
                sb.Append(GetArrayString(arrayType.Rank));
            }
            else
            {
                sb.Append(reference.FullName);
            }

            if (assembly)
            {
                sb.Append(", ");
                sb.Append(reference.Scope.Name);
            }
            return sb.ToString();
        }

    }
}
