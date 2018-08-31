using FZYK .Core;
using System;
using System .Collections .Generic;
using System .ComponentModel;
using System .Data;
using System .Drawing;
using System .Linq;
using System .Reflection;
using System .Text;
using System .Threading .Tasks;
using System .Windows .Forms;

namespace FZYK .WinUI
{
      public partial class HostMain : Form
      {
            public HostMain()
            {
                  InitializeComponent();
            }

            private void btnNesting_Click(object sender, EventArgs e)
            {
                  try
                  {
                        Assembly _assembly = Assembly .LoadFile(System .IO .Path .GetDirectoryName(Assembly .GetExecutingAssembly() .Location) + "\\FZYK.Nesting.dll");
                        Form frm = _assembly .CreateInstance("FZYK.Nesting.FrmCut") as Form;
                        frm .Show();
                  }
                  catch (Exception ex) { throw ex; }
            }
      }
}
