using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Symbols
{
    class VariableSymbol : Symbol
    {
        public bool initialized { get; set; }

        public VariableSymbol(string Name, Symbol Type) : base(Name, Type)
        {
            this.initialized = false;
        }

        public override string ToString()
        {
            return String.Format("<{0}('{1}', '{2}')>",
                this.GetType(),
                this.Name,
                this.Type);
        }
    }
}
