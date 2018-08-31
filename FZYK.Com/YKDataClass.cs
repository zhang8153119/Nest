using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FZYK.Com
{
    ///<summary>
    ///���ݿ���ʻ�����(����SQLServer) 
    ///</summary>
    public class YKDataClass
    {
        ///<summary>���ݿ������ַ���</summary>

        public static string ConnectionString = "";

        #region ���÷���
        ///<summary>���һ���򿪵�����(ʹ�ú�ǵùر�)</summary>
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = 5;
            try
            {
                conn.Open();
                return conn;
            }
            catch
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
                return null;
            }
        }

        public static bool TestConnection()
        {
            using (SqlConnection conn = YKDataClass.Connection())
            {
                if (conn == null) return false;
                else return true;
            }
        }

        ///<summary>��ʼ��һ��SqlCommand�������</summary>
        ///<param name="cmd">SqlCommand����</param>
        ///<param name="conn">SqlConnection����</param>
        ///<param name="txn">SqlTransaction����</param>
        ///<param name="strSql">SQL���</param>
        ///<param name="par">SqlParameter����</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction txn, string strSql, SqlParameter[] par)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;
            cmd.Connection = conn;
            if (txn != null) cmd.Transaction = txn;
            if (par != null) foreach (SqlParameter pm in par) cmd.Parameters.Add(pm);
        }
        ///<summary>��ʼ��һ��SqlCommand�������</summary>
        ///<param name="cmd">SqlCommand����</param>
        ///<param name="conn">SqlConnection����</param>
        ///<param name="txn">SqlTransaction����</param>
        ///<param name="strSql">SQL���</param>
        ///<param name="par">Hashtable</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction txn, string strSql, Hashtable par)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;
            cmd.Connection = conn;
            if (txn != null) cmd.Transaction = txn;
            if (par != null)
                foreach (string tKey in par.Keys)
                {
                    SqlParameter sp = new SqlParameter(tKey, par[tKey]);
                    cmd.Parameters.Add(sp);
                }
        }
        ///<summary>��ʼ��һ��SqlCommand�������</summary>
        ///<param name="cmd">SqlCommand����</param>
        ///<param name="conn">SqlConnection����</param>
        ///<param name="txn">SqlTransaction����</param>
        ///<param name="procName">�洢��������</param>
        ///<param name="par">Hashtable</param>
        private static void PrepareCommand2(SqlCommand cmd, SqlConnection conn, SqlTransaction txn, string procname, Hashtable par)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procname;
            cmd.Connection = conn;
            if (txn != null) cmd.Transaction = txn;
            if (par != null)
                foreach (string tKey in par.Keys)
                {
                    SqlParameter sp = new SqlParameter(tKey, par[tKey]);
                    cmd.Parameters.Add(sp);
                }
        }
        /// <summary>
        /// ��һ�ű�������¼��,�����һ��
        /// </summary>
        /// <param name="FieldName">�ֶ���</param>
        /// <param name="TableName">����</param>
        /// <returns></returns>
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ") from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null) return 0;
            else return int.Parse(obj.ToString());
        }

        /// <summary>
        /// ��һ�ű������һ����¼������ֵNum
        /// </summary>
        /// <param name="FieldName">�ֶ���</param>
        /// <param name="TableName">����</param>
        /// <returns></returns>
        public static string GetMaxNum(string FieldName, string TableName)
        {
            string strsql = "select   " + FieldName + " from " + TableName + " order by " + FieldName + " desc";
            object obj = GetSingle(strsql);
            if (obj == null) return "";
            else return obj.ToString();
        }

        /// <summary>
        /// ��ѯһ����Ч�����Ƿ����,����,�ֶ���,ֵ������,
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static bool ifExistsValid(string Table, string Field, string value)
        {
            string strSql = "select  1 from " + Table + " where " + Field + "='" + value + "' and Valid=1";
            object obj = GetSingle(strSql);
            if (object.Equals(obj, null)) return false;
            else return true;
        }

        /// <summary>
        /// ��ѯһ����¼ĳ�ֶ��Ƿ���ڸ�ֵ,����,�ֶ���,ֵ������,
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static bool ifExists(string Table, string Field, string value)
        {
            string strSql = "select  1 from " + Table + " where " + Field + "='" + value + "' ";
            object obj = GetSingle(strSql);
            if (object.Equals(obj, null)) return false;
            else return true;
        }

        /// <summary>
        /// ��ѯһ�������Ƿ����
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// ��ѯ�Ƿ��ж�ĳtable���Ƿ����ĳ�ֶ�
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool ifExistField(string table, string fieldName)
        {
            string sql = "select 1 from syscolumns where id=object_id('" + table + "') and name='" + fieldName + "'";
            return Exists(sql);
        }
        #endregion

        #region ִ��Sql���

        ///<summary>ִ��SQL���,���ؼ�¼��</summary>
        ///<param name="strSql">SQL���</param>
        ///<returns>����Ӱ��ļ�¼��</returns>
        public static int SqlCommand(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlCommand cmd = new SqlCommand(strSql, conn))
            {
                cmd.CommandTimeout = 800;
                try
                { return cmd.ExecuteNonQuery(); }
                catch (SqlException e)
                { conn.Close(); throw e; }
            }
        }

        /// <summary>
        /// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlCommand cmd = new SqlCommand(strSQL, conn))
            {
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();
                    return rows;
                }
                catch (SqlException e)
                { conn.Close(); throw e; }

            }
        }


        /// <summary>
        /// ִ��һ��Sql��䣬����һ��object
        /// </summary>
        /// <param name="sql">sql���</param>
        public static object SqlCommandForObject(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlCommand cmd = new SqlCommand(strSql, conn))
                try
                {
                    object ob = cmd.ExecuteScalar();
                    conn.Close();
                    return ob;
                }
                catch (SqlException e)
                { conn.Close(); throw e; }
        }

        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����</summary>
        ///<param name="strSql">SQL���</param>		
        public static bool SqlCommandTrans(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlTransaction txn = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(strSql, conn, txn);
            try { cmd.ExecuteNonQuery(); txn.Commit(); conn.Close(); return true; }
            catch (SqlException e) { txn.Rollback(); conn.Close(); throw e; }
        }


        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����</summary>
        ///<param name="htSql">SQL���Ĺ�ϣ��(keyΪsql���,value�Ǹ�����SqlParameter[])</param>
        public static void SqlCommandTrans(Hashtable htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    foreach (DictionaryEntry de in htSql)
                    {
                        PrepareCommand(cmd, conn, txn, de.Key.ToString(), (SqlParameter[])de.Value);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        //conn.Close();
                    }
                    txn.Commit();
                }
                catch { txn.Rollback(); throw; }
                finally
                {
                    conn.Close();
                }
            }
        }

        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����</summary>
        ///<param name="htSql">���ͣ�SQL��䣬���ڰ�װ����˳��</param>
        public static bool SqlCommandTrans(List<string> htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                int n = 0;
                try
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < htSql.Count; i++)
                    {
                        PrepareCommand(cmd, conn, txn, htSql[i], ht);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        n++;
                        //conn.Close();
                    }
                    txn.Commit();
                    return true;
                }
                catch { txn.Rollback(); return false; }
                finally
                {
                    conn.Close();
                }
            }
        }

        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����,����г���,�򷵻ش��������</summary>
        ///<param name="htSql">���ͣ�SQL��䣬���ڰ�װ����˳��</param>
        public static string SqlCommandTransStr(List<string> htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                int n = 0;
                try
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < htSql.Count; i++)
                    {
                        PrepareCommand(cmd, conn, txn, htSql[i], ht);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        n++;
                    }
                    txn.Commit();
                    return "";
                }
                catch (Exception e) { txn.Rollback(); return "��" + n + "���д�(����������)! \r\n" + e; }
                finally
                {
                    conn.Close();
                }
            }
        }

        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����,����г���,�򷵻ش��������</summary>
        ///<param name="htSql">���ͣ�SQL��䣬���ڰ�װ����˳��</param>
        public static int SqlCommandTransInt(List<string> htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                int n = 0;
                try
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < htSql.Count; i++)
                    {
                        PrepareCommand(cmd, conn, txn, htSql[i], ht);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        n++;
                    }
                    txn.Commit();
                    return -1;
                }
                catch (Exception) { txn.Rollback(); return n; }
                finally
                {
                    conn.Close();
                }
            }
        }

        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����,�������һ���з���ֵ</summary>
        ///<param name="htSql">���ͣ�SQL��䣬���ڰ�װ����˳��</param>
        public static object SqlCommandTransForObject(List<string> htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < htSql.Count - 1; i++)
                    {
                        PrepareCommand(cmd, conn, txn, htSql[i], ht);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        //conn.Close();
                    }
                    if (htSql.Count > 0)
                    {
                        cmd.CommandText = htSql[htSql.Count - 1];
                        object ob = cmd.ExecuteScalar();
                        txn.Commit();
                        conn.Close();
                        return ob;
                    }
                    else
                        return null;
                }
                catch { txn.Rollback(); conn.Close(); return null; }
            }
        }


        ///<summary>ִ�ж���SQL���,ʵ�����ݿ�����(2008.05.07 �����޸�)</summary>
        ///<param name="htSql">SQL���Ĺ�ϣ��(keyΪsql���,value�Ǹ�����Hashtable)</param>
        public static bool SqlCommandTrans(Queue<YKParams<string, Hashtable>> htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    int count = htSql.Count;
                    for (int i = 0; i < count; i++)
                    {
                        YKParams<string, Hashtable> prams = htSql.Dequeue();
                        PrepareCommand(cmd, conn, txn, prams.GetT, prams.GetM);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    txn.Commit();
                    return true;
                }
                catch (Exception ex) { txn.Rollback(); throw ex; }
                finally
                {
                    conn.Close();
                }
            }
        }

        ///<summary>ִ�д洢����,ʵ�����ݿ�����(2008.06.02 �ƽ����޸�)</summary>
        ///<param name="htSql">SQL���Ĺ�ϣ��(keyΪsql���,value�Ǹ�����Hashtable)</param>
        public static bool SqlCommandTrans2(Queue<YKParams<string, Hashtable>> htSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlTransaction txn = conn.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    int count = htSql.Count;
                    for (int i = 0; i < count; i++)
                    {
                        YKParams<string, Hashtable> prams = htSql.Dequeue();
                        if (prams.GetT.IndexOf(" ") > 0)
                            PrepareCommand(cmd, conn, txn, prams.GetT, prams.GetM);
                        else
                            PrepareCommand2(cmd, conn, txn, prams.GetT, prams.GetM);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    txn.Commit();
                    return true;
                }
                catch { txn.Rollback(); return false; }
                finally
                {
                    conn.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------------

        ///<summary>ִ��һ�������ѯ������,���ز�ѯ���(object)</summary>
        ///<param name="strSql">�����ѯ������</param>
        ///<returns>��ѯ���(object)</returns>
        public static object GetSingle(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            using (SqlCommand cmd = new SqlCommand(strSql, conn))
            {
                try
                {
                    object obj = cmd.ExecuteScalar();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value)) return null;
                    else return obj;
                }
                catch (SqlException e)
                {
                    conn.Close();
                    throw e;
                }
            }
        }


        ///<summary>ִ�в�ѯ���,����DataSet(����"table")</summary>
        ///<param name="strSql">��ѯ���</param>
        ///<returns>DataSet</returns>
        public static DataSet Query(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(strSql, conn);
                da.Fill(ds);
                conn.Close();
            }
            catch (SqlException e) { conn.Close(); throw e; }
            return ds;
        }
        ///<summary>ִ�в�ѯ���,����DataSet(����"table")  07-23��,�ƽ������</summary>
        ///<param name="strSql">��ѯ���</param>
        ///<returns>DataSetֻ��һ����</returns>
        public static DataSet Query(List<string> str)
        {
            SqlConnection conn = YKDataClass.Connection();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            try
            {
                for (int i = 0; i < str.Count; i++)
                {
                    da = new SqlDataAdapter(str[i].ToString(), conn);
                    da.Fill(ds);
                }
                conn.Close();
            }
            catch (SqlException e) { conn.Close(); throw e; }
            return ds;
        }


        ///<summary>ִ�в�ѯ���,����DataTable</summary>
        ///<param name="strSql">��ѯsql���</param>
        ///<returns>DataTable</returns>
        public static DataTable getDataTable(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(strSql, conn);
                da.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception e)
            { if (conn != null) { conn.Close(); throw e; } return null; }

        }


        ///<summary>ִ�в�ѯ���,����SqlDataReader</summary>
        ///<param name="strSQL">��ѯ���</param>
        ///<returns>SqlDataReader</returns>
        public static SqlDataReader SqlCommandForDataReader(string strSQL)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlCommand cmd = new SqlCommand(strSQL, conn);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (SqlException e) { conn.Close(); throw e; }
        }

        #endregion

        #region ���д�������sql���

        /// <summary>
        /// ���д�������sql���,����һ��Object
        /// </summary>
        /// <param name="sql">������sql���</param>
        /// <param name="ht">������</param>
        /// <returns></returns>
        public static object ExecuteParameterSqlForObject(string sql, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                foreach (string tKey in ht.Keys)
                {
                    SqlParameter sp = new SqlParameter(tKey, ht[tKey]);
                    cmd.Parameters.Add(sp);
                }
                object ob = cmd.ExecuteScalar();
                conn.Close();
                return ob;
            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }


        /// <summary>
        /// ���д�������sql���,����Ӱ������
        /// </summary>
        /// <param name="sql">��������sql���</param>
        /// <param name="ht">������</param>
        /// <returns>Ӱ������</returns>
        public static int ExecuteParameterSql(string sql, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (ht != null)
                {
                    foreach (string tKey in ht.Keys)
                    {
                        SqlParameter sp = new SqlParameter(tKey, ht[tKey]);
                        cmd.Parameters.Add(sp);
                    }
                }
                return cmd.ExecuteNonQuery();

            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }

        /// <summary>
        /// ���д�������sql���,����Ӱ������
        /// </summary>
        /// <param name="sql">��������sql���</param>
        /// <param name="ht">������</param>
        /// <returns>Ӱ������</returns>
        public static DataTable ExecuteParameterSqlForDatatable(string sql, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (ht != null)
                {
                    foreach (string tKey in ht.Keys)
                    {
                        SqlParameter sp = new SqlParameter(tKey, ht[tKey]);
                        cmd.Parameters.Add(sp);
                    }
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd.Parameters.Clear();
                    return dt;
                }
            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }

        #endregion


        #region �洢���̲���


        ///<summary>ִ�д洢����,����������</summary>
        ///<param name="storedProcName">�洢������</param>
        ///<param name="parameters">�洢���̲���</param>
        ///<returns>SqlDataReader</returns>
        public static string RunSqlProc(string storedProcName, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            try
            {
                SqlCommand cmd = BuildQueryCommand(conn, storedProcName, ht);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();
                return "";
            }
            catch (SqlException e)
            { conn.Close(); return e.ToString(); }
        }

        ///<summary>ִ�д洢����,����һ��DataSet</summary>
        ///<param name="storedProcName">�洢������</param>
        ///<param name="parameters">�洢���̲���</param>
        ///<returns>DataSet</returns>
        public static DataSet RunSqlProcForDataSet(string storedProcName, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = BuildQueryCommand(conn, storedProcName, ht);
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }


        ///<summary>ִ�д洢����,����һ�ű�</summary>
        ///<param name="storedProcName">�洢������</param>
        ///<param name="parameters">�洢���̲���</param>
        ///<returns>DataTable</returns>
        public static DataTable RunSqlProcForTable(string storedProcName, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = BuildQueryCommand(conn, storedProcName, ht);
                da.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }

        /// <summary>
        /// ִ�д洢����,����һ���洢���̷���ֵ(string)
        /// </summary>
        ///<param name="storedProcName">�洢������</param>
        ///<param name="parameters">�洢���̲���</param>
        ///<returns>�洢���̷���ֵ(string)</returns>
        public static object RunSqlProcForObject(string storedProcName, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            try
            {
                SqlCommand cmd = BuildQueryCommand(conn, storedProcName, ht);
                SqlParameter rtnval = cmd.Parameters.Add("rval", SqlDbType.NVarChar, 20);
                rtnval.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                conn.Close();
                return rtnval.Value;
            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }

        ///<summary>ִ�д洢����,����sqlDataReader</summary>
        ///<param name="storedProcName">�洢������</param>
        ///<param name="parameters">�洢���̲���</param>
        public static SqlDataReader RunSqlProcForDataReader(string storedProcName, Hashtable ht)
        {
            SqlConnection conn = YKDataClass.Connection();
            try
            {
                SqlCommand cmd = BuildQueryCommand(conn, storedProcName, ht);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return sr;
            }
            catch (SqlException e)
            { conn.Close(); throw e; }
        }


        ///<summary>����SqlCommand����(��������һ�������,������һ������ֵ)</summary>
        ///<param name="conn">���ݿ�����</param>
        ///<param name="storedProcName">�洢������</param>
        ///<param name="paras">�洢���̲���</param>
        ///<returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection conn, string storedProcName, Hashtable ht)
        {
            SqlCommand cmd = new SqlCommand(storedProcName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 800;
            if (ht != null)
            {
                foreach (string tKey in ht.Keys)
                {
                    SqlParameter sp = new SqlParameter(tKey, ht[tKey].ToString().Replace("'", ""));
                    cmd.Parameters.Add(sp);
                }
            }
            return cmd;
        }

        /// <summary>
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����,ArrayList���鷽ʽʵ�֡�
        /// </summary>
        /// <param name="SQLStringList">����SQL���</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
                conn.Close();
            }
            catch (SqlException E)
            {
                tx.Rollback();
                conn.Close();
                throw E;
            }
        }



        #endregion

    }
}