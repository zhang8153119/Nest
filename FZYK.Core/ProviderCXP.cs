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
    public partial class Provider
    {
        /// <summary>
        /// 生产任务下达选择框
        /// 添加：天生 2013年1月8日 15:42:43
        /// </summary>
        /// <param name="Parent">传入对应界面的参数</param>
        /// <returns></returns>
        public static BaseSelectDialog GetProduceTaskSelect(string Parent)
        {
            try
            {
                object[] args = new object[2];
                args[0] = Parent;
                args[1] = "";
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmProduceTaskSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        #region CXP
        /// <summary>
        ///  长安工资结算CXP 2013-05-27
        /// </summary> 
        /// <returns></returns>
        public static System.Windows.Forms.Form GetFrmCAWage(string title, string _gyid, string _pjtime, string _tableName)
        {
            try
            {
                object[] args = new object[4];
                args[0] = title;
                args[1] = _gyid;
                args[2] = _pjtime;
                args[3] = _tableName;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.GongZi.dll");
                System.Windows.Forms.Form form = _assembly.CreateInstance("FZYK.WinUI.GongZi.FrmCAWage", false, BindingFlags.CreateInstance, null, args, null, null) as System.Windows.Forms.Form;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// CXP 2013-4-8点塔选择框
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetDiantaSelect()
        {
            try
            {
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.ProducePlan.dll");
                BaseSelectDialog frm = _assembly.CreateInstance("FZYK.WinUI.ProducePlan.FrmSelectDiantaMain") as BaseSelectDialog;
                return frm;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取小燕的自动采集程序
        /// </summary>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetZDCJProcess(string pjMaker, string dcNum, bool ifBan)
        {
            try
            {
                object[] args = new object[3];
                args[0] = pjMaker;
                args[1] = dcNum;
                args[2] = ifBan;
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.UI.FaYin.dll");
                System.Windows.Forms.Form bs = assembly.CreateInstance("FZYK_FY.FrmLogin", false, BindingFlags.CreateInstance, null, args, null, null) as System.Windows.Forms.Form;
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 弹开数据采集界面 
        /// 添加：CXP
        /// </summary>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetJijianByDiaoDu(int pgID, string ttime)
        {
            try
            {
                object[] args = new object[2];
                args[0] = pgID;
                args[1] = ttime;
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                System.Windows.Forms.Form bs = assembly.CreateInstance("FZYK.WinUI.Produce.FrmJijianByDiaoDu", false, BindingFlags.CreateInstance, null, args, null, null) as System.Windows.Forms.Form;
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CXP 2013-1-11派工设备多选择框
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetProcessesDeviceSelectMulti(string wsName, string pid)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[2];
                args[0] = wsName;
                args[1] = pid;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog frm = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmProcessesDeviceSelectMulti", true, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return frm;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// CXP 2013-1-11派工设备多选择框
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetProcessesGroupSelectMulti(string wsName, string pid)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[2];
                args[0] = wsName;
                args[1] = pid;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog frm = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmProcessesGroupSelectMulti", true, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return frm;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// CXP 2012-11-26派工设备选择框
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetProcessesDeviceSelect(string wsName, string pid)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[2];
                args[0] = wsName;
                args[1] = pid;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSelectDialog frm = _assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmProcessesDeviceSelect", true, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return frm;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 弹开排班员工界面(cxp2012-8-3)
        /// 添加：CXP
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetProcessesGroupEmployee(string datetime, int pgid)
        {
            try
            {
                object[] args = new object[2];
                args[0] = datetime;
                args[1] = pgid;
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.BaseSet.dll");
                BaseSingle bs = assembly.CreateInstance("FZYK.WinUI.BaseSet.FrmProcessesGroupEmployee", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///CXP 2012-10-08
        /// <summary>
        /// 得到加工进度界面
        /// </summary>
        /// <param name="ptid">下达单ptID</param>
        /// <returns></returns>
        public static BaseSingle GetProcessesDetailByPTID(int ptID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = ptID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.Produce.StatProduce.FrmProcessesDetail", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        ///CXP 2012-10-17
        /// <summary>
        /// 得到统计日产量报表
        /// </summary>
        /// <param name="ptid">下达单ptID</param>
        /// <returns></returns>
        public static BaseSearch GetStatMonthForCompanyDetailByPTID(int ptID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = ptID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                BaseSearch form = _assembly.CreateInstance("FZYK.WinUI.Produce.StatProduce.FrmStatMonthForCompanyDetail", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSearch;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        public static Form GetFrmTwoDGVStat(string otNum, string typeName)
        {
            try
            {
                //通过反射得到实例
                object[] args = new object[2];
                args[0] = otNum;
                args[1] = typeName;
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                Form frm = _assembly.CreateInstance("FZYK.WinUI.Produce.StatProduce.FrmTwoDGVStat", true, BindingFlags.CreateInstance, null, args, null, null) as Form;
                return frm;
            }
            catch (Exception ex) { throw ex; }
        }


        ///CXP 2013-05-13
        /// <summary>
        /// 通过合同编号得到TC生产任务下达进度跟踪表
        /// </summary>
        /// <param name="ptid">下达单ptID</param>
        /// <returns></returns>
        public static BaseSearch GetFrmTCProduceTaskStat(string cNum)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cNum;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                BaseSearch form = _assembly.CreateInstance("FZYK.WinUI.Produce.StatProduce.FrmTCProduceTaskStat", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSearch;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        ///CXP 2013-05-08
        /// <summary>
        /// 得到TC生产任务下达进度跟踪表
        /// </summary>
        /// <param name="ptid">下达单ptID</param>
        /// <returns></returns>
        public static BaseSearch GetFrmTCProduceTaskStat(int ptID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = ptID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                BaseSearch form = _assembly.CreateInstance("FZYK.WinUI.Produce.StatProduce.FrmTCProduceTaskStat", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSearch;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        ///  选择调度设备方案CXP 2013-05-28
        /// </summary> 
        /// <returns></returns>
        public static BaseSelectDialog GetFrmProcessesPlan(string DDName, string paType, bool ifSelectOne)
        {
            try
            {
                object[] args = new object[3];
                args[0] = DDName;
                args[1] = paType;
                args[2] = ifSelectOne;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.ProducePlan.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.ProducePlan.FrmProcessesPlan", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        public static BaseSingle GetProcessesDetailByPTIDWhere(int ptID, string dsID)
        {
            try
            {
                object[] args = new object[2];
                args[0] = ptID;
                args[1] = dsID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Produce.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.Produce.StatProduce.FrmProcessesDetail", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion



    } 
}
