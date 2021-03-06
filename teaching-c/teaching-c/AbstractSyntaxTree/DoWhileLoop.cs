﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class DoWhileLoop : AbstractSyntaxTree
    {
        public Compound Body { get; set; }
        public object Expression { get; set; }

        public DoWhileLoop(Token Token, object Expression, Compound Body)
        {
            this.Token = Token;
            this.Body = Body;
            this.Expression = Expression;
        }
    }
}
