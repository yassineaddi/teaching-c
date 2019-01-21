using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Symbols
{
    class Symbol
    {
        public string Name { get; set; }
        public Symbol Type { get; set; }

        public Symbol(string Name, Symbol Type = null)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }
}
