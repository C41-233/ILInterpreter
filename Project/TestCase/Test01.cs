using System;

namespace TestCase
{
    public static class Test01
    {

        public static void Run1()
        {
            Console.WriteLine("Hello World!");
        }

        public static string Run2()
        {
            return "Hello World!";
        }

        public static int Run3(int val)
        {
            var obj = new TestClass();
            return obj.Calc(val);
        }

        public class TestClass
        {

            public int Calc(int v)
            {
                return v * v;   
            }

        }

    }

}
