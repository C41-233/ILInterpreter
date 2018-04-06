using System;

namespace TestMain.TestBase
{

    public class AssertException : Exception
    {

        public AssertException(string msg) : base(msg)
        {
        }

    }

}
