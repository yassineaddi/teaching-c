using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class ArrayAssign : AbstractSyntaxTree
    {
        public Variable Left { get; set; }
        public object Subscript { get; set; }
        public Token Operator { get; set; }
        public object Right { get; set; }

        public ArrayAssign(Variable Left, Token Token, object Subscript, object Right)
        {
            this.Left = Left;
            this.Subscript = Subscript;
            this.Token = this.Operator = Token;
            this.Right = Right;
        }
    }
}
