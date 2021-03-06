﻿using System;
using System.Linq;
using System.Text;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{
    public abstract partial class ILType
    {

        private FastList<ILType> genericTypes;

        internal abstract ILType CreateGenericTypeInternal(FastList<ILType> genericArugments);

        internal ILType CreateGenericType(FastList<ILType> genericArguments)
        {
            var type = CreateGenericTypeInternal(genericArguments);

            lock (Environment)
            {
                if (genericTypes == null)
                {
                    genericTypes = new FastList<ILType>();
                }
                genericTypes.Add(type);
            }
            return type;
        }

        public ILType MakeGenericType(params ILType[] types)
        {
            lock (Environment)
            {
                if (genericTypes != null)
                {
                    foreach (var type in genericTypes)
                    {
                        if (CompareSupport.Equals(type.GenericArguments, types))
                        {
                            return type;
                        }
                    }
                }
            }
            var sb = new StringBuilder();
            sb.Append(FullQulifiedName);
            sb.Append('[');
            for (var i = 0; i < types.Length; i++)
            {
                sb.Append('[');
                sb.Append(types[i].AssemblyQualifiedName);
                sb.Append(']');
                if (i != types.Length - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append("], ");
            sb.Append(AssemblyName);
            return Environment.GetType(sb.ToString());
        }

        public ILType MakeGenericType(params Type[] genericArguments)
        {
            var arguments = new ILType[genericArguments.Length];
            for (var i = 0; i < genericArguments.Length; i++)
            {
                arguments[i] = Environment.GetType(genericArguments[i]);
            }
            return MakeGenericType(arguments);
        }

        public abstract bool IsGenericTypeDefinition { get; }

        public abstract bool IsGenericParameter { get; }

        public abstract bool IsGenericType { get; }

        public abstract ILType GenericTypeDefinition { get; }

        public abstract IListView<ILType> GenericArguments { get; }

        public abstract int GenericParameterPosition { get; }

    }
}
