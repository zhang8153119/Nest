using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using FZYK.Com;
namespace FZYK.Core
{
    /// <summary>
    /// 提供者
    /// </summary>
    public partial class Provider
    {



        #region  鄢国平
        /// <summary>
        /// 重新弹出提醒框
        /// 5-19
        /// 修改 2013年7月3日 16:03:38
        /// </summary>
        public static void ShowAlert(string Type)
        {
            //for (int i = 0; i < Application.OpenForms.Count; i++)
            //{
            //    if (Application.OpenForms[i].Name == "AlertForm")
            //    {
            //        Form frm = Application.OpenForms[i];
            //        frm.Close();
            //    }
            //}
            if (Type == "Approve")
            {
                string message = "";
                message = YKCommonSql.GetNewestMessage(Com.UserInfo.eID);
                if (message != "")
                {
                    Form frm = Core.Provider.ShowAlertForm(message);
                }
            }
            else if (Type == "Notify")
            {
                FZYK.Com.UserInfo.dsMessage = YKCommonSql.GetNewNotify(Com.UserInfo.eID);
                if (Com.UserInfo.dsMessage.Tables[0].Rows.Count > 0)
                {
                    string Content = "Notify#" + Com.UserInfo.dsMessage.Tables[0].Rows.Count.ToString() + "#" + Com.UserInfo.dsMessage.Tables[0].Rows[0]["nbID"].ToString() + "#" + Com.UserInfo.dsMessage.Tables[0].Rows[0]["Writer"].ToString() + " " + Com.UserInfo.dsMessage.Tables[0].Rows[0]["WriteDate"].ToString() + "#" + Com.UserInfo.dsMessage.Tables[0].Rows[0]["nbTitle"].ToString() + "";
                    Form frm = Core.Provider.ShowAlertForm(Content);
                }
            }
        }
        /// <summary>
        /// 销售合同选择框
        /// 鄢国平 2014年6月10日 11:23:42
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPaperCompact()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPaperCompactSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 补件选择框
        /// 鄢国平 2013年5月30日 22:50:26
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPatchSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 补件选择框
        /// 鄢国平 2013年5月30日 22:50:26
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPatchSelect(string sql,string tag)
        {
            try
            {
                object[] args = new object[2];
                args[0] = sql;
                args[0] = tag;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 得到客户选择框
        /// 鄢国平 2013年5月29日 19:50:31
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetCustomerSelect(string ParentNum)
        {
            try
            {
                object[] args = new object[1];
                args[0] = ParentNum;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmCustomerSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 进入补件Single界面
        /// 鄢国平 2013年2月1日 11:22:16
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetPatchSingle(DataTable dt)
        {
            try
            {
                object[] args = new object[1];
                args[0] = dt;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchSingle", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        public static BaseSelectDialog GetTypeNameOrderTaskSelect(string where)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[1];
                args[0] = where;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTypeNameOrderTaskSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到任务单塔型选择框
        /// 添加：鄢国平 2013年4月3日 16:07:22
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTypeNameOrderTaskSelect(int cID, string operate)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[2];
                args[0] = cID;
                args[1] = operate;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTypeNameOrderTaskSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        
        /// <summary>
        /// 得到补件申请单塔型明细
        /// 添加：鄢国平 2013年4月3日 16:07:22
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPatchDetail(string operate)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[1];
                args[0] = operate;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到补件申请单塔型明细
        /// 添加：鄢国平 2013年4月3日 16:07:22
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPatchDetail(string operate, int cCustomerID)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[2];
                args[0] = operate;
                args[1] = cCustomerID;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 图纸提料塔型选择框
        /// 鄢国平 2013年4月3日 21:35:39
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetDrawingsTiLaioSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmDrawingsTiLaioSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 成品出库选择框
        /// 鄢国平 2013年1月12日 16:32:10
        /// 修改 2013年2月25日 16:48:59
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPackOutSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.ProduceFinish.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.ProduceFinish.FrmPackOutSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 成品出库选择框 带参数
        /// 鄢国平 2014年6月5日 20:46:49
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPackOutSelect(string oper)
        {
            try
            {
                object[] args = new object[1];
                args[0] = oper;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.ProduceFinish.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.ProduceFinish.FrmPackOutSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 进入订单目录Single界面
        /// 鄢国平 2013年2月1日 11:22:16
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetCompactSingle(int cID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmCompactSingle", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 运费登记选择框
        /// 鄢国平 2013年1月30日 16:27:24
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetShipCheckSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Money.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Money.FrmShipCheckSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到OA提醒框
        ///  添加：鄢国平 2013年1月28日 09:37:33
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetLimitDate(DataSet ds)
        {
            try
            {
                object[] args = new object[1];
                args[0] = ds;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.OA.FrmLimitDateMain", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到OA提醒框
        ///  添加：鄢国平 2013年1月28日 09:37:33
        /// </summary>
        /// <returns></returns>
        public static DockContent GetHomePage()
        {
            try
            {
                object[] args = new object[0];
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                DockContent form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmHomePage") as DockContent;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 包装清单选择框
        /// 鄢国平 2013年1月22日 22:06:47
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPackSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.ProduceFinish.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.ProduceFinish.FrmPackSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 档案文件选择框
        /// 鄢国平 2013年1月18日 10:07:10
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetDocumentSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OF.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.OF.FrmDocumentSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        
        /// <summary>
        /// 得到补件申请单塔型明细
        /// 添加：鄢国平 2012-11-26
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPatchDetail()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchDetailSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到公告信息框
        /// 添加：鄢国平 2012-11-26
        /// </summary>
        /// <returns></returns>
        public static DockContent GetMessage()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                DockContent form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmMessageMain") as DockContent;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到公告信息框
        /// 添加：鄢国平 2012-11-26
        /// </summary>
        /// <returns></returns>
        public static DockContent GetMessage(int nbID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = nbID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                DockContent form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmMessageMain", false, BindingFlags.CreateInstance, null, args, null, null) as DockContent;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到公告发送框
        /// 添加：鄢国平 2012-11-26
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetNotifyBoard(string nbTitle, string nbContent, string nbTurn, string nbFormName)
        {
            try
            {
                object[] args = new object[4];
                args[0] = nbTitle;
                args[1] = nbContent;
                args[2] = nbTurn;
                args[3] = nbFormName;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmNotifyBoardSingle", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到得到接收情况
        /// 添加：鄢国平 2012-11-26
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetNotifyBoardSign(string nbTurn, string nbFormName)
        {
            try
            {
                object[] args = new object[2];
                args[0] = nbTurn;
                args[1] = nbFormName;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmNotifyBoardSign", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取员工选择框
        /// 添加：鄢国平
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetEmployeeSelect()
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog bs = assembly.CreateInstance("FZYK.WinUI.OA.FrmEmployeeSelect") as BaseSelectDialog;
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取员工选择框
        /// 添加：鄢国平
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetEmployeeSelect(string eName)
        {
            try
            {
                object[] args = new object[1];
                args[0] = eName;
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog bs = assembly.CreateInstance("FZYK.WinUI.OA.FrmEmployeeSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取员工选择框
        /// 添加：鄢国平 2014年6月9日 09:25:01
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetEmployeeSelect(string sqlWhere,string tag)
        {
            try
            {
                object[] args = new object[2];
                args[0] = sqlWhere;
                args[1] = tag;
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog bs = assembly.CreateInstance("FZYK.WinUI.OA.FrmEmployeeSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;

                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得部门选择框
        /// 添加：鄢国平
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetDeptSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmDeptSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///  客户名称选择框
        ///  添加：鄢国平
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetCustomerSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmCustomerSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到合同选择框
        /// 添加：鄢国平
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetCompactSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmCompactSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到合同明细选择框
        ///  添加：鄢国平
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetCompaceDetailSelect(int cid)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cid;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmCompaceDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到合同选择框
        /// 鄢国平 2012-08-09 根据客户ID过滤合同
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetCompactSelect(int cID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmCompactSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        
        /// <summary>
        /// 得到合同选择框
        /// 鄢国平 2012-12-24 根据操作过滤数据
        /// </summary>
        /// <param name="opera">Sect为过滤已生成销售杆塔信息合同</param>
        /// <returns></returns>
        public static BaseSelectDialog GetCompactSelect(string opera)
        {
            try
            {
                object[] args = new object[1];
                args[0] = opera;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmCompactSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        ///鄢国平 2012-08-15
        /// <summary>
        /// 得到任务单明细选择框
        /// </summary>
        /// <param name="tdid">塔形TDID，没有的话传0</param>
        /// <returns></returns>
        public static BaseSelectDialog GetOrderTaskDetailSelect(int tdid)
        {
            try
            {
                object[] args = new object[1];
                args[0] = tdid;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmOrderTaskDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        ///鄢国平 2012-08-15
        /// <summary>
        /// 得到任务单明细选择框
        /// </summary>
        /// <param name="tdid">塔形TDID，没有的话传0</param>
        /// <returns></returns>
        public static BaseSelectDialog GetOrderTaskDetailSelect(int tdid,string oper,string sql)
        {
            try
            {
                object[] args = new object[3];
                args[0] = tdid;
                args[1] = oper;
                args[2] = sql;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmOrderTaskDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到塔形选择框
        /// 鄢国平 2012-08-03
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTypeNameSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTypeNameSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        public static BaseSelectDialog GetTypeNameSelect(string where)
        {
            try
            {
                object[] args = new object[1];
                args[0] = where;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTypeNameSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 得到塔形选择框 带参数
        /// 鄢国平 2012年12月24日 16:35:42
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTypeNameSelect(int flag)
        {
            try
            {
                object[] args = new object[1];
                args[0] = flag;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTypeNameSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到塔形选择框
        /// 鄢国平 2012-08-03
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTypeNameOrderTaskSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTypeNameOrderTaskSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 发运通知单选择框
        /// 鄢国平 2012-09-10
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetShippingNoticeSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmShippingNoticeSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 发运选择框
        /// 鄢国平 2014年6月17日 21:17:58
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetShippingNoticeSelect(string tag)
        {
            try
            {
                object[] args = new object[1];
                args[0] = tag;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmShippingNoticeSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 包库存选择框
        /// 鄢国平 2012-09-10
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetPackStoreSelect(int cID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.ProduceFinish.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.ProduceFinish.FrmPackStoreSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 日志查询
        /// 鄢国平 2012-10-9
        /// </summary>
        /// <returns></returns>
        public static BaseLog GetLogMain(string logClassName, string logKeyName, string LogKeyValue)
        {
            try
            {
                object[] args = new object[3];
                args[0] = logClassName;
                args[1] = logKeyName;
                args[2] = LogKeyValue;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                BaseLog form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmLogMain", false, BindingFlags.CreateInstance, null, args, null, null) as BaseLog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 修改密码
        /// 鄢国平 2012年10月19日 08:58:52
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetUpdatePasswordForm()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.SystemManage.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.SystemManage.FrmUpdatePassword") as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 绑定销售付款方式 鄢国平2012-10-26
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindSalePayWay(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "select * from B_SalePayWay where pwvalid=1 order by pwifDefault desc";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pwName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pwName", "pwName", "pwName");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 得到杆塔明细选择框
        ///  添加：鄢国平
        ///  2013年8月28日 01:46:56
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetSectConfigDetailSelect(int cID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmSectConfiDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 补件统计
        /// 鄢国平 2013年8月28日 01:51:13
        /// </summary>
        /// <returns></returns>
        public static BaseSearch GetPatchTotal()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSearch form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmPatchTotal") as BaseSearch;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 银行基础设置选择框
        /// 鄢国平 2014年4月4日 11:23:45
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetBank()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmBankSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到运输招标结果选择框
        /// 添加：鄢国平 2013年7月10日 15:50:08
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTransportBid(string unit)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[1];
                args[0] = unit;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTransportBidSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到运输合同选择框
        /// 添加：鄢国平 2013年7月10日 15:50:08
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTransportCompact(string oper)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[1];
                args[0] = oper;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmTransportCompactSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到员工因公借款单选择框
        /// 添加：鄢国平 2014年5月3日 13:37:01
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetEmployeeLoan(string oper)
        {
            try
            {
                //通过反射得到实例

                object[] args = new object[1];
                args[0] = oper;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.OA.FrmEmployeeLoanSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到货车选择框
        /// 添加：鄢国平 2014年5月28日 15:43:43
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTruckSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.EnterpriseBusiness.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.EnterpriseBusiness.FrmTruckSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到货车请车单选择框
        /// 添加：鄢国平 2014年6月4日 10:51:24
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetTruckApplySelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.EnterpriseBusiness.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.EnterpriseBusiness.FrmTruckApplySelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到费用选择框
        /// 鄢国平 2014年6月14日 16:29:02
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetHospitalitySelect(string sql)
        {
            try
            {
                object[] args = new object[1];
                args[0] = sql;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.EnterpriseBusiness.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.EnterpriseBusiness.FrmHospitalitySelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 个人信息
        /// 鄢国平 2014年6月16日 22:14:47
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetLoginPerfectSingle(int eID)
        {
            try
            {
                object[] args = new object[3];
                args[0] = null;
                args[1] = null;
                args[2] = eID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.OA.FrmLoginPerfectSingle", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 住宿申请表
        /// 鄢国平 2014年7月2日 14:39:57
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetStayApplySelect(string oper)
        {
            try
            {
                object[] args = new object[1];
                args[0] = oper;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.OA.FrmStayApplySelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到工构件任务单明细选择框
        ///  添加：鄢国平
        ///  2015年1月13日 20:08:29
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetOrderTaskJinJianDetailSelect(string strWhere, string tag, string tag1)
        {
            try
            {
                object[] args = new object[3];
                args[0] = strWhere;
                args[1] = tag;
                args[2] = tag1;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Sale.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Sale.FrmOrderTaskJinJianDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 得到部门岗位选择框
        /// 添加：鄢国平 2015年3月26日 15:42:00
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetEmployeePostitleSelect(string sql)
        {
            try
            {
                //通过反射得到实例

                object[] args = new object[1];
                args[0] = sql;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.OA.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.OA.FrmEmployeePostitleSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion



    }
}
