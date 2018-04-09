using TestMain.TestBase;

namespace TestMain.Engine.TestBase
{
    public static class Assert
    {

        public static void AreEquals(object actual, object expected)
        {
            if (!Equals(actual, expected))
            {
                throw new AssertException($"Assert fail \nexpected = {Info(expected)} \nactual   = {Info(actual)}");
            }
        }

        public static void AreEquals(long actual, long expected)
        {
            if (actual != expected)
            {
                throw new AssertException($"Assert fail expected={expected} actual={actual}");
            }
        }

        public static void AreEquals(int actual, int expected)
        {
            if (actual != expected)
            {
                throw new AssertException($"Assert fail expected={expected} actual={actual}");
            }
        }

        public static void AreEquals(bool actual, bool expected)
        {
            if (actual != expected)
            {
                throw new AssertException($"Assert fail expected={expected} actual={actual}");
            }
        }

        public static void AreSame(object actual, object expected)
        {
            if (!ReferenceEquals(actual, expected))
            {
                throw new AssertException($"Assert fail \nexpected = {Info(expected)} \nactual   = {Info(actual)}");
            }
        }

        public static void True(bool value)
        {
            if (!value)
            {
                throw new AssertException("Assert fail expected=true actual=false");
            }
        }

        public static void False(bool value)
        {
            if (value)
            {
                throw new AssertException("Assert fail expected=false actual=true");
            }
        }

        public static void Null(object value)
        {
            if (value != null)
            {
                throw new AssertException($"Assert fail expected=null actual={Info(value)}");
            }
        }

        public static void NotNull(object value)
        {
            if (value == null)
            {
                throw new AssertException($"Assert fail expected not null actual=null");
            }
        }

        private static string Info(object value)
        {
            if (value == null)
            {
                return "null";
            }
            return $"[{value.GetType()}]{value}";
        }

    }
}
