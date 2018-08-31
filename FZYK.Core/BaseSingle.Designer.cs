namespace FZYK.Core
{
    partial class BaseSingle
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
              this .tlspBaseSingle = new System .Windows .Forms .ToolStrip();
              this .tsbtnSave = new System .Windows .Forms .ToolStripButton();
              this .tsbtnAdd = new System .Windows .Forms .ToolStripButton();
              this .tsbtnDelete = new System .Windows .Forms .ToolStripButton();
              this .tsbtnAudit = new System .Windows .Forms .ToolStripButton();
              this .tsbtnUnAudit = new System .Windows .Forms .ToolStripButton();
              this .tsbtnPrint = new System .Windows .Forms .ToolStripButton();
              this .tsbtnTools = new System .Windows .Forms .ToolStripDropDownButton();
              this .tsbtnImport = new System .Windows .Forms .ToolStripMenuItem();
              this .tsbtnBack = new System .Windows .Forms .ToolStripButton();
              this .tsbtnForword = new System .Windows .Forms .ToolStripButton();
              this .tsbtnNotify = new System .Windows .Forms .ToolStripButton();
              this .tsbtnSign = new System .Windows .Forms .ToolStripButton();
              this .tsbtnClose = new System .Windows .Forms .ToolStripButton();
              this .tlspBaseSingle .SuspendLayout();
              this .SuspendLayout();
              // 
              // tlspBaseSingle
              // 
              this .tlspBaseSingle .Items .AddRange(new System .Windows .Forms .ToolStripItem[] {
            this.tsbtnSave,
            this.tsbtnAdd,
            this.tsbtnDelete,
            this.tsbtnAudit,
            this.tsbtnUnAudit,
            this.tsbtnPrint,
            this.tsbtnTools,
            this.tsbtnBack,
            this.tsbtnForword,
            this.tsbtnNotify,
            this.tsbtnSign,
            this.tsbtnClose});
              this .tlspBaseSingle .Location = new System .Drawing .Point(0, 0);
              this .tlspBaseSingle .Name = "tlspBaseSingle";
              this .tlspBaseSingle .Size = new System .Drawing .Size(904, 25);
              this .tlspBaseSingle .TabIndex = 0;
              this .tlspBaseSingle .Text = "toolStrip1";
              // 
              // tsbtnSave
              // 
              this .tsbtnSave .Image = global::FZYK .Core .Properties .Resources .saveHS;
              this .tsbtnSave .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnSave .Name = "tsbtnSave";
              this .tsbtnSave .Size = new System .Drawing .Size(67, 22);
              this .tsbtnSave .Text = "保存(&S)";
              this .tsbtnSave .Click += new System .EventHandler(this .tsbtnSave_Click);
              // 
              // tsbtnAdd
              // 
              this .tsbtnAdd .Image = global::FZYK .Core .Properties .Resources .application_form_add;
              this .tsbtnAdd .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnAdd .Name = "tsbtnAdd";
              this .tsbtnAdd .Size = new System .Drawing .Size(70, 22);
              this .tsbtnAdd .Text = "添加(&N)";
              this .tsbtnAdd .Click += new System .EventHandler(this .tsbtnAdd_Click);
              // 
              // tsbtnDelete
              // 
              this .tsbtnDelete .Image = global::FZYK .Core .Properties .Resources .document_delete;
              this .tsbtnDelete .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnDelete .Name = "tsbtnDelete";
              this .tsbtnDelete .Size = new System .Drawing .Size(69, 22);
              this .tsbtnDelete .Text = "删除(&D)";
              this .tsbtnDelete .Click += new System .EventHandler(this .tsbtnDelete_Click);
              // 
              // tsbtnAudit
              // 
              this .tsbtnAudit .Image = global::FZYK .Core .Properties .Resources .audit;
              this .tsbtnAudit .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnAudit .Name = "tsbtnAudit";
              this .tsbtnAudit .Size = new System .Drawing .Size(68, 22);
              this .tsbtnAudit .Text = "锁定(&A)";
              this .tsbtnAudit .Click += new System .EventHandler(this .tsbtnAudit_Click);
              // 
              // tsbtnUnAudit
              // 
              this .tsbtnUnAudit .Image = global::FZYK .Core .Properties .Resources .unaudit;
              this .tsbtnUnAudit .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnUnAudit .Name = "tsbtnUnAudit";
              this .tsbtnUnAudit .Size = new System .Drawing .Size(70, 22);
              this .tsbtnUnAudit .Text = "解锁(&Q)";
              this .tsbtnUnAudit .Click += new System .EventHandler(this .tsbtnUnAudit_Click);
              // 
              // tsbtnPrint
              // 
              this .tsbtnPrint .Image = global::FZYK .Core .Properties .Resources .printer;
              this .tsbtnPrint .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnPrint .Name = "tsbtnPrint";
              this .tsbtnPrint .Size = new System .Drawing .Size(67, 22);
              this .tsbtnPrint .Text = "打印(&P)";
              this .tsbtnPrint .Click += new System .EventHandler(this .tsbtnPrint_Click);
              // 
              // tsbtnTools
              // 
              this .tsbtnTools .DropDownItems .AddRange(new System .Windows .Forms .ToolStripItem[] {
            this.tsbtnImport});
              this .tsbtnTools .Image = global::FZYK .Core .Properties .Resources .plugin;
              this .tsbtnTools .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnTools .Name = "tsbtnTools";
              this .tsbtnTools .Size = new System .Drawing .Size(100, 22);
              this .tsbtnTools .Text = "扩展功能(&T)";
              // 
              // tsbtnImport
              // 
              this .tsbtnImport .Image = global::FZYK .Core .Properties .Resources .excel_go;
              this .tsbtnImport .Name = "tsbtnImport";
              this .tsbtnImport .Size = new System .Drawing .Size(100, 22);
              this .tsbtnImport .Text = "导出";
              this .tsbtnImport .Click += new System .EventHandler(this .tsbtnImport_Click);
              // 
              // tsbtnBack
              // 
              this .tsbtnBack .Image = global::FZYK .Core .Properties .Resources .arrow_left;
              this .tsbtnBack .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnBack .Name = "tsbtnBack";
              this .tsbtnBack .Size = new System .Drawing .Size(80, 22);
              this .tsbtnBack .Text = "上一条(&B)";
              this .tsbtnBack .Visible = false;
              this .tsbtnBack .Click += new System .EventHandler(this .tsbtnBack_Click);
              // 
              // tsbtnForword
              // 
              this .tsbtnForword .Image = global::FZYK .Core .Properties .Resources .arrow_right;
              this .tsbtnForword .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnForword .Name = "tsbtnForword";
              this .tsbtnForword .Size = new System .Drawing .Size(78, 22);
              this .tsbtnForword .Text = "下一条(&F)";
              this .tsbtnForword .Visible = false;
              this .tsbtnForword .Click += new System .EventHandler(this .tsbtnForword_Click);
              // 
              // tsbtnNotify
              // 
              this .tsbtnNotify .Image = global::FZYK .Core .Properties .Resources .email_go;
              this .tsbtnNotify .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnNotify .Name = "tsbtnNotify";
              this .tsbtnNotify .Size = new System .Drawing .Size(69, 22);
              this .tsbtnNotify .Text = "传递(&G)";
              this .tsbtnNotify .Visible = false;
              this .tsbtnNotify .Click += new System .EventHandler(this .tsbtnNotify_Click);
              // 
              // tsbtnSign
              // 
              this .tsbtnSign .Image = global::FZYK .Core .Properties .Resources .email_edit;
              this .tsbtnSign .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnSign .Name = "tsbtnSign";
              this .tsbtnSign .Size = new System .Drawing .Size(65, 22);
              this .tsbtnSign .Text = "接收(&J)";
              this .tsbtnSign .Visible = false;
              this .tsbtnSign .Click += new System .EventHandler(this .tsbtnSign_Click);
              // 
              // tsbtnClose
              // 
              this .tsbtnClose .Image = global::FZYK .Core .Properties .Resources .door_in;
              this .tsbtnClose .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnClose .Name = "tsbtnClose";
              this .tsbtnClose .Size = new System .Drawing .Size(68, 22);
              this .tsbtnClose .Text = "关闭(&C)";
              this .tsbtnClose .Click += new System .EventHandler(this .tsbtnClose_Click);
              // 
              // BaseSingle
              // 
              this .AutoScaleDimensions = new System .Drawing .SizeF(6F, 12F);
              this .AutoScaleMode = System .Windows .Forms .AutoScaleMode .Font;
              this .BackColor = System .Drawing .Color .White;
              this .ClientSize = new System .Drawing .Size(904, 457);
              this .Controls .Add(this .tlspBaseSingle);
              this .Font = new System .Drawing .Font("宋体", 9F, System .Drawing .FontStyle .Regular, System .Drawing .GraphicsUnit .Point, ((byte)(134)));
              this .Name = "BaseSingle";
              this .StartPosition = System .Windows .Forms .FormStartPosition .CenterScreen;
              this .tlspBaseSingle .ResumeLayout(false);
              this .tlspBaseSingle .PerformLayout();
              this .ResumeLayout(false);
              this .PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tlspBaseSingle;
        private System.Windows.Forms.ToolStripMenuItem tsbtnImport;
        public System.Windows.Forms.ToolStripButton tsbtnSave;
        public System.Windows.Forms.ToolStripButton tsbtnAdd;
        public System.Windows.Forms.ToolStripButton tsbtnDelete;
        public System.Windows.Forms.ToolStripButton tsbtnAudit;
        public System.Windows.Forms.ToolStripButton tsbtnUnAudit;
        public System.Windows.Forms.ToolStripButton tsbtnPrint;
        public System.Windows.Forms.ToolStripDropDownButton tsbtnTools;
        public System.Windows.Forms.ToolStripButton tsbtnBack;
        public System.Windows.Forms.ToolStripButton tsbtnForword;
        public System.Windows.Forms.ToolStripButton tsbtnNotify;
        public System.Windows.Forms.ToolStripButton tsbtnSign;
        public System .Windows .Forms .ToolStripButton tsbtnClose;
    }
}