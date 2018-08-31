namespace FZYK.Core
{
    partial class YKDownload
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
            this.ykDownloadControl1 = new FZYK.WinCtrl.YKDownloadControl();
            this.SuspendLayout();
            // 
            // ykDownloadControl1
            // 
            this.ykDownloadControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ykDownloadControl1.Location = new System.Drawing.Point(0, 0);
            this.ykDownloadControl1.Name = "ykDownloadControl1";
            this.ykDownloadControl1.Size = new System.Drawing.Size(749, 260);
            this.ykDownloadControl1.TabIndex = 0;
            // 
            // YKDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 260);
            this.Controls.Add(this.ykDownloadControl1);
            this.Name = "YKDownload";
            this.Text = "附件";
            this.Load += new System.EventHandler(this.YKDownload_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private WinCtrl.YKDownloadControl ykDownloadControl1;


    }
}