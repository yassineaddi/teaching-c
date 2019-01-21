using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Program : AbstractSyntaxTree
    {
        public Compound Compound { get; set; }

        public Program(Compound Compound)
        {
            this.Compound = Compound;
        }
    }
}
