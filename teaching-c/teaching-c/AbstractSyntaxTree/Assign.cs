using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Assign : AbstractSyntaxTree
    {
        public Variable Left { get; set; }
        public Token Operator { get; set; }
        public object Right { get; set; }

        public Assign(Variable Left, Token Token, object Right)
        {
            this.Left = Left;
            this.Token = this.Operator = Token;
            this.Right = Right;
        }
    }
}
