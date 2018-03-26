using System;
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

        public override string ToString()
        {
            return name.ToString();
        }
    }
}
