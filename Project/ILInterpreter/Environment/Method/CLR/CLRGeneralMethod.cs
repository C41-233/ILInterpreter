using ILInterpreter.Environment.TypeSystem;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method.CLR
{
    internal abstract class CLRGeneralMethod : ILMethod
    {

        public sealed override bool Matches(ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
        {
            var parameters = Parameters;
            if (parameterTypes == null && parameters.Count != 0)
            {
                return false;
            }
            if (parameterTypes.Length != parameters.Count)
            {
                return false;
            }

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterTypes[i] != parameters[i].ParameterType)
                {
                    return false;
                }
            }

            if (ReturnType != returnType)
            {
                return false;
            }

            return true;
        }

        internal sealed override bool Matches(MethodReference reference)
        {
            if (ReturnType != Environment.GetType(reference.ReturnType))
            {
                return false;
            }
            var referenceParameters = reference.Parameters;
            if (ParametersCount != referenceParameters.Count)
            {
                return false;
            }
            for (var i = 0; i < referenceParameters.Count; i++)
            {
                if (Parameters[i].ParameterType != Environment.GetType(referenceParameters[i].ParameterType))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
