﻿namespace GPU
{
    partial class OpenClSetting
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
                  this.label1 = new System.Windows.Forms.Label();
                  this.platformComboBox = new System.Windows.Forms.ComboBox();
                  this.label2 = new System.Windows.Forms.Label();
                  this.deviceComboBox = new System.Windows.Forms.ComboBox();
                  this.label3 = new System.Windows.Forms.Label();
                  this.label4 = new System.Windows.Forms.Label();
                  this.computeUnitLabel = new System.Windows.Forms.Label();
                  this.label5 = new System.Windows.Forms.Label();
                  this.vendorNameLabel = new System.Windows.Forms.Label();
                  this.fpTypeComboBox = new System.Windows.Forms.ComboBox();
                  this.exitButton = new System.Windows.Forms.Button();
                  this.okButton = new System.Windows.Forms.Button();
                  this.messageLabel = new System.Windows.Forms.Label();
                  this.label6 = new System.Windows.Forms.Label();
                  this.deviceTypeLabel = new System.Windows.Forms.Label();
                  this.SuspendLayout();
                  // 
                  // label1
                  // 
                  this.label1.AutoSize = true;
                  this.label1.Location = new System.Drawing.Point(10, 13);
                  this.label1.Name = "label1";
                  this.label1.Size = new System.Drawing.Size(35, 12);
                  this.label1.TabIndex = 0;
                  this.label1.Text = "平台:";
                  // 
                  // platformComboBox
                  // 
                  this.platformComboBox.DisplayMember = "Name";
                  this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                  this.platformComboBox.FormattingEnabled = true;
                  this.platformComboBox.Location = new System.Drawing.Point(13, 28);
                  this.platformComboBox.Name = "platformComboBox";
                  this.platformComboBox.Size = new System.Drawing.Size(218, 20);
                  this.platformComboBox.TabIndex = 1;
                  this.platformComboBox.ValueMember = "Name";
                  this.platformComboBox.SelectedIndexChanged += new System.EventHandler(this.platformComboBox_SelectedIndexChanged);
                  // 
                  // label2
                  // 
                  this.label2.AutoSize = true;
                  this.label2.Location = new System.Drawing.Point(10, 54);
                  this.label2.Name = "label2";
                  this.label2.Size = new System.Drawing.Size(35, 12);
                  this.label2.TabIndex = 2;
                  this.label2.Text = "设备:";
                  // 
                  // deviceComboBox
                  // 
                  this.deviceComboBox.DisplayMember = "Name";
                  this.deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                  this.deviceComboBox.FormattingEnabled = true;
                  this.deviceComboBox.Location = new System.Drawing.Point(13, 69);
                  this.deviceComboBox.Name = "deviceComboBox";
                  this.deviceComboBox.Size = new System.Drawing.Size(218, 20);
                  this.deviceComboBox.TabIndex = 3;
                  this.deviceComboBox.ValueMember = "Name";
                  this.deviceComboBox.SelectedIndexChanged += new System.EventHandler(this.deviceComboBox_SelectedIndexChanged);
                  // 
                  // label3
                  // 
                  this.label3.AutoSize = true;
                  this.label3.Location = new System.Drawing.Point(10, 96);
                  this.label3.Name = "label3";
                  this.label3.Size = new System.Drawing.Size(71, 12);
                  this.label3.TabIndex = 4;
                  this.label3.Text = "浮点数精度:";
                  // 
                  // label4
                  // 
                  this.label4.AutoSize = true;
                  this.label4.Location = new System.Drawing.Point(249, 70);
                  this.label4.Name = "label4";
                  this.label4.Size = new System.Drawing.Size(47, 12);
                  this.label4.TabIndex = 5;
                  this.label4.Text = "核心数:";
                  // 
                  // computeUnitLabel
                  // 
                  this.computeUnitLabel.AutoSize = true;
                  this.computeUnitLabel.Location = new System.Drawing.Point(302, 70);
                  this.computeUnitLabel.Name = "computeUnitLabel";
                  this.computeUnitLabel.Size = new System.Drawing.Size(59, 12);
                  this.computeUnitLabel.TabIndex = 6;
                  this.computeUnitLabel.Text = "(unknown)";
                  // 
                  // label5
                  // 
                  this.label5.AutoSize = true;
                  this.label5.Location = new System.Drawing.Point(249, 30);
                  this.label5.Name = "label5";
                  this.label5.Size = new System.Drawing.Size(35, 12);
                  this.label5.TabIndex = 7;
                  this.label5.Text = "品牌:";
                  // 
                  // vendorNameLabel
                  // 
                  this.vendorNameLabel.Location = new System.Drawing.Point(287, 30);
                  this.vendorNameLabel.Name = "vendorNameLabel";
                  this.vendorNameLabel.Size = new System.Drawing.Size(120, 36);
                  this.vendorNameLabel.TabIndex = 8;
                  this.vendorNameLabel.Text = "(unknown)";
                  // 
                  // fpTypeComboBox
                  // 
                  this.fpTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                  this.fpTypeComboBox.FormattingEnabled = true;
                  this.fpTypeComboBox.Items.AddRange(new object[] {
            "Single Precision"});
                  this.fpTypeComboBox.Location = new System.Drawing.Point(13, 111);
                  this.fpTypeComboBox.Name = "fpTypeComboBox";
                  this.fpTypeComboBox.Size = new System.Drawing.Size(218, 20);
                  this.fpTypeComboBox.TabIndex = 9;
                  this.fpTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.fpTypeComboBox_SelectedIndexChanged);
                  // 
                  // exitButton
                  // 
                  this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                  this.exitButton.Location = new System.Drawing.Point(332, 189);
                  this.exitButton.Name = "exitButton";
                  this.exitButton.Size = new System.Drawing.Size(75, 21);
                  this.exitButton.TabIndex = 10;
                  this.exitButton.Text = "E&xit";
                  this.exitButton.UseVisualStyleBackColor = true;
                  this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
                  // 
                  // okButton
                  // 
                  this.okButton.Enabled = false;
                  this.okButton.Location = new System.Drawing.Point(251, 189);
                  this.okButton.Name = "okButton";
                  this.okButton.Size = new System.Drawing.Size(75, 21);
                  this.okButton.TabIndex = 11;
                  this.okButton.Text = "OK";
                  this.okButton.UseVisualStyleBackColor = true;
                  this.okButton.Click += new System.EventHandler(this.okButton_Click);
                  // 
                  // messageLabel
                  // 
                  this.messageLabel.Location = new System.Drawing.Point(10, 137);
                  this.messageLabel.Name = "messageLabel";
                  this.messageLabel.Size = new System.Drawing.Size(397, 50);
                  this.messageLabel.TabIndex = 12;
                  this.messageLabel.Text = "Ready";
                  // 
                  // label6
                  // 
                  this.label6.AutoSize = true;
                  this.label6.Location = new System.Drawing.Point(237, 96);
                  this.label6.Name = "label6";
                  this.label6.Size = new System.Drawing.Size(59, 12);
                  this.label6.TabIndex = 13;
                  this.label6.Text = "设备类型:";
                  // 
                  // deviceTypeLabel
                  // 
                  this.deviceTypeLabel.AutoSize = true;
                  this.deviceTypeLabel.Location = new System.Drawing.Point(302, 96);
                  this.deviceTypeLabel.Name = "deviceTypeLabel";
                  this.deviceTypeLabel.Size = new System.Drawing.Size(59, 12);
                  this.deviceTypeLabel.TabIndex = 14;
                  this.deviceTypeLabel.Text = "(unknown)";
                  // 
                  // OpenClSetting
                  // 
                  this.AcceptButton = this.okButton;
                  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.CancelButton = this.exitButton;
                  this.ClientSize = new System.Drawing.Size(419, 222);
                  this.Controls.Add(this.deviceTypeLabel);
                  this.Controls.Add(this.label6);
                  this.Controls.Add(this.messageLabel);
                  this.Controls.Add(this.okButton);
                  this.Controls.Add(this.exitButton);
                  this.Controls.Add(this.fpTypeComboBox);
                  this.Controls.Add(this.vendorNameLabel);
                  this.Controls.Add(this.label5);
                  this.Controls.Add(this.computeUnitLabel);
                  this.Controls.Add(this.label4);
                  this.Controls.Add(this.label3);
                  this.Controls.Add(this.deviceComboBox);
                  this.Controls.Add(this.label2);
                  this.Controls.Add(this.platformComboBox);
                  this.Controls.Add(this.label1);
                  this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                  this.MaximizeBox = false;
                  this.MinimizeBox = false;
                  this.Name = "OpenClSetting";
                  this.ShowInTaskbar = false;
                  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                  this.Text = "硬件加速";
                  this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OpenClSetting_FormClosed);
                  this.Load += new System.EventHandler(this.OpenClSetting_Load);
                  this.ResumeLayout(false);
                  this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label computeUnitLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label vendorNameLabel;
        private System.Windows.Forms.ComboBox fpTypeComboBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label deviceTypeLabel;
    }
}