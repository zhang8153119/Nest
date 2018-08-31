using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FZYK.Core
{
    /// <summary>
    /// 窗体容器。可以在此窗体弹出多个窗体，并设置停靠方式
    /// </summary>
    public partial class FrmDockForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmDockForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 记录已经弹出的子窗体
        /// </summary>
        private Dictionary<string, DockContent> dicForms = new Dictionary<string, DockContent>(2);
        private void FrmDockForm_Load(object sender, EventArgs e)
        {
            dicForms.Clear();
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="frm">要弹出的窗体</param>
        /// <param name="dockState">停靠方式</param>
        public void ShowForm(DockContent frm, DockState dockState)
        {
            if (!dicForms.ContainsKey(frm.Name))
            {
                dicForms.Add(frm.Name, frm);
                frm.Show(dockPanel1, dockState);
            }
            else
                dicForms[frm.Name].Activate();
        }
    }
}
