using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Array : AbstractSyntaxTree
    {
        public Variable Identifier { get; set; }
        public Type Type { get; set; }
        public object Size { get; set; }
        public List<object> Elements { get; set; }

        public Array(Token Token, Type Type, object Size = null, List<object> Elements = null)
        {
            this.Token = Token;
            this.Type = Type;
            this.Size = Size;
            if (null == Elements)
            {
                this.Elements = new List<object>();
            }
            else
            {
                this.Elements = Elements;
            }
        }
    }
}
