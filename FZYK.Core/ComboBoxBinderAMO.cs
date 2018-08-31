using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Data;
namespace FZYK.Core
{
    /// <summary>
    /// 下拉框绑定类
    /// </summary>
    public partial class ComboBoxBinder
    {
        public static void BindptFastDDName(ToolStripDropDownButton ddb)
        {
            string sqlstr = "SELECT cb.setMemo,cb.value,cb.sort FROM B_CommonForCombox cb WHERE cb.setName='F_ptFastDDName' ORDER BY cb.sort";
            DataTable dt = Com.YKDataClass.getDataTable(sqlstr);
            if (dt != null && dt.Rows.Count > 0)
            {
                ddb.DropDownItems.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string alText = Convert.ToString(row["setMemo"]);
                    string alName = Convert.ToString(row["value"]);

                    ddb.DropDownItems.Add(alText);
                    foreach (ToolStripItem item in ddb.DropDownItems)
                    {
                        if (item.Text == alText)
                        {
                            item.Tag = alName;
                        }
                    }

                }
            }
        }

        public static void BindDeviceWXType(FZYK.WinCtrl.YKComboBox cmb)
        {
            cmb.Items.Clear();
            cmb.Items.Add("");
            cmb.Items.Add("厂内维修");
            cmb.Items.Add("厂外维修");
        }
        public static void BindFYTaskMainState(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("fPTState", typeof(string));
            dt.Columns.Add("sort", typeof(int));
            DataRow r1 = dt.NewRow();
            r1["fPTState"] = "";
            r1["sort"] = 0;
            DataRow r2 = dt.NewRow();
            r2["fPTState"] = "未下单";
            r2["sort"] = 1;
            DataRow r3 = dt.NewRow();
            r3["fPTState"] = "部分下单";
            r3["sort"] = 2;
            DataRow r4 = dt.NewRow();
            r4["fPTState"] = "下单完";
            r4["sort"] = 3;
            DataRow r5 = dt.NewRow();
            r5["fPTState"] = "标识下单完";
            r5["sort"] = 4;
            dt.Rows.Add(r1);
            dt.Rows.Add(r2);
            dt.Rows.Add(r3);
            dt.Rows.Add(r4);
            dt.Rows.Add(r5);
            dt.AcceptChanges();
            cmb.BindData(dt, "fPTState", "fPTState", "sort");
        }
        public static void BindDevicerkdSuppliers(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT DISTINCT rkdSuppliers FROM D_DeviceRuKuDetail ORDER BY rkdSuppliers";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["rkdSuppliers"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "rkdSuppliers", "rkdSuppliers", "");
        }
        public static void BindDevicebxGuZhangMemo(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='D_bxGuZhangMemo'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        public static void BindDeviceJiaoHuoFangShi(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='D_JiaoHuoFangShi'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        public static void BindDeviceJieSuanFangShi(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='D_JieSuanFangShi'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        public static void BindDeviceSheBeiXingZhi(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='D_dSheBeiXingZhi'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 获取设备使用车间
        /// 陈天生 2014年4月11日01:59:31
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDeviceDPlace(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT DISTINCT dUseDepartment FROM D_Device ORDER BY dUseDepartment";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["dUseDepartment"] = "";
            dr["dUseDepartment"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "dUseDepartment", "dUseDepartment", "");
        }

        public static void BindAccountListType(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("alType", typeof(string));
            dt.Columns.Add("sort", typeof(int));
            DataRow r1 = dt.NewRow();
            r1["alType"] = "塔材";
            r1["sort"] = 0;
            DataRow r2 = dt.NewRow();
            r2["alType"] = "补件";
            r2["sort"] = 1;
            dt.Rows.Add(r1);
            dt.Rows.Add(r2);
            dt.AcceptChanges();
            cmb.BindData(dt, "alType", "alType", "sort");
        }

        /// <summary>
        ///  下达试装状态
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindptszState(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='F_ptszState'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmb">控件</param>
        public static void BindMC_GategoryName(FZYK.WinCtrl.YKComboBox cmb, string MS_SortName)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from DM_MeasureCategory  WHERE MS_SortName='" + MS_SortName + "'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["MC_GategoryName"] = "[全部]";
            dr["MC_ID"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            DevCommon.ComboBoxBindHavingBlank(cmb, dt, "MC_GategoryName", "MC_ID");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmb">控件</param>
        public static void BindMS_SortName(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from DM_MeasureSort");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["MS_SortName"] = "[全部]";
            dr["MS_ID"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            DevCommon.ComboBoxBindHavingBlank(cmb, dt, "MS_SortName", "MS_ID");
        }
        /// <summary>
        ///  放样杂项工时工作类型
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFYZXGSType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='F_ZXGSType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定设备类型
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDeviceType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT ddt.dtID, ddt.dtNum, ddt.dtName, ddt.dtParentNum "
                            + " FROM D_DeviceType ddt WHERE ISNULL(ddt.dtValid,1)=1"
                            + " ORDER BY ddt.dtNum";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["dtID"] = 0;
            dr["dtName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "dtName", "dtID", "dtName");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        ///  使用状态
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindRunningState(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='D_DeviceRunningState'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "value", "value", "sort");
        }

        /// <summary>
        /// 绑定设备状态下拉框
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDeviceState(FZYK.WinCtrl.YKComboBox cmb)
        {
            BindCommonForCombox(cmb, "DeviceState");
        }

        /// <summary>
        /// 绑定放样塔型数据段界面的放样类型
        /// 添加时间：2012年7月24日 01:45:58 添加人：AMo
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDrawingsType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT DISTINCT value,sort FROM B_CommonForCombox WHERE setName='F_DrawingsType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定放样塔型数据段界面的塔类型
        /// 添加时间：2012年7月24日 01:45:58 添加人：AMo
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindTowerType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT DISTINCT value,sort FROM B_CommonForCombox WHERE setName='F_TowerType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定放样塔型数据段界面的塔规格
        /// 添加时间：2012年7月24日 01:45:58 添加人：AMo
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindTowerSpec(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT DISTINCT value,sort FROM B_CommonForCombox WHERE setName='F_TowerSpec'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }

        /// <summary>
        /// 绑定螺栓塔型数据中垫片损耗计算方式下拉框
        /// 添加：天生  2012年9月18日 16:33:49
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDPLossWay(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "  SELECT bcfc.setMemo, bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='F_DPLossWay'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "setMemo", "value", "sort");
        }
        /// <summary>
        /// 绑定螺栓塔型数据中防松罩损耗计算方式下拉框
        /// 添加：天生  2012年9月18日 16:33:49
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFSZLossWay(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "  SELECT bcfc.setMemo, bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='F_FSZLossWay'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "setMemo", "value", "sort");
        }

        /// <summary>
        /// 绑定下达打印格式
        /// 添加 天生 2012年10月31日 00:33:12
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProduceTaskPrint(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "  SELECT bcfc.setMemo, bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='ProduceTaskPrint'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["setMemo"] = "[选择格式]";
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "setMemo", "value", "sort");
        }
        /// <summary>
        /// 绑定放样班分部门下拉框
        /// 添加 天生 2012年10月31日 00:33:12
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFYDepartment(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='F_FYDepartment'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定生产任务下达的单据类型
        /// 添加 天生 2013年1月11日 11:39:16
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProduceTaskptType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='F_ptType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
    }
}
