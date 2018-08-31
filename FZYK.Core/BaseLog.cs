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
    public partial class BaseLog : DockContent
    {
        public BaseLog()
        {
            InitializeComponent();
        }

        private void BaseLog_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 显示数据
        /// </summary>
        public virtual void BindDataGrid() { }

    }
}
