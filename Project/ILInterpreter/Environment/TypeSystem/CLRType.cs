using System;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{
    public sealed class CLRType : ILType
    {

        internal CLRType(Type type, ILEnvironment env) : base(env)
        {
            clrType = type;
            assemblyName = new AssemblyName(type.Assembly.GetName());

            if (type.HasElementType)
            {
                elementType = env.GetType(type.GetElementType());
            }

            arrayRank = type.IsArray ? type.GetArrayRank() : 0;

            if (type.IsGenericType)
            {
                if (!type.IsGenericTypeDefinition)
                {
                    genericTypeDefinition = env.GetType(type.GetGenericTypeDefinition());
                }
                GenericArgumentsList = new FastList<ILType>();
                foreach (var generic in type.GetGenericArguments())
                {
                    GenericArgumentsList.Add(env.GetType(generic));
                }
            }
        }

        private readonly Type clrType;
        public override Type TypeForCLR
        {
            get { return clrType; }
        }

        private readonly AssemblyName assemblyName;
        public override AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        private readonly ILType elementType;
        public override ILType ElementType
        {
            get { return elementType; }
        }

        public override bool IsByRef
        {
            get { return clrType.IsByRef; }
        }

        public override bool IsPointer
        {
            get { return clrType.IsPointer; }
        }

        private readonly int arrayRank;
        public override int ArrayRank
        {
            get { return arrayRank; }
        }

        public override bool IsGenericTypeDefinition
        {
            get { return clrType.IsGenericTypeDefinition; }
        }

        private readonly ILType genericTypeDefinition;

        public override ILType GenericTypeDefinition
        {
            get { return genericTypeDefinition; }
        }
    }
}
