using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ILInterpreter.Environment;
using ILInterpreter.Environment.Method.Runtime;
using ILInterpreter.Interpreter.Stack;
using ILInterpreter.Support;
using Mono.Cecil.Cil;
using Instruction = ILInterpreter.Environment.Method.Runtime.Instruction;

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
            clrArguments.Add(0, null);
        }

        public object Invoke(RuntimeMethod method, object instance, object[] parameters)
        {
            var mObjects = stack.ManagedObjects;
            var esp = stack.StackBase;

            //push this
            if (method.HasThis)
            {
                esp = StackObject.PushObject(esp, mObjects, instance);
            }

            //push parameters
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    esp = StackObject.PushObject(esp, mObjects, parameter);
                }
            }

            bool unhandledException;
            esp = Execute(method, esp, out unhandledException);

            return null;
        }

        private StackObject* Execute(RuntimeMethod method, StackObject* esp, out bool unhandledException)
        {
            var env = method.Environment;
            var mObjects = stack.ManagedObjects;
            var body = method.Body;
            var localVariableCount = method.LocalVariableCount;
            var localVariables = method.LocalVariables;

            unhandledException = false;

            for (var i = 0; i < localVariableCount; i++)
            {
                var variable = localVariables[i];

            }

            fixed (Instruction* methodBase = body)
            {
                var ip = methodBase;
                while (true)
                {
                    switch (ip->Code)
                    {
                        case Code.Nop:
                        {
                            break;
                        }
                        case Code.Ret:
                        {
                            goto Return;
                        }

                        #region ldc
                        case Code.Ldstr:
                        {
                            var value = StringPool.Get(ip->High32, ip->Low32);
                            esp = StackObject.PushObject(esp, mObjects, value);
                            break;
                        }
                        #endregion

                        #region call
                        case Code.Call:
                        {
                            var type = env.GetType(ip->High32);
                            var call = type.GetDeclaredMethod(ip->Low32);
                            var argumentCount = call.ParametersCount;
                            var arguments = GetClrArguments(argumentCount);
                            var pArgument = StackObject.Minus(esp);
                            for (var i=0; i<argumentCount; i++)
                            {
                                arguments[i] = StackObject.ToObject(pArgument, env, mObjects);
                                pArgument++;
                            }
                            object instance = null;
                            if (call.HasThis)
                            {
                                instance = StackObject.ToObject(pArgument - 1, env, mObjects);
                            }
                            var rst = call.Invoke(instance, arguments);
                            break;
                        }
                        #endregion
                    }
                    ip++;
                }
                Return:
                ;
            }
            return esp;
        }

        public void Clear()
        {
            stack.Clear();
        }

        private readonly Dictionary<int, object[]> clrArguments = new Dictionary<int, object[]>();

        private object[] GetClrArguments(int count)
        {
            object[] args;
            if (!clrArguments.TryGetValue(count, out args))
            {
                args = new object[count];
                clrArguments.Add(count, args);
            }
            return args;
        }

    }
}
