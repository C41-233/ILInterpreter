using System;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Runtime;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method.Runtime
{
    internal sealed partial class RuntimeMethod : ILMethod
    {

        private readonly RuntimeType type;
        private readonly MethodDefinition definition;

        public RuntimeMethod(MethodDefinition definition, RuntimeType type)
        {
            this.type = type;
            this.definition = definition;
        }

        private FastList<MethodParameter> parameters;
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

                var monoParameters = definition.Parameters;
                parameters = new FastList<MethodParameter>(monoParameters.Count);
                foreach (var monoParameter in monoParameters)
                {
                    var parameter = new MethodParameter(
                        Environment.GetType(monoParameter.ParameterType)
                    );
                    parameters.Add(parameter);
                }

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

        public override IListView<MethodParameter> Parameters
        {
            get
            {
                CheckDefinitionInit();
                return parameters;
            }
        }

        public override ILType ReturnType
        {
            get
            {
                CheckDefinitionInit();
                return returnType;
            }
        }

        public override bool Matches(ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
        {
            CheckDefinitionInit();
            if (genericArguments == null)
            {
                genericArguments = Array<ILType>.Empty;
            }
            if (parameterTypes == null)
            {
                parameterTypes = Array<ILType>.Empty;
            }
            if (returnType == null)
            {
                returnType = Environment.Void;
            }

            if (parameters.Count != parameterTypes.Length)
            {
                return false;
            }
            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i])
                {
                    return false;
                }
            }

            if (this.returnType != returnType)
            {
                return false;
            }
            return true;
        }

        internal override bool Matches(MethodReference reference)
        {
            return definition == reference;
        }

        public override object Invoke(object instance, params object[] parameters)
        {
            return Environment.Invoke(this, instance, parameters);
        }
    }
}
