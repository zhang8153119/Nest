using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace FZYK.Core
{
    public partial class BaseToolBox : DockContent
    {
        public BaseToolBox()
        {
            InitializeComponent();           
        }
        public void Connect(IFrmMainToolBox iFrmMainToolBox)
        {
            _iFrmMainToolBox = iFrmMainToolBox;
        }
        public IFrmMainToolBox _iFrmMainToolBox;
        public virtual void LoadData() { }
        public void ShowChildrenFormWithArgs(string frmName, string frmText, object[] args, string frmTitle, string className)
        {
            _iFrmMainToolBox.ShowChildrenFormWithArgs(frmName, frmText, args, frmTitle, className);
        }
        public void ShowChildrenFormWithNotify(string frmName, string frmText, object[] args, string frmTitle, string sqlwhere, string className)
        {
            _iFrmMainToolBox.ShowChildrenFormWithNotify(frmName, frmText, args, frmTitle, sqlwhere, className);
        }
        public void ShowChildrenForm(string frmName, string frmText, string className)
        {
            _iFrmMainToolBox.ShowChildrenForm(frmName, frmText, className);
        }
        public void ShowSingleFormWithArgs(string frmName, string frmText, object[] args, string frmTitle, string className)
        {
            _iFrmMainToolBox.ShowSingleFormWithArgs(frmName, frmText, args, frmTitle, className);
        }
        public void ShowSingleForm(string frmName, string frmText, string className)
        {
            _iFrmMainToolBox.ShowSingleForm(frmName, frmText, className);
        }
    }
}
