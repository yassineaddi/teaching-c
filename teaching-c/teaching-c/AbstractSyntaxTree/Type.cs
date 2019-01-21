using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Type : AbstractSyntaxTree
    {
        public object Value { get; set; }
        public Type(Token Token)
        {
            this.Token = Token;
            this.Value = this.Token.Value;
        }
    }
}
