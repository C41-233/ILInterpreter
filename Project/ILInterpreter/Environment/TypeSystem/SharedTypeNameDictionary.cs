using System.Collections.Generic;

namespace ILInterpreter.Environment.TypeSystem
{
    internal sealed class SharedTypeNameDictionary
    {

        private readonly Dictionary<string, List<ILType>> dict = new Dictionary<string, List<ILType>>();

        public void Add(string name, ILType type)
        {
            lock (dict)
            {
                List<ILType> list;
                if (dict.TryGetValue(name, out list))
                {
                    list.Add(type);
                }
                //通常只会有一个匹配项，节省内存
                list = new List<ILType>(1){type};
                dict.Add(name, list);
            }
        }

        public bool TryGetValue(string name, out ILType type)
        {
            lock (dict)
            {
                List<ILType> list;
                if (dict.TryGetValue(name, out list))
                {
                    type = list[0];
                    return true;
                }
            }
            type = null;
            return false;
        }

        public bool TryGetValue(ITypeSymbol symbol, out ILType type)
        {
            var fullname = symbol.FullName;
            lock (dict)
            {
                List<ILType> list;
                if (!dict.TryGetValue(fullname, out list))
                {
                    type = null;
                    return false;
                }
                foreach (var target in list)
                {
                    if (GetTypeWeakMatchScore(target, symbol) >= 0)
                    {
                        type = target;
                        return true;
                    }
                }
                type = null;
                return false;
            }
        }

        private static int GetTypeWeakMatchScore(ILType type, ITypeSymbol symbol)
        {
            var pointerSymbol = symbol as PointerSymbol;
            if (pointerSymbol != null)
            {
                return GetTypeWeakMatchScore(type.ElementType, pointerSymbol.Element);
            }

            var nameSymbol = symbol as NameSymbol;
            if (nameSymbol != null)
            {
                if (type.FullName != nameSymbol.Name)
                {
                    return -1;
                }
                return type.AssemblyName.GetWeakMatchScore(symbol.AssemblyName);
            }
            return -1;
        }

    }
}
