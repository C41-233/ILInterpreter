namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal sealed class NameSymbol : ITypeSymbol
    {
        public readonly string Name;

        public override string FullName
        {
            get { return Name; }
        }

        public NameSymbol(string name, string assembly) : base(assembly)
        {
            Name = name;
        }
    }
}