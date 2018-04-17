using System;
using System.Runtime.InteropServices;
using ILInterpreter.Environment;
using ILInterpreter.Support;

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


        public static StackObject* PushObject(StackObject* esp, FastList<object> mObjects, object instance)
        {
            if (instance == null)
            {
                return PushNull(esp);
            }
            esp->ObjectType = StackObjectType.Object;
            esp->Int32 = mObjects.Count;
            mObjects.Add(instance);
            return esp + 1;
        }

        public static StackObject* PushNull(StackObject* esp)
        {
            esp->ObjectType = StackObjectType.Null;
            esp->Int64 = 0;
            return esp + 1;
        }

        public static object ToObject(StackObject* esp, ILEnvironment env, FastList<object> mObjects)
        {
            switch (esp->ObjectType)
            {
                case StackObjectType.Object:
                    return mObjects[esp->Int32];
            }
            throw new NotSupportedException();
        }

        public static StackObject* Plus(StackObject* pointer)
        {
            return pointer + 1;
        }

        public static StackObject* Minus(StackObject* pointer)
        {
            return pointer - 1;
        }

        //because of unity error
        public static StackObject* Plus(StackObject* pointer, int step)
        {
            return (StackObject*)((long)pointer + sizeof(StackObject) * step);
        }

        //because of unity error
        public static StackObject* Minus(StackObject* pointer, int step)
        {
            return (StackObject*)((long)pointer - sizeof(StackObject) * step);
        }

    }
}
