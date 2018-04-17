using System.Threading;

namespace ILInterpreter.Support
{
    internal static class IdConstant
    {

        private static int Type = 100000;
        private static int Method = 200000;

        public static int NextTypeId()
        {
            return Interlocked.Increment(ref Type);
        }

        public static int NextMethodId()
        {
            return Interlocked.Increment(ref Method);
        }
    }
}
