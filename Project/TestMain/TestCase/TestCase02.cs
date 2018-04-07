using System.IO;
using ILInterpreter.Environment;
using TestMain.Engine.TestBase;

namespace TestMain.TestCase
{

    [TestClass]
    public static class TestCase02
    {

        private static MemoryStream stream;
        private static ILEnvironment env;

        [BeforeClass]
        public static void Init()
        {
            stream = new MemoryStream(File.ReadAllBytes(Constant.TestCaseDll));
        }

        [Before]
        public static void Before()
        {
            env = new ILEnvironment();
            env.LoadAssembly(stream);
        }

        [After]
        public static void After()
        {
            env = null;
        }

        [Test]
        public static void Test01()
        {
        }

    }
}
