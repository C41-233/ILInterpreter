using System;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRByRefType : CLRType
        {

            public CLRByRefType(ILType elementType, Type type, ILEnvironment env) : base(type, env)
            {
                this.elementType = elementType;
            }

            private readonly ILType elementType;

            public override ILType ElementType
            {
                get { return elementType; }
            }

            public override bool HasElementType
            {
                get { return true; }
            }

            public override bool IsByRef
            {
                get { return true; }
            }

            public override string FullName
            {
                get { return elementType.FullName + "&"; }
            }

            public override string FullQulifiedName
            {
                get { return elementType.FullQulifiedName + "&"; }
            }

        }

    }
}
