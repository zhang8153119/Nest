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
    /// 选择框的基类
    /// </summary>
    public partial class BaseSelectDialog : DockContent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseSelectDialog()
        {
            InitializeComponent();
        }
        #region 属性
        /// <summary>
        /// 
        /// </summary>
        public string Key = String.Empty;
        public Dictionary<string, object> ResultList = new Dictionary<string, object>(2);
        public List<Dictionary<string, object>> MultiResult = new List<Dictionary<string, object>>(2);
        public virtual void AddResultList() { }
        #endregion
    }
}
