using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Crownwood.DotNetMagic.Controls;

namespace FZYK.Core
{
    public partial class FrmToolBoxCheJinXML : BaseToolBox
    {
        public FrmToolBoxCheJinXML()
        {
            InitializeComponent();
        }
        public FrmToolBoxCheJinXML(string xmlName)
            :this()
        {
            _xmlName = xmlName;
        }
        private string _xmlName = "";
        private void FrmToolBoxCheJinXML_Load(object sender, EventArgs e)
        {
            treeControl1.SetTreeControlStyle(Crownwood.DotNetMagic.Controls.TreeControlStyles.GroupOfficeLight);           
            LoadData();
        }
        public override void LoadData()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            XmlDocument _xmldoc = new XmlDocument();
            _xmldoc.Load(path + _xmlName);
            BindTree(treeControl1, _xmldoc);
           
        }
        /// <summary>
        /// 动态绑定TreeView，这里只限XML树为三级。
        /// </summary>
        private void BindTree(Crownwood.DotNetMagic.Controls.TreeControl treeControl, XmlDocument _xmldoc)
        {

            XmlNodeList _list = _xmldoc.DocumentElement.ChildNodes;
            foreach (XmlElement elem in _list)
            {
                string strText = elem.Attributes["name"].Value;
                Crownwood.DotNetMagic.Controls.Node treeNode = new Crownwood.DotNetMagic.Controls.Node(strText);
                treeControl.Nodes.Add(treeNode);
                XmlNodeList clist = elem.ChildNodes;
                if (clist.Count > 0)
                {
                    foreach (XmlElement celem in clist)
                    {
                        if (celem.ChildNodes.Count > 0)
                        {
                            string cText = celem.Attributes["name"].Value;
                            Crownwood.DotNetMagic.Controls.Node ctreeNode = new Crownwood.DotNetMagic.Controls.Node(cText);
                            ctreeNode.Tag = null;
                            treeNode.Nodes.Add(ctreeNode);
                            foreach (XmlElement elem3 in celem.ChildNodes)
                            {
                                string ctext3 = elem3.Attributes["name"].Value;
                                Crownwood.DotNetMagic.Controls.Node ctreeNode3 = new Crownwood.DotNetMagic.Controls.Node(ctext3);
                                ctreeNode3.Tag = elem3.Attributes["url"].Value + "&" + elem3.Attributes["class"].Value;
                                ctreeNode.Nodes.Add(ctreeNode3);
                            }
                        }
                        else
                        {
                            string cText = celem.Attributes["name"].Value;
                            Crownwood.DotNetMagic.Controls.Node ctreeNode = new Crownwood.DotNetMagic.Controls.Node(cText);
                            ctreeNode.Tag = celem.Attributes["url"].Value + "&" + celem.Attributes["class"].Value;
                            treeNode.Nodes.Add(ctreeNode);
                        }
                    }
                }
            }
        }

        private void treeControl1_Click(object sender, EventArgs e)
        {
            if (((TreeControl)sender).SelectedNode != null && ((TreeControl)sender).SelectedNode.Tag != null)
            {
                string frmName = "";
                string className = "";
                string frmName1 = "";
                string[] tags = ((TreeControl)sender).SelectedNode.Tag.ToString().Split(new char[] { '&' });
                frmName1 = frmName = tags[0];
                className = tags[1];
                object[] args = new object[1];//将要传给显示窗体的参数数组
                //规定将要传递的参数(现只限一个)写在XML节点url属性后面用@分隔
                if (frmName.IndexOf("@") > 0)//存在传递的参数
                {
                    string[] strlist = frmName.Split(new char[] { '@' });//分离窗体名称和参数
                    frmName = strlist[0];//窗体名
                    if (strlist[1].Length > 0)
                        args[0] = strlist[1];//参数
                }
                if (!CanSee(frmName) && !CanSee(frmName1))
                {
                    Core.YKMessageBox.ShowBox("您没权限查看此菜单的内容！");
                    return;
                }
                //else
                //    frmName = ((TreeControl)sender).SelectedNode.Tag.ToString();//不存在参数则用url全部
                string frmText = ((TreeControl)sender).SelectedNode.Text;


                if (args[0] != null && args[0].ToString().Length > 0)//如果存在参数，调用带参数的反射方法
                {
                    ShowChildrenFormWithArgs(frmName, ((TreeControl)sender).SelectedNode.Tag.ToString(), args, frmText, className);
                }
                else//如果不存在参数，调用无参数反射方法
                    ShowChildrenForm(frmName, frmText, className);
            }
        }
        private bool CanSee(string frmName)
        {
            frmName = frmName.Trim();
            if (Com.UserInfo.eIBase == "1" && frmName == "FZYK.WinUI.BaseSet.FrmAdminPopedom")
                return true;
            if (Com.UserInfo.htRights != null && Com.UserInfo.htRights.ContainsKey(frmName) && Com.UserInfo.htRights[frmName].ISee)
            {
                return true;
            }
            else
            {
                if (frmName.IndexOf("@") > 0)//存在传递的参数
                {
                    string[] strlist = frmName.Split(new char[] { '@' });//分离窗体名称和参数
                    frmName = strlist[0];//窗体名
                    return CanSee(frmName);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
