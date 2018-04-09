using System;
using System.Text;
using ILInterpreter.Environment.Method;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeArrayType : RuntimeSpecificationType
        {

            public RuntimeArrayType(ILType elementType, ArrayType reference, ILEnvironment env) : base(elementType, reference, env)
            {
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

            public override bool IsArray
            {
                get { return true; }
            }

            private readonly int rank;

            public override int ArrayRank
            {
                get { return rank; }
            }

            public override bool IsSealed
            {
                get { return true; }
            }

            public override bool IsPublic
            {
                get { return elementType.IsPublic; }
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
