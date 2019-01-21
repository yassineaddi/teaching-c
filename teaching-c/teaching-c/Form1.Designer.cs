namespace teaching_c
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.memory = new System.Windows.Forms.Panel();
            this.source = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.diagnostics = new System.Windows.Forms.TabPage();
            this.diagnos = new System.Windows.Forms.RichTextBox();
            this.console = new System.Windows.Forms.TabPage();
            this.consoles = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.start = new teaching_c.MyButton();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.diagnostics.SuspendLayout();
            this.console.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2MinSize = 120;
            this.splitContainer1.Size = new System.Drawing.Size(747, 552);
            this.splitContainer1.SplitterDistance = 289;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.memory);
            this.splitContainer2.Panel1MinSize = 50;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.source);
            this.splitContainer2.Panel2MinSize = 70;
            this.splitContainer2.Size = new System.Drawing.Size(747, 289);
            this.splitContainer2.SplitterDistance = 361;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // memory
            // 
            this.memory.AutoScroll = true;
            this.memory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memory.Location = new System.Drawing.Point(0, 0);
            this.memory.Name = "memory";
            this.memory.Size = new System.Drawing.Size(357, 285);
            this.memory.TabIndex = 0;
            // 
            // source
            // 
            this.source.AcceptsTab = true;
            this.source.BackColor = System.Drawing.Color.White;
            this.source.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.source.DetectUrls = false;
            this.source.Dock = System.Windows.Forms.DockStyle.Fill;
            this.source.Location = new System.Drawing.Point(0, 0);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(378, 285);
            this.source.TabIndex = 0;
            this.source.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(743, 255);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.diagnostics);
            this.tabControl1.Controls.Add(this.console);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(0, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(743, 171);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            // 
            // diagnostics
            // 
            this.diagnostics.AutoScroll = true;
            this.diagnostics.Controls.Add(this.diagnos);
            this.diagnostics.Location = new System.Drawing.Point(4, 22);
            this.diagnostics.Name = "diagnostics";
            this.diagnostics.Padding = new System.Windows.Forms.Padding(3);
            this.diagnostics.Size = new System.Drawing.Size(735, 145);
            this.diagnostics.TabIndex = 0;
            this.diagnostics.Text = "Diagnostics";
            this.diagnostics.UseVisualStyleBackColor = true;
            // 
            // diagnos
            // 
            this.diagnos.BackColor = System.Drawing.Color.White;
            this.diagnos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.diagnos.DetectUrls = false;
            this.diagnos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnos.Location = new System.Drawing.Point(3, 3);
            this.diagnos.Name = "diagnos";
            this.diagnos.ReadOnly = true;
            this.diagnos.Size = new System.Drawing.Size(729, 139);
            this.diagnos.TabIndex = 1;
            this.diagnos.Text = "";
            // 
            // console
            // 
            this.console.AutoScroll = true;
            this.console.Controls.Add(this.consoles);
            this.console.Location = new System.Drawing.Point(4, 22);
            this.console.Name = "console";
            this.console.Padding = new System.Windows.Forms.Padding(3);
            this.console.Size = new System.Drawing.Size(735, 145);
            this.console.TabIndex = 1;
            this.console.Text = "Console";
            this.console.UseVisualStyleBackColor = true;
            // 
            // consoles
            // 
            this.consoles.BackColor = System.Drawing.Color.White;
            this.consoles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.consoles.DetectUrls = false;
            this.consoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoles.Location = new System.Drawing.Point(3, 3);
            this.consoles.Name = "consoles";
            this.consoles.ReadOnly = true;
            this.consoles.Size = new System.Drawing.Size(729, 139);
            this.consoles.TabIndex = 0;
            this.consoles.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.start);
            this.panel2.Controls.Add(this.trackBar1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(743, 84);
            this.panel2.TabIndex = 0;
            // 
            // start
            // 
            this.start.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.start.Location = new System.Drawing.Point(327, 45);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.Location = new System.Drawing.Point(0, 0);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(743, 84);
            this.trackBar1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 552);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.diagnostics.ResumeLayout(false);
            this.console.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage diagnostics;
        private System.Windows.Forms.TabPage console;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox diagnos;
        private System.Windows.Forms.RichTextBox consoles;
        private MyButton start;
        public System.Windows.Forms.RichTextBox source;
        public System.Windows.Forms.Panel memory;
        public System.Windows.Forms.TrackBar trackBar1;








    }
}

