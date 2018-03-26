using System;
using System.IO;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Support;

namespace ILInterpreter.Environment
{
    public sealed class ILEnvironment
    {

        public ILEnvironment()
        {
            
        }

        #region 类型缓存
        private readonly SharedDictionary<Type, ILType> TypeToILType = new SharedDictionary<Type, ILType>();
        #endregion

        #region 类型系统

        public ILType GetType<T>()
        {
            return GetType(typeof(T));
        }

        public ILType GetType(Type clrType)
        {
            ILType type;
            if (TypeToILType.TryGetValue(clrType, out type))
            {
                return type;
            }

            type = new CLRType(clrType, this);
            return type;
        }
        #endregion

    }
}
