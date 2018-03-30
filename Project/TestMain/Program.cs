using System;
using System.Collections.Generic;
using ILInterpreter.Environment;

namespace TestMain
{
    public class Program
    {

        public class A
        {

            public class B
            {

            }

        }

        public class Test<T>
        {

            public Dictionary<int, int> h;

        }

        public static void Main(string[] args)
        {
            var env = new ILEnvironment();
            var a = env.GetType(typeof(List<int>));
            Console.WriteLine(a);
            var b = env.GetType("System.Collections.Generic.List`1[System.Int32]");
            Console.WriteLine(b);
            var c = env.GetType("System.Collections.Generic.List`1").MakeGenericType(env.GetType<int>());
            Console.WriteLine(c);

            Console.WriteLine(a==b);
            Console.WriteLine(a == c);
        }
    }
}
