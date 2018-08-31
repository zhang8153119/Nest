using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace FZYK.Core
{
    /// <summary>
    /// 提供者
    /// </summary>
    public partial class Provider
    {
        public static BaseSelectDialog GetSectConfigDetailSelect(string where)
        {
            try
            {
                object[] args = new object[1];
                args[0] = where;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmSectConfigDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 设备发票
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static BaseSelectDialog GetDeviceFaPiaoSelect(string where)
        {
            try
            {
                object[] args = new object[1];
                args[0] = where;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Device.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Device.FrmDeviceFaPiaoSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        public static BaseSearch GetTeZhongSheBeiMain(string flag)
        {
            try
            {
                object[] args = new object[1];
                args[0] = flag;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Device.dll");
                BaseSearch form = _assembly.CreateInstance("FZYK.WinUI.Device.FrmTeZhongSheBeiMain", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSearch;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 按订单ID汇总杆塔图纸重量
        /// </summary>
        /// <param name="cID">cID</param>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetDrawingsDataScdSummaryByCid(int cID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = cID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                System.Windows.Forms.Form form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmDrawingsDataScdSummaryByCid", false, BindingFlags.CreateInstance, null, args, null, null) as System.Windows.Forms.Form;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        public static BaseSearch GetMeasureDeviceMain(string flag)
        {
            try
            {
                object[] args = new object[1];
                args[0] = flag;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.DeviceMeasure.dll");
                BaseSearch form = _assembly.CreateInstance("FZYK.WinUI.DeviceMeasure.FrmMeasureDeviceMain", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSearch;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取电焊件选择框
        /// </summary>
        /// <param name="tdID">塔型ID</param>
        /// <returns></returns>
        public static BaseSelectDialog GetWeldingPartSelect(int fID, string mdPartsID)
        {
            try
            {
                object[] args = new object[2];
                args[0] = fID;
                args[1] = mdPartsID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmWeldingPartSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取设备台帐选择框
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static BaseSelectDialog GetDeviceSelect(string where)
        {
            try
            {
                object[] args = new object[1];
                args[0] = where;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.Device.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.Device.FrmDeviceSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获得toolbox实例。
        /// </summary>
        /// <returns></returns>
        public static BaseToolBox GetToolBox()
        {
            //也可以将FrmToolBoxXML封装于dll中，此处用反射实例化。
            return new FrmToolBoxXML() as BaseToolBox;
        }

        /// <summary>
        /// 获得设置可打包后放样修改记录的窗体
        /// </summary>
        /// <param name="fID">fID</param>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetMaterialModifyAfterCanPack(int fID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = fID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                System.Windows.Forms.Form form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmMaterialModifyAfterCanPack", false, BindingFlags.CreateInstance, null, args, null, null) as System.Windows.Forms.Form;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 获取生产任务下达singleForm实例
        /// </summary>
        /// <param name="tdID">塔型ID</param>
        /// <returns></returns>
        public static BaseSingle GetProduceTaskSingle(int ptID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = ptID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmProduceTaskSingle", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 放样的塔型选择框，无法跟到任务单
        /// 添加：AMo      时间：2012年9月7日 14:50:27
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetFYTaskTypeSelect()
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog bs = assembly.CreateInstance("FZYK.WinUI.FangYang.FrmFYTaskTypeSelect") as BaseSelectDialog;
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 放样的塔型选择框，无法跟到任务单
        /// 添加：AMo      时间：2013年7月29日 09:10:26
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetFYTaskTypeSelect(string flag)
        {
            try
            {
                object[] args = new object[1];
                args[0] = flag;
                Assembly assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog bs = assembly.CreateInstance("FZYK.WinUI.FangYang.FrmFYTaskTypeSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取放样塔型数据选择框
        /// </summary>
        /// <param name="tdID">fID</param>
        /// <returns></returns>
        public static BaseSelectDialog GetMaterialDetailSelect(int fID)
        {
            try
            {
                object[] args = new object[1];
                args[0] = fID;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmMaterialDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取放样塔型数据选择框
        /// </summary>
        /// <param name="fID">fID</param>
        /// <param name="where">必须加AND 字段前缀是fmsdv</param>
        /// <returns></returns>
        public static BaseSelectDialog GetMaterialDetailSelect(int fID, string where)
        {
            try
            {
                object[] args = new object[2];
                args[0] = fID;
                args[1] = where;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmMaterialDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 放样杆塔选择框，已关联任务单。带参数，参数可为空。
        /// 添加：AMo      时间：2012年9月7日 14:50:27
        /// </summary>
        /// <param name="fID">放样塔型ID fID</param>
        /// <param name="from">来自什么界面弹出的选择框 包装写“pack” 下达写“producetask”</param>
        /// <returns></returns>
        public static BaseSelectDialog GetSectConfigDetailSelect(int fID, string from)
        {
            try
            {
                object[] args = new object[2];
                args[0] = fID;
                args[1] = from;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmSectConfigDetailSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        public static BaseSelectDialog GetSectConfigDetailSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmSectConfigDetailSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 生产任务下达选择框
        /// 添加：天生  2012年9月14日 11:29:54
        /// </summary>
        /// <returns></returns>
        public static BaseSelectDialog GetProduceTaskSelect()
        {
            try
            {
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmProduceTaskSelect") as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 生产任务下达选择框
        /// 添加：天生 2013年1月8日 15:42:43
        /// </summary>
        /// <param name="Parent">传入对应界面的参数</param>
        /// <returns></returns>
        public static BaseSelectDialog GetProduceTaskSelect(string Parent, string fTypeName)
        {
            try
            {
                object[] args = new object[2];
                args[0] = Parent;
                args[1] = fTypeName;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmProduceTaskSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 生产任务下达选择框
        /// </summary>
        /// <param name="sqlWhere">自定义条件，必须 AND 开头</param>
        /// <param name="flag">传入空值</param>
        /// <param name="flag2">传入空值</param>
        /// <returns></returns>
        public static BaseSelectDialog GetProduceTaskSelect(string sqlWhere, string flag, string flag2)
        {
            try
            {
                object[] args = new object[3];
                args[0] = sqlWhere;
                args[1] = flag;
                args[2] = flag2;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmProduceTaskSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 生产任务下达选择框
        /// 添加：天生 2013年1月8日 15:42:43
        /// </summary>
        /// <param name="Parent">传入对应界面的参数</param>
        /// <returns></returns>
        public static BaseSelectDialog GetMaterialDetailSelect(string Parent)
        {
            try
            {
                object[] args = new object[1];
                args[0] = Parent;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSelectDialog form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmProduceTaskSelect", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSelectDialog;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取腿长配段基础设置界面
        /// 添加：天生  2012年12月13日 09:39:47
        /// </summary>
        /// <returns></returns>
        public static BaseSingle GetScdLegSectSetting(int cID, string project)
        {
            try
            {
                object[] args = new object[2];
                args[0] = cID;
                args[1] = project;
                //通过反射得到实例
                Assembly _assembly = Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FZYK.WinUI.FangYang.dll");
                BaseSingle form = _assembly.CreateInstance("FZYK.WinUI.FangYang.FrmScdLegSectSetting", false, BindingFlags.CreateInstance, null, args, null, null) as BaseSingle;
                return form;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
