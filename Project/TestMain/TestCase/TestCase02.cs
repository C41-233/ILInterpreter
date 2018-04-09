using System;
using System.IO;
using ILInterpreter.Environment;
using ILInterpreter.Environment.TypeSystem;
using TestMain.Engine.TestBase;

namespace TestMain.TestCase
{

    [TestClass]
    public static class TestCase02
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
            Assert.AreEquals(type.FullName, "TestCase.Test01");
            Assert.AreEquals(type.BaseType.TypeForCLR, typeof(object));
            Assert.AreSame(type.MakeArrayType(), env.GetType("TestCase.Test01[]"));
        }

        [Test]
        public static void Test02()
        {
            var type = env.GetType("TestCase.Test01+TestClass");
            Assert.False(type.IsPublic);
            Assert.True(type.IsNestedPublic);
        }

        public struct StructTest
        {
            
        }

        [Test]
        public static void Test03()
        {
            Action<ILType, Type> test = (ilType, clrType) =>
            {
                Assert.AreEquals(ilType.IsPublic, clrType.IsPublic);
                Assert.AreEquals(ilType.IsNestedPublic, clrType.IsNestedPublic);
                Assert.AreEquals(ilType.IsAbstract, clrType.IsAbstract);
                Assert.AreEquals(ilType.IsSealed, clrType.IsSealed);
            };
            test(env.GetType("TestCase.Test02+StructTest"), typeof(StructTest));
        }

    }
}
