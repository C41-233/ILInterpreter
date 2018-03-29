namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal abstract class ComponentSymbol : ITypeSymbol
    {

        public readonly ITypeSymbol Element;

        protected ComponentSymbol(ITypeSymbol element) : base(element.AssemblyName)
        {
            Element = element;
        }
    }
}