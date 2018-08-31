namespace FZYK.Core
{
    partial class FrmSetColumnVisible
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetColumnVisible));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ykPanel1 = new FZYK.WinCtrl.YKPanel(this.components);
            this.btnCancel = new FZYK.WinCtrl.YKButton(this.components);
            this.btnSure = new FZYK.WinCtrl.YKButton(this.components);
            this.cmbdgvList = new FZYK.WinCtrl.YKComboBox();
            this.ykLabel1 = new FZYK.WinCtrl.YKLabel(this.components);
            this.dgvColumns = new FZYK.WinCtrl.YKDataGridView(this.components);
            this.cvsColText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cvsVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cvsColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cvsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cvsOldVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ykPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // ykPanel1
            // 
            this.ykPanel1.Controls.Add(this.btnCancel);
            this.ykPanel1.Controls.Add(this.btnSure);
            this.ykPanel1.Controls.Add(this.cmbdgvList);
            this.ykPanel1.Controls.Add(this.ykLabel1);
            this.ykPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ykPanel1.Location = new System.Drawing.Point(0, 0);
            this.ykPanel1.Name = "ykPanel1";
            this.ykPanel1.Size = new System.Drawing.Size(262, 68);
            this.ykPanel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.A_BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCancel.A_GotFocusColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCancel.A_MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btnCancel.A_MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnCancel.DisplayFocusCues = false;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(129, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "关闭";
            this.btnCancel.TextType = FZYK.WinCtrl.YKButton.YKBtnTextType.Normal;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSure
            // 
            this.btnSure.A_BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnSure.A_GotFocusColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSure.A_MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btnSure.A_MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnSure.DisplayFocusCues = false;
            this.btnSure.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
            this.btnSure.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btnSure.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnSure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSure.ForeColor = System.Drawing.Color.Black;
            this.btnSure.Location = new System.Drawing.Point(48, 35);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 2;
            this.btnSure.Text = "应用";
            this.btnSure.TextType = FZYK.WinCtrl.YKButton.YKBtnTextType.Normal;
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // cmbdgvList
            // 
            this.cmbdgvList.BackColor = System.Drawing.Color.White;
            this.cmbdgvList.BorderColor = System.Drawing.Color.Black;
            this.cmbdgvList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbdgvList.FormattingEnabled = true;
            this.cmbdgvList.Location = new System.Drawing.Point(48, 9);
            this.cmbdgvList.Name = "cmbdgvList";
            this.cmbdgvList.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbdgvList.PromptForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbdgvList.PromptText = "";
            this.cmbdgvList.Size = new System.Drawing.Size(193, 20);
            this.cmbdgvList.TabIndex = 1;
            this.cmbdgvList.SelectedIndexChanged += new System.EventHandler(this.cmbdgvList_SelectedIndexChanged);
            // 
            // ykLabel1
            // 
            this.ykLabel1.AutoSize = true;
            this.ykLabel1.IsTitle = false;
            this.ykLabel1.Location = new System.Drawing.Point(13, 12);
            this.ykLabel1.Name = "ykLabel1";
            this.ykLabel1.Size = new System.Drawing.Size(29, 12);
            this.ykLabel1.TabIndex = 0;
            this.ykLabel1.Text = "标题";
            // 
            // dgvColumns
            // 
            this.dgvColumns.A_IsAppendPaste = false;
            this.dgvColumns.A_IsRemoveBlank = false;
            this.dgvColumns.A_IsSetColumnIndex = true;
            this.dgvColumns.A_myName = "";
            this.dgvColumns.A_NoPasteColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("dgvColumns.A_NoPasteColumns")));
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.AllowUserToOrderColumns = true;
            this.dgvColumns.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColumns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvColumns.ColumnHeadersHeight = 30;
            this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cvsColText,
            this.cvsVisible,
            this.cvsColName,
            this.cvsID,
            this.cvsOldVisible});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Wheat;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DarkSlateBlue;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvColumns.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumns.EnableHeadersVisualStyles = false;
            this.dgvColumns.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgvColumns.IfMulitSortEnable = false;
            this.dgvColumns.IfSetAlternatingRowStyle = false;
            this.dgvColumns.Location = new System.Drawing.Point(0, 68);
            this.dgvColumns.Name = "dgvColumns";
            this.dgvColumns.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColumns.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvColumns.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvColumns.RowTemplate.Height = 23;
            this.dgvColumns.Size = new System.Drawing.Size(262, 431);
            this.dgvColumns.TabIndex = 1;
            // 
            // cvsColText
            // 
            this.cvsColText.DataPropertyName = "cvsColText";
            this.cvsColText.HeaderText = "列名";
            this.cvsColText.Name = "cvsColText";
            this.cvsColText.Width = 120;
            // 
            // cvsVisible
            // 
            this.cvsVisible.DataPropertyName = "cvsVisible";
            this.cvsVisible.FalseValue = "0";
            this.cvsVisible.HeaderText = "可视性";
            this.cvsVisible.Name = "cvsVisible";
            this.cvsVisible.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cvsVisible.TrueValue = "1";
            this.cvsVisible.Width = 60;
            // 
            // cvsColName
            // 
            this.cvsColName.DataPropertyName = "cvsColName";
            this.cvsColName.HeaderText = "colName";
            this.cvsColName.Name = "cvsColName";
            this.cvsColName.Visible = false;
            // 
            // cvsID
            // 
            this.cvsID.DataPropertyName = "cvsID";
            this.cvsID.HeaderText = "cvsID";
            this.cvsID.Name = "cvsID";
            this.cvsID.Visible = false;
            // 
            // cvsOldVisible
            // 
            this.cvsOldVisible.DataPropertyName = "cvsOldVisible";
            this.cvsOldVisible.HeaderText = "cvsOldVisible";
            this.cvsOldVisible.Name = "cvsOldVisible";
            this.cvsOldVisible.Visible = false;
            // 
            // FrmSetColumnVisible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(262, 499);
            this.Controls.Add(this.dgvColumns);
            this.Controls.Add(this.ykPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmSetColumnVisible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置显示隐藏列";
            this.Load += new System.EventHandler(this.FrmSetColumnVisible_Load);
            this.Shown += new System.EventHandler(this.FrmSetColumnVisible_Shown);
            this.ykPanel1.ResumeLayout(false);
            this.ykPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WinCtrl.YKPanel ykPanel1;
        private WinCtrl.YKButton btnCancel;
        private WinCtrl.YKButton btnSure;
        private WinCtrl.YKComboBox cmbdgvList;
        private WinCtrl.YKLabel ykLabel1;
        private WinCtrl.YKDataGridView dgvColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn cvsColText;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cvsVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn cvsColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cvsID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cvsOldVisible;
    }
}