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
            var env = ILEnvironment.Create();
            env.GetType("System.Collections.Generic.Dictionary`2[System.Collections.Generic.Dictionary`2[System.Int32,System.Int32],System.Collections.Generic.Dictionary`2[System.Int32,System.Int32]]");
        }
    }
}
