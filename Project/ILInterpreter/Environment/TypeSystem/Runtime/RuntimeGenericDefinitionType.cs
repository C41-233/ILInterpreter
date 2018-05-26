using System;
using ILInterpreter.Interpreter;
using ILInterpreter.Interpreter.Type;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeGenericDefinitionType : RuntimeDefinitonType
        {

            public RuntimeGenericDefinitionType(TypeDefinition definition, ILEnvironment env) : base(definition, env)
            {
            }

            public override Type TypeForCLR
            {
                get { return typeof(RuntimeTypeInstance); }
            }

            public override string FullName
            {
                get { return definition.FullName; }
            }

            public override string FullQulifiedName
            {
                get { return definition.FullName + ", " + AssemblyName; }
            }
        }

    }
}
