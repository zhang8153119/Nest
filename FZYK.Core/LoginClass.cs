using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using FZYK.Com;
using System.Xml;
using System.Collections;
namespace FZYK.Core
{
   public  class LoginClass
    {

        /// <summary>
        /// 检查用户名登录
        /// </summary>
        /// <param name="uname">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static DataSet checkuser(string uname, string pass)
        {

            string md5Pass = UserMd5(pass);
            string sqlstr = "select a.*,b.dNum,b.dName FROM OE_Employee a  INNER JOIN B_Dept b ON a.dID=b.dID where "
               + " ( eNum='" + uname + "' or eName='" + uname + "') and (ePassword='" + pass + "' or ePassword='" + md5Pass + "') "
               + " ORDER BY ISNULL(a.eValid,1) DESC";
            DataSet ds = FZYK.Com.YKDataClass.Query(sqlstr);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            else
            {
                if (!ds.Tables[0].Rows[0]["ePassword"].ToString().Equals(md5Pass))
                {
                    sqlstr = "update OE_Employee set ePassword='" + md5Pass + "' where eID='" + ds.Tables[0].Rows[0]["eID"].ToString() + "'";
                    FZYK.Com.YKDataClass.SqlCommand(sqlstr);
                }
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    FZYK.Com.UserInfo.eID = Convert.ToInt32(dt.Rows[0]["eID"]);
                    FZYK.Com.UserInfo.eNum = dt.Rows[0]["eNum"].ToString();
                    FZYK.Com.UserInfo.dID = Convert.ToInt32(dt.Rows[0]["dID"]);
                    FZYK.Com.UserInfo.dNum = dt.Rows[0]["dNum"].ToString();
                    FZYK.Com.UserInfo.dName = dt.Rows[0]["dName"].ToString();
                    FZYK.Com.UserInfo.eName = dt.Rows[0]["eName"].ToString();
                    FZYK.Com.UserInfo.eIBase = dt.Rows[0]["eIBase"].ToString();
                    FZYK.Com.UserInfo.eValid = Convert.ToString(dt.Rows[0]["eValid"]);
                    //FZYK.Com.UserInfo.userInfo.eNum = DevCommon.getSubDDNum();
                    FZYK.Com.UserInfo.htRights = GetUserRights(FZYK.Com.UserInfo.eID, false);
                    FZYK.Com.UserInfo.configSys = GetConfigureSys();
                    //DevCommon.insertLog(FZYK.Com.UserInfo.userInfo.eNum, DevCommon.userInfo.E_Name);
                    FZYK.Com.UserInfo.xmlName = GetdNameXMLFile(FZYK.Com.UserInfo.dName);
                    FZYK.Core.YKControlText.InitControlText();
                    Hashtable ht = new Hashtable();
                    ht.Add("@eID", Convert.ToInt32(dt.Rows[0]["eID"]));
                    FZYK.Com.UserInfo.dsMessage = YKDataClass.RunSqlProcForDataSet("SM_GetMessage", ht);
                    FZYK.Com.UserInfo.dsOALimit = GetLimitDate(FZYK.Com.UserInfo.eNum);
                 }
                return ds;
            }
        }

        /// <summary>
        /// 得到到期提醒的数据
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLimitDate(string eNum)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            #region 得到培训证书到期数据
            sbSql.Append("if exists(select 1 from B_EmployeeAuthorityView where eaIAudit=1 and eNum='" + eNum + "' and alName='OE_ProfessionCertificateValid')");
            sbSql.Append(" select * from OE_EmployeeProfessionCertificateView where pccWarmDay>=0 and ((epValidDate<=dateadd(day,pccWarmDay,getdate()) and epValidDate>=convert(varchar(10),getdate(),120))");
            sbSql.Append("  or (epCheckDate<=dateadd(day,pccWarmDay,getdate()) and epCheckDate>=convert(varchar(10),getdate(),120)))");
            dt = YKDataClass.getDataTable(sbSql.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.TableName = "OE_ProfessionCertificateValid";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["epCertificationDate"] != null && dr["epCertificationDate"] != DBNull.Value && dr["epCertificationDate"].ToString() != "" && Convert.ToDateTime(dr["epCertificationDate"].ToString()).Year == 1900)
                        dr["epCertificationDate"] = DBNull.Value;
                    if (dr["epCheckDate"] != null && dr["epCheckDate"] != DBNull.Value && dr["epCheckDate"].ToString() != "" && Convert.ToDateTime(dr["epCheckDate"].ToString()).Year == 1900)
                        dr["epCheckDate"] = DBNull.Value;
                    if (dr["epValidDate"] != null && dr["epValidDate"] != DBNull.Value && dr["epValidDate"].ToString() != "" && Convert.ToDateTime(dr["epValidDate"].ToString()).Year == 1900)
                        dr["epValidDate"] = DBNull.Value;
                    if (dr["epUploadDate"] != null && dr["epUploadDate"] != DBNull.Value && dr["epUploadDate"].ToString() != "" && Convert.ToDateTime(dr["epUploadDate"].ToString()).Year == 1900)
                        dr["epUploadDate"] = DBNull.Value;
                    if (dr["WriteDate"] != null && dr["WriteDate"] != DBNull.Value && dr["WriteDate"].ToString() != "" && Convert.ToDateTime(dr["WriteDate"].ToString()).Year == 1900)
                        dr["WriteDate"] = DBNull.Value;
                    if (dr["LockDate"] != null && dr["LockDate"] != DBNull.Value && dr["LockDate"].ToString() != "" && Convert.ToDateTime(dr["LockDate"].ToString()).Year == 1900)
                        dr["LockDate"] = DBNull.Value;
                }
                ds.Tables.Add(dt);
            }
            #endregion
            #region 转正提醒,提醒本人
            sbSql.Clear();
            sbSql.Append("SELECT ");
            //sbSql.Append("dbo.fSumWorkAgeHavingLeave(eID,2)workAge,");
            sbSql.Append("null as workAge,");
            sbSql.Append("dbo.fGetPY(OEEV.eName) as py,OEEV.[eID],OEEV.[dID],OEEV.[dNum],OEEV.[dName],OEEV.[eNum],OEEV.[eName],OEEV.[eCertificate],OEEV.[ePassword],OEEV.[eIDCard],OEEV.[eIBase],OEEV.[eSex],OEEV.[eValid],OEEV.[eTel],OEEV.[eMobile],OEEV.[eAddress],OEEV.[eBirthday],OEEV.[eIfMarry],OEEV.[eWorkValue],OEEV.[eCollege],OEEV.[eMemo],OEEV.[eEducation],OEEV.[ePostitle],OEEV.[eNation],OEEV.[eNativePlace],OEEV.[eDegree],OEEV.[eSpecialized],OEEV.[eEnterTime],OEEV.[eLevel] ");
            sbSql.Append(",eIfManager,eLeaveTime,eInsureNum,replace(dbo.fSumWorkAge(eBirthday,getdate(),0),'年','岁')eAge");
            sbSql.Append(",eisID,eJoinDate,eTran,eRetireMoney,eRetireDate,eCode,eIfTwoWorker,eCompactLimit1,eCompactLimit2,eCompactLimit3");
            sbSql.Append(",eGroupWorkAge,eHKCompany,eZQCompany,eSkillNum,eSmallGroupID,eSmallGroup,eBanZuID,eTryLimit");
            sbSql.Append(" FROM OE_EmployeeView OEEV");
            sbSql.Append(" where eID=" + Com.UserInfo.eID + " and ISNULL(eTryLimit,0)>0 and DATEADD(month,ISNULL(eTryLimit,0),eEnterTime)>DATEADD(day,-7,getdate())");
            DataTable dtPosttive = YKDataClass.getDataTable(sbSql.ToString());
            if (dtPosttive != null && dtPosttive.Rows.Count > 0)
            {
                dtPosttive.TableName = "OE_EmployeePosttive";
                ds.Tables.Add(dtPosttive);
            }
            #endregion
            #region 保险提醒
            #endregion
            return ds;
        }
        public static string GetdNameXMLFile(string dName)
        {
            string sqlstr = "SELECT TOP  1 [value] FROM B_CommonForCombox cfc WHERE ISNULL(cfc.valid,1)=1 AND cfc.setMemo='" + dName + "' AND cfc.setName='B_dNameXMLFile'";
            object o = YKDataClass.GetSingle(sqlstr);
            if (o != null)
                return o.ToString();
            return "";

        }
       public static void InsertUserIP(int eID,string ip,int port)
       {
           string sqlstr = "UPDATE B_UserIP SET Valid = 0 WHERE Valid=1 AND eID=" + eID + "; INSERT INTO B_UserIP(eID,buIP,buPort,buDate,Valid) VALUES(" + eID + ",'" + ip + "'," + port + ",GETDATE(),1)";
           YKDataClass.SqlCommand(sqlstr);
       }
       public static void UpdateUserIP(int eID)
       {
           string sqlstr = "UPDATE B_UserIP SET Valid = 0 WHERE Valid=1 AND eID=" + eID;
           YKDataClass.SqlCommand(sqlstr);
       }
        /// <summary>
        /// 用户登陆时将所有系统设置填充到UserInfo中
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetConfigureSys()
        {
            DataTable dt = FZYK.Com.YKDataClass.getDataTable("SELECT ISNULL(SetName,'') AS SetName,ISNULL(SetValue,'') AS SetValue FROM B_ConfigureSys");
            Dictionary<string, string> _configSys = new Dictionary<string, string>(2);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (!_configSys.ContainsKey(row["SetName"].ToString()))
                        _configSys.Add(row["SetName"].ToString(), row["SetValue"].ToString());
                }
                return _configSys;
            }
            else
                return null;
        }
         /// <summary>
        /// 根据员工编号获取权限信息
        /// </summary>
        /// <param name="eNum">员工编号</param>
        /// <param name="isReadOnly">是否只读。当访问的是结存的数据库是，只有查看权限</param>
        /// <returns></returns>
        public static Dictionary<string, Rights> GetUserRights(int eID, bool isReadOnly)
        {
            Dictionary<string, Rights> dic = new Dictionary<string, Rights>(2);
            DataTable dt1 = GetAuthorityByRole(eID);
            foreach (DataRow row1 in dt1.Rows)
            {
                if (isReadOnly)
                {
                    row1["braIEdit"] = 0;
                    row1["braIAudit"] = 0;
                    row1["braIApprove"] = 0;
                }
                if (!dic.ContainsKey(Convert.ToString(row1["alName"])))
                    dic.Add(Convert.ToString(row1["alName"]), new Rights(Convert.ToInt16(row1["braISee"]), Convert.ToInt16(row1["braIEdit"]), Convert.ToInt16(row1["braIAudit"]), Convert.ToInt16(row1["braIApprove"])));
                else
                {
                    Rights r = new Rights(Convert.ToInt16(row1["braISee"]), Convert.ToInt16(row1["braIEdit"]), Convert.ToInt16(row1["braIAudit"]), Convert.ToInt16(row1["braIApprove"]));
                    if (r.ISee)
                        dic[Convert.ToString(row1["alName"])].ISee = r.ISee;
                    if (r.IEdit)
                        dic[Convert.ToString(row1["alName"])].IEdit = r.IEdit;
                    if (r.IAudit)
                        dic[Convert.ToString(row1["alName"])].IAudit = r.IAudit;
                    if (r.IApprove)
                        dic[Convert.ToString(row1["alName"])].IApprove = r.IApprove;
                }
            }

            DataTable dt = GetAuthority(eID);
            foreach (DataRow row in dt.Rows)
            {
                if (isReadOnly)
                {
                    row["eaIEdit"] = 0;
                    row["eaIAudit"] = 0;
                    row["eaIApprove"] = 0;
                }
                if (!dic.ContainsKey(Convert.ToString(row["alName"])))
                    dic.Add(Convert.ToString(row["alName"]), new Rights(Convert.ToInt16(row["eaISee"]), Convert.ToInt16(row["eaIEdit"]), Convert.ToInt16(row["eaIAudit"]), Convert.ToInt16(row["eaIApprove"])));
            }
           
            return dic;
        }
        public static DataTable GetAuthorityByRole(int eID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT alv.mID, alv.alID, alv.mName, alv.mTab, alv.alName, alv.alClass, alv.alText,");
            sb.Append(" alv.alRemark, alv.alValid, a.braISee, a.braIEdit, a.braIAudit,a.braIApprove");
            sb.Append(" FROM B_AuthorityListView alv INNER JOIN (");
            sb.Append(" SELECT bra.alID, bra.braISee, bra.braIEdit, bra.braIAudit, bra.braIApprove");
            sb.Append(" FROM B_RoleAuthority bra INNER JOIN B_RoleEmployee bre ON bre.brID = bra.brID");
            sb.Append(" WHERE bre.eID=" + eID + ") a");
            sb.Append(" ON a.alID = alv.alID");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sb.ToString());
            return dt;
        }
        /// <summary>
        /// 根据员工编号获取权限信息
        /// </summary>
        /// <param name="eNum">员工编号</param>
        /// <returns>权限列表</returns>
        public static DataTable GetAuthority(int eID)
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append("SELECT eav.mID,eav.mName,eav.mTab,eav.alID,eav.alName,eav.alClass,eav.alText,eav.alRemark,eav.alValid");
            sqlstr.Append(",eav.eNum,eav.eaISee,eav.eaIEdit,eav.eaIAudit,eav.eaIApprove ");
            sqlstr.Append("FROM B_EmployeeAuthorityView eav WHERE eav.eNum=(SELECT eNum FROM OE_Employee WHERE eID=" + eID + ")");
            DataTable dt = FZYK.Com.YKDataClass.getDataTable(sqlstr.ToString());
            return dt;
        }
        /// <summary>
        /// md5加密算法
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static string UserMd5(string str1)
        {
            string cl1 = str1;
            string pwd = "";
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组 
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得 
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;
        }

        /// <summary>
        /// 登陆框中获取服务器和数据库下拉框 
        /// </summary>
        /// <returns></returns>
        public static DataSet getDBForCmb(string startUrl,string theUrl)
        {
            DataSet ds = new DataSet();
            #region 获取初始连接字符串
            try
            {
                XmlDocument xmlIPDoc = new XmlDocument();
                xmlIPDoc.Load(startUrl + "\\UpdateList.xml");
                if (theUrl == "url2")
                {
                    XmlNode xsDBConnect = xmlIPDoc.SelectSingleNode("//sDBConnect2");
                    if (xsDBConnect != null)
                    {
                          //YKDataClass .ConnectionString = FZYK .Com .YKSysCom .Decrypt(xsDBConnect .InnerText);
                          YKDataClass .ConnectionString = xsDBConnect .InnerText;
                    }
                    else
                    {
                        XmlNode xDBConnect = xmlIPDoc.SelectSingleNode("//DBConnect2");
                        if (xDBConnect != null)
                            YKDataClass.ConnectionString = xDBConnect.InnerText;//数据库连接字符串
                    }
                }
                else
                {
                    XmlNode xsDBConnect = xmlIPDoc.SelectSingleNode("//sDBConnect");
                    if (xsDBConnect != null)
                    {
                          //YKDataClass .ConnectionString = FZYK .Com .YKSysCom .Decrypt(xsDBConnect .InnerText);
                          YKDataClass .ConnectionString = xsDBConnect .InnerText;
                    }
                    else
                    {
                        XmlNode xDBConnect = xmlIPDoc.SelectSingleNode("//DBConnect");
                        if (xDBConnect != null)
                            YKDataClass.ConnectionString = xDBConnect.InnerText;//数据库连接字符串
                    }
                }
               
                string sqlstr = "select * from IPRecord;select * from DBrecord order by id desc;select convert(char(10),getdate(),120); ";
                ds = YKDataClass.Query(sqlstr);

                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["isReadOnly"] != null && ds.Tables[0].Rows[i]["isReadOnly"].ToString().Equals("1"))
                            ds.Tables[0].Rows[i]["ServerIP"] = ds.Tables[0].Rows[i]["ServerIP"] + "(ReadOnly)";
                    }
                }
            #endregion
            }
            catch (Exception) { }
            return ds;
        }
       /// <summary>
       /// 根据类名返回控件text属性配置
       /// 添加时间：2012年8月1日 09:17:05  添加人：AMo
       /// </summary>
       /// <param name="ClassName">类名</param>
       /// <returns></returns>
        public static DataTable GetControlText(string ClassName)
        {
              //try
              //{
              //      Hashtable ht = new Hashtable(2);
              //      ht .Add("@ClassName", ClassName);
              //      string sqlstr = "SELECT ClassName,ControlName,ControlText FROM B_ControlText WHERE ClassName=@ClassName";
              //      return YKDataClass .ExecuteParameterSqlForDatatable(sqlstr, ht);
              //}
              //catch (Exception ex)
              //{
              //      YKMessageBox .ShowBoxException(ex .Message, ex);
              //      return null;
              //}
              return null;
        }
    }
}
