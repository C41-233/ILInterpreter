namespace TestCase
{
    public static class Test02
    {

        public class GenericTest<T>
        {
            public T Get()
            {
                return default(T);
            }
        }

        public static void Run()
        {
            var a = new GenericTest<string>();
            var b = new GenericTest<int>();
        }

    }
}
