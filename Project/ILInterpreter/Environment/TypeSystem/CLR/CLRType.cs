using System;
using System.Collections.Generic;
using System.Reflection;
using ILInterpreter.Environment.Method;
using ILInterpreter.Environment.Method.CLR;
using ILInterpreter.Support;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace ILInterpreter.Environment.TypeSystem.CLR
{

    internal abstract partial class CLRType : ILType
    {

        internal CLRType(Type type, ILEnvironment env) : base(env)
        {
            typeForCLR = type;
            assemblyName = new AssemblyName(type.Assembly.GetName());
        }

        protected readonly Type typeForCLR;
        public sealed override Type TypeForCLR
        {
            get { return typeForCLR; }
        }

        private readonly AssemblyName assemblyName;
        public sealed override AssemblyName AssemblyName
        {
            get { return assemblyName; }
        }

        public sealed override string Namespace
        {
            get { return typeForCLR.Namespace; }
        }

        public sealed override string Name
        {
            get { return typeForCLR.Name; }
        }

        public override ILType ElementType
        {
            get { return null; }
        }

        public override bool HasElementType
        {
            get { return false; }
        }

        private ILType baseType;
        private bool isBaseTypeInit;
        public override ILType BaseType
        {
            get
            {
                if (!isBaseTypeInit)
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
                if (isBaseTypeInit)
                {
                    return;
                }
                if (typeForCLR.BaseType != null)
                {
                    baseType = Environment.GetType(typeForCLR.BaseType);
                }
                isBaseTypeInit = true;
            }
        }

        public override ILType DeclaringType
        {
            get { return null; }
        }

        #region Modifiers
        public sealed override bool IsAbstract
        {
            get { return typeForCLR.IsAbstract; }
        }

        public sealed override bool IsSealed
        {
            get { return typeForCLR.IsSealed; }
        }

        public override bool IsPublic
        {
            get { return typeForCLR.IsPublic; }
        }

        public override bool IsNestedPublic
        {
            get { return typeForCLR.IsNestedPublic; }
        }

        #endregion


        #region Ref
        public override bool IsByRef
        {
            get { return false; }
        }

        internal sealed override ILType CreateByRefTypeInternal()
        {
            var type = typeForCLR.MakeByRefType();
            var self = new CLRByRefType(this, type, Environment);
            return self;
        }
        #endregion

        #region Pointer
        public override bool IsPointer
        {
            get { return false; }
        }

        internal sealed override ILType CreatePointerTypeInternal()
        {
            var type = typeForCLR.MakePointerType();
            var self = new CLRPointerType(this, type, Environment);
            return self;
        }
        #endregion

        #region Array
        public override bool IsArray
        {
            get { return false; }
        }

        public override int ArrayRank
        {
            get { return 0; }
        }

        internal sealed override ILType CreateArrayTypeInternal(int rank)
        {
            var type = rank == 1 ? typeForCLR.MakeArrayType() : typeForCLR.MakeArrayType(rank);
            var self = new CLRArrayType(this, type, Environment);
            return self;
        }
        #endregion

        #region Generic

        public override bool IsGenericTypeDefinition
        {
            get { return false; }
        }

        public override bool IsGenericParameter
        {
            get { return false; }
        }

        public override bool IsGenericType
        {
            get { return false; }
        }

        public override ILType GenericTypeDefinition
        {
            get { return null; }
        }

        public override IListView<ILType> GenericArguments
        {
            get { return null; }
        }

        public override int GenericParameterPosition
        {
            get { return -1; }
        }

        internal override ILType CreateGenericTypeInternal(FastList<ILType> genericArugments)
        {
            return null;
        }
        #endregion

        #region Methods
        private Dictionary<string, FastList<CLRMethod>> _methods;

        private Dictionary<string, FastList<CLRMethod>> Methods
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
                if (_methods != null)
                {
                    return;
                }
                var methods = new Dictionary<string, FastList<CLRMethod>>();
                var clrMethods = typeForCLR.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var method in clrMethods)
                {
                    FastList<CLRMethod> list;
                    if (!methods.TryGetValue(method.Name, out list))
                    {
                        list = new FastList<CLRMethod>();
                        methods.Add(method.Name, list);
                    }
                    list.Add(new CLRMethod(method, this));
                }

                foreach (var list in methods.Values)
                {
                    list.Trim();
                }

                _methods = methods;
            }
        }

        public override ILMethod GetDeclaredMethod(string name, ILType[] genericArguments, ILType[] parameterTypes, ILType returnType)
        {
            //todo
            return null;
        }
        #endregion


        internal static CLRType Create(Type type, ILEnvironment env)
        {
            if (type.IsPointer)
            {
                var elementType = env.GetType(type.GetElementType());
                return new CLRPointerType(elementType, type, env);
            }

            if (type.IsByRef)
            {
                var elementType = env.GetType(type.GetElementType());
                return new CLRByRefType(elementType, type, env);
            }

            if (type.IsArray)
            {
                var elementType = env.GetType(type.GetElementType());
                return new CLRArrayType(elementType, type, env);
            }

            if (type.IsGenericParameter)
            {
                if (type.DeclaringType != null)
                {
                    return new CLRTypeGenericParameterType(type, env);
                }
            }

            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition)
                {
                    return new CLRGenericDefinitionType(type, env);
                }

                var clrGenericArguments = type.GetGenericArguments();
                var genericArguments = new FastList<ILType>(clrGenericArguments.Length);
                foreach (var generic in clrGenericArguments)
                {
                    genericArguments.Add(env.GetType(generic));
                }
                var genericTypeDefinition = env.GetType(type.GetGenericTypeDefinition());
                return new CLRGenericSpecificationType(genericTypeDefinition, genericArguments, type, env);
            }

            return new CLRDirectType(type, env);
        }

    }
}
