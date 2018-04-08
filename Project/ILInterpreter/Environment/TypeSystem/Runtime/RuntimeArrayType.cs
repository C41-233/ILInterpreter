using System;
using System.Text;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeArrayType : RuntimeType
        {

            public RuntimeArrayType(ILType elementType, ArrayType reference, ILEnvironment env) : base(reference, env)
            {
                this.elementType = elementType;
                rank = reference.Rank;
                if (rank == 1)
                {
                    typeForCLR = elementType.TypeForCLR.MakeArrayType();
                }
                else
                {
                    typeForCLR = elementType.TypeForCLR.MakeArrayType(rank);
                }
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

            public override bool IsAbstract
            {
                get { return false; }
            }

            public override bool IsSealed
            {
                get { return true; }
            }

            private readonly Type typeForCLR;
            public override Type TypeForCLR
            {
                get { return typeForCLR; }
            }

            private string fullName;
            public override string FullName
            {
                get
                {
                    if (fullName ==  null)
                    {
                        var sb = new StringBuilder();
                        sb.Append(elementType.FullName);
                        sb.Append(TypeSupport.GetArrayString(rank));
                        fullName = sb.ToString();
                    }
                    return fullName;
                }
            }

            private string fullQualifiedName;
            public override string FullQulifiedName
            {
                get
                {
                    if (fullQualifiedName == null)
                    {
                        var sb = new StringBuilder();
                        sb.Append(elementType.FullQulifiedName);
                        sb.Append(TypeSupport.GetArrayString(rank));
                        fullQualifiedName = sb.ToString();
                    }
                    return fullQualifiedName;
                }
            }

            private ILType baseType;
            public override ILType BaseType
            {
                get { return baseType ?? (baseType = Environment.GetType<Array>()); }
            }

        }
    }
}
