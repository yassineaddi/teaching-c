using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c.Utils
{
    public static class TokenUtil
    {
        public const int RESERVED_KEYWORDS = 10;

        public static readonly string[] DATA_TYPES = { "int", "float", "double", "char" };

        public static string Token(string token)
        {
            switch (token)
            {
                case "OpenParen": return "(";
                case "CloseParen": return ")";
                case "OpenBrace": return "{";
                case "CloseBrace": return "}";
                case "Semicolon": return ";";
                case "AssignOp": return "=";
                case "GreaterOp": return ">";
                case "GreaterEqOp": return ">=";
                case "LessOp": return "<";
                case "LessEqOp": return "<=";
                case "Equal": return "==";
                case "NotEqual": return "!=";
                case "LogicalAnd": return "&&";
                case "LogicalOr": return "||";
                case "Comma": return ",";
                case "Addition": return "+";
                case "Subtraction": return "-";
                case "Multiplication": return "*";
                case "Division": return "/";
                case "Modulus": return "%";
                case "IncrementOp":
                case "PrefixIncOp":
                case "SuffixIncOp": return "++";
                case "DecrementOp":
                case "PrefixDecOp":
                case "SuffixDecOp": return "--";
                case "OpenBracket": return "[";
                case "CloseBracket": return "]";
                case "EOF": return "end of input";
                default: return token.ToLower();
            }
        }
    }
}
