using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private abstract class RuntimeDefinitonType : RuntimeType
        {

            protected readonly TypeDefinition definition;

            protected RuntimeDefinitonType(TypeDefinition definition, ILEnvironment env) : base(definition, env)
            {
                this.definition = definition;
            }

            public sealed override bool IsAbstract
            {
                get { return definition.IsAbstract; }
            }

            public sealed override bool IsSealed
            {
                get { return definition.IsSealed; }
            }

            private ILType baseType;

            public sealed override ILType BaseType
            {
                get
                {
                    if (baseType == null)
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
                    if (baseType != null)
                    {
                        return;
                    }
                    baseType = Environment.GetType(definition.BaseType.GetTypeAssemblyQualifiedName());
                }
            }
        }
    }
}
