using System;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRPointerType : CLRType
        {

            public CLRPointerType(ILType elementType, Type type, ILEnvironment env) : base(type, env)
            {
                this.elementType = elementType;
            }

            private readonly ILType elementType;

            public override bool HasElementType
            {
                get { return true; }
            }

            public override ILType ElementType
            {
                get { return elementType; }
            }

            public override bool IsPointer
            {
                get { return true; }
            }
        }

    }
}
