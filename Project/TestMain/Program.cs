using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            Console.WriteLine(env.GetType("System.Collections.Generic.List`1[][,]"));
        }
    }
}
