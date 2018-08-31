using System;
using System .Collections .Generic;
using System .ComponentModel;
using System .Data;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;
using System .Windows .Forms;

namespace FZYK .Nest
{
      public partial class FrmText : Form
      {
            public FrmText(int[,] txt)
            {
                  InitializeComponent();
                  _txt = txt;
            }
            int[,] _txt;

            private void printvalue()
            {
                  string str = "";
                  for (int i = 0; i < _txt .GetLength(0); i++)
                  {
                        for (int j = 0; j < _txt .GetLength(1); j++)
                        {
                              str += _txt[i, j] .ToString() + " ";
                        }
                        str += "\r\n";
                  }
                  txtvalue .Text = str;
            }

            private void frmtext_Shown(object sender, EventArgs e)
            {
                  printvalue();
            }
      }
}
