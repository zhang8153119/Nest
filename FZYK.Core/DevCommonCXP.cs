using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FZYK.WinCtrl;
using System.Windows.Forms;
using System.Data;

namespace FZYK.Core
{
  public   partial class DevCommon
    {
        /// <summary>
        /// 给一个时间判断他属于哪年哪月的数据
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static string getYearAndMonth(DateTime dtime)
        {
            int aa = Convert.ToInt32(Com.UserInfo.configSys["ProduceFrom"]);
            if (aa == 1)
                return dtime.Year.ToString() + "-" + dtime.Month.ToString();
            else if (dtime.Day >= aa)
                return dtime.AddMonths(1).Year.ToString() + "-" + dtime.AddMonths(1).Month.ToString();
            else
                return dtime.Year.ToString() + "-" + dtime.Month.ToString();
        }

       

        /// <summary>
        /// DGV动态列增加文本列
        /// </summary>
        /// <param name="dgvTemp">DGV</param>
        /// <param name="colName">列名</param>
        /// <param name="colHeaderText">列头</param>
        /// <param name="DataPropertyName">属性名称</param>
        /// <param name="Tag">特殊要求</param>
        public static void AddGridColumnTextBox(YKDataGridView dgvTemp, string colName, string colHeaderText, string DataPropertyName, string Tag)
        {
            DataGridViewTextBoxColumn tmpCol = new DataGridViewTextBoxColumn();
            tmpCol.HeaderText = colHeaderText;
            tmpCol.Name = colName;
            tmpCol.DataPropertyName = DataPropertyName;
            tmpCol.Tag = Tag;
            dgvTemp.Columns.Add(tmpCol);
        }
        /// <summary>
        /// DGV动态列增加checkbox列
        /// </summary>
        /// <param name="dgvTemp">DGV</param>
        /// <param name="colName">列名</param>
        /// <param name="colHeaderText">列头</param>
        /// <param name="DataPropertyName">属性</param>
        /// <param name="Tag">特殊要求</param>
        public static void AddGridColumnCheckBox(YKDataGridView dgvTemp, string colName, string colHeaderText, string DataPropertyName, string Tag)
        {
            DataGridViewCheckBoxColumn tmpCol = new DataGridViewCheckBoxColumn();
            tmpCol.HeaderText = colHeaderText;
            tmpCol.Name = colName;
            tmpCol.DataPropertyName = DataPropertyName;
            tmpCol.Tag = Tag;
            dgvTemp.Columns.Add(tmpCol);
        }



        /// <summary>
        /// 将Datatable绑定下拉框
        /// </summary>
        /// <param name="ddl">下拉框</param>
        /// <param name="dt">数据源</param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        public static void ComboBoxBind(YKComboBox ddl, DataTable dt, string TextField, string ValueField)
        {
            ddl.DataSource = dt;
            ddl.DisplayMember = TextField;
            if (ValueField.Trim().Length > 0)
                ddl.ValueMember = ValueField;
        }

        /// <summary>
        /// 是否禁止DataGridView中所有列排序
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag"></param>
        public static void GVNoSort(DataGridView dgv, bool flag)
        {
            if (dgv.Columns.Count > 0)
            {
                if (flag)
                {
                    foreach (DataGridViewColumn dgvc in dgv.Columns)
                    {
                        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                else
                    foreach (DataGridViewColumn dgvc in dgv.Columns)
                    {
                        dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    }
            }
        }

       

        /// <summary>
        /// 通过年月查找该年月的生产起始日期及终止日期,flag=0获取起始，flag=1获取终止
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static DateTime[] getProduceFromOrToByMonth(int year, int month)
        {
            DateTime[] result = new DateTime[2];
            int aa;
            aa = Convert.ToInt32(Com.UserInfo.configSys["ProduceFrom"]);
            if (aa > 15)
            {
                if (month == 1)
                    result[0] = Convert.ToDateTime(Convert.ToString(year - 1) + "-12-" + aa);
                else
                    result[0] = Convert.ToDateTime(year.ToString() + "-" + (month - 1).ToString() + "-" + aa);
            }
            else
                result[0] = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-" + aa);
            result[1] = result[0].AddMonths(1).AddDays(-1);
            return result;
        }

      

    }
}
