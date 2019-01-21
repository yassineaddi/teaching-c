using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class CompoundStatement : AbstractSyntaxTree
    {
        public Compound Body { get; set; }

        public CompoundStatement(Token Token)
        {
            this.Token = Token;
            this.Body = new Compound();
        }
    }
}
