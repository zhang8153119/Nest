using System;
using System .Collections .Generic;
using System .Data;
using System .Windows .Forms;
using FZYK .Core;
using myCad .DXFOper;
using myCad .Model;
using FZYK .Nest .db;
using System .Windows .Forms .DataVisualization .Charting;
using System .Linq;
using System .Drawing;
using myCad .Shape;
using myCad .ShapeOper;
using System .Threading;

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
            int _popsize = 0;//种群数量
            decimal _pcross = 0;//交叉率
            decimal _pmutation = 0;//变异率

            List<PlateModel> _part = new List<PlateModel>();
            List<PlateCombine> _partCombine = new List<PlateCombine>();

            PlateHelper ph = new PlateHelper();
            GridHelper gh = new GridHelper();
            RectHelper rh = new RectHelper();
            NestDB _db = new NestDB();
            GA _ga = new GA();

            DataTable _dtPart = new DataTable();
            private Queue<double> _dataQueue = new Queue<double>(100);
            private Queue<double> _dataQueueAvg = new Queue<double>(100);
            bool _stop = false;

            List<DNA> _currentPop = new List<DNA>();
            #endregion
            #region 加载
            /// <summary>
            /// 加载
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void FrmNest_Shown(object sender, EventArgs e)
            {
                  dgvPart .AutoGenerateColumns = false;
                  dgvStock .AutoGenerateColumns = false;
                  BindSet();
                  InitChart();
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
                  _popsize = Convert .ToInt32(dic["Pop"]);
                  _pcross = Convert .ToDecimal(dic["Cross"]);
                  _pmutation = Convert .ToDecimal(dic["Mutation"]);

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
            /// <summary>
            /// 读取DXF文件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsmiReadDxf_Click(object sender, EventArgs e)
            {
                  DxfInputB di = new DxfInputB();
                  List<PlateModel> plate = new List<PlateModel>();
                  plate = di .mainmethod(cadInterfaceMain);

                  _dtPart = DevCommon .DgvToTableEmpty(dgvPart);

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

                        DataRow dr = _dtPart .NewRow();
                        dr["id"] = pm .id;
                        dr["pName"] = pm .PlateName;
                        dr["rCount"] = pm .PlateCount;
                        dr["pCount"] = pm .PlateCount;
                        dr["pLength"] = Convert .ToInt32(Math .Max(pm .Rect .Width, pm .Rect .Height));
                        dr["pWidth"] = Convert .ToInt32(Math .Min(pm .Rect .Width, pm .Rect .Height));
                        dr["mfSpec"] = "-10";
                        dr["lName"] = "Q345B";
                        _dtPart .Rows .Add(dr);
                  }

                  dgvPart .DataSource = _dtPart;
                  BindlNamemfSpec();
            }
            /// <summary>
            /// 清除
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsmiDelete_Click(object sender, EventArgs e)
            {
                  _dtPart .Clear();
                  cadInterfaceMain .currentShapes .Clear();
                  cadInterfaceMain .currentPlates .Clear();
                  cadInterfaceMain .DrawShap();
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
            /// <summary>
            /// 绑定材质规格列表
            /// </summary>
            private void BindlNamemfSpec()
            {
                  List<string> strlist = new List<string>();
                  for (int i = 0; i < _dtPart .Rows .Count; i++)
                  {
                        string str = _dtPart .Rows[i]["lName"] .ToString() + "  " + _dtPart .Rows[i]["mfSpec"] .ToString();
                        if (!strlist .Contains(str))
                              strlist .Add(str);
                  }
                  cmblName_mfSpec .DataSource = strlist;
            }
            /// <summary>
            /// 按材质规格提取件号和库存
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnGet_Click(object sender, EventArgs e)
            {
                  if (cmblName_mfSpec .Text == "")
                        return;
                  string lname_mfspec = cmblName_mfSpec .Text;
                  string lname = lname_mfspec .Split(new string[] { "  " }, StringSplitOptions .None)[0];
                  string mfspec = lname_mfspec .Split(new string[] { "  " }, StringSplitOptions .None)[1];

                  DataView dv = new DataView(_dtPart);
                  dv .RowFilter = "lName = '" + lname + "' AND mfSpec = '" + mfspec + "'";
                  dgvPart .DataSource = dv .ToTable();

                  DataTable dtstock = new DataTable();
                  dtstock = _db .GetStock(lname, mfspec);
                  dgvStock .DataSource = dtstock;
            }
            #endregion
            #region 遗传算法过程
            /// <summary>
            /// 开始
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsbtnStart_Click(object sender, EventArgs e)
            {
                  _stop = false;
                  _dataQueue .Clear();
                  _dataQueueAvg .Clear();
                  this .chartMain .Series[0] .Points .Clear();
                  this .chartMain .Series[1] .Points .Clear();
                  Thread t = new Thread(DoWork);
                  t .Start();
            }
            /// <summary>
            /// 监控状态
            /// </summary>
            /// <param name="gen"></param>
            /// <param name="maxfitness"></param>
            /// <param name="avgfitness"></param>
            private void GeneProgress(string gen, string maxfitness, string avgfitness)
            {
                  MethodInvoker progressInvoker = () =>
                  {
                        txtResult .AppendText("[" + gen .ToString() + "]：" + maxfitness .ToString() + "\r\n");
                        txtResult .Focus();
                        txtResult .ScrollToCaret();
                        UpdateChart(Convert .ToDouble(maxfitness), Convert .ToDouble(avgfitness));
                  };
                  if (txtResult .InvokeRequired)
                  {
                        txtResult .Invoke(progressInvoker);
                  }
                  else if (chartMain .InvokeRequired)
                  {
                        chartMain .Invoke(progressInvoker);
                  }
                  else
                  {
                        progressInvoker();
                  }
            }
            /// <summary>
            /// 更新图
            /// </summary>
            /// <param name="value"></param>
            /// <param name="avgvalue"></param>
            private void UpdateChart(double value, double avgvalue)
            {
                  _dataQueue .Enqueue(value);
                  _dataQueueAvg .Enqueue(avgvalue);
                  this .chartMain .Series[0] .Points .Clear();
                  this .chartMain .Series[1] .Points .Clear();
                  for (int i = 0; i < _dataQueue .Count; i++)
                  {
                        this .chartMain .Series[0] .Points .AddXY((i + 1), _dataQueue .ElementAt(i));
                  }
                  this .chartMain .Series[1] .Points .Clear();
                  for (int i = 0; i < _dataQueueAvg .Count; i++)
                  {
                        this .chartMain .Series[1] .Points .AddXY((i + 1), _dataQueueAvg .ElementAt(i));
                  }
            }
            /// <summary>
            /// 显示计算进程
            /// </summary>
            /// <param name="current"></param>
            /// <param name="total"></param>
            private void ShowProgress(string current, string total)
            {
                  MethodInvoker progressInvoker = () =>
                  {
                        pbMain .Value = Convert .ToInt32(current) / Convert .ToInt32(total) * 100;
                  };
                  if (pbMain .InvokeRequired)
                  {
                        pbMain .Invoke(progressInvoker);
                  }
                  else
                  {
                        progressInvoker();
                  }
            }
            /// <summary>
            /// 计算线程
            /// </summary>
            private void DoWork()
            {
                  Go(GeneProgress, ShowProgress);
            }
            /// <summary>
            /// 计算
            /// </summary>
            /// <param name="GeneProgress"></param>
            /// <param name="ShowProgress"></param>
            private void Go(Action<string, string, string> GeneProgress, Action<string, string> ShowProgress)
            {
                  List<Stock> s = new List<Stock>();
                  List<DNA> pop = new List<DNA>();
                  s = CreateStock();
                  if (s == null)
                        return;
                  _ga = new GA(_part, _partCombine, s, T, _popsize, _rotate, _type, _popsize, _pcross, _pmutation);
                  pop = _ga .Create();
                  int gen = 1;
                  while (!_stop)
                  {
                        List<DNA> newpop = new List<DNA>();
                        //精英保留
                        _ga .SortByFitness(ref pop);
                        DNA bestdna = pop[0] .Copy();

                        for (int i = 0; i < _popsize; i += 2)
                        {
                              DNA dna1 = _ga .SelectOne(pop);
                              DNA dna2 = _ga .SelectOne(pop);

                              DNA child1 = new DNA();
                              DNA child2 = new DNA();

                              Tuple<DNA, DNA> tp = _ga .Crossover(dna1, dna2);
                              child1 = tp .Item1;
                              child2 = tp .Item2;

                              DNA dnamutation1 = _ga .Mutation(child1);
                              if (dnamutation1 != null)
                              {
                                    newpop .Add(dnamutation1);
                              }
                              else
                              {
                                    newpop .Add(child1);
                              }

                              DNA dnamutation2 = _ga .Mutation(child2);
                              if (dnamutation2 != null)
                              {
                                    newpop .Add(dnamutation2);
                              }
                              else
                              {
                                    newpop .Add(child2);
                              }

                              ShowProgress?.Invoke((i + 2) .ToString(), _popsize .ToString());
                        }

                        _ga .SortByFitness(ref newpop);
                        newpop .RemoveAt(newpop .Count - 1);
                        newpop .Insert(0, bestdna);
                        pop = newpop;
                        //Filter();
                        List<DNA> currentpop = new List<DNA>();
                        float maxfitness = 0;
                        float avgfitness = 0;
                        float sumfitness = 0;
                        int bestindex = 0;
                        for (int i = 0; i < pop .Count; i++)
                        {
                              currentpop .Add(pop[i] .Copy());
                              sumfitness += pop[i] .Fitness;
                              if (pop[i] .Fitness > maxfitness)
                              {
                                    maxfitness = pop[i] .Fitness;
                                    bestindex = i;
                              }
                        }
                        avgfitness = sumfitness / pop .Count;
                        GeneProgress?.Invoke(gen .ToString(), maxfitness .ToString(), avgfitness .ToString());

                        _currentPop = currentpop;
                        gen++;
                  }
            }
            /// <summary>
            /// 停止
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsbtnStop_Click(object sender, EventArgs e)
            {
                  _stop = true;
            }

            /// <summary>
            /// 提取库存
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private List<Stock> CreateStock()
            {
                  List<Stock> s = new List<Stock>();
                  if (dgvStock .SelectedRows .Count <= 0)
                  {
                        MessageBox .Show("请选择库存");
                        return null;
                  }
                  for (int i = 0; i < dgvStock .SelectedRows .Count; i++)
                  {
                        if (!dgvStock .SelectedRows[i] .IsNewRow)
                        {
                              float slength = Convert .ToSingle(dgvStock .SelectedRows[i] .Cells["sLength"] .Value .ToString());
                              float swidth = Convert .ToSingle(dgvStock .SelectedRows[i] .Cells["sWidth"] .Value .ToString());
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
                              s .Add(stock);
                        }
                  }
                  return s;
            }
            #endregion
            #region 分析图
            /// <summary>
            /// 初始化图表
            /// </summary>
            private void InitChart()
            {
                  //定义图表区域
                  this .chartMain .ChartAreas .Clear();
                  ChartArea chartArea1 = new ChartArea("C1");
                  this .chartMain .ChartAreas .Add(chartArea1);
                  //定义存储和显示点的容器
                  this .chartMain .Series .Clear();
                  Series series1 = new Series("S1");
                  Series series2 = new Series("S2");
                  series1 .ChartArea = "C1";
                  series2 .ChartArea = "C1";
                  this .chartMain .Series .Add(series1);
                  this .chartMain .Series .Add(series2);
                  //设置图表显示样式
                  //this .chart1 .ChartAreas[0] .AxisY .Minimum = 0;
                  this .chartMain .ChartAreas[0] .AxisY .Maximum = 100;
                  this .chartMain .ChartAreas[0] .AxisX .Interval = 5;
                  this .chartMain .ChartAreas[0] .AxisX .MajorGrid .LineColor = System .Drawing .Color .Silver;
                  this .chartMain .ChartAreas[0] .AxisY .MajorGrid .LineColor = System .Drawing .Color .Silver;
                  this .chartMain .ChartAreas[0] .AxisY .IsStartedFromZero = false;

                  this .chartMain .ChartAreas[0] .AxisX .MajorGrid .Enabled = false;
                  this .chartMain .ChartAreas[0] .AxisX .MajorTickMark .Enabled = false;
                  this .chartMain .ChartAreas[0] .AxisX .IsMarginVisible = false;
                  this .chartMain .ChartAreas[0] .AxisX .LabelStyle .Enabled = false;
                  //设置标题
                  this .chartMain .Titles .Clear();
                  this .chartMain .Titles .Add("S01");
                  //this .chart1 .Titles[0] .Text = "XXX显示";
                  this .chartMain .Titles[0] .ForeColor = Color .RoyalBlue;
                  this .chartMain .Titles[0] .Font = new System .Drawing .Font("Microsoft Sans Serif", 12F);
                  //设置图表显示样式
                  this .chartMain .Series[0] .Color = Color .Red;
                  this .chartMain .Series[1] .Color = Color .Blue;

                  this .chartMain .Titles[0] .Text = "";
                  this .chartMain .Series[0] .ChartType = SeriesChartType .Line;
                  this .chartMain .Series[0] .Points .Clear();
                  this .chartMain .Series[1] .ChartType = SeriesChartType .Line;
                  this .chartMain .Series[1] .Points .Clear();
            }

            #endregion
            #region 查看结果 
            /// <summary>
            /// 查看
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnSee_Click(object sender, EventArgs e)
            {
                  _ga .SortByFitness(ref _currentPop);
                  /*DNA dna = _ga .CountFitnessRectangle(_currentPop[0]);
                  Stock best = dna .Stock[0];*/
                  DrawStockLine(_currentPop[0].Stock[0]);
            }

            private void btnSee2_Click(object sender, EventArgs e)
            {
                  _ga .SortByFitness(ref _currentPop);
                  DNA dna = _ga .CountFitnessRectangle(_currentPop[0]);
                  DrawStockLine(dna .Stock[0]);
            }
            /// <summary>
            /// 画排料图，线图
            /// </summary>
            /// <param name="s"></param>
            private void DrawStockLine(Stock s)
            {
                  PlateHelper ph = new PlateHelper();
                  cadInterfaceMain .currentShapes .Clear();
                  cadInterfaceMain .currentPlates .Clear();
                  cadInterfaceMain .DrawShap();

                  List<PartInfo> part = s .PartInfoList;
                  List<BaseShape> bs = new List<BaseShape>();
                  for (int i = 0; i < part .Count; i++)
                  {
                        int id = part[i] .ID;
                        string type = part[i] .Type;
                        float angle = part[i] .Angle;
                        PointF p = new PointF(part[i] .Location .Y * T, part[i] .Location .X * T);
                        if (type == "1")
                        {
                              PlateModel pm = _ga ._part .Where(t => t .id == id) .ToList()[0];
                              pm = ph .RotateAndMove(pm, angle);
                              pm = ph .Move(pm, p .X, p .Y);
                              new RotateOper() .RotateCSYS(pm, s .Height);//旋转坐标系
                              cadInterfaceMain .currentShapes .AddRange(pm .OutModel .ListShape);
                              cadInterfaceMain .currentShapes .Add(new Text(pm .PlateName .ToString(), pm .PowCenter, 0, 30));
                              for (int j = 0; j < pm .InnerModel .Count; i++)
                              {
                                    cadInterfaceMain .currentShapes .AddRange(pm .InnerModel[i] .ListShape);
                              }

                              string key = id .ToString() + "/" + angle .ToString();
                              //var tp = _ga ._basicLib[key];
                              //AddRectLine(ref bs, p, tp .GridArray .GetLength(0) * T, tp .GridArray .GetLength(1) * T);
                              /*if (chkGrid .Checked)
                                    AddGridLine(ref bs, tp .GridArray, p, T);*/

                        }
                        else
                        {
                              PlateCombine pc = _ga ._partCombine .Where(t => t .id == id) .ToList()[0];
                              pc = ph .RotateAndMove(pc, angle);
                              PlateModel pm1 = ph .Move(pc .Plate1, p .X, p .Y);
                              PlateModel pm2 = ph .Move(pc .Plate2, p .X, p .Y);

                              new RotateOper() .RotateCSYS(pm1, s .Height);
                              new RotateOper() .RotateCSYS(pm2, s .Height);
                              cadInterfaceMain .currentShapes .AddRange(pm1 .OutModel .ListShape);
                              cadInterfaceMain .currentShapes .Add(new Text(pm1 .PlateName .ToString(), pm1 .PowCenter, 0, 30));
                              for (int j = 0; j < pm1 .InnerModel .Count; i++)
                              {
                                    cadInterfaceMain .currentShapes .AddRange(pm1 .InnerModel[i] .ListShape);
                              }

                              cadInterfaceMain .currentShapes .AddRange(pm2 .OutModel .ListShape);
                              cadInterfaceMain .currentShapes .Add(new Text(pm2 .PlateName .ToString(), pm2 .PowCenter, 0, 30));
                              for (int j = 0; j < pm2 .InnerModel .Count; i++)
                              {
                                    cadInterfaceMain .currentShapes .AddRange(pm2 .InnerModel[i] .ListShape);
                              }

                              string key = "C" + id .ToString() + "/" + angle .ToString();
                              //var tp = _ga ._basicLib[key];
                              //AddRectLineCombine(ref bs, p, tp .GridArray .GetLength(0) * T, tp .GridArray .GetLength(1) * T);
                              /*if (chkGrid .Checked)
                                    AddGridLineCombine(ref bs, tp .GridArray, p, T);*/
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
                  cadInterfaceMain .currentShapes .AddRange(bs);
                  cadInterfaceMain .DrawShap();
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
            #endregion
            #region 测试
            private void btntest_Click(object sender, EventArgs e)
            {
                  List<Stock> s = new List<Stock>();
                  List<DNA> pop = new List<DNA>();
                  s = CreateStock();
                  if (s == null)
                        return;
                  _ga = new GA(_part, _partCombine, s, T, _popsize, _rotate, _type, _popsize, _pcross, _pmutation);
                  pop = _ga .Create();
                  _currentPop = pop;
                  DrawStockLine(_currentPop[0] .Stock[0]);

            }
            #endregion

      }
}
