using System;
using System.Runtime.InteropServices;
using ILInterpreter.Support;

namespace ILInterpreter.Interpreter.Stack
{
    internal unsafe class RuntimeStack
    {

        public const int RuntimeStackLength = 1024*16;

        private IntPtr nativePointer;

        public readonly StackObject* StackBase;
        public readonly StackObject* StackEnd;

        public readonly FastList<object> ManagedObjects = new FastList<object>();

        public RuntimeStack()
        {
            nativePointer = Marshal.AllocHGlobal(sizeof(StackObject) * RuntimeStackLength);
            StackBase = (StackObject*) nativePointer.ToPointer();
            StackEnd = StackObject.Plus(StackBase, RuntimeStackLength);
        }

        public void Clear()
        {
            ManagedObjects.Clear();
        }

        ~RuntimeStack()
        {
            if (nativePointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativePointer);
                nativePointer = IntPtr.Zero;
            }
        }

    }
}
