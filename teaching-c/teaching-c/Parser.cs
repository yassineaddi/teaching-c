using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST = teaching_c.AbstractSyntaxTree;
using teaching_c.Utils;

namespace teaching_c
{
    class Parser
    {
        public Lexer Lexer { get; set; }
        public Token CurrentToken { get; set; }

        public Parser(Lexer Lexer)
        {
            this.Lexer = Lexer;
            this.CurrentToken = this.Lexer.UpcomingToken();
        }

        private void Error(string message, bool WithPosition = true)
        {
            if (WithPosition)
            {
                message = String.Format("{0} at ({1}, {2})", message,
                    this.CurrentToken.Position.Line,
                    this.CurrentToken.Position.Column);
            }

            throw new Exceptions.ParserException(message);
        }

        private void Eat(object TokenType)
        {
            if (this.CurrentToken.Type == (int)TokenType)
            {
                this.CurrentToken = this.Lexer.UpcomingToken();
            }
            else
            {
                string expected = TokenUtil.Token(
                    Enum.GetName(typeof(Tokens), (int)TokenType));
                string got = TokenUtil.Token(
                    Enum.GetName(typeof(Tokens), this.CurrentToken.Type));

                this.Error(String.Format(Messages.EXPECTED_GOT, expected, got));
            }
        }

        private AST.Program Program()
        {
            AST.Compound compound = this.CompoundStatement();

            AST.Program program = new AST.Program(compound);

            return program;
        }

        private AST.Compound CompoundStatement()
        {
            AST.Compound root = new AST.Compound();

            while (this.CurrentToken.Type != (int)Tokens.EOF)
            {
                if (TokenUtil.DATA_TYPES.Contains(this.CurrentToken.Value))
                {
                    AST.Type type = this.Type();
                    AST.Variable id = this.Variable();

                    if (this.CurrentToken.Type == (int)Tokens.OpenParen)
                    {
                        AST.SubroutineDeclaration function = new AST.SubroutineDeclaration(
                            id,
                            type,
                            this.SubroutineParams(),
                            this.SubroutineDeclaration());

                        root.Children.Add(function);
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.AssignOp)
                    {
                        AST.VariableDeclaration declaration = new AST.VariableDeclaration(id, type);

                        Token token = this.CurrentToken;
                        this.Eat(Tokens.AssignOp);
                        AST.Assign assignment = new AST.Assign(id, token, this.Expression());
                        this.Eat(Tokens.Semicolon);

                        root.Children.Add(declaration);
                        root.Children.Add(assignment);
                    }
                    else
                    {
                        this.Eat(Tokens.Semicolon);
                        AST.VariableDeclaration declaration = new AST.VariableDeclaration(id, type);

                        root.Children.Add(declaration);
                    }
                }
                else
                {
                    this.Error(Messages.INVALID_SYNTAX);
                }
            }

            return root;
        }

        private List<AST.Param> SubroutineParams()
        {
            List<AST.Param> node = new List<AST.Param>();

            this.Eat(Tokens.OpenParen);

            while (this.CurrentToken.Type != (int)Tokens.CloseParen)
            {
                if (0 != node.Count())
                {
                    this.Eat(Tokens.Comma);
                }

                if ( ! TokenUtil.DATA_TYPES.Contains(this.CurrentToken.Value))
                {
                    if (this.CurrentToken.Type != (int)Tokens.Identifier)
                    {
                        this.Error(Messages.EXPECTED_DECLARATION);
                    }
                    if (TokenUtil.DATA_TYPES.Contains(this.CurrentToken.Value.ToString().ToLower()))
                    {
                        this.Error(
                            String.Format(Messages.UNRECOGNIZED_TYPE_NAME_WITH,
                            this.CurrentToken.Value,
                            this.CurrentToken.Value.ToString().ToLower()));
                    }

                    this.Error(String.Format(Messages.UNRECOGNIZED_TYPE_NAME, this.CurrentToken.Value));
                }

                AST.Type type = this.Type();
                AST.Param parameter = new AST.Param(null, type);
                if (this.CurrentToken.Type == (int)Tokens.Identifier)
                {
                    AST.Variable variable = this.Variable();
                    parameter = new AST.Param(variable, type);
                }

                node.Add(parameter);
            }

            this.Eat(Tokens.CloseParen);

            return node;
        }

        private AST.Compound SubroutineDeclaration()
        {
            if (this.CurrentToken.Type == (int)Tokens.Semicolon)
            {
                this.Eat(Tokens.Semicolon);
                return null;
            }

            this.Eat(Tokens.OpenBrace);
            List<object> Nodes = this.StatementList();
            this.Eat(Tokens.CloseBrace);

            AST.Compound block = new AST.Compound();
            foreach (object Node in Nodes)
            {
                block.Children.Add(Node);
            }

            return block;
        }

        private List<object> StatementList(int Length = -1)
        {
            List<object> results = new List<object>();

            while (this.CurrentToken.Type != (int)Tokens.CloseBrace)
            {
                if (this.CurrentToken.Type == (int)Tokens.EOF)
                {
                    break;
                }

                if (TokenUtil.DATA_TYPES.Contains(this.CurrentToken.Value))
                {
                    AST.Type type = this.Type();
                    AST.Variable variable = this.Variable();

                    if (this.CurrentToken.Type == (int)Tokens.OpenBracket)
                    {
                        this.Eat(Tokens.OpenBracket);

                        List<object> elements = new List<object>();

                        AST.Array array = new AST.Array(variable.Token, type, this.Expression());

                        this.Eat(Tokens.CloseBracket);

                        if (this.CurrentToken.Type == (int)Tokens.AssignOp)
                        {
                            this.Eat(Tokens.AssignOp);
                            this.Eat(Tokens.OpenBrace);
                            elements.Add(this.Expression());
                            while (this.CurrentToken.Type == (int)Tokens.Comma)
                            {
                                this.Eat(Tokens.Comma);
                                elements.Add(this.Expression());
                            }
                            this.Eat(Tokens.CloseBrace);

                            array.Elements = elements;
                        }

                        results.Add(array);
                    }
                    else
                    {
                        AST.VariableDeclaration declaration = new AST.VariableDeclaration(variable, type);

                        results.Add(declaration);

                        if (this.CurrentToken.Type == (int)Tokens.AssignOp)
                        {
                            Token token = this.CurrentToken;
                            this.Eat(Tokens.AssignOp);
                            AST.Assign assignment = new AST.Assign(variable, token, this.Expression());

                            results.Add(assignment);
                        }
                    }

                    this.Eat(Tokens.Semicolon);
                }
                else if (this.CurrentToken.Type == (int)Tokens.Identifier)
                {
                    AST.Variable left = this.Variable();
                    Token token = this.CurrentToken;

                    if (token.Type == (int)Tokens.OpenParen)
                    {
                        this.Eat(Tokens.OpenParen);

                        List<object> args = new List<object>();

                        while (this.CurrentToken.Type != (int)Tokens.CloseParen)
                        {
                            if (0 != args.Count())
                            {
                                this.Eat(Tokens.Comma);
                            }

                            args.Add(this.Expression());
                        }
                        this.Eat(Tokens.CloseParen);

                        AST.Call call = new AST.Call(left, args);

                        results.Add(call);
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.AssignOp)
                    {
                        this.Eat(Tokens.AssignOp);
                        object right = this.Expression();

                        object assign = new AST.Assign(left, token, right);

                        results.Add(assign);
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.IncrementOp)
                    {
                        this.Eat(Tokens.IncrementOp);

                        token.Type = (int)Tokens.SuffixIncOp;

                        AST.UnaryOperator unaryop = new AST.UnaryOperator(token, left);

                        results.Add(unaryop);
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.DecrementOp)
                    {
                        this.Eat(Tokens.DecrementOp);

                        token.Type = (int)Tokens.SuffixDecOp;

                        AST.UnaryOperator unaryop = new AST.UnaryOperator(token, left);

                        results.Add(unaryop);
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.OpenBracket)
                    {
                        this.Eat(Tokens.OpenBracket);

                        AST.ArrayElement arrayele = new AST.ArrayElement(token, left, this.Expression());

                        this.Eat(Tokens.CloseBracket);

                        if (this.CurrentToken.Type == (int)Tokens.AssignOp)
                        {
                            this.Eat(Tokens.AssignOp);
                            object right = this.Expression();

                            AST.ArrayAssign arrayassign = new AST.ArrayAssign(left, token, arrayele.Subscript, right);

                            results.Add(arrayassign);
                        }
                        else
                        {
                            results.Add(arrayele);
                        }
                    }

                    if (this.CurrentToken.Type != (int)Tokens.Semicolon)
                    {
                        this.Expression();
                    }

                    this.Eat(Tokens.Semicolon);
                }
                else if (this.CurrentToken.Type == (int)Tokens.Semicolon)
                {
                    results.Add(this.Null());

                    this.Eat(Tokens.Semicolon);
                }
                else if (this.CurrentToken.Type == (int)Tokens.Return)
                {
                    Token token = this.CurrentToken;
                    this.Eat(Tokens.Return);
                    AST.ReturnStatement ret = new AST.ReturnStatement(token, this.Expression());

                    results.Add(ret);

                    this.Eat(Tokens.Semicolon);
                }
                else if (this.CurrentToken.Type == (int)Tokens.While)
                {
                    Token token = this.CurrentToken;
                    this.Eat(Tokens.While);
                    this.Eat(Tokens.OpenParen);
                    object expr = this.Expression();
                    this.Eat(Tokens.CloseParen);
                    this.Eat(Tokens.OpenBrace);
                    List<object> whilenode = this.StatementList();
                    this.Eat(Tokens.CloseBrace);

                    AST.Compound whileblock = new AST.Compound();
                    foreach (object Node in whilenode)
                    {
                        whileblock.Children.Add(Node);
                    }

                    AST.WhileLoop whileloop = new AST.WhileLoop(token, expr, whileblock);

                    results.Add(whileloop);
                }
                else if (this.CurrentToken.Type == (int)Tokens.For)
                {
                    Token token = this.CurrentToken;
                    object init = null, condition = null, increment = null;

                    this.Eat(Tokens.For);
                    this.Eat(Tokens.OpenParen);
                    if (this.CurrentToken.Type == (int)Tokens.Semicolon)
                    {
                        init = this.Null();
                    }
                    else
                    {
                        AST.Variable left = this.Variable();
                        this.Eat(Tokens.AssignOp);
                        object right = this.Expression();

                        init = new AST.Assign(left, token, right);
                    }
                    this.Eat(Tokens.Semicolon);
                    if (this.CurrentToken.Type == (int)Tokens.Semicolon)
                    {
                        condition = this.Null();
                    }
                    else
                    {
                        condition = this.Expression();
                    }
                    this.Eat(Tokens.Semicolon);
                    if (this.CurrentToken.Type == (int)Tokens.CloseParen)
                    {
                        increment = this.Null();
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.Identifier)
                    {
                        AST.Variable left = this.Variable();

                        if (this.CurrentToken.Type == (int)Tokens.AssignOp)
                        {
                            this.Eat(Tokens.AssignOp);
                            object right = this.Expression();

                            increment = new AST.Assign(left, token, right);
                        }
                        else if (this.CurrentToken.Type == (int)Tokens.IncrementOp)
                        {
                            this.Eat(Tokens.IncrementOp);

                            token.Type = (int)Tokens.SuffixIncOp;

                            increment = new AST.UnaryOperator(token, left);
                        }
                        else if (this.CurrentToken.Type == (int)Tokens.DecrementOp)
                        {
                            this.Eat(Tokens.DecrementOp);

                            token.Type = (int)Tokens.SuffixDecOp;

                            increment = new AST.UnaryOperator(token, left);
                        }
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.IncrementOp)
                    {
                        this.Eat(Tokens.IncrementOp);

                        token.Type = (int)Tokens.PrefixIncOp;

                        object variable = this.Variable();

                        increment = new AST.UnaryOperator(token, variable);
                    }
                    else if (this.CurrentToken.Type == (int)Tokens.DecrementOp)
                    {
                        this.Eat(Tokens.DecrementOp);

                        token.Type = (int)Tokens.PrefixDecOp;

                        object variable = this.Variable();

                        increment = new AST.UnaryOperator(token, variable);
                    }
                    this.Eat(Tokens.CloseParen);
                    this.Eat(Tokens.OpenBrace);
                    List<object> fornode = this.StatementList();
                    this.Eat(Tokens.CloseBrace);

                    AST.Compound forblock = new AST.Compound();
                    foreach (object Node in fornode)
                    {
                        forblock.Children.Add(Node);
                    }

                    AST.ForLoop forloop = new AST.ForLoop(token, init, condition, increment, forblock);

                    results.Add(forloop);
                }
                else if (this.CurrentToken.Type == (int)Tokens.Do)
                {
                    Token token = this.CurrentToken;
                    this.Eat(Tokens.Do);
                    this.Eat(Tokens.OpenBrace);
                    List<object> dowhilenode = this.StatementList();
                    this.Eat(Tokens.CloseBrace);
                    this.Eat(Tokens.While);
                    this.Eat(Tokens.OpenParen);
                    object expr = this.Expression();
                    this.Eat(Tokens.CloseParen);
                    this.Eat(Tokens.Semicolon);

                    AST.Compound dowhileblock = new AST.Compound();
                    foreach (object Node in dowhilenode)
                    {
                        dowhileblock.Children.Add(Node);
                    }

                    AST.DoWhileLoop whileloop = new AST.DoWhileLoop(token, expr, dowhileblock);

                    results.Add(whileloop);
                }
                else if (this.CurrentToken.Type == (int)Tokens.If)
                {
                    Token token = this.CurrentToken;
                    this.Eat(Tokens.If);
                    this.Eat(Tokens.OpenParen);
                    object expr = this.Expression();
                    this.Eat(Tokens.CloseParen);
                    this.Eat(Tokens.OpenBrace);
                    List<object> ifnode = this.StatementList();
                    this.Eat(Tokens.CloseBrace);

                    AST.Compound ifblock = new AST.Compound();
                    foreach (object Node in ifnode)
                    {
                        ifblock.Children.Add(Node);
                    }

                    AST.ConditionalStatement conditional = new AST.ConditionalStatement(token, expr, ifblock);

                    if (this.CurrentToken.Type == (int)Tokens.Else)
                    {
                        this.Eat(Tokens.Else);
                        this.Eat(Tokens.OpenBrace);
                        List<object> elsenode = this.StatementList();
                        this.Eat(Tokens.CloseBrace);

                        AST.Compound elseblock = new AST.Compound();
                        foreach (object Node in elsenode)
                        {
                            elseblock.Children.Add(Node);
                        }

                        conditional.Else = elseblock;
                    }

                    results.Add(conditional);
                }
                else if (this.CurrentToken.Type == (int)Tokens.Addition ||
                    this.CurrentToken.Type == (int)Tokens.Subtraction ||
                    this.CurrentToken.Type == (int)Tokens.IncrementOp ||
                    this.CurrentToken.Type == (int)Tokens.DecrementOp ||
                    this.CurrentToken.Type == (int)Tokens.OpenParen ||
                    this.CurrentToken.Type == (int)Tokens.IntLiteral ||
                    this.CurrentToken.Type == (int)Tokens.RealLiteral ||
                    this.CurrentToken.Type == (int)Tokens.CharLiteral)
                {
                    this.Expression();

                    this.Eat(Tokens.Semicolon);
                }
                else if (this.CurrentToken.Type == (int)Tokens.OpenBrace)
                {
                    Token token = this.CurrentToken;
                    this.Eat(Tokens.OpenBrace);
                    List<object> statements = this.StatementList();
                    this.Eat(Tokens.CloseBrace);

                    AST.CompoundStatement block = new AST.CompoundStatement(token);
                    foreach (object Node in statements)
                    {
                        block.Body.Children.Add(Node);
                    }

                    results.Add(block);
                }
                else
                {
                    this.Error(Messages.INVALID_SYNTAX);
                }
            }

            if (Length != -1 && results.Count() == Length)
            {
                return results;
            }

            return results;
        }

        private AST.Type Type()
        {
            if ( ! TokenUtil.DATA_TYPES.Contains(this.CurrentToken.Value))
            {
                if (TokenUtil.DATA_TYPES.Contains(this.CurrentToken.Value.ToString().ToLower()))
                {
                    this.Error(
                        String.Format(Messages.UNRECOGNIZED_TYPE_NAME_WITH,
                        this.CurrentToken.Value,
                        this.CurrentToken.Value.ToString().ToLower()));
                }

                this.Error(String.Format(Messages.UNRECOGNIZED_TYPE_NAME, this.CurrentToken.Value));
            }

            AST.Type Node = new AST.Type(this.CurrentToken);
            this.Eat(this.CurrentToken.Type);

            return Node;
        }

        private AST.Variable Variable()
        {
            AST.Variable Node = new AST.Variable(this.CurrentToken);
            this.Eat(Tokens.Identifier);

            return Node;
        }

        private object Null()
        {
            return new AST.NullStatement();
        }

        private object Expression()
        {
            object Node = this.LogicalOr();

            return Node;
        }

        private object LogicalOr()
        {
            object Node = this.LogicalAnd();

            if (Node == null)
            {
                this.Error(String.Format(Messages.EXPECTED_EXPRESSION, this.CurrentToken.Value));
            }

            while (this.CurrentToken.Type == (int)Tokens.LogicalOr)
            {
                Token token = this.CurrentToken;
                this.Eat(Tokens.LogicalOr);

                Node = new AST.BinaryOperator(
                    Node,
                    token,
                    this.LogicalAnd());
            }

            return Node;
        }
        
        private object LogicalAnd()
        {
            object Node = this.RelationalEq();

            if (Node == null)
            {
                this.Error(String.Format(Messages.EXPECTED_EXPRESSION, this.CurrentToken.Value));
            }

            while (this.CurrentToken.Type == (int)Tokens.LogicalAnd)
            {
                Token token = this.CurrentToken;
                this.Eat(Tokens.LogicalAnd);

                Node = new AST.BinaryOperator(
                    Node,
                    token,
                    this.RelationalEq());
            }

            return Node;
        }

        private object RelationalEq()
        {
            object Node = this.RelationalLtGr();

            if (Node == null)
            {
                this.Error(String.Format(Messages.EXPECTED_EXPRESSION, this.CurrentToken.Value));
            }

            while (this.CurrentToken.Type == (int)Tokens.Equal ||
                this.CurrentToken.Type == (int)Tokens.NotEqual)
            {
                Token token = this.CurrentToken;
                if (token.Type == (int)Tokens.Equal)
                {
                    this.Eat(Tokens.Equal);
                }
                else if (token.Type == (int)Tokens.NotEqual)
                {
                    this.Eat(Tokens.NotEqual);
                }

                Node = new AST.BinaryOperator(
                    Node,
                    token,
                    this.RelationalLtGr());
            }

            return Node;
        }

        private object RelationalLtGr()
        {
            object Node = this.Arithmetic();

            if (Node == null)
            {
                this.Error(String.Format(Messages.EXPECTED_EXPRESSION, this.CurrentToken.Value));
            }

            while (this.CurrentToken.Type == (int)Tokens.GreaterOp ||
                this.CurrentToken.Type == (int)Tokens.GreaterEqOp ||
                this.CurrentToken.Type == (int)Tokens.LessOp ||
                this.CurrentToken.Type == (int)Tokens.LessEqOp)
            {
                Token token = this.CurrentToken;
                if (token.Type == (int)Tokens.GreaterOp)
                {
                    this.Eat(Tokens.GreaterOp);
                }
                else if (token.Type == (int)Tokens.GreaterEqOp)
                {
                    this.Eat(Tokens.GreaterEqOp);
                }
                else if (token.Type == (int)Tokens.LessOp)
                {
                    this.Eat(Tokens.LessOp);
                }
                else if (token.Type == (int)Tokens.LessEqOp)
                {
                    this.Eat(Tokens.LessEqOp);
                }

                Node = new AST.BinaryOperator(
                    Node,
                    token,
                    this.Arithmetic());
            }

            return Node;
        }

        private object Arithmetic()
        {
            object Node = this.Term();

            while (this.CurrentToken.Type == (int)Tokens.Addition ||
                this.CurrentToken.Type == (int)Tokens.Subtraction)
            {
                Token token = this.CurrentToken;
                if (token.Type == (int)Tokens.Addition)
                {
                    this.Eat(Tokens.Addition);
                }
                else if (token.Type == (int)Tokens.Subtraction)
                {
                    this.Eat(Tokens.Subtraction);
                }

                Node = new AST.BinaryOperator(
                    Node,
                    token,
                    this.Term());
            }

            return Node;
        }

        private object Term()
        {
            object Node = this.Factor();

            while (this.CurrentToken.Type == (int)Tokens.Multiplication ||
                this.CurrentToken.Type == (int)Tokens.Division ||
                this.CurrentToken.Type == (int)Tokens.Modulus)
            {
                Token token = this.CurrentToken;
                if (token.Type == (int)Tokens.Multiplication)
                {
                    this.Eat(Tokens.Multiplication);
                }
                else if (token.Type == (int)Tokens.Division)
                {
                    this.Eat(Tokens.Division);
                }
                else if (token.Type == (int)Tokens.Modulus)
                {
                    this.Eat(Tokens.Modulus);
                }

                Node = new AST.BinaryOperator(
                    Node,
                    token,
                    this.Factor());
            }

            return Node;
        }

        private object Factor()
        {
            Token token = this.CurrentToken;

            if (token.Type == (int)Tokens.Addition)
            {
                this.Eat(Tokens.Addition);
                object node = new AST.UnaryOperator(token, this.Factor());
                return node;
            }
            else if (token.Type == (int)Tokens.Subtraction)
            {
                this.Eat(Tokens.Subtraction);
                object node = new AST.UnaryOperator(token, this.Factor());
                return node;
            }
            else if (token.Type == (int)Tokens.IntLiteral)
            {
                this.Eat(Tokens.IntLiteral);
                return new AST.Number(token);
            }
            else if (token.Type == (int)Tokens.RealLiteral)
            {
                this.Eat(Tokens.RealLiteral);
                return new AST.Number(token);
            }
            else if (token.Type == (int)Tokens.CharLiteral)
            {
                this.Eat(Tokens.CharLiteral);
                return new AST.Character(token);
            }
            else if (token.Type == (int)Tokens.OpenParen)
            {
                this.Eat(Tokens.OpenParen);
                object node = this.Expression();
                this.Eat(Tokens.CloseParen);
                return node;
            }
            else if (token.Type == (int)Tokens.IncrementOp ||
                token.Type == (int)Tokens.DecrementOp)
            {
                if (this.CurrentToken.Type == (int)Tokens.IncrementOp)
                {
                    token.Type = (int)Tokens.PrefixIncOp;
                    this.Eat(Tokens.PrefixIncOp);
                }
                if (this.CurrentToken.Type == (int)Tokens.DecrementOp)
                {
                    token.Type = (int)Tokens.PrefixDecOp;
                    this.Eat(Tokens.PrefixDecOp);
                }

                object variable = this.Variable();

                object node = new AST.UnaryOperator(token, variable);
                return node;
            }
            else if (token.Type == (int)Tokens.Identifier)
            {
                object node = this.Variable();

                if (this.CurrentToken.Type == (int)Tokens.OpenParen)
                {
                    this.Eat(Tokens.OpenParen);

                    List<object> args = new List<object>();

                    while (this.CurrentToken.Type != (int)Tokens.CloseParen)
                    {
                        if (0 != args.Count())
                        {
                            this.Eat(Tokens.Comma);
                        }

                        args.Add(this.Expression());
                    }
                    this.Eat(Tokens.CloseParen);

                    node = new AST.Call(node as AST.Variable, args);
                }
                else if (this.CurrentToken.Type == (int)Tokens.IncrementOp)
                {
                    this.Eat(Tokens.IncrementOp);

                    token.Type = (int)Tokens.SuffixIncOp;

                    node = new AST.UnaryOperator(token, node);
                }
                else if (this.CurrentToken.Type == (int)Tokens.DecrementOp)
                {
                    this.Eat(Tokens.DecrementOp);

                    token.Type = (int)Tokens.SuffixDecOp;

                    node = new AST.UnaryOperator(token, node);
                }
                else if (this.CurrentToken.Type == (int)Tokens.OpenBracket)
                {
                    this.Eat(Tokens.OpenBracket);

                    node = new AST.ArrayElement(token, node as AST.Variable, this.Expression());

                    this.Eat(Tokens.CloseBracket);
                }

                return node;
            }

            return null;
        }

        public object Parse()
        {
            AST.Program program = this.Program();

            if (this.CurrentToken.Type != (int)Tokens.EOF)
            {
                this.Error(String.Format(Messages.UNEXPECTED, this.CurrentToken.Value));
            }

            if (0 == program.Compound.Children.Count())
            {
                return null;
            }

            return program;
        }
    }
}
