using System.Collections.Generic;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{
    internal sealed class TypeNameDictionary
    {

        private readonly Dictionary<string, FastList<ILType>> dict = new Dictionary<string, FastList<ILType>>();

        public void Add(string name, ILType type)
        {
            FastList<ILType> list;
            if (dict.TryGetValue(name, out list))
            {
                list.Add(type);
                return;
            }

            //通常只会有一个匹配项，节省内存
            list = new FastList<ILType>(1){type};
            dict.Add(name, list);
        }

        public bool TryGetValue(string name, out ILType type)
        {
            FastList<ILType> list;
            if (dict.TryGetValue(name, out list))
            {
                type = list[0];
                return true;
            }
            type = null;
            return false;
        }

        public bool TryGetValues(string name, out IListView<ILType> list)
        {
            FastList<ILType> fastList;
            if (dict.TryGetValue(name, out fastList))
            {
                list = fastList;
                return true;
            }
            list = null;
            return false;
        }
    }
}
