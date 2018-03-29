﻿using System;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Symbol;
using ILInterpreter.Support;

namespace ILInterpreter.Environment
{
    public sealed partial class ILEnvironment
    {

        private ILType GetTypeInternal(ITypeSymbol symbol)
        {
            var fullname = symbol.FullName;
            IListView<ILType> list;
            if (!NameToTypes.TryGetValues(fullname, out list))
            {
                return CreateTypeInternal(symbol);
            }

            int currentScore = -1;
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
            return currentType ?? CreateTypeInternal(symbol);
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

            var nameSymbol = symbol as NameSymbol;
            if(nameSymbol != null)
            {
                var clr = Type.GetType(nameSymbol.AssemblyQualifiedName);
                var type = CLRType.Create(clr, this);
                return CacheTypeInternal(type);
            }
            return null;
        }

    }
}