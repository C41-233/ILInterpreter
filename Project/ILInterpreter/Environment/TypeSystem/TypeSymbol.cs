namespace ILInterpreter.Environment.TypeSystem
{

    internal static class TypeSymbol
    {

        public static ITypeSymbol Parse(string symbol)
        {
            string name, assembly;
            SplitAssembly(symbol, out name, out assembly);
            return ParseDirect(name, assembly);
        }

        private static ITypeSymbol ParseDirect(string name, string assembly)
        {
            if (name.EndsWith("*"))
            {
                if (name.Length < 2)
                {
                    return null;
                }
                var element = ParseDirect(name.Substring(0, name.Length - 1), assembly);
                return new PointerSymbol(element);
            }

            return new NameSymbol(name, assembly);
        }

        private static void SplitAssembly(string symbol, out string name, out string assembly)
        {
            var balance = 0;
            for (var i = 0; i < symbol.Length; i++)
            {
                char ch = symbol[i];
                if (ch == '[')
                {
                    balance++;
                }
                else if (ch == ']')
                {
                    if (balance > 0)
                    {
                        balance--;
                    }
                }
                else if (ch == ',')
                {
                    if (balance == 0)
                    {
                        if (i >= symbol.Length - 1)
                        {
                            break;
                        }
                        name = symbol.Substring(0, i);
                        assembly = symbol.Substring(i + 1);
                        return;
                    }
                }
            }
            name = symbol;
            assembly = null;
        }
    }

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
    }

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

    internal abstract class ComponentSymbol : ITypeSymbol
    {

        public readonly ITypeSymbol Element;

        protected ComponentSymbol(ITypeSymbol element) : base(element.AssemblyName)
        {
            Element = element;
        }
    }

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
