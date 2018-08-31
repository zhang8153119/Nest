using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Windows .Forms;
using System .Data;
using System .Xml;
using FZYK .WinCtrl;
using FZYK .Com;
using System .Reflection;
using System .Text;
using ThoughtWorks .QRCode .Codec;
using ZXing .Common;
using ZXing;
using ZXing .QrCode;

namespace FZYK .Core
{
      /// <summary>
      /// 公用方法类
      /// </summary>
      public partial class DevCommon
      {
            /// <summary>
            /// 生成二维码图片
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static byte[] GetQRCodeNew(string str)
            {
                  EncodingOptions options = null;
                  BarcodeWriter writer = null;

                  options = new QrCodeEncodingOptions
                  {
                        DisableECI = true,
                        CharacterSet = "UTF-8",
                        Width = 150,
                        Height = 150
                        //Width = pictureBox1.Width,
                        //Height = pictureBox1.Height
                  };
                  writer = new BarcodeWriter();
                  writer .Format = BarcodeFormat .QR_CODE;
                  writer .Options = options;

                  System .Drawing .Bitmap bitmap = writer .Write(str);
                  System .IO .MemoryStream MStream = new System .IO .MemoryStream();
                  bitmap .Save(MStream, System .Drawing .Imaging .ImageFormat .Jpeg);
                  return MStream .GetBuffer();
            }
            /// <summary>
            /// 生成二维码图片
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static System .Drawing .Bitmap GetQRCode1(string str)
            {
                  QRCodeEncoder encoder = new QRCodeEncoder();
                  encoder .QRCodeEncodeMode = QRCodeEncoder .ENCODE_MODE .BYTE;
                  encoder .QRCodeVersion = 7;
                  encoder .QRCodeScale = 2;
                  encoder .QRCodeErrorCorrect = QRCodeEncoder .ERROR_CORRECTION .M;
                  //System .Drawing .Bitmap pic = encoder .Encode(str, Encoding .GetEncoding("GB2312"));
                  //System .IO .MemoryStream MStream = new System .IO .MemoryStream();
                  //pic .Save(MStream, System .Drawing .Imaging .ImageFormat .Bmp);
                  //return MStream .GetBuffer();
                  return encoder .Encode(str, Encoding .GetEncoding("UTF-8"));
            }
            /// <summary>
            /// 生成二维码图片
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static byte[] GetQRCode(string str)
            {
                  QRCodeEncoder encoder = new QRCodeEncoder();
                  encoder .QRCodeEncodeMode = QRCodeEncoder .ENCODE_MODE .BYTE;
                  encoder .QRCodeVersion = 7;
                  encoder .QRCodeScale = 4;
                  encoder .QRCodeErrorCorrect = QRCodeEncoder .ERROR_CORRECTION .M;
                  System .Drawing .Bitmap pic = encoder .Encode(str, Encoding .GetEncoding("UTF-8"));
                  System .IO .MemoryStream MStream = new System .IO .MemoryStream();
                  pic .Save(MStream, System .Drawing .Imaging .ImageFormat .Bmp);
                  return MStream .GetBuffer();
            }
            /// <summary>
            /// 获取tag
            /// 张镇波 2013-11-20
            /// </summary>
            /// <param name="tag"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public static string GetTag(string tag, int count)
            {
                  string tag1 = "";
                  string[] str = tag .Split(new char[] { '#' });
                  if (str .Length > count)
                  {
                        tag1 = str[count];
                  }
                  return tag1;
            }

            /// <summary>
            /// dgv转换成table 4-23 zhang修改
            /// </summary>
            /// <param name="dgv"></param>
            /// <returns></returns>
            public static DataTable DgvToTableEmpty(DataGridView dgv)
            {
                  DataTable dt = new DataTable();
                  for (int c = 0; c < dgv .Columns .Count; c++)
                  {
                        if (dgv .Columns[c] .Name != "")
                        {
                              DataColumn dc = new DataColumn(dgv .Columns[c] .Name);
                              dt .Columns .Add(dc);
                        }
                  }
                  return dt;
            }
            /// <summary>
            /// dgv转换成table 4-23 zhang增加（按数据绑定名称）
            /// </summary>
            /// <param name="dgv"></param>
            /// <returns></returns>
            public static DataTable DgvToTableEmptyByProperty(DataGridView dgv)
            {
                  DataTable dt = new DataTable();
                  for (int c = 0; c < dgv .Columns .Count; c++)
                  {
                        if (dgv .Columns[c] .DataPropertyName != "")
                        {
                              DataColumn dc = new DataColumn(dgv .Columns[c] .DataPropertyName);
                              dt .Columns .Add(dc);
                        }
                  }
                  return dt;
            }






            /// <summary>
            /// 创建树结构
            /// </summary>
            /// <param name="tv">需要创建的树</param>
            /// <param name="dt">需要绑定的数据</param>
            /// <param name="display">显示字段</param>
            /// <param name="display">显示在括号中的字段</param>
            /// <param name="father">父编码字段</param>
            public static void CreateTreeView(DataTable dt, TreeView tv, TreeNode tn, string id, string display, string display2, string fartherID)
            {
                  if (dt != null && dt .Rows .Count > 0)
                  {
                        DataView dv = new DataView(dt);
                        string filter = "fartherID = '" + fartherID + "'";
                        dv .RowFilter = filter;
                        foreach (DataRowView drv in dv)
                        {
                              TreeNode tempNode = new TreeNode();
                              tempNode .Tag = drv[id] .ToString();
                              if (display2 == "" || drv[display2] .ToString() == "")
                              {
                                    tempNode .Text = drv[display] .ToString();
                              }
                              else
                              {
                                    tempNode .Text = drv[display] .ToString() + "(" + drv[display2] .ToString() + ")";
                              }
                              if (tn == null)
                              {
                                    tv .Nodes .Add(tempNode);
                              }
                              else
                              {
                                    tn .Nodes .Add(tempNode);
                              }
                              CreateTreeView(dt, tv, tempNode, id, display, display2, tempNode .Tag .ToString());
                        }
                        dt .Dispose();
                  }
            }


            /// <summary>
            /// dgv转换成table 4-2 修改 
            /// </summary>
            /// <param name="dgv"></param>
            /// <returns></returns>
            public static DataTable DgvToTable(DataGridView dgv)
            {
                  DataTable dt = new DataTable();
                  for (int c = 0; c < dgv .Columns .Count; c++)
                  {
                        DataColumn dc = new DataColumn(dgv .Columns[c] .Name);
                        dt .Columns .Add(dc);
                  }
                  for (int r = 0; r < dgv .Rows .Count; r++)
                  {
                        if (!dgv .Rows[r] .IsNewRow)
                        {
                              DataRow dr = dt .NewRow();
                              for (int i = 0; i < dgv .Columns .Count; i++)
                              {
                                    if (dgv .Rows[r] .Cells[i] .Value != null)
                                    {
                                          dr[i] = dgv .Rows[r] .Cells[i] .Value .ToString();
                                    }
                              }
                              dt .Rows .Add(dr);
                        }
                  }
                  return dt;
            }

            /// <summary>
            /// 展开指定的节点2012- 8-1
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="tagTxt"></param>
            public static void ExpandTreeNode(TreeView tv, string tagTxt)
            {
                  foreach (TreeNode tn in tv .Nodes)
                  {
                        TreeNode thisNode = FindNode(tn, tagTxt);
                        if (thisNode != null)
                        {
                              tv .SelectedNode = thisNode;
                              thisNode .Expand();
                              TreeNode parNode = new TreeNode();
                              int count = thisNode .Level;
                              for (int i = 0; i < count + 1; i++)
                              {
                                    parNode = thisNode .Parent;
                                    thisNode = thisNode .Parent;
                                    if (parNode != null)
                                    {
                                          parNode .Expand();
                                    }
                              }
                        }
                  }
            }
            /// <summary>
            /// 查找一个树节点，通过Tag属性 2012-8-1
            /// </summary>
            /// <param name="tnParent"></param>
            /// <param name="tagTxt"></param>
            /// <returns></returns>
            public static TreeNode FindNode(TreeNode tnParent, string tagTxt)
            {
                  TreeNode node2 = new TreeNode();
                  if (tnParent == null)
                        return null;
                  if (tnParent .Tag .ToString() == tagTxt)
                        return tnParent;
                  if (tnParent .Level > 0)
                        tnParent .Collapse();
                  TreeNode tnRet = null;
                  foreach (TreeNode tn in tnParent .Nodes)
                  {
                        tnRet = FindNode(tn, tagTxt);
                        if (tnRet != null)
                              break;
                  }
                  return tnRet;
            }
            /// <summary>
            /// 将name 与 num拆开，适用形式 name(num) 如 角钢(010001) 2012-7-31 zhang
            /// </summary>
            /// <param name="str">返回数组 [0]:name，[1]:num</param>
            /// <returns></returns>
            public static String[] GetNameAndNum(string str)
            {

                  string[] array = new string[2];
                  string name = "";
                  string num = "";
                  if (str != "" && str != "无")
                  {
                        for (int i = (str .Length - 2); i >= 0; i--)
                        {
                              if (str[i] .ToString() == "(")
                              {
                                    break;
                              }
                              num = str[i] + num;
                        }
                        name = str .Remove(str .Length - num .Length - 2);
                        array[0] = name;
                        array[1] = num;
                  }
                  else
                  {
                        array[0] = "无";
                        array[1] = "";
                  }
                  return array;
            }
            /// <summary>
            /// 按比重计算重量 2013-1-22 修改（采购仓库单位不同）
            /// 2013-4-23 修改，增加到货单的单位单独配置
            /// 2013-6-21 修改，区别螺栓重量计算方法
            /// 2014-6-17 增加法兰重量计算
            /// </summary>
            /// /// <param name="module">模块代号</param>
            /// <param name="count">数量</param>
            /// <param name="length">长度</param>
            /// <param name="width">宽度</param>
            /// <param name="density">比重</param>
            /// <returns></returns>
            public static decimal ComputeWeightByDensity(string mfID, string tag, string module, decimal count, decimal length, decimal width, decimal density)
            {
                  string sql = " SELECT * FROM B_MaterialFile WHERE mfID = " + mfID;
                  DataTable dt = YKDataClass .getDataTable(sql);
                  string weightWay = "算法1(比重，尺寸，数量)";
                  if (mfID != "0" || dt .Rows .Count > 0)
                  {
                        weightWay = dt .Rows[0]["mfWeightWay"] .ToString();
                  }
                  string lengthUnit = "";
                  string weightUnit = "";
                  if (module == "C")
                  {
                        lengthUnit = UserInfo .configSys["C_LengthUnit"] .ToString();
                        weightUnit = UserInfo .configSys["C_WeightUnit"] .ToString();
                  }
                  else if (module == "W")
                  {
                        lengthUnit = UserInfo .configSys["W_LengthUnit"] .ToString();
                        weightUnit = UserInfo .configSys["W_WeightUnit"] .ToString();
                  }
                  else if (module == "GA")
                  {
                        lengthUnit = UserInfo .configSys["W_GoodsAcceptLengthUnit"] .ToString();
                        weightUnit = UserInfo .configSys["W_GoodsAcceptWeightUnit"] .ToString();
                  }

                  //测试
                  //string lengthUnit = "mm";
                  //string weightUnit = "kg";
                  decimal weight = 0M;
                  decimal unit = 0M;
                  if (tag == "原材料" && weightWay != "算法2(比重，数量)")
                  {//原材料按比重计算
                        if (lengthUnit .ToLower() == "mm")
                        {
                              length /= 1000;
                              width /= 1000;
                        }
                        if (width == 0)
                        {
                              unit = length;
                        }
                        else if (length == 0)
                        {
                              unit = width;
                        }
                        else
                        {
                              unit = length * width;
                        }
                        weight = count * unit * density;
                  }
                  else if (tag == "螺栓" || weightWay == "算法2(比重，数量)")
                  {//螺栓数量*单重
                        weight = count * density;
                  }
                  else
                  {//辅材不计算重量
                        weight = 0;
                  }

                  if (weightUnit .ToLower() == "t")
                  {
                        weight /= 1000;
                  }
                  return weight;
            }
            /// <summary>
            /// 按比重计算重量 2013-1-22 修改（采购仓库单位不同）
            /// 2013-4-23 修改，增加到货单的单位单独配置
            /// 2013-6-21 修改，区别螺栓重量计算方法
            /// 2014-6-17 增加法兰重量计算
            /// </summary>
            /// /// <param name="module">模块代号</param>
            /// <param name="count">数量</param>
            /// <param name="length">长度</param>
            /// <param name="width">宽度</param>
            /// <param name="density">比重</param>
            /// <returns></returns>
            public static decimal ComputeWeightByDensityNew(string way, string mfID, string tag, string module, decimal count, decimal length, decimal width, decimal density)
            {
                  string sql = " SELECT * FROM B_MaterialFile WHERE mfID = " + mfID;
                  DataTable dt = YKDataClass .getDataTable(sql);
                  string weightWay = "算法1(比重，尺寸，数量)";

                  decimal mfdensity = 0M;
                  if (mfID != "0" || dt .Rows .Count > 0)
                  {
                        weightWay = dt .Rows[0]["mfWeightWay"] .ToString();
                        mfdensity = Convert .ToDecimal(dt .Rows[0]["mfDensity"] .ToString());
                  }
                  string lengthUnit = "";
                  string weightUnit = "";
                  if (module == "C")
                  {
                        lengthUnit = UserInfo .configSys["C_LengthUnit"] .ToString();
                        weightUnit = UserInfo .configSys["C_WeightUnit"] .ToString();
                  }
                  else if (module == "W")
                  {
                        lengthUnit = UserInfo .configSys["W_LengthUnit"] .ToString();
                        weightUnit = UserInfo .configSys["W_WeightUnit"] .ToString();
                  }
                  else if (module == "GA")
                  {
                        lengthUnit = UserInfo .configSys["W_GoodsAcceptLengthUnit"] .ToString();
                        weightUnit = UserInfo .configSys["W_GoodsAcceptWeightUnit"] .ToString();
                  }

                  //测试
                  //string lengthUnit = "mm";
                  //string weightUnit = "kg";
                  decimal weight = 0M;
                  decimal unit = 0M;
                  if (tag == "原材料" && way == "过磅")
                  {
                        weight = count * density;
                  }
                  else if (tag == "原材料" && weightWay != "算法2(比重，数量)")
                  {//原材料按比重计算
                        if (lengthUnit .ToLower() == "mm")
                        {
                              length /= 1000;
                              width /= 1000;
                        }
                        if (width == 0)
                        {
                              unit = length;
                        }
                        else if (length == 0)
                        {
                              unit = width;
                        }
                        else
                        {
                              unit = length * width;
                        }
                        weight = count * unit * mfdensity;
                        if (weightUnit .ToLower() == "t")
                        {
                              weight /= 1000;
                        }
                  }
                  else if (tag == "螺栓" || weightWay == "算法2(比重，数量)")
                  {//螺栓数量*单重
                        weight = count * mfdensity;
                        if (weightUnit .ToLower() == "t")
                        {
                              weight /= 1000;
                        }
                  }
                  else
                  {//辅材不计算重量
                        weight = 0;
                  }

                  return weight;
            }

            /// <summary>
            /// 判断窗体是否已经打开 2012-8-20 zhang
            /// </summary>
            /// <param name="frmName"></param>
            /// <returns></returns>
            public static bool AlreadyOpen(string text)
            {
                  bool open = false;
                  for (int i = 0; i < Application .OpenForms .Count; i++)
                  {
                        if (Application .OpenForms[i] .Text .Equals(text))
                        {
                              Application .OpenForms[i] .Activate();
                              open = true;
                        }
                  }
                  return open;
            }

            /// <summary>
            /// 处理语句中的保留字符 2012-8-20 zhang
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static string ReplaceSqlStr(string sql)
            {
                  sql = sql .Replace("[", "[[]");
                  sql = sql .Replace("*", "[*]");
                  return sql;
            }

            /// <summary>
            /// 绑定下拉框 1，（全部）2、（无）3、不增加新项目 4、空
            /// </summary>
            /// <param name="cmb"></param>
            /// <param name="dt"></param>
            /// <param name="display"></param>
            /// <param name="value"></param>
            /// <param name="initial"></param>
            /// <param name="flag">1，（全部）2、（无）3、不增加新项目</param>
            public static void BindComboBox(ComboBox cmb, DataTable dt, string display, string value, string initial, int flag)
            {
                  if (dt != null && dt .Rows .Count > 0)
                  {
                        DataRow dr = dt .NewRow();
                        if (flag == 1)
                        {
                              dr[value] = "0";
                              dr[display] = "[全部]";
                              dt .Rows .InsertAt(dr, 0);
                        }
                        else if (flag == 2)
                        {
                              dr[value] = "0";
                              dr[display] = "[无]";
                              dt .Rows .InsertAt(dr, 0);
                        }
                        else if (flag == 4)
                        {
                              dr[value] = "0";
                              dr[display] = "";
                              dt .Rows .InsertAt(dr, 0);
                        }
                        cmb .DisplayMember = display;
                        cmb .ValueMember = value;
                        cmb .DataSource = dt;
                        if (initial != "")
                        {
                              cmb .Text = initial;
                        }
                        //else
                        //{
                        //      cmb .SelectedIndex = 0;
                        //}
                  }
            }
      }
}
