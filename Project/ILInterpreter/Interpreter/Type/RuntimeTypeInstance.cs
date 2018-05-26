using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Runtime;

namespace ILInterpreter.Interpreter.Type
{
    internal class RuntimeTypeInstance
    {

        public RuntimeType RuntimeType
        {
            get
            {
                return runtimeType;
            }
        }

        private readonly RuntimeType runtimeType;

        public RuntimeTypeInstance(RuntimeType type)
        {
            runtimeType = type;
        }

    }
}
