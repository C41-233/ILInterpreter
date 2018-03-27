using SystemAssemblyName = System.Reflection.AssemblyName;

namespace ILInterpreter.Environment.TypeSystem
{
    public class AssemblyName
    {

        private readonly SystemAssemblyName name;

        internal AssemblyName(string assembly)
        {
            name = new SystemAssemblyName(assembly);
        }

        internal AssemblyName(SystemAssemblyName assembly)
        {
            name = assembly;
        }

        public string Name
        {
            get { return name.Name; }
        }

        internal int GetWeakMatchScore(AssemblyName other)
        {
            if (other == null)
            {
                return 0;
            }

            int total = 0;
            if (!FillWeakMatchScore(Name, other.Name, ref total))
            {
                return -1;
            }
            return total;
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

        public override string ToString()
        {
            return name.ToString();
        }
    }
}
