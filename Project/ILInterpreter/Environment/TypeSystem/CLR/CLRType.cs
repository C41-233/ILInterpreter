using System;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem.CLR
{

    internal abstract partial class CLRType : ILType
    {

        internal CLRType(Type type, ILEnvironment env) : base(env)
        {
            clrType = type;
            assemblyName = new AssemblyName(type.Assembly.GetName());
        }

        protected readonly Type clrType;
        public sealed override Type TypeForCLR
        {
            get { return clrType; }
        }

        private readonly AssemblyName assemblyName;
        public sealed override AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        public sealed override string Namespace
        {
            get { return clrType.Namespace; }
        }

        public sealed override string Name
        {
            get { return clrType.Name; }
        }

        public override ILType ElementType
        {
            get { return null; }
        }

        public override bool HasElementType
        {
            get { return false; }
        }

        private ILType baseType;
        private bool isBaseTypeInit;
        public override ILType BaseType
        {
            get
            {
                if (!isBaseTypeInit)
                {
                    InitBaseType();
                }
                return baseType;
            }
        }
        private void InitBaseType()
        {
            lock (Environment)
            {
                if (isBaseTypeInit)
                {
                    return;
                }
                if (clrType.BaseType != null)
                {
                    baseType = Environment.GetType(clrType.BaseType);
                }
                isBaseTypeInit = true;
            }
        }

        public override ILType DeclaringType
        {
            get { return null; }
        }

        #region Ref
        public override bool IsByRef
        {
            get { return false; }
        }

        internal sealed override ILType CreateByRefTypeInternal()
        {
            var type = clrType.MakeByRefType();
            var self = new CLRByRefType(this, type, Environment);
            return self;
        }
        #endregion

        #region Pointer
        public override bool IsPointer
        {
            get { return false; }
        }

        internal sealed override ILType CreatePointerTypeInternal()
        {
            var type = clrType.MakePointerType();
            var self = new CLRPointerType(this, type, Environment);
            return self;
        }
        #endregion

        #region Array
        public override bool IsArray
        {
            get { return false; }
        }

        public override int ArrayRank
        {
            get { return 0; }
        }

        internal sealed override ILType CreateArrayTypeInternal(int rank)
        {
            var type = rank == 1 ? clrType.MakeArrayType() : clrType.MakeArrayType(rank);
            var self = new CLRArrayType(this, type, Environment);
            return self;
        }
        #endregion

        #region Generic

        public override bool IsGenericTypeDefinition
        {
            get { return false; }
        }

        public override bool IsGenericParameter
        {
            get { return false; }
        }

        public override bool IsGenericType
        {
            get { return false; }
        }

        public override ILType GenericTypeDefinition
        {
            get { return null; }
        }

        public override IListView<ILType> GenericArguments
        {
            get { return null; }
        }

        public override int GenericParameterPosition
        {
            get { throw new InvalidOperationException(string.Format("type {0} is not GenericParameterType.", FullName)); }
        }

        internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
        {
            throw new InvalidOperationException(string.Format("type {0} is not GenericDefinitionType.", FullName));
        }
        #endregion

        internal static CLRType Create(Type type, ILEnvironment env)
        {
            if (type.IsPointer)
            {
                var elementType = env.GetType(type.GetElementType());
                return new CLRPointerType(elementType, type, env);
            }

            if (type.IsByRef)
            {
                var elementType = env.GetType(type.GetElementType());
                return new CLRByRefType(elementType, type, env);
            }

            if (type.IsArray)
            {
                var elementType = env.GetType(type.GetElementType());
                return new CLRArrayType(elementType, type, env);
            }

            if (type.IsGenericParameter)
            {
                return new CLRGenericParameterType(type, env);
            }

            if (type.IsGenericType)
            {
                var genericArguments = new FastList<ILType>();
                foreach (var generic in type.GetGenericArguments())
                {
                    genericArguments.Add(env.GetType(generic));
                }
                if (type.IsGenericTypeDefinition)
                {
                    return new CLRGenericDefinitionType(genericArguments, type, env);
                }
                var genericTypeDefinition = env.GetType(type.GetGenericTypeDefinition());
                return new CLRGenericSpecificationType(genericTypeDefinition, genericArguments, type, env);
            }

            return new CLRDirectType(type, env);
        }

    }
}
