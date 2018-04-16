namespace ILInterpreter.Interpreter.Stack
{

    internal enum StackObjectType : short
    {
        Null,
        Int32,
        Int64,
        Float32,
        Float64,
        Object,
        CLRObjectReference,
        StackObjectReference,
    }

}
