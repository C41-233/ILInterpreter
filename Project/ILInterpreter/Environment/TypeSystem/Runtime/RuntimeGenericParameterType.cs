using System;
using ILInterpreter.Interpreter;
using ILInterpreter.Interpreter.Type;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeGenericParameterType : RuntimeDefinitonType
        {

            public RuntimeGenericParameterType(TypeDefinition definition, ILEnvironment env) : base(definition, env)
            {
            }

            public override Type TypeForCLR
            {
                get { return typeof(RuntimeTypeInstance); }
            }

            public override string FullName
            {
                get { return DeclaringType.FullName + "!" + GenericParameterPosition; }
            }

            public override string FullQulifiedName
            {
                get { return DeclaringType.FullQulifiedName + "!" + GenericParameterPosition; }
            }

            public override bool IsGenericParameter
            {
                get { return true; }
            }

            private ILType declaringType;
            public override ILType DeclaringType
            {
                get
                {
                    if (declaringType == null)
                    {
                        InitDeclaringType();
                    }
                    return declaringType;
                }
            }

            private void InitDeclaringType()
            {
                lock (Environment)
                {
                    if (declaringType != null)
                    {
                        return;
                    }
                    declaringType = Environment.GetType(definition.DeclaringType);
                }
            }

        }

    }
}
