using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Call : AbstractSyntaxTree
    {
        public Variable SubroutineName { get; set; }
        public List<object> Params { get; set; }

        public Call(Variable SubroutineName, List<object> Params)
        {
            this.Token = SubroutineName.Token;
            this.SubroutineName = SubroutineName;
            this.Params = Params;
        }

        public string param()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Token parameter in this.Params)
            {
                builder.Append(Enum.GetName(typeof(Tokens), parameter.Type) + ",");
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}
