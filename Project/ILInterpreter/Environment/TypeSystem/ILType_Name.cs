using System.Linq;

namespace ILInterpreter.Environment.TypeSystem
{
    public abstract partial class ILType
    {
        public override string ToString()
        {
            if (HasFullName)
            {
                return FullName;
            }
            return Name;
        }

        public abstract AssemblyName AssemblyName { get; }

        public abstract string Namespace { get; }

        private void InitFullNameInternal()
        {
            lock (Environment)
            {
                if (isFullNameInit)
                {
                    return;
                }
                if (IsGenericParameter)
                {
                    return;
                }
                if (HasElementType && !ElementType.HasFullName)
                {
                    return;
                }
                if (IsGenericType && GenericArguments.Any(t => !t.HasFullName))
                {
                    return;
                }
                fullName = TypeNameExtends.Parse(this, false);
                isFullNameInit = true;
            }
        }

        private bool isFullNameInit;

        public bool HasFullName
        {
            get
            {
                return FullName != null;
            }
        }

        private string fullName;
        public string FullName
        {
            get
            {
                if (!isFullNameInit)
                {
                    InitFullNameInternal();
                }
                return fullName;
            }
        }

        private string fullQualifiedName;
        public string FullQulifiedName
        {
            get
            {
                if (HasFullName)
                {
                    return fullQualifiedName ?? (fullQualifiedName = TypeNameExtends.Parse(this, true));
                }
                return null;
            }
        }

        public string AssemblyQualifiedName
        {
            get
            {
                if (HasFullName)
                {
                    return FullQulifiedName + ", " + AssemblyName;
                }
                return null;
            }
        }

        public abstract string Name { get; }

    }
}
