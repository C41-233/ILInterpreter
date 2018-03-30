using System;
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
            get { return FullQulifiedName + AssemblyName; }
        }
        #endregion

        public abstract ILType ElementType { get; }

        public bool HasElementType
        {
            get { return ElementType != null; }
        }

        #region Ref
        public virtual ILType MakeByRefType()
        {
            return Environment.GetType(FullQulifiedName + "&");
        }
        internal abstract ILType CreateByRefType();
        public abstract bool IsByRef { get; }
        #endregion

        #region Pointer
        public virtual ILType MakePointerType()
        {
            return Environment.GetType(FullQulifiedName + "*");
        }
        internal abstract ILType CreatePointerType();
        public abstract bool IsPointer { get; }
        #endregion

        public bool IsArray
        {
            get { return ArrayRank > 0; }
        }

        public abstract int ArrayRank { get; }

        public abstract bool IsGenericTypeDefinition { get; }

        public bool IsGenericType
        {
            get { return GenericArgumentsList != null; }
        }

        public abstract ILType GenericTypeDefinition { get; }

        internal FastList<ILType> GenericArgumentsList;
        public ILType[] GenericArguments
        {
            get { return GenericArgumentsList != null ? GenericArgumentsList.ToArray() : null; }
        }

        internal abstract ILType CreateArrayType(int rank);
    }
}
