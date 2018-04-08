using System;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRDirectType : CLRType
        {

            public CLRDirectType(Type type, ILEnvironment env) : base(type, env)
            {
            }

            public override string FullName
            {
                get { return typeForCLR.FullName; }
            }

            public override string FullQulifiedName
            {
                get { return typeForCLR.FullName; }
            }
        }
    }
}
