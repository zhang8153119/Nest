using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FZYK.Com
{
    /// <summary>
    /// 页面数据校验类
    /// </summary>
   public class YKPageValidate
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex(@"^[+-]?\d+[.]?\d+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");


        /// <summary>
        /// 是否数字字符串,整数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>是否数字型数据</returns>
        public static bool IsNumber(string inputData)
        {
            if (inputData.Equals(""))
                return true;
            else
            {
                inputData = CharConverter(inputData);
                Match m = RegNumber.Match(inputData);
                return m.Success;
            }
        }
        /// <summary>
        /// 是否整数数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>是否整数型数据</returns>
        public static bool IsNumberSign(string inputData)
        {
            if (inputData.Equals(""))
                return true;
            else
            {
                inputData = CharConverter(inputData);
                Match m = RegNumberSign.Match(inputData);
                return m.Success;
            }
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>是否浮点数</returns>
        public static bool IsDecimal(string inputData)
        {
            inputData = CharConverter(inputData);
            if (inputData.Equals("0") || inputData.Equals(""))
                return true;
            else
            {
                Match m = RegDecimal.Match(inputData);
                Match m1 = RegNumber.Match(inputData);
                bool aa = m.Success || m1.Success;
                return aa;
            }
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>是否浮点数</returns>
        public static bool IsDecimalSign(string inputData)
        {
            inputData = CharConverter(inputData);
            if (inputData.Equals("0") || inputData.Equals("") || IsDecimal(inputData) || IsNumberSign(inputData))
                return true;
            else
            {
                Match m = RegDecimalSign.Match(inputData);
                return m.Success;
            }
        }

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>是否含中文</returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是有效的Email字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns>是否是Email字符串</returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>子串</returns>
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }


        /// <summary>
        /// 字符串清理(检查是否为空,检查长度,替换危险字符,替换单引号)
        /// </summary>
        /// <param name="inputString">待清理字符串</param>
        /// <param name="maxLength">长度</param>
        /// <returns>清理后的字符串</returns>
        public static string InputText(string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder();

            // 检查是否为空
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();
                //检查长度
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                //替换危险字符
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }

                // 替换单引号
                retVal.Replace("'", " ");
            }

            return retVal.ToString();

        }

        /// <summary>
        /// 判断是否为合法日期，必须大于1900年1月1日
        /// </summary>
        /// <param name="strDate">输入日期字符串</param>
        /// <returns>True/False</returns>
        public static bool IsDateTime(string strDate)
        {
            try
            {
                DateTime oDate = DateTime.Parse(strDate);
                if (oDate.CompareTo(DateTime.Parse("1900-1-1")) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// 将传进来的某数字型字符串变成是数字从而排序
        /// </summary>
        /// <param name="str">某数值型字符串</param>
        /// <returns>转换好的数字</returns>
        public static int getSortString(string str)
        {
            string returnStr = "";
            if (str == "" || char.IsNumber(str, 0))
                returnStr = "30";
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str, i))
                    returnStr += str[i].ToString();
            }
            int aa = 5000;
            if (returnStr != "")
                aa = Convert.ToInt32(returnStr);
            return aa;
        }


        /// <summary>
        /// 格式化日期字符串
        /// </summary>
        /// <param name="beginDay">开始日期字符串</param>
        /// <param name="endDay">结束日期字符串</param>
        /// <returns>正常日期年月日</returns>
        public static string[] FormatDateString(string beginDay, string endDay)
        {
            string[] day = new string[2];
            day[0] = "";
            day[1] = "";
            if (beginDay != "" || endDay != "")
            {
                if (YKPageValidate.IsDateTime(beginDay))
                    beginDay = beginDay + " 0:00:00";
                if (YKPageValidate.IsDateTime(endDay))
                    endDay = endDay + " 23:59";
                day[0] = beginDay;
                day[1] = endDay;
            }
            return day;
        }

        /// <summary>
        /// 某字符串是否是每个字符都是数字型
        /// </summary>
        /// <param name="inputtext">输入字符串</param>
        /// <returns>是否数字型字符串</returns>
        public static bool ISCharNumber(string inputtext)
        {
            bool map = true;
            for (int i = 0; i < inputtext.Length; i++)
            {
                if (!Char.IsNumber(inputtext, i))
                {
                    map = false;
                    break;
                }
            }
            return map;
        }

        /// <summary>
        /// 将输入的字符串全角变成半角
        /// </summary>
        /// <param name="source">输入的字符串</param>
        /// <returns>返回的半角字符串</returns>
        public static string CharConverter(string source)
        {
            if (source.Length == 0)
                return "";
            System.Text.StringBuilder result = new System.Text.StringBuilder(source.Length, source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] >= 65281 && source[i] <= 65373)
                {
                    result.Append((char)(source[i] - 65248));
                }
                else if (source[i] == 12288)
                {
                    result.Append(' ');
                }
                else
                {
                    result.Append(source[i]);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>返回的全角字符串</returns>
        public static string CharToSBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }

                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 判断一个String数组是否存在某个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ifContainsString(string[] str, string id)
        {
            bool map = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Equals(id))
                {
                    map = true;
                    break;
                }
            }
            return map;
        }


    }
}
