﻿using System;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRTypeGenericParameterType : CLRType
        {

            public CLRTypeGenericParameterType(Type type, ILEnvironment env) : base(type, env)
            {
            }

            public override bool IsGenericParameter
            {
                get { return true; }
            }

            public override int GenericParameterPosition
            {
                get { return typeForCLR.GenericParameterPosition; }
            }

            private ILType declaringType;
            private bool isDeclaringTypeInit;

            public override ILType DeclaringType
            {
                get
                {
                    if (!isDeclaringTypeInit)
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
                    if (isDeclaringTypeInit)
                    {
                        return;
                    }
                    declaringType = Environment.GetType(typeForCLR.DeclaringType);
                    isDeclaringTypeInit = true;
                }
            }

            public override string FullName
            {
                get { return DeclaringType.FullName + "!" + GenericParameterPosition; }
            }

            public override string FullQulifiedName
            {
                get { return DeclaringType.FullQulifiedName + "!" + GenericParameterPosition; }
            }

        }
    }
}
