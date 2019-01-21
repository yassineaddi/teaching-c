using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using teaching_c.Utils;

namespace teaching_c
{
    class Lexer
    {
        public string Source { get; set; }
        public int ReadingPosition { get; set; }
        public char CurrentCharacter { get; set; }
        public int Column { get; set; }
        public int Line { get; set; }

        public Lexer(string Source)
        {
            this.Source = Source;
            this.ReadingPosition = 0;
            this.CurrentCharacter = (this.ReadingPosition <= Source.Length - 1) ? Source[this.ReadingPosition] : '\0';
            this.Column = 1;
            this.Line = 1;
        }

        private void Error(string message, int line, int col)
        {
            message = String.Format("{0} at ({1}, {2})", message, line, col);

            throw new Exceptions.LexerException(message);
        }

        public char Peek()
        {
            if (this.ReadingPosition + 1 > this.Source.Length - 1)
            {
                return '\0';
            }

            return this.Source[this.ReadingPosition + 1];
        }

        public void Advance()
        {
            if (this.ReadingPosition <= this.Source.Length - 1 &&
                this.Source[this.ReadingPosition] == '\n')
            {
                this.Line++;
                this.Column = 0;
            }

            this.ReadingPosition++;
            this.Column++;

            if (this.ReadingPosition > this.Source.Length - 1)
            {
                this.CurrentCharacter = '\0';
            }
            else
            {
                this.CurrentCharacter = this.Source[this.ReadingPosition];
            }
        }

        private void SkipWhiteSpace()
        {
            this.Advance();

            while (Char.IsWhiteSpace(this.CurrentCharacter))
            {
                this.Advance();
            }
        }

        private void SkipSingleLineComment()
        {
            this.Advance();
            this.Advance();

            while (this.CurrentCharacter != '\0' && this.CurrentCharacter != '\n')
            {
                this.Advance();
            }
        }

        private void SkipMultiLineComment()
        {
            int line = this.Line;
            int col = this.Column;

            this.Advance();
            this.Advance();

            while (this.CurrentCharacter != '*' || this.Peek() != '/')
            {
                if (this.CurrentCharacter == '\0')
                {
                    this.Error(Messages.UNTERMINATED_COMMENT, line, col);
                }

                this.Advance();
            }

            this.Advance();
            this.Advance();
        }

        private string CollectLetters()
        {
            StringBuilder builder = new StringBuilder();

            while (Char.IsLetterOrDigit(this.CurrentCharacter) || this.CurrentCharacter == '_')
            {
                builder.Append(this.CurrentCharacter);
                this.Advance();
            }

            return builder.ToString();
        }

        private object CollectNumber()
        {
            int line = this.Line;
            int col = this.Column;

            StringBuilder builder = new StringBuilder();

            while (Char.IsDigit(this.CurrentCharacter))
            {
                builder.Append(this.CurrentCharacter);

                if (builder[0] == '0' &&
                    (int)(this.CurrentCharacter - '0') > 7)
                {
                    this.Error(Messages.OCTAL_LITERAL, line, col);
                }

                this.Advance();
            }

            if (builder[0] == '0' &&
                this.CurrentCharacter.ToString().ToLower() == "x")
            {
                builder.Append(this.CurrentCharacter);
                this.Advance();

                while (Char.IsLetterOrDigit(this.CurrentCharacter))
                {
                    builder.Append(this.CurrentCharacter);
                    this.Advance();
                }

                if (builder.Length == 2)
                {
                    this.Error(Messages.HEXA_LITERAL, line, col);
                }

                return Convert.ToInt64(builder.ToString(), 16);
            }

            if (this.CurrentCharacter == '.')
            {
                builder.Append(this.CurrentCharacter);
                this.Advance();

                while (Char.IsDigit(this.CurrentCharacter))
                {
                    builder.Append(this.CurrentCharacter);
                    this.Advance();
                }

                return Convert.ToDecimal(builder.ToString(), CultureInfo.InvariantCulture);

            }
            if (builder[0] == '0')
            {
                return Convert.ToInt64(builder.ToString(), 8);
            }
            return Convert.ToInt64(builder.ToString());
        }

        private char CharLiteral()
        {
            int line = this.Line;
            int col = this.Column;

            this.Advance();

            char c = this.CurrentCharacter;

            if (c == '\'')
            {
                this.Error(Messages.EMPTY_CHAR, line, col);
            }

            if (c == '\\')
            {
                this.Advance();

                switch (this.CurrentCharacter)
                {
                    case 'n':
                        c = '\n';
                        break;
                    case 't':
                        c = '\t';
                        break;
                    case '0':
                        c = '\0';
                        break;
                    default:
                        this.Error(String.Format(Messages.UNKNOWN_ESCAPE_SEQ, this.CurrentCharacter), line, col);
                        break;
                }
            }

            this.Advance();

            if (this.CurrentCharacter != '\'')
            {
                this.Error(Messages.MULTI_CHAR_CONSTANT, line, col);
            }

            this.Advance();

            return c;
        }

        public Token UpcomingToken()
        {
            while (this.CurrentCharacter != '\0')
            {
                if (Char.IsWhiteSpace(this.CurrentCharacter))
                {
                    this.SkipWhiteSpace();
                    continue;
                }

                if (this.CurrentCharacter == '/' && this.Peek() == '/')
                {
                    this.SkipSingleLineComment();
                    continue;
                }

                if (this.CurrentCharacter == '/' && this.Peek() == '*')
                {
                    this.SkipMultiLineComment();
                    continue;
                }

                if (this.CurrentCharacter == '\'')
                {
                    int line = this.Line;
                    int col = this.Column;

                    return new Token((int)Tokens.CharLiteral,
                            this.CharLiteral(),
                            new TokenPosition(col, line));
                }

                if (this.CurrentCharacter == '(')
                {
                    Token token = new Token((int) Tokens.OpenParen,
                        "(",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == ')')
                {
                    Token token = new Token((int)Tokens.CloseParen,
                        ")",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '[')
                {
                    Token token = new Token((int)Tokens.OpenBracket,
                        "[",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == ']')
                {
                    Token token = new Token((int)Tokens.CloseBracket,
                        "]",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '{')
                {
                    Token token = new Token((int)Tokens.OpenBrace,
                        "{",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '}')
                {
                    Token token = new Token((int)Tokens.CloseBrace,
                        "}",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == ';')
                {
                    Token token = new Token((int)Tokens.Semicolon,
                        ";",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    
                    return token;
                }

                if (this.CurrentCharacter == ',')
                {
                    Token token = new Token((int)Tokens.Comma,
                        ",",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '+' && this.Peek() == '+')
                {
                    Token token = new Token((int)Tokens.IncrementOp,
                        "++",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '+')
                {
                    Token token = new Token((int)Tokens.Addition,
                        "+",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '-' && this.Peek() == '-')
                {
                    Token token = new Token((int)Tokens.DecrementOp,
                        "--",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '-')
                {
                    Token token = new Token((int)Tokens.Subtraction,
                        "-",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '*')
                {
                    Token token = new Token((int)Tokens.Multiplication,
                        "*",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '/')
                {
                    Token token = new Token((int)Tokens.Division,
                        "/",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '%')
                {
                    Token token = new Token((int)Tokens.Modulus,
                        "%",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '=' && this.Peek() == '=')
                {
                    Token token = new Token((int)Tokens.Equal,
                        "==",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '!' && this.Peek() == '=')
                {
                    Token token = new Token((int)Tokens.NotEqual,
                        "!=",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '=')
                {
                    Token token = new Token((int)Tokens.AssignOp,
                        "=",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '>' && this.Peek() == '=')
                {
                    Token token = new Token((int)Tokens.GreaterEqOp,
                        ">=",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }
                
                if (this.CurrentCharacter == '>')
                {
                    Token token = new Token((int)Tokens.GreaterOp,
                        ">",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '<' && this.Peek() == '=')
                {
                    Token token = new Token((int)Tokens.LessEqOp,
                        "<=",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '<')
                {
                    Token token = new Token((int)Tokens.LessOp,
                        "<",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '&' && this.Peek() == '&')
                {
                    Token token = new Token((int)Tokens.LogicalAnd,
                        "&&",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (this.CurrentCharacter == '|' && this.Peek() == '|')
                {
                    Token token = new Token((int)Tokens.LogicalOr,
                        "||",
                        new TokenPosition(this.Column, this.Line));
                    this.Advance();
                    this.Advance();

                    return token;
                }

                if (Char.IsDigit(this.CurrentCharacter))
                {
                    int line = this.Line;
                    int col = this.Column;

                    object number = this.CollectNumber();
                    if (number is int)
                    {
                        return new Token((int)Tokens.IntLiteral,
                            number,
                            new TokenPosition(col, line));
                    }

                    return new Token((int)Tokens.RealLiteral,
                        number,
                        new TokenPosition(col, line));
                }

                if (Char.IsLetter(this.CurrentCharacter) || this.CurrentCharacter == '_')
                {
                    int line = this.Line;
                    int col = this.Column;

                    string letters = this.CollectLetters();
                    
                    foreach (Tokens token in Utils.EnumUtil.GetValues<Tokens>().Take(TokenUtil.RESERVED_KEYWORDS))
                    {
                        if (Enum.GetName(typeof(Tokens), token).ToLower() == letters)
                        {
                            return new Token((int) token,
                                Enum.GetName(typeof(Tokens), token).ToLower(),
                                new TokenPosition(col, line));
                        }
                    }

                    return new Token((int) Tokens.Identifier,
                        letters,
                        new TokenPosition(col, line));
                }

                this.Error(
                    String.Format(Messages.STRAY, this.CurrentCharacter),
                    this.Line,
                    this.Column);
            }

            return new Token((int) Tokens.EOF,
                Enum.GetName(typeof(Tokens), Tokens.EOF),
                new TokenPosition(this.Column, this.Line));
        }
    }
}
