using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using FZYK .Com;
using System .Data;

namespace FZYK .Nest .db
{
      public class NestDB
      {
            /// <summary>
            /// 保存设置
            /// </summary>
            /// <param name="dic"></param>
            /// <returns></returns>
            public int SaveSet(Dictionary<string, string> dic)
            {
                  string sql = " delete from N_NestSet ";
                  foreach (var item in dic)
                  {
                        sql += " insert into N_NestSet (nsName,nsValue) values";
                        sql += "('" + item .Key + "','" + item .Value + "') ";
                  }
                  return YKDataClass .SqlCommand(sql);
            }
            /// <summary>
            /// 获取设置
            /// </summary>
            /// <returns></returns>
            public Dictionary<string, string> GetSet()
            {
                  Dictionary<string, string> dic = new Dictionary<string, string>();
                  string sql = " select nsName,nsValue from N_NestSet";
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  for (int i = 0; i < dt .Rows .Count; i++)
                  {
                        dic .Add(dt .Rows[i]["nsName"] .ToString(), dt .Rows[i]["nsValue"] .ToString());
                  }
                  return dic;
            }
      }
}
