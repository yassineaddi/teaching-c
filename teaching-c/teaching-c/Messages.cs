using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teaching_c
{
    static class Messages
    {
        public const string MULTI_CHAR_CONSTANT = "multi-character constant";
        public const string UNKNOWN_ESCAPE_SEQ = "unknown escape sequence '{0}'";
        public const string EMPTY_CHAR = "empty character constant";
        public const string OCTAL_LITERAL = "octal literal indicated by leading digit 0 cannot contain digit 8 or 9";
        public const string HEXA_LITERAL = "hexadecimal literal indicated by leading 0x must contain at least one digit";
        public const string UNTERMINATED_COMMENT = "unterminated comment";
        public const string STRAY = "stray '{0}' in program";

        public const string EXPECTED_GOT = "invalid syntax, expected '{0}' got '{1}'";
        public const string EXPECTED_DECLARATION = "invalid syntax, expected declaration specifiers";
        public const string EXPECTED_EXPRESSION = "invalid syntax, expected expression got '{0}'";
        public const string UNEXPECTED = "invalid syntax, unexpected '{0}'";
        public const string INVALID_SYNTAX = "invalid syntax";
        public const string UNRECOGNIZED_TYPE_NAME = "unrecognized type name '{0}'";
        public const string UNRECOGNIZED_TYPE_NAME_WITH = "unrecognized type name '{0}'; did you mean '{1}'?";

        public const string UNDEFINED_REFERENCE = "undefined reference to 'main'";
        public const string MUST_RETURN_INT = "'main' must return 'int'";
        public const string CANNOT_HAVE_PARAMS = "'main' cannot have parameters";
        public const string DUPLICATE_DECLARATION = "duplicate declaration of '{0}'";
        public const string UNDECLARED = "undeclared '{0}'";
        public const string UNDECLARED_FUNC = "undeclared function '{0}'";
        public const string UNINTIALIZED = "unintialized variable '{0}'";
        public const string MISSING_NAME_PARAM = "missing name for parameter in function definition of '{0}'";
        public const string FUNC_SIGNATURE = "function definition signature of '{0}' does not agree with function declaration signature";
        public const string FUNC_ARGS = "function '{0}' takes {1} arguments, not {2}";
        public const string NOT_FUNC = "'{0}' is not a function";
        public const string INVALID_OPS_MODULUS = "invalid operands to %";
        
        public const string MISSING_RETURN_STMT = "missing return statement for '{0}'";
        public const string UNDEFINED_FUNC = "undefined function '{0}'";
    }
}
