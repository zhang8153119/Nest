namespace FZYK.Core
{
    partial class FrmToolBoxCheJinXML
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
            this.treeControl1 = new Crownwood.DotNetMagic.Controls.TreeControl();
            this.SuspendLayout();
            // 
            // treeControl1
            // 
            this.treeControl1.BorderStyle = Crownwood.DotNetMagic.Controls.TreeBorderStyle.None;
            this.treeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeControl1.GroupFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.treeControl1.HotBackColor = System.Drawing.Color.Empty;
            this.treeControl1.HotForeColor = System.Drawing.Color.Empty;
            this.treeControl1.Location = new System.Drawing.Point(0, 0);
            this.treeControl1.MinimumNodeHeight = 14;
            this.treeControl1.Name = "treeControl1";
            this.treeControl1.SelectedNode = null;
            this.treeControl1.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control;
            this.treeControl1.Size = new System.Drawing.Size(246, 447);
            this.treeControl1.TabIndex = 2;
            this.treeControl1.Text = "treeControl2";
            this.treeControl1.Click += new System.EventHandler(this.treeControl1_Click);
            // 
            // FrmToolBoxCheJinXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 447);
            this.Controls.Add(this.treeControl1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmToolBoxCheJinXML";
            this.Text = "菜单";
            this.Load += new System.EventHandler(this.FrmToolBoxCheJinXML_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Crownwood.DotNetMagic.Controls.TreeControl treeControl1;
    }
}