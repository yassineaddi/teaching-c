using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using AST = teaching_c.AbstractSyntaxTree;

namespace teaching_c
{
    public partial class Form1 : Form
    {
        public Console Diagnostics { get; set; }
        public Console Console { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            int fontlength = Properties.Resources.gohufont_uni_11.Length;
            byte[] fontdata = Properties.Resources.gohufont_uni_11;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontlength);
            Marshal.Copy(fontdata, 0, data, fontlength);
            pfc.AddMemoryFont(data, fontlength);

            Font font = new Font("", 13, GraphicsUnit.Pixel);
            source.Font = font;
            //source.Font = diagnos.Font = consoles.Font = font;

            this.Diagnostics = new Console(diagnostics);
            this.Console = new Console(console);

            source.Text = @"
int main() {

	return 0;
}";
            source.Focus();
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            source.Focus();
        }

        private void start_Click(object sender, EventArgs e)
        {
            this.Diagnostics.Clear();
            this.Console.Clear();
            memory.Controls.Clear();
            tabControl1.SelectedTab = diagnostics;

            try
            {
                Lexer Lexer = new Lexer(source.Text);
                Parser Parser = new Parser(Lexer);
                object Tree = Parser.Parse();

                SemanticAnalyzer SemanticAnalyzer = new SemanticAnalyzer();
                SemanticAnalyzer.Visit(Tree);

                Interpreter Interpreter = new Interpreter(this);
                object result = Interpreter.Interpret(Tree);

                if (null != result)
                {
                    Diagnostics.WriteLn(String.Format("\nfinished with exit code {0}", result));
                }
            }
            catch (Exceptions.LexerException ex)
            {
                this.Diagnostics.WriteLn(ex.Message);
            }
            catch (Exceptions.ParserException ex)
            {
                this.Diagnostics.WriteLn(ex.Message);
            }
            catch (Exceptions.SemanticAnalyzerException ex)
            {
                this.Diagnostics.WriteLn(ex.Message);
            }
            catch (Exceptions.InterpreterException ex)
            {
                this.Diagnostics.WriteLn(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string FormatData(object obj)
        {
            if (obj is int)
            {
                return obj.ToString();
            }
            else if (obj is float)
            {
                return ((float)obj).ToString("0.00###############").Replace(',', '.');
            }
            else if (obj is double)
            {
                return ((double)obj).ToString("0.00##############").Replace(',', '.');
            }
            else if (obj is char)
            {
                char c = Convert.ToChar(obj);
                switch (c)
                {
                    case '\n': return "'\\n'";
                    case '\t': return "'\\t'";
                    case '\0': return "'\\0'";
                    default: return String.Format("'{0}'", c);
                }
            }

            return obj.ToString();
        }

        public void MarkSingleLine(int Line, int Column)
        {
            source.SelectAll();
            source.SelectionBackColor = System.Drawing.Color.White;
            int firstcharindex = source.GetFirstCharIndexFromLine(Line - 1);
            string currentline = source.Lines[Line - 1];
            source.Select(firstcharindex, currentline.Length);
            source.SelectionBackColor = System.Drawing.Color.Yellow;
            source.SelectionStart = firstcharindex;
            source.SelectionLength = 0;

            source.Update();
        }

        internal void Visualize(Stack<StackFrame> CallStack)
        {
            memory.Controls.Clear();

            List<StackFrame> frames = new List<StackFrame>();

            StackFrame enclosing = CallStack.Peek().EnclosingFrame;
            while (null != enclosing)
            {
                frames.Add(enclosing);
                enclosing = enclosing.EnclosingFrame;
            }
            frames.Reverse();
            frames.Add(CallStack.Peek());

            foreach (StackFrame frame in frames)
            {
                MyGroupBox s = new MyGroupBox();
                s.Top = 5;
                s.Left = 5;
                s.Height = 50;
                s.Width = 100;
                s.Padding = new Padding(0, 0, 10, 0);
                s.AutoSize = true;
                s.Text = frame.Name;
                if (memory.Controls.Count > 0)
                {
                    MyGroupBox ls = memory.Controls[memory.Controls.Count - 1] as MyGroupBox;
                    s.Top = ls.Top + ls.Height + 5;
                }

                foreach (KeyValuePair<string, List<object>> var in frame.LOCAL_VARIABLES)
                {
                    MyGroupBox v = new MyGroupBox();
                    v.Height = 50;
                    v.Width = 70;
                    v.Left = 10 + s.Left;
                    v.Top = 15;
                    if (s.Controls.Count > 0)
                    {
                        MyGroupBox lv = s.Controls[s.Controls.Count - 1] as MyGroupBox;
                        v.Top = lv.Top + lv.Height + 5;
                    }
                    v.AutoSize = true;
                    v.Text = var.Key;

                    s.Controls.Add(v);

                    MyLabel d = new MyLabel();
                    d.Dock = DockStyle.Fill;
                    d.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    d.Text = (null == frame.Lookup(var.Key)) ? "" : this.FormatData(frame.Lookup(var.Key));
                    Size size = TextRenderer.MeasureText(d.Text, d.Font);
                    if (d.Width - v.Padding.Vertical < size.Width + size.Width / 2)
                    {
                        d.AutoSize = true;
                    }

                    v.Controls.Add(d);
                }

                memory.Controls.Add(s);
            }

            memory.Update();
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text,
                e.Font,
                Brushes.Black,
                e.Bounds.Left,
                e.Bounds.Top + e.Bounds.Height / 4,
                StringFormat.GenericDefault);
        }
    }

    public class MyGroupBox : GroupBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            base.OnPaint(e);
        }
    }

    public class MyLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            base.OnPaint(e);
        }
    }

    public class MyButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            base.OnPaint(e);
        }
    }
}
