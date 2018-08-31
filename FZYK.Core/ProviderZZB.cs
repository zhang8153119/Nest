using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Reflection;
using System .Data;
using WeifenLuo .WinFormsUI .Docking;
using System .Windows .Forms;
using FZYK .Com;

namespace FZYK .Core
{
      /// <summary>
      /// 提供者
      /// </summary>
      public partial class Provider
      {
            public static BaseSelectDialog GetBoltStockInSelectNew()
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmBoltStockInSelectNew") as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            public static BaseSelectDialog GetBoltStockInSelect()
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmBoltStockInSelect") as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 生成领料单
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetPickingPlanAll(int ptID)
            {
                  try
                  {
                        object[] obj = new object[1] { ptID };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmPickingPlanAll", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 手签名
            /// </summary>
            /// <returns></returns>
            public static Form GetSignForm()
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.ApproveControl.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.ApproveControl.FrmSign") as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 生成领料单
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetStockPickSingle(string tag, string tag1, int dsID, string flag)
            {
                  try
                  {
                        object[] obj = new object[4] { tag, tag1, dsID, flag };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockPick", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 选择库存
            /// 张镇波 2016-6-19
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetStockSelect(int wID, string source, string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[4] { wID, source, tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch { return null; }
            }
            public static void RefreshHomePage()
            {
                  BaseLog frm = Application .OpenForms["FrmHomePage"] as BaseLog;
                  if (frm != null)
                  {
                        frm .BindDataGrid();
                  }
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
            #region  张镇波
            /// <summary>
            /// 出库选择框
            /// 2014-5-20 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockOutSelect(int wID, string soPickDept, string tag, string tag1, string type)
            {
                  try
                  {
                        object[] obj = new object[5] { wID, soPickDept, tag, tag1, type };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockOutSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 出库选择框
            /// 2014-5-20 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockOutSelect(string tag, string tag1, string type)
            {
                  try
                  {
                        object[] obj = new object[3] { tag, tag1, type };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockOutSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 出库选择框
            /// 2015-8-10 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockOutSelect(string tag, string tag1, string type, string wherestr)
            {
                  try
                  {
                        object[] obj = new object[4] { tag, tag1, type, wherestr };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockOutSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 采购计划
            /// 2014-5-15 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetPurchaseApply(DataTable dt)
            {
                  try
                  {
                        object[] obj = new object[1] { dt };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmPurchaseApply", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 领料单
            /// 2014-5-8 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetStockOut(string tableName, string Key, int Value, string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[5] { tableName, Key, Value, tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockOut", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            /// <summary>
            /// 入库明细选择框，有tag1参数,供应商名称参数
            /// 2014-3-28 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockInSelect(string tag, string tag1, string sfName, string user)
            {
                  try
                  {
                        object[] obj = new object[4] { tag, tag1, sfName, user };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockInSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 入库明细选择框，有tag1参数
            /// 2014-2-24 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockInSelect(string tag, string tag1, int sfID, string user)
            {
                  try
                  {
                        object[] obj = new object[4] { tag, tag1, sfID, user };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockInSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            #region 物料档案选择框
            /// <summary>
            /// 物料档案选择框（按分类过滤） 2013-10-11 增加
            /// 2014-1-16 修改
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetMaterialSelect(string tag, int mcID, string tag1)
            {
                  try
                  {
                        object[] obj = new object[3] { tag, mcID, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmMaterialFileSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }
            /// <summary>
            /// 物料档案选择框（有参数） 2012-12-10 修改
            /// 2014-1-16 修改
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetMaterialSelect(DataTable dt, string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[3] { dt, tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmMaterialFileSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }

            /// <summary>
            /// 物料档案选择框2012-12-10修改
            /// 2014-1-16 修改
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetMaterialSelect(string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[2] { tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmMaterialFileSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }
            #endregion
            /// <summary>
            /// 采购计划（领料单生成）
            /// 2013-12-2 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetPurchaseApply(string sodIDList, string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[3] { sodIDList, tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmPurchaseApply", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }
            /// <summary>
            /// 到货单选择框 
            /// 2013-11-20 新增 张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetGoodsAccept(int sfID, string flag, string way, string tag, string tag1, string Company)
            {
                  try
                  {
                        object[] obj = new object[6] { sfID, flag, way, tag, tag1, Company };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmGoodsAcceptSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }

            public static BaseSelectDialog GetGoodsAccept(int sfID, string flag, string way, string tag, string tag1, string Company, string wName)
            {
                  try
                  {
                        object[] obj = new object[7] { sfID, flag, way, tag, tag1, Company, wName };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmGoodsAcceptSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }
            /// <summary>
            /// 库存提醒框 张镇波 2013-10-30 增加
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockAlert()
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockAlert") as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }



            /// <summary>
            /// 选择采购计划明细
            /// 2013-11-20 新增张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetPurchaseApplySelect(string type, string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[3] { type, tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmPurchaseApplySelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            /// <summary>
            /// 供应商选择框(带参数) 2013-3-14 增加
            /// 添加：张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetSupplierSelect(string tag)
            {
                  try
                  {
                        object[] obj = new object[1] { tag };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmSupplierFileSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 送检明细选择框 
            /// 2013-6-27 张
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetTestApplySelect(string tag, string Company)
            {
                  try
                  {
                        object[] obj = new object[2] { tag, Company };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmTestApplySelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            /// <summary>
            /// 送检明细选择框 
            /// 2013-6-27 张
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetTestApplySelect(string tag, string tag1, string Company)
            {
                  try
                  {
                        object[] obj = new object[3] { tag, tag1, Company };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmTestApplySelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }


            /// <summary>
            /// 到货单选择框 2013-1-19新增 zhang
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetGoodsAccept(int sfID, string flag, string way)
            {
                  try
                  {
                        object[] obj = new object[3] { sfID, flag, way };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmGoodsAcceptSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }

            /// <summary>
            /// 供应商选择框
            /// 添加：张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetSupplierSelect()
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmSupplierFileSelect") as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            /// <summary>
            /// 到货单选择框 zhang 2012-8-9
            /// 添加：张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetGoodsAccept()
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmGoodsAcceptSelect") as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }
            /// <summary>
            /// 到货单选择框 2012-8-16增加
            /// 添加：张镇波
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetGoodsAccept(int sfID)
            {
                  try
                  {
                        object[] obj = new object[1] { sfID };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmGoodsAcceptSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }
            /// <summary>
            /// FTP上传界面 zhang 9-20
            /// </summary>
            /// <param name="module"></param>
            /// <param name="fcMenu"></param>
            /// <param name="fcTable"></param>
            /// <param name="fcKey"></param>
            /// <param name="fcValue"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetFTPUpload(string module, string fcMenu, string fcTable, string fcKey, int fcValue)
            {
                  try
                  {
                        object[] obj = new object[5] { module, fcMenu, fcTable, fcKey, fcValue };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmFTP", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 到货单选择框 11-29修改 zhang
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetGoodsAccept(int sfID, string flag)
            {
                  try
                  {
                        object[] obj = new object[2] { sfID, flag };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmGoodsAcceptSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw; }
            }

            /// <summary>
            /// 审批流程 2012-12-28
            /// </summary>
            /// <param name="fmID"></param>
            /// <param name="table"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static BaseSingle GetFrmApproveDetail(int fmID, string table, string key, int value)
            {
                  try
                  {
                        object[] obj = new object[4] { fmID, table, key, value };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmApproveDetail", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 流程选择 2012-12-29
            /// </summary>
            /// <param name="menu"></param>
            /// <param name="table"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetFrmFlowSelect(string menu, string table, string key, int value)
            {
                  try
                  {
                        object[] obj = new object[4] { menu, table, key, value };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmFlowSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 件号匹配 2013-1-5 zhang
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetFrmCuttingDetailPart(int ptID)
            {
                  try
                  {
                        object[] obj = new object[1] { ptID };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmCuttingDetailPart", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            #endregion
            /// <summary>
            /// 选择发票
            /// 张镇波 2014-4-18
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetBillCheckinSelect(string tag)
            {
                  try
                  {
                        object[] obj = new object[1] { tag };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmBillCheckinSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch { return null; }
            }
            /// <summary>
            /// 选择发票
            /// 张镇波 2014-4-18
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetBillCheckinSelect(string tag, string sfName)
            {
                  try
                  {
                        object[] obj = new object[2] { tag, sfName };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Purchase.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Purchase.FrmBillCheckinSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch { return null; }
            }
            /// <summary>
            /// 弹出审批单 2013-4-11 张
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetApproveDetail(int amID)
            {
                  try
                  {
                        object[] obj = new object[1] { amID };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmApproveDetail", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 弹出审批消息提醒
            /// 张镇波
            /// 2013-5-19 修改
            /// </summary>
            /// <returns></returns>
            public static Form ShowAlertForm(string content)
            {
                  try
                  {
                        object[] obj = new object[1] { content };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        Form frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.AlertForm", true, BindingFlags .CreateInstance, null, obj, null, null) as Form;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            /// <summary>
            /// 按明细ID串弹出不合格审理单
            /// </summary>
            /// <returns></returns>
            public static BaseSingle GetProduceReject(string codeList)
            {
                  try
                  {
                        object[] obj = new object[1] { codeList };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Quality.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Quality.FrmProduceReject", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }

            /// <summary>
            /// 弹出审批相关的single界面
            /// 张镇波 2013-5-21
            /// </summary>
            /// <param name="amID"></param>
            /// <returns></returns>
            public static BaseSingle GetSingleApprove(int amID)
            {
                  try
                  {
                        string sql1 = "SELECT amValue,fmID FROM dbo.B_ApproveMain AS bam WHERE amID = " + amID .ToString();
                        DataTable dt1 = new DataTable();
                        dt1 = YKDataClass .getDataTable(sql1);
                        int keyValue = Convert .ToInt32(dt1 .Rows[0]["amValue"] .ToString());
                        int fmID = Convert .ToInt32(dt1 .Rows[0]["fmID"] .ToString());

                        string sql2 = "SELECT bal.alSingle,alClass FROM dbo.B_FlowMain AS bfm "
                                    + " INNER JOIN dbo.B_AuthorityList AS bal ON bal.alName = bfm.mName WHERE fmID = " + fmID .ToString();
                        DataTable dt2 = new DataTable();
                        dt2 = YKDataClass .getDataTable(sql2);
                        string alsingle = dt2 .Rows[0]["alSingle"] .ToString();
                        string alClass = dt2 .Rows[0]["alClass"] .ToString();

                        object[] obj = new object[1] { keyValue };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\" + alClass + ".dll");
                        BaseSingle frm = _assembly .CreateInstance(alsingle, true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch { return null; }
            }
            /// <summary>
            /// 选择库存
            /// 张镇波 2013-5-22
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetStockSelect(string tag)
            {
                  try
                  {
                        object[] obj = new object[1] { tag };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Quality.FrmStockSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch { return null; }
            }
            /// <summary>
            /// 入库明细选择框
            /// 2013-5-25 张（修改）
            /// </summary>
            /// <returns></returns>
            public static BaseSelectDialog GetStockInSelect(string tag, int sfID, string user)
            {
                  try
                  {
                        object[] obj = new object[3] { tag, sfID, user };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmStockInSelect", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch (Exception ex) { throw ex; }
            }
            /// <summary>
            /// 打印备注
            /// 张镇波 2013-6-17
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSelectDialog GetPrintRemark(string prName)
            {
                  try
                  {
                        object[] obj = new object[1] { prName };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.BaseSet.dll");
                        BaseSelectDialog frm = _assembly .CreateInstance("FZYK.WinUI.BaseSet.FrmPrintRemark", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSelectDialog;
                        return frm;
                  }
                  catch { return null; }
            }

            /// <summary>
            /// 生成复检送检单
            /// 张镇波 2013-7-18
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static BaseSingle GetTestApply(string oldIDList, string tag, string tag1)
            {
                  try
                  {
                        object[] obj = new object[3] { oldIDList, tag, tag1 };
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.WinUI.Warehouse.dll");
                        BaseSingle frm = _assembly .CreateInstance("FZYK.WinUI.Warehouse.FrmTestApply", true, BindingFlags .CreateInstance, null, obj, null, null) as BaseSingle;
                        return frm;
                  }
                  catch { return null; }
            }
      }
}
