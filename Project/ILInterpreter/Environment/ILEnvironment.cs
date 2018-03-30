﻿using System;
using System.Collections.Generic;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Symbol;

namespace ILInterpreter.Environment
{
    public sealed partial class ILEnvironment
    {
        #region 类型缓存
        private readonly Dictionary<Type, ILType> TypeToILType = new Dictionary<Type, ILType>();
        private readonly Dictionary<int, ILType> IdToType = new Dictionary<int, ILType>();
        private readonly TypeNameDictionary NameToTypes = new TypeNameDictionary();
        #endregion

        #region 类型系统

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
            IdToType[type.Id] = type;
            NameToTypes.Add(type.FullName, type);
            return type;
        }
        #endregion

    }
}
