﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.AbstractSyntaxTree
{
    class Compound : AbstractSyntaxTree
    {
        public List<object> Children = new List<object>();

        public Compound()
        {
        }
    }
}
