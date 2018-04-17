using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method
{
    public abstract class ILMethod
    {

        internal ILMethod()
        {
            hashCode = IdConstant.NextMethodId();
        }

        public abstract ILType DeclaringType { get; }

        public ILEnvironment Environment
        {
            get { return DeclaringType.Environment; }
        }

        #region Property
        public abstract string Name { get; }
        public abstract bool HasThis { get; }
        #endregion

        public abstract IListView<MethodParameter> Parameters { get; }

        public abstract ILType ReturnType { get; }

        public int ParametersCount
        {
            get { return Parameters.Count; }
        }

        public abstract bool Matches(ILType[] genericArguments, ILType[] parameterTypes, ILType returnType);

        internal abstract bool Matches(MethodReference reference);

        public abstract object Invoke(object instance, params object[] parameters);

        private readonly int hashCode;

        public sealed override int GetHashCode()
        {
            return hashCode;
        }
    }
}
