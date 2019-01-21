using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class SubroutineDeclaration : AbstractSyntaxTree
    {
        public Variable SubroutineName { get; set; }
        public Type ReturnType { get; set; }
        public List<Param> Params { get; set; }
        public Compound Body { get; set; }

        public SubroutineDeclaration(Variable SubroutineName, Type ReturnType, List<Param> Params, Compound Body = null)
        {
            this.Token = SubroutineName.Token;
            this.SubroutineName = SubroutineName;
            this.ReturnType = ReturnType;
            this.Params = Params;
            this.Body = Body;
        }

        public string param()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Param parameter in this.Params)
            {
                builder.Append(parameter.TypeNode.Token.Value + ",");
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}
