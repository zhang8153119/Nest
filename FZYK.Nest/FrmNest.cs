using System;
using System .Collections .Generic;
using System .ComponentModel;
using System .Data;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Windows .Forms;
using FZYK .Core;
using myCad;
using myCad .Utility;
using myCad .DXFOper;
using myCad .Model;
using FZYK .Nest .db;

namespace FZYK .Nest
{
      public partial class FrmNest : BaseSingle
      {
            public FrmNest()
            {
                  InitializeComponent();
            }
            #region 全局变量
            float T = 10f;//栅格精度
            float _size = 0;//小件长宽
            float _protect = 0;//保护量
            bool _rotate = true;//非组合零件交替旋转排入
            string _type = "";//组合模型
            int _pop = 0;
            decimal _cross = 0;
            decimal _mutation = 0;

            List<PlateModel> _part = new List<PlateModel>();
            List<PlateCombine> _partCombine = new List<PlateCombine>();

            PlateHelper ph = new PlateHelper();
            GridHelper gh = new GridHelper();
            RectHelper rh = new RectHelper();
            NestDB _db = new NestDB();
            #endregion
            #region 加载
            private void FrmNest_Shown(object sender, EventArgs e)
            {
                  BindSet();
                  cadInterfaceMain .Init();
            }
            /// <summary>
            /// 绑定设置
            /// </summary>
            private void BindSet()
            {
                  Dictionary<string, string> dic = new Dictionary<string, string>();
                  dic = _db .GetSet();
                  T = Convert .ToSingle(dic["T"]);
                  _size = Convert .ToSingle(dic["Size"]);
                  _protect = Convert .ToSingle(dic["Protect"]);
                  _rotate = dic["Rotate"] .ToString() .Equals("是");
                  _pop = Convert .ToInt32(dic["Pop"]);
                  _cross = Convert .ToDecimal(dic["Cross"]);
                  _mutation = Convert .ToDecimal(dic["Mutation"]);

                  string type = dic["Type"] .ToString();
                  if (type == "矩形")
                        _type = "rect";
                  else if (type == "平行四边形")
                        _type = "para";
                  else if (type == "混合")
                        _type = "mix";
                  else
                        _type = "";
            }
            #endregion
            #region 读取零件数据
            private void tsmiReadDxf_Click(object sender, EventArgs e)
            {
                  DxfInputB di = new DxfInputB();
                  List<PlateModel> plate = new List<PlateModel>();
                  plate = di .mainmethod();

                  DataTable dt = new DataTable();
                  dt = DevCommon .DgvToTableEmpty(dgvPart);

                  for (int i = 0; i < plate .Count; i++)
                  {
                        PlateModel pm = ph .GetMinPlate(rh .Expand(plate[i], _protect));
                        pm .GridValue = gh .GetGridValue(pm, T) .Grid;
                        pm .id = i;
                        _part .Add(pm);

                        if (plate[i] .PlateCount > 1)
                        {
                              PlateCombine pc = ph .GetMinPlateCombine(pm, T, _type);
                              pc .GridValue = gh .GetGridValueCombine(pc, T) .Grid;
                              pc .id = i;
                              _partCombine .Add(pc);
                        }

                        DataRow dr = dt .NewRow();
                        dr["id"] = pm .id;
                        dr["pName"] = pm .PlateName;
                        dr["rCount"] = pm .PlateCount;
                        dr["pCount"] = pm .PlateCount;
                        dr["pLength"] = Convert .ToInt32(Math .Max(pm .Rect .Width, pm .Rect .Height));
                        dr["pWidth"] = Convert .ToInt32(Math .Min(pm .Rect .Width, pm .Rect .Height));
                        dt .Rows .Add(dr);
                  }
                  
                  dgvPart .DataSource = dt;
                  
            }

            /// <summary>
            /// 设置
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsbtnSet_Click(object sender, EventArgs e)
            {
                  FrmNestSet frm = new Nest .FrmNestSet();
                  if (frm .ShowDialog() == DialogResult .OK)
                  {
                        BindSet();
                  }
            }
            #endregion

      }
}
