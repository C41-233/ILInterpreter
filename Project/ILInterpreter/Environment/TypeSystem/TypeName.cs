﻿using System;
using System.Text;

namespace ILInterpreter.Environment.TypeSystem
{
    internal static class TypeName
    {

        public static string Parse(ILType type, bool assembly)
        {
            return Parse(type, assembly, assembly);
        }

        private static string Parse(ILType type, bool partialAssembly, bool assembly)
        {
            if (type is CLRType)
            {
                return Parse(type, partialAssembly, assembly, () => type.TypeForCLR.FullName);
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
                sb.Append('[');
                foreach (var generic in type.GenericArgumentsList)
                {
                    if (assembly)
                    {
                        sb.Append('[');
                    }
                    sb.Append(Parse(generic, assembly, assembly));
                    if (assembly)
                    {
                        sb.Append(']');
                    }
                }
                sb.Append(']');
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

    }
}