using System.Text;

namespace ILInterpreter.Environment.TypeSystem.Symbol
{
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
}