using ILInterpreter.Environment.TypeSystem;

namespace ILInterpreter.Environment.Method
{
    public abstract class ILMethod
    {

        public const string ConstructorName = ".ctor";

        public abstract ILType DeclaringType { get; }

        public ILEnvironment Environment
        {
            get { return DeclaringType.Environment; }
        }

        public abstract string Name { get; }

        public abstract bool Matches(ILType[] genericArguments, ILType[] parameterTypes, ILType returnType);

    }
}
