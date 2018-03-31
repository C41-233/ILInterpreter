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

            public Dictionary<T, int> h;

        }

        public static void Main(string[] args)
        {
            var env = new ILEnvironment();
            Console.WriteLine(typeof(Test<>).GetField("h").FieldType);
            var t = env.GetType(typeof(Test<>).GetField("h").FieldType);
            Console.WriteLine(t);
        }
    }
}
