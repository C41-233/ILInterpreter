using System.Collections.Generic;
using ILInterpreter.Environment.Method;
using ILInterpreter.Environment.Method.Runtime;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private abstract class RuntimeDefinitonType : RuntimeType
        {

            protected readonly TypeDefinition definition;

            protected RuntimeDefinitonType(TypeDefinition definition, ILEnvironment env) : base(definition, env)
            {
                this.definition = definition;
            }

            public sealed override bool IsAbstract
            {
                get { return definition.IsAbstract; }
            }

            public sealed override bool IsSealed
            {
                get { return definition.IsSealed; }
            }

            public override bool IsPublic
            {
                get { return definition.IsPublic; }
            }

            public override bool IsNestedPublic
            {
                get { return definition.IsNestedPublic; }
            }

            private ILType baseType;

            public sealed override ILType BaseType
            {
                get
                {
                    if (baseType == null)
                    {
                        InitBaseType();
                    }
                    return baseType;
                }
            }

            private void InitBaseType()
            {
                lock (Environment)
                {
                    if (baseType != null)
                    {
                        return;
                    }
                    baseType = Environment.GetType(definition.BaseType);
                }
            }

            #region Methods

            private Dictionary<string, FastList<RuntimeMethod>> _methods;

            private Dictionary<string, FastList<RuntimeMethod>> Methods
            {
                get
                {
                    if (_methods == null)
                    {
                        InitMethods();        
                    }
                    return _methods;
                }
            }

            private void InitMethods()
            {
                lock (Environment)
                {
                    _methods = new Dictionary<string, FastList<RuntimeMethod>>();
                    foreach (var method in definition.Methods)
                    {
                        FastList<RuntimeMethod> list;
                        if (!_methods.TryGetValue(method.Name, out list))
                        {
                            list = new FastList<RuntimeMethod>();
                            _methods.Add(method.Name, list);
                        }
                        var runtimeMethod = new RuntimeMethod(method, this);
                        list.Add(runtimeMethod);
                    }

                    foreach (var list in _methods.Values)
                    {
                        list.Trim();
                    }
                }
            }

            #endregion

            public override ILMethod GetDeclaredMethod(string name, ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
            {
                FastList<RuntimeMethod> list;
                if (!Methods.TryGetValue(name, out list))
                {
                    return null;
                }
                foreach (var method in list)
                {
                    if (method.Matches(genericArguments, parameterTypes, returnType))
                    {
                        return method;
                    }
                }
                return null;
            }

        }
    }
}
