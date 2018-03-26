using System.Text;

namespace ILInterpreter.Support
{
    internal static class ListExtends
    {

        public static string Join<T>(this FastList<T> list, string seperate, string start, string end)
        {
            var sb = new StringBuilder();
            sb.Append(start);
            for (var i = 0; i < list.Count; i++)
            {
                sb.Append(list[i]);
                if (i != list.Count - 1)
                {
                    sb.Append(seperate);
                }
            }
            sb.Append(end);
            return sb.ToString();
        }


    }
}
