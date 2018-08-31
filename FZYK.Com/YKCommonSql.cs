using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Data;
using System .Collections;
using System .Net;
using FZYK .Com .Msg;
using System .Net .Sockets;
using System .Net .NetworkInformation;
using System .Threading;
using System .IO;
namespace FZYK .Com
{
      public class YKCommonSql
      {
            /// <summary>
            /// 得到电子公告信息
            /// </summary>
            /// <param name="eID"></param>
            /// <returns></returns>
            public static DataSet GetNewNotify(int eID)
            {
                  Hashtable ht = new Hashtable();
                  ht .Add("@eID", eID);
                  return YKDataClass .RunSqlProcForDataSet("SM_GetMessage", ht);
            }

            /// <summary>
            /// 获取tag1值 2014-4-23 zhang
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string GetTag1(string tableName, string key, int value)
            {
                  string sql = " SELECT Tag1 FROM " + tableName + " WHERE " + key + " = " + value .ToString();
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  if (dt .Rows .Count > 0)
                  {
                        return dt .Rows[0][0] .ToString();
                  }
                  else
                  {
                        return "";
                  }
            }
            /// <summary>
            /// 开启系统启动连接、关闭连接
            /// </summary>
            /// <param name="eID">员工ID</param>
            /// <param name="message">发送信息</param>
            /// <returns></returns>
            public static bool SendLoginAndClose(string Type)
            {
                  try
                  {
                        if (Type == "Login")
                        {
                              string ServerIP = (UserInfo .configSys .ContainsKey("B_FTPIP")) ? UserInfo .configSys["B_FTPIP"] .ToString() : "";
                              if (ServerIP == "")
                              {
                                    return false;
                              }
                              if (Com .UserInfo .sockClient != null)
                                    Com .UserInfo .sockClient .Dispose();
                              Com .UserInfo .sockClient = new Socket(AddressFamily .InterNetwork, SocketType .Stream, ProtocolType .Tcp);
                              IPAddress address = IPAddress .Parse(Com .UserInfo .UserIP);
                              IPEndPoint LocalendPoint = new IPEndPoint(address, Com .UserInfo .UserPort);
                              Com .UserInfo .sockClient .Bind(LocalendPoint);
                              Com .UserInfo .sockClient .Connect(IPAddress .Parse(ServerIP), 9000);
                        }
                        YKClassMsg msg = new YKClassMsg();
                        msg .SID = Convert .ToString(Com .UserInfo .eID);
                        msg .msgID = Type;
                        byte[] Data = new YKClassSerializers() .SerializeBinary(msg) .ToArray();
                        if (Com .UserInfo .sockClient != null && Com .UserInfo .sockClient .Connected)
                        {
                              Com .UserInfo .sockClient .Send(Data);
                              if (Type == "Close")
                                    Com .UserInfo .sockClient .Close();
                        }
                        return true;
                  }
                  catch (SocketException e)
                  {
                        return false;
                  }
            }
            /// <summary>
            /// 判断是否与服务器连接
            /// </summary>
            /// <returns></returns>
            public static bool IfConnect()
            {
                  string ServerIP = (UserInfo .configSys .ContainsKey("B_FTPIP")) ? UserInfo .configSys["B_FTPIP"] .ToString() : "";
                  Ping ping = new Ping();

                  PingReply reply = ping .Send(ServerIP);
                  if (reply .Status == IPStatus .Success)
                  {
                        return true;
                  }
                  else // DISCONNECTED
                  {
                        return false;
                  }
            }

            ///// <summary>
            ///// 即时发送
            ///// </summary>
            ///// <param name="eID">员工ID</param>
            ///// <param name="message">发送信息</param>
            ///// <returns></returns>
            //public static bool Send(int eID, string message, string Type)
            //{
            //      return true;
            //      //try
            //      //{
            //      //      string ServerIP = (UserInfo .configSys .ContainsKey("B_FTPIP")) ? UserInfo .configSys["B_FTPIP"] .ToString() : "";
            //      //      if (ServerIP == "")
            //      //      {
            //      //            return false;
            //      //      }
            //      //      DataTable dtIP = GetEmployeeIP(eID);
            //      //      DataTable dtSelfPort = GetEmployeeIP(Com .UserInfo .eID);
            //      //      string MySelfPort = dtSelfPort .Rows[0]["buPort"] .ToString();
            //      //      if (dtIP != null && dtIP .Rows .Count > 0)
            //      //      {
            //      //            if (dtIP .Rows[0]["buIP"] .ToString() != "")
            //      //            {
            //      //                  YKClassMsg msg = new YKClassMsg();
            //      //                  msg .Data = Encoding .Unicode .GetBytes(message);
            //      //                  msg .RID = Convert .ToString(eID);
            //      //                  msg .RIP = dtIP .Rows[0]["buIP"] .ToString();
            //      //                  msg .RPort = dtIP .Rows[0]["buPort"] .ToString();
            //      //                  msg .SID = Convert .ToString(Com .UserInfo .eID);
            //      //                  msg .SIP = YKGetClient .GetIP(); ;
            //      //                  msg .SPort = MySelfPort;
            //      //                  msg .msgID = Type;
            //      //                  Com .YKGetClient .Send(IPAddress .Parse(ServerIP), 9000, new YKClassSerializers() .SerializeBinary(msg) .ToArray());

            //      //            }
            //      //      }
            //      //      return true;
            //      //}
            //      //catch (Exception e)
            //      //{
            //      //      throw e;
            //      //}
            //}
            /// <summary>
            /// 得到在线人员IP
            /// </summary>
            /// <param name="eID"></param>
            /// <returns></returns>
            public static DataTable GetEmployeeIP(int eID)
            {
                  StringBuilder sbSql = new StringBuilder("SELECT ");
                  sbSql .Append("buIP,buPort FROM B_USERIP where eID=" + eID + " and Valid=1");
                  return YKDataClass .getDataTable(sbSql .ToString());
            }
            /// <summary>
            /// 得到企业信息
            /// </summary>
            /// <returns></returns>
            public static DataTable GetFactoryName()
            {
                  string sql = "select * from B_factoryname";
                  return YKDataClass .getDataTable(sql);
            }
            /// <summary>
            /// 获取单重 
            /// </summary>
            /// <param name="cainame">材料</param>
            /// <param name="guige">规格</param>
            /// <param name="ycount">数量 传1</param>
            /// <param name="length">长</param>
            /// <param name="width">宽</param>
            /// <param name="outRound">厚度</param>
            /// <param name="inRound">传0</param>
            /// <param name="BianShu">边数</param>
            /// <param name="FLWay">法兰类型 传0</param>
            /// <returns></returns>
            public static double GetPerWeight(string cainame, string guige, string ycount, string length, string width, string outRound, string inRound, string BianShu, string FLWay)
            {
                  try
                  {
                        DataTable dt = new DataTable();

                        //理论重量=数量*比重
                        StringBuilder sql = new StringBuilder();
                        sql .Append(" SELECT dbo.F_fGetPerWeight('" + cainame + "','" + guige + "','" + ycount
                        + "','" + length + "','" + width + "','" + outRound + "','" + inRound + "','" + BianShu + "','" + FLWay + "')");
                        object perweight = FZYK .Com .YKDataClass .SqlCommandForObject(sql .ToString());
                        if (perweight != System .DBNull .Value)
                              return Convert .ToDouble(perweight .ToString());
                        else
                              return 0.00;
                  }
                  catch (Exception) { return 0.00; }
            }
            /// <summary>
            /// 获取库存管理方式
            ///3-20 增加 zhang
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public static int GetStockWay(string tag)
            {
                  try
                  {
                        string sql = "";
                        if (tag == "原材料")
                        {
                              sql = "SELECT SetValue FROM dbo.B_ConfigureSys WHERE SetName = 'W_SteelNeedBarCode'";
                        }
                        else if (tag == "辅材")
                        {
                              sql = "SELECT SetValue FROM dbo.B_ConfigureSys WHERE SetName = 'W_HardwareNeedBarCode'";
                        }
                        else if (tag == "螺栓")
                        {
                              sql = "SELECT SetValue FROM dbo.B_ConfigureSys WHERE SetName = 'W_BoltNeedBarCode'";
                        }
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return Convert .ToInt32(dt .Rows[0][0] .ToString());
                        }
                        else
                        {
                              return -1;
                        }
                  }
                  catch
                  {
                        return -1;
                  }
            }
            /// <summary>
            /// 获取@值 2013-3-14 zhang
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string GetTag(string tableName, string key, int value)
            {
                  string sql = " SELECT Tag FROM " + tableName + " WHERE " + key + " = " + value .ToString();
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  if (dt .Rows .Count > 0)
                  {
                        return dt .Rows[0][0] .ToString();
                  }
                  else
                  {
                        return "";
                  }
            }
            /// <summary>
            /// 获取服务器时间
            /// </summary>
            /// <returns></returns>
            public static string getServerTime()
            {
                  string sql = "select convert(varchar(50),getdate(),120)";
                  try
                  {
                        return YKDataClass .GetSingle(sql) .ToString();
                        //Service.Service service = new CS2008App.Service.Service();
                        //service.Url = Properties.Settings.Default.CS2008App_Service_Service;
                        //return service.Query(sql).Tables[0].Rows[0].ToString();
                  }
                  catch (Exception) { return DateTime .Now .ToString(); }
            }

            /// <summary>
            /// 获取一张表的所有字段及类型长度
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <returns></returns>
            public static DataTable getTableColumns(string tableName)
            {
                  string sql = "select a.name AS colName,a.length,b.name from syscolumns a INNER JOIN systypes b "
           + "  ON a.xusertype=b.xusertype where a.id=object_id('" + tableName + "') ";
                  return YKDataClass .getDataTable(sql);
            }

            /// <summary>
            /// 删除某张表某行
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="fieldName">主键字段名</param>
            /// <param name="fieldValue">主键值</param>
            /// <returns></returns>
            public static int DeleteTableRow(string tableName, string fieldName, string fieldValue)
            {
                  string sql = " delete from " + tableName + " where " + fieldName + "='" + fieldValue + "'";
                  return YKDataClass .SqlCommand(sql);
            }

            /// <summary>
            /// 设置某行数据无效
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="validFieldName">是否有效的字段名</param>
            /// <param name="fieldName">主键字段名</param>
            /// <param name="fieldValue">主键值</param>
            /// <returns></returns>
            public static int SetRowUnValid(string tableName, string validFieldName, bool ifTrueOrFalse, string fieldName, string fieldValue)
            {
                  string sql = " update " + tableName + " set " + validFieldName + "=" + ifTrueOrFalse .CompareTo(false) + " where " + fieldName + "='" + fieldValue + "'";
                  return YKDataClass .SqlCommand(sql);
            }

            /// <summary>
            /// 得到最大ID(没有Parent的情况) ,位数自定，2013-2-2修改 张
            /// </summary>
            public static string GetMaxIdNoParentCount(string tableName, string filedName, int Count)
            {
                  string sql = "declare @num varchar(" + Count + "); select @num= right('0000000000',"
                        + Count + " -LEN(cast(MAX(" + filedName + ")+1 as char(" + Count .ToString() + "))))"
                      + "+ cast(MAX(" + filedName + ")+1 as char(" + Count .ToString() + "))  from "
                      + tableName + " where  isnumeric(" + filedName + ")=1 AND LEN(" + filedName + ") = " + Count .ToString()
                      + " ;if @num is null set @num=right('0000000000'," + Convert .ToString(Count - 1) + ")+'1';select @num";
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  if (dt .Rows .Count > 0)
                  {
                        return dt .Rows[0][0] .ToString();
                  }
                  else
                  {
                        return "";
                  }
            }
            /// <summary>
            /// 得到最大ID(有Parent的情况) ,位数自定，2013-2-2修改 张
            /// </summary>
            public static string GetMaxIdNoParentCount(string tableName, string filedName, string parent, int Count)
            {
                  //Count = Count + parent.Length;
                  int sumCount = Count + parent .Length;
                  if (parent == "")
                        return GetMaxIdNoParentCount(tableName, filedName, Count);
                  string sql = "declare @num varchar(" + sumCount + "); select @num='" + parent
                        + "' +right('0000000000'," + Count + " - LEN(cast(MAX(substring(" + filedName + ","
                        + Convert .ToString(parent .Length + 1) + ",len(" + filedName + ")-" + parent .Length + "))+1 as char(" + sumCount + "))))"
                      + "+ cast(MAX(substring(" + filedName + "," + Convert .ToString(parent .Length + 1)
                      + ",len(" + filedName + ")-" + parent .Length + "))+1 as char(" + sumCount + "))  from "
                      + tableName + " where  isnumeric(" + filedName + ")=1 and " + filedName + " like '" + parent + "%' AND LEN(" + filedName + ") = LEN('" + parent + "')+" + Count .ToString() + " ;if @num is null set @num='" + parent + "'+right('0000000000'," + Convert .ToString(Count - 1) + ")+'1';select @num";
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  if (dt .Rows .Count > 0)
                  {
                        return dt .Rows[0][0] .ToString();
                  }
                  else
                  {
                        return "";
                  }
            }
            /// <summary>
            /// 进行单据锁定
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="col1name"></param>
            /// <param name="col1"></param>
            /// <param name="col2name"></param>
            /// <param name="col2"></param>
            /// <param name="idname"></param>
            /// <param name="ID"></param>
            /// <returns></returns>
            public static bool setAudiorApprover(string tableName, string col1name, string col1, string col2name, string col2, string idname, int ID)
            {
                  string sql = "UPDATE " + tableName + " set " + col1name + "='" + col1 + "'," + col2name + "='" + col2 + "' where " + idname + "=" + ID;
                  if (YKDataClass .SqlCommand(sql) > 0)
                        return true;
                  else
                        return false;
            }
            /// <summary>
            /// 锁定与解锁
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="pkName">主键名</param>
            /// <param name="pkValue">主键值</param>
            /// <param name="Auditor">锁定人</param>
            /// <param name="AorU">1为锁定，-1为解锁</param>
            /// <returns></returns>
            public static int AuditAndUnAudit(string tableName, string pkName, int pkValue, string Auditor, int AorU)
            {
                  string sql = "";
                  if (AorU == 1)
                  {
                        sql = "UPDATE " + tableName + " SET Locker =  '" + Auditor + "' ,LockDate = GETDATE()  WHERE " + pkName + " = " + pkValue .ToString();
                  }
                  else if (AorU == -1)
                  {
                        sql = "UPDATE " + tableName + " SET Locker =  '' ,LockDate = '1900-1-1'  WHERE " + pkName + " = " + pkValue .ToString();
                  }
                  return YKDataClass .SqlCommand(sql);
            }
            /// <summary>
            /// 单据号自动编码
            /// 张镇波 2012年8月6日 08:56:02
            /// </summary>
            /// <param name="TableName"></param>
            /// <returns></returns>
            public static string AutoCodeByTable(string TableName, string CHN)
            {
                  Hashtable ht = new Hashtable();
                  ht .Add("@tableName", TableName);
                  ht .Add("@chn", CHN);
                  DataSet ds = new DataSet();
                  ds = YKDataClass .RunSqlProcForDataSet("AutoCodeByTableAndCHN", ht);
                  if (ds .Tables .Count > 0)
                        return ds .Tables[ds .Tables .Count - 1] .Rows[0][0] .ToString();
                  else
                        return "";
            }
            /// <summary>
            /// 获取仓库开始日期和结束日期 2012-8-30
            /// </summary>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <returns></returns>
            public static String[] GetStartAndEndDate(string year, string month)
            {
                  string[] array = new string[2];
                  int startDay = 0;
                  DataTable dt = new DataTable();
                  string dateStart = "";
                  string dateEnd = "";
                  string sql = "SELECT setValue FROM dbo.B_ConfigureSys  WHERE setName = 'W_MonthStartDay'";
                  dt = YKDataClass .getDataTable(sql);
                  if (dt .Rows .Count <= 0)
                  {
                        array[0] = "0";
                        array[1] = "0";
                        return array;
                  }
                  else
                  {
                        startDay = Convert .ToInt32(dt .Rows[0][0] .ToString());
                        if (startDay < 0)
                        {
                              startDay = -1 * startDay;
                              if (month == "1")
                              {
                                    dateStart = Convert .ToString(Convert .ToInt32(year) - 1) + "-12-" + startDay .ToString();
                                    dateEnd = Convert .ToDateTime(year + "-1-" + startDay .ToString()) .AddDays(-1) .ToString("yyyy-MM-dd");
                              }
                              else
                              {
                                    dateStart = year + "-" + Convert .ToString(Convert .ToInt32(month) - 1) + "-" + startDay .ToString();
                                    dateEnd = Convert .ToDateTime(year + "-" + month + "-" + startDay .ToString()) .AddDays(-1) .ToString("yyyy-MM-dd");
                              }
                        }
                        else
                        {
                              if (month == "12")
                              {
                                    dateStart = year + "-12-" + startDay .ToString();
                                    dateEnd = Convert .ToDateTime(Convert .ToString(Convert .ToInt32(year) + 1) + "-1-" + startDay .ToString()) .AddDays(-1) .ToString("yyyy-MM-dd");
                              }
                              else
                              {
                                    dateStart = year + "-" + month + "-" + startDay .ToString();
                                    dateEnd = Convert .ToDateTime(year + "-" + Convert .ToString(Convert .ToInt32(month) + 1) + "-" + startDay .ToString()) .AddDays(-1) .ToString("yyyy-MM-dd");
                              }
                        }
                        array[0] = dateStart;
                        array[1] = dateEnd;
                  }
                  return array;
            }
            /// <summary>
            /// 鄢国平 dgv刷新
            /// </summary>
            /// <param name="dtSource"></param>
            /// <param name="IDName"></param>
            /// <param name="ID"></param>
            /// <param name="flagAUD"></param>
            public static void RefreshDataSource(ref DataTable dtSource, DataTable dtChange, string IDName, int ID, int flagAUD)
            {

                  switch (flagAUD)
                  {
                        case 1:
                              if (dtChange != null && dtChange .Rows .Count > 0)
                              {
                                    if (dtSource == null)
                                          dtSource = dtChange .Clone();
                                    DataRow dr = dtSource .NewRow();
                                    for (int i = 0; i < dtSource .Columns .Count; i++)
                                    {
                                          dr[dtSource .Columns[i] .ColumnName] = dtChange .Rows[0][dtSource .Columns[i] .ColumnName];
                                    }
                                    dtSource .Rows .InsertAt(dr, 0);
                              }
                              break;
                        case 2:
                              if (dtChange != null && dtChange .Rows .Count > 0)
                              {
                                    DataRow[] dr = dtSource .Select(IDName + "=" + ID);
                                    if (dr .Length > 0)
                                    {
                                          for (int i = 0; i < dtSource .Columns .Count; i++)
                                          {
                                                dr[0][dtSource .Columns[i] .ColumnName] = dtChange .Rows[0][dtSource .Columns[i] .ColumnName];
                                          }
                                    }
                              }
                              break;
                        case 3:
                              DataRow[] row = dtSource .Select(IDName + "=" + ID);
                              if (row .Length > 0)
                                    dtSource .Rows .Remove(row[0]);
                              break;
                  }
                  dtSource .AcceptChanges();
            }
            /// <summary>
            /// 得到附件数量，鄢国平 2012-12-01
            /// </summary>
            /// <param name="nbID"></param>
            /// <returns></returns>
            public static int GetAttachmentNumber(string fcTable, string fcKey, int fcValue)
            {
                  string sqlStr = "select count(*) from OF_FileCatalog where fcTable='" + fcTable + "' and fcKey='" + fcKey + "'and fcValue=" + fcValue;
                  object obj = FZYK .Com .YKDataClass .SqlCommandForObject(sqlStr);
                  if (obj != null)
                        return Convert .ToInt32(obj);
                  else
                        return 0;
            }

            /// <summary>
            /// 获取审批记录 3-14 修改 zhang
            /// </summary>
            /// <param name="table"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static DataTable GetApproveRecord(string table, string key, int value)
            {
                  string sql = "SELECT bar.arID,bar.amID,bar.eID,bar.arTime,bar.arOpinion"
                  + " ,CASE bar.arPass WHEN 1 THEN '同意' ELSE '不同意' END AS arPass"
                  + " ,bar.flLevel,oe.eName"
                  + " FROM dbo.B_ApproveRecord AS bar INNER JOIN dbo.OE_Employee AS oe ON bar.eID = oe.eID"
                  + " INNER JOIN dbo.B_ApproveMain AS bam ON bar.amID = bam.amID"
                  + " WHERE bam.amKey = '" + key + "' AND bam.amTable = '" + table + "' AND bam.amValue = " + value .ToString()
                  + " AND arPass >= 0";
                  return YKDataClass .getDataTable(sql);
            }


            /// <summary>
            /// 获取最新的信息
            /// 张 2013-5-22
            /// </summary>
            /// <param name="eID"></param>
            /// <returns></returns>
            public static string GetNewestMessage(int eID)
            {
                  Hashtable ht = new Hashtable();
                  ht .Add("@eID", eID);
                  DataTable dt = new DataTable();
                  dt = YKDataClass .RunSqlProcForTable("B_GetApproveMainListApproveNewest", ht);
                  string message = "";
                  if (dt .Rows .Count > 0)
                  {
                        string count = dt .Rows .Count .ToString();
                        string amID = dt .Rows[0]["amID"] .ToString();
                        string writer = dt .Rows[0]["Writer"] .ToString() + " " + dt .Rows[0]["WriteTime"] .ToString() .Substring(0, 16);
                        string title = dt .Rows[0]["Title"] .ToString();

                        message = "Approve#" + count + "#" + amID + "#" + writer + "#" + title;
                  }
                  return message;
            }

            /// <summary>
            /// 根据名称和规格获取编码和ID
            /// </summary>
            /// <param name="mfName"></param>
            /// <param name="mfSpec"></param>
            /// <returns></returns>
            public static DataTable GetmfID(string mfName, string mfSpec)
            {
                  string sql = "SELECT mfID,mfNum,mfDensity FROM B_MaterialFile WHERE mfName = '" + mfName + "' AND mfSpec = '" + mfSpec + "'";
                  return YKDataClass .getDataTable(sql);
            }

            /// <summary>
            /// 获取单据的当前步骤
            /// 张镇波
            /// 2013-5-9 修改
            /// </summary>
            /// <param name="mName"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public static string GetApproveState(string mName, int level)
            {
                  string sql = " SELECT flName FROM dbo.B_FlowLevelView AS bflv WHERE mName = '" + mName + "' AND flLevel = " + level .ToString();
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  if (dt .Rows .Count > 0)
                  {
                        return dt .Rows[0][0] .ToString();
                  }
                  if (level > 0)
                  {
                        return "审批通过";
                  }
                  return "未提交";
            }

            /// <summary>
            /// 获取数据库字段的小数位数
            /// 张镇波 2013-5-15
            /// </summary>
            /// <param name="table"></param>
            /// <param name="column"></param>
            /// <returns></returns>
            public static int GetColumnsXscale(string table, string column)
            {
                  try
                  {
                        string sql = "SELECT c.xscale FROM sys.syscolumns AS c INNER JOIN sys.sysobjects AS o ON c.id = o.id"
                              + " WHERE  o.type = 'U' AND o.name ='" + table + "'  AND c.name = '" + column + "'";
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return Convert .ToInt32(dt .Rows[0][0] .ToString());
                        }
                        return -1;
                  }
                  catch
                  {
                        return -1;
                  }
            }

            /// <summary>
            /// 四舍五入,并格式化多余的0
            /// 张镇波 2013-5-15
            /// </summary>
            /// <param name="d"></param>
            /// <param name="decimals"></param>
            /// <returns></returns>
            public static decimal RoundYK(decimal d, int decimals)
            {
                  decimal number = Math .Round(d, decimals, MidpointRounding .AwayFromZero);
                  string numstr = number .ToString("0.###############");
                  return Convert .ToDecimal(numstr);
            }

            /// <summary>
            /// 获取导入导出格式
            /// 张镇波 2013-5-17
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            public static string GetExcelForm(string num)
            {
                  try
                  {
                        string sql = " SELECT TOP 1 efCode FROM dbo.B_ExcelForm AS bef WHERE efValid = 1 AND efNum = '" + num + "'";
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return dt .Rows[0][0] .ToString();
                        }
                        else
                        {
                              return "";
                        }
                  }
                  catch
                  {
                        return "";
                  }
            }

            /// <summary>
            /// 判断是否设置了审批流程
            /// </summary>
            /// <returns></returns>
            public static bool IfSetApproveFlow(string mName)
            {
                  try
                  {
                        string sql = "SELECT TOP 1 1 FROM dbo.B_FlowMain AS bfm WHERE mName = '" + mName + "'";
                        return YKDataClass .Exists(sql);
                  }
                  catch
                  {
                        return false;
                  }
            }

            /// <summary>
            /// 获取审批流程的最大等级
            /// 张镇波 2013-5-17
            /// </summary>
            /// <param name="mName"></param>
            /// <returns></returns>
            public static int GetFlowMaxLevel(string mName)
            {
                  try
                  {
                        string sql = "SELECT MAX(flLevel) FROM dbo.B_FlowLevelView WHERE mName = '" + mName + "'";
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return Convert .ToInt32(dt .Rows[0][0] .ToString());
                        }
                        else
                        {
                              return -1;
                        }
                  }
                  catch
                  {
                        return -1;
                  }
            }

            /// <summary>
            /// 获取菜单是否设置了审批流程
            /// 2013-8-5 张
            /// </summary>
            /// <param name="mName"></param>
            /// <returns></returns>
            public static bool NeedApprove(string mName)
            {
                  try
                  {
                        string sql = " SELECT TOP 1 1 FROM B_FlowMain WHERE mName = '" + mName + "'";
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return true;
                        }
                        else
                        {
                              return false;
                        }
                  }
                  catch
                  {
                        return false;
                  }
            }
            /// <summary>
            /// 获取出入库类型对应的结算方式，-99表示获取数据异常，-1表示不入账
            /// 张镇波 2013-5-22
            /// </summary>
            /// <param name="iotID"></param>
            /// <returns></returns>
            public static int GetInOutTypeSettle(int iotID)
            {
                  try
                  {
                        string sql = " SELECT iotSettle FROM B_InOutType WHERE iotID = " + iotID .ToString();
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return Convert .ToInt32(dt .Rows[0][0] .ToString());
                        }
                        else
                        {
                              return -99;
                        }
                  }
                  catch
                  {
                        return -99;
                  }
            }

            /// <summary>
            /// 判断是否显示分公司
            /// 张镇波 5-29
            /// </summary>
            /// <param name="mName"></param>
            /// <returns></returns>
            public static bool ShowCompany(string mName)
            {
                  string sql = " SELECT TOP 1 1 FROM B_ShowCompany WHERE scName = '" + mName + "'";
                  return YKDataClass .Exists(sql);
            }

            /// <summary>
            /// 获取导入导出格式说明
            /// 张镇波 2013-6-6
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            public static string GetExcelFormRemark(string num)
            {
                  try
                  {
                        string sql = " SELECT TOP 1 efRemark FROM dbo.B_ExcelForm AS bef WHERE efValid = 1 AND efNum = '" + num + "'";
                        DataTable dt = new DataTable();
                        dt = YKDataClass .getDataTable(sql);
                        if (dt .Rows .Count > 0)
                        {
                              return dt .Rows[0][0] .ToString();
                        }
                        else
                        {
                              return "";
                        }
                  }
                  catch
                  {
                        return "";
                  }
            }
            /// <summary>
            /// 获取下达单配料状态
            /// 张镇波 2013-6-7
            /// </summary>
            /// <param name="ptID"></param>
            /// <returns></returns>
            public static int GetProduceTaskState(int ptID)
            {
                  Hashtable ht = new Hashtable();
                  ht .Add("@ptID", ptID);
                  DataTable dt = new DataTable();
                  dt = YKDataClass .RunSqlProcForTable("W_GetProduceTaskState", ht);
                  if (dt == null || dt .Rows .Count == 0)
                  {
                        return 0;
                  }
                  else
                  {
                        return 1;
                  }
            }
            /// <summary>
            /// 获取分公司编码
            /// 张镇波 2013-6-19
            /// </summary>
            /// <param name="company"></param>
            /// <returns></returns>
            public static string GetCompanyCode(string company)
            {
                  string sql = " SELECT ccCode FROM B_CompanyCode WHERE ccName = '" + company + "'";
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  if (dt == null || dt .Rows .Count == 0)
                  {
                        return "";
                  }
                  else
                  {
                        return dt .Rows[0][0] .ToString();
                  }
            }
            /// <summary>
            /// 判断菜单的编码是否为只读
            /// 6-26 张
            /// </summary>
            /// <param name="CHN"></param>
            /// <returns></returns>
            public static bool AutoCodeIsReadOnly(string CHN)
            {
                  string sql = " SELECT TOP 1 1 FROM AutoCode WHERE A_TableCHN = '" + CHN + "' AND A_ReadOnly = 1";
                  return YKDataClass .Exists(sql);
            }
            /// <summary>
            /// 判断操作人是否存在流程中
            /// 2013-6-25 张
            /// </summary>
            /// <param name="table"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <param name="eID"></param>
            /// <returns></returns>
            public static bool InFlow(string table, string key, int value, int eID)
            {
                  string sql = " SELECT TOP 1 1 FROM dbo.B_ApproveMain AS bam INNER JOIN dbo.B_FlowApproveView AS bfa ON bam.fmID = bfa.fmID "
                        + " WHERE amTable = '" + table + "' AND amKey = '" + key + "' AND amValue = " + value + " AND bfa.eID = " + eID .ToString();
                  return YKDataClass .Exists(sql);
            }

            /// <summary>
            /// 获取规格转换值
            /// </summary>
            /// <param name="spec"></param>
            /// <returns></returns>
            public static decimal GetSpecValue(string spec)
            {
                  string sql = " SELECT dbo.fGetSpecValue('" + spec + "')";
                  DataTable dt = new DataTable();
                  dt = YKDataClass .getDataTable(sql);
                  return Convert .ToDecimal(dt .Rows[0][0]);
            }
      }
}
