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
       

        /// <summary>
        /// 绑定招聘记录 合同性质
        /// 鄢国平 2013年6月12日 10:36:58
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindHuntNature(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='HuntNature'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定离职类型
        /// 鄢国平 2015年4月2日 11:29:13
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindEmployeeLeave(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='EmployeeLeave'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定请假类型
        /// 鄢国平 2013年6月12日 10:36:58
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindEmployeeOffType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='EmployeeOffType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 文件借阅申请归还状态
        /// 鄢国平 2015年4月22日 20:58:49
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDocumentBorrowState(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='DocumentBorrowState'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定成品仓库
        /// 2015年4月24日 15:56:32
        /// 鄢国平
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindWarehouseCP(FZYK.WinCtrl.YKComboBox cmb, string Company)
        {
            string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='成品'";
            if (Company != "")
                sqlStr = sqlStr + " and Company='" + Company + "'";
            sqlStr = sqlStr + "ORDER BY wSort ASC";
            DataTable dt = new DataTable();
            dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            //DevCommon.BindComboBox(cmb, dt, "wName", "wID", "", 3);
            cmb.BindData(dt, "wName", "wID", "wID");
        }
        /// <summary>
        /// 绑定有油卡号
        /// 2015年4月24日 20:16:38
        /// 鄢国平
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindOilCard(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = " select * from EB_OilCard";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["ocNum"] = "";
            dr["ocID"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "ocNum", "ocID", "ocID");
            
        }
        /// <summary>
        /// 绑定车辆
        /// 2015年4月24日 20:16:38
        /// 鄢国平
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindVehicleNum(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = " select * from EB_VehicleNumSet";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["vNum"] = "";
            dr["vID"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "vNum", "vID", "vID");

        }
        /// <summary>
        /// 加油记录表类型
        /// 2015年4月24日 20:19:11
        /// 鄢国平
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindOilRecordType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='OilRecordType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定工伤类型
        /// 鄢国平 2013年6月12日 10:36:58
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindInjuryType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='InjuryType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定运费登记货物类别
        /// 鄢国平 2013年3月12日 21:13:54
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindVehicleType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='VehicleType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定年份，
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindYear(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("Year"))
                dt.Columns.Add("Year", typeof(string));
            if (!dt.Columns.Contains("sort"))
                dt.Columns.Add("sort", typeof(int));
            for (int i = 2000; i < 2050; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Year"] = i;
                dr["sort"] = i;
                dt.Rows.Add(dr);
            }
            DataRow drAll = dt.NewRow();
            drAll["Year"] = "";
            dt.Rows.Add(drAll);
            drAll["sort"] = 0;
            cmb.BindData(dt, "Year", "Year", "sort");
        }
        /// <summary>
        /// 绑定月份
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindMonth(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("Month"))
                dt.Columns.Add("Month", typeof(string));
            if (!dt.Columns.Contains("sort"))
                dt.Columns.Add("sort", typeof(int));
            for (int i = 1; i < 13; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Month"] = i;
                dr["sort"] = i;
                dt.Rows.Add(dr);
            }
            DataRow drAll = dt.NewRow();
            drAll["Month"] = "";
            drAll["sort"] = 0;
            dt.Rows.Add(drAll);
            cmb.BindData(dt, "Month", "Month", "sort");
        }

        /// <summary>
        /// 绑定招待费用核算类别
        /// 鄢国平 2013年12月17日 14:41:50
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindHospitalityType(FZYK.WinCtrl.YKComboBox cmb, string Name)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='" + Name + "'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定招待费用核算类别
        /// 鄢国平 2013年12月17日 14:41:50
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindHospitalityType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='招待费'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定员工 学历
        /// 鄢国平 2013年5月28日 20:38:12
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindeEducation(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='eEducation'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定非在职人员档案编号
        /// 鄢国平 2014年4月21日 11:49:14
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFileNumByLeave(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "select distinct eFileNum from OE_EmployeeView oeev where not exists(select 1 from OE_EmployeeView oeev1 where oeev.eFileNum=oeev1.eFileNum and oeev1.eValid='在职')";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["eFileNum"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "eFileNum", "eFileNum", "eFileNum");
        }
        /// <summary>
        /// 绑定销售付款方式 鄢国平2012-10-26
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindSalePayWay(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            string sqlstr = "select * from B_SalePayWay where pwvalid=1 order by pwifDefault desc";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ToolStripItem si = new ToolStripMenuItem();
                si.Text = dt.Rows[i]["pwName"].ToString();
                si.Tag = dt.Rows[i]["pwName"].ToString();
                ms.Items.Add(si);
            }
        }
        /// <summary>
        /// 性别下拉框绑定
        /// 鄢国平 2013年3月13日 11:00:03
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindSex(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("Sex"))
                dt.Columns.Add("Sex", typeof(string));
            DataRow dr = dt.NewRow();
            dr["Sex"] = "男";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow();
            dr1["Sex"] = "女";
            dt.Rows.Add(dr1);
            cmb.BindData(dt, "Sex", "Sex", "Sex");
        }
        /// <summary>
        /// 绑定员工 在职状态
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindeValid(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='EmployeeValid'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定员工 职工级别
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindEmployeeLevel(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='EmployeeLevel'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定运费登记运费类别
        /// 鄢国平 2013年3月12日 21:13:54
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindShipCheckMoney(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='ShipCheckMoney'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定运费登记运输方式
        /// 鄢国平 2013年3月12日 21:13:54
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindShipCheckWay(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='ShipCheckWay'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定公告类别 鄢国平 2013年1月19日 22:15:36
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindMessageType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='MessageType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }

        /// <summary>
        /// 绑定报销类别 鄢国平 2013年1月19日 22:15:36
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindExpenseType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='ExpenseType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定考勤类型 鄢国平 2013年1月18日 15:19:04
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindKaoQinType(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from K_KaoQinTypeSet");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["kqType"].ToString() + "-" + dt.Rows[i]["kqImage"].ToString());
            }
        }
        /// <summary>
        /// 绑定证书类型 鄢国平 2013年1月14日 17:07:34
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProfessionCertificateType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='ProfessionCertificateType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定证书类型 鄢国平 2013年1月14日 17:07:34
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindEmployeeCompactType(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select value,sort  from B_CommonForCombox where setName='EmployeeCompactType'");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定补件类型
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindPatchType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sql = "select distinct value,sort from B_CommonForCombox where setName='PatchType' and valid=1";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sql);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定补件原因
        /// 鄢国平 2014年6月18日 16:57:48
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindPatchReason(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sql = "select distinct value,sort from B_CommonForCombox where setName='PatchReason' and valid=1";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sql);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定类型 鄢国平 2013-01-06
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="scdID">杆塔ID</param>
        public static void BindPackType(FZYK.WinCtrl.YKComboBox cmb, int pID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT distinct pdType FROM PF_PackDetail where pID=" + pID);
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            cmb.BindData(dt, "pdType", "pdType", "pdType");
        }
        /// <summary>
        /// 绑定开户行
        /// 鄢国平 2012年11月20日 09:45:52
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindBank(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = " select distinct bName from B_Bank";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "bName", "bName", "bName");
        }

        /// <summary>
        /// 绑定国家 鄢国平 2012-11-10
        /// </summary>
        /// <param name="cmb">控件</param>
        public static void BindCountry(FZYK.WinCtrl.YKComboBox cmb)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from B_Countries");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DevCommon.ComboBoxBindHavingBlank(cmb, dt, "cName", "cID");
        }
        /// <summary>
        /// 绑定省份 鄢国平 2012-11-10
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="CountryID">国家ID</param>
        public static void BindProvince(FZYK.WinCtrl.YKComboBox cmb, int CountryID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from B_Province");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DevCommon.ComboBoxBindHavingBlank(cmb, dt, "pName", "pID");
        }
        /// <summary>
        /// 绑定市 鄢国平 2012-11-10
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="pID">省份ID</param>
        public static void Bindcity(FZYK.WinCtrl.YKComboBox cmb, int pID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from B_City where (1=1)");
            if (pID > 0)
                sbSql.Append(" and pID=" + pID);
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DevCommon.ComboBoxBindHavingBlank(cmb, dt, "cName", "cID");
        }

        /// <summary>
        /// 绑定销售类型
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindSalaType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "select * from B_SaleType where stvalid=1 order by stifDefault desc";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["stName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "stName", "stName", "stName");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// //绑定运输方式
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindShippingType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "select * from B_ShippingType where stvalid=1 order by stifDefault desc";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["stName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "stName", "stName", "stName");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定履约类型
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindAgreeType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='S_AgreeType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "value");
        }

        /// <summary>
        /// 绑定电压等级下拉框
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindPressure(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            string sqlstr = "SELECT cfc.value AS Pressure,dbo.fSortNumber(cfc.value) AS mysort FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='Pressure'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["Pressure"].ToString());
            }
        }
        /// <summary>
        /// 绑定中标类型下拉框
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindBidType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct [value],[sort] FROM B_CommonForCombox WHERE setName='BidType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定产品类别
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProductName(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct [value],[sort] FROM B_CommonForCombox WHERE setName='ProductName'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
            cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定部门
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDept(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT * FROM B_Dept where dValid=1";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "dName", "dNum", "dSort");
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
        /// 江津绑定分公司
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindCompanyJJ(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sql = "declare @CompanyLen int"
                + " select @CompanyLen=SetValue from B_ConfigureSys where SetName='B_CompanyLen'"
                + " select dName from B_Dept where len(dNum)=ISNULL(@CompanyLen,2) and dValid=1";
            DataTable dtDept = FZYK.Com.YKDataClass.getDataTable(sql);
            DataRow dr = dtDept.NewRow();
            dr["dName"] = "";
            dtDept.Rows.InsertAt(dr, 0);
            dtDept.AcceptChanges();
            cmb.BindData(dtDept, "dName", "dName", "dName");
        }

        /// <summary>
        /// 江津绑定支付方式
        /// 2014年4月11日 15:40:32
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindPayType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT cfc.value,sort  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='PayType' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 江津绑定财务收支款项来源
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFinancialSource(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT cfc.value,sort  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='FinancialSource' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 会议预约表会议地点
        /// 鄢国平
        /// 2014年6月6日 10:28:06
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindMeetPlace(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT cfc.value,sort  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='MeetPlace' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 江津绑定费用名称
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFinancialName(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT cfc.value,sort  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='FinancialName' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 江津资金支付计划绑定部门
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindPayPlanDept(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dtDept = FZYK.Com.YKDataClass.getDataTable(" select distinct dName from M_PayPlanView");
            DataRow dr = dtDept.NewRow();
            dr["dName"] = "";
            dtDept.Rows.InsertAt(dr, 0);
            dtDept.AcceptChanges();
            cmb.BindData(dtDept, "dName", "dName", "dName");
        }
        /// <summary>
        /// 江津 内部资金划转表绑定支付方式
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFundTrasnferType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT cfc.value,sort  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='FundTrasnferType' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 江津绑定分公司
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindCompanyJJ(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(" select dName from B_Dept where len(dNum)=4 and dValid=1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["dName"].ToString());
            }
        }

        /// <summary>
        /// 江津绑定授信类别
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindCreditType(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(" SELECT cfc.value  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='CreditType'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 江津绑定融资类别
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindFinanceType(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(" SELECT cfc.value  FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='FinanceType'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定电压等级下拉框
        /// </summary>
        /// <param name="cmb">下拉框控件</param>
        public static void BindPressure(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT cfc.value AS Pressure,dbo.fSortNumber(cfc.value) AS mysort FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='Pressure'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "Pressure", "mysort", "mysort");
        }
        /// <summary>
        /// 绑定电压等级下拉框
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindPressure(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT cfc.value AS Pressure,dbo.fSortNumber(cfc.value) AS mysort FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='Pressure'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["Pressure"].ToString());
            }
        }
        /// <summary>
        /// 绑定合同合同订单方式
        /// </summary>
        /// <param name="cmb">下拉框控件</param>
        public static void BindOrderType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='S_Compact'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定合同联系单位的类型
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindCustomerType(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='S_CompactContact'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定产品类别
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindProductName(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='ProductName'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-07-30 绑定变更类型
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindOrderTaskDetailChange(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='OrderTaskDetailChange'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定保函申请单种类
        /// 鄢国平 2014年3月25日 15:40:19
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindGuaranteeApplyType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='GuaranteeApplyType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定融资收款单类型
        /// 鄢国平 2014年3月25日 15:40:19
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindFinanceCollectType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='FinanceCollectType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定融资支付请款单类型
        /// 鄢国平 2014年3月25日 15:40:19
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindFinancePayApplyType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='FinancePayApplyType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-08-09 绑定h回款类型
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindAcceptType(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='AcceptType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-08-06 绑定币种
        /// 鄢国平 2012-08-09 修改默认第一条数据
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindMoneyType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='MoneyType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "value", "value", "sort");
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定来款方式
        /// 鄢国平  2014年3月24日 20:42:13 来款方式
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindReturnType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='ReturnType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定来款性质
        /// 鄢国平  2014年3月24日 20:42:13 来款性质
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindReturnNature(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='ReturnNature'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定代收代付申请单来款性质
        /// 鄢国平  2014年4月30日 10:06:45 来款性质
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindPayRelaceReturnNature(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='PayRelaceReturnNature'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.Add(dr);
            cmb.BindData(dt, "value", "value", "sort");
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-08-06 绑定币种
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindMoneyType(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='MoneyType' order by sort";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定产品名称下拉框
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindProductName(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            string sqlstr = "SELECT cfc.value AS ProductName,dbo.fSortNumber(cfc.value) AS mysort FROM B_CommonForCombox cfc WHERE cfc.valid=1 AND cfc.setName='ProductName'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["ProductName"].ToString());
            }
        }


        /// <summary>
        /// 绑定合同联系单位的类型
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindCustomerType(FZYK.WinCtrl.YKContextMenuStrip ms)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='S_CompactContact'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ms.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }
        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-07-30 绑定变更类型
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindOrderTaskDetailChange(FZYK.WinCtrl.YKContextMenuStrip cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='OrderTaskDetailChange'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }

        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-08-07 绑定发票类型
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindBillType(DataGridViewComboBoxColumn cmb)
        {
            string sqlstr = "SELECT distinct value FROM B_CommonForCombox WHERE setName='BillType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb.Items.Add(dt.Rows[i]["value"].ToString());
            }
        }

        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2012-08-06 绑定销售发票开票状态
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindCompactBillType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='S_CompactBillState'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            cmb.BindData(dt, "value", "value", "sort");
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定班组 
        /// 鄢国平 2013年7月30日 14:18:48
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="wsName">车间</param>
        /// <param name="ifTou">工序TfTou，100为所有工序</param>
        public static void BindpgName(FZYK.WinCtrl.YKContextMenuStrip ms, string wsName, int ifTou)
        {
            ms.Items.Clear();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" SELECT pgID,pgName FROM B_ProcessesGroupView where (1=1)");
            if (wsName != "")
                sbSql.Append(" AND wsName='" + wsName + "'");
            if (ifTou != 100)
                sbSql.Append(" AND IfTou=" + ifTou);
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = dt.Rows[i]["pgName"].ToString();
                tsmi.Tag = dt.Rows[i]["pgID"].ToString();
                ms.Items.Add(tsmi);
            }
        }

        /// <summary>
        /// 绑定班组 鄢国平2012-08-28
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="wsName">车间</param>
        /// <param name="ifTou">工序TfTou，100为所有工序</param>
        public static void BindpgName(FZYK.WinCtrl.YKComboBox cmb, string wsName, int ifTou)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" SELECT pgID,pgName FROM B_ProcessesGroupView where (1=1)");
            if (wsName != "")
                sbSql.Append(" AND wsName='" + wsName + "'");
            if (ifTou != 100)
                sbSql.Append(" AND IfTou=" + ifTou);
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            cmb.BindData(dt, "pgName", "pgID", "");
            if (dt.Rows.Count > 0)
                cmb.SelectedIndex = 0;
            else
                cmb.Text = "";
        }
        /// <summary>
        /// 绑定包名 鄢国平 2012-09-11
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="scdID">杆塔ID</param>
        public static void BindPackDetail(FZYK.WinCtrl.YKComboBox cmb, int pID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT pdID,pdName,dbo.fSortNumber(pdName) as pdSort FROM PF_PackDetailView where pID=" + pID);
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            DataRow dr = dt.NewRow();
            dr["pdName"] = "";
            dr["pdID"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "pdName", "pdID", "pdSort");
        }
        /// <summary>
        /// 绑定包名 鄢国平 2012-09-11
        /// </summary>
        /// <param name="cmb">控件</param>
        /// <param name="scdID">杆塔ID</param>
        public static void BindPackDetail(FZYK.WinCtrl.YKContextMenuStrip ms, int pID)
        {
            ms.Items.Clear();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT pdID,(pdName+' 数量:'+ltrim(pdSumCount)+' 重量:'+ltrim(pdSumWeight)) AS pdName FROM PF_PackDetailView where pID=" + pID + " order by dbo.fSortNumber(pdName)");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sbSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ToolStripItem si = new ToolStripMenuItem();
                si.Text = dt.Rows[i]["pdName"].ToString();
                si.Tag = dt.Rows[i]["pdID"].ToString();
                ms.Items.Add(si);
            }
        }
        /// <summary>
        /// 绑定变更类别
        /// 鄢国平 2013-01-11绑定员工人员类别
        /// </summary>
        /// <param name="cmb">DGV下拉框列</param>
        public static void BindEmployeeType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct value,sort FROM B_CommonForCombox WHERE setName='EmployeeType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            cmb.BindData(dt, "value", "value", "sort");
            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定车辆基础设置公司划分类型
        /// 添加 鄢国平 2014年4月21日 16:46:25
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindVehicleCompanyType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='vCompanyType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定银承收支日报票据号
        /// 添加 鄢国平 2014年4月21日 16:46:25
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindBankAcceptanceBillNo(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "select badBillNo,SUM(badMoney-badPayMoney)badBanlance from M_BankAcceptanceDetail group by badBillNo having SUM(badMoney-badPayMoney)>0";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["badBillNo"] = "";
            dr["badBanlance"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "badBillNo", "badBanlance", "badBillNo");
        }
        /// <summary>
        /// 车辆维修类别
        /// 添加 鄢国平 2014年4月24日 14:54:14
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindVehicleMaintainType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "select distinct vmmType from EB_VehicleMendMaintain";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["vmmType"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "vmmType", "vmmType", "vmmType");
        }
        /// <summary>
        /// 绑定是/否
        /// 鄢国平 2013年8月26日 11:25:41
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindYesOrNo(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("text"))
                dt.Columns.Add("text", typeof(string));
            if (!dt.Columns.Contains("value"))
                dt.Columns.Add("value", typeof(int));
            if (!dt.Columns.Contains("sort"))
                dt.Columns.Add("sort", typeof(int));
            DataRow dr = dt.NewRow();
            dr["text"] = "";
            dr["value"] = "0";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            DataRow dr1 = dt.NewRow();
            dr1["text"] = "是";
            dr1["value"] = "1";
            dr1["sort"] = 1;
            dt.Rows.InsertAt(dr1, 0);
            DataRow dr2 = dt.NewRow();
            dr2["text"] = "否";
            dr2["value"] = "0";
            dr2["sort"] = 2;
            dt.Rows.InsertAt(dr2, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "text", "value", "sort");
        }
        /// <summary>
        /// 绑定单据状态
        /// 鄢国平 2013年8月26日 11:25:41
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindPayApplyState(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("text"))
                dt.Columns.Add("text", typeof(string));
            DataRow dr = dt.NewRow();
            dr["text"] = "";
            dt.Rows.InsertAt(dr, 0);
            DataRow dr1 = dt.NewRow();
            dr1["text"] = "特批";
            dt.Rows.InsertAt(dr1, 0);
            DataRow dr2 = dt.NewRow();
            dr2["text"] = "对冲";
            dt.Rows.InsertAt(dr2, 0);
            DataRow dr3 = dt.NewRow();
            dr3["text"] = "计划内";
            dt.Rows.InsertAt(dr3, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "text", "text", "text");
        }
        /// <summary>
        /// 绑定来款状态
        /// 鄢国平 2014年3月28日 11:56:05
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindReturnMoneyState(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("text"))
                dt.Columns.Add("text", typeof(string));
            DataRow dr = dt.NewRow();
            dr["text"] = "";
            dt.Rows.InsertAt(dr, 0);
            DataRow dr1 = dt.NewRow();
            dr1["text"] = "计划内必回";
            dt.Rows.InsertAt(dr1, 0);
            DataRow dr2 = dt.NewRow();
            dr2["text"] = "计划内争取";
            dt.Rows.InsertAt(dr2, 0);
            DataRow dr3 = dt.NewRow();
            dr3["text"] = "计划外";
            dt.Rows.InsertAt(dr3, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "text", "text", "text");
        }
        ///// <summary>
        ///// 绑定审批状态
        ///// 鄢国平 2014年4月13日 23:52:37
        ///// </summary>
        ///// <param name="cmb"></param>
        //public static void BindApproveState(FZYK.WinCtrl.YKComboBox cmb)
        //{
        //    DataTable dt = new DataTable();
        //    if (!dt.Columns.Contains("text"))
        //        dt.Columns.Add("text", typeof(string));
        //    DataRow dr = dt.NewRow();
        //    dr["text"] = "";
        //    dt.Rows.InsertAt(dr, 0);
        //    DataRow dr1 = dt.NewRow();
        //    dr1["text"] = "审批通过";
        //    dt.Rows.InsertAt(dr1, 0);
        //    DataRow dr2 = dt.NewRow();
        //    dr2["text"] = "未提交";
        //    dt.Rows.InsertAt(dr2, 0);
        //    DataRow dr3 = dt.NewRow();
        //    dr3["text"] = "不同意";
        //    dt.Rows.InsertAt(dr3, 0);
        //    dt.AcceptChanges();
        //    cmb.BindData(dt, "text", "text", "text");
        //}
        /// <summary>
        /// 绑定预算申报费用名称
        /// 鄢国平 2014年5月5日 10:42:03
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindBudgetApplyName(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT bcfc.[value], bcfc.sort FROM B_CommonForCombox bcfc WHERE bcfc.valid=1 AND bcfc.setName='BudgetApplyName'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定饭店下拉框
        /// 鄢国平 2014年5月16日 11:04:31
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindHotelAgreement(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "select hadName from EB_HotelAgreementDetail where hadAttribute='饭店'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["hadName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "hadName", "hadName", "hadName");
        }

        /// <summary>
        /// 绑定费用名称
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindBudgetName(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "select bsOneItem from M_BudgetSet where ISNULL(bsOneItem,'')<>''";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["bsOneItem"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "bsOneItem", "bsOneItem", "bsOneItem");
        }
        /// <summary>
        /// 绑定残疾等级
        /// 鄢国平 2014年7月22日 21:10:31
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDeformityRank(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='DeformityRank'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定残疾类别
        /// 鄢国平 2014年7月22日 21:10:31
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindDeformityType(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "SELECT value, sort FROM B_CommonForCombox WHERE valid=1 AND setName='DeformityType'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");
        }
        /// <summary>
        /// 绑定结算状态
        /// 鄢国平 2014年7月22日 21:10:31
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindAccountState(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlStr = "select col as value from DBO.f_split(',未结算,部分结算,全部结算',',')";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            cmb.BindData(dt, "value", "value", "");
        }
        /// <summary>
        /// 绑定关联状态
        /// 鄢国平 2014年8月6日 19:33:42
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindRelationState(FZYK.WinCtrl.YKComboBox cmb)
        {
            DataTable dt = new DataTable();
            if (!dt.Columns.Contains("text"))
                dt.Columns.Add("text", typeof(string));
            DataRow dr = dt.NewRow();
            dr["text"] = "";
            dt.Rows.InsertAt(dr, 0);
            DataRow dr1 = dt.NewRow();
            dr1["text"] = "未关联";
            dt.Rows.InsertAt(dr1, 0);
            DataRow dr2 = dt.NewRow();
            dr2["text"] = "部分关联";
            dt.Rows.InsertAt(dr2, 0);
            DataRow dr3 = dt.NewRow();
            dr3["text"] = "全部关联";
            dt.Rows.InsertAt(dr3, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "text", "text", "text");
        }
        /// <summary>
        /// 成品仓库库位
        /// 添加 鄢国平 2014年10月28日 11:22:06
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindGoodsSpace(FZYK.WinCtrl.YKComboBox cmb, int wID)
        {
            string sqlStr = "select distinct gsName from B_GoodsSpace where (1=1) and Tag='成品'";
            //if (wID > 0)
            //    sqlStr += " and wID =" + wID;
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlStr);
            DataRow dr = dt.NewRow();
            dr["gsName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "gsName", "gsName", "gsName");
        }
        /// <summary>
        /// 成品仓库区位
        /// 添加 鄢国平 2014年10月28日 11:22:06
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindZone(FZYK.WinCtrl.YKComboBox cmb, int wID, string gsName)
        {
            string sql = "select zName,gsName from B_Zone where (1=1) ";
            //if (wID > 0)
            //    sql = sql + " and wID=" + wID;
            if (gsName != "")
                sql = sql + " and gsName='" + gsName + "'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sql);
            DataRow dr = dt.NewRow();
            dr["zName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "zName", "zName", "zName");
        }

        /// <summary>
        /// 绑定产品类别
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindProductName(FZYK.WinCtrl.YKCheckListComboBox cmb)
        {
            string sqlstr = "SELECT distinct [value],[sort] FROM B_CommonForCombox WHERE setName='ProductName'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "sort");

        }
        /// <summary>
        /// 绑定片区
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindArea(FZYK.WinCtrl.YKComboBox cmb)
        {
            string sqlstr = "SELECT distinct [value],[sort] FROM B_CommonForCombox WHERE setName='Area'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "value", "sort");

        }
        /// <summary>
        /// 绑定片区
        /// </summary>
        /// <param name="cmb"></param>
        public static void BindArea(FZYK.WinCtrl.YKCheckListComboBox cmb)
        {
            string sqlstr = "SELECT distinct [value],[sort] FROM B_CommonForCombox WHERE setName='Area'";
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr);
            DataRow dr = dt.NewRow();
            dr["value"] = "";
            dr["sort"] = 0;
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            cmb.BindData(dt, "value", "sort");

        }
    }
}
