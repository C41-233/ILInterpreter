namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal sealed class PointerSymbol : ComponentSymbol
    {
        public PointerSymbol(ITypeSymbol element) : base(element)
        {
        }

        public override string FullName
        {
            get { return Element.FullName + "*"; }
        }
    }
}