using System.Text;
using ILInterpreter.Support;

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

        private static ITypeSymbol ParseFull(string name)
        {
            if (name.StartsWith("[") && name.EndsWith("]"))
            {
                name = name.Substring(1, name.Length - 2);
            }
            return Parse(name);
        }

        private static ITypeSymbol ParseDirect(string name, string assembly)
        {
            if (name.EndsWith("*"))
            {
                if (name.Length < 2)
                {
                    throw new ILTypeLoadException(name);
                }
                var element = ParseDirect(name.Substring(0, name.Length - 1), assembly);
                return new PointerSymbol(element);
            }
            if (name.EndsWith("&"))
            {
                if (name.Length < 2)
                {
                    throw new ILTypeLoadException(name);
                }
                var element = ParseDirect(name.Substring(0, name.Length - 1), assembly);
                return new RefSymbol(element);
            }
            var arraySymbol = TryParseArray(name, assembly);
            if (arraySymbol != null)
            {
                return arraySymbol;
            }

            var genericSymbol = TryParseGeneric(name, assembly);
            if (genericSymbol != null)
            {
                return genericSymbol;
            }

            return new NameSymbol(name, assembly);
        }

        private static ArraySymbol TryParseArray(string name, string assembly)
        {
            if (!name.EndsWith("]"))
            {
                return null;
            }

            var iLeft = name.LastIndexOf('[');
            if (iLeft <= 0)
            {
                throw new ILTypeLoadException(name);
            }

            var rank = 1;
            for (var i = iLeft + 1; i < name.Length - 1; i++)
            {
                if (name[i] != ',')
                {
                    return null;
                }
                rank++;
            }
            return new ArraySymbol(
                ParseDirect(name.Substring(0, iLeft), assembly),
                rank
            );
        }

        private static GenericSymbol TryParseGeneric(string name, string assembly)
        {
            if (!name.EndsWith("]"))
            {
                return null;
            }
            var iLeft = name.IndexOf('[');
            if (iLeft <= 0)
            {
                throw new ILTypeLoadException(name);
            }

            var element = ParseDirect(name.Substring(0, iLeft), assembly);
            var type = new GenericSymbol(element);
            var balance = 0;
            var start = iLeft + 1;
            for (int i = start; i < name.Length - 1; i++)
            {
                var ch = name[i];
                if (ch == '[')
                {
                    balance++;
                }
                else if (ch == ']')
                {
                    balance--;
                }
                else if (ch == ',' && balance == 0)
                {
                    type.GenericParameters.Add(ParseFull(name.Substring(start, i - start)));
                    start = i + 1;
                }
            }

            type.GenericParameters.Add(ParseFull(name.Substring(start, name.Length - start - 1)));
            return type;
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
                    else
                    {
                        throw new ILTypeLoadException(symbol);
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

            if (balance != 0)
            {
                throw new ILTypeLoadException(symbol);
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

        public override string ToString()
        {
            return FullName;
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

    internal sealed class ArraySymbol : ComponentSymbol
    {

        public readonly int Rank;

        public ArraySymbol(ITypeSymbol element, int rank) : base(element)
        {
            Rank = rank;
        }

        private string fullname;
        public override string FullName
        {
            get
            {
                if (fullname == null)
                {
                    var sb = new StringBuilder();
                    sb.Append(Element.FullName);
                    sb.Append('[');
                    for (var i = 0; i < Rank - 1; i++)
                    {
                        sb.Append(',');
                    }
                    sb.Append(']');
                    fullname = sb.ToString();
                }
                return fullname;
            }
        }
    }

    internal sealed class GenericSymbol : ComponentSymbol
    {

        public readonly FastList<ITypeSymbol> GenericParameters = new FastList<ITypeSymbol>();

        public GenericSymbol(ITypeSymbol element) : base(element)
        {
        }

        private string fullname;
        private int count;
        public override string FullName
        {
            get
            {
                if (fullname == null || count != GenericParameters.Count)
                {
                    var sb = new StringBuilder();
                    sb.Append(Element.FullName);
                    sb.Append('[');
                    for (var i = 0; i < GenericParameters.Count; i++)
                    {
                        sb.Append(GenericParameters[i].FullName);
                        if (i < GenericParameters.Count - 1)
                        {
                            sb.Append(',');
                        }
                    }
                    sb.Append(']');

                    fullname = sb.ToString();
                    count = GenericParameters.Count;
                }
                return fullname;
            }
        }
    }

}
