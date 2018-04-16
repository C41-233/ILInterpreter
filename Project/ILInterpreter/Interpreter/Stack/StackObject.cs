using System.Runtime.InteropServices;

namespace ILInterpreter.Interpreter.Stack
{

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct StackObject
    {

        [FieldOffset(0)]
        public StackObjectType ObjectType;

        [FieldOffset(2)]
        public int Int32;

        [FieldOffset(2)]
        public int Int64;

        public static StackObject* Plus(StackObject* pointer)
        {
            return pointer + 1;
        }

        //because of unity error
        public static StackObject* Plus(StackObject* pointer, int step)
        {
            return (StackObject*)((long)pointer + sizeof(StackObject) * step);
        }

    }
}
