using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class ReturnStatement : AbstractSyntaxTree
    {
        public object Expression { get; set; }

        public ReturnStatement(Token Token, object Expression)
        {
            this.Token = Token;
            this.Expression = Expression;
        }
    }
}
