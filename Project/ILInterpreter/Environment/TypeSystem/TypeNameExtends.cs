using System;
using System.Text;
using ILInterpreter.Environment.TypeSystem.CLR;
using ILInterpreter.Environment.TypeSystem.Runtime;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem
{
    internal static class TypeNameExtends
    {

        public static string Parse(ILType type, bool isFullQualifiedName)
        {
            return Parse(type, false, isFullQualifiedName);
        }

        private static string Parse(ILType type, bool partialAssembly, bool assembly)
        {
            if (type is CLRType)
            {
                return Parse(type, partialAssembly, assembly, () => type.TypeForCLR.FullName);
            }
            var runtimeType = type as RuntimeType;
            if (runtimeType != null)
            {
                return Parse(runtimeType, partialAssembly, assembly, () =>
                {
                    string fullname = runtimeType.TypeReference.FullName;
                    //嵌套类型
                    fullname = fullname.Replace('/', '+');
                    return fullname;
                });
            }
            throw new NotSupportedException();
        }

        private static string Parse(ILType type, bool partialAssembly, bool assembly, Func<string> getAtomicName)
        {
            var sb = new StringBuilder();
            if (type.HasElementType)
            {
                sb.Append(Parse(type.ElementType, false, assembly));
            }
            if (type.IsByRef)
            {
                sb.Append('&');
            }
            else if (type.IsPointer)
            {
                sb.Append('*');
            }
            else if (type.IsArray)
            {
                var rank = type.ArrayRank;
                sb.Append('[');
                for (var i = 0; i < rank - 1; i++)
                {
                    sb.Append(',');
                }
                sb.Append(']');
            }
            else if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                sb.Append(Parse(type.GenericTypeDefinition, false, assembly));
                sb.Append(type.GenericArguments.ToJoinString("[", "]", ",", (appender, t) =>
                {
                    if (assembly)
                    {
                        appender.Append('[');
                    }
                    appender.Append(Parse(t, assembly, assembly));
                    if (assembly)
                    {
                        appender.Append(']');
                    }
                }));
            }
            else
            {
                sb.Append(getAtomicName());
            }

            if (partialAssembly)
            {
                sb.Append(", ").Append(type.AssemblyName);
            }
            return sb.ToString();
        }

        public static string ParseAssemblyQualifiedName(TypeReference type)
        {
            return Parse(type, true);
        }

        private static string Parse(TypeReference type, bool partialAssembly)
        {
            var sb = new StringBuilder();
            var arrayType = type as ArrayType;
            if (arrayType != null)
            {
                sb.Append(Parse(arrayType.ElementType, false));
                sb.Append('[');
                for (var i=0; i<arrayType.Rank-1; i++)
                {
                    sb.Append(',');
                }
                sb.Append(']');
            }

            var byRefType = type as ByReferenceType;
            if (byRefType != null)
            {
                sb.Append(Parse(byRefType.ElementType, false));
                sb.Append('&');
            }

            var pointerType = type as PointerType;
            if (pointerType != null)
            {
                sb.Append(Parse(pointerType.ElementType, false));
                sb.Append('*');
            }

            var definitionType = type as TypeDefinition;
            if (definitionType != null)
            {
                sb.Append(definitionType.FullName);
            }

            if (partialAssembly)
            {
                sb.Append(", ").Append(type.Module.Assembly);
            }
            return sb.ToString();
        }

    }
}
