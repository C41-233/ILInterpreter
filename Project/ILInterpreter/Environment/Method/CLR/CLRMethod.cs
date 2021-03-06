﻿using System.Reflection;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.CLR;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method.CLR
{
    internal sealed class CLRMethod : CLRGeneralMethod
    {

        private readonly MethodInfo method;

        public CLRMethod(MethodInfo method, CLRType type)
        {
            this.method = method;
            declaringType = type;
        }

        private readonly CLRType declaringType;

        public override ILType DeclaringType
        {
            get { return declaringType; }
        }

        public override string Name
        {
            get { return method.Name; }
        }

        public override bool HasThis
        {
            get { return !method.IsStatic; }
        }

        public override bool IsMethod
        {
            get { return true; }
        }

        public override bool IsConstructor
        {
            get { return false; }
        }

        public override bool IsStaticConstructor
        {
            get { return false; }
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

                var parameterInfos = method.GetParameters();
                parameters = new FastList<MethodParameter>(parameterInfos.Length);
                foreach (var parameterInfo in parameterInfos)
                {
                    parameters.Add(new MethodParameter(
                        Environment.GetType(parameterInfo.ParameterType)    
                    ));
                }

                returnType = Environment.GetType(method.ReturnType);

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

        public override object Invoke(object instance, params object[] parameters)
        {
            return method.Invoke(instance, parameters);
        }
    }
}
