using System;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Support;

namespace ILInterpreter.Environment
{
    public sealed class ILEnvironment
    {
        #region 类型缓存
        private readonly SharedDictionary<Type, ILType> TypeToILType = new SharedDictionary<Type, ILType>();
        private readonly SharedDictionary<int, ILType> IdToType = new SharedDictionary<int, ILType>();
        private readonly SharedTypeNameDictionary NameToTypes = new SharedTypeNameDictionary();
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
            ILType type;
            if (TypeToILType.TryGetValue(clrType, out type))
            {
                return type;
            }

            type = new CLRType(clrType, this);
            CacheType(type);
            return type;
        }

        public ILType GetType(string fullname)
        {
            if (fullname == null)
            {
                return null;
            }
            ILType type;
            if (NameToTypes.TryGetValue(fullname, out type))
            {
                return type;
            }

            var symbol = TypeSymbol.Parse(fullname);
            if (NameToTypes.TryGetValue(symbol, out type))
            {
                return type;
            }
            return null;
        }

        private void CacheType(ILType type)
        {
            var clrType = type as CLRType;
            if (clrType != null)
            {
                //todo 这里还要区分泛型参数是RuntimeType的情况
                TypeToILType[type.TypeForCLR] = type;
                
            }
            IdToType[type.Id] = type;
            NameToTypes.Add(type.FullName, type);
        }
        #endregion

    }
}
