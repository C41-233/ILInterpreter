using System;
using System.Collections.Generic;
using System.Text;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{
    public abstract class ILType
    {

        public int Id { get; private set; }

        public ILEnvironment Environment { get; private set; }

        public abstract Type TypeForCLR { get; }

        internal ILType(ILEnvironment env)
        {
            Id = IdConstant.NextTypeId();
            Environment = env;
        }

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return FullName;
        }

        #region Name
        public abstract AssemblyName AssemblyName { get; }

        private string fullName;
        public string FullName
        {
            get { return fullName ?? (fullName = TypeName.Parse(this, false)); }
        }

        private string fullQualifiedName;
        public string FullQulifiedName
        {
            get { return fullQualifiedName ?? (fullQualifiedName = TypeName.Parse(this, true)); }
        }

        public string AssemblyQualifiedName
        {
            get { return FullQulifiedName + ", " + AssemblyName; }
        }
        #endregion

        public abstract ILType ElementType { get; }

        public bool HasElementType
        {
            get { return ElementType != null; }
        }

        #region Ref
        private ILType byRefType;
        public ILType MakeByRefType()
        {
            lock (Environment)
            {
                if (byRefType != null)
                {
                    return byRefType;
                }
            }
            return Environment.GetType(FullQulifiedName + "&, " + AssemblyName);
        }
        internal ILType CreateByRefType()
        {
            var type = CreateByRefTypeInternal();
            byRefType = type;
            return type;
        }
        internal abstract ILType CreateByRefTypeInternal();
        public abstract bool IsByRef { get; }
        #endregion

        #region Pointer
        private ILType pointerType;
        public ILType MakePointerType()
        {
            lock (Environment)
            {
                if (pointerType != null)
                {
                    return pointerType;
                }
            }
            return Environment.GetType(FullQulifiedName + "*, " + AssemblyName);
        }

        internal ILType CreatePointerType()
        {
            var type = CreatePointerTypeInternal();
            pointerType = type;
            return type;
        }

        internal abstract ILType CreatePointerTypeInternal();
        public abstract bool IsPointer { get; }
        #endregion

        #region Array
        private Dictionary<int, ILType> arrayTypes;
        public bool IsArray
        {
            get { return ArrayRank > 0; }
        }

        public abstract int ArrayRank { get; }

        internal ILType CreateArrayType(int rank)
        {
            var type = CreateArrayTypeInternal(rank);
            if (arrayTypes == null)
            {
                arrayTypes = new Dictionary<int, ILType>(1);
            }
            arrayTypes[rank] = type;
            return type;
        }

        internal abstract ILType CreateArrayTypeInternal(int rank);

        public ILType MakeArrayType()
        {
            return MakeArrayType(1);
        }

        public ILType MakeArrayType(int rank)
        {
            if (rank <= 0)
            {
                throw new ArgumentException();
            }
            lock (Environment)
            {
                ILType type;
                if (arrayTypes != null && arrayTypes.TryGetValue(rank, out type))
                {
                    return type;
                }
            }
            var sb = new StringBuilder();
            sb.Append(FullQulifiedName);
            sb.Append('[');
            for (var i = 0; i < rank - 1; i++)
            {
                sb.Append(',');
            }
            sb.Append("], ");
            sb.Append(AssemblyName);
            return Environment.GetType(sb.ToString());
        }
        #endregion

        #region Generic
        private FastList<ILType> genericTypes;

        internal abstract ILType CreateGenericTypeInternal(FastList<ILType> genericArugments);

        internal ILType CreateGenericType(FastList<ILType> genericArguments)
        {
            var type = CreateGenericTypeInternal(genericArguments);

            if (genericTypes == null)
            {
                genericTypes = new FastList<ILType>();
            }
            genericTypes.Add(type);
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
                        if (EnumerableSupport.Equals(type.genericArguments, types))
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

        public abstract bool IsGenericTypeDefinition { get; }

        public bool IsGenericType
        {
            get { return genericArguments != null; }
        }

        public abstract ILType GenericTypeDefinition { get; }

        internal FastList<ILType> genericArguments;

        public IListView<ILType> GenericArguments
        {
            get { return genericArguments; }
        }

        #endregion

    }
}
