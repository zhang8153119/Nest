using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
namespace FZYK.Com
{
    /// <summary>
    /// 文件操作类
    /// </summary>
   public  class YKFileOperate
    {

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirtory(string path)
        {
            if (!File.Exists(path))
            {
                string[] dirArray = path.Split('\\');
                string temp = string.Empty;
                for (int i = 0; i < dirArray.Length - 1; i++)
                {
                    temp += dirArray[i].Trim() + "\\";
                    if (!Directory.Exists(temp))
                        Directory.CreateDirectory(temp);
                }
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="objPath">目的路径</param>
        public static void CopyFile(string sourcePath, string objPath)
        {
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                string[] childfile = files[i].Split('\\');
                File.Copy(files[i], objPath + @"\" + childfile[childfile.Length - 1], true);
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < dirs.Length; i++)
            {
                string[] childdir = dirs[i].Split('\\');
                CopyFile(dirs[i], objPath + @"\" + childdir[childdir.Length - 1]);
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读出的字符串</returns>
        public static string file_get_contents(string path)
        {
            StringBuilder s = new StringBuilder();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("GB2312")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    s.Append(line);
                }
            }
            return s.ToString();
        }

       

        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        public static void DeleteFolder(string dir)
        {
            foreach (string str in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(str))
                {
                    FileInfo info = new FileInfo(str);
                    if (info.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(str);
                }
                else
                {
                    DeleteFolder(str);
                }
            }
            Directory.Delete(dir);
        }


        private static byte[] key = ASCIIEncoding.ASCII.GetBytes("caikelun");
        private static byte[] iv = ASCIIEncoding.ASCII.GetBytes("12345678");
        /// <summary>
        /// DES加密。
        /// </summary>
        /// <param name="inputString">输入字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string DESEncrypt(string inputString)
        {
            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamWriter sw = null;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                sw = new StreamWriter(cs);
                sw.Write(inputString);
                sw.Flush();
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            finally
            {
                if (sw != null) sw.Close();
                if (cs != null) cs.Close();
                if (ms != null) ms.Close();
            }
        }

        /// <summary>
        /// DES解密。
        /// </summary>
        /// <param name="inputString">输入字符串。</param>
        /// <returns>解密后的字符串。</returns>
        public static string DESDecrypt(string inputString)
        {
            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamReader sr = null;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                ms = new MemoryStream(Convert.FromBase64String(inputString));
                cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            finally
            {
                if (sr != null) sr.Close();
                if (cs != null) cs.Close();
                if (ms != null) ms.Close();
            }
        }
        #region ...... 读写INI文件
        public string path;    //INI文件名

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //声明读写INI文件的API函数

        public static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SUN.ini");
        }

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SUN.ini");
            return temp.ToString();

        }
        #endregion
        /// <summary>
        /// 将Excel数据添加到数据库中 鄢国平 9-25
        /// </summary>
        /// <param name="file"></param>
        public static DataTable ImportExcelFile(string file)
        {
            try
            {
                DataTable dt = new DataTable();
                string excelStr = " Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + file + ";Extended Properties=\"Excel 8.0;HDR=no;IMEX=1;\"";
                using (OleDbConnection cn = new OleDbConnection(excelStr))
                {
                    string sqlda = "SELECT * FROM [Sheet1$]";
                    using (OleDbDataAdapter oda = new OleDbDataAdapter(sqlda, excelStr))
                    {
                        oda.Fill(dt);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
   
}
