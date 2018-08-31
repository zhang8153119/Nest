using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace FZYK.Core
{
    public partial class YKMessageBox : Form
    {
        public YKMessageBox()
        {
            InitializeComponent();
        }
        static YKMessageBox newMessageBox;
        private static Exception _ex; 
        private void YKMessageBox_Load(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 提示内容
        /// </summary>
        /// <param name="txtMessage"></param>
        /// <returns></returns>
        public static void  ShowBox(string txtMessage)
        {
            newMessageBox = new YKMessageBox();
            newMessageBox.Text = "提示";
            newMessageBox.lblMessage.Text = txtMessage;
            newMessageBox.btnCancel.Visible = false;
             newMessageBox.ShowDialog();
        }

        /// <summary>
        /// 提示异常内容
        /// </summary>
        /// <param name="txtMessage"></param>
        /// <returns></returns>
        public static void ShowBoxException(string txtMessage,Exception ex)
        {
            _ex = ex;
            newMessageBox = new YKMessageBox();
            newMessageBox.Text = "异常错误";
            newMessageBox.lblMessage.Text = txtMessage;
            newMessageBox.btnCancel.Visible = false;
            newMessageBox.btnSent.Visible = true;
            newMessageBox.ShowDialog();
        }

        /// <summary>
        /// 提示内容及标题
        /// </summary>
        /// <param name="txtMessage"></param>
        /// <param name="txtTitle"></param>
        /// <returns></returns>
        public static void  ShowBox(string txtMessage, string txtTitle)
        {
            newMessageBox = new YKMessageBox();
            newMessageBox.Text = txtTitle;
            newMessageBox.lblMessage.Text = txtMessage;
            newMessageBox.btnCancel.Visible = false;
             newMessageBox.ShowDialog();
        }

        /// <summary>
        /// 删除提示
        /// </summary>
        /// <param name="txtMessage"></param>
        /// <param name="txtTitle"></param>
        /// <returns></returns>       
        public static DialogResult ShowBoxDel(string txtMessage)
        {
            newMessageBox = new YKMessageBox();
            newMessageBox.Text = "删除确认";
            if (string.IsNullOrEmpty(txtMessage))
                txtMessage = "确认要删除本条记录吗？";
            newMessageBox.lblMessage.Text = txtMessage;
            return newMessageBox.ShowDialog();
        }

        /// <summary>
        /// 询问提示提示
        /// </summary>
        /// <param name="txtMessage"></param>
        /// <param name="txtTitle"></param>
        /// <returns></returns>
        public static DialogResult ShowBoxIf(string txtMessage)
        {
            newMessageBox = new YKMessageBox();
            newMessageBox.Text = "删除确认";
            newMessageBox.lblMessage.Text = txtMessage;
            return newMessageBox.ShowDialog();
        }

        private void YKMessageBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics mGraphics = e.Graphics;
            Pen pen1 = new Pen(Color.FromArgb(96, 155, 173), 1);

            Rectangle Area1 = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush LGB = new LinearGradientBrush(Area1, Color.FromArgb(96, 155, 173), Color.FromArgb(245, 251, 251), LinearGradientMode.Vertical);
            mGraphics.FillRectangle(LGB, Area1);
            mGraphics.DrawRectangle(pen1, Area1);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //returnResult = Button_id.OK;
            newMessageBox.Close(); 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            //Button_id = "Cancle";
            newMessageBox.Close();
        }

        private void btnSent_Click(object sender, EventArgs e)
        {
            string _DeskTop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            _DeskTop += @"\异常" + DateTime.Now.ToLocalTime().ToString("MM月dd日HH点mm分ss秒") + ".txt";
            FileStream fs = new FileStream(_DeskTop, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(lblMessage.Text.Trim()+_ex.StackTrace);
            sw.Close();
            fs.Close();
        }


    }
}
