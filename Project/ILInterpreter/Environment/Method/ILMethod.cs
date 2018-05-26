using System;
using System.Text;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method
{
    public abstract class ILMethod
    {

        public const string ConstructorName = ".ctor";

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

        public abstract bool IsMethod { get; }

        public abstract bool IsConstructor { get; }
        
        public abstract bool IsStaticConstructor { get; }
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

        private string fullname;
        public sealed override string ToString()
        {
            if (fullname == null)
            {
                lock (Environment)
                {
                    if (fullname != null)
                    {
                        return fullname;
                    }
                    var sb = new StringBuilder();
                    sb.Append(ReturnType);
                    sb.Append(' ');
                    sb.Append(DeclaringType);
                    sb.Append("::");
                    sb.Append(Name);
                    sb.Append(Parameters.ToJoinString("(", ")", ",", parameter => parameter.ParameterType.ToString()));
                    fullname = sb.ToString();
                }
            }
            return fullname;
        }

    }
}
