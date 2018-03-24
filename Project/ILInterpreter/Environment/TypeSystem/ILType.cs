using System;

namespace ILInterpreter.Environment.TypeSystem
{
    public abstract class ILType
    {

        public int Token { get; private set; }
        public abstract string Symbol { get; }
        public abstract Type Type { get; }

        public ILEnvironment Environment { get; private set; }

        internal ILType(ILEnvironment environment)
        {
            Environment = environment;
        }

        public override string ToString()
        {
            return Symbol;
        }

        internal ILType ElementType;

        public ILType GetElementType()
        {
            return ElementType;
        }

        internal abstract ILType CreatePointerType();
        internal abstract ILType CreateRefType();
        internal abstract ILType CreateArrayType(int rank);
        internal abstract ILType CreateGenericInstance(params ILType[] generics);
    }
}
