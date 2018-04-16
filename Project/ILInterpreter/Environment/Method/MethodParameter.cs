using ILInterpreter.Environment.TypeSystem;

namespace ILInterpreter.Environment.Method
{
    public sealed class MethodParameter
    {

        internal MethodParameter(ILType type)
        {
            ParameterType = type;
        }

        public ILType ParameterType { get; private set; }

    }
}
