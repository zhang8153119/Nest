using myCad .CADInterfaceCtrl;
using myCad .DXFOper;
using myCad .Model;
using myCad .PaiYangSuanFa;
using myCad .Shape;
using myCad .ShapeOper;
using myCad .Utility;
using myCad;
using System;
using System .Collections .Generic;
using System .ComponentModel;
using System .Data;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .Linq;
using System .Text;
using System .Threading;
using System .Threading .Tasks;
using System .Windows .Forms;
using System .Windows .Forms .DataVisualization .Charting;

namespace FZYK .Nest
{
      public partial class FrmCut : Form
      {
            public FrmCut()
            {
                  InitializeComponent();
                  _pnllen = cad .Width;
                  _pnlwid = cad .Height;
            }

            public BufferedGraphics _bg;
            public BufferedGraphics _bgcad;
            public BufferedGraphics _bgtest;
            const float SIDE = 20f;
            const float T = 10f;//栅格精度
            float _small = 600 / T;//长或宽大于此数字时，为大件
            bool _rotate = true;//单件排入后，下一个单件是否旋转180度
            float _scale = 1f;
            float _pnllen = 0;
            float _pnlwid = 0;
            float _rate = 1f;//缩放比例
            List<PlateModel> _plate = new List<PlateModel>();
            string _type = "";

            int[,] _gridarray;
            List<GridData> _grid = new List<GridData>();
            List<PlateModel> _part = new List<PlateModel>();
            List<PlateCombine> _partCombine = new List<PlateCombine>();
            List<Stock> _stock = new List<Stock>();
            DataTable _dtpart = new DataTable();

            private void FrmCut_Shown(object sender, EventArgs e)
            {
                  tabMain .SelectedIndex = 1;
                  InitChart();
                  txtscale .Text = "20";
                  Graphics g = pnlgrid .CreateGraphics();
                  _bg = BufferedGraphicsManager .Current .Allocate(g, ClientRectangle);
                  _bg .Graphics .Clear(Color .White);

                  Graphics gtest = pnlcad .CreateGraphics();
                  _bgtest = BufferedGraphicsManager .Current .Allocate(gtest, ClientRectangle);
                  _bgtest .Graphics .Clear(Color .White);

                  Graphics gcad = cad .CreateGraphics();
                  _bgcad = BufferedGraphicsManager .Current .Allocate(gcad, ClientRectangle);

                  _dtpart .Columns .Add("part");
                  _dtpart .Columns .Add("area");
                  _dtpart .Columns .Add("arearate");
                  _dtpart .Columns .Add("lenrate");
                  _dtpart .Columns .Add("len");
                  _dtpart .Columns .Add("wid");
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                  base .OnPaint(e);
                  //DrawStock();
                  DrawPart();
            }

            private void pnldraw_SizeChanged(object sender, EventArgs e)
            {
                  _pnllen = pnlgrid .Width;
                  _pnlwid = pnlgrid .Height;
            }
            /// <summary>
            /// 提取库存
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnstock_Click(object sender, EventArgs e)
            {
                  _stock .Clear();
                  if (dgvStock .Rows .Count == 1)
                  {
                        MessageBox .Show("请输入库存");
                  }
                  for (int i = 0; i < dgvStock .Rows .Count; i++)
                  {
                        if (!dgvStock .Rows[i] .IsNewRow)
                        {
                              float slength = Convert .ToSingle(dgvStock .Rows[i] .Cells["sLength"] .Value .ToString());
                              float swidth = Convert .ToSingle(dgvStock .Rows[i] .Cells["sWidth"] .Value .ToString());
                              Stock stock = new Stock();
                              stock .Width = slength;
                              stock .Height = swidth;
                              stock .id = i;

                              int rowcount = Convert .ToInt32(Math .Floor(stock .Width / T));
                              int colcount = Convert .ToInt32(Math .Floor(stock .Height / T));
                              int[,] grid = new int[rowcount, colcount];
                              for (int k = 0; k < rowcount; k++)
                              {
                                    for (int j = 0; j < colcount; j++)
                                    {
                                          grid[k, j] = 0;
                                    }
                              }
                              stock .Disable = grid;
                              _stock .Add(stock);
                        }
                  }
            }
            /*
            /// <summary>
            /// 画板
            /// </summary>
            private void DrawPlate()
            {
                  dgvStock .EndEdit();
                  _scale = Convert .ToSingle(txtscale .Text);
                  if (dgvStock .Rows .Count == 1)
                  {
                        return;
                  }
                  float slength = Convert .ToSingle(dgvStock .Rows[0] .Cells["sLength"] .Value .ToString());
                  float swidth = Convert .ToSingle(dgvStock .Rows[0] .Cells["sWidth"] .Value .ToString());

                  float len = _pnllen - 2 * SIDE;
                  _rate = len / slength;
                  float wid = swidth * _rate;
                  float thisscale = _scale * _rate;

                  GraphicsPath gp = new GraphicsPath();
                  gp .AddRectangle(new RectangleF(SIDE, (_pnlwid - wid) / 2, len, wid));
                  _bg .Graphics .Clear(Color .White);
                  using (Pen pen = new Pen(Color .Red))
                  {
                        _bg .Graphics .DrawPath(pen, gp);
                        //g .DrawRectangle(pen, SIDE, (pnlwid - wid) / 2, len, wid);
                  }
                  using (Pen pen = new Pen(Color .Red))
                  {
                        pen .DashStyle = DashStyle .Dot;

                        for (float i = thisscale; i < len; i += thisscale)
                        {
                              _bg .Graphics .DrawLine(pen
                                    , i + SIDE
                                    , (_pnlwid - wid) / 2f
                                    , i + SIDE
                                    , (_pnlwid - wid) / 2f + wid);
                        }
                        for (float j = thisscale; j < wid; j += thisscale)
                        {
                              _bg .Graphics .DrawLine(pen
                                    , SIDE
                                    , (_pnlwid - wid) / 2f + j
                                    , SIDE + len
                                    , (_pnlwid - wid) / 2f + j);
                        }
                  }
                  using (SolidBrush b = new SolidBrush(Color .Green))
                  {
                        for (int i = 0; i < _grid .Count; i++)
                        {
                              _bg .Graphics .FillRectangle(b
                                    , new RectangleF(_grid[i] .Col * thisscale
                                    , _grid[i] .Row * thisscale
                                    , thisscale
                                    , thisscale));
                        }
                  }
                  _bg .Render();
            }*/
            //获取原材料
            private void GetStock()
            {
                  Line line1 = new Line(new PointF(0, 0), new PointF(float .Parse(this .txtwidth .Text .Trim()), 0));
                  Line line2 = new Line(
                      new PointF(float .Parse(this .txtwidth .Text .Trim()), 0),
                      new PointF(float .Parse(this .txtwidth .Text .Trim()), float .Parse(this .txtheight .Text .Trim())));
                  Line line3 = new Line(
                      new PointF(float .Parse(this .txtwidth .Text .Trim()), float .Parse(this .txtheight .Text .Trim())),
                      new PointF(0, float .Parse(this .txtheight .Text .Trim())));
                  Line line4 = new Line(
                      new PointF(0, float .Parse(this .txtheight .Text .Trim())),
                      new PointF(0, 0));

                  Stock stock = new Stock();
                  stock .StockForm .Add(line1);
                  stock .StockForm .Add(line2);
                  stock .StockForm .Add(line3);
                  stock .StockForm .Add(line4);

                  stock .MinPoint = new PointF(0, 0);
                  stock .MaxPoint = new PointF(float .Parse(this .txtwidth .Text .Trim()), float .Parse(this .txtheight .Text .Trim()));

                  //stock .Height = float .Parse(this .height .Text .Trim());
                  //stock .Width = float .Parse(this .width .Text .Trim());
                  //stock .Num = int .Parse(this .number .Text .Trim());
                  //stock .StockId = drawBoard .listStock .Count;
                  //drawBoard .listStock .Add(stock);
            }
            /// <summary>
            /// 画原材料
            /// </summary>
            /// <param name="stock"></param>
            private void DrawStock(Stock stock)
            {
                  //_bg .Graphics .Clear(Color .White);
                  float width = stock .Width;
                  float height = stock .Height;
                  _rate = pnlgrid .Width / width;
                  using (Pen p = new Pen(Color .Red))
                  {
                        _bg .Graphics .DrawRectangle(p, new Rectangle(0, 0, pnlgrid .Width, Convert .ToInt32(Math .Ceiling(_rate * height))));
                  }
                  _bg .Render();
            }
            /// <summary>
            /// 排入
            /// </summary>
            private void CutAdd()
            {
                  float sumarea = 0;
                  double drawtime = 0;
                  System .Diagnostics .Stopwatch watch = new System .Diagnostics .Stopwatch();
                  watch .Start();  //开始监视代码运行时间
                  Stock stock = new Stock();
                  stock .Width = Convert .ToSingle(txtwidth .Text);
                  stock .Height = Convert .ToSingle(txtheight .Text);
                  DrawStock(stock);
                  int rowcount = Convert .ToInt32(Math .Floor(stock .Width / T));
                  int colcount = Convert .ToInt32(Math .Floor(stock .Height / T));
                  int[,] grid = new int[rowcount, colcount];
                  for (int i = 0; i < rowcount; i++)
                  {
                        for (int j = 0; j < colcount; j++)
                        {
                              grid[i, j] = 0;
                        }
                  }
                  int test_step = 0;
                  stock .Disable = grid;
                  for (int i = 0; i < _part .Count; i++)
                  {
                        PlateModel pm = _part[i];
                        for (int m = 0; m < rowcount; m++)
                        {
                              bool ok = false;
                              int n = 0;
                              while (n < colcount)
                              {
                                    if (stock .Disable[m, n] > 0)
                                    {
                                          if (stock .Disable[m, n] > 1)
                                                test_step += (stock .Disable[m, n] - 1);
                                          n += stock .Disable[m, n];
                                          continue;
                                    }

                                    if (CanAdd(stock, pm, m, n))
                                    {
                                          sumarea += pm .Area;
                                          stock .Disable = GridAdd(stock .Disable, pm, m, n);

                                          System .Diagnostics .Stopwatch watch2 = new System .Diagnostics .Stopwatch();
                                          watch2 .Start();  //开始监视代码运行时间
                                          DrawPart2(pm .GridValue, m, n);
                                          watch2 .Stop();  //停止监视
                                          TimeSpan timespan2 = watch2 .Elapsed;  //获取当前实例测量得出的总时间
                                          drawtime += timespan2 .TotalMilliseconds;

                                          ok = true;
                                          break;
                                    }
                                    n++;
                              }
                              if (ok)
                                    break;
                        }
                  }

                  int index = 0;
                  for (int i = 0; i < stock .Disable .GetLength(0); i++)
                  {
                        bool find = false;
                        for (int j = 0; j < stock .Disable .GetLength(1); j++)
                        {
                              if (stock .Disable[i, j] != 0)
                              {
                                    find = true;
                                    break;
                              }
                        }
                        if (!find)
                        {
                              index = i + 1;
                              break;
                        }
                  }
                  watch .Stop();  //停止监视
                  TimeSpan timespan = watch .Elapsed;  //获取当前实例测量得出的总时间
                  MessageBox .Show("耗时：" + timespan .TotalMilliseconds .ToString() + "ms" + "画图：" + drawtime .ToString() + "ms 面积：" + sumarea .ToString() + " 右：" + (index * T) .ToString() + "mm" + " 跳过" + test_step .ToString() + "次");  //总毫秒数
            }
            /// <summary>
            /// 清除
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btndel_Click(object sender, EventArgs e)
            {
                  _pop .Clear();
                  _basicLib .Clear();
                  _part .Clear();
                  _partCombine .Clear();
                  _dtpart .Clear();
                  _bg .Graphics .Clear(Color .White);
                  _bg .Render();

                  _bgtest .Graphics .Clear(Color .White);
                  _bgtest .Render();

                  cad .currentShapes .Clear();
                  cad .currentPlates .Clear();
                  cad .DrawShap();
                  _thisindex = 0;
            }
            /// <summary>
            /// 判断是否可以排入
            /// </summary>
            /// <param name="s"></param>
            /// <param name="pm"></param>
            /// <param name="rowindex"></param>
            /// <param name="colindex"></param>
            /// <returns></returns>
            private bool CanAdd(Stock s, PlateModel pm, int rowindex, int colindex)
            {
                  List<GridData> grid = pm .GridValue;
                  int row0 = grid[0] .Row;//先判断特殊点，通过再逐点判断
                  int col0 = grid[0] .Col;
                  int rown = grid[grid .Count - 1] .Row;
                  int coln = grid[grid .Count - 1] .Col;
                  if ((row0 + rowindex + 1) * T > s .Width || (col0 + colindex + 1) * T > s .Height || s .Disable[row0 + rowindex, col0 + colindex] > 0
                        || (rown + rowindex + 1) * T > s .Width || (coln + colindex + 1) * T > s .Height || s .Disable[rown + rowindex, coln + colindex] > 0)
                  {
                        return false;
                  }
                  for (int i = 0; i < grid .Count; i++)
                  {
                        int row = grid[i] .Row;
                        int col = grid[i] .Col;
                        if ((row + rowindex + 1) * T > s .Width || (col + colindex + 1) * T > s .Height)
                        {
                              return false;
                        }
                        if (s .Disable[row + rowindex, col + colindex] > 0)
                        {
                              return false;
                        }
                  }
                  return true;
            }
            /// <summary>
            /// 栅格排入
            /// </summary>
            /// <param name="gridarray"></param>
            /// <param name="pm"></param>
            /// <param name="rowindex"></param>
            /// <param name="colindex"></param>
            /// <returns></returns>
            private int[,] GridAdd(int[,] gridarray, PlateModel pm, int rowindex, int colindex)
            {
                  List<GridData> grid = pm .GridValue;
                  for (int i = 0; i < grid .Count; i++)
                  {
                        int row = grid[i] .Row;
                        int col = grid[i] .Col;
                        gridarray[row + rowindex, col + colindex] = grid[i] .Value;
                  }
                  return gridarray;
            }
            /// <summary>
            /// 画栅格图沿y方向画图
            /// </summary>
            private void DrawPart2(List<GridData> grid, int m, int n)
            {
                  if (grid != null)
                  {
                        //_bg .Graphics .Clear(Color .White);
                        using (SolidBrush b = new SolidBrush(Color .Green))
                        {
                              for (int i = 0; i < grid .Count; i++)
                              {
                                    _bg .Graphics .FillRectangle(b
                                          , new RectangleF((grid[i] .Row + m) * T * _rate
                                          , (grid[i] .Col + n) * T * _rate
                                          , T * _rate
                                          , T * _rate));
                              }
                        }
                  }
                  _bg .Render();
            }
            /// <summary>
            /// 画栅格图沿y方向画图
            /// </summary>
            private void DrawPart()
            {
                  if (_partCombine .Count == 0)
                        return;
                  _rate = 1;
                  List<GridData> gd = new List<GridData>();
                  gd = _partCombine[0] .GridValue;
                  if (gd != null)
                  {
                        //_bg .Graphics .Clear(Color .White);
                        using (SolidBrush b = new SolidBrush(Color .Green))
                        {
                              for (int i = 0; i < gd .Count; i++)
                              {
                                    _bgtest .Graphics .FillRectangle(b
                                          , new RectangleF(gd[i] .Row * T * _rate
                                          , gd[i] .Col * T * _rate
                                          , T * _rate
                                          , T * _rate));
                              }
                        }
                  }
                  _bgtest .Render();
            }
            private void pnlcad_Paint(object sender, PaintEventArgs e)
            {
                  DrawPart();
            }
            /// <summary>
            /// 提取DXF
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnDXF_Click(object sender, EventArgs e)
            {
                  float dis = Convert .ToSingle(txtDis .Text);
                  _bgcad .Graphics .Clear(Color .Black);
                  //DrawStock();
                  DxfInputB di = new DxfInputB();
                  _plate = di .mainmethod();
                  for (int i = 0; i < _plate .Count; i++)
                  {
                        txtpart .Text = _plate[0] .PlateName;
                        PlateHelper ph = new PlateHelper();
                        GridHelper gh = new GridHelper();
                        RectHelper rh = new RectHelper();

                        PlateModel pm = ph .GetMinPlate(rh .Expand(_plate[i], dis));
                        pm .GridValue = gh .GetGridValue(pm, T) .Grid;
                        pm .id = i;
                        _part .Add(pm);

                        cad .currentShapes .Clear();
                        cad .currentPlates .Clear();

                        if (cmbType .Text == "")
                        {
                              MessageBox .Show("请选择组合模型");
                              return;
                        }
                        else if (cmbType .Text == "矩形")
                              _type = "rect";
                        else if (cmbType .Text == "平行四边形")
                              _type = "para";
                        else if (cmbType .Text == "混合")
                              _type = "mix";
                        else
                              _type = "";
                        if (_plate[i] .PlateCount > 1)
                        {
                              PlateCombine pc = ph .GetMinPlateCombine(pm, T, _type);
                              //PlateCombine pc = ph .GetMinPlateCombine(inputPlate[i], T);
                              pc .GridValue = gh .GetGridValueCombine(pc, T) .Grid;
                              pc .id = i;
                              _partCombine .Add(pc);
                        }

                        //for (int c = 0; c < inputPlate[i] .PlateCount; c++)
                        //{
                        //      /*Random rd = new Random();
                        //      float angle = Convert .ToSingle(rd .Next(0, 359));
                        //      PlateModel pmnew = Rotate(inputPlate[i], angle);
                        //      pmnew .GridValue = GetGridValue(pmnew);
                        //      _part .Add(pmnew);*/

                        //      PlateModel pmnew = inputPlate[i] .Copy();
                        //      Dictionary<string, object> dicgrid = new Dictionary<string, object>();
                        //      dicgrid = ph .GetGridValueReturnDic(pmnew, T);
                        //      pmnew .GridValue = (List<GridData>)dicgrid["grid"];
                        //      pmnew .GridLen = Convert .ToSingle(dicgrid["len"]);
                        //      pmnew .GridWid = Convert .ToSingle(dicgrid["wid"]);
                        //      pmnew .id = id;
                        //      _part .Add(pmnew);

                        //      DataRow dr = _dtpart .NewRow();
                        //      dr["part"] = pmnew .PlateCode;
                        //      dr["area"] = pmnew .Area;
                        //      dr["len"] = pmnew .GridLen;
                        //      dr["wid"] = pmnew .GridWid;
                        //      dr["lenrate"] = pmnew .GridLen > pmnew .GridWid ? pmnew .GridLen / pmnew .GridWid : pmnew .GridWid / pmnew .GridLen;
                        //      dr["arearate"] = pmnew .Area / (pmnew .GridLen * pmnew .GridWid * T * T);
                        //      _dtpart .Rows .Add(dr);
                        //      id++;
                        //}
                  }

                  //dgvshape .DataSource = _dtpart;
                  //CutAdd();
                  DrawPart();
            }

            /// <summary>
            /// 旋转
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnxuanzhuan_Click(object sender, EventArgs e)
            {
                  PointF p = new PointF(0, 0);// _part[0] .RotateCenter;
                  PlateModel pm = new RotateOper() .RotatePlate(_part[0], p, Convert .ToSingle(txtjiaodu .Text));
                  PlateHelper ph = new PlateHelper();
                  GridHelper gh = new GridHelper();
                  _grid = gh .GetGridValue(pm, T) .Grid;
                  _bgtest .Graphics .Clear(Color .White);
                  DrawPart();
                  cad .DrawShap();
            }
            /// <summary>
            /// 最小矩形
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnrect_Click(object sender, EventArgs e)
            {
                  System .Diagnostics .Stopwatch watch = new System .Diagnostics .Stopwatch();
                  watch .Start();  //开始监视代码运行时间
                  List<BaseShape> bm = _part[0] .OutModel .ListShape;
                  PlateModel best = new PlateModel();
                  float maxarea = -1;
                  float jd = 0;
                  for (int i = 0; i < bm .Count; i++)
                  {
                        if (bm[i] .ShapeClass == "Line")
                        {
                              float jiaodu = 0;
                              if (bm[i] .EndPoint .X == bm[i] .StartPoint .X)
                              {
                                    jiaodu = 90;
                              }
                              else
                              {
                                    jiaodu = Convert .ToSingle(Math .Atan((bm[i] .EndPoint .Y - bm[i] .StartPoint .Y)
                                          / (bm[i] .EndPoint .X - bm[i] .StartPoint .X)) * 180 / Math .PI);
                              }

                              PointF p = new PointF(0, 0);// _part[0] .RotateCenter;
                              PlateModel pm = new RotateOper() .RotatePlate(_part[0], p, jiaodu);
                              float maxX = pm .OutModel .ListPoint[0] .X;
                              float maxY = pm .OutModel .ListPoint[0] .Y;
                              float minX = pm .OutModel .ListPoint[0] .X;
                              float minY = pm .OutModel .ListPoint[0] .Y;
                              for (int j = 0; j < pm .OutModel .ListPoint .Count; j++)
                              {
                                    if (maxX < pm .OutModel .ListPoint[j] .X)
                                          maxX = pm .OutModel .ListPoint[j] .X;
                                    if (maxY < pm .OutModel .ListPoint[j] .Y)
                                          maxY = pm .OutModel .ListPoint[j] .Y;

                                    if (minX > pm .OutModel .ListPoint[j] .X)
                                          minX = pm .OutModel .ListPoint[j] .X;
                                    if (minY > pm .OutModel .ListPoint[j] .Y)
                                          minY = pm .OutModel .ListPoint[j] .Y;
                              }

                              if ((maxY - minY) * (maxX - minX) < maxarea || maxarea < 0)
                              {
                                    maxarea = (maxY - minY) * (maxX - minX);
                                    jd = jiaodu;
                                    best = pm .Copy();
                              }
                              cad .DrawShap();
                              //Thread .Sleep(1000);
                        }
                  }
                  new CopyOper() .CopyPlate(best);
                  cad .DrawShap();
                  txtrectarea .Text = maxarea .ToString();
                  watch .Stop();  //停止监视
                  TimeSpan timespan = watch .Elapsed;  //获取当前实例测量得出的总时间
                  MessageBox .Show("面积：" + maxarea .ToString() + "耗时：" + timespan .TotalMilliseconds .ToString() + "ms");
            }
            private void btnrect2_Click(object sender, EventArgs e)
            {
                  System .Diagnostics .Stopwatch watch = new System .Diagnostics .Stopwatch();
                  watch .Start();  //开始监视代码运行时间

                  RectHelper rect = new RectHelper();
                  PlateHelper platehelper = new PlateHelper();
                  Dictionary<string, object> dic = rect .MinRect(_part[0] .OutModel .ListPoint);
                  PlateModel pm = platehelper .RotateAndMove(_part[0], Convert .ToSingle(dic["angle"]));//原图按最小矩形旋转移动到坐标原点
                  List<PointF> p = pm .OutModel .ListPoint;
                  List<PointF> cx = rect .GetConvexHull(pm .OutModel .ListPoint .ToList());

                  CopyOper co = new CopyOper();
                  PlateModel pmnew = co .CopyPlate(pm);//旋转180后的图形
                  platehelper .Rotate(pmnew, 180);
                  List<PointF> pnew = pmnew .OutModel .ListPoint;
                  List<PointF> cxnew = rect .GetConvexHull(pmnew .OutModel .ListPoint .ToList());

                  Dictionary<string, object> dicCombine2 = rect .Combine(p, cx, pnew, cxnew, T, -1, "rect");
                  List<PointF> chcombine = (List<PointF>)dicCombine2["convexhull"];
                  DrawLines(chcombine);
                  DrawLines(pm .OutModel .ListPoint);
                  new MoveOper() .MovePlate(pmnew, Convert .ToSingle(dicCombine2["movex"]), Convert .ToSingle(dicCombine2["movey"]));
                  DrawLines(pmnew .OutModel .ListPoint);
                  platehelper .RotateAndMove(pm, pmnew, Convert .ToSingle(dicCombine2["angle"]));

                  cad .DrawShap();

                  watch .Stop();  //停止监视
                  TimeSpan timespan = watch .Elapsed;  //获取当前实例测量得出的总时间
                  MessageBox .Show("面积：" + dic["area"] .ToString() + "耗时：" + timespan .TotalMilliseconds .ToString() + "ms");
            }

            //画凸包用
            private void DrawLines(List<PointF> chcombine)
            {
                  for (int i = 0; i < chcombine .Count; i++)
                  {
                        int next = i + 1;
                        if (i == chcombine .Count - 1)
                              next = 0;
                        Line nLine = new Line(new PointF(chcombine[i] .X, chcombine[i] .Y), new PointF(chcombine[next] .X, chcombine[next] .Y));
                        nLine .PenColor = Color .Red;
                        nLine .ShapeID = cad .globalID;
                        cad .globalID = cad .globalID + 1;
                        cad .currentShapes .Add(nLine);
                  }
            }
            int _thisindex = 0;
            List<BaseShape> _bm = new List<BaseShape>();
            /// <summary>
            /// 旋转2,旋转到下一条边
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnxuanzhuan2_Click(object sender, EventArgs e)
            {
                  if (_bm .Count == 0)
                  {
                        _bm = _part[0] .OutModel .ListShape;
                  }
                  if (_thisindex >= _bm .Count)
                  {
                        _thisindex = 0;
                  }
                  if (_bm[_thisindex] .ShapeClass == "Line")
                  {
                        float jiaodu = 0;
                        if (_bm[_thisindex] .EndPoint .X == _bm[_thisindex] .StartPoint .X)
                        {
                              jiaodu = 90;
                        }
                        else
                        {
                              jiaodu = Convert .ToSingle(Math .Atan((_bm[_thisindex] .EndPoint .Y - _bm[_thisindex] .StartPoint .Y)
                                    / (_bm[_thisindex] .EndPoint .X - _bm[_thisindex] .StartPoint .X)) * 180 / Math .PI);
                        }

                        PointF p = new PointF(0, 0);// _part[0] .RotateCenter;
                        PlateModel pm = new RotateOper() .RotatePlate(_part[0], p, jiaodu);
                        float maxX = pm .OutModel .ListPoint[0] .X;
                        float maxY = pm .OutModel .ListPoint[0] .Y;
                        float minX = pm .OutModel .ListPoint[0] .X;
                        float minY = pm .OutModel .ListPoint[0] .Y;
                        for (int i = 0; i < pm .OutModel .ListPoint .Count; i++)
                        {
                              if (maxX < pm .OutModel .ListPoint[i] .X)
                                    maxX = pm .OutModel .ListPoint[i] .X;
                              if (maxY < pm .OutModel .ListPoint[i] .Y)
                                    maxY = pm .OutModel .ListPoint[i] .Y;

                              if (minX > pm .OutModel .ListPoint[i] .X)
                                    minX = pm .OutModel .ListPoint[i] .X;
                              if (minY > pm .OutModel .ListPoint[i] .Y)
                                    minY = pm .OutModel .ListPoint[i] .Y;
                        }

                        txtrectarea .Text = ((maxY - minY) * (maxX - minX)) .ToString() + "(" + jiaodu .ToString() + "°)";
                        PlateModel pmnew = new MoveOper() .MovePlate(pm, -minX, -minY);
                        PlateHelper ph = new PlateHelper();
                        GridHelper gh = new GridHelper();
                        _grid = gh .GetGridValue(pmnew, T) .Grid;
                        _bgtest .Graphics .Clear(Color .White);
                        DrawPart();
                        cad .DrawShap();
                  }
                  _thisindex++;
            }



            private void btnprint_Click(object sender, EventArgs e)
            {
                  Basic b = _pop[0] .Basic[0];
                  var tp = GetBasic(b);
                  int[,] array;
                  if (tp .Item2 .GridArray != null)
                  {
                        array = tp .Item2 .GridArray;
                  }
                  else
                  {
                        array = tp .Item1 .GridArray;
                  }
                  FrmText frm = new FrmText(array);
                  frm .Show();
            }
            #region 分析图
            private Queue<double> _dataQueue = new Queue<double>(100);
            private Queue<double> _dataQueueAvg = new Queue<double>(100);
            private int _num = 1;//每次删除增加几个点
            /// <summary>
            /// 初始化图表
            /// </summary>
            private void InitChart()
            {
                  //定义图表区域
                  this .chart1 .ChartAreas .Clear();
                  ChartArea chartArea1 = new ChartArea("C1");
                  this .chart1 .ChartAreas .Add(chartArea1);
                  //定义存储和显示点的容器
                  this .chart1 .Series .Clear();
                  Series series1 = new Series("S1");
                  Series series2 = new Series("S2");
                  series1 .ChartArea = "C1";
                  series2 .ChartArea = "C1";
                  this .chart1 .Series .Add(series1);
                  this .chart1 .Series .Add(series2);
                  //设置图表显示样式
                  //this .chart1 .ChartAreas[0] .AxisY .Minimum = 0;
                  this .chart1 .ChartAreas[0] .AxisY .Maximum = 100;
                  this .chart1 .ChartAreas[0] .AxisX .Interval = 5;
                  this .chart1 .ChartAreas[0] .AxisX .MajorGrid .LineColor = System .Drawing .Color .Silver;
                  this .chart1 .ChartAreas[0] .AxisY .MajorGrid .LineColor = System .Drawing .Color .Silver;
                  this .chart1 .ChartAreas[0] .AxisY .IsStartedFromZero = false;

                  this .chart1 .ChartAreas[0] .AxisX .MajorGrid .Enabled = false;
                  this .chart1 .ChartAreas[0] .AxisX .MajorTickMark .Enabled = false;
                  this .chart1 .ChartAreas[0] .AxisX .IsMarginVisible = false;
                  this .chart1 .ChartAreas[0] .AxisX .LabelStyle .Enabled = false;
                  //设置标题
                  this .chart1 .Titles .Clear();
                  this .chart1 .Titles .Add("S01");
                  //this .chart1 .Titles[0] .Text = "XXX显示";
                  this .chart1 .Titles[0] .ForeColor = Color .RoyalBlue;
                  this .chart1 .Titles[0] .Font = new System .Drawing .Font("Microsoft Sans Serif", 12F);
                  //设置图表显示样式
                  this .chart1 .Series[0] .Color = Color .Red;
                  this .chart1 .Series[1] .Color = Color .Blue;

                  this .chart1 .Titles[0] .Text = "";
                  this .chart1 .Series[0] .ChartType = SeriesChartType .Line;
                  this .chart1 .Series[0] .Points .Clear();
                  this .chart1 .Series[1] .ChartType = SeriesChartType .Line;
                  this .chart1 .Series[1] .Points .Clear();
            }

            /// <summary>
            /// 刷新图
            /// </summary>
            private void RefreshChart()
            {
                  UpdateQueueValue();
                  this .chart1 .Series[0] .Points .Clear();
                  for (int i = 0; i < _dataQueue .Count; i++)
                  {
                        this .chart1 .Series[0] .Points .AddXY((i + 1), _dataQueue .ElementAt(i));
                  }
            }
            //更新队列中的值
            private void UpdateQueueValue()
            {
                  /*
                  if (_dataQueue .Count > 100)
                  {
                        //先出列
                        for (int i = 0; i < _num; i++)
                        {
                              _dataQueue .Dequeue();
                        }
                  }*/
                  Random r = new Random();
                  for (int i = 0; i < _num; i++)
                  {
                        _dataQueue .Enqueue(r .Next(0, 100));
                  }

            }

            private void btntest_Click(object sender, EventArgs e)
            {
                  RefreshChart();
            }
            #endregion
            #region 开始排板
            List<DNA> _pop = new List<DNA>();
            Dictionary<string, GridLib> _basicLib = new Dictionary<string, GridLib>();//基因库
            decimal _pcrossover = 0;
            decimal _pmutation = 0;
            bool _stop = false;
            int _size = 0;
            Stock _best = new Stock();
            System .Diagnostics .Stopwatch _watch = new System .Diagnostics .Stopwatch();
            private void btnGo_Click(object sender, EventArgs e)
            {
                  _watch .Start();
                  _stop = false;
                  _pop .Clear();
                  LoadStock();
                  Thread t = new Thread(DoWork);
                  t .Start();
            }

            private void LoadStock()
            {
                  if (dgvStock .Rows .Count == 1)
                  {
                        MessageBox .Show("请输入库存");
                        return;
                  }
                  _stock .Clear();
                  for (int i = 0; i < dgvStock .Rows .Count; i++)
                  {
                        if (!dgvStock .Rows[i] .IsNewRow)
                        {
                              float slength = Convert .ToSingle(dgvStock .Rows[i] .Cells["sLength"] .Value .ToString());
                              float swidth = Convert .ToSingle(dgvStock .Rows[i] .Cells["sWidth"] .Value .ToString());
                              Stock stock = new Stock();
                              stock .Width = slength;
                              stock .Height = swidth;
                              stock .id = i;

                              int rowcount = Convert .ToInt32(Math .Floor(stock .Width / T));
                              int colcount = Convert .ToInt32(Math .Floor(stock .Height / T));
                              int[,] grid = new int[rowcount, colcount];
                              for (int k = 0; k < rowcount; k++)
                              {
                                    for (int j = 0; j < colcount; j++)
                                    {
                                          grid[k, j] = 0;
                                    }
                              }
                              stock .Disable = grid;
                              _stock .Add(stock);
                        }
                  }
            }
            private void GeneProgress(string gen, string maxfitness, string avgfitness)
            {
                  MethodInvoker progressInvoker = () =>
                  {
                        txttestvalue .AppendText("第" + gen .ToString() + "代，利用率：" + maxfitness .ToString() + "\r\n");
                        txttestvalue .Focus();
                        txttestvalue .ScrollToCaret();
                        UpdateChart(Convert .ToDouble(maxfitness), Convert .ToDouble(avgfitness));
                  };
                  if (txttestvalue .InvokeRequired)
                  {
                        txttestvalue .Invoke(progressInvoker);
                  }
                  else if (chart1 .InvokeRequired)
                  {
                        chart1 .Invoke(progressInvoker);
                  }
                  else
                  {
                        progressInvoker();
                  }
            }
            private void UpdateChart(double value, double avgvalue)
            {
                  _dataQueue .Enqueue(value);
                  _dataQueueAvg .Enqueue(avgvalue);
                  this .chart1 .Series[0] .Points .Clear();
                  for (int i = 0; i < _dataQueue .Count; i++)
                  {
                        this .chart1 .Series[0] .Points .AddXY((i + 1), _dataQueue .ElementAt(i));
                  }
                  this .chart1 .Series[1] .Points .Clear();
                  for (int i = 0; i < _dataQueueAvg .Count; i++)
                  {
                        this .chart1 .Series[1] .Points .AddXY((i + 1), _dataQueueAvg .ElementAt(i));
                  }
            }
            private void ShowProgress(string current, string total)
            {
                  MethodInvoker progressInvoker = () =>
                  {
                        txtprogress .Text = current + "/" + total;
                  };
                  if (txtprogress .InvokeRequired)
                  {
                        txtprogress .Invoke(progressInvoker);
                  }
                  else
                  {
                        progressInvoker();
                  }
            }
            private void DoWork()
            {
                  Go(GeneProgress, ShowProgress);
            }
            private void Go(Action<string, string, string> GeneProgress, Action<string, string> ShowProgress)
            {
                  _size = Convert .ToInt32(txtSize .Text);//种群大小
                  _pcrossover = Convert .ToDecimal(txtpcrossover .Text);
                  _pmutation = Convert .ToDecimal(txtmutation .Text);
                  Create();
                  int gen = 1;
                  while (!_stop)
                  {
                        List<DNA> newpop = new List<DNA>();
                        //精英保留
                        CompareDNA comp = new CompareDNA();
                        _pop .Sort(comp);
                        DNA bestdna = _pop[0] .Copy();

                        for (int i = 0; i < _size; i += 2)
                        {
                              DNA dna1 = SelectOne();
                              DNA dna2 = SelectOne();

                              DNA child1 = new DNA();
                              DNA child2 = new DNA();

                              Tuple<DNA, DNA> tp = Crossover(dna1, dna2);
                              child1 = tp .Item1;
                              child2 = tp .Item2;

                              DNA dnamutation1 = Mutation(child1);
                              if (dnamutation1 != null)
                              {
                                    newpop .Add(dnamutation1);
                              }
                              else
                              {
                                    newpop .Add(child1);
                              }

                              DNA dnamutation2 = Mutation(child2);
                              if (dnamutation2 != null)
                              {
                                    newpop .Add(dnamutation2);
                              }
                              else
                              {
                                    newpop .Add(child2);
                              }

                              ShowProgress?.Invoke((i + 2) .ToString(), _size .ToString());
                        }

                        newpop .Sort(comp);
                        newpop .RemoveAt(newpop .Count - 1);
                        newpop .Add(bestdna);
                        _pop = newpop;
                        //Filter();
                        float maxfitness = 0;
                        float avgfitness = 0;
                        float sumfitness = 0;
                        int bestindex = 0;
                        for (int i = 0; i < _pop .Count; i++)
                        {
                              sumfitness += _pop[i] .Fitness;
                              if (_pop[i] .Fitness > maxfitness)
                              {
                                    maxfitness = _pop[i] .Fitness;
                                    bestindex = i;
                              }
                        }
                        avgfitness = sumfitness / _pop .Count;
                        //_best = _pop[bestindex] .Stock[0] .Copy();
                        GeneProgress?.Invoke(gen .ToString(), maxfitness .ToString(), avgfitness .ToString());
                        gen++;
                  }

                  //test
                  /*string str = "";
                  for (int i = 0; i < _pop .Count; i++)
                  {
                        List<int[]> dna = new List<int[]>();
                        dna = _pop[i];
                        str += "【【【";
                        for (int j = 0; j < dna .Count; j++)
                        {
                              str += "[" + dna[j][0] .ToString() + "," + dna[j][1] .ToString() + "] ";
                        }
                        str += "】】】\r\n";
                  }
                  txttestvalue .Text = str;*/
            }
            /// <summary>
            /// 停止
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnStop_Click(object sender, EventArgs e)
            {
                  _stop = true;
                  _watch .Stop();
                  TimeSpan timespan = _watch .Elapsed;
                  txttestvalue .AppendText((timespan .TotalMilliseconds / 1000d) .ToString() + "s\r\n");
            }
            /// <summary>
            /// 生成初始种群
            /// </summary>
            private void Create()
            {
                  int c = _part .Count;
                  int sc = _stock .Count;
                  for (int i = 0; i < _size; i++)
                  {
                        List<Basic> basiclist = new List<Basic>();
                        int[] sort = new int[c];
                        sort = RandomSort(c);//随机顺序
                        for (int j = 0; j < sort .Length; j++)
                        {
                              Random r = new Random(GetRandomSeed());
                              int s = sort[j];
                              int id = _part[s] .id;
                              int angle = r .Next(0, 4) * 90;
                              int angleCombine = r .Next(0, 2) * 90;
                              float width = 0f;
                              float height = 0f;
                              //0;
                              // r .Next(0, 360);//随机旋转角度0-359
                              int[] baseGroup = new int[] { id, angle, angleCombine };

                              string key = id .ToString() + "/" + angle .ToString();//旋转后加入基因库
                              CopyOper co = new CopyOper();
                              PlateHelper ph = new PlateHelper();
                              GridHelper gh = new GridHelper();
                              if (!_basicLib .ContainsKey(key))
                              {
                                    PlateModel pm2 = new PlateModel();
                                    pm2 = ph .RotateAndMove(co .CopyPlate(_part[s]), Convert .ToSingle(angle));
                                    GridLib gl = gh .GetGridValue(pm2, T);
                                    _basicLib .Add(key, gl);
                              }
                              string key2 = "C" + id .ToString() + "/" + angleCombine;
                              if (!_basicLib .ContainsKey(key2))
                              {
                                    List<PlateCombine> pclist = _partCombine .Where(t => t .id == s) .ToList();
                                    if (pclist .Count > 0)
                                    {
                                          GridLib gl = gh .GetGridValueCombine(ph .RotateAndMove(pclist[0], Convert .ToSingle(angleCombine)), T);
                                          _basicLib .Add(key2, gl);
                                    }
                              }
                              string key3 = id .ToString() + "/" + ((angle + 90) % 360) .ToString();//再次旋转90度后加入
                              if (!_basicLib .ContainsKey(key3))
                              {
                                    PlateModel pm3 = new PlateModel();
                                    pm3 = ph .RotateAndMove(co .CopyPlate(_part[s]), Convert .ToSingle((angle + 90) % 360));
                                    GridLib gl = gh .GetGridValue(pm3, T);
                                    _basicLib .Add(key3, gl);
                              }
                              string key4 = id .ToString() + "/" + ((angle + 180) % 360) .ToString();//再次旋转90度后加入
                              if (!_basicLib .ContainsKey(key4))
                              {
                                    PlateModel pm4 = new PlateModel();
                                    pm4 = ph .RotateAndMove(co .CopyPlate(_part[s]), Convert .ToSingle((angle + 180) % 360));
                                    GridLib gl = gh .GetGridValue(pm4, T);
                                    _basicLib .Add(key4, gl);
                              }
                              basiclist .Add(new Basic(id, angle, angleCombine, _part[s] .Area, _part[s] .PlateCount));
                        }

                        List<Stock> dnastock = new List<Stock>();
                        int[] sortstock = new int[c];
                        sortstock = RandomSort(sc);
                        for (int k = 0; k < sortstock .Length; k++)
                        {
                              int s = sortstock[k];
                              dnastock .Add(_stock[s] .Copy());
                        }
                        DNA dnathis = new DNA(dnastock, basiclist);
                        dnathis = CountFitnessRectangle(dnathis);
                        _pop .Add(dnathis);
                  }

                  //Parallel .ForEach(_pop, item => item = CountFitnessRectangle(item));//, new ParallelOptions { MaxDegreeOfParallelism = 5 }
            }
            /// <summary>
            /// 生成随机序列
            /// </summary>
            /// <param name="n"></param>
            /// <param name="seedEx"></param>
            /// <returns></returns>
            private int[] RandomSort(int n)
            {
                  List<int> poptmp = new List<int>();
                  for (int i = 0; i < n; i++)
                  {
                        poptmp .Add(i);
                  }

                  int[] result = new int[n];
                  for (int j = 0; j < n; j++)
                  {
                        Random r = new Random(GetRandomSeed());
                        int index = r .Next(0, n - j);
                        int v = poptmp[index];
                        result[j] = v;
                        poptmp .RemoveAt(index);
                  }
                  return result;
            }
            /// <summary>
            /// 随机种子
            /// </summary>
            /// <returns></returns>
            private int GetRandomSeed()
            {
                  byte[] bytes = new byte[4];
                  System .Security .Cryptography .RNGCryptoServiceProvider rng = new System .Security .Cryptography .RNGCryptoServiceProvider();
                  rng .GetBytes(bytes);
                  return BitConverter .ToInt32(bytes, 0);
            }
            /// <summary>
            /// 计算栅格码，先从库中找
            /// </summary>
            /// <param name="basic"></param>
            /// <returns></returns>
            private Tuple<GridLib, GridLib, GridLib, GridLib> GetBasic(Basic basic)
            {
                  int id = basic .Id;
                  int angle = basic .Angle;
                  int anglecombine = basic .AngleCombine;
                  string key = id .ToString() + "/" + angle .ToString();
                  string key2 = "C" + id .ToString() + "/" + anglecombine .ToString();
                  string key3 = id .ToString() + "/" + ((angle + 90) % 360) .ToString();
                  string key4 = id .ToString() + "/" + ((angle + 180) % 360) .ToString();
                  PlateHelper ph = new PlateHelper();
                  GridHelper gh = new GridHelper();
                  GridLib gl = new GridLib();
                  if (!_basicLib .ContainsKey(key))
                  {
                        PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                        gl = gh .GetGridValue(ph .RotateAndMove(pm, Convert .ToSingle(angle)), T);
                        _basicLib .Add(key, gl);
                  }
                  else
                        gl = _basicLib[key];

                  GridLib gl2 = new GridLib();
                  if (!_basicLib .ContainsKey(key2))
                  {
                        List<PlateCombine> pclist = _partCombine .Where(t => t .id == id) .ToList();
                        if (pclist .Count > 0)
                        {
                              PlateCombine pc = pclist[0];
                              gl2 = gh .GetGridValueCombine(ph .RotateAndMove(pc, Convert .ToSingle(angle)), T);
                              _basicLib .Add(key2, gl2);
                        }
                  }
                  else
                        gl2 = _basicLib[key2];

                  GridLib gl3 = new GridLib();
                  if (!_basicLib .ContainsKey(key3))
                  {
                        PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                        gl3 = gh .GetGridValue(ph .RotateAndMove(pm, Convert .ToSingle((angle + 90) % 360)), T);
                        _basicLib .Add(key3, gl3);
                  }
                  else
                        gl3 = _basicLib[key3];

                  GridLib gl4 = new GridLib();
                  if (!_basicLib .ContainsKey(key4))
                  {
                        PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                        gl4 = gh .GetGridValue(ph .RotateAndMove(pm, Convert .ToSingle((angle + 180) % 360)), T);
                        _basicLib .Add(key4, gl4);
                  }
                  else
                        gl4 = _basicLib[key4];
                  return new Tuple<GridLib, GridLib, GridLib, GridLib>(gl, gl2, gl3, gl4);
            }
            #region 剩余矩形动态匹配算法
            /// <summary>
            /// 剩余矩形动态匹配算法解码
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            private DNA CountFitnessRectangle(DNA d)
            {
                  DNA dna = d .Copy();
                  float sumArea = 0f;
                  //所有库存初始化
                  for (int i = 0; i < dna .Stock .Count; i++)
                  {
                        //检测组合模型大小
                        float limit = dna .Stock[i] .Height;
                        PlateHelper ph = new PlateHelper();
                        //List<PlateCombine> pc = _partCombine .Where(t => t .Rect .Height >= limit || t .Rect .Width >= limit) .ToList();
                        for (int t = 0; t < _partCombine .Count; t++)
                        {
                              if (_partCombine[t] .Rect .Height >= limit || _partCombine[t] .Rect .Width >= limit)
                              {
                                    PlateCombine pcold = _partCombine[t];
                                    PlateModel pm = _part .Where(item => item .id == pcold .id) .ToList()[0];
                                    PlateCombine pcnew = ph .GetMinPlateCombine(pm, T, _type, limit);
                                    pcnew .id = pcold .id;
                                    _partCombine[t] = pcnew;
                                    var b = _basicLib .Keys .Where(item => item .Contains("C" + pcnew .id + "/")) .ToList();
                                    for (int n = 0; n < b .Count; n++)
                                    {
                                          _basicLib .Remove(b[n]);
                                    }
                              }
                        }

                        dna .Stock[i] .PartInfoList .Clear();
                        dna .Stock[i] .Use = 0;
                        for (int r = 0; r < dna .Stock[i] .Disable .GetLength(0); r++)
                        {
                              for (int c = 0; c < dna .Stock[i] .Disable .GetLength(1); c++)
                              {
                                    dna .Stock[i] .Disable[r, c] = 0;
                              }
                        }

                        List<MyRectangle> rect = new List<MyRectangle>();//剩余空间
                        Stock s = dna .Stock[i];
                        rect .Add(new MyRectangle(0, 0, s .Disable .GetLength(0), s .Disable .GetLength(1)));

                        while (rect .Count > 0)
                        {
                              rect .Sort(new CompareRect());
                              MyRectangle r = rect[0];
                              int n = 0;
                              bool nestin = false;
                              while (n < dna .Basic .Count)
                              {
                                    Basic b = dna .Basic[n];
                                    int count = b .Count;
                                    if (count == 0)
                                    {
                                          n++;
                                          continue;
                                    }
                                    var tp = GetBasic(b);
                                    List<GridData> grid = tp .Item1 .Grid;
                                    List<GridData> gridCombine = tp .Item2 .Grid;
                                    int[,] array = tp .Item1 .GridArray;
                                    int[,] arrayCombine = tp .Item2 .GridArray;

                                    List<GridData> grid90 = tp .Item3 .Grid;
                                    int[,] array90 = tp .Item3 .GridArray;

                                    float area = b .Area;

                                    bool find = false;
                                    //判断组合零件是否可以排在此处
                                    if (count > 1 && gridCombine != null)
                                    {
                                          int width = arrayCombine .GetLength(0);
                                          int height = arrayCombine .GetLength(1);
                                          if (height <= r .Height
                                                && width <= r .Width)// && (height > _small || width > _small)
                                          {
                                                find = true;
                                                Point p = BottomLeftMove(ref s, ref rect, ref arrayCombine, ref gridCombine);
                                                s .PartInfoList .Add(new PartInfo(b .Id, "2", b .AngleCombine, p));
                                                #region 方法
                                                ////微调，向左向上移动
                                                //int movex = 0;
                                                //int movey = 0;
                                                //string last = "first";
                                                //while (1 == 1)
                                                //{
                                                //      if (last == "first" || last == "up")//如果上一次移动是向上移动，则计算是否能向左移动。第一次都要计算，先向左移
                                                //      {
                                                //            int minx = -1;
                                                //            for (int h = 0; h < height; h++)
                                                //            {
                                                //                  int right = 0;
                                                //                  while (arrayCombine[right, h] <= 0)
                                                //                  {
                                                //                        right++;
                                                //                  }
                                                //                  int left = 0;
                                                //                  while (r .X - left - 1 - movex >= 0 && s .Disable[r .X - left - 1 - movex, r .Y + h - movey] <= 0)
                                                //                  {
                                                //                        left++;
                                                //                  }
                                                //                  if (right + left < minx || minx < 0)
                                                //                  {
                                                //                        minx = right + left;
                                                //                  }
                                                //                  if (minx == 0)
                                                //                  {
                                                //                        break;
                                                //                  }
                                                //            }
                                                //            movex += minx;
                                                //            if (last != "first" && minx == 0)
                                                //            {
                                                //                  break;
                                                //            }
                                                //            else if (last != "first")
                                                //            {
                                                //                  last = "left";
                                                //            }
                                                //      }
                                                //      if (last == "first" || last == "left")//如果上一次是向左移动，则计算是否可以向上移动，第一次都要计算
                                                //      {
                                                //            int miny = -1;
                                                //            for (int w = 0; w < width; w++)
                                                //            {
                                                //                  int down = 0;
                                                //                  while (arrayCombine[w, down] <= 0)
                                                //                  {
                                                //                        down++;
                                                //                  }
                                                //                  int up = 0;
                                                //                  while (r .Y - up - 1 - movey >= 0 && s .Disable[r .X + w - movex, r .Y - up - 1 - movey] <= 0)
                                                //                  {
                                                //                        up++;
                                                //                  }
                                                //                  if (down + up < miny || miny < 0)
                                                //                  {
                                                //                        miny = down + up;
                                                //                  }
                                                //                  if (miny == 0)
                                                //                  {
                                                //                        break;
                                                //                  }
                                                //            }
                                                //            movey += miny;
                                                //            if (miny == 0)
                                                //            {
                                                //                  break;
                                                //            }
                                                //            else
                                                //            {
                                                //                  last = "up";
                                                //            }
                                                //      }
                                                //}
                                                ////排入
                                                //for (int j = 0; j < gridCombine .Count; j++)
                                                //{
                                                //      int row = gridCombine[j] .Row;
                                                //      int col = gridCombine[j] .Col;
                                                //      //test
                                                //      /*if (s .Disable[row + r .X, col + r .Y] != 0)
                                                //      {
                                                //            int test = 1;
                                                //      }*/
                                                //      s .Disable[row + r .X - movex, col + r .Y - movey] = gridCombine[j] .Value;
                                                //}
                                                ////Parallel .ForEach(gridCombine, item => s .Disable[item .Row + r .X, item .Col + r .Y] = item .Value);
                                                //if ((r .X + width - movex) * T > s .Use)
                                                //{
                                                //      s .Use = (r .X + width - movex) * T;
                                                //}
                                                //MyRectangle rpart = new MyRectangle(r .X - movex, r .Y - movey, width, height);
                                                //rect = RectangleCreate(rect, rpart);
                                                #endregion
                                                dna .Basic[n] .Count -= 2;
                                                sumArea += area * 2;
                                                nestin = true;
                                                break;
                                          }
                                    }
                                    //判断单一图形是否可以排入
                                    if (!find)
                                    {
                                          int width = array .GetLength(0);
                                          int height = array .GetLength(1);
                                          if (height <= r .Height
                                                && width <= r .Width)//&& (height > _small || width > _small)
                                          {
                                                find = true;
                                                Point p = BottomLeftMove(ref s, ref rect, ref array, ref grid);
                                                s .PartInfoList .Add(new PartInfo(b .Id, "1", b .Angle, p));
                                                #region 方法
                                                ////微调，向左向上移动
                                                //int movex = 0;
                                                //int movey = 0;
                                                //string last = "first";
                                                //while (1 == 1)
                                                //{
                                                //      if (last == "first" || last == "up")//如果上一次移动是向上移动，则计算是否能向左移动。第一次都要计算，先向左移
                                                //      {
                                                //            int minx = -1;
                                                //            for (int h = 0; h < height; h++)
                                                //            {
                                                //                  int right = 0;
                                                //                  while (array[right, h] <= 0)
                                                //                  {
                                                //                        right++;
                                                //                  }
                                                //                  int left = 0;
                                                //                  while (r .X - left - 1 - movex >= 0 && s .Disable[r .X - left - 1 - movex, r .Y + h - movey] <= 0)
                                                //                  {
                                                //                        left++;
                                                //                  }
                                                //                  if (right + left < minx || minx < 0)
                                                //                  {
                                                //                        minx = right + left;
                                                //                  }
                                                //                  if (minx == 0)
                                                //                  {
                                                //                        break;
                                                //                  }
                                                //            }
                                                //            movex += minx;
                                                //            if (last != "first" && minx == 0)
                                                //            {
                                                //                  break;
                                                //            }
                                                //            else if (last != "first")
                                                //            {
                                                //                  last = "left";
                                                //            }
                                                //      }
                                                //      if (last == "first" || last == "left")//如果上一次是向左移动，则计算是否可以向上移动，第一次都要计算
                                                //      {
                                                //            int miny = -1;
                                                //            for (int w = 0; w < width; w++)
                                                //            {
                                                //                  int down = 0;
                                                //                  while (array[w, down] <= 0)
                                                //                  {
                                                //                        down++;
                                                //                  }
                                                //                  int up = 0;
                                                //                  while (r .Y - up - 1 - movey >= 0 && s .Disable[r .X + w - movex, r .Y - up - 1 - movey] <= 0)
                                                //                  {
                                                //                        up++;
                                                //                  }
                                                //                  if (down + up < miny || miny < 0)
                                                //                  {
                                                //                        miny = down + up;
                                                //                  }
                                                //                  if (miny == 0)
                                                //                  {
                                                //                        break;
                                                //                  }
                                                //            }
                                                //            movey += miny;
                                                //            if (miny == 0)
                                                //            {
                                                //                  break;
                                                //            }
                                                //            else
                                                //            {
                                                //                  last = "up";
                                                //            }
                                                //      }
                                                //}
                                                ////排入
                                                //for (int j = 0; j < grid .Count; j++)
                                                //{
                                                //      int row = grid[j] .Row;
                                                //      int col = grid[j] .Col;
                                                //      //test
                                                //      /*if (s .Disable[row + r .X, col + r .Y] != 0)
                                                //      {
                                                //            int test = 1;
                                                //      }*/

                                                //      s .Disable[row + r .X - movex, col + r .Y - movey] = grid[j] .Value;
                                                //}
                                                ////Parallel .ForEach(grid, item => s .Disable[item .Row + r .X, item .Col + r .Y] = item .Value);
                                                //if ((r .X - movex + width) * T > s .Use)
                                                //{
                                                //      s .Use = (r .X + width - movex) * T;
                                                //}
                                                //MyRectangle rpart = new MyRectangle(r .X - movex, r .Y - movey, width, height);
                                                //rect = RectangleCreate(rect, rpart);
                                                #endregion
                                                dna .Basic[n] .Count--;
                                                if (_rotate)//单件排入后旋转180度
                                                {
                                                      dna .Basic[n] .Angle = (dna .Basic[n] .Angle + 180) % 360;
                                                }
                                                sumArea += area;
                                                nestin = true;
                                                break;
                                          }
                                    }
                                    //单一图形旋转90度后再次试排
                                    if (!find)
                                    {
                                          int width = array90 .GetLength(0);
                                          int height = array90 .GetLength(1);
                                          if (height <= r .Height
                                                && width <= r .Width)//&& (height > _small || width > _small)
                                          {
                                                find = true;
                                                Point p = BottomLeftMove(ref s, ref rect, ref array90, ref grid90);
                                                s .PartInfoList .Add(new PartInfo(b .Id, "1", (b .Angle + 90) % 360, p));
                                                dna .Basic[n] .Count--;
                                                if (_rotate)//单件排入后旋转180度
                                                {
                                                      dna .Basic[n] .Angle = (dna .Basic[n] .Angle + 180) % 360;
                                                }
                                                sumArea += area;
                                                nestin = true;
                                                break;
                                          }
                                    }
                                    //如果当前零件不合适，则向后搜索宽度最匹配零件插入当前位置
                                    if (!find)
                                    {
                                          int bestheight = 0;
                                          int bestindex = -1;
                                          for (int t = n + 1; t < dna .Basic .Count; t++)
                                          {
                                                Basic test = dna .Basic[t];
                                                int testcount = test .Count;

                                                if (testcount == 0)
                                                {
                                                      continue;
                                                }
                                                var testtp = GetBasic(test);
                                                List<GridData> testgrid = testtp .Item1 .Grid;
                                                List<GridData> testgridCombine = testtp .Item2 .Grid;
                                                int[,] testarray = testtp .Item1 .GridArray;
                                                int[,] testarrayCombine = testtp .Item2 .GridArray;

                                                List<GridData> testgrid90 = testtp .Item3 .Grid;
                                                int[,] testarray90 = testtp .Item3 .GridArray;

                                                bool testfind = false;
                                                if (testcount > 1 && testgridCombine != null)
                                                {
                                                      int testwidth = testarrayCombine .GetLength(0);
                                                      int testheight = testarrayCombine .GetLength(1);
                                                      if (testheight <= r .Height
                                                            && testwidth <= r .Width)//&& (testheight > _small || testwidth > _small)
                                                      {
                                                            testfind = true;
                                                            if (testheight > bestheight)
                                                            {
                                                                  bestheight = testheight;
                                                                  bestindex = t;
                                                            }
                                                      }
                                                }
                                                if (!testfind)
                                                {
                                                      int testwidth = testarray .GetLength(0);
                                                      int testheight = testarray .GetLength(1);
                                                      if (testheight <= r .Height
                                                            && testwidth <= r .Width)//&& (testheight > _small || testwidth > _small)
                                                      {
                                                            testfind = true;
                                                            if (testheight > bestheight)
                                                            {
                                                                  bestheight = testheight;
                                                                  bestindex = t;
                                                            }
                                                      }
                                                }
                                                if (!testfind)
                                                {
                                                      int testwidth = testarray90 .GetLength(0);
                                                      int testheight = testarray90 .GetLength(1);
                                                      if (testheight <= r .Height
                                                            && testwidth <= r .Width)//&& (testheight > _small || testwidth > _small)
                                                      {
                                                            testfind = true;
                                                            if (testheight > bestheight)
                                                            {
                                                                  bestheight = testheight;
                                                                  bestindex = t;
                                                            }
                                                      }
                                                }
                                          }
                                          if (bestindex > 0)
                                          {
                                                Basic testb = dna .Basic[bestindex] .Copy();
                                                dna .Basic .RemoveAt(bestindex);
                                                dna .Basic .Insert(n + 1, testb);//前插会导致小件挤在左边
                                                find = true;
                                                n++;
                                          }
                                          else
                                          {
                                                find = false;
                                                n++;
                                          }
                                    }
                              }

                              if (!nestin)
                              {
                                    rect .RemoveAt(0);//若无法排入任何件号，则删除该矩形
                              }
                        }//end while (rect .Count > 0)

                        #region 剩余零件插孔排入
                        for (int b = 0; b < dna .Basic .Count; b++)
                        {
                              int count = dna .Basic[b] .Count;
                              if (count > 0)
                              {
                                    Basic basic = dna .Basic[b];
                                    var tp = GetBasic(basic);
                                    List<GridData> grid = tp .Item1 .Grid;
                                    List<GridData> gridCombine = tp .Item2 .Grid;
                                    int[,] array = tp .Item1 .GridArray;
                                    int[,] arrayCombine = tp .Item2 .GridArray;
                                    List<GridData> grid90 = tp .Item3 .Grid;
                                    int[,] array90 = tp .Item3 .GridArray;
                                    List<GridData> grid180 = tp .Item4 .Grid;
                                    int[,] array180 = tp .Item4 .GridArray;

                                    int length = -1;
                                    int width = -1;
                                    if (array != null)
                                    {
                                          length = array .GetLength(0);
                                          width = array .GetLength(1);
                                    }
                                    int lengthC = -1;
                                    int widthC = -1;
                                    if (arrayCombine != null)
                                    {
                                          lengthC = arrayCombine .GetLength(0);
                                          widthC = arrayCombine .GetLength(1);
                                    }
                                    int length90 = -1;
                                    int width90 = -1;
                                    if (array90 != null)
                                    {
                                          length90 = array90 .GetLength(0);
                                          width90 = array90 .GetLength(1);
                                    }
                                    int length180 = -1;
                                    int width180 = -1;
                                    if (array180 != null)
                                    {
                                          length180 = array180 .GetLength(0);
                                          width180 = array180 .GetLength(1);
                                    }
                                    float area = basic .Area;

                                    int rowcount = s .Disable .GetLength(0);
                                    int colcount = s .Disable .GetLength(1);

                                    while (count > 0)
                                    {
                                          bool added = false;
                                          if (lengthC < _small && widthC < _small && lengthC > 0 && widthC > 0 && count > 1)//lengthC < _small && widthC < _small && 
                                          {
                                                for (int m = 0; m < rowcount; m++)
                                                {
                                                      //组合图形适配
                                                      int n = 0;
                                                      while (n < colcount && count >= 2)
                                                      {
                                                            //if (s .Disable[m, n] > 0)
                                                            //{
                                                            //      n += s .Disable[m, n];
                                                            //      continue;
                                                            //}

                                                            if (CanAdd(s, gridCombine, m, n))
                                                            {
                                                                  s .Disable = GridAdd(s .Disable, gridCombine, m, n);
                                                                  sumArea += area * 2;
                                                                  added = true;
                                                                  count -= 2;
                                                                  dna .Basic[b] .Count -= 2;
                                                                  s .PartInfoList .Add(new PartInfo(basic .Id, "2", basic .AngleCombine, new Point(m, n)));
                                                                  break;
                                                            }
                                                            n++;
                                                      }
                                                      if (added)
                                                            break;
                                                }//end for(rowcount)
                                          }
                                          if (!added)
                                          {
                                                if (length < _small && width < _small && length > 0 && width > 0 && count > 0)//length < _small && width < _small && 
                                                {
                                                      for (int m = 0; m < rowcount; m++)
                                                      {
                                                            //单一图形适配
                                                            int n = 0;
                                                            while (n < colcount && count >= 1)
                                                            {
                                                                  //if (s .Disable[m, n] > 0)
                                                                  //{
                                                                  //      n += s .Disable[m, n];
                                                                  //      continue;
                                                                  //}

                                                                  if (CanAdd(s, grid, m, n))
                                                                  {
                                                                        s .Disable = GridAdd(s .Disable, grid, m, n);
                                                                        sumArea += area;
                                                                        added = true;
                                                                        count -= 1;
                                                                        dna .Basic[b] .Count -= 1;
                                                                        s .PartInfoList .Add(new PartInfo(basic .Id, "1", basic .Angle, new Point(m, n)));
                                                                        break;
                                                                  }
                                                                  n++;
                                                            }
                                                            if (added)
                                                                  break;
                                                      }//end for(rowcount)
                                                }
                                          }
                                          if (!added)
                                          {
                                                if (length90 < _small && width90 < _small && length90 > 0 && width90 > 0 && count > 0)//length < _small && width < _small && 
                                                {
                                                      for (int m = 0; m < rowcount; m++)
                                                      {
                                                            //单一图形适配旋转90
                                                            int n = 0;
                                                            while (n < colcount && count >= 1)
                                                            {
                                                                  //if (s .Disable[m, n] > 0)
                                                                  //{
                                                                  //      n += s .Disable[m, n];
                                                                  //      continue;
                                                                  //}

                                                                  if (CanAdd(s, grid90, m, n))
                                                                  {
                                                                        s .Disable = GridAdd(s .Disable, grid90, m, n);
                                                                        sumArea += area;
                                                                        added = true;
                                                                        count -= 1;
                                                                        dna .Basic[b] .Count -= 1;
                                                                        s .PartInfoList .Add(new PartInfo(basic .Id, "1", (basic .Angle + 90) % 360, new Point(m, n)));
                                                                        break;
                                                                  }
                                                                  n++;
                                                            }
                                                            if (added)
                                                                  break;
                                                      }//end for(rowcount)
                                                }
                                          }
                                          if (!added)
                                          {
                                                if (length180 < _small && width180 < _small && length180 > 0 && width180 > 0 && count > 0)//length < _small && width < _small && 
                                                {
                                                      for (int m = 0; m < rowcount; m++)
                                                      {
                                                            //单一图形适配旋转180
                                                            int n = 0;
                                                            while (n < colcount && count >= 1)
                                                            {
                                                                  //if (s .Disable[m, n] > 0)
                                                                  //{
                                                                  //      n += s .Disable[m, n];
                                                                  //      continue;
                                                                  //}

                                                                  if (CanAdd(s, grid180, m, n))
                                                                  {
                                                                        s .Disable = GridAdd(s .Disable, grid180, m, n);
                                                                        sumArea += area;
                                                                        added = true;
                                                                        count -= 1;
                                                                        dna .Basic[b] .Count -= 1;
                                                                        s .PartInfoList .Add(new PartInfo(basic .Id, "1", (basic .Angle + 180) % 360, new Point(m, n)));
                                                                        break;
                                                                  }
                                                                  n++;
                                                            }
                                                            if (added)
                                                                  break;
                                                      }//end for(rowcount)
                                                }
                                          }
                                          if (!added)
                                                break;
                                    }
                              }
                        }
                        #endregion
                        dna .Stock[i] = s;
                  }
                  d .Stock = dna .Stock .ToList();

                  //计算每块板的使用量
                  float sumStock = 0f;
                  float fitness = 0f;
                  for (int i = 0; i < dna .Stock .Count; i++)
                  {
                        /*float use = dna .Stock[i] .Use;
                        if (use == 0 && i > 0)//若为0，则上一块为最后一块，-整块面积+使用面积，结束计算
                        {
                              sumStock += (use * dna .Stock[i - 1] .Height - dna .Stock[i - 1] .Height * dna .Stock[i - 1] .Width);
                              break;
                        }
                        else if (use > 0 && i == dna .Stock .Count - 1)//找到最后一个还是大于0，则本块为最后一块
                        {
                              sumStock += use * dna .Stock[i] .Height;
                        }
                        else
                        {
                              sumStock += dna .Stock[i] .Height * dna .Stock[i] .Width;
                        }*/
                        float use = dna .Stock[i] .Use;
                        if (use > 0)
                        {
                              sumStock += dna .Stock[i] .Height * dna .Stock[i] .Width;
                        }
                  }

                  //适应度计算
                  if (sumStock > 0)
                  {
                        fitness = sumArea / sumStock * 100;
                  }
                  d .Fitness = fitness;
                  return d;
            }
            /// <summary>
            /// 按左底原则排入
            /// </summary>
            /// <param name="s"></param>
            /// <param name="r"></param>
            /// <param name="array"></param>
            /// <param name="grid"></param>
            public Point BottomLeftMove(ref Stock s, ref List<MyRectangle> rect, ref int[,] array, ref List<GridData> grid)
            {
                  MyRectangle r = rect[0];
                  int width = array .GetLength(0);
                  int height = array .GetLength(1);

                  //微调，向左向上移动
                  int movex = 0;
                  int movey = 0;
                  string last = "first";
                  while (1 == 1)
                  {
                        if (!chkPress .Checked)
                              break;
                        //搜索方式：先找到距边缘最近的栅格，再从栅格处，反向搜索找到板上距离最近的栅格，计算距离
                        if (last == "first" || last == "up")//如果上一次移动是向上移动，则计算是否能向左移动。第一次都要计算，先向左移
                        {
                              int minx = -1;
                              for (int h = 0; h < height; h++)
                              {
                                    int right = 0;
                                    while (right < width && array[right, h] <= 0)
                                    {
                                          right++;
                                    }
                                    int left = 0;
                                    while (r .X + right - left - 1 - movex >= 0
                                          && r .Y + h - movey >= 0
                                          && s .Disable[r .X + right - left - 1 - movex, r .Y + h - movey] <= 0)
                                    {
                                          left++;
                                    }
                                    if (left < minx || minx < 0)
                                    {
                                          minx = left;
                                    }
                                    if (minx == 0)
                                    {
                                          break;
                                    }
                              }
                              movex += minx;
                              if (last != "first" && minx == 0)
                              {
                                    break;
                              }
                              else if (last != "first")
                              {
                                    last = "left";
                              }
                        }
                        if (last == "first" || last == "left")//如果上一次是向左移动，则计算是否可以向上移动，第一次都要计算
                        {
                              int miny = -1;
                              for (int w = 0; w < width; w++)
                              {
                                    int down = 0;
                                    while (down < height && array[w, down] <= 0)
                                    {
                                          down++;
                                    }
                                    int up = 0;
                                    while (r .Y + down - up - 1 - movey >= 0
                                          && r .X + w - movex >= 0
                                          && s .Disable[r .X + w - movex, r .Y + down - up - 1 - movey] <= 0)
                                    {
                                          up++;
                                    }
                                    if (up < miny || miny < 0)
                                    {
                                          miny = up;
                                    }
                                    if (miny == 0)
                                    {
                                          break;
                                    }
                              }
                              movey += miny;
                              if (miny == 0)
                              {
                                    break;
                              }
                              else
                              {
                                    last = "up";
                              }
                        }
                  }
                  //test
                  //movex = movey = 0;
                  //排入
                  for (int j = 0; j < grid .Count; j++)
                  {
                        int row = grid[j] .Row;
                        int col = grid[j] .Col;
                        //test

                        if (s .Disable[row + r .X, col + r .Y] != 0)
                        {
                              int test = 1;
                        }

                        s .Disable[row + r .X - movex, col + r .Y - movey] = grid[j] .Value;
                  }
                  //Parallel .ForEach(gridCombine, item => s .Disable[item .Row + r .X, item .Col + r .Y] = item .Value);
                  if ((r .X + width - movex) * T > s .Use)
                  {
                        s .Use = (r .X + width - movex) * T;
                  }
                  MyRectangle rpart = new MyRectangle(r .X - movex, r .Y - movey, width, height);
                  rect = RectangleCreate(rect, rpart);
                  return new Point(r .X - movex, r .Y - movey);
            }

            /// <summary>
            /// 生成新的矩阵序列
            /// </summary>
            /// <param name="r"></param>
            /// <param name="rnew"></param>
            /// <returns></returns>
            public List<MyRectangle> RectangleCreate(List<MyRectangle> r, MyRectangle rnew)
            {
                  List<MyRectangle> cutlist = new List<MyRectangle>();
                  for (int i = r .Count - 1; i >= 0; i--)
                  {
                        List<MyRectangle> rectlist = new List<MyRectangle>();
                        rectlist = RectangleCut(r[i], rnew);
                        if (rectlist .Count > 0)
                        {
                              cutlist .AddRange(rectlist);
                        }
                        if (RectangleCross(r[i], rnew))//相交不一定会产生子矩形（重叠）
                        {
                              r .RemoveAt(i);
                        }
                  }
                  if (cutlist .Count > 0)
                  {
                        r = RectangleCombine(r, cutlist);
                  }
                  return r;
            }
            /// <summary>
            /// 矩形列表合并，消除被包含的矩形
            /// </summary>
            /// <returns></returns>
            public List<MyRectangle> RectangleCombine(List<MyRectangle> r, List<MyRectangle> rnew)
            {
                  for (int i = 0; i < rnew .Count; i++)
                  {
                        bool find = false;
                        for (int j = r .Count - 1; j >= 0; j--)
                        {
                              if (RectangleContain(r[j], rnew[i]))
                              {
                                    find = true;
                                    break;
                              }
                              else if (RectangleContain(rnew[i], r[j]))
                              {
                                    r .RemoveAt(j);
                              }
                        }
                        //如果没有找到可以包含这个矩形的，则将此矩形加入目标序列中
                        if (!find)
                        {
                              r .Insert(0, rnew[i] .Copy());
                        }
                  }
                  return r;
            }
            /// <summary>
            /// 判断矩形相交
            /// </summary>
            /// <param name="r0"></param>
            /// <param name="rA"></param>
            /// <returns></returns>
            public bool RectangleCross(MyRectangle r0, MyRectangle rA)
            {
                  if (r0 .Y + r0 .Height >= rA .Y
                        && r0 .X + r0 .Width >= rA .X
                        && r0 .Y <= rA .Y + rA .Height
                        && r0 .X <= rA .X + rA .Width)
                  {
                        return true;
                  }
                  return false;
            }
            /// <summary>
            /// 矩形分割
            /// </summary>
            public List<MyRectangle> RectangleCut(MyRectangle r0, MyRectangle rA)
            {
                  int type = r0 .Type;
                  List<MyRectangle> list = new List<MyRectangle>();
                  if (r0 .Y + r0 .Height >= rA .Y
                        && r0 .X + r0 .Width >= rA .X
                        && r0 .Y <= rA .Y + rA .Height
                        && r0 .X <= rA .X + rA .Width)
                  {
                        if (rA .Y - r0 .Y > 0)//上
                        {
                              list .Add(new MyRectangle(r0 .X, r0 .Y, r0 .Width, rA .Y - r0 .Y, type));
                        }
                        if (r0 .Y + r0 .Height - rA .Y - rA .Height > 0)//下
                        {
                              list .Add(new MyRectangle(r0 .X, rA .Y + rA .Height, r0 .Width, r0 .Y + r0 .Height - rA .Y - rA .Height, type));
                        }
                        if (rA .X - r0 .X > 0)//左，生成的矩形为孔
                        {
                              list .Add(new MyRectangle(r0 .X, r0 .Y, rA .X - r0 .X, r0 .Height, 1));
                        }
                        if (r0 .X + r0 .Width - rA .X - rA .Width > 0)//右
                        {
                              list .Add(new MyRectangle(rA .X + rA .Width, r0 .Y, r0 .X + r0 .Width - rA .X - rA .Width, r0 .Height, type));
                        }
                  }
                  return list;
            }
            /// <summary>
            /// 判断矩形包含
            /// </summary>
            /// <param name="r0"></param>
            /// <param name="rA"></param>
            /// <returns></returns>
            public bool RectangleContain(MyRectangle r0, MyRectangle rA)
            {
                  if (rA .X >= r0 .X
                        && rA .Y >= r0 .Y
                        && rA .X + rA .Width <= r0 .X + r0 .Width
                        && rA .Y + rA .Height <= r0 .Y + r0 .Height)
                  {
                        return true;
                  }
                  return false;
            }
            /// <summary>
            /// 剩余矩形按照x值方向排序
            /// </summary>
            public class CompareRect : IComparer<MyRectangle>
            {
                  public int Compare(MyRectangle rect1, MyRectangle rect2)
                  {
                        if (rect1 .X > rect2 .X)
                        {
                              return 1;
                        }
                        else if (rect1 .X == rect2 .X)
                        {
                              if (rect1 .Y > rect2 .Y)
                              {
                                    return 1;
                              }
                              else if (rect1 .Y < rect2 .Y)
                              {
                                    return -1;
                              }
                              else
                              {
                                    if (rect1 .Height > rect2 .Height)
                                    {
                                          return 1;
                                    }
                                    return -1;
                              }
                        }
                        return -1;
                  }
            }
            #endregion
            /// <summary>
            /// 解码计算适应度
            /// </summary>
            private float CountFitness(DNA dna)
            {
                  //所有库存初始化
                  for (int i = 0; i < dna .Stock .Count; i++)
                  {
                        for (int r = 0; r < dna .Stock[i] .Disable .GetLength(0); r++)
                        {
                              for (int c = 0; c < dna .Stock[i] .Disable .GetLength(1); c++)
                              {
                                    dna .Stock[i] .Disable[r, c] = 0;
                              }
                        }
                  }
                  float sumArea = 0f;
                  float sumStock = 0f;
                  for (int i = 0; i < dna .Basic .Count; i++)
                  {
                        var tp = GetBasic(dna .Basic[i]);
                        int count = dna .Basic[i] .Count;
                        List<GridData> grid = tp .Item1 .Grid;
                        List<GridData> gridcombine = tp .Item2 .Grid;
                        while (count > 0)
                        {
                              bool added = false;
                              //先判断组合图形是否可以排入
                              //如果无法排入，在判断单一图形是否可以排入
                              //如果都无法排入，换下一块板，如果全都无法排入，放弃，继续下一个件号
                              for (int j = 0; j < dna .Stock .Count; j++)
                              {
                                    int rowcount = dna .Stock[j] .Disable .GetLength(0);
                                    int colcount = dna .Stock[j] .Disable .GetLength(1);

                                    for (int m = 0; m < rowcount; m++)
                                    {
                                          //组合图形适配
                                          int n = 0;
                                          while (n < colcount && count >= 2)
                                          {
                                                if (dna .Stock[j] .Disable[m, n] > 0)
                                                {
                                                      n += dna .Stock[j] .Disable[m, n];
                                                      continue;
                                                }

                                                if (CanAdd(dna .Stock[j], gridcombine, m, n))
                                                {
                                                      dna .Stock[j] .Disable = GridAdd(dna .Stock[j] .Disable, gridcombine, m, n);
                                                      sumArea += dna .Basic[i] .Area;
                                                      added = true;
                                                      count -= 2;
                                                      break;
                                                }
                                                n++;
                                          }
                                          if (added)
                                                break;
                                    }//end for(rowcount)
                              }//end for(stock.count)
                              if (!added)
                              {
                                    for (int j = 0; j < dna .Stock .Count; j++)
                                    {
                                          int rowcount = dna .Stock[j] .Disable .GetLength(0);
                                          int colcount = dna .Stock[j] .Disable .GetLength(1);

                                          for (int m = 0; m < rowcount; m++)
                                          {
                                                //组合图形适配
                                                int n = 0;
                                                while (n < colcount && count >= 2)
                                                {
                                                      if (dna .Stock[j] .Disable[m, n] > 0)
                                                      {
                                                            n += dna .Stock[j] .Disable[m, n];
                                                            continue;
                                                      }

                                                      if (CanAdd(dna .Stock[j], grid, m, n))
                                                      {
                                                            dna .Stock[j] .Disable = GridAdd(dna .Stock[j] .Disable, grid, m, n);
                                                            sumArea += dna .Basic[i] .Area;
                                                            added = true;
                                                            count -= 1;
                                                            break;
                                                      }
                                                      n++;
                                                }
                                                if (added)
                                                      break;
                                          }//end for(rowcount)
                                    }//end for(stock.count)
                              }
                              if (!added)
                                    break;
                        }//end while(count>0)
                  }
                  //计算每块板的使用量
                  for (int i = 0; i < dna .Stock .Count; i++)
                  {
                        int index = 0;
                        for (int r = 0; r < dna .Stock[i] .Disable .GetLength(0); r++)
                        {
                              bool find = false;
                              for (int c = 0; c < dna .Stock[i] .Disable .GetLength(1); c++)
                              {
                                    if (dna .Stock[i] .Disable[r, c] != 0)
                                    {
                                          find = true;
                                          break;
                                    }
                              }
                              if (!find)
                              {
                                    if (r > 0)
                                    {
                                          index = r + 1;
                                    }
                                    break;
                              }
                        }
                        dna .Stock[i] .Use = index * T;
                        if (index == 0 && i > 0)//若为0，则上一块为最后一块，-整块面积+使用面积，结束计算
                        {
                              sumStock += (index * T * dna .Stock[i - 1] .Height - dna .Stock[i - 1] .Height * dna .Stock[i - 1] .Width);
                              break;
                        }
                        else if (index > 0 && i == dna .Stock .Count - 1)//找到最后一个还是大于0，则本块为最后一块
                        {
                              sumStock += index * T * dna .Stock[i] .Height;
                        }
                        else
                        {
                              sumStock += dna .Stock[i] .Height * dna .Stock[i] .Width;
                        }
                  }

                  //适应度计算
                  if (sumStock > 0)
                  {
                        return sumArea / sumStock * 100;
                  }
                  return 0;
            }


            /// <summary>
            /// 判断是否可以排入
            /// </summary>
            /// <param name="s"></param>
            /// <param name="pm"></param>
            /// <param name="rowindex"></param>
            /// <param name="colindex"></param>
            /// <returns></returns>
            private bool CanAdd(Stock s, List<GridData> grid, int rowindex, int colindex)
            {
                  int row0 = grid[0] .Row;//先判断特殊点，通过再逐点判断
                  int col0 = grid[0] .Col;
                  int rown = grid[grid .Count - 1] .Row;
                  int coln = grid[grid .Count - 1] .Col;
                  if ((row0 + rowindex + 1) * T > s .Width || (col0 + colindex + 1) * T > s .Height || s .Disable[row0 + rowindex, col0 + colindex] > 0
                        || (rown + rowindex + 1) * T > s .Width || (coln + colindex + 1) * T > s .Height || s .Disable[rown + rowindex, coln + colindex] > 0)
                  {
                        return false;
                  }
                  for (int i = 0; i < grid .Count; i++)
                  {
                        int row = grid[i] .Row;
                        int col = grid[i] .Col;
                        if ((row + rowindex + 1) * T > s .Width || (col + colindex + 1) * T > s .Height)
                        {
                              return false;
                        }
                        if (s .Disable[row + rowindex, col + colindex] > 0)
                        {
                              return false;
                        }
                  }
                  return true;
            }
            /// <summary>
            /// 栅格排入
            /// </summary>
            /// <param name="gridarray"></param>
            /// <param name="pm"></param>
            /// <param name="rowindex"></param>
            /// <param name="colindex"></param>
            /// <returns></returns>
            private int[,] GridAdd(int[,] gridarray, List<GridData> grid, int rowindex, int colindex)
            {
                  for (int i = 0; i < grid .Count; i++)
                  {
                        int row = grid[i] .Row;
                        int col = grid[i] .Col;
                        gridarray[row + rowindex, col + colindex] = grid[i] .Value;
                  }
                  return gridarray;
            }
            /// <summary>
            /// 交叉
            /// </summary>
            /// <param name="dna1"></param>
            /// <param name="dna2"></param>
            private Tuple<DNA, DNA> Crossover(DNA dna1, DNA dna2)
            {
                  DNA new1 = new DNA();
                  DNA new2 = new DNA();
                  if (Flip(_pcrossover))
                  {
                        Random r = new Random(GetRandomSeed());
                        //生成子代1
                        int stockpoint1 = r .Next(0, dna1 .Stock .Count);
                        List<int> sidlist1 = new List<int>();
                        for (int i = stockpoint1; i < dna1 .Stock .Count; i++)
                        {
                              sidlist1 .Add(dna1 .Stock[i] .id);
                              new1 .Stock .Add(dna1 .Stock[i] .Copy());
                        }
                        for (int i = 0; i < dna2 .Stock .Count; i++)
                        {
                              if (!sidlist1 .Contains(dna2 .Stock[i] .id))
                              {
                                    sidlist1 .Add(dna2 .Stock[i] .id);
                                    new1 .Stock .Add(dna2 .Stock[i] .Copy());
                              }
                        }

                        int basicpoint1 = r .Next(0, dna1 .Basic .Count);
                        List<int> bidlist1 = new List<int>();
                        for (int i = basicpoint1; i < dna1 .Basic .Count; i++)
                        {
                              bidlist1 .Add(dna1 .Basic[i] .Id);
                              new1 .Basic .Add(dna1 .Basic[i] .Copy());
                        }
                        for (int i = 0; i < dna2 .Basic .Count; i++)
                        {
                              if (!bidlist1 .Contains(dna2 .Basic[i] .Id))
                              {
                                    bidlist1 .Add(dna2 .Basic[i] .Id);
                                    new1 .Basic .Add(dna2 .Basic[i] .Copy());
                              }
                        }

                        //生成子代2
                        int stockpoint2 = r .Next(0, dna2 .Stock .Count);
                        List<int> sidlist2 = new List<int>();
                        for (int i = stockpoint2; i < dna2 .Stock .Count; i++)
                        {
                              sidlist2 .Add(dna2 .Stock[i] .id);
                              new2 .Stock .Add(dna2 .Stock[i] .Copy());
                        }
                        for (int i = 0; i < dna1 .Stock .Count; i++)
                        {
                              if (!sidlist2 .Contains(dna1 .Stock[i] .id))
                              {
                                    sidlist2 .Add(dna1 .Stock[i] .id);
                                    new2 .Stock .Add(dna1 .Stock[i] .Copy());
                              }
                        }

                        int basicpoint2 = r .Next(0, dna2 .Basic .Count);
                        List<int> bidlist2 = new List<int>();
                        for (int i = basicpoint2; i < dna2 .Basic .Count; i++)
                        {
                              bidlist2 .Add(dna2 .Basic[i] .Id);
                              new2 .Basic .Add(dna2 .Basic[i] .Copy());
                        }
                        for (int i = 0; i < dna1 .Basic .Count; i++)
                        {
                              if (!bidlist2 .Contains(dna1 .Basic[i] .Id))
                              {
                                    bidlist2 .Add(dna1 .Basic[i] .Id);
                                    new2 .Basic .Add(dna1 .Basic[i] .Copy());
                              }
                        }

                        new1 = CountFitnessRectangle(new1);
                        new2 = CountFitnessRectangle(new2);
                  }
                  else
                  {
                        new1 = dna1 .Copy();
                        new2 = dna2 .Copy();
                  }
                  return new Tuple<DNA, DNA>(new1, new2);
            }
            /// <summary>
            /// 变异
            /// </summary>
            /// <param name="dna"></param>
            /// <returns></returns>
            private DNA Mutation(DNA dna)
            {
                  if (Flip(_pmutation))
                  {
                        Random r = new Random(GetRandomSeed());
                        if (Flip(0.5M))
                        {
                              int stockpoint1 = r .Next(0, dna .Stock .Count);
                              int stockpoint2 = r .Next(0, dna .Stock .Count);
                              Stock s1 = dna .Stock[stockpoint1];
                              Stock s2 = dna .Stock[stockpoint2];
                              dna .Stock[stockpoint1] = s2;
                              dna .Stock[stockpoint2] = s1;

                              int basicpoint1 = r .Next(0, dna .Basic .Count);
                              int basicpoint2 = r .Next(0, dna .Basic .Count);
                              Basic b1 = dna .Basic[basicpoint1];
                              Basic b2 = dna .Basic[basicpoint2];
                              dna .Basic[basicpoint1] = b2;
                              dna .Basic[basicpoint2] = b1;
                        }
                        else
                        {
                              int anglepoint = r .Next(0, dna .Basic .Count);
                              int angle = r .Next(0, 4) * 90;
                              int anglecombine = r .Next(0, 2) * 90;
                              dna .Basic[anglepoint] .Angle = angle;
                              dna .Basic[anglepoint] .AngleCombine = anglecombine;
                        }
                        dna = CountFitnessRectangle(dna);
                        return dna;
                  }
                  return null;
            }
            /// <summary>
            /// 选择
            /// </summary>
            private void Filter()
            {
                  /*CompareDNA comp = new CompareDNA();
                  _pop .Sort(comp);
                  for (int i = _pop .Count - 1; i >= _size; i--)
                  {
                        _pop .RemoveAt(i);
                  }*/

                  List<DNA> dnalist = new List<DNA>();
                  CompareDNA comp = new CompareDNA();
                  _pop .Sort(comp);
                  dnalist .Add(_pop[0] .Copy());
                  _pop .RemoveAt(0);

                  for (int i = 0; i < _size; i++)
                  {
                        DNA dna = SelectOne();
                        _pop .Remove(dna);
                        dnalist .Add(dna .Copy());
                  }
                  _pop = dnalist;
            }
            /// <summary>
            /// 适应度排序
            /// </summary>
            public class CompareDNA : IComparer<DNA>
            {
                  public int Compare(DNA dna1, DNA dna2)
                  {
                        if (dna2 .Fitness == dna1 .Fitness)
                        {
                              return dna1 .Stock[dna1 .Stock .Count - 1] .Use .CompareTo(dna2 .Stock[dna2 .Stock .Count - 1] .Use);
                        }
                        else
                        {
                              return dna2 .Fitness .CompareTo(dna1 .Fitness);// Compare(pt1.X, pt2.X);
                        }
                  }
            }
            /// <summary>
            /// 判断是否概率命中
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public bool Flip(decimal p)
            {
                  decimal p100 = p * 100;
                  Random r = new Random(GetRandomSeed());
                  int c = r .Next(0, 100);
                  if (c < p100)
                  {
                        return true;
                  }
                  return false;
            }
            /// <summary>
            /// 选择，适应度大的选中概率大
            /// </summary>
            /// <returns></returns>
            public DNA SelectOne()
            {
                  Random r = new Random(GetRandomSeed());
                  double pick = r .NextDouble();
                  float sumfitness = 0;
                  float sum = 0;
                  int index = 0;
                  for (int i = 0; i < _pop .Count; i++)
                  {
                        sumfitness += _pop[i] .Fitness;
                  }
                  for (int i = 0; i < _pop .Count; i++)
                  {
                        sum += _pop[i] .Fitness / sumfitness;
                        if (sum >= pick)
                        {
                              index = i;
                              break;
                        }
                  }
                  return _pop[index];
            }
            #endregion

            private void btntest2_Click(object sender, EventArgs e)
            {
                  Random r = new Random(GetRandomSeed());
                  int a1 = r .Next(0, 10);
                  int a2 = r .Next(0, 10);
                  int a3 = r .Next(0, 10);
                  int a4 = r .Next(0, 10);

                  txttestvalue .Text = a1 .ToString() + " " + a2 .ToString() + " " + a3 .ToString() + " " + a4 .ToString();
            }

            private void btndrawbest_Click(object sender, EventArgs e)
            {
                  DrawBest();
            }
            private void DrawBest()
            {
                  Stock best = _pop[0] .Stock[0];
                  _bg .Graphics .Clear(Color .White);
                  DrawStock(best);
                  using (SolidBrush b = new SolidBrush(Color .Green))
                  {
                        int[,] array = best .Disable;
                        for (int i = 0; i < array .GetLength(0); i++)
                        {
                              for (int j = 0; j < array .GetLength(1); j++)
                              {
                                    if (array[i, j] > 0)
                                    {
                                          _bg .Graphics .FillRectangle(b
                                          , new RectangleF(i * T * _rate
                                          , j * T * _rate
                                          , T * _rate
                                          , T * _rate));
                                    }
                              }
                        }
                  }
                  DrawStockLine(best);
                  _bg .Render();
            }

            private void btndecodetest_Click(object sender, EventArgs e)
            {

                  System .Diagnostics .Stopwatch watch = new System .Diagnostics .Stopwatch();
                  watch .Start();  //开始监视代码运行时间
                  _pop .Clear();
                  float fitness = 0f;
                  int c = _part .Count;
                  int sc = _stock .Count;
                  for (int i = 0; i < 1; i++)
                  {
                        List<Basic> basiclist = new List<Basic>();
                        int[] sort = new int[c];
                        sort = RandomSort(c);//随机顺序
                        for (int j = 0; j < sort .Length; j++)
                        {
                              Random r = new Random(GetRandomSeed());
                              int s = sort[j];
                              int id = _part[s] .id;
                              int angle = r .Next(0, 4) * 90;
                              int angleCombine = r .Next(0, 2) * 90;
                              float width = 0f;
                              float height = 0f;
                              //0;
                              // r .Next(0, 360);//随机旋转角度0-359
                              int[] baseGroup = new int[] { id, angle, angleCombine };

                              PlateHelper ph = new PlateHelper();
                              GridHelper gh = new GridHelper();
                              CopyOper co = new CopyOper();
                              string key = id .ToString() + "/" + angle .ToString();//旋转后加入基因库
                              if (!_basicLib .ContainsKey(key))
                              {
                                    PlateModel pm2 = new PlateModel();
                                    pm2 = ph .RotateAndMove(co .CopyPlate(_part[s]), Convert .ToSingle(angle));
                                    GridLib gl = gh .GetGridValue(pm2, T);
                                    _basicLib .Add(key, gl);
                              }
                              string key2 = "C" + id .ToString() + "/" + angleCombine;
                              if (!_basicLib .ContainsKey(key2))
                              {
                                    List<PlateCombine> pclist = _partCombine .Where(t => t .id == s) .ToList();
                                    if (pclist .Count > 0)
                                    {
                                          PlateCombine pc = pclist[0];
                                          GridLib gl = gh .GetGridValueCombine(ph .RotateAndMove(pc, Convert .ToSingle(angleCombine)), T);
                                          _basicLib .Add(key2, gl);
                                    }
                              }
                              basiclist .Add(new Basic(id, angle, angleCombine, _part[s] .Area, _part[s] .PlateCount));
                        }

                        List<Stock> dnastock = new List<Stock>();
                        int[] sortstock = new int[c];
                        sortstock = RandomSort(sc);
                        for (int k = 0; k < sortstock .Length; k++)
                        {
                              int s = sortstock[k];
                              dnastock .Add(_stock[s] .Copy());
                        }
                        DNA dnathis = new DNA(dnastock, basiclist);
                        dnathis = CountFitnessRectangle(dnathis);
                        _pop .Add(dnathis);
                        _best = _pop[0] .Stock[0];
                  }

                  DrawBest();
                  DrawStockLine(_pop[0] .Stock[0]);
                  watch .Stop();  //停止监视
                  TimeSpan timespan = watch .Elapsed;  //获取当前实例测量得出的总时间
                  lblinfo .Text = "耗时：" + timespan .TotalMilliseconds .ToString() + "ms" + ",利用率:" + _pop[0] .Fitness .ToString();
            }

            private void btnchongxinjiema_Click(object sender, EventArgs e)
            {
                  if (_pop .Count == 0)
                  {
                        MessageBox .Show("未配料");
                        return;
                  }
                  _pop .Sort(new CompareDNA());
                  DNA dna = _pop[0];
                  dna = CountFitnessRectangle(dna);

                  Stock best = dna .Stock[0];
                  _bg .Graphics .Clear(Color .White);
                  DrawStock(best);
                  DrawStockLine(best);
                  using (SolidBrush b = new SolidBrush(Color .Green))
                  {
                        int[,] array = best .Disable;
                        for (int i = 0; i < array .GetLength(0); i++)
                        {
                              for (int j = 0; j < array .GetLength(1); j++)
                              {
                                    if (array[i, j] > 0)
                                    {
                                          _bg .Graphics .FillRectangle(b
                                          , new RectangleF(i * T * _rate
                                          , j * T * _rate
                                          , T * _rate
                                          , T * _rate));
                                    }
                              }
                        }
                  }
                  lblinfo .Text = "利用率:" + _pop[0] .Fitness .ToString();
                  _bg .Render();
            }

            private void btncreate_Click(object sender, EventArgs e)
            {
                  System .Diagnostics .Stopwatch watch = new System .Diagnostics .Stopwatch();
                  watch .Start();  //开始监视代码运行时间
                  _pop .Clear();
                  _basicLib .Clear();
                  _size = Convert .ToInt32(txtSize .Text);
                  Create();
                  watch .Stop();  //停止监视
                  TimeSpan timespan = watch .Elapsed;  //获取当前实例测量得出的总时间
                  lblinfo .Text = "耗时：" + timespan .TotalMilliseconds .ToString() + "ms";
            }

            private void btndraw_Click(object sender, EventArgs e)
            {
                  cad .currentShapes .Clear();
                  cad .currentPlates .Clear();

                  //cad .currentPlates .Add(_part[0] .OutModel);
                  //for(int i = 0;i< _part[0] .InnerModel.Count;i++)
                  //{
                  //      cad .currentPlates .Add(_part[0] .InnerModel[i]);
                  //}
                  cad .currentShapes .AddRange(_part[0] .OutModel .ListShape);
                  for (int i = 0; i < _part[0] .InnerModel .Count; i++)
                  {
                        cad .currentShapes .AddRange(_part[0] .InnerModel[i] .ListShape);
                  }

                  cad .currentShapes .AddRange(_part[1] .OutModel .ListShape);
                  for (int i = 0; i < _part[1] .InnerModel .Count; i++)
                  {
                        cad .currentShapes .AddRange(_part[1] .InnerModel[i] .ListShape);
                  }
                  cad .DrawShap();
            }
            /// <summary>
            /// 画排料图，线图
            /// </summary>
            /// <param name="s"></param>
            private void DrawStockLine(Stock s)
            {
                  PlateHelper ph = new PlateHelper();
                  cad .currentShapes .Clear();
                  cad .currentPlates .Clear();
                  cad .DrawShap();

                  List<PartInfo> part = s .PartInfoList;
                  List<BaseShape> bs = new List<BaseShape>();
                  for (int i = 0; i < part .Count; i++)
                  {
                        int id = part[i] .ID;
                        string type = part[i] .Type;
                        float angle = part[i] .Angle;
                        PointF p = new PointF(part[i] .Location .Y * T, part[i] .Location .X * T);
                        //PointF p = new PointF(part[i] .Location .X * T, part[i] .Location .Y * T);
                        CopyOper co = new CopyOper();
                        if (type == "1")
                        {
                              PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                              pm = ph .RotateAndMove(pm, angle);
                              pm = ph .Move(pm, p .X, p .Y);
                              new RotateOper() .RotateCSYS(pm, s .Height);//旋转坐标系
                              cad .currentShapes .AddRange(pm .OutModel .ListShape);
                              cad .currentShapes .Add(new Text(pm .PlateName .ToString(), pm .PowCenter, 0, 20));
                              for (int j = 0; j < pm .InnerModel .Count; i++)
                              {
                                    cad .currentShapes .AddRange(pm .InnerModel[i] .ListShape);
                              }

                              string key = id .ToString() + "/" + angle .ToString();
                              var tp = _basicLib[key];
                              if (chkDrawRect .Checked)
                                    AddRectLine(ref bs, p, tp .GridArray .GetLength(0) * T, tp .GridArray .GetLength(1) * T);
                              if (chkGrid .Checked)
                                    AddGridLine(ref bs, tp .GridArray, p, T);

                        }
                        else
                        {
                              PlateCombine pc = _partCombine .Where(t => t .id == id) .ToList()[0];
                              pc = ph .RotateAndMove(pc, angle);
                              PlateModel pm1 = ph .Move(pc .Plate1, p .X, p .Y);
                              PlateModel pm2 = ph .Move(pc .Plate2, p .X, p .Y);

                              new RotateOper() .RotateCSYS(pm1, s .Height);
                              new RotateOper() .RotateCSYS(pm2, s .Height);
                              cad .currentShapes .AddRange(pm1 .OutModel .ListShape);
                              cad .currentShapes .Add(new Text(pm1 .PlateName .ToString(), pm1 .PowCenter, 0, 20));
                              for (int j = 0; j < pm1 .InnerModel .Count; i++)
                              {
                                    cad .currentShapes .AddRange(pm1 .InnerModel[i] .ListShape);
                              }

                              cad .currentShapes .AddRange(pm2 .OutModel .ListShape);
                              cad .currentShapes .Add(new Text(pm2 .PlateName .ToString(), pm2 .PowCenter, 0, 20));
                              for (int j = 0; j < pm2 .InnerModel .Count; i++)
                              {
                                    cad .currentShapes .AddRange(pm2 .InnerModel[i] .ListShape);
                              }

                              string key = "C" + id .ToString() + "/" + angle .ToString();
                              var tp = _basicLib[key];
                              if (chkDrawRect .Checked)
                                    AddRectLineCombine(ref bs, p, tp .GridArray .GetLength(0) * T, tp .GridArray .GetLength(1) * T);
                              if (chkGrid .Checked)
                                    AddGridLineCombine(ref bs, tp .GridArray, p, T);
                        }
                  }
                  Line Line1 = new Line(new PointF(-1, -1), new PointF(s .Height + 1, -1));
                  Line Line2 = new Line(new PointF(-1, -1), new PointF(-1, s .Width + 1));
                  Line Line3 = new Line(new PointF(s .Height + 1, -1), new PointF(s .Height + 1, s .Width + 1));
                  Line Line4 = new Line(new PointF(-1, s .Width + 1), new PointF(s .Height + 1, s .Width + 1));
                  Line1 .PenColor = Color .Red;
                  Line2 .PenColor = Color .Red;
                  Line3 .PenColor = Color .Red;
                  Line4 .PenColor = Color .Red;
                  bs .Add(Line1);
                  bs .Add(Line2);
                  bs .Add(Line3);
                  bs .Add(Line4);
                  new RotateOper() .RotateCSYS(bs, s .Height);
                  cad .currentShapes .AddRange(bs);
                  cad .DrawShap();
            }
            private void AddGridLine(ref List<BaseShape> bs, int[,] array, PointF p, float T)
            {
                  for (int i = 0; i < array .GetLength(0); i++)
                  {
                        for (int j = 0; j < array .GetLength(1); j++)
                        {
                              if (array[i, j] > 0)
                              {
                                    PointF pnew = new PointF(p .X + j * T, p .Y + i * T);
                                    AddRectLine(ref bs, pnew, T, T);
                              }
                        }
                  }
            }
            private void AddGridLineCombine(ref List<BaseShape> bs, int[,] array, PointF p, float T)
            {
                  for (int i = 0; i < array .GetLength(0); i++)
                  {
                        for (int j = 0; j < array .GetLength(1); j++)
                        {
                              if (array[i, j] > 0)
                              {
                                    PointF pnew = new PointF(p .X + j * T, p .Y + i * T);
                                    AddRectLineCombine(ref bs, pnew, T, T);
                              }
                        }
                  }
            }
            private void AddRectLine(ref List<BaseShape> bs, PointF p, float width, float heigth)
            {
                  Line Line1 = new Line(p, new PointF(p .X + heigth, p .Y));
                  Line Line2 = new Line(p, new PointF(p .X, p .Y + width));
                  Line Line3 = new Line(new PointF(p .X + heigth, p .Y), new PointF(p .X + heigth, p .Y + width));
                  Line Line4 = new Line(new PointF(p .X, p .Y + width), new PointF(p .X + heigth, p .Y + width));
                  Line1 .PenColor = Color .Green;
                  Line2 .PenColor = Color .Green;
                  Line3 .PenColor = Color .Green;
                  Line4 .PenColor = Color .Green;
                  bs .Add(Line1);
                  bs .Add(Line2);
                  bs .Add(Line3);
                  bs .Add(Line4);
            }
            private void AddRectLineCombine(ref List<BaseShape> bs, PointF p, float width, float heigth)
            {
                  Line Line1 = new Line(p, new PointF(p .X + heigth, p .Y));
                  Line Line2 = new Line(p, new PointF(p .X, p .Y + width));
                  Line Line3 = new Line(new PointF(p .X + heigth, p .Y), new PointF(p .X + heigth, p .Y + width));
                  Line Line4 = new Line(new PointF(p .X, p .Y + width), new PointF(p .X + heigth, p .Y + width));
                  Line1 .PenColor = Color .Yellow;
                  Line2 .PenColor = Color .Yellow;
                  Line3 .PenColor = Color .Yellow;
                  Line4 .PenColor = Color .Yellow;
                  bs .Add(Line1);
                  bs .Add(Line2);
                  bs .Add(Line3);
                  bs .Add(Line4);
            }
            /// <summary>
            /// 扩大测试
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnBig_Click(object sender, EventArgs e)
            {
                  cad .currentShapes .Clear();
                  PlateModel pm = _part[0];
                  CopyOper co = new CopyOper();
                  PlateModel pmnew = co .CopyPlate(pm);
                  //RectHelper rh = new RectHelper();
                  //List<PointF> pnew = rh .Expand(pmnew .OutModel .ListPoint, Convert .ToSingle(txtDis .Text));
                  //List<Line> line = new List<Line>();
                  //for (int i = 0; i < pnew .Count; i++)
                  //{
                  //      Line l = new Line(pnew[i], pnew[(i + 1) % pnew .Count]);
                  //      line .Add(l);
                  //}

                  cad .currentShapes .AddRange(pm .OutModel .ListShape);
                  cad .currentShapes .AddRange(pm .OutModel .ExpandShape);
                  cad .DrawShap();
            }
      }
}
