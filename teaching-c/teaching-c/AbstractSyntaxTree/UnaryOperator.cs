using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class UnaryOperator : AbstractSyntaxTree
    {
        public Token Operator { get; set; }
        public object Expression { get; set; }

        public UnaryOperator(Token Operator, object Expression)
        {
            this.Token = this.Operator = Operator;
            this.Expression = Expression;
        }
    }
}
