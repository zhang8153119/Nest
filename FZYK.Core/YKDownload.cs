using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FZYK.Core
{
    public partial class YKDownload : Form
    {
        public YKDownload(string MenuName, string ModuleName, string tableName, string keyName, int keyValue)
        {
            _MenuName = MenuName;
            _ModuleName = ModuleName;
            _tableName = tableName;
            _keyName = keyName;
            _keyValue = keyValue;
            InitializeComponent();
        }
        public YKDownload()
        {
            InitializeComponent();
        }
        string _MenuName = "";//菜单名
        string _ModuleName = "";//模块名
        string _tableName = "";//表名
        string _keyName = "";//主键名
        int _keyValue = 0;//主键值
        static YKDownload DownLoad;
        private void YKDownload_Load(object sender, EventArgs e)
        {
            BindText();
        }
        /// <summary>
        /// 绑定控件
        /// </summary>
        private void BindText()
        {
            ykDownloadControl1._menuName = _MenuName;
            ykDownloadControl1._module = _ModuleName;
            ykDownloadControl1._tableName = _tableName;
            ykDownloadControl1._keyName = _keyName;
            ykDownloadControl1._keyvalue = _keyValue;
            ykDownloadControl1.BindView();
        }
        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="MenuName"></param>
        /// <param name="ModuleName"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        public static void ShowBox(string MenuName, string ModuleName, string tableName, string keyName, int keyValue)
        {
            DownLoad = new YKDownload();
            DownLoad._MenuName = MenuName;
            DownLoad._ModuleName = ModuleName;
            DownLoad._tableName = tableName;
            DownLoad._keyName = keyName;
            DownLoad._keyValue = keyValue;
            DownLoad.BindText();
            DownLoad.ShowDialog();
        }
    }
}
