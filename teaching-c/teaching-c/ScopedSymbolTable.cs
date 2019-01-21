using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c
{
    class ScopedSymbolTable
    {
        public Dictionary<string, Symbols.Symbol> Symbols { get; set; }
        public string ScopeName { get; set; }
        public int ScopeLevel { get; set; }
        public ScopedSymbolTable EnclosingScope { get; set; }

        public ScopedSymbolTable(string ScopeName, int ScopeLevel, ScopedSymbolTable EnclosingScope = null)
        {
            this.Symbols = new Dictionary<string, Symbols.Symbol>();
            this.ScopeName = ScopeName;
            this.ScopeLevel = ScopeLevel;
            this.EnclosingScope = EnclosingScope;
        }

        public void InitBuiltins()
        {
            foreach (string datatype in Utils.TokenUtil.DATA_TYPES)
            {
                this.Insert(new Symbols.BuiltinTypeSymbol(datatype));
            }
        }

        public void Insert(Symbols.Symbol Symbol)
        {
            this.Symbols[Symbol.Name] = Symbol;
        }

        public Symbols.Symbol Lookup(string Name, bool CurrentScopeOnly = false)
        {
            if (this.Symbols.Keys.Contains(Name))
            {
                return this.Symbols[Name];
            }

            if (CurrentScopeOnly)
            {
                return null;
            }

            if (this.EnclosingScope != null)
            {
                return this.EnclosingScope.Lookup(Name);
            }

            return null;
        }
    }
}
