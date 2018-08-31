using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Data;

namespace FZYK .Core
{
      public partial class ContextMenuStripBinder
      {
            /// <summary>
            /// 右键绑定库位（新）
            /// </summary>
            /// <param name="cms"></param>
            public static void BindgsNameNew(FZYK .WinCtrl .YKContextMenuStrip cms, int wID)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT DISTINCT gsName FROM W_Zone WHERE (1=1) ";
                  if (wID != 0)
                  {
                        sql += " AND wID = " + wID .ToString();
                  }
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }
            /// <summary>
            /// 右键绑定库位（新）
            /// </summary>
            /// <param name="cms"></param>
            public static void BindzNameNew(FZYK .WinCtrl .YKContextMenuStrip cms, int wID, string gsName)
            {
                  cms .Items .Clear();
                  DataTable dt = new DataTable();
                  string sql = "SELECT DISTINCT zName FROM W_Zone WHERE (1=1) ";
                  if (wID != 0)
                  {
                        sql += " AND wID = " + wID .ToString();
                  }
                  if (gsName != "")
                  {
                        sql += " AND gsName = '" + gsName + "'";
                  }
                  dt = FZYK .Com .YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        cms .Items .Add(dt .Rows[i][0] .ToString());
                  }
            }


      }
}
