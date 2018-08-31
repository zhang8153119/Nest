using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FZYK.Com
{
   public class YKException
    {

       public static string HandleTryCatch(string EType, Exception error)
        {
            if (string.IsNullOrEmpty(EType))
                EType = "已截获异常";
            string str = "";
            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            if (error != null)
            {
                str = string.Format("异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                                   error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("未截获异常:{0}", error.ToString());
            }
           // YKMessageBox.ShowBoxException(strDateInfo + str);
             writeLog(EType,error );
             return strDateInfo + str;
         }

       static  void writeLog(string EType, Exception error)
        {
            Log.YKLog yklog = new Log.YKLog( "", "", "");
            yklog.Error(error);
        }

    }
}
