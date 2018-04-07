using System;
using ILInterpreter.Interpreter;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType : ILType
    {

        private sealed class RuntimeByRefType : RuntimeType
        {

            public RuntimeByRefType(ILType elementType, ByReferenceType reference, ILEnvironment env) : base(reference, env)
            {
                this.elementType = elementType;
                typeForClr = elementType.TypeForCLR.MakeByRefType();
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

            private readonly Type typeForClr;

            public override Type TypeForCLR
            {
                get { return typeForClr; }
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

            public override bool IsByRef
            {
                get { return true; }
            }

            internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
            {
                throw new NotImplementedException();
            }

            public override bool IsGenericTypeDefinition
            {
                get { throw new NotImplementedException(); }
            }

            public override bool IsGenericParameter
            {
                get { throw new NotImplementedException(); }
            }

            public override bool IsGenericType
            {
                get { throw new NotImplementedException(); }
            }

            public override ILType GenericTypeDefinition
            {
                get { throw new NotImplementedException(); }
            }

            public override IListView<ILType> GenericArguments
            {
                get { throw new NotImplementedException(); }
            }

            public override int GenericParameterPosition
            {
                get { throw new NotImplementedException(); }
            }
        }

    }
}
