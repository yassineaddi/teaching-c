using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Exceptions
{
    class SemanticAnalyzerException : Exception
    {
        public SemanticAnalyzerException()
            : base()
        {
        }

        public SemanticAnalyzerException(string message)
            : base(message)
        {
        }
    }
}
