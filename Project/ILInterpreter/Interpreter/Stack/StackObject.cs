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
        public long Int64;

        [FieldOffset(2)]
        public long Value;

        public static void PushObject(StackObject* esp, FastList<object> mObjects, object instance)
        {
            if (instance == null)
            {
                PushNull(esp);
            }
            else
            {
                esp->ObjectType = StackObjectType.Object;
                esp->Int32 = mObjects.Push(instance);
            }
        }

        public static void InitObject(StackObject* esp, FastList<object> mObjects)
        {
            esp->ObjectType = StackObjectType.Object;
            esp->Int32 = mObjects.Push(null);
        }


        public static void PushNull(StackObject* esp)
        {
            esp->ObjectType = StackObjectType.Null;
            esp->Int64 = 0;
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

        public static void Free(StackObject* esp, FastList<object> mObjects)
        {
            if (esp->ObjectType == StackObjectType.Object)
            {
                mObjects.Pop();
            }
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
