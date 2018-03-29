namespace ILInterpreter.Environment.TypeSystem.Symbol
{
    internal static class Score
    {

        private static bool Result(out int target, int value)
        {
            target = value;
            return value >= 0;
        }

        public static bool GetTypeWeakMatchScore(ILType type, ITypeSymbol symbol, out int score)
        {
            var genericSymbol = symbol as GenericSymbol;
            if (genericSymbol != null)
            {
                int totalScore;
                if (!GetTypeWeakMatchScore(type.GenericTypeDefinition, genericSymbol.Element, out totalScore))
                {
                    return Result(out score, -1);
                }
                var genericArguments = type.GenericArguments;
                for (var i = 0; i < genericArguments.Length; i++)
                {
                    int genericScore;
                    if (!GetTypeWeakMatchScore(genericArguments[i], genericSymbol.GenericParameters[i], out genericScore))
                    {
                        return Result(out score, -1);
                    }
                    totalScore += genericScore;
                }
                return Result(out score, totalScore);
            }

            var componentSymbol = symbol as ComponentSymbol;
            if (componentSymbol != null)
            {
                return GetTypeWeakMatchScore(type.ElementType, componentSymbol.Element, out score);
            }

            var nameSymbol = symbol as NameSymbol;
            if (nameSymbol != null)
            {
                if (type.FullName != nameSymbol.Name)
                {
                    return Result(out score, -1);
                }
                return GetAssemblyWeakMatchScore(type.AssemblyName, symbol.AssemblyName, out score);
            }
            return Result(out score, -1);
        }

        public static bool GetAssemblyWeakMatchScore(AssemblyName self, AssemblyName other, out int score)
        {
            if (other == null)
            {
                return Result(out score, 0);
            }

            int total = 0;
            if (!FillWeakMatchScore(self.Name, other.Name, ref total))
            {
                score = -1;
                return false;
            }
            score = total;
            return true;
        }

        private static bool FillWeakMatchScore(string a, string b, ref int score)
        {
            if (b == null)
            {
                return true;
            }
            if (a != b)
            {
                return false;
            }
            score++;
            return true;
        }
    }
}
