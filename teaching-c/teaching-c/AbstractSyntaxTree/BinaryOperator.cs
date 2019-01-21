using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class BinaryOperator : AbstractSyntaxTree
    {
        public object Left { get; set; }
        public Token Operator { get; set; }
        public object Right { get; set; }

        public BinaryOperator(object Left, Token Operator, object Right)
        {
            this.Left = Left;
            this.Token = this.Operator = Operator;
            this.Right = Right;
        }
    }
}
