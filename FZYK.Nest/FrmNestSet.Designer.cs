namespace FZYK .Nest
{
      partial class FrmNestSet
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
                  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNestSet));
                  this.ykLabel1 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.txtProtect = new FZYK.WinCtrl.YKTextBox(this.components);
                  this.ykLabel2 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.ykLabel3 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.cmbType = new FZYK.WinCtrl.YKComboBox();
                  this.ykLabel4 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.txtPop = new FZYK.WinCtrl.YKTextBox(this.components);
                  this.txtCross = new FZYK.WinCtrl.YKTextBox(this.components);
                  this.txtMutation = new FZYK.WinCtrl.YKTextBox(this.components);
                  this.ykLabel5 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.ykLabel6 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.txtT = new FZYK.WinCtrl.YKTextBox(this.components);
                  this.ykLabel7 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.txtSize = new FZYK.WinCtrl.YKTextBox(this.components);
                  this.ykLabel8 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.ykLabel9 = new FZYK.WinCtrl.YKLabel(this.components);
                  this.cmbRotate = new FZYK.WinCtrl.YKComboBox();
                  this.btnSure = new FZYK.WinCtrl.YKButton(this.components);
                  this.btnCancel = new FZYK.WinCtrl.YKButton(this.components);
                  this.SuspendLayout();
                  // 
                  // ykLabel1
                  // 
                  this.ykLabel1.AutoSize = true;
                  this.ykLabel1.IsTitle = false;
                  this.ykLabel1.Location = new System.Drawing.Point(186, 29);
                  this.ykLabel1.Name = "ykLabel1";
                  this.ykLabel1.Size = new System.Drawing.Size(65, 12);
                  this.ykLabel1.TabIndex = 0;
                  this.ykLabel1.Text = "碰撞保护量";
                  // 
                  // txtProtect
                  // 
                  this.txtProtect.BackColor = System.Drawing.Color.White;
                  this.txtProtect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                  this.txtProtect.FocusSelect = true;
                  this.txtProtect.InputDecimalPlace = 2;
                  this.txtProtect.InputFilterType = FZYK.WinCtrl.FilterTypeEnum.Normal;
                  this.txtProtect.Location = new System.Drawing.Point(257, 26);
                  this.txtProtect.Name = "txtProtect";
                  this.txtProtect.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.txtProtect.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.txtProtect.PromptText = "";
                  this.txtProtect.Size = new System.Drawing.Size(82, 21);
                  this.txtProtect.TabIndex = 1;
                  this.txtProtect.UnderLine = true;
                  this.txtProtect.UnderLineColor = System.Drawing.Color.Black;
                  // 
                  // ykLabel2
                  // 
                  this.ykLabel2.AutoSize = true;
                  this.ykLabel2.IsTitle = false;
                  this.ykLabel2.Location = new System.Drawing.Point(344, 30);
                  this.ykLabel2.Name = "ykLabel2";
                  this.ykLabel2.Size = new System.Drawing.Size(17, 12);
                  this.ykLabel2.TabIndex = 0;
                  this.ykLabel2.Text = "mm";
                  // 
                  // ykLabel3
                  // 
                  this.ykLabel3.AutoSize = true;
                  this.ykLabel3.IsTitle = false;
                  this.ykLabel3.Location = new System.Drawing.Point(198, 62);
                  this.ykLabel3.Name = "ykLabel3";
                  this.ykLabel3.Size = new System.Drawing.Size(53, 12);
                  this.ykLabel3.TabIndex = 0;
                  this.ykLabel3.Text = "组合模型";
                  // 
                  // cmbType
                  // 
                  this.cmbType.BackColor = System.Drawing.Color.White;
                  this.cmbType.BorderColor = System.Drawing.Color.Black;
                  this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                  this.cmbType.Enabled = false;
                  this.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                  this.cmbType.FormattingEnabled = true;
                  this.cmbType.Items.AddRange(new object[] {
            "矩形"});
                  this.cmbType.Location = new System.Drawing.Point(257, 60);
                  this.cmbType.Name = "cmbType";
                  this.cmbType.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.cmbType.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.cmbType.PromptText = "";
                  this.cmbType.Size = new System.Drawing.Size(82, 20);
                  this.cmbType.TabIndex = 8;
                  // 
                  // ykLabel4
                  // 
                  this.ykLabel4.AutoSize = true;
                  this.ykLabel4.IsTitle = false;
                  this.ykLabel4.Location = new System.Drawing.Point(198, 154);
                  this.ykLabel4.Name = "ykLabel4";
                  this.ykLabel4.Size = new System.Drawing.Size(53, 12);
                  this.ykLabel4.TabIndex = 0;
                  this.ykLabel4.Text = "内置参数";
                  // 
                  // txtPop
                  // 
                  this.txtPop.BackColor = System.Drawing.Color.White;
                  this.txtPop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                  this.txtPop.FocusSelect = true;
                  this.txtPop.InputDecimalPlace = 2;
                  this.txtPop.InputFilterType = FZYK.WinCtrl.FilterTypeEnum.Normal;
                  this.txtPop.Location = new System.Drawing.Point(257, 149);
                  this.txtPop.Name = "txtPop";
                  this.txtPop.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.txtPop.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.txtPop.PromptText = "";
                  this.txtPop.Size = new System.Drawing.Size(54, 21);
                  this.txtPop.TabIndex = 1;
                  this.txtPop.UnderLine = true;
                  this.txtPop.UnderLineColor = System.Drawing.Color.Black;
                  // 
                  // txtCross
                  // 
                  this.txtCross.BackColor = System.Drawing.Color.White;
                  this.txtCross.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                  this.txtCross.FocusSelect = true;
                  this.txtCross.InputDecimalPlace = 2;
                  this.txtCross.InputFilterType = FZYK.WinCtrl.FilterTypeEnum.Normal;
                  this.txtCross.Location = new System.Drawing.Point(312, 149);
                  this.txtCross.Name = "txtCross";
                  this.txtCross.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.txtCross.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.txtCross.PromptText = "";
                  this.txtCross.Size = new System.Drawing.Size(54, 21);
                  this.txtCross.TabIndex = 1;
                  this.txtCross.UnderLine = true;
                  this.txtCross.UnderLineColor = System.Drawing.Color.Black;
                  // 
                  // txtMutation
                  // 
                  this.txtMutation.BackColor = System.Drawing.Color.White;
                  this.txtMutation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                  this.txtMutation.FocusSelect = true;
                  this.txtMutation.InputDecimalPlace = 2;
                  this.txtMutation.InputFilterType = FZYK.WinCtrl.FilterTypeEnum.Normal;
                  this.txtMutation.Location = new System.Drawing.Point(367, 149);
                  this.txtMutation.Name = "txtMutation";
                  this.txtMutation.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.txtMutation.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.txtMutation.PromptText = "";
                  this.txtMutation.Size = new System.Drawing.Size(54, 21);
                  this.txtMutation.TabIndex = 1;
                  this.txtMutation.UnderLine = true;
                  this.txtMutation.UnderLineColor = System.Drawing.Color.Black;
                  // 
                  // ykLabel5
                  // 
                  this.ykLabel5.AutoSize = true;
                  this.ykLabel5.IsTitle = false;
                  this.ykLabel5.Location = new System.Drawing.Point(198, 123);
                  this.ykLabel5.Name = "ykLabel5";
                  this.ykLabel5.Size = new System.Drawing.Size(53, 12);
                  this.ykLabel5.TabIndex = 0;
                  this.ykLabel5.Text = "栅格精度";
                  // 
                  // ykLabel6
                  // 
                  this.ykLabel6.AutoSize = true;
                  this.ykLabel6.IsTitle = false;
                  this.ykLabel6.Location = new System.Drawing.Point(349, 123);
                  this.ykLabel6.Name = "ykLabel6";
                  this.ykLabel6.Size = new System.Drawing.Size(17, 12);
                  this.ykLabel6.TabIndex = 0;
                  this.ykLabel6.Text = "mm";
                  // 
                  // txtT
                  // 
                  this.txtT.BackColor = System.Drawing.Color.White;
                  this.txtT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                  this.txtT.FocusSelect = true;
                  this.txtT.InputDecimalPlace = 2;
                  this.txtT.InputFilterType = FZYK.WinCtrl.FilterTypeEnum.Normal;
                  this.txtT.Location = new System.Drawing.Point(257, 119);
                  this.txtT.Name = "txtT";
                  this.txtT.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.txtT.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.txtT.PromptText = "";
                  this.txtT.Size = new System.Drawing.Size(82, 21);
                  this.txtT.TabIndex = 1;
                  this.txtT.UnderLine = true;
                  this.txtT.UnderLineColor = System.Drawing.Color.Black;
                  // 
                  // ykLabel7
                  // 
                  this.ykLabel7.AutoSize = true;
                  this.ykLabel7.IsTitle = false;
                  this.ykLabel7.Location = new System.Drawing.Point(174, 94);
                  this.ykLabel7.Name = "ykLabel7";
                  this.ykLabel7.Size = new System.Drawing.Size(77, 12);
                  this.ykLabel7.TabIndex = 0;
                  this.ykLabel7.Text = "零件长宽小于";
                  // 
                  // txtSize
                  // 
                  this.txtSize.BackColor = System.Drawing.Color.White;
                  this.txtSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                  this.txtSize.FocusSelect = true;
                  this.txtSize.InputDecimalPlace = 2;
                  this.txtSize.InputFilterType = FZYK.WinCtrl.FilterTypeEnum.Normal;
                  this.txtSize.Location = new System.Drawing.Point(257, 90);
                  this.txtSize.Name = "txtSize";
                  this.txtSize.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.txtSize.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.txtSize.PromptText = "";
                  this.txtSize.Size = new System.Drawing.Size(82, 21);
                  this.txtSize.TabIndex = 1;
                  this.txtSize.UnderLine = true;
                  this.txtSize.UnderLineColor = System.Drawing.Color.Black;
                  // 
                  // ykLabel8
                  // 
                  this.ykLabel8.AutoSize = true;
                  this.ykLabel8.IsTitle = false;
                  this.ykLabel8.Location = new System.Drawing.Point(349, 94);
                  this.ykLabel8.Name = "ykLabel8";
                  this.ykLabel8.Size = new System.Drawing.Size(83, 12);
                  this.ykLabel8.TabIndex = 0;
                  this.ykLabel8.Text = "mm 定义为小件";
                  // 
                  // ykLabel9
                  // 
                  this.ykLabel9.AutoSize = true;
                  this.ykLabel9.IsTitle = false;
                  this.ykLabel9.Location = new System.Drawing.Point(183, 185);
                  this.ykLabel9.Name = "ykLabel9";
                  this.ykLabel9.Size = new System.Drawing.Size(179, 12);
                  this.ykLabel9.TabIndex = 0;
                  this.ykLabel9.Text = "非组合件是否旋转180°交替排入";
                  // 
                  // cmbRotate
                  // 
                  this.cmbRotate.BackColor = System.Drawing.Color.White;
                  this.cmbRotate.BorderColor = System.Drawing.Color.Black;
                  this.cmbRotate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                  this.cmbRotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                  this.cmbRotate.FormattingEnabled = true;
                  this.cmbRotate.Items.AddRange(new object[] {
            "是",
            "否"});
                  this.cmbRotate.Location = new System.Drawing.Point(368, 181);
                  this.cmbRotate.Name = "cmbRotate";
                  this.cmbRotate.PromptFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                  this.cmbRotate.PromptForeColor = System.Drawing.SystemColors.GrayText;
                  this.cmbRotate.PromptText = "";
                  this.cmbRotate.Size = new System.Drawing.Size(62, 20);
                  this.cmbRotate.TabIndex = 9;
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
                  this.btnSure.Image = ((System.Drawing.Image)(resources.GetObject("btnSure.Image")));
                  this.btnSure.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
                  this.btnSure.Location = new System.Drawing.Point(212, 217);
                  this.btnSure.Name = "btnSure";
                  this.btnSure.Size = new System.Drawing.Size(75, 23);
                  this.btnSure.TabIndex = 10;
                  this.btnSure.Text = "确定";
                  this.btnSure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                  this.btnSure.TextType = FZYK.WinCtrl.YKButton.YKBtnTextType.Sure;
                  this.btnSure.UseVisualStyleBackColor = true;
                  this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
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
                  this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
                  this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
                  this.btnCancel.Location = new System.Drawing.Point(311, 217);
                  this.btnCancel.Name = "btnCancel";
                  this.btnCancel.Size = new System.Drawing.Size(75, 23);
                  this.btnCancel.TabIndex = 10;
                  this.btnCancel.Text = "取消";
                  this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                  this.btnCancel.TextType = FZYK.WinCtrl.YKButton.YKBtnTextType.Cancel;
                  this.btnCancel.UseVisualStyleBackColor = true;
                  this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
                  // 
                  // FrmNestSet
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.ClientSize = new System.Drawing.Size(611, 259);
                  this.Controls.Add(this.btnCancel);
                  this.Controls.Add(this.btnSure);
                  this.Controls.Add(this.cmbRotate);
                  this.Controls.Add(this.cmbType);
                  this.Controls.Add(this.txtMutation);
                  this.Controls.Add(this.txtCross);
                  this.Controls.Add(this.txtPop);
                  this.Controls.Add(this.txtSize);
                  this.Controls.Add(this.txtT);
                  this.Controls.Add(this.ykLabel8);
                  this.Controls.Add(this.ykLabel6);
                  this.Controls.Add(this.txtProtect);
                  this.Controls.Add(this.ykLabel2);
                  this.Controls.Add(this.ykLabel7);
                  this.Controls.Add(this.ykLabel5);
                  this.Controls.Add(this.ykLabel9);
                  this.Controls.Add(this.ykLabel4);
                  this.Controls.Add(this.ykLabel3);
                  this.Controls.Add(this.ykLabel1);
                  this.Name = "FrmNestSet";
                  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                  this.Text = "设置";
                  this.Shown += new System.EventHandler(this.FrmNestSet_Shown);
                  this.ResumeLayout(false);
                  this.PerformLayout();

            }

            #endregion

            private WinCtrl .YKLabel ykLabel1;
            private WinCtrl .YKTextBox txtProtect;
            private WinCtrl .YKLabel ykLabel2;
            private WinCtrl .YKLabel ykLabel3;
            private WinCtrl .YKComboBox cmbType;
            private WinCtrl .YKLabel ykLabel4;
            private WinCtrl .YKTextBox txtPop;
            private WinCtrl .YKTextBox txtCross;
            private WinCtrl .YKTextBox txtMutation;
            private WinCtrl .YKLabel ykLabel5;
            private WinCtrl .YKLabel ykLabel6;
            private WinCtrl .YKTextBox txtT;
            private WinCtrl .YKLabel ykLabel7;
            private WinCtrl .YKTextBox txtSize;
            private WinCtrl .YKLabel ykLabel8;
            private WinCtrl .YKLabel ykLabel9;
            private WinCtrl .YKComboBox cmbRotate;
            private WinCtrl .YKButton btnSure;
            private WinCtrl .YKButton btnCancel;
      }
}