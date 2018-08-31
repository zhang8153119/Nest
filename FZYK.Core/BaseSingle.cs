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
    /// 窗体的状态，是添加 还是修改状态
    /// </summary>
    public enum FormOperate
    {
        /// <summary>
        /// 添加状态
        /// </summary>
        Add,
        /// <summary>
        /// 修改状态
        /// </summary>
        Modify
    }
    public partial class BaseSingle : DockContent
    {
        public BaseSingle()
        {
            InitializeComponent();
            tsbtnAudit.Text = YKControlText.GetLocker() + "(&A)";
            tsbtnUnAudit.Text = YKControlText.GetUnLocker()+"(&Q)";
        }
        public FormOperate _operate = FormOperate.Add;
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
        /// 保存
        /// </summary>
        public virtual void Save() { }
        /// <summary>
        /// 添加
        /// </summary>
        public virtual void Add() { }
        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Del() { }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        public virtual void CloseForm() 
        {
            this.Close();
        }
        /// <summary>
        /// 审核
        /// </summary>
        public virtual void Audit() { }
        /// <summary>
        /// 弃审
        /// </summary>
        public virtual void UnAudit() { }
        /// <summary>
        /// 打印
        /// </summary>
        public virtual void Print() { }
        /// <summary>
        /// 上一条
        /// </summary>
        public virtual void Back() { } //上一个
        /// <summary>
        /// 下一条
        /// </summary>
        public virtual void Forward() { } //下一个
        /// <summary>
        /// 传递
        /// </summary>
        public virtual void Notify() { }//提醒签名
        /// <summary>
        /// 接收
        /// </summary>
        public virtual void Sign() { }//签名反馈
        /// <summary>
        /// 设置界面控件的enabled属性
        /// </summary>
        /// <param name="flag">true，false</param>
        public virtual void LockForm(bool flag) { }
        /// <summary>
        /// 判断用户输入正确性
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidEnter()
        {
            return true;
        }
        public virtual void ExportExcel() { }
        #endregion
        #region 属性
        /// <summary>
        /// 设置是否显示保存按钮
        /// </summary>
        public bool bCanSave
        {
            set { tsbtnSave.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示添加按钮
        /// </summary>
        public bool bCanAdd
        {
            set { tsbtnAdd.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示删除按钮
        /// </summary>
        public bool bCanDelete
        {
            set { tsbtnDelete.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示审核按钮
        /// </summary>
        public bool bCanAudit
        {
            set { tsbtnAudit.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示弃审按钮
        /// </summary>
        public bool bCanUnAudit
        {
            set { tsbtnUnAudit.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示打印按钮
        /// </summary>
        public bool bCanPrint
        {
            set { tsbtnPrint.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示上一条按钮
        /// </summary>
        public bool bCanBack
        {
            set { tsbtnBack.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示下一条按钮
        /// </summary>
        public bool bCanForword
        {
            set { tsbtnForword.Visible = value; }
        }
        /// <summary>
        /// 设置是否显示传递接收按钮
        /// </summary>
        public bool bCanNotifyAndSign
        {
            set 
            {  
                tsbtnNotify.Visible = value;
                tsbtnSign.Visible = value;
            }
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
        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void tsbtnAdd_Click(object sender, EventArgs e)
        {
            _operate = FormOperate.Add;
            this.Add();
        }

        private void tsbtnDelete_Click(object sender, EventArgs e)
        {
            this.Del();
        }

        private void tsbtnAudit_Click(object sender, EventArgs e)
        {
            this.Audit();
        }

        private void tsbtnUnAudit_Click(object sender, EventArgs e)
        {
            this.UnAudit();
        }

        private void tsbtnPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        private void tsbtnBack_Click(object sender, EventArgs e)
        {
            this.Back();
        }

        private void tsbtnForword_Click(object sender, EventArgs e)
        {
            this.Forward();
        }

        private void tsbtnNotify_Click(object sender, EventArgs e)
        {
            this.Notify();
        }

        private void tsbtnSign_Click(object sender, EventArgs e)
        {
            this.Sign();
        }

        private void tsbtnClose_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void tsbtnImport_Click(object sender, EventArgs e)
        {
            this.ExportExcel();
        }
       

    }
}
