using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class VariableDeclaration : AbstractSyntaxTree
    {
        public Variable VariableNode { get; set; }
        public Type TypeNode { get; set; }

        public VariableDeclaration(Variable VariableNode, Type TypeNode)
        {
            this.Token = VariableNode.Token;
            this.VariableNode = VariableNode;
            this.TypeNode = TypeNode;
        }
    }
}
