using System;
using ILInterpreter.Environment.Method;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeByRefType : RuntimeSpecificationType
        {

            public RuntimeByRefType(ILType elementType, ByReferenceType reference, ILEnvironment env) : base(elementType, reference, env)
            {
                typeForCLR = elementType.TypeForCLR.MakeByRefType();
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

            public override bool IsSealed
            {
                get { return false; }
            }

            public override bool IsPublic
            {
                get { return false; }
            }

            public override bool IsByRef
            {
                get { return true; }
            }

            public override ILMethod GetDeclaredMethod(string name, ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
            {
                return null;
            }

        }

    }
}
