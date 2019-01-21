using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class ConditionalStatement : AbstractSyntaxTree
    {
        public Compound If { get; set; }
        public Compound Else { get; set; }
        public object Expression { get; set; }

        public ConditionalStatement(Token Token, object Expression, Compound If, Compound Else = null)
        {
            this.Token = Token;
            this.If = If;
            this.Else = Else;
            this.Expression = Expression;
        }
    }
}
