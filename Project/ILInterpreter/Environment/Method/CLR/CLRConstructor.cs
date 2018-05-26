using System.Reflection;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.CLR;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.Method.CLR
{
    internal sealed class CLRConstructor : CLRGeneralMethod
    {

        private readonly ConstructorInfo constructor;

        public CLRConstructor(ConstructorInfo constructor, CLRType type)
        {
            this.constructor = constructor;
            declaringType = type;
        }

        private readonly CLRType declaringType;

        public override ILType DeclaringType
        {
            get { return declaringType; }
        }

        public override string Name
        {
            get { return ConstructorName; }
        }

        public override bool HasThis
        {
            get { return true; }
        }

        public override bool IsMethod
        {
            get { return false; }
        }

        public override bool IsConstructor
        {
            get { return true; }
        }

        public override bool IsStaticConstructor
        {
            get { return false; }
        }

        private FastList<MethodParameter> parameters;

        private bool isDefinitionInit;
        
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
                return Environment.Void;
            }
        }

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

                var parameterInfos = constructor.GetParameters();
                parameters = new FastList<MethodParameter>(parameterInfos.Length);
                foreach (var parameterInfo in parameterInfos)
                {
                    parameters.Add(new MethodParameter(
                        Environment.GetType(parameterInfo.ParameterType)
                    ));
                }

                isDefinitionInit = true;
            }
        }

        public override object Invoke(object instance, params object[] parameters)
        {
            return constructor.Invoke(parameters);
        }
    }
}
