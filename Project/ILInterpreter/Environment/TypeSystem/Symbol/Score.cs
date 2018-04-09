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
                for (var i = 0; i < genericArguments.Count; i++)
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
                score = GetAssemblyWeakMatchScore(type.AssemblyName, symbol.AssemblyName);
                return true;
            }
            return Result(out score, -1);
        }

        public static int GetAssemblyWeakMatchScore(AssemblyName self, AssemblyName other)
        {
            if (other == null)
            {
                return 0;
            }

            int total = 0;
            FillWeakMatchScore(self.Name, other.Name, ref total);
            FillWeakMatchScore(self.Version.ToString(), other.Version != null ? other.Version.ToString() : null, ref total);
            return total;
        }

        private static void FillWeakMatchScore(string a, string b, ref int score)
        {
            if (b == null)
            {
                return;
            }
            if (a != b)
            {
                score--;
                return;
            }
            score++;
        }
    }
}
