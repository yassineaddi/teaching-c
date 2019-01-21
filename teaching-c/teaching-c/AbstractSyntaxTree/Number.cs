using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Number : AbstractSyntaxTree
    {
        public object Value { get; set; }

        public Number(Token Token)
        {
            this.Token = Token;
            this.Value = Token.Value;
        }
    }
}
