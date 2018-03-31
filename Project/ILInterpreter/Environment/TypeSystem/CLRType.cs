using System;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{
    public sealed class CLRType : ILType
    {

        private CLRType(Type type, ILEnvironment env) : base(env)
        {
            clrType = type;
            assemblyName = new AssemblyName(type.Assembly.GetName());
        }

        internal static CLRType Create(Type type, ILEnvironment env)
        {
            var self = new CLRType(type, env);
            if (type.HasElementType)
            {
                self.elementType = env.GetType(type.GetElementType());
            }

            self.arrayRank = type.IsArray ? type.GetArrayRank() : 0;

            if (type.IsGenericType)
            {
                if (!type.IsGenericTypeDefinition)
                {
                    self.genericTypeDefinition = env.GetType(type.GetGenericTypeDefinition());
                }
                self.genericArguments = new FastList<ILType>();
                foreach (var generic in type.GetGenericArguments())
                {
                    self.genericArguments.Add(env.GetType(generic));
                }
            }
            return self;
        }

        private readonly Type clrType;
        public override Type TypeForCLR
        {
            get { return clrType; }
        }

        private readonly AssemblyName assemblyName;
        public override AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        public override string Name
        {
            get { return clrType.Name; }
        }

        private ILType elementType;

        public override ILType ElementType
        {
            get { return elementType; }
        }

        #region Ref
        internal override ILType CreateByRefTypeInternal()
        {
            var type = clrType.MakeByRefType();
            var self = new CLRType(type, Environment)
            {
                elementType = this,
            };
            return self;
        }

        public override bool IsByRef
        {
            get { return clrType.IsByRef; }
        }
        #endregion

        #region Pointer
        internal override ILType CreatePointerTypeInternal()
        {
            var type = clrType.MakePointerType();
            var self = new CLRType(type, Environment)
            {
                elementType = this
            };
            return self;
        }

        public override bool IsPointer
        {
            get { return clrType.IsPointer; }
        }
        #endregion

        #region Array
        private int arrayRank;
        public override int ArrayRank
        {
            get { return arrayRank; }
        }
        internal override ILType CreateArrayTypeInternal(int rank)
        {
            var type = rank == 1 ? clrType.MakeArrayType() : clrType.MakeArrayType(rank);
            var self = new CLRType(type, Environment)
            {
                elementType = this,
                arrayRank = rank,
            };
            return self;
        }
        #endregion

        #region Generic

        public override bool IsGenericTypeDefinition
        {
            get { return clrType.IsGenericTypeDefinition; }
        }

        public override bool IsGenericParameter
        {
            get { return clrType.IsGenericParameter; }
        }

        private ILType genericTypeDefinition;

        public override ILType GenericTypeDefinition
        {
            get { return genericTypeDefinition; }
        }

        internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
        {
            //todo 处理RuntimeType的情形
            var genericCLRTypes = new Type[genericArugments.Count];
            for (var i = 0; i < genericArugments.Count; i++)
            {
                var generic = genericArugments[i];
                var genericCLRType = generic as CLRType;
                if (genericCLRType != null)
                {
                    genericCLRTypes[i] = genericCLRType.clrType;
                    continue;
                }
                throw new NotSupportedException();
            }
            var type = clrType.MakeGenericType(genericCLRTypes);
            var self = new CLRType(type, Environment)
            {
                genericTypeDefinition = this,
                genericArguments = genericArugments,
            };
            return self;
        }

        #endregion
    }
}
