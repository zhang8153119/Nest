using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace FZYK.Com
{
    public class YKMethod
    {
          /// <summary>
          /// 检查权限授权情况。返回true 则该菜单已经授权当然登录人允许操作
          /// </summary>
          /// <param name="alName">菜单类名，同权限表</param>
          /// <param name="oldeName">原(离职人)姓名</param>
          /// <returns>true / false 为true则该菜单已经授权当然登录人允许操作</returns>
          public static bool CheckAuthorityEmpower(string alName, string oldeName)
          {
                if (UserInfo .eName .Equals(oldeName))
                      return true;
                else
                {
                      StringBuilder sb = new StringBuilder();
                      sb .Append(" SELECT TOP 1 1 FROM B_AuthorityEmpower m");
                      sb .Append(" INNER JOIN B_AuthorityEmpowerDetail d");
                      sb .Append(" ON d.aeID=m.aeID");
                      sb .Append(" WHERE d.isEmpower=1 AND m.[State]=99");
                      sb .Append(" AND d.alName='" + alName + "'");
                      sb .Append(" AND m.oldeName='" + oldeName + "'");
                      sb .Append(" AND m.neweID=" + UserInfo .eID);
                      if (YKDataClass .Exists(sb .ToString()))
                            return true;
                      else
                            return false;
                }
          }
        /// <summary>
        /// 拆分输入的字段，得到表段
        /// </summary>
        /// <param name="Sect"></param>
        /// <returns></returns>
        public static string GetSect(string Sect)
        {

            if (Sect.IndexOf("-") > 0)
            {
                if (Sect.IndexOf(",") > 0)
                {
                    string SectName = "";
                    string[] sectArray = Sect.Split(',');
                    for (int i = 0; i < sectArray.Length; i++)
                    {
                        if (sectArray[i].IndexOf("-") > 0)
                        {
                            SectName += SplitString(sectArray[i]) + ",";
                            //string old = sectArray[i];
                            //string newstr = SplitString(sectArray[i]);
                            //SectName.Replace(old, newstr);
                        }
                        else
                            SectName += sectArray[i] + ",";
                    }
                    return SectName.Substring(0, SectName.Length - 1);
                }
                else
                    return SplitString(Sect);
            }
            else
                return Sect;
        }
        /// <summary>
        /// 将1-7 拆分为1，2，3，4，5，6，7
        /// </summary>
        /// <param name="Sect"></param>
        /// <returns></returns>
        public static string SplitString(string Sect)
        {
            int SectStart = Convert.ToInt32(Sect.Substring(0, Sect.IndexOf('-')));
            int SectEnd = Convert.ToInt32(Sect.Substring(Sect.IndexOf("-") + 1, Sect.Length - Sect.IndexOf("-") - 1));
            string SectString = "";
            for (int i = SectStart; i <= SectEnd; i++)
            {
                SectString = SectString + i + ",";
            }
            return SectString.Substring(0, SectString.Length - 1);
        }
        /// <summary>
        /// 对字符串中的数字进行自增 鄢国平 2012-11-26
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringIdentify(string str)
        {
            int Lenght = str.Length;
            string strName = "" + str;// +str;
            for (int i = Lenght - 1; i >= 0; i--)
            {
                if (YKPageValidate.IsDecimal(str.Substring(i, 1)))
                {
                    int Number = Convert.ToInt32(str.Substring(i, 1)) + 1;
                    if (Number <= 9)
                    {
                        strName = strName.Remove(i, 1);
                        strName = strName.Insert(i, Number.ToString());
                        break;
                    }
                    else
                    {
                        strName = strName.Remove(i, 1);
                        strName = strName.Insert(i, "0");
                        if (i == 0)
                        {
                            strName = strName.Insert(i, "1");
                            break;
                        }
                    }
                }
            }
            return strName;
        }
        /// <summary>
        /// 小写金额转大写金额 鄢国平 2013年1月13日 14:54:28
        /// </summary>
        /// <param name="Money">接收需要转换的小写金额</param>
        /// <returns>返回大写金额</returns>
        public static string ConvertMoney(Decimal Money)
        {
            //金额转换程序
            string MoneyNum = "";//记录小写金额字符串[输入参数]
            string MoneyStr = "";//记录大写金额字符串[输出参数]
            string BNumStr = "零壹贰叁肆伍陆柒捌玖";//模
            string UnitStr = "万仟佰拾亿仟佰拾万仟佰拾圆角分";//模

            MoneyNum = ((long)(Money * 100)).ToString();
            for (int i = 0; i < MoneyNum.Length; i++)
            {
                string DVar = "";//记录生成的单个字符(大写)
                string UnitVar = "";//记录截取的单位
                for (int n = 0; n < 10; n++)
                {
                    //对比后生成单个字符(大写)
                    if (Convert.ToInt32(MoneyNum.Substring(i, 1)) == n)
                    {
                        DVar = BNumStr.Substring(n, 1);//取出单个大写字符
                        //给生成的单个大写字符加单位
                        UnitVar = UnitStr.Substring(15 - (MoneyNum.Length)).Substring(i, 1);
                        n = 10;//退出循环
                    }
                }
                //生成大写金额字符串
                MoneyStr = MoneyStr + DVar + UnitVar;
            }
            //二次处理大写金额字符串
            MoneyStr = MoneyStr + "整";
            while (MoneyStr.Contains("零分") || MoneyStr.Contains("零角") || MoneyStr.Contains("零佰") || MoneyStr.Contains("零仟")
                || MoneyStr.Contains("零万") || MoneyStr.Contains("零亿") || MoneyStr.Contains("零零") || MoneyStr.Contains("零圆")
                || MoneyStr.Contains("亿万") || MoneyStr.Contains("零整") || MoneyStr.Contains("分整"))
            {
                MoneyStr = MoneyStr.Replace("零分", "零");
                MoneyStr = MoneyStr.Replace("零角", "零");
                MoneyStr = MoneyStr.Replace("零拾", "零");
                MoneyStr = MoneyStr.Replace("零佰", "零");
                MoneyStr = MoneyStr.Replace("零仟", "零");
                MoneyStr = MoneyStr.Replace("零万", "万");
                MoneyStr = MoneyStr.Replace("零亿", "亿");
                MoneyStr = MoneyStr.Replace("亿万", "亿");
                MoneyStr = MoneyStr.Replace("零零", "零");
                MoneyStr = MoneyStr.Replace("零圆", "圆零");
                MoneyStr = MoneyStr.Replace("零整", "整");
                MoneyStr = MoneyStr.Replace("分整", "分");
            }
            if (MoneyStr == "整")
            {
                MoneyStr = "零元整";
            }
            return MoneyStr;
        }

        #region  AMO
        /// <summary>
        /// 提取字符串中的数字字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNumberInString(String str)
        {
            string returnStr = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {

                if (Char.IsNumber(str, i) == true)
                {
                    string sdfsdfsd = str.Substring(i, 1);
                    returnStr += str.Substring(i, 1);
                }
                //else
                //{

                //    string sss = str.Substring(i, 1);
                //    if (str.Substring(i, 1) == "A" || str.Substring(i, 1) == " ")
                //    {
                //        returnStr += str.Substring(i, 1);
                //    }
                //}

            }
            return returnStr;
        }
        public static void GetNumberInString(string str, ref string beforeNumber, ref string numberStr, ref string afterNumber)
        {
            if (str.Length == 0)
                return;
            bool before = true;
            bool after = false;
            if (Char.IsNumber(str.ToCharArray()[0]) == true)
            {
                beforeNumber = "";
                before = false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str, i) == true && !after)
                {
                    before = false;
                    numberStr += str.Substring(i, 1);
                }
                else
                {
                    if (before)
                        beforeNumber += str.Substring(i, 1);
                    else
                    {
                        after = true;
                        afterNumber += str.Substring(i, 1);
                    }
                }

            }
        }
        /// <summary>
        /// 高颈法兰重量公式
        /// </summary>
        /// <param name="spec">规格 φD/B </param>
        /// <param name="C">厚度</param>
        /// <param name="H">H</param>
        /// <param name="H1">H1</param>
        /// <param name="N">N</param>
        /// <param name="A">A</param>
        /// <param name="S">S</param>
        /// <returns></returns>
        public static double GetWeldingNeckFlangePerWeight(string spec, int C, int H, int H1, int N, int A, int S)
        {
            spec = spec.Replace("φ", "").Replace("Φ", "");
            string[] specArray = spec.Split(new char[] { '/' });
            int D = Convert.ToInt32(specArray[0]);
            int B = Convert.ToInt32(specArray[1]);
            double weight = ((C * (D * D - B * B) / 4 + (H - H1 - C) * (N * N + N * A + A * A) / 12 - (H - H1 - C) * (B * B + B * (A - 2 * S) + (A - 2 * S) * (A - 2 * S)) / 12 - (A - S) * S * H1) * 7.85 * Math.PI) / 1000000.0;
            return weight;
        }
        /// <summary>
        /// 获取圆管多棱管展开长度
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static string GetSteelPipeSpreadLength(string spec, int N)
        {
            spec = spec.Replace("D", "").Replace("φ", "").Replace("Φ", "");
            if (spec.Contains("/") && spec.Contains("*"))
            {
                string[] specArray1 = spec.Split("*".ToCharArray());
                string D1 = specArray1[0];
                string C1 = specArray1[1];
                string[] DArray = D1.Split(new char[] { '/' });
                return GetSteelPipeSpreadLength(DArray[0] + "*" + C1, N) + "/" + GetSteelPipeSpreadLength(DArray[1] + "*" + C1, N);
            }
            else
            {
                string[] specArray = spec.Split("*".ToCharArray());
                int D = Convert.ToInt32(specArray[0]);
                int C = Convert.ToInt32(specArray[1]);
                double k = 1.2;
                if (N == 6)
                    k = 1.87;
                else if (N == 8)
                    k = 1.67;
                else if (N == 12)
                    k = 1.3;
                else if (N == 16)
                    k = 1.2;
                else if (N == 18)
                    k = 1.1;
                else if (N > 18)
                    k = 1.0;
                int len = 0;
                if (N < 3)
                    len = Convert.ToInt32(Math.PI * (D - 1.2 * C));
                else
                    len = Convert.ToInt32(Math.Tan(Math.PI / N) * N * (D - k * C));
                return len.ToString();
            }
        }
        public static string GetFactoryName()
        {
            string sqlstr = "SELECT top 1 ISNULL(fCnName,'') AS fCnName FROM B_FactoryName";
            return YKDataClass.GetSingle(sqlstr).ToString();
        }
        public static System.Data.DataTable GetFactoryNameToTable()
        {
            string sqlstr = "SELECT top 1 * FROM B_FactoryName";
            return YKDataClass.getDataTable(sqlstr);
        }

        
        /// <summary>
        /// 分类小计，并有合计行，
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nColHeJi">需要显示‘合计’字段的列</param>
        /// <param name="colsHeJi">需要合计的列</param>
        public static void SummaryDataTable(ref DataTable dt, string sColHeJi,string sColXiaoJi,List<string> colsHeJi)
        {
            DataRow dr = dt.NewRow();
            dr[sColHeJi] = "合计";
            dt.Rows.Add(dr);
            //初始化合计数组
            decimal[] arrDec = new decimal[colsHeJi.Count];
            for (int i = 0; i < colsHeJi.Count; i++)
            {
                arrDec[i] = decimal.Zero;
            }
            //合计
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j < colsHeJi.Count; j++)
                {
                    string cName = colsHeJi[j];
                    if (Convert.IsDBNull(dt.Rows[i][cName])) continue;
                    arrDec[j] += Convert.ToDecimal(dt.Rows[i][cName] + "");
                }
            }
            for (int i = 0; i < colsHeJi.Count; i++)
            {
                string cName = colsHeJi[i];
                if (arrDec[i] == decimal.Zero) dr[cName] = DBNull.Value;
                else dr[cName] = arrDec[i];
            }
            if (dt.Rows.Count <= 1) return;
            //小计
            string sRate = "";
            int currSubSumCol = dt.Rows.Count - 2;
            ArrayList ar = new ArrayList();
            for (int i = dt.Rows.Count - 2; i >= 0; i--)
            {
                string currRate = dt.Rows[i][sColXiaoJi] + "";
                if (sRate != currRate)
                {
                    if (i != dt.Rows.Count - 2)
                    {
                        dr = dt.NewRow();
                        dr[sColHeJi] = "小计";
                        for (int j = 0; j < colsHeJi.Count; j++)
                        {
                            string cName = colsHeJi[j];
                            if (arrDec[j] == decimal.Zero) dr[cName] = DBNull.Value;
                            else dr[cName] = arrDec[j];
                        }
                        dt.Rows.InsertAt(dr, currSubSumCol + 1);
                    }
                    currSubSumCol = i;
                    sRate = currRate;
                    for (int j = 0; j < colsHeJi.Count; j++)
                    { //归零
                        arrDec[j] = decimal.Zero;
                    }
                }
                for (int j = 0; j < colsHeJi.Count; j++)
                {
                    string cName = colsHeJi[j];
                    if (Convert.IsDBNull(dt.Rows[i][cName])) continue;
                    arrDec[j] += Convert.ToDecimal(dt.Rows[i][cName] + "");
                }
                if (i == 0)
                {
                    dr = dt.NewRow();
                    dr[sColHeJi] = "小计";
                    for (int j = 0; j < colsHeJi.Count; j++)
                    {
                        string cName = colsHeJi[j];
                        if (arrDec[j] == decimal.Zero) dr[cName] = DBNull.Value;
                        else dr[cName] = arrDec[j];
                    }
                    dt.Rows.InsertAt(dr, currSubSumCol + 1);
                }
            }
             
        }

        #endregion
    }
}
