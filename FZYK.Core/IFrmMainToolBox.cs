using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
namespace FZYK.Core
{
    /// <summary>
    /// 主窗体接口
    /// </summary>
    public interface IFrmMainToolBox
    {
        DockPanel IDockPanel { get; set; }
        /// <summary>
        /// 使用反射机制，动态创建窗体。方法为public，在ToolBox窗体那边被调用
        /// </summary>
        /// <param name="frmName">窗体的类名</param>
        /// <param name="frmText">窗体的Text属性值</param>
        /// <param name="args">所要传递给窗体构造函数的参数数组,参数数组在数量、顺序和类型方面必须与要调用的构造函数的参数匹配。</param>
        void ShowChildrenFormWithArgs(string frmName, string frmText, object[] args, string frmTitle,string className);
         /// <summary>
        /// 使用反射机制，动态创建窗体。方法为public，在ToolBox窗体那边被调用
        /// </summary>
        /// <param name="frmName">窗体的类名</param>
        /// <param name="frmText">窗体的Text属性值</param>
        /// <param name="args">所要传递给窗体构造函数的参数数组,参数数组在数量、顺序和类型方面必须与要调用的构造函数的参数匹配。</param>
        void ShowChildrenFormWithNotify(string frmName, string frmText, object[] args, string frmTitle, string sqlwhere, string className);
        /// <summary>
        /// 使用反射机制，动态创建窗体。方法为public，在ToolBox窗体那边被调用
        /// </summary>
        /// <param name="frmName">窗体的类名</param>
        /// <param name="frmText">窗体的Text属性值</param>
        void ShowChildrenForm(string frmName, string frmText, string className);

        /// <summary>
        /// 使用反射机制，动态创建窗体。方法为public，在ToolBox窗体那边被调用
        /// </summary>
        /// <param name="frmName">窗体的类名</param>
        /// <param name="frmText">窗体的Text属性值</param>
        /// <param name="args">所要传递给窗体构造函数的参数数组,参数数组在数量、顺序和类型方面必须与要调用的构造函数的参数匹配。</param>
        void ShowSingleFormWithArgs(string frmName, string frmText, object[] args, string frmTitle, string className);

        /// <summary>
        /// 使用反射机制，动态创建窗体。方法为public，在ToolBox窗体那边被调用
        /// </summary>
        /// <param name="frmName">窗体的类名</param>
        /// <param name="frmText">窗体的Text属性值</param>
        void ShowSingleForm(string frmName, string frmText, string className);
    }
}
