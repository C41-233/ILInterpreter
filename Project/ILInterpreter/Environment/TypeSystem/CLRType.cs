using System;
using System.Linq;

namespace ILInterpreter.Environment.TypeSystem
{
    public sealed class CLRType : ILType
    {

        private string symbol;
        public override string Symbol
        {
            get
            {
                if (symbol == null)
                {
                    symbol = clrType.FullName;
                }
                return symbol;
            }
        }

        public override Type Type
        {
            get { return clrType; }
        }

        private readonly Type clrType;

        public CLRType(Type type, ILEnvironment environment) : base(environment)
        {
            clrType = type;
        }

        internal override ILType CreatePointerType()
        {
            return new CLRType(clrType.MakePointerType(), Environment)
            {
                ElementType = this
            };
        }

        internal override ILType CreateRefType()
        {
            return new CLRType(clrType.MakeByRefType(), Environment)
            {
                ElementType = this
            };
        }

        internal override ILType CreateArrayType(int rank)
        {
            var clrArrayType = rank == 1 ? clrType.MakeArrayType() : clrType.MakeArrayType(rank);
            return new CLRType(clrArrayType, Environment)
            {
                ElementType = this
            };
        }

        internal override ILType CreateGenericInstance(params ILType[] generics)
        {
            var type = clrType.MakeGenericType(generics.Select(t => t.Type).ToArray());
            return new CLRType(type, Environment);
        }
    }

}
