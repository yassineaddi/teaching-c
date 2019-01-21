using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c
{
    class TokenPosition
    {
        public int Column { get; set; }
        public int Line { get; set; }

        public TokenPosition(int Column, int Line)
        {
            this.Column = Column;
            this.Line = Line;
        }
    }

    class Token
    {
        public TokenPosition Position { get; set; }
        public int Type { get; set; }
        public object Value { get; set; }

        public Token(int Type, object Value, TokenPosition Position)
        {
            this.Position = Position;
            this.Type = Type;
            this.Value = Value;
        }

        public override string ToString()
        {
            return String.Format("Token<{0}, {1} : \"{2}\">",
                Enum.GetName(typeof(Tokens),this.Type),
                this.Value.GetType(),
                this.Value);
        }
    }
}
