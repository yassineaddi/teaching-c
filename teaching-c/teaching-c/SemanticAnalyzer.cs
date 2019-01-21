using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST = teaching_c.AbstractSyntaxTree;
using teaching_c.Symbols;

namespace teaching_c
{
    class SemanticAnalyzer
    {
        public ScopedSymbolTable CurrentScope { get; set; }

        public SemanticAnalyzer()
        {
            this.CurrentScope = null;
        }

        private void Error(string message, TokenPosition position = null)
        {
            if (position == null)
            {
                throw new Exceptions.SemanticAnalyzerException(message);
            }
            throw new Exceptions.SemanticAnalyzerException(
                string.Format("{0} at ({1}, {2})",
                message,
                position.Line,
                position.Column));
        }

        public object Visit(object Node)
        {
            if (Node == null)
            {
                return null;
            }

            if (Node is AST.Program)
            {
                return this.VisitProgram(Node as AST.Program);
            }
            if (Node is AST.Compound)
            {
                return this.VisitCompound(Node as AST.Compound);
            }
            if (Node is AST.CompoundStatement)
            {
                return this.VisitCompoundStatement(Node as AST.CompoundStatement);
            }
            if (Node is AST.NullStatement)
            {
                return this.VisitNullStatement(Node as AST.NullStatement);
            }
            if (Node is AST.ConditionalStatement)
            {
                return this.VisitConditionalStatement(Node as AST.ConditionalStatement);
            }
            if (Node is AST.WhileLoop)
            {
                return this.VisitWhileLoop(Node as AST.WhileLoop);
            }
            if (Node is AST.ForLoop)
            {
                return this.VisitForLoop(Node as AST.ForLoop);
            }
            if (Node is AST.DoWhileLoop)
            {
                return this.VisitDoWhileLoop(Node as AST.DoWhileLoop);
            }
            if (Node is AST.VariableDeclaration)
            {
                return this.VisitVariableDeclaration(Node as AST.VariableDeclaration);
            }
            if (Node is AST.Assign)
            {
                return this.VisitAssign(Node as AST.Assign);
            }
            if (Node is AST.Number)
            {
                return this.VisitNumber(Node as AST.Number);
            }
            if (Node is AST.Character)
            {
                return this.VisitCharacter(Node as AST.Character);
            }
            if (Node is AST.BinaryOperator)
            {
                return this.VisitBinaryOperator(Node as AST.BinaryOperator);
            }
            if (Node is AST.UnaryOperator)
            {
                return this.VisitUnaryOperator(Node as AST.UnaryOperator);
            }
            if (Node is AST.Variable)
            {
                return this.VisitVariable(Node as AST.Variable);
            }
            if (Node is AST.SubroutineDeclaration)
            {
                return this.VisitSubroutineDeclaration(Node as AST.SubroutineDeclaration);
            }
            if (Node is AST.ReturnStatement)
            {
                return this.VisitReturnStatement(Node as AST.ReturnStatement);
            }
            if (Node is AST.Call)
            {
                return this.VisitCall(Node as AST.Call);
            }

            throw new Exception(
                String.Format("No Visit{0} method",
                Node.GetType().ToString().Split('.').Last()));
        }

        public object VisitProgram(AST.Program Node)
        {
            ScopedSymbolTable globalscope = new ScopedSymbolTable("global", 1);

            globalscope.InitBuiltins();

            this.CurrentScope = globalscope;

            this.Visit(Node.Compound);

            Symbol entrypoint = this.CurrentScope.Lookup("main", true);
            if (entrypoint == null || !(entrypoint is SubroutineSymbol))
            {
                this.Error(Messages.UNDEFINED_REFERENCE);
            }
            if (entrypoint.Type.Name != Enum.GetName(typeof(Tokens), Tokens.Int).ToLower())
            {
                this.Error(Messages.MUST_RETURN_INT);
            }
            if (0 != (entrypoint as SubroutineSymbol).Parameters.Count())
            {
                this.Error(Messages.CANNOT_HAVE_PARAMS);
            }

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitCompound(AST.Compound Node)
        {
            foreach (object Child in Node.Children)
            {
                this.Visit(Child);
            }

            return null;
        }

        public object VisitCompoundStatement(AST.CompoundStatement Node)
        {
            ScopedSymbolTable scope = new ScopedSymbolTable(
                "",
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = scope;

            this.Visit(Node.Body);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitConditionalStatement(AST.ConditionalStatement Node)
        {
            this.Visit(Node.Expression);

            ScopedSymbolTable ifscope = new ScopedSymbolTable(
                "if",
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = ifscope;

            this.Visit(Node.If);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            ScopedSymbolTable elsescope = new ScopedSymbolTable(
                "else",
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = elsescope;

            this.Visit(Node.Else);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitWhileLoop(AST.WhileLoop Node)
        {
            this.Visit(Node.Expression);

            ScopedSymbolTable whilescope = new ScopedSymbolTable(
                "while",
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = whilescope;

            this.Visit(Node.Body);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitForLoop(AST.ForLoop Node)
        {
            this.Visit(Node.Init);
            this.Visit(Node.Condition);
            this.Visit(Node.Increment);

            ScopedSymbolTable forscope = new ScopedSymbolTable(
                "for",
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = forscope;

            this.Visit(Node.Body);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitDoWhileLoop(AST.DoWhileLoop Node)
        {
            this.Visit(Node.Expression);

            ScopedSymbolTable whilescope = new ScopedSymbolTable(
                "do while",
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = whilescope;

            this.Visit(Node.Body);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitNullStatement(AST.NullStatement Node)
        {
            return null;
        }

        public object VisitVariableDeclaration(AST.VariableDeclaration Node)
        {
            string typename = (string)Node.TypeNode.Token.Value;
            Symbol typesymbol = this.CurrentScope.Lookup(typename);

            string varname = (string)Node.VariableNode.Value;
            VariableSymbol varsymbol = new VariableSymbol(varname, typesymbol);

            if (this.CurrentScope.Lookup(varname, true) != null)
            {
                this.Error(String.Format(Messages.DUPLICATE_DECLARATION, varname),
                    Node.VariableNode.Token.Position);
            }

            if (this.CurrentScope.ScopeLevel == 1)
            {
                varsymbol.initialized = true;
            }

            this.CurrentScope.Insert(varsymbol);

            return null;
        }

        public object VisitAssign(AST.Assign Node)
        {
            string varname = (string)Node.Left.Value;
            VariableSymbol varsymbol = this.CurrentScope.Lookup(varname) as VariableSymbol;
            if (null == varsymbol)
            {
                this.Error(String.Format(Messages.UNDECLARED, varname), Node.Left.Token.Position);
            }

            this.Visit(Node.Right);

            varsymbol.initialized = true;

            return null;
        }

        public object VisitNumber(AST.Number Node)
        {
            return Node.Value;
        }

        public object VisitCharacter(AST.Character Node)
        {
            return Node.Value;
        }

        public object VisitBinaryOperator(AST.BinaryOperator Node)
        {
            object left = this.Visit(Node.Left);
            object right = this.Visit(Node.Right);

            if (Node.Operator.Type == (int)Tokens.Modulus &&
                (left is decimal || right is decimal))
            {
                this.Error(Messages.INVALID_OPS_MODULUS, Node.Operator.Position);
            }

            this.Visit(Node.Left);
            this.Visit(Node.Right);

            return null;
        }

        public object VisitUnaryOperator(AST.UnaryOperator Node)
        {
            return this.Visit(Node.Expression);
        }

        public object VisitVariable(AST.Variable Node)
        {
            string varname = (string)Node.Value;
            VariableSymbol varsymbol = this.CurrentScope.Lookup(varname) as VariableSymbol;
            if (varsymbol == null)
            {
                this.Error(String.Format(Messages.UNDECLARED, varname), Node.Token.Position);
            }
            if (false == varsymbol.initialized)
            {
                this.Error(
                    String.Format(Messages.UNINTIALIZED,
                    varname),
                    Node.Token.Position);
            }

            return null;
        }

        public object VisitSubroutineDeclaration(AST.SubroutineDeclaration Node)
        {
            string subname = (string)Node.SubroutineName.Value;
            string typename = (string)Node.ReturnType.Token.Value;
            Symbol typesymbol = this.CurrentScope.Lookup((string)Node.ReturnType.Token.Value);

            if (null != Node.Body &&
                0 != Node.Params.Count())
            {
                foreach (AbstractSyntaxTree.Param parameter in Node.Params)
                {
                    if (null == parameter.VariableNode)
                    {
                        this.Error(
                            String.Format(Messages.MISSING_NAME_PARAM,
                            subname),
                            parameter.TypeNode.Token.Position);
                    }
                }
            }

            SubroutineSymbol subroutine = this.CurrentScope.Lookup(subname, true) as SubroutineSymbol;
            if (subroutine != null)
            {
                if (true == subroutine.defined || Node.Body == null)
                {
                    this.Error(
                        String.Format(Messages.DUPLICATE_DECLARATION,
                        subname),
                        Node.SubroutineName.Token.Position);
                }
                if (subroutine.Type.Name != typename ||
                    subroutine.param() != Node.param())
                {
                    this.Error(
                        String.Format(Messages.FUNC_SIGNATURE,
                        subname),
                        Node.SubroutineName.Token.Position);
                }
            }

            SubroutineSymbol subsymbol = new SubroutineSymbol(subname, typesymbol);
            if (Node.Body == null)
            {
                subsymbol.defined = false;
            }
            this.CurrentScope.Insert(subsymbol);

            ScopedSymbolTable subscope = new ScopedSymbolTable(
                subname,
                this.CurrentScope.ScopeLevel + 1,
                this.CurrentScope);
            this.CurrentScope = subscope;

            foreach (AST.Param parameter in Node.Params)
            {
                Symbol paramtype = this.CurrentScope.Lookup((string)parameter.TypeNode.Token.Value);
                string paramname = null;
                if (null != parameter.VariableNode)
                {
                    paramname = (string)parameter.VariableNode.Value;
                }
                VariableSymbol varsymbol = new VariableSymbol(paramname, paramtype);
                varsymbol.initialized = true;

                if (null != paramname)
                {
                    this.CurrentScope.Insert(varsymbol);
                }

                subsymbol.Parameters.Add(varsymbol);
            }

            this.Visit(Node.Body);

            this.CurrentScope = this.CurrentScope.EnclosingScope;

            return null;
        }

        public object VisitCall(AST.Call Node)
        {
            string subname = (string)Node.SubroutineName.Value;
            SubroutineSymbol subcall = this.CurrentScope.Lookup(subname) as SubroutineSymbol;

            foreach (object parameter in Node.Params)
            {
                this.Visit(parameter);
            }

            if (null == subcall)
            {
                VariableSymbol var = this.CurrentScope.Lookup(subname) as VariableSymbol;

                if (null != var)
                {
                    this.Error(
                    String.Format(Messages.NOT_FUNC,
                    subname),
                    Node.SubroutineName.Token.Position);
                }

                this.Error(
                    String.Format(Messages.UNDECLARED_FUNC,
                    subname),
                    Node.SubroutineName.Token.Position);
            }
            if (subcall.Parameters.Count() != Node.Params.Count())
            {
                this.Error(
                    String.Format(Messages.FUNC_ARGS,
                    subname,
                    subcall.Parameters.Count(),
                    Node.Params.Count()));
            }

            return null;
        }

        public object VisitReturnStatement(AST.ReturnStatement Node)
        {
            return this.Visit(Node.Expression);
        }
    }
}
