using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using ILInterpreter.Environment;
using ILInterpreter.Environment.TypeSystem;
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
            var method = type.GetDeclaredMethod("Run3", null, new ILType[] {env.Int}, env.Int);
            Console.WriteLine(method.Invoke(null, 10));
            //Console.WriteLine(typeof(object).GetConstructors()[0].Invoke(null));
        }

    }
}
