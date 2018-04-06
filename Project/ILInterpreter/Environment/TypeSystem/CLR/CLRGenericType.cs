using System;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRGenericType : CLRType
        {

            public CLRGenericType(ILType genericTypeDefinition, FastList<ILType> genericArguments, Type type, ILEnvironment env) : base(type, env)
            {
                this.genericTypeDefinition = genericTypeDefinition;
                this.genericArguments = genericArguments;
            }

            public override bool IsGenericTypeDefinition
            {
                get { return clrType.IsGenericTypeDefinition; }
            }

            public override bool IsGenericType
            {
                get { return true; }
            }

            private readonly ILType genericTypeDefinition;
            private readonly FastList<ILType> genericArguments;

            public override ILType GenericTypeDefinition
            {
                get { return genericTypeDefinition; }
            }

            public override IListView<ILType> GenericArguments
            {
                get { return genericArguments; }
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
                        genericCLRTypes[i] = genericCLRType.clrType;
                        continue;
                    }
                    throw new NotSupportedException();
                }
                var type = clrType.MakeGenericType(genericCLRTypes);
                var self = new CLRGenericType(this, genericArugments, type, Environment);
                return self;
            }
        }

    }
}
