namespace FZYK.Core
{
    partial class FrmToolBoxXML
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ykTabControl1 = new FZYK.WinCtrl.YKTabControl(this.components);
            this.ykTabPage1 = new FZYK.WinCtrl.YKTabPage();
            this.treeControl1 = new Crownwood.DotNetMagic.Controls.TreeControl();
            this.ykTabPage2 = new FZYK.WinCtrl.YKTabPage();
            this.treeControl2 = new Crownwood.DotNetMagic.Controls.TreeControl();
            this.ykTabPage3 = new FZYK.WinCtrl.YKTabPage();
            this.treeControl3 = new Crownwood.DotNetMagic.Controls.TreeControl();
            this.ykTabControl1.SuspendLayout();
            this.ykTabPage1.SuspendLayout();
            this.ykTabPage2.SuspendLayout();
            this.ykTabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ykTabControl1
            // 
            this.ykTabControl1.Controls.Add(this.ykTabPage1);
            this.ykTabControl1.Controls.Add(this.ykTabPage2);
            this.ykTabControl1.Controls.Add(this.ykTabPage3);
            this.ykTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ykTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ykTabControl1.myBackColor = System.Drawing.Color.White;
            this.ykTabControl1.Name = "ykTabControl1";
            this.ykTabControl1.SelectedIndex = 0;
            this.ykTabControl1.Size = new System.Drawing.Size(291, 427);
            this.ykTabControl1.TabIndex = 0;
            // 
            // ykTabPage1
            // 
            this.ykTabPage1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ykTabPage1.Controls.Add(this.treeControl1);
            this.ykTabPage1.Location = new System.Drawing.Point(4, 26);
            this.ykTabPage1.Margin = new System.Windows.Forms.Padding(1);
            this.ykTabPage1.Name = "ykTabPage1";
            this.ykTabPage1.Padding = new System.Windows.Forms.Padding(1);
            this.ykTabPage1.Size = new System.Drawing.Size(283, 397);
            this.ykTabPage1.TabIndex = 0;
            this.ykTabPage1.Text = "基础设置";
            // 
            // treeControl1
            // 
            this.treeControl1.BorderStyle = Crownwood.DotNetMagic.Controls.TreeBorderStyle.None;
            this.treeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeControl1.GroupFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.treeControl1.HotBackColor = System.Drawing.Color.Empty;
            this.treeControl1.HotForeColor = System.Drawing.Color.Empty;
            this.treeControl1.Location = new System.Drawing.Point(1, 1);
            this.treeControl1.MinimumNodeHeight = 14;
            this.treeControl1.Name = "treeControl1";
            this.treeControl1.SelectedNode = null;
            this.treeControl1.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control;
            this.treeControl1.Size = new System.Drawing.Size(281, 395);
            this.treeControl1.TabIndex = 0;
            this.treeControl1.Text = "treeControl1";
            this.treeControl1.Click += new System.EventHandler(this.treeControl1_Click);
            // 
            // ykTabPage2
            // 
            this.ykTabPage2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ykTabPage2.Controls.Add(this.treeControl2);
            this.ykTabPage2.Location = new System.Drawing.Point(4, 26);
            this.ykTabPage2.Margin = new System.Windows.Forms.Padding(1);
            this.ykTabPage2.Name = "ykTabPage2";
            this.ykTabPage2.Padding = new System.Windows.Forms.Padding(1);
            this.ykTabPage2.Size = new System.Drawing.Size(283, 397);
            this.ykTabPage2.TabIndex = 1;
            this.ykTabPage2.Text = "供应链";
            // 
            // treeControl2
            // 
            this.treeControl2.BorderStyle = Crownwood.DotNetMagic.Controls.TreeBorderStyle.None;
            this.treeControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeControl2.GroupFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.treeControl2.HotBackColor = System.Drawing.Color.Empty;
            this.treeControl2.HotForeColor = System.Drawing.Color.Empty;
            this.treeControl2.Location = new System.Drawing.Point(1, 1);
            this.treeControl2.MinimumNodeHeight = 14;
            this.treeControl2.Name = "treeControl2";
            this.treeControl2.SelectedNode = null;
            this.treeControl2.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control;
            this.treeControl2.Size = new System.Drawing.Size(190, 68);
            this.treeControl2.TabIndex = 1;
            this.treeControl2.Text = "treeControl2";
            this.treeControl2.Click += new System.EventHandler(this.treeControl1_Click);
            // 
            // ykTabPage3
            // 
            this.ykTabPage3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ykTabPage3.Controls.Add(this.treeControl3);
            this.ykTabPage3.Location = new System.Drawing.Point(4, 26);
            this.ykTabPage3.Margin = new System.Windows.Forms.Padding(1);
            this.ykTabPage3.Name = "ykTabPage3";
            this.ykTabPage3.Padding = new System.Windows.Forms.Padding(1);
            this.ykTabPage3.Size = new System.Drawing.Size(283, 397);
            this.ykTabPage3.TabIndex = 2;
            this.ykTabPage3.Text = "OA";
            // 
            // treeControl3
            // 
            this.treeControl3.BorderStyle = Crownwood.DotNetMagic.Controls.TreeBorderStyle.None;
            this.treeControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeControl3.GroupFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.treeControl3.HotBackColor = System.Drawing.Color.Empty;
            this.treeControl3.HotForeColor = System.Drawing.Color.Empty;
            this.treeControl3.Location = new System.Drawing.Point(1, 1);
            this.treeControl3.MinimumNodeHeight = 14;
            this.treeControl3.Name = "treeControl3";
            this.treeControl3.SelectedNode = null;
            this.treeControl3.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control;
            this.treeControl3.Size = new System.Drawing.Size(190, 68);
            this.treeControl3.TabIndex = 1;
            this.treeControl3.Text = "treeControl3";
            this.treeControl3.Click += new System.EventHandler(this.treeControl1_Click);
            // 
            // FrmToolBoxXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 427);
            this.Controls.Add(this.ykTabControl1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmToolBoxXML";
            this.Text = "菜单";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmToolBoxXML_FormClosing);
            this.Load += new System.EventHandler(this.FrmToolBoxXML_Load);
            this.ykTabControl1.ResumeLayout(false);
            this.ykTabPage1.ResumeLayout(false);
            this.ykTabPage2.ResumeLayout(false);
            this.ykTabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WinCtrl.YKTabControl ykTabControl1;
        private WinCtrl.YKTabPage ykTabPage1;
        private Crownwood.DotNetMagic.Controls.TreeControl treeControl1;
        private WinCtrl.YKTabPage ykTabPage2;
        private Crownwood.DotNetMagic.Controls.TreeControl treeControl2;
        private WinCtrl.YKTabPage ykTabPage3;
        private Crownwood.DotNetMagic.Controls.TreeControl treeControl3;
    }
}