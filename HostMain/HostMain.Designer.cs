namespace FZYK .WinUI
{
      partial class HostMain
      {
            /// <summary>
            /// 必需的设计器变量。
            /// </summary>
            private System .ComponentModel .IContainer components = null;

            /// <summary>
            /// 清理所有正在使用的资源。
            /// </summary>
            /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
            protected override void Dispose(bool disposing)
            {
                  if (disposing && (components != null))
                  {
                        components .Dispose();
                  }
                  base .Dispose(disposing);
            }

            #region Windows 窗体设计器生成的代码

            /// <summary>
            /// 设计器支持所需的方法 - 不要修改
            /// 使用代码编辑器修改此方法的内容。
            /// </summary>
            private void InitializeComponent()
            {
                  this.components = new System.ComponentModel.Container();
                  this.btnNesting = new FZYK.WinCtrl.YKButton(this.components);
                  this.SuspendLayout();
                  // 
                  // btnNesting
                  // 
                  this.btnNesting.A_BorderColor = System.Drawing.SystemColors.Desktop;
                  this.btnNesting.A_GotFocusColor = System.Drawing.SystemColors.ActiveCaption;
                  this.btnNesting.A_MouseDownBackColor = System.Drawing.Color.SteelBlue;
                  this.btnNesting.A_MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
                  this.btnNesting.DisplayFocusCues = false;
                  this.btnNesting.FlatAppearance.BorderColor = System.Drawing.SystemColors.Desktop;
                  this.btnNesting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
                  this.btnNesting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
                  this.btnNesting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                  this.btnNesting.ForeColor = System.Drawing.Color.Black;
                  this.btnNesting.Location = new System.Drawing.Point(48, 25);
                  this.btnNesting.Name = "btnNesting";
                  this.btnNesting.Size = new System.Drawing.Size(75, 23);
                  this.btnNesting.TabIndex = 0;
                  this.btnNesting.Text = "套料";
                  this.btnNesting.TextType = FZYK.WinCtrl.YKButton.YKBtnTextType.Normal;
                  this.btnNesting.UseVisualStyleBackColor = true;
                  this.btnNesting.Click += new System.EventHandler(this.btnNesting_Click);
                  // 
                  // HostMain
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.ClientSize = new System.Drawing.Size(763, 373);
                  this.Controls.Add(this.btnNesting);
                  this.Name = "HostMain";
                  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                  this.Text = "系统入口";
                  this.ResumeLayout(false);

            }

            #endregion

            private WinCtrl .YKButton btnNesting;
      }
}

