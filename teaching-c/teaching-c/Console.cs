using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace teaching_c
{
    public class Console
    {
        private TabPage tp { get; set; }
        private RichTextBox console { get; set; }

        public Console(TabPage tp)
        {
            this.tp = tp;
            this.console = tp.Controls[0] as RichTextBox;
        }

        private void Reset()
        {
            int lastindex = this.tp.Text.Count() - 1;

            if (this.tp.Text[lastindex] == ')')
            {
                int startindex = lastindex - 1;

                while (Char.IsNumber(this.tp.Text[startindex]))
                {
                    startindex--;
                }

                this.tp.Text = String.Format("{0}",
                    this.tp.Text.Substring(0, startindex - 1));
            }
        }

        private void Records()
        {
            int linecount = 0;

            foreach (string line in this.console.Lines)
            {
                if ( ! String.IsNullOrWhiteSpace(line))
                {
                    linecount++;
                }
            }

            this.Reset();

            this.tp.Text += String.Format(" ({0})", linecount);
        }

        public void Write(string message)
        {
            this.console.Text += message;

            this.Records();
        }

        public void WriteLn(string message)
        {
            this.console.Text += message + "\n";
            
            this.Records();
        }

        public void Clear()
        {
            this.Reset();

            this.console.Clear();
        }
    }
}
