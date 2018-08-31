using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace FZYK.Com.Log
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class YKLog
    {
        #region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
        public YKLog(string Module, string MenuName, string ClassName)
		{
            _ClassName = ClassName;
            _Module = Module;
            _MenuName = MenuName;
            _E_Name = UserInfo.eName;
            _E_Num = UserInfo.eNum;
            _DD_Name = UserInfo.dName;
		}
        /// <summary>
        /// 构造函数
        /// </summary>
        public YKLog()
        {}
		#endregion
        
        private string _E_Name="";//操作人
        private string _E_Num="";//操作人编号
        private string _DD_Name="";//操作人部门
        private string _Module="";//模块
        private string _MenuName="";//菜单名
        private string _ClassName = "";//类名
        private string _ErrPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Errlog\";//异常保存文件夹
        private string _LogPath = System.AppDomain.CurrentDomain.BaseDirectory + @"log\";//临时保存记录文件
        private string _str = ""; //数据串
        private int _nrow = 0;//日志行数
        ////声明委托
        public delegate void AsyncEventHandler();
       
        #region 写文件
        /// <summary>
        /// 获取写到文件的数据集
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="op_type">操作类型</param>
        /// <param name="P_KeyName">主键名</param>
        /// <param name="P_KeyValue">主键值</param>
        /// <returns></returns>
        private string GetInfo(string Content,string op_type,string P_KeyName,string P_KeyValue)
        {
            
            StringBuilder sbstr = new System.Text.StringBuilder(2) ;
            sbstr.Append(Content);
            sbstr.Append(" ");
            sbstr.Append(_E_Name);
            sbstr.Append(" ");
            sbstr.Append(_E_Num);
            sbstr.Append(" ");
            sbstr.Append(_DD_Name);
            sbstr.Append(" ");
            sbstr.Append(DateTime.Now.ToString());
            sbstr.Append(" ");
            sbstr.Append(op_type);
            sbstr.Append(" ");
            sbstr.Append(_Module);
            sbstr.Append(" ");
            sbstr.Append(_MenuName);
            sbstr.Append(" ");
            sbstr.Append(_ClassName);
            sbstr.Append(" ");
            sbstr.Append(P_KeyName);
            sbstr.Append(" ");
            sbstr.Append(P_KeyValue);
            sbstr.Append(" ");
            sbstr.Append(YKGetClient.GetIP());
            sbstr.Append(" ");
            sbstr.Append(YKGetClient.GetHostName());
            sbstr.Append("\r\n");
            return sbstr.ToString();
        }
        /// <summary>
        /// 日志操作
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="op_type">操作类型</param>
        /// <param name="P_KeyName">主键名</param>
        /// <param name="P_KeyValue">主键值</param>
        public void Info(string Content,string op_type,string P_KeyName,string P_KeyValue)
        {
            //_str = GetInfo(Content, op_type, P_KeyName, P_KeyValue);
            //_nrow++;
            //if (!Directory.Exists(_LogPath))
            //{
            //    Directory.CreateDirectory(_LogPath);
            //}
            //Write(_LogPath+"log.txt", _str);
            #region 直接写入日志
            Hashtable ht = new Hashtable();
            ht.Add("@LogContent", Content);
            ht.Add("@LogOperater", _E_Name);
            ht.Add("@LogOperaterNum", _E_Num);
            ht.Add("@LogOperaterDept", _DD_Name);
            //ht.Add("@LogOperateDate", DateTime.Now.ToString());
            ht.Add("@LogOperateType", op_type);
            ht.Add("@LogModule", _Module);
            ht.Add("@LogMenuName", _MenuName);
            ht.Add("@LogClassName", _ClassName);
            ht.Add("@LogKeyName", P_KeyName);
            ht.Add("@LogKeyValue", P_KeyValue);
            ht.Add("@LogClientIP", YKGetClient.GetIP());
            ht.Add("@LogClientName", YKGetClient.GetHostName());
            YKDataClass.ExecuteParameterSql(GetSql("B_Log"), ht);
            #endregion
        }
        /// <summary>
        /// 将日志写入文件
        /// </summary>
        /// <param name="_filename">文件名</param>
        /// <param name="_str">写入数据字符串</param>
        private void Write(string _filename,string _str)
        {
            try
            {
                FileStream fs = new FileStream(_filename, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(_str);
                sw.Close();
                fs.Close();                
                if (_nrow >= 5)
                {
                    asyInsert();
                    _nrow = 0;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        /// <summary>
        /// 判断文件是否被打开
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsFileOpen(string filePath)
        {
            bool result = false;
            System.IO.FileStream fs = null;
            try
            {
                fs = File.OpenWrite(filePath);
                fs.Close();
            }
            catch (Exception ex)
            {
                result = true;
            }
            return result;//true 打开 false 没有打开
        }
        #endregion
        #region 将文件写到数据库中
        private void asyInsert()
        {
            AsyncEventHandler asy = new AsyncEventHandler(InsertDataBase);
            IAsyncResult ia = asy.BeginInvoke(null, null);
            asy.EndInvoke(ia);
        }
        /// <summary>
        /// 将暂时保存在文件中的数据写到数据库中
        /// </summary>
        /// <returns></returns>
        public void InsertDataBase()
        {
            //if (!File.Exists(_LogPath + "log.txt"))
            //{
            //    return;
            //}
            //Queue<YKParams<string, Hashtable>> myQueue = new Queue<YKParams<string, Hashtable>>();
            //StreamReader sr = new StreamReader(_LogPath + "log.txt");
            //string logstr = "";
            //while ((logstr = sr.ReadLine()) != null)
            //{
            //    myQueue.Enqueue(new YKParams<string, Hashtable>(GetSql("B_Log"), GetHash(logstr)));
            //} 
            //sr.Close();
            //if (YKDataClass.SqlCommandTrans(myQueue))
            //{
            //    if (File.Exists(_LogPath + "log.txt"))
            //        File.Delete(_LogPath + "log.txt");
            //}
        }
        /// <summary>
        /// 根据本地日志文件得到插入数据库日志数据
        /// </summary>
        /// <param name="logstr">本地文件取出字符串数据</param>
        /// <returns></returns>
        private Hashtable GetHash(string logstr)
        {
            Hashtable ht = new Hashtable();

            string[] str = logstr.Split(' ');
            ht.Add("@LogContent", str[0]);
            ht.Add("@LogOperater", str[1]);
            ht.Add("@LogOperaterNum", str[2]);
            ht.Add("@LogOperaterDept", str[3]);
            ht.Add("@LogOperateDate", str[4] + " " + str[5]);
            ht.Add("@LogOperateType", str[6]);
            ht.Add("@LogModule", str[7]);
            ht.Add("@LogMenuName", str[8]);
            ht.Add("@LogClassName", str[9]);
            ht.Add("@LogKeyName", str[10]);
            ht.Add("@LogKeyValue", str[11]);
            ht.Add("@LogClientIP", str[12]);
            ht.Add("@LogClientName", str[13]);
            return ht;
        }
        /// <summary>
        /// 插入SQL,日志表和错误表
        /// </summary>
        /// <param name="TableName">日志、异常表名</param>
        /// <returns></returns>
        private string GetSql(string TableName)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append("insert into " + TableName + "(LogContent,LogOperater,LogOperaterNum,LogOperaterDept,LogOperateDate");
            sbsql.Append(",LogOperateType,LogModule,LogMenuName,LogClassName,LogKeyName,LogKeyValue,LogClientIP,LogClientName)");
            sbsql.Append("values(@LogContent,@LogOperater,@LogOperaterNum,@LogOperaterDept,getdate()");
            sbsql.Append(",@LogOperateType,@LogModule,@LogMenuName,@LogClassName,@LogKeyName,@LogKeyValue,@LogClientIP,@LogClientName)");
            return sbsql.ToString();
        }
        #endregion
        #region 异常
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="error">错误Exception</param>
        public void Error(Exception error)
        {
            if (!Directory.Exists(_ErrPath))
            {
                Directory.CreateDirectory(_ErrPath);
            }
            YKDataClass.ExecuteParameterSql(GetSql("B_LogError"), GetHash(error));
            Write(_ErrPath+"error"+DateTime.Now.ToString("yyyyMMdd")+".txt", string.Format("异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                                   error.GetType().Name, error.Message, error.StackTrace));
        }
        /// <summary>
        /// 根据Exception得到插入异常表数据
        /// </summary>
        /// <param name="e">错误Exception</param>
        /// <returns></returns>
        private Hashtable GetHash(Exception e)
        {
            Hashtable ht = new Hashtable();
            string strerr = string.Format("异常消息：{0}\r\n异常信息：{1}\r\n",
                       e.Message, e.StackTrace);
            string ErrType = string.Format("异常类型：{0}\r\n", e.GetType().Name);
            ht.Add("@LogContent", strerr);
            ht.Add("@LogOperater", _E_Name);
            ht.Add("@LogOperaterNum", _E_Num);
            ht.Add("@LogOperaterDept", _DD_Name);
            ht.Add("@LogOperateDate", DateTime.Now.ToString());
            ht.Add("@LogOperateType", ErrType);
            ht.Add("@LogModule", _Module);
            ht.Add("@LogMenuName", _MenuName);
            ht.Add("@LogClassName", "");
            ht.Add("@LogKeyName", "");
            ht.Add("@LogKeyValue", "");
            ht.Add("@LogClientIP", YKGetClient.GetIP());
            ht.Add("@LogClientName", YKGetClient.GetHostName());
            return ht;
        }
        #endregion
    }
}
