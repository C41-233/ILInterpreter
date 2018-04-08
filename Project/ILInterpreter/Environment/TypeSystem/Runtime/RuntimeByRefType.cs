using System;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeByRefType : RuntimeType
        {

            public RuntimeByRefType(ILType elementType, ByReferenceType reference, ILEnvironment env) : base(reference, env)
            {
                this.elementType = elementType;
                typeForCLR = elementType.TypeForCLR.MakeByRefType();
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

            private readonly Type typeForCLR;
            public override Type TypeForCLR
            {
                get { return typeForCLR; }
            }

            public override string FullName
            {
                get { return elementType.FullName + "&"; }
            }

            public override string FullQulifiedName
            {
                get { return elementType.FullQulifiedName + "&"; }
            }

            public override ILType BaseType
            {
                get { return null; }
            }

            public override bool IsAbstract
            {
                get { return false; }
            }

            public override bool IsSealed
            {
                get { return false; }
            }

            public override bool IsByRef
            {
                get { return true; }
            }

        }

    }
}
