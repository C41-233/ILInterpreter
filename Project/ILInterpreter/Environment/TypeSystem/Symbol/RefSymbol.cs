namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal sealed class RefSymbol : ComponentSymbol
    {
        public RefSymbol(ITypeSymbol element) : base(element)
        {
        }

        public override string FullName
        {
            get { return Element.FullName + "&"; }
        }
    }
}