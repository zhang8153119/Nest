namespace FZYK.Core
{
    partial class BaseSearch
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
              this .tlspBaseSearch = new System .Windows .Forms .ToolStrip();
              this .tsbtnAdd = new System .Windows .Forms .ToolStripButton();
              this .tsbtnSave = new System .Windows .Forms .ToolStripButton();
              this .tsbtnModify = new System .Windows .Forms .ToolStripButton();
              this .tsbtnDelete = new System .Windows .Forms .ToolStripButton();
              this .tsbtnShowTop = new System .Windows .Forms .ToolStripSplitButton();
              this .tstxtTopN = new System .Windows .Forms .ToolStripTextBox();
              this .输入行数ToolStripMenuItem = new System .Windows .Forms .ToolStripMenuItem();
              this .tsbtnSearch = new System .Windows .Forms .ToolStripButton();
              this .tsbtnTools = new System .Windows .Forms .ToolStripDropDownButton();
              this .tsbtnColSetting = new System .Windows .Forms .ToolStripMenuItem();
              this .tsbtnPrint = new System .Windows .Forms .ToolStripMenuItem();
              this .tsbtnImport = new System .Windows .Forms .ToolStripButton();
              this .tsbtnClose = new System .Windows .Forms .ToolStripButton();
              this .tsbtnShowToday = new System .Windows .Forms .ToolStripButton();
              this .tlspBaseSearch .SuspendLayout();
              this .SuspendLayout();
              // 
              // tlspBaseSearch
              // 
              this .tlspBaseSearch .Items .AddRange(new System .Windows .Forms .ToolStripItem[] {
            this.tsbtnAdd,
            this.tsbtnSave,
            this.tsbtnModify,
            this.tsbtnDelete,
            this.tsbtnShowTop,
            this.tsbtnSearch,
            this.tsbtnTools,
            this.tsbtnImport,
            this.tsbtnClose,
            this.tsbtnShowToday});
              this .tlspBaseSearch .LayoutStyle = System .Windows .Forms .ToolStripLayoutStyle .HorizontalStackWithOverflow;
              this .tlspBaseSearch .Location = new System .Drawing .Point(0, 0);
              this .tlspBaseSearch .Name = "tlspBaseSearch";
              this .tlspBaseSearch .Size = new System .Drawing .Size(849, 25);
              this .tlspBaseSearch .TabIndex = 0;
              this .tlspBaseSearch .Text = "ykToolStrip1";
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
              // tsbtnSave
              // 
              this .tsbtnSave .Image = global::FZYK .Core .Properties .Resources .saveHS;
              this .tsbtnSave .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnSave .Name = "tsbtnSave";
              this .tsbtnSave .Size = new System .Drawing .Size(67, 22);
              this .tsbtnSave .Text = "保存(&S)";
              this .tsbtnSave .Visible = false;
              this .tsbtnSave .Click += new System .EventHandler(this .tsbtnSave_Click);
              // 
              // tsbtnModify
              // 
              this .tsbtnModify .Image = global::FZYK .Core .Properties .Resources .application_form_edit;
              this .tsbtnModify .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnModify .Name = "tsbtnModify";
              this .tsbtnModify .Size = new System .Drawing .Size(67, 22);
              this .tsbtnModify .Text = "查看(&E)";
              this .tsbtnModify .Click += new System .EventHandler(this .tsbtnModify_Click);
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
              // tsbtnShowTop
              // 
              this .tsbtnShowTop .DropDownItems .AddRange(new System .Windows .Forms .ToolStripItem[] {
            this.tstxtTopN,
            this.输入行数ToolStripMenuItem});
              this .tsbtnShowTop .Image = global::FZYK .Core .Properties .Resources .table_multiple;
              this .tsbtnShowTop .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnShowTop .Name = "tsbtnShowTop";
              this .tsbtnShowTop .Size = new System .Drawing .Size(117, 22);
              this .tsbtnShowTop .Text = "显示前30行(&T)";
              this .tsbtnShowTop .ButtonClick += new System .EventHandler(this .tsbtnShowTop_Click);
              // 
              // tstxtTopN
              // 
              this .tstxtTopN .BorderStyle = System .Windows .Forms .BorderStyle .FixedSingle;
              this .tstxtTopN .Name = "tstxtTopN";
              this .tstxtTopN .Size = new System .Drawing .Size(100, 23);
              this .tstxtTopN .KeyPress += new System .Windows .Forms .KeyPressEventHandler(this .tstxtTopN_KeyPress);
              // 
              // 输入行数ToolStripMenuItem
              // 
              this .输入行数ToolStripMenuItem .Name = "输入行数ToolStripMenuItem";
              this .输入行数ToolStripMenuItem .Size = new System .Drawing .Size(190, 22);
              this .输入行数ToolStripMenuItem .Text = "<输入行数后按回车>";
              // 
              // tsbtnSearch
              // 
              this .tsbtnSearch .Image = global::FZYK .Core .Properties .Resources .find;
              this .tsbtnSearch .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnSearch .Name = "tsbtnSearch";
              this .tsbtnSearch .Size = new System .Drawing .Size(66, 22);
              this .tsbtnSearch .Text = "查找(&F)";
              this .tsbtnSearch .Click += new System .EventHandler(this .tsbtnSearch_Click);
              // 
              // tsbtnTools
              // 
              this .tsbtnTools .DropDownItems .AddRange(new System .Windows .Forms .ToolStripItem[] {
            this.tsbtnColSetting,
            this.tsbtnPrint});
              this .tsbtnTools .Image = global::FZYK .Core .Properties .Resources .plugin;
              this .tsbtnTools .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnTools .Name = "tsbtnTools";
              this .tsbtnTools .Size = new System .Drawing .Size(85, 22);
              this .tsbtnTools .Text = "扩展功能";
              // 
              // tsbtnColSetting
              // 
              this .tsbtnColSetting .Image = global::FZYK .Core .Properties .Resources .table_edit;
              this .tsbtnColSetting .Name = "tsbtnColSetting";
              this .tsbtnColSetting .Size = new System .Drawing .Size(148, 22);
              this .tsbtnColSetting .Text = "列头显示设置";
              this .tsbtnColSetting .Click += new System .EventHandler(this .tsbtnColSetting_Click);
              // 
              // tsbtnPrint
              // 
              this .tsbtnPrint .Image = global::FZYK .Core .Properties .Resources .printer;
              this .tsbtnPrint .Name = "tsbtnPrint";
              this .tsbtnPrint .Size = new System .Drawing .Size(148, 22);
              this .tsbtnPrint .Text = "打印";
              this .tsbtnPrint .Click += new System .EventHandler(this .tsbtnPrint_Click);
              // 
              // tsbtnImport
              // 
              this .tsbtnImport .Image = global::FZYK .Core .Properties .Resources .excel_go;
              this .tsbtnImport .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnImport .Name = "tsbtnImport";
              this .tsbtnImport .Size = new System .Drawing .Size(70, 22);
              this .tsbtnImport .Text = "导出(&O)";
              this .tsbtnImport .Click += new System .EventHandler(this .tsbtnImport_Click);
              // 
              // tsbtnClose
              // 
              this .tsbtnClose .Image = global::FZYK .Core .Properties .Resources .door_in;
              this .tsbtnClose .Name = "tsbtnClose";
              this .tsbtnClose .Size = new System .Drawing .Size(68, 22);
              this .tsbtnClose .Text = "关闭(&C)";
              this .tsbtnClose .Click += new System .EventHandler(this .tsbtnClose_Click);
              // 
              // tsbtnShowToday
              // 
              this .tsbtnShowToday .Image = global::FZYK .Core .Properties .Resources .table_multiple;
              this .tsbtnShowToday .ImageTransparentColor = System .Drawing .Color .Magenta;
              this .tsbtnShowToday .Name = "tsbtnShowToday";
              this .tsbtnShowToday .Size = new System .Drawing .Size(100, 22);
              this .tsbtnShowToday .Text = "查看今日数据";
              this .tsbtnShowToday .Visible = false;
              this .tsbtnShowToday .Click += new System .EventHandler(this .tsbtnShowToday_Click);
              // 
              // BaseSearch
              // 
              this .AutoScaleDimensions = new System .Drawing .SizeF(6F, 12F);
              this .AutoScaleMode = System .Windows .Forms .AutoScaleMode .Font;
              this .BackColor = System .Drawing .Color .White;
              this .ClientSize = new System .Drawing .Size(849, 435);
              this .Controls .Add(this .tlspBaseSearch);
              this .Font = new System .Drawing .Font("宋体", 9F, System .Drawing .FontStyle .Regular, System .Drawing .GraphicsUnit .Point, ((byte)(134)));
              this .Name = "BaseSearch";
              this .tlspBaseSearch .ResumeLayout(false);
              this .tlspBaseSearch .PerformLayout();
              this .ResumeLayout(false);
              this .PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tlspBaseSearch;
        private System.Windows.Forms.ToolStripMenuItem tsbtnColSetting;
        private System.Windows.Forms.ToolStripMenuItem tsbtnPrint;
        public System.Windows.Forms.ToolStripButton tsbtnSave;
        public System.Windows.Forms.ToolStripButton tsbtnAdd;
        public System.Windows.Forms.ToolStripButton tsbtnModify;
        public System.Windows.Forms.ToolStripButton tsbtnDelete;
        public System.Windows.Forms.ToolStripButton tsbtnSearch;
        public System.Windows.Forms.ToolStripDropDownButton tsbtnTools;
        public System.Windows.Forms.ToolStripButton tsbtnImport;
        public System.Windows.Forms.ToolStripButton tsbtnClose;
        public System.Windows.Forms.ToolStripSplitButton tsbtnShowTop;
        private System.Windows.Forms.ToolStripTextBox tstxtTopN;
        private System.Windows.Forms.ToolStripMenuItem 输入行数ToolStripMenuItem;
        public System .Windows .Forms .ToolStripButton tsbtnShowToday;
    }
}

