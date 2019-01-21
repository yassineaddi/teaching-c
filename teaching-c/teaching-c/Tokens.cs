using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c
{
    public enum Tokens
    {
        Int, Float, Double, Char,
        Return, If, Else, While, For, Do,

        Addition, Subtraction, Multiplication, Division, Modulus,

        OpenParen, CloseParen, OpenBrace, CloseBrace, Semicolon, Comma,
        AssignOp, GreaterOp, GreaterEqOp, LessOp, LessEqOp, Equal, NotEqual,
        LogicalAnd, LogicalOr, IncrementOp, DecrementOp,
        SuffixIncOp, SuffixDecOp, PrefixIncOp, PrefixDecOp,
        OpenBracket, CloseBracket,

        IntLiteral, RealLiteral, CharLiteral,

        Identifier, EOF
    }
}
