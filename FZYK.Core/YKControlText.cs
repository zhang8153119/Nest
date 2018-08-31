using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FZYK.Com;
namespace FZYK.Core
{
    /// <summary>
    /// 获取数据库中配置的控件的text属性值
    /// </summary>
    public class YKControlText
    {
        /// <summary>
        /// 登陆的时候加载数据库中配置的全局设置
        /// </summary>
        public static void InitControlText()
        {
            AppendDic("ALL");
        }
        private static Dictionary<string, string> _dicControlText = new Dictionary<string, string>(2);
        private static List<string> _listClassName = new List<string>(2);
        /// <summary>
        /// 获得制单Label的Text属性值
        /// </summary>
        /// <returns></returns>
        public static string GetWriter()
        {
            string str=GetText("ALL", "Writer");
            if (str.Equals(""))
                return "制单";
            return str;
        }
        /// <summary>
        /// 获得锁定Label的Text属性值
        /// </summary>
        /// <returns></returns>
        public static string GetLocker()
        {
            string str = GetText("ALL", "Locker");
            if (str.Equals(""))
                return "锁定";
            return str;             
        }
        /// <summary>
        /// 获得解锁Label的Text属性值
        /// </summary>
        /// <returns></returns>
        public static string GetUnLocker()
        {
            string str = GetText("ALL", "UnLocker");
            if (str.Equals(""))
                return "解锁";
            return str;    
        }
        /// <summary>
        /// 根据类名和控件名称获得数据库中配置的Text属性值
        /// </summary>
        /// <param name="ClassName">类名</param>
        /// <param name="ControlName">控件名</param>
        /// <returns></returns>
        public static string GetText(string ClassName, string ControlName)
        {
            if (!_listClassName.Contains(ClassName))
            {
                AppendDic(ClassName);
            }
            if (_dicControlText.ContainsKey(ClassName + ControlName))
                return _dicControlText[ClassName + ControlName];
            return "";
        }
        /// <summary>
        /// 追加配置
        /// </summary>
        /// <param name="ClassName"></param>
        private static void AppendDic(string ClassName)
        {
            //if (!_listClassName.Contains(ClassName))
            //    _listClassName.Add(ClassName);
            //DataTable dt = LoginClass.GetControlText(ClassName);
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (!_dicControlText.ContainsKey(ClassName + Convert.ToString(row["ControlName"])))
            //        _dicControlText.Add(ClassName + Convert.ToString(row["ControlName"]), Convert.ToString(row["ControlText"]));
            //}
        }
    }   
}
