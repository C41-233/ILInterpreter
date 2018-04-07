using System;
using System.Collections.Generic;
using System.Linq;
using ILInterpreter.Environment;
using Mono.Cecil;
using TestMain.Engine;
using TestMain.Engine.TestBase;
using TestMain.TestCase;

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
        }

    }
}
