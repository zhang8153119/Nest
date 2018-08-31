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
        
        #region yan 2012-07-05 绑定树节点
        /// <summary>
        /// 绑定部门树节点，绑定有上下级的树节点
        /// </summary>
        /// <param name="YKTreeView1">树节点</param>
        public static void BingTreeViewDept(TreeView TreeView1, DataTable dt, string fID, string fNum, string fName, string fParentNum)
        {
            TreeView1.Nodes.Clear();
            //DataTable dt = GetAllData();
            //string fid = "dNum";
            //string fName = "dName";
            string ParentNum = "";
            TreeNode tempNd = null;
            AddTreeByParentID(TreeView1, tempNd, dt, ParentNum, fID, fNum, fName, fParentNum);
        }
        /// <summary>
        /// 根据父节添加子节点
        /// </summary>
        /// <param name="tmpNd"></param>
        /// <param name="tempDt"></param>
        /// <param name="ParentNum"></param>
        /// <param name="fNum"></param>
        /// <param name="fName"></param>
        private static void AddTreeByParentID(TreeView YKTreeView1, TreeNode tmpNd, DataTable tempDt, string ParentNum, string fID, string fNum, string fName, string fParentNum)
        {

            DataView dvroles = new DataView(tempDt);
            dvroles.RowFilter = "" + fParentNum + "='" + ParentNum + "'";
            foreach (DataRowView drvrole in dvroles)
            {
                TreeNode childNd = new TreeNode();
                //YKTreeNode childNd = new YKTreeNode();
                childNd.Tag = drvrole[fID].ToString();//ID
                childNd.Text = drvrole[fName].ToString();//Name
                childNd.Text = drvrole[fName].ToString() + "(" + drvrole[fNum] + ")";
                if (tmpNd == null)
                    YKTreeView1.Nodes.Add(childNd);
                else
                {
                    tmpNd.Nodes.Add(childNd);

                }

                AddTreeByParentID(YKTreeView1, childNd, tempDt, drvrole[fNum].ToString(), fID, fNum, fName, fParentNum);
            }
            tempDt.Dispose();
        }
        /// <summary>
        /// 绑定没有上下级的树节点
        /// </summary>
        /// <param name="TreeView1">树名称</param>
        /// <param name="dt">数据集</param>
        /// <param name="fID">ID名</param>
        /// <param name="fNum"></param>
        /// <param name="fName"></param>
        public static void BingTreeViewCustomer(TreeView TreeView1, DataTable dt, string fID, string fNum, string fName)
        {
            TreeView1.Nodes.Clear();
            DataView dvroles = new DataView(dt);
            foreach (DataRowView drvrole in dvroles)
            {
                TreeNode childNd = new TreeNode();
                //YKTreeNode childNd = new YKTreeNode();
                childNd.Tag = drvrole[fID].ToString();//ID
                childNd.Text = drvrole[fName].ToString();//Name
                childNd.Text = drvrole[fName].ToString() + "(" + drvrole[fNum] + ")";
                TreeView1.Nodes.Add(childNd);
            }
        }
        #endregion

        
        /// <summary>
        /// 绑定包装清单的树节点，根据包状态进行加载
        /// 鄢国平 2012-09-13
        /// </summary>
        /// <param name="TN"></param>
        /// <param name="dt"></param>
        public static void BindPackState(TreeNode TN, DataTable dt)
        {
            TN.Nodes.Clear();
            DataView dvroles = new DataView(dt);
            foreach (DataRowView drvrole in dvroles)
            {
                TreeNode childNd = new TreeNode();
                childNd.Tag = drvrole["tID"].ToString();//ID
                childNd.Text = drvrole["tName"].ToString();//Name
                TN.Nodes.Add(childNd);
            }
        }

        /// <summary>
        /// 将Datatable绑定下拉框,有空白 
        /// 鄢国平 2012-11-09
        /// </summary>
        /// <param name="ddl">下拉框</param>
        /// <param name="dt">数据源</param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        public static void ComboBoxBindHavingBlank(YKComboBox ddl, DataTable dt, string TextField, string ValueField)
        {
            DataRow dr = dt.NewRow();
            dr[TextField] = "";
            dr[ValueField] = 0;
            dt.Rows.Add(dr);
            ddl.DataSource = dt;
            ddl.DisplayMember = TextField;
            if (ValueField.Trim().Length > 0)
                ddl.ValueMember = ValueField;
        }
        /// <summary>
        /// 得到当前树借点的最高父节点，鄢国平 2012-11-26
        /// </summary>
        /// <param name="tnParent"></param>
        /// <param name="tagTxt"></param>
        /// <returns></returns>
        public static TreeNode GetTheTopParent(TreeNode tn)
        {
            if (tn.Parent == null)
                return tn;
            else
                return GetTheTopParent(tn.Parent);
        }
        
    }
}
