using System;

namespace ILInterpreter.Environment
{

    public class ILTypeLoadException : Exception
    {

        public ILTypeLoadException(string msg) : base(msg)
        {
            
        }

        public ILTypeLoadException(string msg, Exception e) : base(msg, e)
        {
            
        }

    }
}
