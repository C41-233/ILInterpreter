using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Support;

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

        public abstract IListView<MethodParameter> Parameters { get; }

        public abstract ILType ReturnType { get; }

        public int ParametersCount
        {
            get { return Parameters.Count; }
        }

        public abstract bool Matches(ILType[] genericArguments, ILType[] parameterTypes, ILType returnType);

        public abstract object Invoke(object instance, params object[] parameters);

    }
}
