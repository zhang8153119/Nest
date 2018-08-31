using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using FZYK.WinCtrl;

namespace FZYK.Core
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FrmSetColumnVisible : Form
    {
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public FrmSetColumnVisible()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Info"></param>
        public FrmSetColumnVisible(SetDgvColumnInfo Info)
            : this()
        {
            _listInfos.Add(Info);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInfos"></param>
        public FrmSetColumnVisible(List<SetDgvColumnInfo> listInfos)
            : this()
        {
            _listInfos = listInfos;
        }
        #endregion

        #region 参数
        private SetDgvColumnInfo _Info;
        private List<SetDgvColumnInfo> _listInfos = new List<SetDgvColumnInfo>(2);
        #endregion

        #region load和shown事件

        private void FrmSetColumnVisible_Load(object sender, EventArgs e)
        {
             
        }

        private void FrmSetColumnVisible_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            dgvColumns.AutoGenerateColumns = false;
            BindCmb();
            BindGridView(cmbdgvList.SelectedValue.ToString());
            
        }

        #endregion

        #region 方法
        private void BindCmb()
        {
            cmbdgvList.SelectedIndexChanged-=new EventHandler(cmbdgvList_SelectedIndexChanged);
            cmbdgvList.DataSource = _listInfos;
            cmbdgvList.DisplayMember = "dgvText";
            cmbdgvList.ValueMember = "dgvName";
            cmbdgvList.SelectedIndex = 0;
            cmbdgvList.SelectedIndexChanged += new EventHandler(cmbdgvList_SelectedIndexChanged);
        }
        private void BindGridView(string dgvName)
        {
            string sqlstr = "SELECT * FROM B_ColumnVisibleSetting WHERE cvsFormName='" + dgvName + "'";
            DataTable dt = Com.YKDataClass.getDataTable(sqlstr);
            _Info = GetDGV(dgvName);
            foreach (DataGridViewColumn col in _Info.dgv.Columns)
            {
                if (dt.Select("cvsColName='" + col.Name + "'").Length==0)
                {
                    DataRow row = dt.NewRow();
                    row["cvsID"] = 0;
                    row["cvsFormName"] = _Info.dgvName;
                    row["cvsFormText"] = _Info.dgvText;
                    row["cvsColName"] = col.Name;
                    row["cvsColText"] = col.HeaderText;
                    row["cvsVisible"] = col.Visible == true ? 1 : 0;
                    row["cvsOldVisible"] = col.Visible == true ? 1 : 0;
                    dt.Rows.Add(row);
                }
            }
            dt.AcceptChanges();
            dgvColumns.DataSource = dt;
            foreach (DataGridViewRow row in dgvColumns.Rows)
            {
                if (row.Cells["cvsOldVisible"].Value != DBNull.Value
                    && Convert.ToInt32(row.Cells["cvsOldVisible"].Value) == 0)
                    row.Visible = false;
            }
                
        }
        private SetDgvColumnInfo GetDGV(string dgvName)
        {
            foreach (SetDgvColumnInfo info in _listInfos)
            {
                if (info.dgvName == dgvName)
                    return info;
            }
            return null;
        }
        #endregion

        private void btnSure_Click(object sender, EventArgs e)
        {
            dgvColumns.EndEdit();
            List<string> listsql = new List<string>(2);
            foreach (DataGridViewRow row in dgvColumns.Rows)
            {
                int mycvsID = Convert.ToInt32(row.Cells["cvsID"].Value);
                if (mycvsID > 0)
                {
                    listsql.Add("UPDATE B_ColumnVisibleSetting SET cvsVisible=" + Convert.ToInt32(row.Cells["cvsVisible"].Value) + " WHERE cvsID=" + mycvsID);

                }
                else
                {
                    listsql.Add(" INSERT INTO B_ColumnVisibleSetting(cvsFormName,cvsFormText,cvsColName,cvsColText,cvsVisible,cvsOldVisible) "
                        + " VALUES('" + _Info.dgvName + "','" + _Info.dgvText + "','" + Convert.ToString(row.Cells["cvsColName"].Value)
                        + "','" + Convert.ToString(row.Cells["cvsColText"].Value) + "'," + Convert.ToInt32(row.Cells["cvsVisible"].Value) + ","
                        + Convert.ToInt32(row.Cells["cvsOldVisible"].Value) + ")");

                }
            }
            Com.YKDataClass.SqlCommandTrans(listsql);
            Core.DevCommon.InitDgvColumnVisible(_Info.dgvName, _Info.dgv);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbdgvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(cmbdgvList.SelectedValue.ToString());
        }

        #region 事件

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetDgvColumnInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public SetDgvColumnInfo()
        {
        }
        private string _dgvName = "";
        /// <summary>
        /// dgv的name属性 必须唯一
        /// </summary>
        public string dgvName
        {
            get { return _dgvName; }
            set { _dgvName = value; }
        }
        private string _dgvText = "";
        /// <summary>
        /// dgv的文字描述
        /// </summary>
        public string dgvText
        {
            get { return _dgvText; }
            set { _dgvText = value; }
        }
        private YKDataGridView _dgv;
        /// <summary>
        /// dgv
        /// </summary>
        public YKDataGridView dgv
        {
            get { return _dgv; }
            set { _dgv = value; }
        }
    }
}
