using System;
using System.Collections.Generic;
using System.Text;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{
    public abstract partial class ILType
    {

        /// <summary>
        /// 类型token，与GetHashCode返回值一致
        /// </summary>
        public int Id { get; private set; }

        public ILEnvironment Environment { get; private set; }

        /// <summary>
        /// 在解释执行时实际运行的CLR类型，这不代表该类型所表示的类型
        /// </summary>
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

        #region Name
        public sealed override string ToString()
        {
            return FullName;
        }

        public abstract AssemblyName AssemblyName { get; }

        public abstract string Namespace { get; }

        public abstract string FullName { get; }

        public abstract string FullQulifiedName { get; }

        public string AssemblyQualifiedName
        {
            get { return FullQulifiedName + ", " + AssemblyName; }
        }

        public abstract string Name { get; }
        #endregion

        public abstract ILType ElementType { get; }

        public abstract bool HasElementType { get; }

        public abstract ILType BaseType { get; }

        public abstract ILType DeclaringType { get; }

        public abstract bool IsAbstract { get; }

        public abstract bool IsSealed { get; }

        public bool IsStatic
        {
            get { return IsAbstract && IsSealed; }
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

        public abstract bool IsArray { get; }

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

        public ILType MakeArrayType(int rank = 1)
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
            sb.Append(TypeSupport.GetArrayString(rank));
            sb.Append(", ");
            sb.Append(AssemblyName);
            return Environment.GetType(sb.ToString());
        }
        #endregion

    }
}
