using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Data;

namespace FZYK .Core
{
      public partial class ContextMenuStripBinder
      {
            #region AMo
            public static void BindWorkShop(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT bws.wsName FROM B_WorkShop bws";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  cms .Items .Add("");
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }
            #endregion
            #region 张镇波
            /// <summary>
            /// 绑定辅材物料分类 
            /// 2013-11-10 张镇波 增加
            /// </summary>
            /// <param name="cms"></param>
            public static void BindmcNameFC(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT mcName FROM dbo.B_MaterialClass WHERE mcTag = '辅材'";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }
            /// <summary>
            /// 右键绑定区位 
            /// zhang 2015-3-19 修改
            /// </summary>
            /// <param name="cms"></param>
            public static void BindZone(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT DISTINCT zName FROM W_Zone";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }
            /// <summary>
            /// 右键绑定到货去向 1-19新增 zhang
            /// </summary>
            /// <param name="cms"></param>
            public static void BindSendTo(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT stName FROM C_SendTo";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }


            /// <summary>
            /// 绑定规格到右键菜单，1-19新增 zhang
            /// </summary>
            /// <param name="cms"></param>
            public static void BindSpec(FZYK .WinCtrl .YKContextMenuStrip cms, string name)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT mfSpec,CAST(dbo.getTypeWidthOrLength(mfSpec,0) AS decimal) AS s0 "
                  + " ,CAST(dbo.getTypeWidthOrLength(mfSpec,0) AS decimal) AS s1 FROM B_MaterialFile WHERE mfName ='" + name + "' ORDER BY s0,s1";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }
            /// <summary>
            /// 右键绑定库位
            /// 2014-9-24 增加Tag1参数
            /// </summary>
            /// <param name="cms"></param>
            public static void BindGoodsSpace(FZYK .WinCtrl .YKContextMenuStrip cms, string tag, string tag1)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT gsName FROM B_GoodsSpace WHERE Tag = '" + tag + "' AND Tag1 = '" + tag1 + "'";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }

            /// <summary>
            /// 绑定材质到右键菜单，
            /// </summary>
            /// <param name="cms"></param>
            /// <param name="type">类型：“原材料”，“五金”，“螺栓”</param>
            public static void BindLimber(FZYK .WinCtrl .YKContextMenuStrip cms, string type)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT lName FROM B_Limber WHERE lType ='" + type + "' ORDER BY lSort ASC";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }

            /// <summary>
            /// 绑定计量方式到右键菜单
            /// </summary>
            /// <param name="cms"></param>
            public static void BindMertingWay(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT mwName FROM B_MeteringWay ORDER BY mwSort ASC";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }
            /// <summary>
            /// 绑定类型2右键菜单 10-16改减少一个字段
            /// </summary>
            /// <param name="cms"></param>
            public static void BindetName(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT etName FROM B_ExtraType";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }

            /// <summary>
            /// 绑定生产厂家 2013-8-29 张
            /// </summary>
            /// <param name="cms"></param>
            public static void BindManufactuer(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "         IF EXISTS(  SELECT TOP 1 1 FROM dbo.B_ConfigureSys WHERE SetName = 'C_ShowsfShortName' AND SetValue = 1) "
                    + " SELECT sfShortName AS sfName ,sfID FROM dbo.B_SupplierFile WHERE sfIsFactory = 1 "
                    + " ELSE SELECT sfName ,sfID FROM dbo.B_SupplierFile WHERE sfIsFactory = 1";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i]["sfName"] .ToString());
                        cms .Items[i] .Tag = dt .Rows[i]["sfID"] .ToString();
                  }
                  cms .Items .Add("");
                  cms .Items[cms .Items .Count - 1] .Tag = 0;
            }
            /// <summary>
            /// 绑定结算方式到右键菜单 2012-11-13新增 张
            /// </summary>
            /// <param name="cms"></param>
            public static void BindSettleWay(FZYK .WinCtrl .YKContextMenuStrip cms)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT swName FROM B_SettleWay";
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }

            #endregion
      }
}
