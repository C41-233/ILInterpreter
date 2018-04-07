using System.Collections.Generic;
using ILInterpreter.Environment;
using TestMain.Engine.TestBase;
using TestMain.TestBase;

namespace TestMain.TestCase
{
    [TestClass]
    public static class TestCase01
    {

        private static ILEnvironment env;

        [Before]
        public static void Before()
        {
            env = new ILEnvironment();
        }

        [After]
        public static void After()
        {
            env = null;
        }

        [Test]
        public static void Test01()
        {
            var type = env.GetType(typeof(int));
            Assert.AreEquals(type.FullName, "System.Int32");
            Assert.AreEquals(type.FullQulifiedName, "System.Int32");
            Assert.AreEquals(type.AssemblyQualifiedName, typeof(int).AssemblyQualifiedName);
            Assert.AreSame(type, env.GetType("System.Int32"));
            Assert.AreSame(type, env.GetType("System.Int32,mscorlib"));
            Assert.AreSame(type, env.GetType(typeof(int).AssemblyQualifiedName));
            Assert.False(type.IsArray);
            Assert.AreEquals(type.ArrayRank, 0);
            Assert.False(type.IsPointer);
            Assert.False(type.IsByRef);
            Assert.False(type.IsGenericTypeDefinition);
            Assert.False(type.IsGenericType);
            Assert.False(type.IsGenericParameter);
            Assert.Null(type.GenericTypeDefinition);
        }

        [Test]
        public static void Test02()
        {
            var type = env.GetType(typeof(int[][,]));
            Assert.AreEquals(type.FullName, "System.Int32[,][]");
            Assert.AreEquals(type.AssemblyQualifiedName, typeof(int[][,]).AssemblyQualifiedName);
            Assert.True(type.IsArray);
            Assert.AreEquals(type.ArrayRank, 1);
            Assert.AreSame(type, env.GetType("System.Int32[,][]"));
            Assert.AreSame(type, env.GetType("System.Int32[,][], mscorlib"));
            Assert.AreSame(type, env.GetType("System.Int32[,],mscorlib").MakeArrayType());
            Assert.AreSame(type, type.ElementType.MakeArrayType());
        }

        [Test]
        public static void Test03()
        {
            var type = env.GetType(typeof(List<>));
            Assert.AreEquals(type.FullName, "System.Collections.Generic.List`1");
            Assert.AreEquals(type.FullQulifiedName, typeof(List<>).FullName);
            Assert.AreEquals(type.AssemblyQualifiedName, typeof(List<>).AssemblyQualifiedName);
            Assert.AreSame(type, env.GetType(typeof(List<>)));
            Assert.AreSame(type, env.GetType("System.Collections.Generic.List`1"));
            Assert.AreSame(type, env.GetType("System.Collections.Generic.List`1[System.Int32]").GenericTypeDefinition);
            Assert.AreSame(type.MakeGenericType(env.GetType(typeof(int))), env.GetType("System.Collections.Generic.List`1[[System.Int32, mscorlib]]"));
            Assert.AreSame(type.MakeGenericType(env.GetType(typeof(int))), env.GetType(typeof(List<int>)));
        }

        [Test]
        public static void Test04()
        {
            var type = typeof(Dictionary<List<int>, int[,]>);
            Assert.AreEquals(env.GetType(type).FullName, "System.Collections.Generic.Dictionary`2[System.Collections.Generic.List`1[System.Int32],System.Int32[,]]");
            Assert.AreEquals(env.GetType(type).FullQulifiedName, type.FullName);
            Assert.AreEquals(env.GetType(type).AssemblyQualifiedName, type.AssemblyQualifiedName);
            Assert.AreSame(env.GetType(type), env.GetType("System.Collections.Generic.Dictionary`2[System.Collections.Generic.List`1[System.Int32],System.Int32[,]]"));
            Assert.AreSame(env.GetType(type), env.GetType(typeof(Dictionary<,>)).MakeGenericType(typeof(List<int>),typeof(int[,])));
        }
    }

}
