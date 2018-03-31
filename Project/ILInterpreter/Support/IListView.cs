using System;
using System.Collections.Generic;
using System.Text;

namespace ILInterpreter.Support
{
    public interface IListView<T> : IEnumerable<T>
    {

        int Count { get; }

        T this[int index] { get; }

    }

    internal static class ListExtends
    {

        public static string ToJoinString<T>(this IListView<T> self, string begin, string end, string split, Action<StringBuilder, T> appender)
        {
            var sb = new StringBuilder();
            sb.Append(begin);
            if (self.Count > 0)
            {
                appender(sb, self[0]);
            }
            for (var i = 1; i < self.Count; i++)
            {
                sb.Append(split);
                appender(sb, self[i]);
            }
            sb.Append(end);
            return sb.ToString();
        }

    }
}
