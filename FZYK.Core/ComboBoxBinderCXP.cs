using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FZYK.Core
{
   public   partial class ComboBoxBinder
    {
        /// <summary>
        /// 绑定补件类型CXP2013-03-22
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindPatchTypeForSearch(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sql = "select distinct value,sort from B_CommonForCombox where setName='PatchType' and valid=1";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sql);
            DataRow dr1 = dt.NewRow();
            dr1["value"] = "非补件";
            dr1["sort"] = 0;
            dt.Rows.InsertAt(dr1, 0);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = -1;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定CA试组产品名称 ,供批量刷条码时的选择  2013-4-15
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindCASZGongZiBase(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT * FROM GZ_CASZGongZiBase  ORDER BY szID";
            DataTable dt = new DataTable();
            dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr1 = dt.NewRow();
            dr1["szName"] = "";
            dt.Rows.InsertAt(dr1, 0);
            cmb.BindData(dt, "szName", "szName", "szID");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 根据工序绑定班组下拉框CXP2013-01-19
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="ifTou"></param>
        /// <param name="sqlWhere"></param>
        public static void BindProcessesGroup(FZYK.WinCtrl.YKComboBox cmb, int ifTou, string sqlWhere)
        {
            string sqlstr = " SELECT * FROM B_ProcessesGroupView where pgValid=1 ";
            if (ifTou != 0)
                sqlstr += " and ifTou=" + ifTou.ToString();
            if (!string.IsNullOrEmpty(sqlWhere))
                sqlstr += sqlWhere;
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pgID"] = 0;
            dr["pgName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pgName", "pgID", "");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 根据key找B_CommonForCombox表中值绑定下拉框 
        /// 修改时间：2012年7月25日 09:06:11  修改人：CXP 修改内容：增加sort排序
        /// 修改时间：2012年9月2日 14:06:11  修改人：CXP 修改内容：修改sort排序
        /// </summary>
        /// <param name="cmb">下拉框</param>
        /// <param name="key">检索关键字</param>
        public static void BindCommonForCombox(FZYK.WinCtrl.YKComboBox cmb, string key)
        {
            string sqlstr = "SELECT cfc.value,sort  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='" + key + "' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 绑定班组中的工资结算方式
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindpgWageType(FZYK.WinCtrl.YKComboBox cmb)
        {
            BindCommonForCombox(cmb, "pWageType");
        }

        /// <summary>
        /// 绑定工艺班组中员工的岗位职位
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDeviceEmpJob(FZYK.WinCtrl.YKComboBox cmb)
        {
            BindCommonForCombox(cmb, "DeviceEmployeeJob");
        }

        /// <summary>
        /// 绑定分公司
        /// 修改时间：2012年9月2日 14:06:11  修改人：CXP 修改内容：如只一个分公司默认选中这个项
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindCompany(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = " SELECT DISTINCT wsCompany FROM B_WorkShop ";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["wsCompany"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "wsCompany", "wsCompany", "");
            cmb.SelectedIndex = 0;
            if (dt.Rows.Count <= 2)
                cmb.SelectedIndex = 1;
        }
        /// <summary>
        /// 根据车间like 绑定工序
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProcesses(FZYK.WinCtrl.YKComboBox cmb, string wsName)
        {
            string sqlstr = " SELECT pID,pName,ifTou,ifHole FROM B_Processesview where 1=1";
            if (!string.IsNullOrEmpty(wsName))
                sqlstr += " and wsName like '" + wsName + "%'";
            sqlstr += " order by pSort,pID";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pName"] = "";
            dr["pID"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pName", "pID", "");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// CXP2012-12-26根据车间like 绑定工序
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProcesses(FZYK.WinCtrl.YKComboBox cmb, string wsName, string sqlWhere)
        {
            string sqlstr = " SELECT pID,pName,ifTou,ifHole FROM B_Processesview where 1=1";
            if (!string.IsNullOrEmpty(sqlWhere))
                sqlstr += sqlWhere;
            if (!string.IsNullOrEmpty(wsName))
                sqlstr += " and wsName like '" + wsName + "%'";
            sqlstr += " order by pSort,pID";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pName"] = "";
            dr["pID"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pName", "pID", "");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定设备班组的分类别下拉框(从已有的数据中Distinct)
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="wsName"></param>
        public static void BindProcessesDevicePDType(FZYK.WinCtrl.YKComboBox cmb, string wsName)
        {
            string sqlstr = " SELECT DISTINCT pdType FROM B_ProcessesDeviceview where 1=1";
            if (!string.IsNullOrEmpty(wsName))
                sqlstr += " and wsName like '" + wsName + "%'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pdType"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pdType", "pdType", "");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 根据setName获取同类别的setvalue返回DT 2012-08-29 CXP
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        public static DataTable getCommonForComboxSetValue(string setName)
        {
            string sqlStr = "SELECT * FROM B_CommonForCombox where setName='" + setName + "'  ORDER BY Sort";
            return Com.YKDataClass.getDataTable(sqlStr);
        }

        /// <summary>
        /// 根据工序绑定设备班组下拉框CXP2012-09-04
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="pID"></param>
        public static void BindProcessesDevice(FZYK.WinCtrl.YKComboBox cmb, string pID)
        {
            string sqlstr = " SELECT * FROM B_ProcessesDevice where pdValid=1";
            if (!string.IsNullOrEmpty(pID))
                sqlstr += " and pID ='" + pID + "' ";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pdID"] = 0;
            dr["pdName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pdName", "pdID", "");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定车间(CXP2012-09-15 改ValueMemer为wsID)
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindWorkShop(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = " SELECT wsID,wsName FROM B_WorkShop ";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["wsName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "wsName", "wsID", "");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 根据分公司绑定车间(CXP2012-09-15 改ValueMemer为wsID)
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindWorkShop(FZYK.WinCtrl.YKComboBox cmb, string Company)
        {
            string sqlstr = " SELECT wsID,wsName FROM B_WorkShop ";
            if (!Company.Equals(""))
                sqlstr += " where wsCompany='" + Company + "'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["wsName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "wsName", "wsID", "");
            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 获取生产起始日期及终止日期,flag=0获取起始，flag=1获取终止
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public static DateTime getProduceFromOrTo(int flag)
        {
            int aa;
            DateTime temp = DateTime.Now;
            aa = Convert.ToInt32(Com.UserInfo.configSys["ProduceFrom"]);
            if (temp.Day < aa)
                temp = Convert.ToDateTime(temp.AddMonths(-1).Year + "-" + temp.AddMonths(-1).Month + "-" + aa);
            else
                temp = Convert.ToDateTime(temp.Year + "-" + temp.Month + "-" + aa);
            if (flag == 1)
            {
                temp = temp.AddMonths(1).AddDays(-1);
            }
            return temp;
        }
        /// <summary>
        /// 根据工序绑定设备班组下拉框CXP2012-09-24
        /// CXP2012-10-12
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="pID"></param>
        public static void BindProcessesDeviceLike(FZYK.WinCtrl.YKComboBox cmb, string pID)
        {
            string sqlstr = " DECLARE @ifTou INT,@ifHole INT;"
        + " select @ifTou=isnull(ifTou,0),@ifHole=isnull(ifHole,0) FROM B_Processes WHERE pID='" + pID + "';"
        + " SELECT * FROM B_ProcessesDeviceView where pdValid=1 and isnull(ifTou,0)=@ifTou and isnull(ifHole,0)=@ifHole;";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pdID"] = 0;
            dr["pdName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pdName", "pdID", "");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 根据工序绑定设备班组下拉框CXP2012-09-24
        /// CXP2012-10-12
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="pID"></param>
        public static void BindProcessesDeviceLike(FZYK.WinCtrl.YKComboBox cmb, string pID, string wsName)
        {
            string sqlstr = " DECLARE @ifTou INT,@ifHole INT;";
            if (!pID.Equals("") && !pID.Equals("0"))
            {
                sqlstr += " select @ifTou=isnull(ifTou,0),@ifHole=isnull(ifHole,0) FROM B_Processes WHERE pID='" + pID + "'"
                     + " ;SELECT * FROM B_ProcessesDeviceView where pdValid=1 and isnull(ifTou,0)=@ifTou and isnull(ifHole,0)=@ifHole;";
            }
            else if (!wsName.Equals("") && !wsName.Equals("[全部]"))
                sqlstr += "SELECT * FROM B_ProcessesDeviceView where pdValid=1  and wsName='" + wsName + "';";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["pdID"] = 0;
            dr["pdName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pdName", "pdID", "");
            cmb.SelectedIndex = 0;
        }


    }
}
