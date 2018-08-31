using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using FZYK.WinCtrl;
using FZYK.Com;
using System.Reflection;

namespace FZYK.Core
{
    /// <summary>
    /// 公用方法类
    /// </summary>
    public partial class DevCommon
    {
        /// <summary>
        /// 重置一个控件里的所有控件(可以是面板) 
        /// 如果是多层容器控件，请传入要重置控件所在的上一层容器控件
        /// </summary>
        /// <param name="control">容器控件</param>
        public static void Reset(Control control)
        {

            foreach (Control co in control.Controls)
            {
                string mytext = co.GetType().ToString();
                if (mytext != "FZYK.WinCtrl.YKLabel" && mytext != "System.Windows.Forms.Label")
                {
                    if (mytext == "FZYK.WinCtrl.YKTextBox" || mytext == "FZYK.WinCtrl.YKTextBoxSplit" || mytext == "System.Windows.Forms.TextBox")
                        ((TextBox)co).Text = "";
                    else if ((mytext == "FZYK.WinCtrl.YKComboBox" || mytext == "System.Windows.Forms.ComboBox") && ((ComboBox)co).Items.Count > 0)
                        ((ComboBox)co).SelectedIndex = 0;
                    else if (mytext == "FZYK.WinCtrl.YKCheckBox" || mytext == "System.Windows.Forms.CheckBox")
                        ((CheckBox)co).Checked = false;
                    else if (mytext == "FZYK.WinCtrl.YKDateTimePicker" || mytext == "System.Windows.Forms.DateTimePicker")
                    {
                        ((DateTimePicker)co).Value = DateTime.Now;
                        ((DateTimePicker)co).Checked = false;
                    }
                    else if (mytext == "FZYK.WinCtrl.YKDateTimePickerPopup")
                    {
                        ((YKDateTimePickerPopup)co).Text = "";
                    }
                    else if (mytext == "FZYK.WinCtrl.YKNumericUpDown" || mytext == "System.Windows.Forms.NumericUpDown")
                        ((NumericUpDown)co).Value = ((NumericUpDown)co).Minimum;
                }
            }
        }

        #region 将控件的值赋给实体相对应的属性 或将实体赋值给控件  添加日期：2008-4-24 成志岭 纪念YDC
        /// <summary>
        /// 将控件的值赋给实体相对应的属性
        /// </summary>
        /// <param name="panel">控件所在的容器</param>
        /// <param name="str">控件前缀列表，以逗号(半角)隔开，顺序：1.TextBox的前缀；2时间控件的前缀；3checkbox的前缀</param>
        /// 如容器中没有时间控件或下拉框控件，以空字符串代替其前缀
        /// <param name="obj">实体</param>
        public static void SetObjectValue(Control panel, string str, ref object obj)
        {
            string[] flag = str.Split(',');//存储控件的前缀，最少一种，最多3种，依次是TextBox，DateTimePicker,checkbox
            foreach (Control content in panel.Controls)
            {
                if (content.Name != null)
                {
                    if (content.GetType() == typeof(TextBox) || content.GetType() == typeof(YKTextBox) || content.GetType() == typeof(YKTextBoxSplit))
                    {
                        PropertyInfo propertyInfo = content.GetType().GetProperty("Text");
                        if (propertyInfo != null)
                            SetProperty(obj, content.Name.Replace(flag[0], ""), propertyInfo.GetValue(content, null));
                    }
                    else if ((content.GetType() == typeof(DateTimePicker) || content.GetType() == typeof(YKDateTimePicker) || content.GetType() == typeof(YKDateTimePickerPopup))
                        && flag.Length > 1)
                    {
                        if (flag[1].Length > 0)
                        {
                            PropertyInfo propertyInfo = content.GetType().GetProperty("Text");
                            if (propertyInfo != null)
                                SetProperty(obj, content.Name.Replace(flag[1], ""), propertyInfo.GetValue(content, null));
                        }
                    }
                    else if ((content.GetType() == typeof(CheckBox) || content.GetType() == typeof(YKCheckBox))
                        && flag.Length > 2)
                    {
                        if (flag[2].Length > 0)
                        {
                            PropertyInfo propertyInfo = content.GetType().GetProperty("Checked");
                            if (propertyInfo != null)
                                SetProperty(obj, content.Name.Replace(flag[2], ""), propertyInfo.GetValue(content, null));
                        }
                    }
                    else if (content.GetType() != typeof(Label) && content.GetType() != typeof(YKLabel)
                        && content.GetType() != typeof(Button) && content.GetType() != typeof(YKButton)
                        && content.GetType() != typeof(DataGridView) && content.GetType() != typeof(YKDataGridView)
                        && content.GetType() != typeof(ComboBox) && content.GetType() != typeof(YKComboBox))
                        SetObjectValue(content, str, ref obj);
                }
            }
        }
        /// <summary>
        /// 将实体属性值赋给对应的控件值
        /// </summary>
        /// <param name="panel">控件所在的容器</param>
        /// <param name="str">控件前缀列表，以逗号(半角)隔开，顺序：1.TextBox的前缀；2时间控件的前缀；3checkbox的前缀</param>
        /// 如容器中没有时间控件或下拉框控件，以空字符串代替其前缀
        /// <param name="obj">实体</param>
        public static void SetControlValue(Control panel, string str, object obj)
        {
            string[] flag = str.Split(',');
            foreach (Control content in panel.Controls)
            {
                if (content.Name != null)
                {
                    if (content.GetType() == typeof(TextBox) || content.GetType() == typeof(YKTextBox) || content.GetType() == typeof(YKTextBoxSplit))
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(content.Name.Replace(flag[0], ""));
                        if (propertyInfo != null)
                            SetProperty(content, "Text", propertyInfo.GetValue(obj, null));
                    }
                    else if ((content.GetType() == typeof(DateTimePicker) || content.GetType() == typeof(YKDateTimePicker) || content.GetType() == typeof(YKDateTimePickerPopup))
                        && flag.Length > 1)
                    {
                        if (flag[1].Length > 0)
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(content.Name.Replace(flag[1], ""));
                            if (propertyInfo != null)
                            {
                                object o = propertyInfo.GetValue(obj, null);
                                if (o.ToString().Length > 0 && !o.ToString().StartsWith("0"))
                                    SetProperty(content, "Text", o);
                            }
                        }
                    }
                    else if ((content.GetType() == typeof(CheckBox) || content.GetType() == typeof(YKCheckBox))
                        && flag.Length > 2)
                    {
                        if (flag[2].Length > 0)
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(content.Name.Replace(flag[2], ""));
                            if (propertyInfo != null)
                                ((CheckBox)content).Checked = (bool)propertyInfo.GetValue(obj, null);
                        }
                    }
                    else if (content.GetType() != typeof(Label) && content.GetType() != typeof(YKLabel)
                        && content.GetType() != typeof(Button) && content.GetType() != typeof(YKButton)
                        && content.GetType() != typeof(DataGridView) && content.GetType() != typeof(YKDataGridView)
                        && content.GetType() != typeof(ComboBox) && content.GetType() != typeof(YKComboBox))
                        SetControlValue(content, str, obj);
                }
            }
        }
        /// <summary>
        /// 设置对象属性的值
        /// </summary>
        private static void SetProperty(object obj, string name, object value)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(name);
            if (propertyInfo != null && value != null)
            {
                if (value.ToString().Trim().Length == 0 && propertyInfo.PropertyType != typeof(string))
                    return;
                object objValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, objValue, null);
            }
        }
        #endregion


         
        /// <summary>
        /// 清空YKDataGridView
        /// </summary>
        /// <param name="dgv">YKDataGridView控件对象</param>
        public static void ClearDGV(YKDataGridView dgv)
        {
            while (dgv.Rows.Count > 0 && !dgv.Rows[0].IsNewRow)
            {
                dgv.Rows.RemoveAt(0);
            }
        }

        public static void InitDgvColumnVisible(string dgvName, YKDataGridView dgv)
        {
            string sqlstr = "SELECT * FROM B_ColumnVisibleSetting WHERE cvsFormName='" + dgvName + "'";
            DataTable dt = Com.YKDataClass.getDataTable(sqlstr);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (dgv.Columns.Contains(Convert.ToString(row["cvsColName"])))
                    {
                        int iVis = Convert.ToInt32(row["cvsVisible"]);
                        dgv.Columns[Convert.ToString(row["cvsColName"])].Visible = (iVis == 1 ? true : false);
                    }
                }
            }
        }

        /// <summary>
        /// 根据主界面窗体类名获取主界面查找条件
        /// 添加时间：2012年7月26日 11:02:01 添加人：AMO        
        /// </summary>
        /// <param name="ClassName">窗体类名 如 FZYK.WinUI.FangYang.FrmMaterialMain</param>
        /// <returns></returns>
        public static DataTable GetSqlFilter(string ClassName)
        {
            string sqlstr = "SELECT * FROM dbo.B_SqlFilter AS bsf WHERE sfClassName='" + ClassName + "' ORDER BY sfColumnIndex ASC";
            return FZYK.Com.YKDataClass.getDataTable(sqlstr);
        }
    }
}
