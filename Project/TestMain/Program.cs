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

        public class Test<T> : List<T>
        {

            public Dictionary<T, int> h;

        }

        public static void Main(string[] args)
        {
            var env = new ILEnvironment();
            env.LoadAssembyFromFile(@"G:\workspace\ILInterpreter\Project\bin\Debug\TestCase.dll");
            Console.WriteLine(env.GetType(typeof(Test<>)).BaseType);
            Console.WriteLine(env.GetType("TestCase.Test03+GenericTest`1").BaseType);
        }
    }
}
