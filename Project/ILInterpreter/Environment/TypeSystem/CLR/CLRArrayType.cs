using System;
using System.Text;

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

            private string fullName;

            public override string FullName
            {
                get
                {
                    if (fullName == null)
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

        }

    }
}
