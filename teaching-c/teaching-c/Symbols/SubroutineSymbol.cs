using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Symbols
{
    class SubroutineSymbol : Symbol
    {
        public List<VariableSymbol> Parameters { get; set; }
        public bool defined { get; set; }

        public SubroutineSymbol(string Name, Symbol Type, List<VariableSymbol> Parameters = null)
            : base(Name, Type)
        {
            this.defined = true;
            this.Parameters = Parameters;
            if (this.Parameters == null)
            {
                this.Parameters = new List<VariableSymbol>();
            }
        }

        public string param()
        {
            StringBuilder builder = new StringBuilder();
            foreach (VariableSymbol var in this.Parameters)
            {
                builder.Append(var.Type.Name + ",");
            }

            return builder.ToString().TrimEnd(',');
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (VariableSymbol var in this.Parameters)
            {
                builder.Append(var.Type + ":" + var.Name + ", ");
            }

            return String.Format("<{0}('{1}', {2})>",
                this.GetType(),
                this.Name,
                builder.ToString());
        }
    }
}
