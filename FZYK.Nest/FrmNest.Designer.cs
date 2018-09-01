namespace FZYK .Nest
{
      partial class FrmNest
      {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System .ComponentModel .IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                  if (disposing && (components != null))
                  {
                        components .Dispose();
                  }
                  base .Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                  this.components = new System.ComponentModel.Container();
                  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNest));
                  System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
                  System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
                  System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
                  System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
                  System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
                  System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
                  this.tabMain = new FZYK.WinCtrl.YKTabControl(this.components);
                  this.pageMain = new FZYK.WinCtrl.YKTabPage();
                  this.tsMain = new FZYK.WinCtrl.YKToolStrip(this.components);
                  this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripDropDownButton();
                  this.tsmiReadDxf = new System.Windows.Forms.ToolStripMenuItem();
                  this.tsbtnSet = new System.Windows.Forms.ToolStripButton();
                  this.pageSet = new FZYK.WinCtrl.YKTabPage();
                  this.scMain = new FZYK.WinCtrl.YKSplitContainer(this.components);
                  this.dgvPart = new FZYK.WinCtrl.YKDataGridView(this.components);
                  this.scMain2 = new FZYK.WinCtrl.YKSplitContainer(this.components);
                  this.pnlright = new FZYK.WinCtrl.YKPanel(this.components);
                  this.scleft = new FZYK.WinCtrl.YKSplitContainer(this.components);
                  this.dgvStock = new FZYK.WinCtrl.YKDataGridView(this.components);
                  this.pName = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.rCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.pLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.pWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.pCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.lName = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.mfSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.sLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.sWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.sCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.lName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.mfspec2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                  this.cadInterfaceMain = new myCad.CADInterfaceCtrl.CADInterface();
                  this.tabMain.SuspendLayout();
                  this.pageMain.SuspendLayout();
                  this.tsMain.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
                  this.scMain.Panel1.SuspendLayout();
                  this.scMain.Panel2.SuspendLayout();
                  this.scMain.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.dgvPart)).BeginInit();
                  ((System.ComponentModel.ISupportInitialize)(this.scMain2)).BeginInit();
                  this.scMain2.Panel1.SuspendLayout();
                  this.scMain2.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.scleft)).BeginInit();
                  this.scleft.Panel1.SuspendLayout();
                  this.scleft.Panel2.SuspendLayout();
                  this.scleft.SuspendLayout();
                  ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
                  this.SuspendLayout();
                  // 
                  // tabMain
                  // 
                  this.tabMain.Controls.Add(this.pageMain);
                  this.tabMain.Controls.Add(this.pageSet);
                  this.tabMain.Dock = System.Windows.Forms.DockStyle.Top;
                  this.tabMain.Location = new System.Drawing.Point(0, 25);
                  this.tabMain.myBackColor = System.Drawing.Color.White;
                  this.tabMain.Name = "tabMain";
                  this.tabMain.SelectedIndex = 0;
                  this.tabMain.Size = new System.Drawing.Size(1039, 116);
                  this.tabMain.TabIndex = 2;
                  // 
                  // pageMain
                  // 
                  this.pageMain.BackColor = System.Drawing.Color.White;
                  this.pageMain.Controls.Add(this.tsMain);
                  this.pageMain.Location = new System.Drawing.Point(4, 26);
                  this.pageMain.Margin = new System.Windows.Forms.Padding(1);
                  this.pageMain.Name = "pageMain";
                  this.pageMain.Padding = new System.Windows.Forms.Padding(1);
                  this.pageMain.Size = new System.Drawing.Size(1031, 86);
                  this.pageMain.TabIndex = 0;
                  this.pageMain.Text = "板材套料";
                  // 
                  // tsMain
                  // 
                  this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.tsbtnSet});
                  this.tsMain.Location = new System.Drawing.Point(1, 1);
                  this.tsMain.Name = "tsMain";
                  this.tsMain.Size = new System.Drawing.Size(1029, 25);
                  this.tsMain.TabIndex = 0;
                  this.tsMain.Text = "ykToolStrip1";
                  // 
                  // toolStripSplitButton1
                  // 
                  this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiReadDxf});
                  this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
                  this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
                  this.toolStripSplitButton1.Name = "toolStripSplitButton1";
                  this.toolStripSplitButton1.Size = new System.Drawing.Size(85, 22);
                  this.toolStripSplitButton1.Text = "读取零件";
                  // 
                  // tsmiReadDxf
                  // 
                  this.tsmiReadDxf.Name = "tsmiReadDxf";
                  this.tsmiReadDxf.Size = new System.Drawing.Size(147, 22);
                  this.tsmiReadDxf.Text = "读取DXF文件";
                  this.tsmiReadDxf.Click += new System.EventHandler(this.tsmiReadDxf_Click);
                  // 
                  // tsbtnSet
                  // 
                  this.tsbtnSet.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSet.Image")));
                  this.tsbtnSet.ImageTransparentColor = System.Drawing.Color.Magenta;
                  this.tsbtnSet.Name = "tsbtnSet";
                  this.tsbtnSet.Size = new System.Drawing.Size(52, 22);
                  this.tsbtnSet.Text = "设置";
                  this.tsbtnSet.Click += new System.EventHandler(this.tsbtnSet_Click);
                  // 
                  // pageSet
                  // 
                  this.pageSet.BackColor = System.Drawing.Color.White;
                  this.pageSet.Location = new System.Drawing.Point(4, 26);
                  this.pageSet.Margin = new System.Windows.Forms.Padding(1);
                  this.pageSet.Name = "pageSet";
                  this.pageSet.Padding = new System.Windows.Forms.Padding(1);
                  this.pageSet.Size = new System.Drawing.Size(1031, 86);
                  this.pageSet.TabIndex = 1;
                  this.pageSet.Text = "设置";
                  // 
                  // scMain
                  // 
                  this.scMain.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                  this.scMain.CollpasePanel = FZYK.WinCtrl.YKSplitContainer.SplitterPanelEnum.Panel1;
                  this.scMain.Cursor = System.Windows.Forms.Cursors.Default;
                  this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.scMain.Location = new System.Drawing.Point(0, 141);
                  this.scMain.Name = "scMain";
                  // 
                  // scMain.Panel1
                  // 
                  this.scMain.Panel1.BackColor = System.Drawing.Color.White;
                  this.scMain.Panel1.Controls.Add(this.scleft);
                  this.scMain.Panel1.Padding = new System.Windows.Forms.Padding(1);
                  this.scMain.Panel1MinSize = 0;
                  // 
                  // scMain.Panel2
                  // 
                  this.scMain.Panel2.BackColor = System.Drawing.Color.White;
                  this.scMain.Panel2.Controls.Add(this.scMain2);
                  this.scMain.Panel2.Padding = new System.Windows.Forms.Padding(1);
                  this.scMain.Panel2MinSize = 0;
                  this.scMain.Size = new System.Drawing.Size(1039, 428);
                  this.scMain.SplitterDistance = 200;
                  this.scMain.SplitterWidth = 9;
                  this.scMain.TabIndex = 3;
                  // 
                  // dgvPart
                  // 
                  this.dgvPart.A_IsAppendPaste = false;
                  this.dgvPart.A_IsRemoveBlank = false;
                  this.dgvPart.A_IsSetColumnIndex = true;
                  this.dgvPart.A_myName = "";
                  this.dgvPart.A_NoPasteColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvPart.A_NoPasteColumns")));
                  this.dgvPart.AllowUserToAddRows = false;
                  this.dgvPart.AllowUserToDeleteRows = false;
                  this.dgvPart.AllowUserToOrderColumns = true;
                  this.dgvPart.BackgroundColor = System.Drawing.Color.White;
                  dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                  dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
                  dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
                  dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                  dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                  dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                  this.dgvPart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
                  this.dgvPart.ColumnHeadersHeight = 30;
                  this.dgvPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                  this.dgvPart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pName,
            this.rCount,
            this.pLength,
            this.pWidth,
            this.pCount,
            this.id,
            this.lName,
            this.mfSpec});
                  dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                  dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
                  dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
                  dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Wheat;
                  dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.DarkSlateBlue;
                  dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
                  this.dgvPart.DefaultCellStyle = dataGridViewCellStyle8;
                  this.dgvPart.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.dgvPart.EnableHeadersVisualStyles = false;
                  this.dgvPart.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
                  this.dgvPart.IfMulitSortEnable = false;
                  this.dgvPart.IfSetAlternatingRowStyle = false;
                  this.dgvPart.Location = new System.Drawing.Point(1, 1);
                  this.dgvPart.Name = "dgvPart";
                  this.dgvPart.ReadOnly = true;
                  this.dgvPart.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
                  dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                  dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
                  dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
                  dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                  dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                  dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                  this.dgvPart.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
                  this.dgvPart.RowHeadersWidth = 20;
                  this.dgvPart.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                  this.dgvPart.RowTemplate.Height = 23;
                  this.dgvPart.Size = new System.Drawing.Size(196, 282);
                  this.dgvPart.TabIndex = 0;
                  // 
                  // scMain2
                  // 
                  this.scMain2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                  this.scMain2.Cursor = System.Windows.Forms.Cursors.Default;
                  this.scMain2.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.scMain2.Location = new System.Drawing.Point(1, 1);
                  this.scMain2.Name = "scMain2";
                  this.scMain2.Orientation = System.Windows.Forms.Orientation.Horizontal;
                  // 
                  // scMain2.Panel1
                  // 
                  this.scMain2.Panel1.Controls.Add(this.cadInterfaceMain);
                  this.scMain2.Panel1.Controls.Add(this.pnlright);
                  this.scMain2.Panel1.Padding = new System.Windows.Forms.Padding(1);
                  this.scMain2.Panel1MinSize = 0;
                  // 
                  // scMain2.Panel2
                  // 
                  this.scMain2.Panel2.BackColor = System.Drawing.Color.White;
                  this.scMain2.Panel2.Padding = new System.Windows.Forms.Padding(1);
                  this.scMain2.Panel2MinSize = 0;
                  this.scMain2.Size = new System.Drawing.Size(828, 426);
                  this.scMain2.SplitterDistance = 285;
                  this.scMain2.SplitterWidth = 9;
                  this.scMain2.TabIndex = 2;
                  // 
                  // pnlright
                  // 
                  this.pnlright.BackColor = System.Drawing.Color.White;
                  this.pnlright.Dock = System.Windows.Forms.DockStyle.Right;
                  this.pnlright.Location = new System.Drawing.Point(796, 1);
                  this.pnlright.Name = "pnlright";
                  this.pnlright.Size = new System.Drawing.Size(31, 283);
                  this.pnlright.TabIndex = 2;
                  // 
                  // scleft
                  // 
                  this.scleft.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                  this.scleft.Cursor = System.Windows.Forms.Cursors.Default;
                  this.scleft.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.scleft.Location = new System.Drawing.Point(1, 1);
                  this.scleft.Name = "scleft";
                  this.scleft.Orientation = System.Windows.Forms.Orientation.Horizontal;
                  // 
                  // scleft.Panel1
                  // 
                  this.scleft.Panel1.Controls.Add(this.dgvPart);
                  this.scleft.Panel1.Padding = new System.Windows.Forms.Padding(1);
                  this.scleft.Panel1MinSize = 0;
                  // 
                  // scleft.Panel2
                  // 
                  this.scleft.Panel2.Controls.Add(this.dgvStock);
                  this.scleft.Panel2.Padding = new System.Windows.Forms.Padding(1);
                  this.scleft.Panel2MinSize = 0;
                  this.scleft.Size = new System.Drawing.Size(198, 426);
                  this.scleft.SplitterDistance = 284;
                  this.scleft.SplitterWidth = 9;
                  this.scleft.TabIndex = 1;
                  // 
                  // dgvStock
                  // 
                  this.dgvStock.A_IsAppendPaste = false;
                  this.dgvStock.A_IsRemoveBlank = false;
                  this.dgvStock.A_IsSetColumnIndex = true;
                  this.dgvStock.A_myName = "";
                  this.dgvStock.A_NoPasteColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvStock.A_NoPasteColumns")));
                  this.dgvStock.AllowUserToAddRows = false;
                  this.dgvStock.AllowUserToDeleteRows = false;
                  this.dgvStock.BackgroundColor = System.Drawing.Color.White;
                  dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                  dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
                  dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
                  dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                  dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                  dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                  this.dgvStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
                  this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                  this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sLength,
            this.sWidth,
            this.sCount,
            this.lName2,
            this.mfspec2});
                  dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                  dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
                  dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
                  dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.Wheat;
                  dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.DarkSlateBlue;
                  dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
                  this.dgvStock.DefaultCellStyle = dataGridViewCellStyle11;
                  this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.dgvStock.EnableHeadersVisualStyles = false;
                  this.dgvStock.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
                  this.dgvStock.IfMulitSortEnable = false;
                  this.dgvStock.IfSetAlternatingRowStyle = false;
                  this.dgvStock.Location = new System.Drawing.Point(1, 1);
                  this.dgvStock.Name = "dgvStock";
                  this.dgvStock.ReadOnly = true;
                  this.dgvStock.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
                  dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                  dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
                  dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
                  dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                  dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                  dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                  this.dgvStock.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
                  this.dgvStock.RowHeadersWidth = 20;
                  this.dgvStock.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                  this.dgvStock.RowTemplate.Height = 23;
                  this.dgvStock.Size = new System.Drawing.Size(196, 131);
                  this.dgvStock.TabIndex = 0;
                  // 
                  // pName
                  // 
                  this.pName.DataPropertyName = "pName";
                  this.pName.HeaderText = "件号";
                  this.pName.Name = "pName";
                  this.pName.ReadOnly = true;
                  this.pName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.pName.Width = 50;
                  // 
                  // rCount
                  // 
                  this.rCount.DataPropertyName = "rCount";
                  this.rCount.HeaderText = "剩余数";
                  this.rCount.Name = "rCount";
                  this.rCount.ReadOnly = true;
                  this.rCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.rCount.Width = 50;
                  // 
                  // pLength
                  // 
                  this.pLength.DataPropertyName = "pLength";
                  this.pLength.HeaderText = "长度";
                  this.pLength.Name = "pLength";
                  this.pLength.ReadOnly = true;
                  this.pLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.pLength.Width = 50;
                  // 
                  // pWidth
                  // 
                  this.pWidth.DataPropertyName = "pWidth";
                  this.pWidth.HeaderText = "宽度";
                  this.pWidth.Name = "pWidth";
                  this.pWidth.ReadOnly = true;
                  this.pWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.pWidth.Width = 50;
                  // 
                  // pCount
                  // 
                  this.pCount.DataPropertyName = "pCount";
                  this.pCount.HeaderText = "总数";
                  this.pCount.Name = "pCount";
                  this.pCount.ReadOnly = true;
                  this.pCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.pCount.Width = 50;
                  // 
                  // id
                  // 
                  this.id.DataPropertyName = "id";
                  this.id.HeaderText = "id";
                  this.id.Name = "id";
                  this.id.ReadOnly = true;
                  this.id.Visible = false;
                  // 
                  // lName
                  // 
                  this.lName.DataPropertyName = "lName";
                  this.lName.HeaderText = "材质";
                  this.lName.Name = "lName";
                  this.lName.ReadOnly = true;
                  this.lName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.lName.Width = 50;
                  // 
                  // mfSpec
                  // 
                  this.mfSpec.DataPropertyName = "mfSpec";
                  this.mfSpec.HeaderText = "规格";
                  this.mfSpec.Name = "mfSpec";
                  this.mfSpec.ReadOnly = true;
                  this.mfSpec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.mfSpec.Width = 50;
                  // 
                  // sLength
                  // 
                  this.sLength.DataPropertyName = "sLength";
                  this.sLength.HeaderText = "长度";
                  this.sLength.Name = "sLength";
                  this.sLength.ReadOnly = true;
                  this.sLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.sLength.Width = 50;
                  // 
                  // sWidth
                  // 
                  this.sWidth.DataPropertyName = "sWidth";
                  this.sWidth.HeaderText = "宽度";
                  this.sWidth.Name = "sWidth";
                  this.sWidth.ReadOnly = true;
                  this.sWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.sWidth.Width = 50;
                  // 
                  // sCount
                  // 
                  this.sCount.DataPropertyName = "sCount";
                  this.sCount.HeaderText = "数量";
                  this.sCount.Name = "sCount";
                  this.sCount.ReadOnly = true;
                  this.sCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.sCount.Width = 45;
                  // 
                  // lName2
                  // 
                  this.lName2.DataPropertyName = "lName";
                  this.lName2.HeaderText = "材质";
                  this.lName2.Name = "lName2";
                  this.lName2.ReadOnly = true;
                  this.lName2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.lName2.Width = 50;
                  // 
                  // mfspec2
                  // 
                  this.mfspec2.DataPropertyName = "mfspec";
                  this.mfspec2.HeaderText = "规格";
                  this.mfspec2.Name = "mfspec2";
                  this.mfspec2.ReadOnly = true;
                  this.mfspec2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                  this.mfspec2.Width = 50;
                  // 
                  // cadInterfaceMain
                  // 
                  this.cadInterfaceMain.BackColor = System.Drawing.Color.Black;
                  this.cadInterfaceMain.currentPlates = null;
                  this.cadInterfaceMain.currentShapes = null;
                  this.cadInterfaceMain.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.cadInterfaceMain.drawRegionRect = ((System.Drawing.RectangleF)(resources.GetObject("cadInterfaceMain.drawRegionRect")));
                  this.cadInterfaceMain.globalID = 0;
                  this.cadInterfaceMain.globalModelID = 0;
                  this.cadInterfaceMain.globalZoomNum = 1F;
                  this.cadInterfaceMain.Location = new System.Drawing.Point(1, 1);
                  this.cadInterfaceMain.Margin = new System.Windows.Forms.Padding(2);
                  this.cadInterfaceMain.Name = "cadInterfaceMain";
                  this.cadInterfaceMain.nowStock = null;
                  this.cadInterfaceMain.scaleNum = 1F;
                  this.cadInterfaceMain.selectRect = null;
                  this.cadInterfaceMain.Size = new System.Drawing.Size(795, 283);
                  this.cadInterfaceMain.TabIndex = 3;
                  // 
                  // FrmNest
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.ClientSize = new System.Drawing.Size(1039, 569);
                  this.Controls.Add(this.scMain);
                  this.Controls.Add(this.tabMain);
                  this.Name = "FrmNest";
                  this.Text = "板材套料";
                  this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                  this.Shown += new System.EventHandler(this.FrmNest_Shown);
                  this.Controls.SetChildIndex(this.tabMain, 0);
                  this.Controls.SetChildIndex(this.scMain, 0);
                  this.tabMain.ResumeLayout(false);
                  this.pageMain.ResumeLayout(false);
                  this.pageMain.PerformLayout();
                  this.tsMain.ResumeLayout(false);
                  this.tsMain.PerformLayout();
                  this.scMain.Panel1.ResumeLayout(false);
                  this.scMain.Panel2.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
                  this.scMain.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.dgvPart)).EndInit();
                  this.scMain2.Panel1.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.scMain2)).EndInit();
                  this.scMain2.ResumeLayout(false);
                  this.scleft.Panel1.ResumeLayout(false);
                  this.scleft.Panel2.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.scleft)).EndInit();
                  this.scleft.ResumeLayout(false);
                  ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
                  this.ResumeLayout(false);
                  this.PerformLayout();

            }

            #endregion
            
            private WinCtrl .YKTabControl tabMain;
            private WinCtrl .YKTabPage pageMain;
            private WinCtrl .YKSplitContainer scMain;
            private WinCtrl .YKSplitContainer scMain2;
            private WinCtrl .YKPanel pnlright;
            private WinCtrl .YKTabPage pageSet;
            private WinCtrl .YKDataGridView dgvPart;
            private WinCtrl .YKToolStrip tsMain;
            private System .Windows .Forms .ToolStripDropDownButton toolStripSplitButton1;
            private System .Windows .Forms .ToolStripMenuItem tsmiReadDxf;
            private System .Windows .Forms .ToolStripButton tsbtnSet;
            private myCad .CADInterfaceCtrl .CADInterface cadInterfaceMain;
            private WinCtrl .YKSplitContainer scleft;
            private WinCtrl .YKDataGridView dgvStock;
            private System .Windows .Forms .DataGridViewTextBoxColumn pName;
            private System .Windows .Forms .DataGridViewTextBoxColumn rCount;
            private System .Windows .Forms .DataGridViewTextBoxColumn pLength;
            private System .Windows .Forms .DataGridViewTextBoxColumn pWidth;
            private System .Windows .Forms .DataGridViewTextBoxColumn pCount;
            private System .Windows .Forms .DataGridViewTextBoxColumn id;
            private System .Windows .Forms .DataGridViewTextBoxColumn lName;
            private System .Windows .Forms .DataGridViewTextBoxColumn mfSpec;
            private System .Windows .Forms .DataGridViewTextBoxColumn sLength;
            private System .Windows .Forms .DataGridViewTextBoxColumn sWidth;
            private System .Windows .Forms .DataGridViewTextBoxColumn sCount;
            private System .Windows .Forms .DataGridViewTextBoxColumn lName2;
            private System .Windows .Forms .DataGridViewTextBoxColumn mfspec2;
      }
}