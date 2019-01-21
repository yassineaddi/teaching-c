using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class ArrayElement : AbstractSyntaxTree
    {
        public Variable Identifier { get; set; }
        public object Subscript { get; set; }

        public ArrayElement(Token Token, Variable Identifier, object Subscript)
        {
            this.Token = Token;
            this.Identifier = Identifier;
            this.Subscript = Subscript;
        }
    }
}
