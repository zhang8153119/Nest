using System;
using System .Collections .Generic;
using System .ComponentModel;
using System .Data;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Windows .Forms;
using FZYK .Core;
using FZYK .Com;
using FZYK .Nest .db;
using System .IO;

namespace FZYK .Nest
{
      public partial class FrmNestSet : BaseSelectDialog
      {
            public FrmNestSet()
            {
                  InitializeComponent();
            }
            // 声明INI文件的写操作函数 WritePrivateProfileString()  
            [System .Runtime .InteropServices .DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            // 声明INI文件的读操作函数 GetPrivateProfileString()  
            [System .Runtime .InteropServices .DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, System .Text .StringBuilder retVal, int size, string filePath);

            NestDB _db = new NestDB();

            private void FrmNestSet_Shown(object sender, EventArgs e)
            {
                  BindSet();
            }
            private bool Check()
            {
                  if (!YKPageValidate .IsDecimal(txtProtect .Text))
                  {
                        YKMessageBox .ShowBox("保护量请输入数字");
                        return false;
                  }

                  if (!YKPageValidate .IsDecimal(txtSize .Text))
                  {
                        YKMessageBox .ShowBox("小件长宽请输入数字");
                        return false;
                  }
                  if (!YKPageValidate .IsDecimal(txtT .Text))
                  {
                        YKMessageBox .ShowBox("栅格精度请输入数字");
                        return false;
                  }
                  if (!YKPageValidate .IsDecimal(txtPop .Text))
                  {
                        YKMessageBox .ShowBox("内置参数请输入数字");
                        return false;
                  }
                  if (!YKPageValidate .IsDecimal(txtCross .Text))
                  {
                        YKMessageBox .ShowBox("内置参数请输入数字");
                        return false;
                  }
                  if (!YKPageValidate .IsDecimal(txtMutation .Text))
                  {
                        YKMessageBox .ShowBox("内置参数请输入数字");
                        return false;
                  }
                  return true;
            }
            /// <summary>
            /// 绑定
            /// </summary>
            private void BindSet()
            {
                  Dictionary<string, string> dic = new Dictionary<string, string>();
                  dic = _db .GetSet();
                  txtProtect .Text = dic["Protect"] .ToString();
                  cmbType .Text = dic["Type"] .ToString();
                  txtSize .Text = dic["Size"] .ToString();
                  txtT .Text = dic["T"] .ToString();
                  txtPop .Text = dic["Pop"] .ToString();
                  txtCross .Text = dic["Cross"] .ToString();
                  txtMutation .Text = dic["Mutation"] .ToString();
                  cmbRotate .Text = dic["Rotate"] .ToString();
                  txtpath .Text = Getstr();
            }
            /// <summary>
            /// 保存
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnSure_Click(object sender, EventArgs e)
            {
                  if (!Check())
                        return;
                  Dictionary<string, string> dic = new Dictionary<string, string>();
                  dic .Add("Protect", txtProtect .Text);
                  dic .Add("Type", cmbType .Text);
                  dic .Add("Size", txtSize .Text);
                  dic .Add("T", txtT .Text);
                  dic .Add("Pop", txtPop .Text);
                  dic .Add("Cross", txtCross .Text);
                  dic .Add("Mutation", txtMutation .Text);
                  dic .Add("Rotate", cmbRotate .Text);
                  _db .SaveSet(dic);
                  Writeini(txtpath .Text);
                  this .DialogResult = DialogResult .OK;
                  this .Close();
            }
            /// <summary>
            /// 取消
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnCancel_Click(object sender, EventArgs e)
            {
                  this .DialogResult = DialogResult .Cancel;
                  this .Close();
            }

            private void txtpath_BtnOnclick(object sender, EventArgs e)
            {
                  string path = string .Empty;
                  System .Windows .Forms .FolderBrowserDialog fbd = new System .Windows .Forms .FolderBrowserDialog();
                  if (fbd .ShowDialog() == System .Windows .Forms .DialogResult .OK)
                  {
                        path = fbd .SelectedPath;
                        txtpath .Text = path;
                  }
            }
            #region 读取ini
            private string Getstr()
            {
                  string path = Getini("NestConfig", "SavePath");

                  return path;
            }

            private string Getini(string select, string key)
            {
                  string currentpath = Directory .GetCurrentDirectory();
                  string path = currentpath + "\\nest.ini";
                  StringBuilder sb = new StringBuilder();
                  GetPrivateProfileString(select, key, "", sb, 1024, path);
                  return sb .ToString();
            }
            /// <summary> 
            /// 写入INI文件 
            /// </summary> 
            /// <param name="Section">项目名称(如 [TypeName] )</param> 
            /// <param name="Key">键</param> 
            /// <param name="Value">值</param> 
            public void Writeini(string Value)
            {
                  string currentpath = Directory .GetCurrentDirectory();
                  string path = currentpath + "\\nest.ini";
                  WritePrivateProfileString("NestConfig", "SavePath", Value, path);
            }
            #endregion;
      }
}
