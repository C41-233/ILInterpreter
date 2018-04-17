using System;
using System.Collections.Generic;
using ILInterpreter.Support;

namespace ILInterpreter.Interpreter
{
    public static class StringPool
    {

        private static readonly Dictionary<int, FastList<string>> cache = new Dictionary<int, FastList<string>>();

        internal static string Put(string value, out int high32, out int low32)
        {
            var hash = value.GetHashCode();
            high32 = hash;

            lock (cache)
            {
                FastList<string> list;
                if (!cache.TryGetValue(hash, out list))
                {
                    list = new FastList<string>(1);
                    cache.Add(hash, list);
                }
                for(var i=0; i<list.Count; i++)
                {
                    if (value.Equals(list[i], StringComparison.Ordinal))
                    {
                        low32 = i;
                        return list[i];
                    }
                }

                var intern = string.Intern(value);
                list.Add(intern);
                low32 = list.Count - 1;
                return intern;
            }
        }

        public static string Get(string value)
        {
            if (value == null)
            {
                return null;
            }
            var hash = value.GetHashCode();
            lock (cache)
            {
                FastList<string> list;
                if (!cache.TryGetValue(hash, out list))
                {
                    return null;
                }
                foreach (var s in list)
                {
                    if (value.Equals(s, StringComparison.Ordinal))
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        internal static string Get(int high32, int low32)
        {
            lock (cache)
            {
#if DEBUG
                FastList<string> list;
                if (!cache.TryGetValue(high32, out list))
                {
                    return null;
                }
                if (low32 >= list.Count)
                {
                    return null;
                }
                return list[low32];
#else
                return cache[high32][low32];
#endif
            }
        }

    }
}
