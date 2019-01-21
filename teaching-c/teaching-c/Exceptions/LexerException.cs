using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Exceptions
{
    public class LexerException : Exception
    {
        public LexerException()
            : base()
        {
        }

        public LexerException(string message)
            : base(message)
        {
        }
    }
}
