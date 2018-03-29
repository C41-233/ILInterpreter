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
                self.GenericArgumentsList = new FastList<ILType>();
                foreach (var generic in type.GetGenericArguments())
                {
                    self.GenericArgumentsList.Add(env.GetType(generic));
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

        private ILType elementType;
        public override ILType ElementType
        {
            get { return elementType; }
        }

        #region Ref
        private ILType byRefType;
        internal override ILType CreateByRefType()
        {
            var type = clrType.MakeByRefType();
            var self = new CLRType(type, Environment)
            {
                elementType = this,
            };
            byRefType = self;
            return self;
        }

        public override ILType MakeByRefType()
        {
            return byRefType ?? base.MakeByRefType();
        }

        public override bool IsByRef
        {
            get { return clrType.IsByRef; }
        }
        #endregion

        #region Pointer
        private ILType pointerType;
        internal override ILType CreatePointerType()
        {
            var type = clrType.MakePointerType();
            var self = new CLRType(type, Environment)
            {
                elementType = this
            };
            pointerType = self;
            return self;
        }

        public override ILType MakePointerType()
        {
            return pointerType ?? base.MakePointerType();
        }

        public override bool IsPointer
        {
            get { return clrType.IsPointer; }
        }
        #endregion

        private int arrayRank;
        public override int ArrayRank
        {
            get { return arrayRank; }
        }

        public override bool IsGenericTypeDefinition
        {
            get { return clrType.IsGenericTypeDefinition; }
        }

        private ILType genericTypeDefinition;

        public override ILType GenericTypeDefinition
        {
            get { return genericTypeDefinition; }
        }
    }
}
