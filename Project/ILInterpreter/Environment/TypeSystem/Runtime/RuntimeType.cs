using System;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    public sealed class RuntimeType : ILType
    {

        internal TypeReference TypeReference
        {
            get { return reference; }
        }
        private readonly TypeReference reference;

        private RuntimeType(TypeReference reference, ILEnvironment env) : base(env)
        {
            this.reference = reference;
            assemblyName = new AssemblyName(reference.Module.Assembly.FullName);
        }

        internal static RuntimeType Create(TypeDefinition def, ILEnvironment env)
        {
            var type = new RuntimeType(def, env);
            if (def.HasGenericParameters)
            {
                type.isGenericTypeDefinition = true;
            }
            return type;
        }

        private readonly AssemblyName assemblyName;
        public override AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        public override string Namespace
        {
            get { return reference.Namespace; }
        }

        public override string FullName
        {
            get { throw new NotImplementedException(); }
        }

        public override string FullQulifiedName
        {
            get { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { return reference.Name; }
        }

        internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
        {
            throw new NotImplementedException();
        }

        private bool isGenericTypeDefinition;
        public override bool IsGenericTypeDefinition
        {
            get { return isGenericTypeDefinition; }
        }

        public override bool IsGenericParameter
        {
            get { return reference.IsGenericParameter; }
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

        public override Type TypeForCLR
        {
            get { throw new NotImplementedException(); }
        }

        private ILType elementType;
        public override ILType ElementType
        {
            get { return elementType; }
        }

        public override bool HasElementType
        {
            get { throw new NotImplementedException(); }
        }

        private bool isBaseTypeInit;
        private ILType baseType;
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

        public override ILType DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        private void InitBaseType()
        {
            lock (Environment)
            {
                if (isBaseTypeInit)
                {
                    return;
                }
                var def = reference as TypeDefinition;
                if (def != null)
                {
                    if (def.BaseType != null)
                    {
                        
                    }
                }

                var array = reference as ArrayType;
                if (array != null)
                {
                    baseType = Environment.GetType(typeof(Array));
                }
                isBaseTypeInit = true;
            }
        }

        internal override ILType CreateByRefTypeInternal()
        {
            var byRefReference = new ByReferenceType(reference);
            var type = new RuntimeType(byRefReference, Environment)
            {
                elementType = this
            };
            return type;
        }

        public override bool IsByRef
        {
            get { return reference.IsByReference; }
        }

        internal override ILType CreatePointerTypeInternal()
        {
            throw new NotImplementedException();
        }

        public override bool IsPointer
        {
            get { return reference.IsPointer; }
        }

        public override bool IsArray
        {
            get { throw new NotImplementedException(); }
        }

        private int arrayRank;
        public override int ArrayRank
        {
            get { return arrayRank; }
        }

        internal override ILType CreateArrayTypeInternal(int rank)
        {
            var arrayReference = new ArrayType(reference, rank);
            var type = new RuntimeType(arrayReference, Environment)
            {
                elementType = this,
                arrayRank = rank,
            };
            return type;
        }
    }
}
