using System;
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
            Run(typeof(int*[][,]));
            Run(typeof(List<List<int>>));
        }

        private static void Run(Type type)
        {
            var env = new ILEnvironment();
            Console.WriteLine(env.GetType(type).AssemblyQualifiedName);
            Console.WriteLine(type.AssemblyQualifiedName);
        }
    }
}
