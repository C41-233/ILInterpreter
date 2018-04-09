using System.IO;
using ILInterpreter.Environment;
using ILInterpreter.Environment.TypeSystem;
using TestMain.Engine.TestBase;

namespace TestMain.TestCase
{

    [TestClass]
    public static class TestCase03
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
            var type = env.GetType("TestCase.Test01+TestClass");
            var method = type.GetDeclaredMethod("Calc", null, new ILType[] {env.GetType<int>()}, env.GetType<int>());
            Assert.AreEquals(method.Name, "Calc");
        }

    }
}
