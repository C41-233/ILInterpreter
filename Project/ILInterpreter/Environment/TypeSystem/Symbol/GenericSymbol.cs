using System.Text;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem.Symbol
{
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
                    sb.Append(GenericParameters.ToJoinString("[", "]", ",", parameter=>parameter.FullName));
                    fullname = sb.ToString();
                    count = GenericParameters.Count;
                }
                return fullname;
            }
        }
    }
}