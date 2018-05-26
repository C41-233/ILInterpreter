using System.Collections.Generic;
using ILInterpreter.Environment;
using ILInterpreter.Environment.Method.Runtime;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Environment.TypeSystem.Runtime;
using ILInterpreter.Interpreter.Stack;
using ILInterpreter.Interpreter.Type;
using ILInterpreter.Support;
using Mono.Cecil.Cil;
using Instruction = ILInterpreter.Environment.Method.Runtime.Instruction;

namespace ILInterpreter.Interpreter
{
    internal sealed unsafe class RuntimeInterpreter
    {
        private readonly ILEnvironment environment;

        private readonly RuntimeStack stack;

        public RuntimeInterpreter(ILEnvironment environment)
        {
            this.environment = environment;
            stack = new RuntimeStack();
            clrArguments.Add(0, null);
        }

        public object Invoke(RuntimeMethod method, object instance, object[] parameters)
        {
            try
            {
                var env = environment;
                var mObjects = stack.ManagedObjects;
                var esp = stack.StackBase;

                //push this
                if (method.HasThis)
                {
                    StackObject.PushObject(esp, mObjects, instance);
                    esp++;
                }

                //check parameters count
                if (method.ParametersCount != parameters.Length)
                {
                    throw new ILInvokeException("invoke method {0} parameters count mismatch expected={1} actual={2}", method.Name, method.ParametersCount, parameters.Length);
                }

                //push parameters
                foreach (var parameter in parameters)
                {
                    StackObject.PushObject(esp, mObjects, parameter);
                    esp++;
                }

                bool unhandledException;
                esp = Execute(method, esp, out unhandledException);

                if (method.ReturnType == env.Void)
                {
                    return null;
                }

                return StackObject.ToObject(esp, env, mObjects);
            }
            finally
            {
                stack.Clear();
            }
        }

        private StackObject* Execute(RuntimeMethod method, StackObject* esp, out bool unhandledException)
        {
            var env = environment;
            var mObjects = stack.ManagedObjects;
            var body = method.Body;

            var pFrameBase = StackObject.Minus(esp, method.HasThis ? method.ParametersCount + 1 : method.ParametersCount);
            var mObjectBase = mObjects.Count;

            var pArgument0 = pFrameBase;

            var localVariableCount = method.LocalVariableCount;
            var localVariables = method.LocalVariables;
            var pLocalVariable0 = esp;

            unhandledException = false;

            //push localVariable
            for (var i = 0; i < localVariableCount; i++)
            {
                StackObject.InitObject(esp, mObjects);
                esp++;
            }

            fixed (Instruction* methodBase = body)
            {
                var ip = methodBase;

                #region instruction loop
                while (true)
                {
                    switch (ip->Code)
                    {
                        #region base
                        case Code.Nop:
                        {
                            break;
                        }
                        case Code.Ret:
                        {
                            goto Return;
                        }
                        #endregion

                        #region ldc
                        case Code.Ldstr:
                        {
                            var value = StringPool.Get(ip->High32, ip->Low32);
                            StackObject.PushObject(esp, mObjects, value);
                            esp++;
                            break;
                        }
                        #endregion

                        #region ldarg
                        case Code.Ldarg_0:
                        {
                            Copy(esp, pArgument0);
                            break;
                        }
                        #endregion

                        #region ldloc
                        case Code.Ldloc_0:
                        {
                            Copy(esp, pLocalVariable0);
                            esp++;
                            break;
                        }
                        #endregion

                        #region stloc
                        case Code.Stloc_0:
                        {
                            esp--;
                            Stloc(pLocalVariable0, esp);
                            break;
                        }
                        #endregion

                        #region jump
                        case Code.Br:
                        case Code.Br_S:
                        {
                            ip = methodBase + ip->Int32;
                            break;
                        }
                        #endregion

                        #region call
                        case Code.Newobj:
                        {
                            var callType = (RuntimeType)env.GetType(ip->High32);
                            var callConstructor = (RuntimeMethod)callType.GetDeclaredMethod(ip->Low32);
                            var obj = new RuntimeTypeInstance(callType);
                            StackObject.PushObject(esp, mObjects, obj);
                            esp++;
                            esp = Execute(callConstructor, esp, out unhandledException);
                            StackObject.PushObject(esp, mObjects, obj);
                            esp++;
                            break;
                        }
                        case Code.Call:
                        {
                            var callType = env.GetType(ip->High32);
                            var callMethod = callType.GetDeclaredMethod(ip->Low32);

                            //不需要执行new object()
                            if (callMethod == env.ObjectConstructor)
                            {
                                continue;
                            }

                            var argumentCount = callMethod.ParametersCount;
                            var arguments = GetClrArguments(argumentCount);
                            var pArgument = StackObject.Minus(esp);
                            for (var i=0; i<argumentCount; i++)
                            {
                                arguments[i] = StackObject.ToObject(pArgument, env, mObjects);
                                pArgument++;
                            }
                            object instance = null;
                            if (callMethod.HasThis)
                            {
                                instance = StackObject.ToObject(pArgument - 1, env, mObjects);
                            }
                            var rst = callMethod.Invoke(instance, arguments);
                            if (callMethod.ReturnType != env.Void)
                            {
                                StackObject.PushObject(esp, mObjects, rst);
                            }
                            break;
                        }
                        case Code.Callvirt:
                        {
                            //var callType = env.GetType(ip->High32);
                            //var callM

                            //var pInstance = esp - 1;
                            //var instance = mObjects[pInstance->Int32];
                            ////var callMethod = ((RuntimeTypeInstance) instance).RuntimeType.GetVirtualMethod(ip->Low32);

                            //var argumentCount = callMethod.ParametersCount;
                            //var arguments = GetClrArguments(argumentCount);
                            //var pArgument = StackObject.Minus(esp);
                            //for (var i = 0; i < argumentCount; i++)
                            //{
                            //    arguments[i] = StackObject.ToObject(pArgument, env, mObjects);
                            //    pArgument++;
                            //}
                            //var rst = callMethod.Invoke(instance, arguments);
                            //if (callMethod.ReturnType != env.Void)
                            //{
                            //    StackObject.PushObject(esp, mObjects, rst);
                            //}
                            break;
                        }
                        #endregion

                        default:
                        {
                            throw new ILInvokeException("Unrecognized instruction value={0}", ip->Code);
                        }
                    }
                    ip++;
                }
                Return:;
                #endregion
            }

            //recover esp
            if (method.ReturnType != env.Void)
            {
                var pReturn = esp - 1;
                esp = pFrameBase;
                esp->ObjectType = pReturn->ObjectType;
                mObjects[mObjectBase] = mObjects[pReturn->Int32];
                esp++;
                mObjectBase++;
            }
            mObjects.RemoveFrom(mObjectBase);
            return esp;
        }

        private void Stloc(StackObject* dst, StackObject* src)
        {
            var mObjects = stack.ManagedObjects;
            mObjects[dst->Int32] = mObjects[dst->Int32];
        }

        private void Copy(StackObject* dst, StackObject* src)
        {
            dst->ObjectType = src->ObjectType;

            if (dst->ObjectType == StackObjectType.Object)
            {
                var mObjects = stack.ManagedObjects;
                dst->Int32 = mObjects.Push(mObjects[src->Int32]);
            }
            else
            {
                dst->Value = src->Value;
            }
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
