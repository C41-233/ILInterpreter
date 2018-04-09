using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Runtime;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method.Runtime
{
    internal sealed class RuntimeMethod : ILMethod
    {

        private readonly RuntimeType type;
        private readonly MethodDefinition definition;

        public RuntimeMethod(MethodDefinition definition, RuntimeType type)
        {
            this.type = type;
            this.definition = definition;
        }

        private FastList<ILType> parameters;
        private ILType returnType;

        private bool isDefinitionInit;

        private void CheckDefinitionInit()
        {
            if (isDefinitionInit)
            {
                return;
            }
            lock (Environment)
            {
                if (isDefinitionInit)
                {
                    return;
                }
                parameters = new FastList<ILType>();
                foreach (var parameter in definition.Parameters)
                {
                    parameters.Add(Environment.GetType(parameter.ParameterType));
                }
                parameters.Trim();

                returnType = Environment.GetType(definition.ReturnType);

                isDefinitionInit = true;
            }
        }

        public override ILType DeclaringType
        {
            get { return type; }
        }

        public override string Name
        {
            get { return definition.Name; }
        }

        public override bool Matches(ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
        {
            CheckDefinitionInit();
            if (!CompareSupport.Equals(parameters, parameterTypes))
            {
                return false;
            }
            if (this.returnType != returnType)
            {
                return false;
            }
            return true;
        }
    }
}
