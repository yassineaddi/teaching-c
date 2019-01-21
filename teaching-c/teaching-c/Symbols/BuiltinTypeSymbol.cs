using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Symbols
{
    class BuiltinTypeSymbol : Symbol
    {
        public BuiltinTypeSymbol(string Name) : base(Name) { }

        public override string ToString()
        {
            return String.Format("<{0}('{1}')>",
                this.GetType(),
                this.Name);
        }
    }
}
