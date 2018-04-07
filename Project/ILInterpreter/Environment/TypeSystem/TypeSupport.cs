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

            var pointerType = reference as PointerType;
            if (pointerType != null)
            {
                var element = pointerType.GetElementType();
                sb.Append(GetTypeName(element, false));
                sb.Append('*');
            }

            var refType = reference as ByReferenceType;
            if (refType != null)
            {
                var element = refType.GetElementType();
                sb.Append(GetTypeName(element, false));
                sb.Append('&');
            }

            var arrayType = reference as ArrayType;
            if (arrayType != null)
            {
                var element = arrayType.GetElementType();
                sb.Append(GetTypeName(element, false));
                sb.Append(GetArrayString(arrayType.Rank));
            }

            var definition = reference as TypeDefinition;
            if (definition != null)
            {
                sb.Append(definition.FullName);
            }

            if (assembly)
            {
                sb.Append(", ");
                sb.Append(reference.Module.Assembly.FullName);
            }
            return sb.ToString();
        }

    }
}
