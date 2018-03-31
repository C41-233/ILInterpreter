using System;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem
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
            return type;
        }

        private AssemblyName assemblyName;
        public override AssemblyName AssemblyName
        {
            get { throw new NotImplementedException(); }
        }

        public override string Namespace
        {
            get { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
        {
            throw new NotImplementedException();
        }

        public override bool IsGenericTypeDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsGenericParameter
        {
            get { return reference.IsGenericParameter; }
        }

        public override ILType GenericTypeDefinition
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

        internal override ILType CreateByRefTypeInternal()
        {
            throw new NotImplementedException();
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

        private int arrayRank;
        public override int ArrayRank
        {
            get { return arrayRank; }
        }

        internal override ILType CreateArrayTypeInternal(int rank)
        {
            throw new NotImplementedException();
        }
    }
}
