namespace FZYK .Nest
{
      partial class FrmText
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
                  this.txtvalue = new FZYK.WinCtrl.YKRichTextBox(this.components);
                  this.SuspendLayout();
                  // 
                  // txtvalue
                  // 
                  this.txtvalue.Dock = System.Windows.Forms.DockStyle.Fill;
                  this.txtvalue.Font = new System.Drawing.Font("宋体", 5F);
                  this.txtvalue.Location = new System.Drawing.Point(0, 0);
                  this.txtvalue.Name = "txtvalue";
                  this.txtvalue.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
                  this.txtvalue.Size = new System.Drawing.Size(1030, 647);
                  this.txtvalue.TabIndex = 0;
                  this.txtvalue.Text = "";
                  // 
                  // frmtext
                  // 
                  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                  this.ClientSize = new System.Drawing.Size(1030, 647);
                  this.Controls.Add(this.txtvalue);
                  this.Name = "frmtext";
                  this.Text = "frmtext";
                  this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                  this.Shown += new System.EventHandler(this.frmtext_Shown);
                  this.ResumeLayout(false);

            }

            #endregion

            private FZYK .WinCtrl .YKRichTextBox txtvalue;
      }
}