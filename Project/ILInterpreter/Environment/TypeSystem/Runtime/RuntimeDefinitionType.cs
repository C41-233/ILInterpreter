using System.Collections.Generic;
using ILInterpreter.Environment.Method;
using ILInterpreter.Environment.Method.CLR;
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

            private Dictionary<string, FastList<RuntimeMethod>> nameToMethods;
            private Dictionary<int, RuntimeMethod> idToMethods;
            private bool isMethodsInit;

            private void CheckInitMethods()
            {
                if (isMethodsInit)
                {
                    return;
                }

                lock (Environment)
                {
                    if (isMethodsInit)
                    {
                        return;
                    }
                    nameToMethods = new Dictionary<string, FastList<RuntimeMethod>>();
                    idToMethods = new Dictionary<int, RuntimeMethod>();

                    foreach (var method in definition.Methods)
                    {
                        FastList<RuntimeMethod> list;
                        if (!nameToMethods.TryGetValue(method.Name, out list))
                        {
                            list = new FastList<RuntimeMethod>();
                            nameToMethods.Add(method.Name, list);
                        }
                        var runtimeMethod = new RuntimeMethod(method, this);
                        list.Add(runtimeMethod);
                        idToMethods.Add(runtimeMethod.GetHashCode(), runtimeMethod);
                    }

                    foreach (var list in nameToMethods.Values)
                    {
                        list.Trim();
                    }

                    isMethodsInit = true;
                }
            }

            #endregion

            public sealed override ILMethod GetDeclaredMethod(string name, ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
            {
                CheckInitMethods();
                FastList<RuntimeMethod> list;
                if (!nameToMethods.TryGetValue(name, out list))
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

            internal sealed override ILMethod GetDeclaredMethod(MethodReference reference)
            {
                CheckInitMethods();
                FastList<RuntimeMethod> list;
                if (!nameToMethods.TryGetValue(reference.Name, out list))
                {
                    return null;
                }
                foreach (var method in list)
                {
                    if (method.Matches(reference))
                    {
                        return method;
                    }
                }
                return null;
            }

            internal sealed override ILMethod GetDeclaredMethod(int hash)
            {
                CheckInitMethods();
                return idToMethods[hash];
            }

            internal override ILMethod GetVirtualMethod(int hash)
            {
                return GetDeclaredMethod(hash);
            }
        }
    }
}
