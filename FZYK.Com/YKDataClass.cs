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
    ///数据库访问基础类(基于SQLServer) 
    ///</summary>
    public class YKDataClass
    {
        ///<summary>数据库连接字符串</summary>

        public static string ConnectionString = "";

        #region 公用方法
        ///<summary>获得一个打开的连接(使用后记得关闭)</summary>
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

        ///<summary>初始化一个SqlCommand命令对象</summary>
        ///<param name="cmd">SqlCommand对象</param>
        ///<param name="conn">SqlConnection对象</param>
        ///<param name="txn">SqlTransaction对象</param>
        ///<param name="strSql">SQL语句</param>
        ///<param name="par">SqlParameter数组</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction txn, string strSql, SqlParameter[] par)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;
            cmd.Connection = conn;
            if (txn != null) cmd.Transaction = txn;
            if (par != null) foreach (SqlParameter pm in par) cmd.Parameters.Add(pm);
        }
        ///<summary>初始化一个SqlCommand命令对象</summary>
        ///<param name="cmd">SqlCommand对象</param>
        ///<param name="conn">SqlConnection对象</param>
        ///<param name="txn">SqlTransaction对象</param>
        ///<param name="strSql">SQL语句</param>
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
        ///<summary>初始化一个SqlCommand命令对象</summary>
        ///<param name="cmd">SqlCommand对象</param>
        ///<param name="conn">SqlConnection对象</param>
        ///<param name="txn">SqlTransaction对象</param>
        ///<param name="procName">存储过程名称</param>
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
        /// 查一张表中最大记录数,即最后一条
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ") from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null) return 0;
            else return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 查一张表中最后一条记录的主键值Num
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public static string GetMaxNum(string FieldName, string TableName)
        {
            string strsql = "select   " + FieldName + " from " + TableName + " order by " + FieldName + " desc";
            object obj = GetSingle(strsql);
            if (obj == null) return "";
            else return obj.ToString();
        }

        /// <summary>
        /// 查询一个有效对象是否存在,表名,字段名,值灵活给定,
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
        /// 查询一个记录某字段是否存在该值,表名,字段名,值灵活给定,
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
        /// 查询一个对象是否存在
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
        /// 查询是否判断某table中是否存在某字段
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

        #region 执行Sql语句

        ///<summary>执行SQL语句,返回记录数</summary>
        ///<param name="strSql">SQL语句</param>
        ///<returns>返回影响的记录数</returns>
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
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
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
        /// 执行一条Sql语句，返回一个object
        /// </summary>
        /// <param name="sql">sql语句</param>
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

        ///<summary>执行多条SQL语句,实现数据库事务</summary>
        ///<param name="strSql">SQL语句</param>		
        public static bool SqlCommandTrans(string strSql)
        {
            SqlConnection conn = YKDataClass.Connection();
            SqlTransaction txn = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand(strSql, conn, txn);
            try { cmd.ExecuteNonQuery(); txn.Commit(); conn.Close(); return true; }
            catch (SqlException e) { txn.Rollback(); conn.Close(); throw e; }
        }


        ///<summary>执行多条SQL语句,实现数据库事务</summary>
        ///<param name="htSql">SQL语句的哈希表(key为sql语句,value是该语句的SqlParameter[])</param>
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

        ///<summary>执行多条SQL语句,实现数据库事务</summary>
        ///<param name="htSql">泛型，SQL语句，用于包装插入顺序</param>
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

        ///<summary>执行多条SQL语句,实现数据库事务,如果有出错,则返回错误的行数</summary>
        ///<param name="htSql">泛型，SQL语句，用于包装插入顺序</param>
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
                catch (Exception e) { txn.Rollback(); return "第" + n + "行有错(不包括空行)! \r\n" + e; }
                finally
                {
                    conn.Close();
                }
            }
        }

        ///<summary>执行多条SQL语句,实现数据库事务,如果有出错,则返回错误的行数</summary>
        ///<param name="htSql">泛型，SQL语句，用于包装插入顺序</param>
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

        ///<summary>执行多条SQL语句,实现数据库事务,并且最后一句有返回值</summary>
        ///<param name="htSql">泛型，SQL语句，用于包装插入顺序</param>
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


        ///<summary>执行多条SQL语句,实现数据库事务(2008.05.07 天生修改)</summary>
        ///<param name="htSql">SQL语句的哈希表(key为sql语句,value是该语句的Hashtable)</param>
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

        ///<summary>执行存储过程,实现数据库事务(2008.06.02 黄建忠修改)</summary>
        ///<param name="htSql">SQL语句的哈希表(key为sql语句,value是该语句的Hashtable)</param>
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

        ///<summary>执行一条计算查询结果语句,返回查询结果(object)</summary>
        ///<param name="strSql">计算查询结果语句</param>
        ///<returns>查询结果(object)</returns>
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


        ///<summary>执行查询语句,返回DataSet(表名"table")</summary>
        ///<param name="strSql">查询语句</param>
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
        ///<summary>执行查询语句,返回DataSet(表名"table")  07-23晚,黄建忠添加</summary>
        ///<param name="strSql">查询语句</param>
        ///<returns>DataSet只有一个表</returns>
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


        ///<summary>执行查询语句,返回DataTable</summary>
        ///<param name="strSql">查询sql语句</param>
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


        ///<summary>执行查询语句,返回SqlDataReader</summary>
        ///<param name="strSQL">查询语句</param>
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

        #region 运行带参数的sql语句

        /// <summary>
        /// 运行带参数的sql语句,返回一个Object
        /// </summary>
        /// <param name="sql">带参数sql语句</param>
        /// <param name="ht">参数表</param>
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
        /// 运行带参数的sql语句,返回影响行数
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="ht">参数表</param>
        /// <returns>影响行数</returns>
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
        /// 运行带参数的sql语句,返回影响行数
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="ht">参数表</param>
        /// <returns>影响行数</returns>
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


        #region 存储过程操作


        ///<summary>执行存储过程,不返回数据</summary>
        ///<param name="storedProcName">存储过程名</param>
        ///<param name="parameters">存储过程参数</param>
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

        ///<summary>执行存储过程,返回一个DataSet</summary>
        ///<param name="storedProcName">存储过程名</param>
        ///<param name="parameters">存储过程参数</param>
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


        ///<summary>执行存储过程,返回一张表</summary>
        ///<param name="storedProcName">存储过程名</param>
        ///<param name="parameters">存储过程参数</param>
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
        /// 执行存储过程,返回一个存储过程返回值(string)
        /// </summary>
        ///<param name="storedProcName">存储过程名</param>
        ///<param name="parameters">存储过程参数</param>
        ///<returns>存储过程返回值(string)</returns>
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

        ///<summary>执行存储过程,返回sqlDataReader</summary>
        ///<param name="storedProcName">存储过程名</param>
        ///<param name="parameters">存储过程参数</param>
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


        ///<summary>构建SqlCommand对象(用来返回一个结果集,而不是一个整数值)</summary>
        ///<param name="conn">数据库连接</param>
        ///<param name="storedProcName">存储过程名</param>
        ///<param name="paras">存储过程参数</param>
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
        /// 执行多条SQL语句，实现数据库事务,ArrayList数组方式实现。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
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