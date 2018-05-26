using System.IO;
using ILInterpreter.Environment;
using ILInterpreter.Environment.TypeSystem;
using TestMain.Engine.TestBase;

namespace TestMain.TestCase
{

    [TestClass]
    public static class TestCase04
    {
        private static byte[] bs;
        private static ILEnvironment env;

        [BeforeClass]
        public static void Init()
        {
            bs = File.ReadAllBytes(Constant.TestCaseDll);
        }

        [Before]
        public static void Before()
        {
            env = new ILEnvironment();
            env.LoadAssembly(new MemoryStream(bs));
        }

        [After]
        public static void After()
        {
            env = null;
        }

        [Test]
        public static void Test01()
        {
            var type = env.GetType("TestCase.Test01");
            var method = type.GetDeclaredMethod("Run2", null, null, env.String);
            Assert.AreEquals(method.Invoke(null), "Hello World!");
        }

    }
}
