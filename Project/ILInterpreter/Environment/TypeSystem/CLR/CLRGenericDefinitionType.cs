using System;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRGenericDefinitionType : CLRType
        {

            public CLRGenericDefinitionType(Type type, ILEnvironment env) : base(type, env)
            {
            }

            public override bool IsGenericTypeDefinition
            {
                get { return true; }
            }

            public override bool IsGenericType
            {
                get { return true; }
            }

            private FastList<ILType> genericArguments;

            public override ILType GenericTypeDefinition
            {
                get { return this; }
            }

            public override IListView<ILType> GenericArguments
            {
                get
                {
                    if (genericArguments == null)
                    {
                        InitGenericArguments();
                    }
                    return genericArguments;
                }
            }

            private void InitGenericArguments()
            {
                lock (Environment)
                {
                    if (genericArguments != null)
                    {
                        return;
                    }

                    var clrGenericArguments = typeForCLR.GetGenericArguments();
                    genericArguments = new FastList<ILType>(clrGenericArguments.Length);
                    foreach (var arg in clrGenericArguments)
                    {
                        genericArguments.Add(Environment.GetType(arg));
                    }
                }
            }

            internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
            {
                //todo 处理RuntimeType的情形
                var genericCLRTypes = new Type[genericArugments.Count];
                for (var i = 0; i < genericArugments.Count; i++)
                {
                    var generic = genericArugments[i];
                    var genericCLRType = generic as CLRType;
                    if (genericCLRType != null)
                    {
                        genericCLRTypes[i] = genericCLRType.typeForCLR;
                        continue;
                    }
                    throw new NotSupportedException();
                }
                var type = typeForCLR.MakeGenericType(genericCLRTypes);
                var self = new CLRGenericSpecificationType(this, genericArugments, type, Environment);
                return self;
            }

            public override string FullName
            {
                get { return typeForCLR.FullName; }
            }

            public override string FullQulifiedName
            {
                get { return typeForCLR.FullName; }
            }

        }
    }
}
