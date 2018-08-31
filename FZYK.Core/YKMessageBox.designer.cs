namespace FZYK.Core
{
    partial class YKMessageBox
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
              this .components = new System .ComponentModel .Container();
              this .btnSent = new FZYK .WinCtrl .YKButton(this .components);
              this .lblTitle = new System .Windows .Forms .Label();
              this .panel1 = new System .Windows .Forms .Panel();
              this .lblMessage = new System .Windows .Forms .Label();
              this .toolTip1 = new System .Windows .Forms .ToolTip(this .components);
              this .btnOK = new FZYK .WinCtrl .YKButton(this .components);
              this .lblTimer = new System .Windows .Forms .Label();
              this .btnCancel = new FZYK .WinCtrl .YKButton(this .components);
              this .ykPanel1 = new FZYK .WinCtrl .YKPanel(this .components);
              this .panel1 .SuspendLayout();
              this .ykPanel1 .SuspendLayout();
              this .SuspendLayout();
              // 
              // btnSent
              // 
              this .btnSent .A_BorderColor = System .Drawing .SystemColors .Desktop;
              this .btnSent .A_GotFocusColor = System .Drawing .SystemColors .ActiveCaption;
              this .btnSent .A_MouseDownBackColor = System .Drawing .Color .SteelBlue;
              this .btnSent .A_MouseOverBackColor = System .Drawing .Color .LightSteelBlue;
              this .btnSent .DisplayFocusCues = false;
              this .btnSent .FlatAppearance .BorderColor = System .Drawing .SystemColors .Desktop;
              this .btnSent .FlatAppearance .MouseDownBackColor = System .Drawing .Color .SteelBlue;
              this .btnSent .FlatAppearance .MouseOverBackColor = System .Drawing .Color .LightSteelBlue;
              this .btnSent .FlatStyle = System .Windows .Forms .FlatStyle .Flat;
              this .btnSent .ForeColor = System .Drawing .Color .Black;
              this .btnSent .Location = new System .Drawing .Point(3, 10);
              this .btnSent .Name = "btnSent";
              this .btnSent .Size = new System .Drawing .Size(119, 26);
              this .btnSent .TabIndex = 12;
              this .btnSent .Text = "生成文件到桌面";
              this .btnSent .TextType = FZYK .WinCtrl .YKButton .YKBtnTextType .Normal;
              this .toolTip1 .SetToolTip(this .btnSent, "将此异常信息生成文件至桌面,并请将异常文件发送至网管处,谢谢!");
              this .btnSent .UseVisualStyleBackColor = true;
              this .btnSent .Visible = false;
              this .btnSent .Click += new System .EventHandler(this .btnSent_Click);
              // 
              // lblTitle
              // 
              this .lblTitle .AutoSize = true;
              this .lblTitle .BackColor = System .Drawing .Color .Transparent;
              this .lblTitle .Font = new System .Drawing .Font("Tahoma", 8.25F, System .Drawing .FontStyle .Bold, System .Drawing .GraphicsUnit .Point, ((byte)(0)));
              this .lblTitle .ForeColor = System .Drawing .Color .White;
              this .lblTitle .Location = new System .Drawing .Point(12, 12);
              this .lblTitle .Name = "lblTitle";
              this .lblTitle .Size = new System .Drawing .Size(0, 13);
              this .lblTitle .TabIndex = 14;
              // 
              // panel1
              // 
              this .panel1 .AutoScroll = true;
              this .panel1 .BackColor = System .Drawing .Color .Transparent;
              this .panel1 .Controls .Add(this .lblMessage);
              this .panel1 .Location = new System .Drawing .Point(18, 12);
              this .panel1 .Margin = new System .Windows .Forms .Padding(3, 3, 3, 10);
              this .panel1 .Name = "panel1";
              this .panel1 .Size = new System .Drawing .Size(357, 122);
              this .panel1 .TabIndex = 13;
              // 
              // lblMessage
              // 
              this .lblMessage .Dock = System .Windows .Forms .DockStyle .Top;
              this .lblMessage .Font = new System .Drawing .Font("Tahoma", 8.25F, System .Drawing .FontStyle .Regular, System .Drawing .GraphicsUnit .Point, ((byte)(0)));
              this .lblMessage .Location = new System .Drawing .Point(0, 0);
              this .lblMessage .Name = "lblMessage";
              this .lblMessage .Size = new System .Drawing .Size(340, 164);
              this .lblMessage .TabIndex = 0;
              // 
              // toolTip1
              // 
              this .toolTip1 .IsBalloon = true;
              this .toolTip1 .ToolTipTitle = "提示";
              // 
              // btnOK
              // 
              this .btnOK .A_BorderColor = System .Drawing .SystemColors .Desktop;
              this .btnOK .A_GotFocusColor = System .Drawing .SystemColors .ActiveCaption;
              this .btnOK .A_MouseDownBackColor = System .Drawing .Color .SteelBlue;
              this .btnOK .A_MouseOverBackColor = System .Drawing .Color .LightSteelBlue;
              this .btnOK .DisplayFocusCues = false;
              this .btnOK .FlatAppearance .BorderColor = System .Drawing .SystemColors .Desktop;
              this .btnOK .FlatAppearance .MouseDownBackColor = System .Drawing .Color .SteelBlue;
              this .btnOK .FlatAppearance .MouseOverBackColor = System .Drawing .Color .LightSteelBlue;
              this .btnOK .FlatStyle = System .Windows .Forms .FlatStyle .Flat;
              this .btnOK .ForeColor = System .Drawing .Color .Black;
              this .btnOK .Location = new System .Drawing .Point(222, 10);
              this .btnOK .Name = "btnOK";
              this .btnOK .Size = new System .Drawing .Size(70, 26);
              this .btnOK .TabIndex = 9;
              this .btnOK .Text = "确 定";
              this .btnOK .TextType = FZYK .WinCtrl .YKButton .YKBtnTextType .Normal;
              this .btnOK .UseVisualStyleBackColor = true;
              this .btnOK .Click += new System .EventHandler(this .btnOK_Click);
              // 
              // lblTimer
              // 
              this .lblTimer .AutoSize = true;
              this .lblTimer .BackColor = System .Drawing .Color .Transparent;
              this .lblTimer .Font = new System .Drawing .Font("Tahoma", 9.75F, System .Drawing .FontStyle .Bold, System .Drawing .GraphicsUnit .Point, ((byte)(0)));
              this .lblTimer .Location = new System .Drawing .Point(9, 118);
              this .lblTimer .Name = "lblTimer";
              this .lblTimer .Size = new System .Drawing .Size(0, 16);
              this .lblTimer .TabIndex = 11;
              // 
              // btnCancel
              // 
              this .btnCancel .A_BorderColor = System .Drawing .SystemColors .Desktop;
              this .btnCancel .A_GotFocusColor = System .Drawing .SystemColors .ActiveCaption;
              this .btnCancel .A_MouseDownBackColor = System .Drawing .Color .SteelBlue;
              this .btnCancel .A_MouseOverBackColor = System .Drawing .Color .LightSteelBlue;
              this .btnCancel .DisplayFocusCues = false;
              this .btnCancel .FlatAppearance .BorderColor = System .Drawing .SystemColors .Desktop;
              this .btnCancel .FlatAppearance .MouseDownBackColor = System .Drawing .Color .SteelBlue;
              this .btnCancel .FlatAppearance .MouseOverBackColor = System .Drawing .Color .LightSteelBlue;
              this .btnCancel .FlatStyle = System .Windows .Forms .FlatStyle .Flat;
              this .btnCancel .ForeColor = System .Drawing .Color .Black;
              this .btnCancel .Location = new System .Drawing .Point(302, 10);
              this .btnCancel .Name = "btnCancel";
              this .btnCancel .Size = new System .Drawing .Size(70, 26);
              this .btnCancel .TabIndex = 10;
              this .btnCancel .Text = "取 消";
              this .btnCancel .TextType = FZYK .WinCtrl .YKButton .YKBtnTextType .Normal;
              this .btnCancel .UseVisualStyleBackColor = true;
              this .btnCancel .Click += new System .EventHandler(this .btnCancel_Click);
              // 
              // ykPanel1
              // 
              this .ykPanel1 .Controls .Add(this .btnSent);
              this .ykPanel1 .Controls .Add(this .btnCancel);
              this .ykPanel1 .Controls .Add(this .btnOK);
              this .ykPanel1 .Dock = System .Windows .Forms .DockStyle .Bottom;
              this .ykPanel1 .Location = new System .Drawing .Point(0, 152);
              this .ykPanel1 .Name = "ykPanel1";
              this .ykPanel1 .Size = new System .Drawing .Size(382, 44);
              this .ykPanel1 .TabIndex = 15;
              // 
              // YKMessageBox
              // 
              this .AutoScaleDimensions = new System .Drawing .SizeF(6F, 12F);
              this .AutoScaleMode = System .Windows .Forms .AutoScaleMode .Font;
              this .ClientSize = new System .Drawing .Size(382, 196);
              this .ControlBox = false;
              this .Controls .Add(this .ykPanel1);
              this .Controls .Add(this .lblTitle);
              this .Controls .Add(this .panel1);
              this .Controls .Add(this .lblTimer);
              this .FormBorderStyle = System .Windows .Forms .FormBorderStyle .FixedSingle;
              this .Name = "YKMessageBox";
              this .Opacity = 0.9D;
              this .ShowInTaskbar = false;
              this .StartPosition = System .Windows .Forms .FormStartPosition .CenterScreen;
              this .Text = "YKMessageBox";
              this .Load += new System .EventHandler(this .YKMessageBox_Load);
              this .Paint += new System .Windows .Forms .PaintEventHandler(this .YKMessageBox_Paint);
              this .panel1 .ResumeLayout(false);
              this .ykPanel1 .ResumeLayout(false);
              this .ResumeLayout(false);
              this .PerformLayout();

        }

        #endregion

        private WinCtrl.YKButton  btnSent;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMessage;
        private WinCtrl.YKButton btnOK;
        private System.Windows.Forms.Label lblTimer;
        private WinCtrl.YKButton btnCancel;
        private WinCtrl.YKPanel ykPanel1;
    }
}