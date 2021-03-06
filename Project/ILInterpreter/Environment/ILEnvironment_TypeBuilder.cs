﻿using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.CLR;
using ILInterpreter.Environment.TypeSystem.Symbol;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment
{
    public sealed partial class ILEnvironment
    {

        internal ILType GetType(int id)
        {
            lock (this)
            {
                ILType type;
                if (IdToType.TryGetValue(id, out type))
                {
                    return type;
                }
            }
            return null;
        }

        internal ILType GetType(TypeReference reference)
        {
            ILType type;
            lock (this)
            {
                if (TypeReferenceToTypes.TryGetValue(reference.GetHashCode(), out type))
                {
                    return type;
                }
            }
            type = GetType(reference.GetTypeAssemblyQualifiedName());
            if (type == null)
            {
                return null;
            }
            lock (this)
            {
                TypeReferenceToTypes[reference.GetHashCode()] = type;
            }
            return type;
        }

        private ILType GetTypeInternal(ITypeSymbol symbol)
        {
            var fullname = symbol.FullName;
            IListView<ILType> list;
            if (!NameToTypes.TryGetValues(fullname, out list))
            {
                return CreateTypeInternal(symbol);
            }

            int currentScore = int.MinValue;
            ILType currentType = null;
            foreach (var type in list)
            {
                int score;
                if (Score.GetTypeWeakMatchScore(type, symbol, out score))
                {
                    if (score > currentScore)
                    {
                        currentScore = score;
                        currentType = type;
                    }
                }
            }
            if (currentType == null)
            {
                return CreateTypeInternal(symbol);
            }
            if (currentScore >= 0)
            {
                return currentType;
            }
            var typeInternal = CreateTypeInternal(symbol);
            return typeInternal ?? currentType;
        }

        private ILType CreateTypeInternal(ITypeSymbol symbol)
        {
            var pointerSymbol = symbol as PointerSymbol;
            if (pointerSymbol != null)
            {
                var element = GetTypeInternal(pointerSymbol.Element);
                if (element == null)
                {
                    return null;
                }
                var type = element.CreatePointerType();
                return CacheTypeInternal(type);
            }

            var refSymbol = symbol as RefSymbol;
            if (refSymbol != null)
            {
                var element = GetTypeInternal(refSymbol.Element);
                if (element == null)
                {
                    return null;
                }
                var type = element.CreateByRefType();
                return CacheTypeInternal(type);
            }

            var arraySymbol = symbol as ArraySymbol;
            if (arraySymbol != null)
            {
                var element = GetTypeInternal(arraySymbol.Element);
                if (element == null)
                {
                    return null;
                }
                var type = element.CreateArrayType(arraySymbol.Rank);
                return CacheTypeInternal(type);
            }

            var genericSymbol = symbol as GenericSymbol;
            if (genericSymbol != null)
            {
                var elementType = GetTypeInternal(genericSymbol.Element);
                if (elementType == null)
                {
                    return null;
                }

                var genericArugments = new FastList<ILType>(genericSymbol.GenericParameters.Count);
                foreach (var generic in genericSymbol.GenericParameters)
                {
                    var genericType = GetTypeInternal(generic);
                    if (genericType == null)
                    {
                        return null;
                    }
                    genericArugments.Add(genericType);
                }

                var type = elementType.CreateGenericType(genericArugments);
                return CacheTypeInternal(type);
            }

            var nameSymbol = symbol as NameSymbol;
            if(nameSymbol != null)
            {
                var clr = TypeSupport.GetType(nameSymbol.AssemblyQualifiedName);
                if (clr == null)
                {
                    return null;
                }
                var type = CLRType.Create(clr, this);
                return CacheTypeInternal(type);
            }
            return null;
        }

    }
}
