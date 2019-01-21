using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Param : AbstractSyntaxTree
    {
        public Variable VariableNode { get; set; }
        public Type TypeNode { get; set; }

        public Param(Variable VariableNode, Type TypeNode)
        {
            this.VariableNode = VariableNode;
            this.TypeNode = TypeNode;

            if (null != VariableNode)
            {
                this.Token = VariableNode.Token;
            }
        }
    }
}
