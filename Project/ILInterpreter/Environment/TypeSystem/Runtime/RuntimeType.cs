using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType : ILType
    {

        protected readonly TypeReference reference; 

        internal RuntimeType(TypeReference reference, ILEnvironment env) : base(env)
        {
            this.reference = reference;
            assemblyName = new AssemblyName(reference.Module.Assembly.FullName);
        }

        private readonly AssemblyName assemblyName;

        public sealed override AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        public sealed override string Namespace
        {
            get { return reference.Namespace; }
        }

        public sealed override string Name
        {
            get { return reference.Name; }
        }

        public override ILType ElementType
        {
            get { return null; }
        }

        public override bool HasElementType
        {
            get { return false; }
        }

        public override ILType DeclaringType
        {
            get { return null; }
        }

        public override bool IsByRef
        {
            get { return false; }
        }

        public override bool IsPointer
        {
            get { return false; }
        }

        public override bool IsArray
        {
            get { return false; }
        }

        public override int ArrayRank
        {
            get { return 0; }
        }

        internal sealed override ILType CreateByRefTypeInternal()
        {
            var byRef = new ByReferenceType(reference);
            return new RuntimeByRefType(this, byRef, Environment);
        }

        internal sealed override ILType CreatePointerTypeInternal()
        {
            var pointer = new PointerType(reference);
            return new RuntimePointerType(this, pointer, Environment);
        }

        internal sealed override ILType CreateArrayTypeInternal(int rank)
        {
            var array = new ArrayType(reference, rank);
            return new RuntimeArrayType(this, array, Environment);
        }


        internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
        {
            return null;
        }

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
            get { return -1; }
        }

        internal static RuntimeType Create(TypeDefinition definition, ILEnvironment env)
        {
            if (definition.IsGenericParameter)
            {
                return new RuntimeTypeGenericParameterType(definition, env);
            }
            return new RuntimeDirectType(definition, env);
        }

    }

}
