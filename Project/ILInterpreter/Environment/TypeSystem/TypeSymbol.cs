using System;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem
{

    internal static class TypeSymbol
    {

        public static ITypeSymbol Parse(string symbol)
        {
            if (symbol.EndsWith("*"))
            {
                return new PointerSymbol(
                    Parse(symbol.Substring(0, symbol.Length - 1))
                );
            }
            if (symbol.EndsWith("&"))
            {
                return new RefSymbol(
                    Parse(symbol.Substring(0, symbol.Length - 1))
                );
            }
            if (symbol.EndsWith("]"))
            {
                var rank = 1;
                for (var i=symbol.Length-2; i>=0; i--)
                {
                    var ch = symbol[i];
                    if (ch == '[')
                    {
                        return new ArraySymbol(
                            Parse(symbol.Substring(0, i)),
                            rank
                        );
                    }
                    if (ch == ',')
                    {
                        rank++;
                        continue;
                    }
                    break;
                }
            }

            if (symbol.EndsWith("]"))
            {
                var lead = symbol.IndexOf("[", StringComparison.Ordinal);
                var type = new GenerocSymbol(new ClassSymbol(symbol.Substring(0, lead)));

                var start = lead + 1;
                var end = symbol.Length - 2;
                var balance = 0;
                var tokenStart = start;
                for (var i = start; i <= end; i++)
                {
                    var ch = symbol[i];
                    if (ch == '[')
                    {
                        balance++;
                    }
                    else if (ch == ']')
                    {
                        balance--;
                    }
                    else if(ch == ',' && balance == 0)
                    {
                        type.Generics.Add(Parse(symbol.Substring(tokenStart, i-tokenStart)));
                        tokenStart = i + 1;
                    }
                }
                type.Generics.Add(Parse(symbol.Substring(tokenStart, end-tokenStart+1)));
                return type;
            }

            return new ClassSymbol(symbol);
        }

    }

    internal interface ITypeSymbol
    {
    }

    internal sealed class ClassSymbol : ITypeSymbol
    {
        public readonly string Name;

        public ClassSymbol(string name)
        {
            Name = name;
        }


        public override string ToString()
        {
            return Name;
        }
    }

    internal abstract class ComponentSymbol : ITypeSymbol
    {
        public readonly ITypeSymbol ElementType;

        protected ComponentSymbol(ITypeSymbol element)
        {
            ElementType = element;
        }
    }

    internal sealed class GenerocSymbol : ComponentSymbol
    {
        public readonly FastList<ITypeSymbol> Generics = new FastList<ITypeSymbol>();

        public GenerocSymbol(ITypeSymbol element) : base(element)
        {
        }

        private string cachedSymbol;

        public override string ToString()
        {
            return cachedSymbol ?? (cachedSymbol = ElementType + Generics.Join(",", "[", "]"));
        }
    }

    internal sealed class ArraySymbol : ComponentSymbol
    {
        public readonly int Rank;

        public ArraySymbol(ITypeSymbol element, int rank) : base(element)
        {
            Rank = rank;
        }
    }

    internal sealed class RefSymbol : ComponentSymbol
    {
        public RefSymbol(ITypeSymbol element) : base(element)
        {
        }

        public override string ToString()
        {
            return ElementType + "&";
        }
    }

    internal sealed class PointerSymbol : ComponentSymbol
    {
        public PointerSymbol(ITypeSymbol element) : base(element)
        {
        }

        public override string ToString()
        {
            return ElementType + "*";
        }
    }

}
