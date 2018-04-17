using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using ILInterpreter.Environment;
using TestMain.Engine;
using TestMain.TestCase;

namespace TestMain
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Test();
            //TestLoader.Run();
        }

        private static void Test()
        {
            var env = new ILEnvironment();
            env.LoadAssemblyFromFile(Constant.TestCaseDll);
            var type = env.GetType("TestCase.Test01");
            var method = type.GetDeclaredMethod("Run1", null, null, null);
            method.Invoke(null);
            Console.WriteLine(method.ReturnType);
        }

    }
}
