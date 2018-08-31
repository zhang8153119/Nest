using System;
using System .Collections .Generic;
using System .Windows .Forms;
using System .Linq;
using System .Text;
using System .Data;
using System .Reflection;
namespace FZYK .Core
{
      /// <summary>
      /// 下拉框绑定类
      /// </summary>
      public partial class ComboBoxBinder
      {

            #region 张
            /// <summary>
            /// 绑定发料状态
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindOverState(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("");
                  cmb .Items .Add("未发料");
                  cmb .Items .Add("发料中");
                  cmb .Items .Add("发料完成");
            }
            public static void BindCompanyYCL(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sql = " select DISTINCT Tag1 from dbo.B_Tag WHERE Tag = '原材料'";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  DevCommon .BindComboBox(cmb, dt, "Tag1", "Tag1", "", 3);
            }

            /// <summary>
            /// 绑定年
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindTranState(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("未开始");
                  cmb .Items .Add("进行中");
                  cmb .Items .Add("已完成");
            }
            /// <summary>
            /// 绑定年
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindMyYear(FZYK .WinCtrl .YKComboBox cmb)
            {
                  for (int i = 2012; i <= DateTime .Now .Year; i++)
                  {
                        cmb .Items .Add(i);
                  }
                  cmb .Text = "";
            }
            /// <summary>
            /// 绑定年
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindMyMonth(FZYK .WinCtrl .YKComboBox cmb)
            {
                  for (int i = 1; i <= 12; i++)
                  {
                        cmb .Items .Add(i);
                  }
                  cmb .Text = "";
            }
            /// <summary>
            /// 绑定采购部处理意见
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindDealOpinion(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("让步接收");
                  cmb .Items .Add("降级");
                  cmb .Items .Add("退货");
                  cmb .Items .Add("按废钢处理");
                  cmb .Items .Add("第三方复检合格");
            }
            /// <summary>
            /// 原材料品名下拉框（多选）
            /// 20150708 张
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindmfNameCheckYCL(FZYK .WinCtrl .YKCheckListComboBox cmb)
            {
                  string sqlStr = "SELECT mfName FROM B_MaterialFileView WHERE mcTag = '原材料' GROUP BY mfName";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  cmb .BindData(dt, "mfName", "mfName");
            }
            /// <summary>
            /// 绑定仓库多选下拉框
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseCheckFC(FZYK .WinCtrl .YKCheckListComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='辅材' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  cmb .BindData(dt, "wName", "wName");
            }
            /// <summary>
            /// 绑定仓库多选下拉框
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseCheckYCL(FZYK .WinCtrl .YKCheckListComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='原材料' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  cmb .BindData(dt, "wName", "wName");
            }
            /// <summary>
            /// 得到原材料发票选择框
            /// 张镇波 2014年4月16日 20:28:10
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetBillCheckinSelect(string tag, string tag1, string Company)
            {
                  try
                  {
                        object[] obj = new object[3] { tag, tag1, Company };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmBillCheckinSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch { return null; }
            }
            /// <summary>
            /// 绑定入库状态
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindInState(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("[全部]");
                  cmb .Items .Add("未入库");
                  cmb .Items .Add("部分入库");
                  cmb .Items .Add("入库完毕");
            }
            /// <summary>
            /// 绑定“有”“无”，查找条件用
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindYesNo(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("[全部]");
                  cmb .Items .Add("有");
                  cmb .Items .Add("无");
            }
            /// <summary>
            /// 绑定“有”“无”，查找条件用
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindYesAndNo(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("");
                  cmb .Items .Add("是");
                  cmb .Items .Add("否");
            }
            /// <summary>
            /// 绑定物料分类 原材料
            /// 2014-3-18 新增 
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindmcNameYCL(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT mcName,mcID FROM B_MaterialClass WHERE mcTag ='原材料' AND mcTag1 = '' ORDER BY mcNum ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "mcName", "mcID", "", 1);
            }
            /// <summary>
            /// 绑定物料分类 辅材
            /// 2014-3-18 新增 
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindmcNameFC(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT mcName,mcID FROM B_MaterialClass WHERE mcTag ='辅材' AND mcTag1 = '辅材' ORDER BY mcNum ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "mcName", "mcID", "", 4);
            }
            /// <summary>
            /// 绑定是否开票
            /// 2014-2-24 张镇波
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindbillState(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("[全部]");
                  cmb .Items .Add("已开票");
                  cmb .Items .Add("未开票");
            }
            /// <summary>
            /// 绑定仓库[全部] 12-28 张镇波
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseAll(FZYK .WinCtrl .YKComboBox cmb, string tag, string tag1)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='" + tag + "' AND Company = '" + tag1 + "' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 1);
            }
            /// <summary>
            /// 绑定仓库 12-27 张镇波
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouse(FZYK .WinCtrl .YKComboBox cmb, string tag, string tag1)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='" + tag + "' AND Company = '" + tag1 + "' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 3);
            }

            /// <summary>
            /// 绑定仓库 9-3 张镇波
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouse(FZYK .WinCtrl .YKComboBox cmb, string type)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='" + type + "' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 3);
            }

            /// <summary>
            /// 绑定是否生成采购计划
            /// 12-26 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindpaState(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("[全部]");
                  cmb .Items .Add("已生成");
                  cmb .Items .Add("未生成");
            }
            /// <summary>
            /// 绑定审批状态
            /// 2013-12-9 张镇波 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindApproveState(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("");
                  cmb .Items .Add("未提交");
                  cmb .Items .Add("审批中");
                  cmb .Items .Add("审批通过");
                  cmb .Items .Add("审批未通过");
            }

            /// <summary>
            /// 绑定入库类型
            /// 2014-3-17 增加全部选项
            /// 2013-11-4 新增
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindStockInType(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlstr = "SELECT * FROM dbo.B_InOutType WHERE iotType = '入库' ORDER BY iotSettle";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlstr);
                  DevCommon .BindComboBox(cmb, dt, "iotName", "iotID", "", 1);
            }
            /// <summary>
            /// 绑定出库类型
            /// 2014-3-17 增加全部选项
            /// 2013-11-1 新增
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindStockOutType(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlstr = "SELECT * FROM dbo.B_InOutType WHERE iotType = '出库' ORDER BY iotSettle";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlstr);
                  DevCommon .BindComboBox(cmb, dt, "iotName", "iotID", "", 1);
            }

            /// <summary>
            /// 绑定到货去向下拉框
            /// 2013-10-28 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindSendTo(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = "SELECT stName FROM C_SendTo";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "stName", "stName", "", 1);
            }

            /// <summary>
            /// 原材料品名下拉框
            /// 2013-8-13 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindmfNameYCL(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = "SELECT mfName FROM B_MaterialFileView WHERE mcTag = '原材料' GROUP BY mfName ORDER BY MIN(mcID)";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "mfName", "mfName", "", 1);
            }

            /// <summary>
            /// 原材料材质下拉框
            /// 2013-8-13 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindLimberYCL(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = "SELECT * FROM B_Limber WHERE lType = '原材料' ORDER BY lSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "lName", "lName", "", 1);
            }

            /// <summary>
            /// 绑定合同类别
            /// 2013-8-13 张 新增
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindContractClass(FZYK .WinCtrl .YKComboBox cmb)
            {
                  cmb .Items .Add("[全部]");
                  cmb .Items .Add("正常");
                  cmb .Items .Add("增补");
                  cmb .Items .Add("重签");
            }

            /// <summary>
            /// 获取标准下拉框 
            /// 2013-8-13 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindetName(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sql = "SELECT etName FROM B_ExtraType";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  DevCommon .BindComboBox(cmb, dt, "etName", "etName", "", 1);
            }
            /// <summary>
            /// 绑定材质下拉框
            /// 2013-7-3 张 修改
            /// </summary>
            /// <param name="cmb">需要绑定的下拉框</param>
            /// <param name ="type">需要绑定的材质类型</param>
            /// <param name="flag">1，（全部）2、（无）3、不增加新项目</param>
            public static void BindLimber(FZYK .WinCtrl .YKComboBox cmb, string type, int flag)
            {
                  string sqlStr = "SELECT * FROM B_Limber WHERE lType = '" + type + "' ORDER BY lSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "lName", "lID", "", flag);
            }
            /// <summary>
            /// 绑定结论下拉框
            /// 2013-6-19 张
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindConclusion(FZYK .WinCtrl .YKComboBox cmb)
            {
                  DataTable dt = new DataTable();
                  dt .Columns .Add("prdConclusion");
                  DataRow dr1 = dt .NewRow();
                  dr1["prdconclusion"] = "[全部]";
                  DataRow dr2 = dt .NewRow();
                  dr2["prdconclusion"] = "合格";
                  DataRow dr3 = dt .NewRow();
                  dr3["prdconclusion"] = "不合格";
                  dt .Rows .Add(dr1);
                  dt .Rows .Add(dr2);
                  dt .Rows .Add(dr3);
                  cmb .DisplayMember = "prdconclusion";
                  cmb .ValueMember = "prdconclusion";
                  cmb .DataSource = dt;
            }
            /// <summary>
            /// 绑定处理意见
            /// 2013-6-17 张 新增
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindSuggest(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sql = "  SELECT DISTINCT ud_corrective FROM dbo.Q_UnqualifiedSetting";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  DevCommon .BindComboBox(cmb, dt, "ud_corrective", "ud_corrective", "", 1);
            }

            /// <summary>
            /// 绑定仓库[全部] 2013-3-27 张 新增
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseAll(FZYK .WinCtrl .YKComboBox cmb, string type)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='" + type + "' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 1);
            }
            /// <summary>
            /// 绑定仓库(原材料) 2013-4-25 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseYCL(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='原材料' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 1);
            }
            /// <summary>
            /// 绑定仓库(辅材) 2013-4-25 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseFC(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='辅材' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 1);
            }
            /// <summary>
            /// 绑定仓库(螺栓) 2013-4-25 张 新增
            /// 2014-3-17 增加全部选项
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouseLS(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse WHERE wType ='螺栓' ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 1);
            }
            /// <summary>
            /// 绑定物料档案分类 
            /// 修改 20150426
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindmcName(FZYK .WinCtrl .YKComboBox cmb, string tag, string tag1)
            {
                  string sqlStr = "SELECT * FROM dbo.B_MaterialClass mc INNER JOIN dbo.B_Tag bt ON bt.Tag = mc.mcTag AND bt.TagX = mc.mcTag1"
                  + " WHERE bt.Tag ='" + tag + "' AND bt.Tag1 = '" + tag1 + "' ORDER BY mcNum ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "mcName", "mcID", "", 1);
            }

            /// <summary>
            /// 绑定仓库 2012-8-16增加
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindWarehouse(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_Warehouse ORDER BY wSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "wName", "wID", "", 3);
            }

            /// <summary>
            /// 绑定盘点类型
            /// 张镇波 9-3
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindTakeType(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_InOutType WHERE iotType = '盘点' ORDER BY iotSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "iotName", "iotID", "", 3);
            }
            /// <summary>
            /// 绑定入库类型 2012-8-16增加
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindInType(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_InOutType WHERE iotType = '入库' ORDER BY iotSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "iotName", "iotID", "", 3);
            }
            /// <summary>
            /// 绑定出库类型 2012-8-16增加
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindOutType(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_InOutType WHERE iotType = '出库' ORDER BY iotSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "iotName", "iotID", "", 3);
            }
            /// <summary>
            /// 绑定材质下拉框 2012-8-20
            /// </summary>
            /// <param name="cmb">需要绑定的下拉框</param>
            /// <param name ="type">需要绑定的材质类型</param>
            public static void BindLimber(FZYK .WinCtrl .YKComboBox cmb, string type)
            {
                  string sqlStr = "SELECT * FROM B_Limber WHERE lType = '" + type + "' ORDER BY lSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "lName", "lID", "", 1);
            }

            /// <summary>
            /// 绑定标准 2012-11-20 张 修改 
            /// </summary>
            /// <param name="cmb"></param>
            /// <param name="flag">1，（全部）2、（无）3、不增加新项目</param>
            public static void BindExtraType(FZYK .WinCtrl .YKComboBox cmb, int flag)
            {
                  string sqlStr = "SELECT * FROM B_ExtraType";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "etName", "etNum", "", flag);
            }
            /// <summary>
            /// 绑定采购类型下拉框
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindPurchaseType(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_PurchaseType ";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "ptName", "ptNum", "", 3);
            }

            /// <summary>
            /// 绑定结算方式 2012-8-15修改
            /// </summary>
            /// <param name="cmb"></param>
            public static void BindSettleWay(FZYK .WinCtrl .YKComboBox cmb)
            {
                  string sqlStr = " SELECT * FROM B_SettleWay ORDER BY swSort ASC";
                  DataTable dt = new DataTable();
                  dt = FZYK .Com .YKDataClass .getDataTable(sqlStr);
                  DevCommon .BindComboBox(cmb, dt, "swName", "swID", "", 3);
            }

            #endregion
      }
}
