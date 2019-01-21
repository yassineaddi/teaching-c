using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Exceptions
{
    class InterpreterException : Exception
    {
        public InterpreterException()
            : base()
        {
        }

        public InterpreterException(string message)
            : base(message)
        {
        }
    }
}
