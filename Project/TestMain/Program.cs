using System;
using TestMain.Engine;

namespace TestMain
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Test();
            TestLoader.Run();
        }

        private static void Test()
        {
            Console.WriteLine(typeof(int).MakeByRefType().IsAbstract);
        }

    }
}
