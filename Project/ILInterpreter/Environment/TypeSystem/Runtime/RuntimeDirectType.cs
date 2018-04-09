using System;
using ILInterpreter.Interpreter;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeDirectType : RuntimeDefinitonType
        {

            public RuntimeDirectType(TypeDefinition definition, ILEnvironment env) : base(definition, env)
            {
            }

            public override Type TypeForCLR
            {
                get { return typeof(RuntimeTypeInstance); }
            }

            private string fullName;
            public override string FullName
            {
                get
                {
                    if (fullName == null)
                    {
                        var name = definition.FullName;
                        name = name.Replace('/', '+');
                        fullName = name;
                    }
                    return fullName;
                }
            }

            public override string FullQulifiedName
            {
                get { return FullName; }
            }

        }
    }
}
