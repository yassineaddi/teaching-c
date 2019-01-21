using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException()
            : base()
        {
        }

        public ParserException(string message)
            : base(message)
        {
        }
    }
}
