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
    /// 单据查询界面的父窗体，所有查询界面都继承该窗体。
    /// </summary>
    public partial class BaseSearch : DockContent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseSearch()
        {
            InitializeComponent();
            string temp = FZYK.Com.YKFileOperate.IniReadValue("SearchTopN", "SearchTopN");
            if (temp != "")
                _topN = Convert.ToInt32(temp);
            else
                _topN = 30;
            tsbtnShowTop.Text = "显示前" + _topN + "行";
        }
        #region 全局变量
        /// <summary>
        /// 消息提醒跳转提供的where条件
        /// </summary>
        public string _pSqlWhere = "";
        /// <summary>
        /// 该主窗体对应的singleForm窗体的类名
        /// </summary>
        public string _frmSingleFrom = "";
        /// <summary>
        /// 点添加按钮时，是否新实例化子窗体界面。
        /// 默认为true，需要新实例化子窗体。
        /// 当子窗体已经存在时为false
        /// </summary>
        public bool _map = true;
        /// <summary>
        /// 显示前N行
        /// </summary>
        public int _topN = 30;
        #endregion
        #region 虚方法
        /// <summary>
        /// 初始化界面设置 如按钮显示隐藏
        /// </summary>
        public virtual void InitSetting() { }
        /// <summary>
        /// 设置权限
        /// </summary>
        public virtual void ShowRight() { }
        /// <summary>
        /// 添加
        /// </summary>
        public virtual void Add()
        {
            if (!_frmSingleFrom.Equals(""))
            {
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    if (Application.OpenForms[i].Name == _frmSingleFrom)
                    {
                        Application.OpenForms[i].Activate();
                        _map = false;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public virtual void Save() { }
        /// <summary>
        /// 修改
        /// </summary>
        public virtual void Modify()
        {            
            if (!_frmSingleFrom.Equals(""))
            {
                //当子窗体存在实例，先关闭，再根据所选主窗体单据实例化新的子窗体
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    if (Application.OpenForms[i].Name == _frmSingleFrom)
                        Application.OpenForms[i].Close();
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Del() { }
        /// <summary>
        /// 显示前N条数据
        /// </summary>
        public virtual void ShowTopN() { }
        /// <summary>
        /// 查找
        /// </summary>
        public virtual void Search() { }
        public virtual void ShowToday() { }
        /// <summary>
        /// 打印
        /// </summary>
        public virtual void Print() { }
        /// <summary>
        /// 导出
        /// </summary>
        public virtual void ImportExcel() { }
        /// <summary>
        /// 绑定ykDataGridView数据
        /// </summary>
        public virtual void BindDataGrid() { }
        /// <summary>
        ///  绑定ykDataGridView数据
        /// </summary>
        /// <param name="sqlWhere">where条件</param>
        public virtual void BindDataGrid(string sqlWhere) { }
        /// <summary>
        /// 绑定ykDataGridView数据，带参数
        /// </summary>
        /// <param name="dic">查找条件</param>
        public virtual void BindDataGrid(Dictionary<string,string> dic) { }
       
        /// <summary>
        /// 设置界面控件的enabled属性
        /// </summary>
        /// <param name="flag">true，false</param>
        public virtual void LockForm(bool flag) { }
        /// <summary>
        /// 显示设置列窗体界面
        /// </summary>
        public virtual void ShowColumnSetting() { } 
        #endregion
        #region 属性
        /// <summary>
        /// 设置是否显示添加按钮
        /// </summary>
        public bool bCanAdd
        {
            set { tsbtnAdd.Visible = value; }
        }
        public bool bCanSave
        {
            set { tsbtnSave.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示修改按钮
        /// </summary>
        public bool bCanModify
        {
            set { tsbtnModify.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示删除按钮
        /// </summary>
        public bool bCanDel
        {
            set { tsbtnDelete.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示 显示前N行按钮
        /// </summary>
        public bool bCanShowTop
        {
            set { tsbtnShowTop.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示查找按钮
        /// </summary>
        public bool bCanSearch
        {
            set { tsbtnSearch.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示扩展功能按钮
        /// </summary>
        public bool bCanTools
        {
            set { tsbtnTools.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示列头显示设置按钮
        /// </summary>
        public bool bCanColSetting
        {
            set { tsbtnColSetting.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示打印按钮
        /// </summary>
        public bool bCanPrint
        {
            set { tsbtnPrint.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示导出按钮
        /// </summary>
        public bool bCanImport
        {
            set { tsbtnImport.Visible = value; }
        }
        /// <summary>
        /// 显示查看今日数据
        /// </summary>
        public bool bCanShowToday
        {
              set { tsbtnShowToday .Visible = value; }
        }
        #endregion
        #region 基础方法
        /// <summary>
        /// 向扩展功能的下拉框添加选项
        /// </summary>
        /// <param name="Name">新加选项的Name属性</param>
        /// <param name="Text">新加选项的Text属性</param>
        /// <returns>新加选项实例</returns>
        public ToolStripMenuItem AddTools(string Name, string Text)
        {
            ToolStripMenuItem itool = new ToolStripMenuItem();
            itool.Image = global::FZYK.Core.Properties.Resources.iplugs;
            itool.Name = Name;
            itool.Text = Text;
            tsbtnTools.DropDownItems.Add(itool);
            return itool;
        }
        #endregion
        private void tsbtnAdd_Click(object sender, EventArgs e)
        {
            this.Add();
        }

        private void tsbtnModify_Click(object sender, EventArgs e)
        {
            this.Modify();
        }

        private void tsbtnDelete_Click(object sender, EventArgs e)
        {
            this.Del();
        }

        private void tsbtnShowTop_Click(object sender, EventArgs e)
        {
            this.ShowTopN();            
        }

        private void tsbtnSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void tsbtnColSetting_Click(object sender, EventArgs e)
        {
            this.ShowColumnSetting();
        }

        private void tsbtnPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void tsbtnImport_Click(object sender, EventArgs e)
        {
            this.ImportExcel();
        }

        private void tsbtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void tstxtTopN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar== (char)13)
            {
                if (tstxtTopN.Text.Trim().Length == 0)
                {
                    YKMessageBox.ShowBox("请输入要显示的条数!");
                    return;
                }
                if(!FZYK.Com.YKPageValidate.IsNumber(tstxtTopN.Text.Trim()))
                {
                    YKMessageBox.ShowBox("输入必须是正整数!");
                    return;
                }
                _topN = Convert.ToInt32(tstxtTopN.Text.Trim());
                tsbtnShowTop.Text = "显示前" + _topN + "行";
                FZYK.Com.YKFileOperate.IniWriteValue("SearchTopN","SearchTopN", _topN.ToString());
                ShowTopN();
            }
        }

        private void tsbtnShowToday_Click(object sender, EventArgs e)
        {
              this .ShowToday();
        }
    }
}
