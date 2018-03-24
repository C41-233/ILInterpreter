using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ILInterpreter.Environment.TypeSystem;

namespace ILInterpreter.Environment
{
    public sealed class ILEnvironment
    {

        public static ILEnvironment Create()
        {
            return new ILEnvironment();
        }

        private ILEnvironment()
        {
            
        }

        #region 类型缓存
        private readonly Dictionary<Type, CLRType> ClrType2Types = new Dictionary<Type, CLRType>();
        private readonly Dictionary<string, ILType> Symbol2Types = new Dictionary<string, ILType>();
        private readonly Dictionary<int, ILType> Token2Types = new Dictionary<int, ILType>();
        #endregion

        #region IL加载
        public void LoadFile(string v)
        {
        }

        public void Load(Stream stream)
        {

        }
        #endregion

        #region ；类型系统
        public ILType GetType<T>()
        {
            return GetType(typeof(T));
        }

        public ILType GetType(Type clrType)
        {
            CLRType type;
            if (ClrType2Types.TryGetValue(clrType, out type))
            {
                return type;
            }

            type = new CLRType(clrType, this);
            CacheType(type);
            return type;
        }

        public ILType GetType(string symbol)
        {
            ILType type;
            if (Symbol2Types.TryGetValue(symbol, out type))
            {
                return type;
            }

            #region 创建类型
            var parse = TypeSymbol.Parse(symbol);
            return GetType(parse);
            #endregion
        }

        private ILType GetType(ITypeSymbol symbol)
        {
            {
                ILType type;
                if (Symbol2Types.TryGetValue(symbol.ToString(), out type))
                {
                    return type;
                }
            }

            {
                var classSymbol = symbol as ClassSymbol;
                if (classSymbol != null)
                {
                    ILType type;
                    if (Symbol2Types.TryGetValue(classSymbol.Name, out type))
                    {
                        return type;
                    }
                    var clrType = Type.GetType(classSymbol.Name);
                    type = new CLRType(clrType, this);
                    CacheType(type);
                    return type;
                }
            }

            {
                var pointerSymbol = symbol as PointerSymbol;
                if (pointerSymbol != null)
                {
                    var elementType = GetType(pointerSymbol.ElementType);
                    var pointerType = elementType.CreatePointerType();
                    CacheType(pointerType);
                    return pointerType;
                }
            }

            {
                var refSymbol = symbol as RefSymbol;
                if (refSymbol != null)
                {
                    var elementType = GetType(refSymbol.ElementType);
                    var refType = elementType.CreateRefType();
                    CacheType(refType);
                    return refType;
                }
            }

            {
                var arraySymbol = symbol as ArraySymbol;
                if (arraySymbol != null)
                {
                    var elementType = GetType(arraySymbol.ElementType);
                    var arrayType = elementType.CreateArrayType(arraySymbol.Rank);
                    CacheType(arrayType);
                    return arrayType;
                }
            }

            {
                var genericSymbol = symbol as GenerocSymbol;
                if (genericSymbol != null)
                {
                    var elementType = GetType(genericSymbol.ElementType);
                    var generics = genericSymbol.Generics.Select(s=>GetType(s)).ToArray();
                    var genericType = elementType.CreateGenericInstance(generics);
                    CacheType(genericType);
                    return genericType;
                }
            }

            return null;
        }

        private void CacheType(ILType type)
        {
            var clrType = type as CLRType;
            if (clrType != null)
            {
                ClrType2Types[clrType.Type] = clrType;
            }
            Symbol2Types[type.Symbol] = type;
            Token2Types[type.Token] = type;

            Console.WriteLine("cache "+type);
        }
        #endregion

    }
}
