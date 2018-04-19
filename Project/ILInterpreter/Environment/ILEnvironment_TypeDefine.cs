using ILInterpreter.Environment.TypeSystem;

namespace ILInterpreter.Environment
{
    public partial class ILEnvironment
    {

        public ILType Void { get; private set; }

        public ILType Object { get; private set; }

        public ILType Byte { get; private set; }

        public ILType SByte { get; private set; }

        public ILType Char { get; private set; }

        public ILType Short { get; private set; }

        public ILType UShort { get; private set; }

        public ILType Int { get; private set; }

        public ILType UInt { get; private set; }

        public ILType Long { get; private set; }

        public ILType ULong { get; private set; }

        public ILType Float { get; private set; }

        public ILType Double { get; private set; }

        public ILType Decimal { get; private set; }

        public ILType String { get; private set; }

        public ILType ValueType { get; private set; }

        public ILType Enum { get; private set; }

        public ILType Array { get; private set; }

    }
}
