using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Symbol;
using ILInterpreter.Support;

namespace ILInterpreter.Environment
{
    public sealed partial class ILEnvironment
    {

        private ILType GetTypeInternal(ITypeSymbol symbol)
        {
            var fullname = symbol.FullName;
            IListView<ILType> list;
            if (!NameToTypes.TryGetValues(fullname, out list))
            {
                return CreateTypeInternal(symbol);
            }

            int currentScore = -1;
            ILType currentType = null;
            foreach (var type in list)
            {
                int score;
                if (Score.GetTypeWeakMatchScore(type, symbol, out score))
                {
                    if (score > currentScore)
                    {
                        currentScore = score;
                        currentType = type;
                    }
                }
            }
            return currentType;
        }

        private ILType CreateTypeInternal(ITypeSymbol symbol)
        {
            //CacheTypeInternal(type);
            return null;
        }

    }
}
