using System;
using System.Collections.Generic;
using System.IO;
using ILInterpreter.Environment.Method.Runtime;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.CLR;
using ILInterpreter.Environment.TypeSystem.Runtime;
using ILInterpreter.Environment.TypeSystem.Symbol;
using ILInterpreter.Interpreter;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment
{
    public sealed partial class ILEnvironment
    {
        #region Type Cache
        private readonly Dictionary<Type, ILType> TypeToILType = new Dictionary<Type, ILType>();
        private readonly Dictionary<int, ILType> IdToType = new Dictionary<int, ILType>();
        private readonly TypeNameDictionary NameToTypes = new TypeNameDictionary();
        private readonly Dictionary<int, ILType> TypeReferenceToTypes = new Dictionary<int, ILType>();
        #endregion

        #region Type System

        public ILType GetType<T>()
        {
            return GetType(typeof(T));
        }

        public ILType GetType(Type clrType)
        {
            if (clrType == null)
            {
                return null;
            }
            lock (this)
            {
                ILType type;
                if (TypeToILType.TryGetValue(clrType, out type))
                {
                    return type;
                }

                type = CLRType.Create(clrType, this);
                return CacheTypeInternal(type);
            }
        }

        public ILType GetType(string fullname)
        {
            if (fullname == null)
            {
                return null;
            }
            lock (this)
            {
                ILType type;
                if (NameToTypes.TryGetValue(fullname, out type))
                {
                    return type;
                }
                var symbol = TypeSymbol.Parse(fullname);
                return GetTypeInternal(symbol);
            }
        }

        private ILType CacheTypeInternal(ILType type)
        {
            var clrType = type as CLRType;
            if (clrType != null)
            {
                //todo 这里还要区分泛型参数是RuntimeType的情况
                TypeToILType[type.TypeForCLR] = type;
            }
            var runtimeType = type as RuntimeType;
            if (runtimeType != null)
            {
                TypeReferenceToTypes[runtimeType.TypeReference.GetHashCode()] = type;
            }
            IdToType[type.Id] = type;
            NameToTypes.Add(type.FullName, type);
            return type;
        }
        #endregion

        #region Dll Load

        public void LoadAssemblyFromFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                LoadAssembly(stream);
            }
        }

        public void LoadAssembly(Stream stream)
        {
            var module = ModuleDefinition.ReadModule(stream);
            if (module.HasTypes)
            {
                foreach (var definition in module.GetTypes())
                {
                    var type = RuntimeType.Create(definition, this);
                    CacheTypeInternal(type);
                }
            }
        }

        #endregion

        #region Invoke
        private readonly FastList<RuntimeInterpreter> interpreters = new FastList<RuntimeInterpreter>();

        internal object Invoke(RuntimeMethod method, object instance, object[] parameters)
        {
            RuntimeInterpreter interpreter = null;
            lock (interpreters)
            {
                if (interpreters.Count > 0)
                {
                    interpreter = interpreters.Pop();
                }
            }
            if (interpreter == null)
            {
                interpreter = new RuntimeInterpreter(this);
            }

            try
            {
                return interpreter.Invoke(method, instance, parameters);
            }
            finally
            {
                lock (interpreters)
                {
                    interpreter.Clear();
                    interpreters.Push(interpreter);
                }
            }
        }
        #endregion
    }
}
