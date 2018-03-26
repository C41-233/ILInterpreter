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

        public abstract AssemblyName AssemblyName { get; }

        private string fullName;
        public string FullName
        {
            get { return fullName ?? (fullName = TypeName.Parse(this, false)); }
        }

        private string assemblyQualifiedName;

        public string AssemblyQualifiedName
        {
            get { return assemblyQualifiedName ?? (assemblyQualifiedName = TypeName.Parse(this, true));}
        }

        public abstract ILType ElementType { get; }

        public bool HasElementType
        {
            get { return ElementType != null; }
        }

        public abstract bool IsByRef { get; }

        public abstract bool IsPointer { get; }

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

    }
}
