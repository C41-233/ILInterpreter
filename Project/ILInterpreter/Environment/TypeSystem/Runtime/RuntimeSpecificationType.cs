using ILInterpreter.Environment.Method;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private abstract class RuntimeSpecificationType : RuntimeType
        {

            protected readonly ILType elementType;

            protected RuntimeSpecificationType(ILType elementType, TypeReference reference, ILEnvironment env) : base(reference, env)
            {
                this.elementType = elementType;
            }

            public sealed override bool HasElementType
            {
                get { return true; }
            }

            public sealed override ILType ElementType
            {
                get { return elementType; }
            }

            public sealed override bool IsAbstract
            {
                get { return false; }
            }

            public sealed override bool IsNestedPublic
            {
                get { return false; }
            }

            public override ILMethod GetDeclaredMethod(string name, ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
            {
                return null;
            }

        }
    }
}
