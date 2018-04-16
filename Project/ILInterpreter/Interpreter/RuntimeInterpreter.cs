using ILInterpreter.Environment;
using ILInterpreter.Environment.Method.Runtime;
using ILInterpreter.Interpreter.Stack;
using ILInterpreter.Support;

namespace ILInterpreter.Interpreter
{
    internal sealed unsafe class RuntimeInterpreter
    {
        private ILEnvironment iLEnvironment;

        private readonly RuntimeStack stack;

        public RuntimeInterpreter(ILEnvironment iLEnvironment)
        {
            this.iLEnvironment = iLEnvironment;
            stack = new RuntimeStack();
        }

        public object Invoke(RuntimeMethod method, object instance, object[] parameters)
        {
            var mObjects = stack.ManagedObjects;
            var esp = stack.StackBase;

            return null;
        }

        public void Clear()
        {
            stack.Clear();
        }

    }
}
