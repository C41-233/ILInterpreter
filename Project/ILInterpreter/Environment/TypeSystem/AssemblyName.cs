using System;
using System.IO;
using SystemAssemblyName = System.Reflection.AssemblyName;

namespace ILInterpreter.Environment.TypeSystem
{
    public class AssemblyName
    {

        private readonly SystemAssemblyName name;

        internal AssemblyName(string assembly)
        {
            try
            {
                name = new SystemAssemblyName(assembly);
            }
            catch (FileLoadException e)
            {
                throw new ILTypeLoadException(assembly, e);
            }
        }

        internal AssemblyName(SystemAssemblyName assembly)
        {
            name = assembly;
        }

        public string Name
        {
            get { return name.Name; }
        }

        public Version Version
        {
            get { return name.Version; }
        }

        public override string ToString()
        {
            return name.ToString();
        }
    }
}
