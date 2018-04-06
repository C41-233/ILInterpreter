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
                get { return clrType.FullName; }
            }

            public override string FullQulifiedName
            {
                get { return clrType.FullName; }
            }
        }
    }
}
