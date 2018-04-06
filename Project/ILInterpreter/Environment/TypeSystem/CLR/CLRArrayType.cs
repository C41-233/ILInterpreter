using System;

namespace ILInterpreter.Environment.TypeSystem.CLR
{

    internal abstract partial class CLRType
    {

        private sealed class CLRArrayType : CLRType
        {

            public CLRArrayType(ILType elementType, Type type, ILEnvironment env) : base(type, env)
            {
                this.elementType = elementType;
                rank = type.GetArrayRank();
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

            public override bool IsArray
            {
                get { return true; }
            }

            private readonly int rank;

            public override int ArrayRank
            {
                get { return rank; }
            }
        }

    }
}
