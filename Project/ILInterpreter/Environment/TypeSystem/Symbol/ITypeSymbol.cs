namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal abstract class ITypeSymbol
    {

        public readonly AssemblyName AssemblyName;

        public abstract string FullName { get; }

        public string AssemblyQualifiedName
        {
            get { return AssemblyName != null ? FullName + ", " + AssemblyName : FullName; }
        }

        protected ITypeSymbol(AssemblyName assembly)
        {
            AssemblyName = assembly;
        }

        protected ITypeSymbol(string assembly)
        {
            AssemblyName = assembly == null ? null : new AssemblyName(assembly);
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}