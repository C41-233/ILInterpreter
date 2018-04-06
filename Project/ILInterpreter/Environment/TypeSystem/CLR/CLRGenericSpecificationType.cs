using System;
using System.Text;
using ILInterpreter.Support;

namespace ILInterpreter.Environment.TypeSystem.CLR
{
    internal abstract partial class CLRType
    {

        private sealed class CLRGenericSpecificationType : CLRType
        {

            public CLRGenericSpecificationType(ILType genericTypeDefinition, FastList<ILType> genericArguments, Type type, ILEnvironment env) : base(type, env)
            {
                this.genericTypeDefinition = genericTypeDefinition;
                this.genericArguments = genericArguments;
            }

            public override bool IsGenericType
            {
                get { return true; }
            }

            private readonly ILType genericTypeDefinition;
            private readonly FastList<ILType> genericArguments;

            public override ILType GenericTypeDefinition
            {
                get { return genericTypeDefinition; }
            }

            public override IListView<ILType> GenericArguments
            {
                get { return genericArguments; }
            }

            private string fullname;
            public override string FullName
            {
                get
                {
                    if (fullname == null)
                    {
                        var sb = new StringBuilder();
                        sb.Append(genericTypeDefinition.FullName);
                        sb.Append(genericArguments.ToJoinString("[", "]", ",", type=>type.FullName));
                        fullname = sb.ToString();
                    }
                    return fullname;
                }
            }

            private string fullQualifiedName;
            public override string FullQulifiedName
            {
                get
                {
                    if (fullQualifiedName == null)
                    {
                        var sb = new StringBuilder();
                        sb.Append(genericTypeDefinition.FullQulifiedName);
                        sb.Append(genericArguments.ToJoinString("[", "]", ",", type => "["+type.AssemblyQualifiedName+"]"));
                        fullQualifiedName = sb.ToString();
                    }
                    return fullQualifiedName;
                }
            }
        }

    }
}
