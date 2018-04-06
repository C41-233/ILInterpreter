using System;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRGenericParameterType : CLRType
        {

            public CLRGenericParameterType(Type type, ILEnvironment env) : base(type, env)
            {
            }

            public override bool IsGenericParameter
            {
                get { return true; }
            }
        }
    }
}
