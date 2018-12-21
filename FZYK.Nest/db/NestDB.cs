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
            /// <summary>
            /// 获取库存
            /// </summary>
            /// <param name="lName"></param>
            /// <param name="mfSpec"></param>
            /// <returns></returns>
            public DataTable GetStock(string etname, string lName, string mfSpec)
            {
                  string sql = " select lName,mfSpec,sLength,sWidth,sum(sCount) as sCount "
                        + " from W_Stock s inner join B_MaterialFile mf on mf.mfID = s.mfID"
                        + " where tag = '原材料' and etname = '" + etname + "' and lName = '" + lName + "' AND mfSpec = '" + mfSpec + "'"
                        + " group by lName,mfSpec,sLength,sWidth";
                  return YKDataClass .getDataTable(sql);
            }
            /// <summary>
            /// 获取材质规格列表
            /// </summary>
            /// <returns></returns>
            public DataSet GetStockList()
            {
                  string sql = "";
                  sql += " SELECT s.etName,s.lName,mf.mfSpec INTO #s FROM dbo.W_Stock s INNER JOIN dbo.B_MaterialFile mf ON s.mfID = mf.mfID "
                        + " WHERE mf.mfName = '钢板' GROUP BY mf.mfSpec,s.etName,s.lName "
                        + " ORDER BY s.etName,s.lName,mf.mfSpec "
                        + " SELECT etName FROM #s GROUP BY etName ORDER BY etName "
                        + " SELECT lName FROM #s GROUP BY lName ORDER BY CASE WHEN lName LIKE 'Q%' THEN 0 ELSE 1 END,lName "
                        + " SELECT mfSpec FROM #s GROUP BY mfSpec ORDER BY dbo.fSortNumber(mfSpec) "
                        + " DROP TABLE #s ";
                  return YKDataClass .Query(sql);
            }
      }
}
