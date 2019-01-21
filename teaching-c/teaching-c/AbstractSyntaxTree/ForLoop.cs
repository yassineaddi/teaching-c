using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class ForLoop : AbstractSyntaxTree
    {
        public Compound Body { get; set; }
        public object Init { get; set; }
        public object Condition { get; set; }
        public object Increment { get; set; }

        public ForLoop(Token Token, object Init, object Condition, object Increment, Compound Body)
        {
            this.Token = Token;
            this.Body = Body;
            this.Init = Init;
            this.Condition = Condition;
            this.Increment = Increment;
        }
    }
}
