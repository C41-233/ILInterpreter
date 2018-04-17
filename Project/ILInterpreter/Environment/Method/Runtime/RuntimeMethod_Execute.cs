using System.Collections.Generic;
using ILInterpreter.Environment.TypeSystem;
using ILInterpreter.Support;
using Mono.Cecil;

namespace ILInterpreter.Environment.Method.Runtime
{
    internal sealed partial class RuntimeMethod
    {

        public bool HasBody
        {
            get { return definition.HasBody; }
        }

        private Instruction[] body;

        public Instruction[] Body
        {
            get
            {
                CheckInitBody();
                return body;
            }
        }

        public int LocalVariableCount
        {
            get
            {
                CheckDefinitionInit();
                return localVariables.Count;
            }
        }

        private FastList<ILType> localVariables;

        public IListView<ILType> LocalVariables
        {
            get
            {
                CheckDefinitionInit();
                return localVariables;
            }
        }

        public override bool HasThis
        {
            get { return definition.HasThis; }
        }

        private bool isBodyInit;

        private void CheckInitBody()
        {
            if (isBodyInit)
            {
                return;
            }
            lock (Environment)
            {
                if (isBodyInit)
                {
                    return;
                }
                InitBody();
                isBodyInit = true;
            }
        }

        private void InitBody()
        {
            if (HasBody == false)
            {
                return;
            }

            var variables = definition.Body.Variables;
            localVariables = new FastList<ILType>(variables.Count);
            foreach (var variable in variables)
            {
                localVariables.Add(Environment.GetType(variable.VariableType));
            }

            var instructions = definition.Body.Instructions;
            var address = new Dictionary<Mono.Cecil.Cil.Instruction, int>();

            body = new Instruction[instructions.Count];
            for (var i=0; i < body.Length; i++)
            {
                var instruction = instructions[i];
                address[instruction] = i;
            }

            for (var i=0; i < body.Length; i++)
            {
                var instruction = instructions[i];
                body[i] = Instruction.Create(instruction.OpCode.Code, instruction.Operand, Environment, address);
            }

            definition.Body = null;
        }

    }
}
