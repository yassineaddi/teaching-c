using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST = teaching_c.AbstractSyntaxTree;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;

namespace teaching_c
{
    class Interpreter
    {
        public object Tree { get; set; }
        public Stack<StackFrame> CallStack { get; set; }
        public Dictionary<string, AST.SubroutineDeclaration> Subroutines { get; set; }
        public Form1 f { get; set; }

        public Interpreter(Form1 f)
        {
            this.f = f;
            this.CallStack = new Stack<StackFrame>();
            this.Subroutines = new Dictionary<string, AST.SubroutineDeclaration>();
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

        private void Error(string message, TokenPosition position = null)
        {
            if (position == null)
            {
                throw new Exceptions.InterpreterException(message);
            }
            throw new Exceptions.InterpreterException(
                string.Format("{0} at ({1}, {2})",
                message,
                position.Line,
                position.Column));
        }

        public object VisitProgram(AST.Program Node)
        {
            this.CallStack.Push(new StackFrame("global", 1));

            foreach (object Child in Node.Compound.Children)
            {
                if (!(Child is AST.SubroutineDeclaration))
                {
                    this.Visit(Child);
                }
                else
                {
                    AST.SubroutineDeclaration subroutine = Child as AST.SubroutineDeclaration;
                    string subname = (string)subroutine.SubroutineName.Value;
                    if (null != subroutine.Body)
                    {
                        this.Subroutines.Add(subname, subroutine);
                    }
                }
            }

            object result = this.VisitSubroutineDeclaration(this.Subroutines["main"]);

            this.CallStack.Pop();

            return result;
        }

        public object VisitCompound(AST.Compound Node)
        {
            foreach (object Child in Node.Children)
            {
                if (Child is AST.NullStatement)
                {
                    continue;
                }

                this.f.Visualize(this.CallStack);

                TokenPosition position = (Child as AST.AbstractSyntaxTree).Token.Position;
                this.f.MarkSingleLine(position.Line, position.Column);

                System.Threading.Thread.Sleep(1000 * this.f.trackBar1.Value);

                this.Visit(Child);

                if (Child is AST.ReturnStatement ||
                    null != this.CallStack.Peek().ret)
                {
                    break;
                }
            }

            return null;
        }

        public object VisitCompoundStatement(AST.CompoundStatement Node)
        {
            StackFrame sf = new StackFrame(
                "",
                this.CallStack.Peek().Level + 1,
                null,
                this.CallStack.Peek());

            this.CallStack.Push(sf);

            this.VisitCompound(Node.Body);

            object ret = (this.CallStack.Pop()).ret;

            this.CallStack.Peek().ret = ret;

            return ret;
        }

        public object VisitNullStatement(AST.NullStatement Node)
        {
            return null;
        }

        public object VisitConditionalStatement(AST.ConditionalStatement Node)
        {
            bool expr = Convert.ToBoolean(this.Visit(Node.Expression));
            AST.Compound compound;
            int level = this.CallStack.Peek().Level + 1;
            StackFrame sf;
            object ret;

            if (expr)
            {
                sf = new StackFrame("if", level, null, this.CallStack.Peek());
                compound = Node.If;
            }
            else if (null != Node.Else)
            {
                sf = new StackFrame("else", level, null, this.CallStack.Peek());
                compound = Node.Else;
            }
            else
            {
                return null;
            }

            this.CallStack.Push(sf);

            this.VisitCompound(compound);

            ret = (this.CallStack.Pop()).ret;

            this.CallStack.Peek().ret = ret;

            return ret;
        }

        public object VisitWhileLoop(AST.WhileLoop Node)
        {
            bool expr = Convert.ToBoolean(this.Visit(Node.Expression));

            while (expr)
            {
                StackFrame sf = new StackFrame(
                    "while",
                    this.CallStack.Peek().Level + 1,
                    null,
                    this.CallStack.Peek());
                object ret = null;

                this.CallStack.Push(sf);

                this.VisitCompound(Node.Body);

                ret = (this.CallStack.Pop()).ret;

                if (null != ret)
                {
                    this.CallStack.Peek().ret = ret;
                    return ret;
                }

                expr = Convert.ToBoolean(this.Visit(Node.Expression));
            }

            return null;
        }

        public object VisitForLoop(AST.ForLoop Node)
        {
            object init = this.Visit(Node.Init);
            object condition = this.Visit(Node.Condition);
            if (condition == null)
            {
                condition = true;
            }
            else
            {
                condition = Convert.ToBoolean(this.Visit(Node.Condition));
            }

            while ((bool)condition)
            {
                StackFrame sf = new StackFrame(
                    "for",
                    this.CallStack.Peek().Level + 1,
                    null,
                    this.CallStack.Peek());
                object ret = null;

                this.CallStack.Push(sf);

                this.VisitCompound(Node.Body);

                ret = (this.CallStack.Pop()).ret;

                if (null != ret)
                {
                    this.CallStack.Peek().ret = ret;
                    return ret;
                }

                this.Visit(Node.Increment);
                condition = this.Visit(Node.Condition);
                if (condition == null)
                {
                    condition = true;
                }
                else
                {
                    condition = Convert.ToBoolean(this.Visit(Node.Condition));
                }
            }

            return null;
        }

        public object VisitDoWhileLoop(AST.DoWhileLoop Node)
        {
            bool expr;

            do {
                StackFrame sf = new StackFrame(
                    "do while",
                    this.CallStack.Peek().Level + 1,
                    null,
                    this.CallStack.Peek());
                object ret = null;

                this.CallStack.Push(sf);

                this.VisitCompound(Node.Body);

                ret = (this.CallStack.Pop()).ret;

                if (null != ret)
                {
                    this.CallStack.Peek().ret = ret;
                    return ret;
                }

                expr = Convert.ToBoolean(this.Visit(Node.Expression));
            } while(expr);

            return null;
        }

        public object VisitVariableDeclaration(AST.VariableDeclaration Node)
        {
            string varname = Node.VariableNode.Token.Value as string;
            object vartype = Node.TypeNode.Value;

            this.CallStack.Peek().Insert(varname, vartype);

            if (this.CallStack.Peek().Level == 1)
            {
                string type = (string)vartype;
                if (type == "int")
                {
                    this.CallStack.Peek().Assign(varname, (int)0);
                }
                else if (type == "float")
                {
                    this.CallStack.Peek().Assign(varname, (float)0.00);
                }
                else if (type == "double")
                {
                    this.CallStack.Peek().Assign(varname, (double)0.00);
                }
                else if (type == "char")
                {
                    this.CallStack.Peek().Assign(varname, (char)'\0');
                }
            }

            return null;
        }

        public object VisitAssign(AST.Assign Node)
        {
            string varname = (string)Node.Left.Token.Value;
            object value = this.Visit(Node.Right);
            List<object> var = this.CallStack.Peek().Lookup(varname, false, true) as List<object>;
            string vartype = (string)var.ElementAt(1);

            value = this.ConvertAppr(value, vartype);

            this.CallStack.Peek().Assign(varname, value);

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
            dynamic left = this.Visit(Node.Left);
            dynamic right = this.Visit(Node.Right);

            if (Node.Operator.Type == (int)Tokens.Addition)
            {
                return left + right;
            }
            if (Node.Operator.Type == (int)Tokens.Subtraction)
            {
                return left - right;
            }
            if (Node.Operator.Type == (int)Tokens.Multiplication)
            {
                return left * right;
            }
            if (Node.Operator.Type == (int)Tokens.Division)
            {
                return left / right;
            }
            if (Node.Operator.Type == (int)Tokens.Modulus)
            {
                return left % right;
            }
            if (Node.Operator.Type == (int)Tokens.GreaterOp)
            {
                return (left > right) ? 1: 0;
            }
            if (Node.Operator.Type == (int)Tokens.GreaterOp)
            {
                return (left > right) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.GreaterEqOp)
            {
                return (left >= right) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.LessOp)
            {
                return (left < right) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.LessEqOp)
            {
                return (left <= right) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.Equal)
            {
                return (left == right) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.NotEqual)
            {
                return (left != right) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.LogicalAnd)
            {
                return (Convert.ToBoolean(left) && Convert.ToBoolean(right)) ? 1 : 0;
            }
            if (Node.Operator.Type == (int)Tokens.LogicalOr)
            {
                return (Convert.ToBoolean(left) || Convert.ToBoolean(right)) ? 1 : 0;
            }

            return null;
        }

        public object VisitUnaryOperator(AST.UnaryOperator Node)
        {
            dynamic expr = this.Visit(Node.Expression);

            if (Node.Operator.Type == (int)Tokens.Addition)
            {
                return +expr;
            }
            if (Node.Operator.Type == (int)Tokens.Subtraction)
            {
                return -expr;
            }
            if (Node.Operator.Type == (int)Tokens.SuffixIncOp)
            {
                string varname = (string)(Node.Expression as AST.Variable).Value;
                
                this.CallStack.Peek().Assign(varname, expr + 1);

                return expr;
            }
            if (Node.Operator.Type == (int)Tokens.SuffixDecOp)
            {
                string varname = (string)(Node.Expression as AST.Variable).Value;

                this.CallStack.Peek().Assign(varname, expr - 1);

                return expr;
            }
            if (Node.Operator.Type == (int)Tokens.PrefixIncOp)
            {
                string varname = (string)(Node.Expression as AST.Variable).Value;

                this.CallStack.Peek().Assign(varname, expr + 1);

                return expr + 1;
            }
            if (Node.Operator.Type == (int)Tokens.PrefixDecOp)
            {
                string varname = (string)(Node.Expression as AST.Variable).Value;

                this.CallStack.Peek().Assign(varname, expr - 1);

                return expr - 1;
            }

            return null;
        }

        public object VisitVariable(AST.Variable Node)
        {
            string varname = (string)Node.Value;

            return this.CallStack.Peek().Lookup(varname, false);
        }

        public object VisitSubroutineDeclaration(AST.SubroutineDeclaration Node, AST.Call Call = null)
        {
            StackFrame sf = new StackFrame(
                (string)Node.SubroutineName.Value,
                this.CallStack.Peek().Level + 1,
                Node.Params,
                this.CallStack.Peek());

            if (null != Call)
            {
                for (int i = 0; i < Call.Params.Count; i++)
                {
                    string varname = (string)Node.Params[i].VariableNode.Value;
                    string vartype = (string)Node.Params[i].TypeNode.Value;
                    object value = this.ConvertAppr(this.Visit(Call.Params[i]), vartype);

                    sf.Assign(varname, value);
                }
            }

            this.CallStack.Push(sf);

            this.VisitCompound(Node.Body);

            if (null == this.CallStack.Peek().ret)
            {
                this.Error(
                    String.Format(Messages.MISSING_RETURN_STMT,
                    Node.SubroutineName.Value),
                    Node.SubroutineName.Token.Position);
            }

            object ret = (this.CallStack.Pop()).ret;

            string rettype = (string)Node.ReturnType.Value;

            return this.ConvertAppr(ret, rettype);
        }

        public object VisitReturnStatement(AST.ReturnStatement Node)
        {
            this.CallStack.Peek().ret = this.Visit(Node.Expression);

            return null;
        }

        public object VisitCall(AST.Call Node)
        {
            string subname = (string)Node.SubroutineName.Value;

            if (!this.Subroutines.Keys.Contains(subname))
            {
                this.Error(
                    String.Format(Messages.UNDEFINED_FUNC,
                    subname),
                    Node.SubroutineName.Token.Position);
            }

            return this.VisitSubroutineDeclaration(this.Subroutines[subname], Node);
        }

        public object ConvertAppr(object obj, string type)
        {
            if (type == "int")
            {
                if (obj is char)
                {
                    return (int)Convert.ToChar(obj);
                }

                return (int)Convert.ToDouble(obj);
            }
            else if (type == "float")
            {
                if (obj is char)
                {
                    return Convert.ToSingle((int)Convert.ToChar(obj));
                }

                return Convert.ToSingle(obj);
            }
            else if (type == "double")
            {
                if (obj is char)
                {
                    return Convert.ToDouble((int)Convert.ToChar(obj));
                }

                return Convert.ToDouble(obj);
            }
            else if (type == "char")
            {
                return Convert.ToChar(obj);
            }

            return obj;
        }

        public object Interpret(object Tree)
        {
            this.Tree = Tree;

            if (this.Tree == null)
            {
                return null;
            }

            return this.Visit(Tree);
        }
    }
}
