using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
