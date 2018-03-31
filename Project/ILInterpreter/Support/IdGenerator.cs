using System.Threading;

namespace ILInterpreter.Support
{
    internal static class IdConstant
    {

        private static int Type = 100000;

        public static int NextTypeId()
        {
            return Interlocked.Increment(ref Type);
        }

    }
}
