namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal abstract class ITypeSymbol
    {

        public readonly AssemblyName AssemblyName;

        public abstract string FullName { get; }

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