using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ILInterpreter.Interpreter;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ILInterpreter.Environment.Method.Runtime
{

    [StructLayout(LayoutKind.Explicit)]
    internal struct Instruction
    {

        [FieldOffset(0)]
        public Code Code;

        [FieldOffset(4)]
        public int Int32;

        [FieldOffset(4)]
        public long Int64;

        [FieldOffset(4)]
        public int High32;

        [FieldOffset(8)]
        public int Low32;

        public static Instruction Create(Code code, object operand, ILEnvironment env, Dictionary<Mono.Cecil.Cil.Instruction, int> address)
        {
            var instruction = new Instruction
            {
                Code = code,
            };
            switch (code)
            {
                #region jump
                case Code.Leave:
                case Code.Leave_S:
                case Code.Br:
                case Code.Br_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Beq:
                case Code.Beq_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                    instruction.Int32 = address[(Mono.Cecil.Cil.Instruction)operand];
                    break;
                #endregion

                #region ldc
                case Code.Ldc_I4:
                    instruction.Int32 = (int)operand;
                    break;
                case Code.Ldc_I4_S:
                    instruction.Int32 = (sbyte)operand;
                    break;
                case Code.Ldc_I8:
                    instruction.Int64 = (long)operand;
                    break;
                case Code.Ldstr:
                    StringPool.Put((string) operand, out instruction.High32, out instruction.Low32);
                    break;
                    #endregion

                #region call
                case Code.Newobj:
                case Code.Call:
                    var methodReference = (MethodReference) operand;
                    var type = env.GetType(methodReference.DeclaringType);
                    var method = type.GetDeclaredMethod(methodReference);
                    instruction.High32 = type.GetHashCode();
                    instruction.Low32 = method.GetHashCode();
                    break;
                #endregion
            }
            return instruction;
        }

    }
}
