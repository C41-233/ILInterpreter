using System;
using ILInterpreter.Interpreter;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.TypeSystem.Runtime
{
    internal abstract partial class RuntimeType
    {

        private sealed class RuntimeTypeGenericParameterType : RuntimeType
        {

            private readonly TypeDefinition definition;

            public RuntimeTypeGenericParameterType(TypeDefinition definition, ILEnvironment env) : base(definition, env)
            {
                this.definition = definition;
            }

            public override Type TypeForCLR
            {
                get { return typeof(RuntimeTypeInstance); }
            }

            public override string FullName
            {
                get { return DeclaringType.FullName + "!" + GenericParameterPosition; }
            }

            public override string FullQulifiedName
            {
                get { return DeclaringType.FullQulifiedName + "!" + GenericParameterPosition; }
            }

            private ILType baseType;

            public override ILType BaseType
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
                    baseType = Environment.GetType(definition.BaseType.GetTypeAssemblyQualifiedName());
                }
            }

            private ILType declaringType;

            public override ILType DeclaringType
            {
                get
                {
                    if (declaringType == null)
                    {
                        InitDeclaringType();
                    }
                    return declaringType;
                }
            }

            private void InitDeclaringType()
            {
                lock (Environment)
                {
                    if (declaringType != null)
                    {
                        return;
                    }
                    declaringType = Environment.GetType(definition.DeclaringType.GetTypeAssemblyQualifiedName());
                }
            }

            internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
            {
                throw new NotImplementedException();
            }

            public override bool IsGenericTypeDefinition
            {
                get { throw new NotImplementedException(); }
            }

            public override bool IsGenericParameter
            {
                get { throw new NotImplementedException(); }
            }

            public override bool IsGenericType
            {
                get { throw new NotImplementedException(); }
            }

            public override ILType GenericTypeDefinition
            {
                get { throw new NotImplementedException(); }
            }

            public override IListView<ILType> GenericArguments
            {
                get { throw new NotImplementedException(); }
            }

            public override int GenericParameterPosition
            {
                get { throw new NotImplementedException(); }
            }
        }

    }
}
