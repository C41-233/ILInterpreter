using System.Reflection;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.CLR;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.Method.CLR
{
    internal sealed class CLRMethod : ILMethod
    {

        private readonly MethodInfo methodInfo;

        public CLRMethod(MethodInfo method, CLRType type)
        {
            methodInfo = method;
            declaredType = type;
        }

        private readonly CLRType declaredType;

        public override ILType DeclaringType
        {
            get { return declaredType; }
        }

        public override string Name
        {
            get { return methodInfo.Name; }
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

                var parameterInfos = methodInfo.GetParameters();
                parameters = new FastList<MethodParameter>(parameterInfos.Length);
                foreach (var parameterInfo in parameterInfos)
                {
                    parameters.Add(new MethodParameter(
                        Environment.GetType(parameterInfo.ParameterType)    
                    ));
                }

                returnType = Environment.GetType(methodInfo.ReturnType);

                isDefinitionInit = true;
            }
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
            if (parameterTypes == null && parameters.Count != 0)
            {
                return false;
            }
            if (parameterTypes.Length != parameters.Count)
            {
                return false;
            }

            for (var i=0; i<parameterTypes.Length; i++)
            {
                if (parameterTypes[i] != parameters[i].ParameterType)
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

        public override object Invoke(object instance, params object[] parameters)
        {
            return methodInfo.Invoke(instance, parameters);
        }
    }
}
