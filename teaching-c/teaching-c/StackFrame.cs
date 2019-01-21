using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST = teaching_c.AbstractSyntaxTree;

namespace teaching_c
{
    internal class StackFrame
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public Dictionary<string, List<object>> LOCAL_VARIABLES { get; set; }
        public List<AST.Param> ArgumentParameters { get; set; }
        public object ret { get; set; }
        public StackFrame EnclosingFrame { get; set; }

        public StackFrame(string Name, int Level, List<AST.Param> ArgumentParameters = null, StackFrame EnclosingFrame = null)
        {
            this.Name = Name;
            this.Level = Level;
            this.EnclosingFrame = EnclosingFrame;
            this.LOCAL_VARIABLES = new Dictionary<string, List<object>>();
            this.ArgumentParameters = new List<AST.Param>();
            if (null != ArgumentParameters)
            {
                this.ArgumentParameters = ArgumentParameters;
            }

            this.ret = null;

            this.InitMemory();
        }

        public void InitMemory()
        {
            foreach (AST.Param parameter in this.ArgumentParameters)
            {
                string paramname = (string)parameter.VariableNode.Value;
                this.Insert(paramname);
            }
        }

        public void Insert(string varname, object type = null)
        {
            this.LOCAL_VARIABLES.Add(varname, new List<object>());
            this.LOCAL_VARIABLES[varname].Add(null);
            this.LOCAL_VARIABLES[varname].Add(type);
        }

        public void Assign(string varname, object value)
        {
            if (this.LOCAL_VARIABLES.Keys.Contains(varname))
            {
                this.LOCAL_VARIABLES[varname][0] = value;
            }
            else
            {
                if (this.EnclosingFrame != null)
                {
                    this.EnclosingFrame.Assign(varname, value);
                }
            }
        }

        public object Lookup(string varname, bool CurrentFrameOnly = true, bool withExtra = false)
        {
            if (this.LOCAL_VARIABLES.Keys.Contains(varname))
            {
                if (withExtra)
                {
                    return this.LOCAL_VARIABLES[varname];
                }

                return this.LOCAL_VARIABLES[varname].ElementAt(0);
            }

            if (CurrentFrameOnly)
            {
                return null;
            }

            if (this.EnclosingFrame != null)
            {
                return this.EnclosingFrame.Lookup(varname, CurrentFrameOnly, withExtra);
            }

            return null;
        }
    }
}
