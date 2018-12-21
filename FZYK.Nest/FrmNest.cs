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
using GPU;
using Cloo;
using System .IO;
using System .Text;
using netDxf;

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
            float _smallsize = 0;//小件长宽
            float _protect = 0;//保护量
            bool _rotate = true;//非组合零件交替旋转排入
            string _type = "";//组合模型
            int _popsize = 0;//种群数量
            decimal _pcross = 0;//交叉率
            decimal _pmutation = 0;//变异率
            string _path = "";//结果保存路径
            bool _matchwidth = true;
            float _matchrate = 0.9f;
            string _algorithm = "算法一";
            bool _smallfill = false;
            int _oldpageindex = -1;
            List<int> _anglelist = new List<int>();//可旋转的角度

            Dictionary<string, GridLib> _basicLib = new Dictionary<string, GridLib>();//基因库

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
            List<DNA> _result = new List<DNA>();
            DNA _currentdna = new DNA();
            int _currentid = 1;
            int _maxcount = 0;

            DataTable _dtresult = new DataTable();
            DataTable _dtresultdetail = new DataTable();
            string _currentetname = "";
            string _currentlname = "";
            string _currentmfspec = "";

            private IGpuHelper gpu;
            private ComputeProgram gpuprogram;
            // 声明INI文件的写操作函数 WritePrivateProfileString()  
            [System .Runtime .InteropServices .DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            // 声明INI文件的读操作函数 GetPrivateProfileString()  
            [System .Runtime .InteropServices .DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, System .Text .StringBuilder retVal, int size, string filePath);

            #endregion
            #region 加载
            /// <summary>
            /// 加载
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void FrmNest_Shown(object sender, EventArgs e)
            {
                  if (!bwopencl .IsBusy)
                  {
                        CanOp(false);
                        bwopencl .RunWorkerAsync();
                  }
                  bCanAdd = bCanAudit = bCanDelete = bCanSave = bCanUnAudit = bCanPrint = tsbtnTools .Visible = false;
                  dgvPart .AutoGenerateColumns = false;
                  dgvStock .AutoGenerateColumns = false;
                  dgvr1 .AutoGenerateColumns = false;
                  dgvr2 .AutoGenerateColumns = false;
                  GetConfig();
                  Readme();
                  /*BindSet();*/
                  InitChart();
                  BindlNamemfSpec();
                  cadInterfaceMain .Init();
            }
            /// <summary>
            /// 绑定设置
            /// </summary>
            private void BindSet()
            {
                  /* Dictionary<string, string> dic = new Dictionary<string, string>();
                   dic = _db .GetSet();
                   T = Convert .ToSingle(dic["T"]);
                   _smallsize = Convert .ToSingle(dic["Size"]);
                   _protect = Convert .ToSingle(dic["Protect"]);
                   _rotate = dic["Rotate"] .ToString() .Equals("是");
                   _popsize = Convert .ToInt32(dic["Pop"]);
                   _pcross = Convert .ToDecimal(dic["Cross"]);
                   _pmutation = Convert .ToDecimal(dic["Mutation"]);
                   _path = Getini("NestConfig", "SavePath");
                   string type = dic["Type"] .ToString();
                   if (type == "矩形")
                         _type = "rect";
                   else if (type == "平行四边形")
                         _type = "para";
                   else if (type == "混合")
                         _type = "mix";
                   else
                         _type = "";*/
            }

            private void CanOp(bool can)
            {
                  tsMain .Enabled = can;
                  btnGet .Enabled = can;
                  btngo .Enabled = can;
                  btnsavefile .Enabled = can;
            }
            #endregion
            #region 清空
            private void ClearAll()
            {
                  _part .Clear();
                  _partCombine .Clear();
                  _basicLib .Clear();
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
                  ClearAll();
                  DxfInputB di = new DxfInputB();
                  List<PlateModel> plate = new List<PlateModel>();
                  plate = di .mainmethod(cadInterfaceMain);
                  if (plate .Count == 0)
                  {
                        return;
                  }
                  _dtPart = DevCommon .DgvToTableEmpty(dgvPart);

                  for (int i = 0; i < plate .Count; i++)
                  {
                        PlateModel pm = ph .GetMinPlate(rh .Expand(plate[i], _protect));
                        pm .id = i;
                        _part .Add(pm);

                        DataRow dr = _dtPart .NewRow();
                        dr["id"] = pm .id;
                        dr["pName"] = pm .PlateName;
                        dr["rCount"] = pm .PlateCount;
                        dr["pCount"] = pm .PlateCount;
                        dr["pLength"] = Convert .ToInt32(Math .Max(pm .Rect .Width, pm .Rect .Height));
                        dr["pWidth"] = Convert .ToInt32(Math .Min(pm .Rect .Width, pm .Rect .Height));
                        dr["mfSpec"] = "";
                        dr["lName"] = "";
                        _dtPart .Rows .Add(dr);
                  }

                  dgvPart .DataSource = _dtPart;
                  //CreateLib(_type);
                  if (!bwread .IsBusy)
                  {
                        CanOp(false);
                        bwread .RunWorkerAsync(_type);
                  }
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
                  DataSet dtlist = _db .GetStockList();
                  List<string> strlist = new List<string>();
                  cmbetname .DataSource = dtlist .Tables[0];
                  cmbetname .DisplayMember = "etName";
                  cmbetname .ValueMember = "etName";

                  cmblname .DataSource = dtlist .Tables[1];
                  cmblname .DisplayMember = "lName";
                  cmblname .ValueMember = "lName";

                  cmbmfspec .DataSource = dtlist .Tables[2];
                  cmbmfspec .DisplayMember = "mfSpec";
                  cmbmfspec .ValueMember = "mfSpec";
            }
            /// <summary>
            /// 按材质规格提取件号和库存
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnGet_Click(object sender, EventArgs e)
            {
                  _currentetname = cmbetname .Text;
                  _currentlname = cmblname .Text;
                  _currentmfspec = cmbmfspec .Text;

                  DataTable dtstock = new DataTable();
                  dtstock = _db .GetStock(_currentetname, _currentlname, _currentmfspec);
                  dgvStock .DataSource = dtstock;
            }
            #endregion
            #region 创建库
            /// <summary>
            /// 重新转换数据
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tsbtnchange_Click(object sender, EventArgs e)
            {
                  if (!bwread .IsBusy)
                  {
                        CanOp(false);
                        List<PlateModel> partnew = new List<PlateModel>();
                        for (int i = 0; i < _part .Count; i++)
                        {
                              PlateModel pm = ph .GetMinPlate(rh .Expand(_part[i], _protect));
                              pm .id = i;
                              partnew .Add(pm);
                        }
                        _part = partnew;
                        bwread .RunWorkerAsync(_type);
                  }
            }
            /// <summary>
            /// 创建库
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void bwread_DoWork(object sender, System .ComponentModel .DoWorkEventArgs e)
            {
                  _basicLib .Clear();
                  _partCombine .Clear();
                  List<object> list_0 = new List<object>();
                  list_0 .Add(0);
                  bwread .ReportProgress(0, list_0);
                  string type = e .Argument .ToString();
                  for (int n = 0; n < _part .Count; n++)
                  {
                        GridHelper gh = new GridHelper();
                        PlateHelper ph = new PlateHelper();
                        PlateModel pm = _part[n];
                        int id = pm .id;
                        pm .Kind1 = _anglelist .Count;
                        for (int i = 0; i < _anglelist .Count; i++)
                        {
                              string key = id .ToString() + "/" + _anglelist[i] .ToString();//id/角度（角度范围在全局变量anglelist）
                              GridLib gl = new GridLib();
                              PlateModel pm1 = ph .RotateAndMove(pm, _anglelist[i]);
                              gl = gpu .GetGridValue(pm1, T, gpuprogram);
                              gl .Width = pm1 .Rect .Width;
                              gl .Height = pm1 .Rect .Height;
                              //gl = gh .GetGridValue(pm1, T);
                              _basicLib .Add(key, gl);
                        }
                        if (pm .PlateCount > 1)
                        {
                              List<PlateCombine> pc = new List<PlateCombine>();
                              pc = ph .GetMinPlateCombineAll(pm, T, type, -1, false);
                              pm .Kind2 = pc .Count;
                              for (int i = 0; i < pc .Count; i++)
                              {
                                    string key = "C" + id .ToString() + "/" + i .ToString();//Cid/索引（索引范围在_part[i].Kind2)
                                    pc[i] .key = key;
                                    _partCombine .Add(pc[i]);
                                    GridLib gl = new GridLib();
                                    gl = gpu .GetGridValueCombine(pc[i], T, gpuprogram);
                                    gl .Width = pc[i] .Rect .Width;
                                    gl .Height = pc[i] .Rect .Height;
                                    _basicLib .Add(key, gl);
                              }
                        }
                        List<object> list_1 = new List<object>();
                        list_1 .Add(100 * n / (_part .Count - 1));
                        bwread .ReportProgress(0, list_1);
                  }
                  List<object> list_2 = new List<object>();
                  list_2 .Add(100);
                  bwread .ReportProgress(0, list_2);
            }

            private void bwread_ProgressChanged(object sender, System .ComponentModel .ProgressChangedEventArgs e)
            {
                  List<object> objlist = (List<object>)e .UserState;
                  string pro = objlist[0] .ToString();
                  if (pro == "100")
                  {
                        lblinfo .Text = "数据转换完成";
                  }
                  else
                  {
                        lblinfo .Text = "数据转换中..." + pro + "%";
                  }
            }

            private void bwread_RunWorkerCompleted(object sender, System .ComponentModel .RunWorkerCompletedEventArgs e)
            {
                  CanOp(true);
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
                        lblinfo .Text = "计算中..." + Convert .ToInt32(current) / Convert .ToInt32(total) * 100 + "%";
                  };
                  if (lblinfo .InvokeRequired)
                  {
                        lblinfo .Invoke(progressInvoker);
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
                  _ga = new GA(_part, _partCombine, s, T, _popsize, _rotate, _type, _smallsize, _pcross, _pmutation);
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
            #region 过程监控图
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
                  DrawStockLine(_currentPop[0] .Stock[0]);
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
                              PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                              pm = ph .RotateAndMove(pm, angle);
                              pm = ph .Move(pm, p .X, p .Y);
                              new RotateOper() .RotateCSYS(pm, s .Height);//旋转坐标系
                              cadInterfaceMain .currentShapes .AddRange(pm .OutModel .ListShape);
                              cadInterfaceMain .currentShapes .Add(new Text(pm .PlateName .ToString(), pm .PowCenter, 0, 10));
                              for (int j = 0; j < pm .InnerModel .Count; i++)
                              {
                                    cadInterfaceMain .currentShapes .AddRange(pm .InnerModel[i] .ListShape);
                              }

                              string key = id .ToString() + "/" + angle .ToString();
                              var tp = _basicLib[key];
                              if (chkDrawRect .Checked)
                                    AddRectLine(ref bs, p, tp .GridArray .GetLength(0) * T, tp .GridArray .GetLength(1) * T);
                              /*if (chkGrid .Checked)
                                    AddGridLine(ref bs, tp .GridArray, p, T);*/

                        }
                        else
                        {
                              string key = "C" + part[i] .ID .ToString() + "/" + part[i] .combineType .ToString();
                              PlateCombine pc = _partCombine .Where(t => t .key == key) .ToList()[0];
                              pc = ph .RotateAndMove(pc, angle);
                              PlateModel pm1 = ph .Move(pc .Plate1, p .X, p .Y);
                              PlateModel pm2 = ph .Move(pc .Plate2, p .X, p .Y);

                              new RotateOper() .RotateCSYS(pm1, s .Height);
                              new RotateOper() .RotateCSYS(pm2, s .Height);
                              cadInterfaceMain .currentShapes .AddRange(pm1 .OutModel .ListShape);
                              cadInterfaceMain .currentShapes .Add(new Text(pm1 .PlateName .ToString(), pm1 .PowCenter, 0, 10));
                              for (int j = 0; j < pm1 .InnerModel .Count; i++)
                              {
                                    cadInterfaceMain .currentShapes .AddRange(pm1 .InnerModel[i] .ListShape);
                              }

                              cadInterfaceMain .currentShapes .AddRange(pm2 .OutModel .ListShape);
                              cadInterfaceMain .currentShapes .Add(new Text(pm2 .PlateName .ToString(), pm2 .PowCenter, 0, 10));
                              for (int j = 0; j < pm2 .InnerModel .Count; i++)
                              {
                                    cadInterfaceMain .currentShapes .AddRange(pm2 .InnerModel[i] .ListShape);
                              }

                              var tp = _basicLib[key];
                              if (chkDrawRect .Checked)
                                    AddRectLineCombine(ref bs, p, tp .GridArray .GetLength(0) * T, tp .GridArray .GetLength(1) * T);
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
                  _ga = new GA(_part, _partCombine, s, T, _popsize, _rotate, _type, _smallsize, _pcross, _pmutation);
                  pop = _ga .Create();
                  _currentPop = pop;
                  DrawStockLine(_currentPop[0] .Stock[0]);

            }
            #endregion
            #region opencl编译

            private void bwopencl_DoWork(object sender, System .ComponentModel .DoWorkEventArgs e)
            {
                  try
                  {
                        IniHelper ih = new IniHelper();
                        string config = "";
                        if (ih .ExistINIFile())
                        {
                              config = ih .IniReadValue("GPUConfig", "Platform");
                        }
                        if (config == "")
                        {
                              OpenClSetting oclSetting = new OpenClSetting();
                              if (oclSetting .ShowDialog(this) == DialogResult .OK)
                              {
                                    gpu = oclSetting .Gpu;
                              }
                        }
                        else
                        {
                              string platformstr = ih .IniReadValue("GPUConfig", "Platform");
                              string devicestr = ih .IniReadValue("GPUConfig", "Device");
                              string fpTypestr = ih .IniReadValue("GPUConfig", "FpType");

                              FPType fpType;
                              if (fpTypestr == "Double Precision (AMD)")
                              {
                                    fpType = FPType .FP64AMD;
                              }
                              else if (fpTypestr == "Double Precision")
                              {
                                    fpType = FPType .FP64;
                              }
                              else
                              {
                                    fpType = FPType .Single;
                              }

                              ComputePlatform platform = null;
                              var platforms = ComputePlatform .Platforms;
                              foreach (var p in platforms)
                              {
                                    if (p .Name == platformstr)
                                    {
                                          platform = p;
                                          break;
                                    }
                              }
                              //ComputePlatform platform = platforms .TakeWhile(t => t .Name .Equals(platformstr)) .ToList()[0];
                              ComputeDevice device = platform .Devices .Where(t => t .Name == devicestr) .ToList()[0];
                              gpu = GpuHelperFactory .CreateHelper(platform
                                    , device
                                    , fpType);
                        }

                        gpuprogram = gpu .Build();
                        e .Result = "ok";
                  }
                  catch (Exception ex)
                  {
                        e .Result = "error";
                  }
            }
            private void bwopencl_RunWorkerCompleted(object sender, System .ComponentModel .RunWorkerCompletedEventArgs e)
            {
                  string info = e .Result .ToString();
                  if (info == "error")
                  {
                        lblbystate .Text = "初始化失败";
                        string currentpath = Directory .GetCurrentDirectory();
                        string path = currentpath + "\\GPUConfig.ini";
                        WritePrivateProfileString("GPUConfig", "Platform", "", path);
                        WritePrivateProfileString("GPUConfig", "Device", "", path);
                        WritePrivateProfileString("GPUConfig", "FpType", "", path);
                  }
                  else
                  {
                        lblbystate .Text = "";
                        CanOp(true);
                  }
            }
            #endregion
            #region 套料
            /// <summary>
            /// 一次套料，不采用遗传算法
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btngo_Click(object sender, EventArgs e)
            {
                  Dictionary<string, object> param = new Dictionary<string, object>();
                  DNA dna = new DNA();
                  List<Stock> s = new List<Stock>();
                  s = CreateStock();
                  if (s != null)
                  {
                        param .Add("stock", s[0]);
                        param .Add("matchwidth", _matchwidth);
                        param .Add("algorithm", _algorithm);
                        param .Add("matchrate", _matchrate);

                        CreateOne(param);
                  }
            }
            /// <summary>
            /// 生成一个解，从大到小
            /// </summary>
            private void CreateOne(Dictionary<string, object> param)
            {
                  Stock s = (Stock)param["stock"];
                  bool matchwidth = Convert .ToBoolean(param["matchwidth"]);
                  string algorithm = param["algorithm"] .ToString();
                  float matchrate = Convert .ToSingle(param["matchrate"] .ToString());
                  int c = _part .Count;
                  _part .Sort(new CompareArea());

                  List<Basic> basiclist = new List<Basic>();
                  for (int i = 0; i < _part .Count; i++)
                  {
                        int id = _part[i] .id;
                        int angle = 0;
                        int angleCombine = 0;
                        int combinetype = 0;

                        basiclist .Add(new Basic(id, angle, angleCombine, _part[i] .Area, _part[i] .PlateCount, combinetype));
                  }

                  List<Stock> stocklist = new List<Stock>();
                  stocklist .Add(s);
                  DNA dnathis = new DNA(stocklist, basiclist);
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("dna", dnathis);
                  dic .Add("matchwidth", matchwidth);
                  dic .Add("algorithm", algorithm);
                  dic .Add("matchrate", matchrate);
                  if (!bwGo .IsBusy)
                  {
                        CanOp(false);
                        bwGo .RunWorkerAsync(dic);
                  }
            }
            /// <summary>
            /// 任务结束
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void bwGo_RunWorkerCompleted(object sender, System .ComponentModel .RunWorkerCompletedEventArgs e)
            {
                  DNA dna = (DNA)e .Result;
                  _currentdna = dna .Copy();
                  _maxcount = CountMin(dna);
                  DrawStockLine(dna .Stock[0]);
                  lblinfo .Text = "利用率：" + dna .Fitness + "%" + "   ，可排" + _maxcount .ToString() + "张";
                  CanOp(true);
            }
            /// <summary>
            /// 进度反馈
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void bwGo_ProgressChanged(object sender, System .ComponentModel .ProgressChangedEventArgs e)
            {
                  List<object> objlist = (List<object>)e .UserState;
                  string pro = objlist[0] .ToString();
                  if (pro != "-1")
                  {
                        lblinfo .Text = "计算中..." + pro + "%";
                  }
                  else
                  {
                        lblinfo .Text = "数据准备中...";
                  }
            }
            /// <summary>
            /// 以件号找矩形,单次快排
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            private void bwGo_DoWork(object sender, System .ComponentModel .DoWorkEventArgs e)
            {
                  List<object> list_1 = new List<object>();
                  list_1 .Add(-1);
                  bwGo .ReportProgress(0, list_1);
                  Dictionary<string, object> dic = (Dictionary<string, object>)e .Argument;
                  DNA dna = (DNA)dic["dna"];
                  bool matchwidth = Convert .ToBoolean(dic["matchwidth"]);
                  float matchrate = Convert .ToSingle(dic["matchrate"]);
                  string algorithm = dic["algorithm"] .ToString();
                  AddLib(dna .Stock[0], _type, matchwidth, matchrate);
                  DNA d = dna .Copy();
                  float sumArea = 0f;

                  List<object> list0 = new List<object>();
                  list0 .Add(0);
                  bwGo .ReportProgress(0, list0);

                  //所有库存初始化
                  for (int i = 0; i < dna .Stock .Count; i++)
                  {
                        //检测组合模型大小
                        float limit = dna .Stock[i] .Height;
                        PlateHelper ph = new PlateHelper();

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
                        #region 匹配剩余矩形--算法一（判断移动量）
                        if (algorithm == "算法一")
                        {
                              for (int b = 0; b < dna .Basic .Count; b++)
                              {
                                    int count = dna .Basic[b] .Count;
                                    Basic basic = dna .Basic[b];
                                    if (_smallfill)
                                    {
                                          var pm0 = _part .Where(t => t .id == basic .Id) .ToList()[0];
                                          if (pm0 .Rect .Width < _smallsize || pm0 .Rect .Height < _smallsize)
                                          {
                                                break;
                                          }
                                    }

                                    float area = basic .Area;

                                    int rowcount = s .Disable .GetLength(0);
                                    int colcount = s .Disable .GetLength(1);

                                    while (count > 1)
                                    {
                                          rect .Sort(new CompareRect());
                                          bool find = false;
                                          for (int n = 0; n < rect .Count; n++)
                                          {
                                                MyRectangle r = rect[n];
                                                //判断组合零件是否可以排在此处
                                                int minx = -1;
                                                Point pmove = new Point(-1, -1);
                                                GridLib glmove = new GridLib();
                                                int amove = 0;
                                                int kind2 = _part .Where(t => t .id == basic .Id) .ToList()[0] .Kind2;
                                                for (int a = 0; a < kind2; a++)
                                                {
                                                      GridLib gl = GetBasicOne(basic .Id, "C", a);
                                                      List<GridData> g = gl .Grid;
                                                      int[,] array = gl .GridArray;
                                                      int width = array .GetLength(0);
                                                      int height = array .GetLength(1);
                                                      if (width <= r .Width && height <= r .Height)
                                                      {
                                                            Point p = VirtualMove(s, rect, array, g, n);
                                                            if (minx == -1 || minx > width - p .X)
                                                            {
                                                                  minx = width - p .X;
                                                                  pmove = p;
                                                                  glmove = gl;
                                                                  amove = a;
                                                            }
                                                      }
                                                }
                                                if (minx > 0)
                                                {
                                                      List<GridData> gmove = glmove .Grid;
                                                      int[,] arraymove = glmove .GridArray;
                                                      Point location = ActionMove(pmove, ref s, ref rect, ref arraymove, ref gmove, n);
                                                      s .PartInfoList .Add(new PartInfo(basic .Id, "2", 0, location, amove));
                                                      dna .Basic[b] .Count -= 2;
                                                      sumArea += area * 2;
                                                      count -= 2;
                                                      find = true;
                                                      break;
                                                }
                                          }
                                          if (!find)
                                          {
                                                break;
                                          }
                                    }
                                    while (count > 0)
                                    {
                                          rect .Sort(new CompareRect());
                                          bool find = false;
                                          for (int n = 0; n < rect .Count; n++)
                                          {
                                                MyRectangle r = rect[n];
                                                //判断单个零件是否可以排在此处
                                                int minx = -1;
                                                Point pmove = new Point(-1, -1);
                                                GridLib glmove = new GridLib();
                                                int amove = 0;
                                                for (int ac = 0; ac < _anglelist .Count; ac++)
                                                {
                                                      int a = _anglelist[ac];
                                                      GridLib gl = GetBasicOne(basic .Id, "", a);
                                                      List<GridData> g = gl .Grid;
                                                      int[,] array = gl .GridArray;
                                                      int width = array .GetLength(0);
                                                      int height = array .GetLength(1);
                                                      if (width <= r .Width && height <= r .Height)
                                                      {
                                                            Point p = VirtualMove(s, rect, array, g, n);
                                                            if (minx == -1 || minx > width - p .X)
                                                            {
                                                                  minx = width - p .X;
                                                                  pmove = p;
                                                                  glmove = gl;
                                                                  amove = a;
                                                            }

                                                      }
                                                }
                                                if (minx > 0)
                                                {
                                                      List<GridData> gmove = glmove .Grid;
                                                      int[,] arraymove = glmove .GridArray;
                                                      Point location = ActionMove(pmove, ref s, ref rect, ref arraymove, ref gmove, n);
                                                      s .PartInfoList .Add(new PartInfo(basic .Id, "1", amove, location));
                                                      dna .Basic[b] .Count -= 1;
                                                      sumArea += area;
                                                      count -= 1;
                                                      find = true;
                                                      break;
                                                }
                                          }
                                          if (!find)
                                          {
                                                break;
                                          }
                                    }
                                    List<object> list1 = new List<object>();
                                    list1 .Add(100 * b / (dna .Basic .Count - 1) / 2);
                                    bwGo .ReportProgress(0, list1);
                              }

                              List<object> list1_1 = new List<object>();
                              list1_1 .Add(50);
                              bwGo .ReportProgress(0, list1_1);
                        }
                        #endregion
                        #region 匹配剩余矩形--算法二（判断面积和移动量）
                        else if (algorithm == "算法二")
                        {
                              for (int b = 0; b < dna .Basic .Count; b++)
                              {
                                    int count = dna .Basic[b] .Count;
                                    Basic basic = dna .Basic[b];

                                    if (_smallfill)
                                    {
                                          var pm0 = _part .Where(t => t .id == basic .Id) .ToList()[0];
                                          if (pm0 .Rect .Width < _smallsize || pm0 .Rect .Height < _smallsize)
                                          {
                                                break;
                                          }
                                    }

                                    float area = basic .Area;

                                    int rowcount = s .Disable .GetLength(0);
                                    int colcount = s .Disable .GetLength(1);

                                    while (count > 1)
                                    {
                                          rect .Sort(new CompareRect());
                                          bool find = false;
                                          for (int n = 0; n < rect .Count; n++)
                                          {
                                                MyRectangle r = rect[n];
                                                //判断组合零件是否可以排在此处
                                                int minx = -1;
                                                int mins = -1;
                                                Point pmove = new Point(-1, -1);
                                                GridLib glmove = new GridLib();
                                                int amove = 0;
                                                int kind2 = _part .Where(t => t .id == basic .Id) .ToList()[0] .Kind2;
                                                for (int a = 0; a < kind2; a++)
                                                {
                                                      GridLib gl = GetBasicOne(basic .Id, "C", a);
                                                      List<GridData> g = gl .Grid;
                                                      int[,] array = gl .GridArray;
                                                      int width = array .GetLength(0);
                                                      int height = array .GetLength(1);
                                                      if (width <= r .Width && height <= r .Height)
                                                      {
                                                            Point p = VirtualMove(s, rect, array, g, n);
                                                            double rate = Math .Abs(Convert .ToDouble(mins - (width - p .X) * (height - p .Y)) / Convert .ToDouble(mins));
                                                            if (minx == -1
                                                                  || (rate > 0.01 && mins > (width - p .X) * (height - p .Y))
                                                                  || (rate <= 0.01 && minx > width - p .X))
                                                            {
                                                                  mins = (width - p .X) * (height - p .Y);
                                                                  minx = width - p .X;
                                                                  pmove = p;
                                                                  glmove = gl;
                                                                  amove = a;
                                                            }
                                                      }
                                                }
                                                if (minx > 0)
                                                {
                                                      List<GridData> gmove = glmove .Grid;
                                                      int[,] arraymove = glmove .GridArray;
                                                      Point location = ActionMove(pmove, ref s, ref rect, ref arraymove, ref gmove, n);
                                                      s .PartInfoList .Add(new PartInfo(basic .Id, "2", 0, location, amove));
                                                      dna .Basic[b] .Count -= 2;
                                                      sumArea += area * 2;
                                                      count -= 2;
                                                      find = true;
                                                      break;
                                                }
                                          }
                                          if (!find)
                                          {
                                                break;
                                          }
                                    }
                                    while (count > 0)
                                    {
                                          rect .Sort(new CompareRect());
                                          bool find = false;
                                          for (int n = 0; n < rect .Count; n++)
                                          {
                                                MyRectangle r = rect[n];
                                                //判断单个零件是否可以排在此处
                                                int minx = -1;
                                                int mins = -1;
                                                Point pmove = new Point(-1, -1);
                                                GridLib glmove = new GridLib();
                                                int amove = 0;
                                                for (int ac = 0; ac < _anglelist .Count; ac++)
                                                {
                                                      int a = _anglelist[ac];
                                                      GridLib gl = GetBasicOne(basic .Id, "", a);
                                                      List<GridData> g = gl .Grid;
                                                      int[,] array = gl .GridArray;
                                                      int width = array .GetLength(0);
                                                      int height = array .GetLength(1);
                                                      if (width <= r .Width && height <= r .Height)
                                                      {
                                                            Point p = VirtualMove(s, rect, array, g, n);
                                                            double rate = Math .Abs(Convert .ToDouble(mins - (width - p .X) * (height - p .Y)) / Convert .ToDouble(mins));
                                                            if (minx == -1
                                                                  || (rate > 0.01 && mins > (width - p .X) * (height - p .Y))
                                                                  || (rate <= 0.01 && minx > width - p .X))
                                                            {
                                                                  mins = (width - p .X) * (height - p .Y);
                                                                  minx = width - p .X;
                                                                  pmove = p;
                                                                  glmove = gl;
                                                                  amove = a;
                                                            }

                                                      }
                                                }
                                                if (minx > 0)
                                                {
                                                      List<GridData> gmove = glmove .Grid;
                                                      int[,] arraymove = glmove .GridArray;
                                                      Point location = ActionMove(pmove, ref s, ref rect, ref arraymove, ref gmove, n);
                                                      s .PartInfoList .Add(new PartInfo(basic .Id, "1", amove, location));
                                                      dna .Basic[b] .Count -= 1;
                                                      sumArea += area;
                                                      count -= 1;
                                                      find = true;
                                                      break;
                                                }
                                          }
                                          if (!find)
                                          {
                                                break;
                                          }
                                    }
                                    List<object> list1 = new List<object>();
                                    list1 .Add(100 * b / (dna .Basic .Count - 1) / 2);
                                    bwGo .ReportProgress(0, list1);
                              }
                        }
                        #endregion
                        #region 匹配剩余矩形--算法三
                        else if (algorithm == "算法三")
                        {
                              for (int b = 0; b < dna .Basic .Count; b++)
                              {
                                    int count = dna .Basic[b] .Count;
                                    Basic basic = dna .Basic[b];

                                    if (_smallfill)
                                    {
                                          var pm0 = _part .Where(t => t .id == basic .Id) .ToList()[0];
                                          if (pm0 .Rect .Width < _smallsize || pm0 .Rect .Height < _smallsize)
                                          {
                                                break;
                                          }
                                    }

                                    float area = basic .Area;

                                    int rowcount = s .Disable .GetLength(0);
                                    int colcount = s .Disable .GetLength(1);

                                    while (count > 1)
                                    {
                                          rect .Sort(new CompareRect());
                                          bool find = false;

                                          int minx = -1;
                                          Point pmove = new Point(-1, -1);
                                          GridLib glmove = new GridLib();
                                          int amove = 0;
                                          int rindex = 0;
                                          for (int n = 0; n < rect .Count; n++)
                                          {
                                                MyRectangle r = rect[n];
                                                //判断组合零件是否可以排在此处
                                                int kind2 = _part .Where(t => t .id == basic .Id) .ToList()[0] .Kind2;
                                                for (int a = 0; a < kind2; a++)
                                                {
                                                      GridLib gl = GetBasicOne(basic .Id, "C", a);
                                                      List<GridData> g = gl .Grid;
                                                      int[,] array = gl .GridArray;
                                                      int width = array .GetLength(0);
                                                      int height = array .GetLength(1);
                                                      if (width <= r .Width && height <= r .Height)
                                                      {
                                                            Point p = VirtualMove(s, rect, array, g, n);
                                                            if (minx == -1 || minx > r .X + width - p .X)
                                                            {
                                                                  minx = r .X + width - p .X;
                                                                  pmove = p;
                                                                  glmove = gl;
                                                                  amove = a;
                                                                  rindex = n;
                                                            }
                                                      }
                                                }
                                          }
                                          if (minx > 0)
                                          {
                                                List<GridData> gmove = glmove .Grid;
                                                int[,] arraymove = glmove .GridArray;
                                                Point location = ActionMove(pmove, ref s, ref rect, ref arraymove, ref gmove, rindex);
                                                s .PartInfoList .Add(new PartInfo(basic .Id, "2", 0, location, amove));
                                                dna .Basic[b] .Count -= 2;
                                                sumArea += area * 2;
                                                count -= 2;
                                                find = true;
                                                break;
                                          }
                                          if (!find)
                                          {
                                                break;
                                          }
                                    }
                                    while (count > 0)
                                    {
                                          rect .Sort(new CompareRect());
                                          bool find = false;

                                          int minx = -1;
                                          Point pmove = new Point(-1, -1);
                                          GridLib glmove = new GridLib();
                                          int amove = 0;
                                          int rindex = 0;
                                          for (int n = 0; n < rect .Count; n++)
                                          {
                                                MyRectangle r = rect[n];
                                                //判断单个零件是否可以排在此处
                                                for (int ac = 0; ac < _anglelist .Count; ac++)
                                                {
                                                      int a = _anglelist[ac];
                                                      GridLib gl = GetBasicOne(basic .Id, "", a);
                                                      List<GridData> g = gl .Grid;
                                                      int[,] array = gl .GridArray;
                                                      int width = array .GetLength(0);
                                                      int height = array .GetLength(1);
                                                      if (width <= r .Width && height <= r .Height)
                                                      {
                                                            Point p = VirtualMove(s, rect, array, g, n);
                                                            if (minx == -1 || minx > r .X + width - p .X)
                                                            {
                                                                  minx = r .X + width - p .X;
                                                                  pmove = p;
                                                                  glmove = gl;
                                                                  amove = a;
                                                                  rindex = n;
                                                            }

                                                      }
                                                }
                                          }
                                          if (minx > 0)
                                          {
                                                List<GridData> gmove = glmove .Grid;
                                                int[,] arraymove = glmove .GridArray;
                                                Point location = ActionMove(pmove, ref s, ref rect, ref arraymove, ref gmove, rindex);
                                                s .PartInfoList .Add(new PartInfo(basic .Id, "1", amove, location));
                                                dna .Basic[b] .Count -= 1;
                                                sumArea += area;
                                                count -= 1;
                                                find = true;
                                                break;
                                          }
                                          if (!find)
                                          {
                                                break;
                                          }
                                    }
                                    List<object> list1 = new List<object>();
                                    list1 .Add(100 * b / (dna .Basic .Count - 1) / 2);
                                    bwGo .ReportProgress(0, list1);
                              }
                        }
                        #endregion
                        #region 剩余零件插孔排入
                        for (int b = 0; b < dna .Basic .Count; b++)
                        {
                              if (dna .Basic[b] .Count <= 0)
                              {
                                    continue;
                              }
                              Basic basic = dna .Basic[b];
                              int count = basic .Count;
                              float area = basic .Area;
                              int rowcount = s .Disable .GetLength(0);
                              int colcount = s .Disable .GetLength(1);
                              while (count > 1)
                              {
                                    bool find = false;
                                    int kind2 = _part .Where(t => t .id == basic .Id) .ToList()[0] .Kind2;
                                    for (int a = 0; a < kind2; a++)
                                    {
                                          GridLib gl = GetBasicOne(basic .Id, "C", a);
                                          List<GridData> grid = gl .Grid;
                                          int[,] array = gl .GridArray;
                                          int width = array .GetLength(0);
                                          int height = array .GetLength(1);
                                          for (int m = 0; m < rowcount; m++)
                                          {
                                                //组合图形适配
                                                int n = 0;
                                                bool add = false;
                                                while (n < colcount)
                                                {
                                                      if (CanAdd(s, grid, m, n))
                                                      {
                                                            s .Disable = GridAdd(s .Disable, grid, m, n);
                                                            sumArea += area * 2;
                                                            count -= 2;
                                                            dna .Basic[b] .Count -= 2;
                                                            s .PartInfoList .Add(new PartInfo(basic .Id, "2", 0, new Point(m, n), a));
                                                            add = true;
                                                            find = true;
                                                            break;
                                                      }
                                                      n++;
                                                }
                                                if (add)
                                                {
                                                      break;
                                                }
                                          }
                                          if (find)
                                          {
                                                break;
                                          }
                                    }
                                    if (!find)
                                    {
                                          break;
                                    }
                              }
                              while (count > 0)
                              {
                                    bool find = false;
                                    for (int ac = 0; ac < _anglelist .Count; ac++)
                                    {
                                          int a = _anglelist[ac];
                                          GridLib gl = GetBasicOne(basic .Id, "", a);
                                          List<GridData> grid = gl .Grid;
                                          int[,] array = gl .GridArray;
                                          int width = array .GetLength(0);
                                          int height = array .GetLength(1);
                                          for (int m = 0; m < rowcount; m++)
                                          {
                                                //组合图形适配
                                                int n = 0;
                                                bool add = false;
                                                while (n < colcount)
                                                {
                                                      if (CanAdd(s, grid, m, n))
                                                      {
                                                            s .Disable = GridAdd(s .Disable, grid, m, n);
                                                            sumArea += area * 1;
                                                            count -= 1;
                                                            dna .Basic[b] .Count -= 1;
                                                            s .PartInfoList .Add(new PartInfo(basic .Id, "1", a, new Point(m, n)));
                                                            add = true;
                                                            find = true;
                                                            break;
                                                      }
                                                      n++;
                                                }
                                                if (add)
                                                {
                                                      break;
                                                }
                                          }
                                          if (find)
                                          {
                                                break;
                                          }
                                    }
                                    if (!find)
                                    {
                                          break;
                                    }
                              }
                              List<object> list2 = new List<object>();
                              list2 .Add(50 + 100 * b / (dna .Basic .Count - 1) / 2);
                              bwGo .ReportProgress(0, list2);
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
                  e .Result = d;

                  List<object> list2_2 = new List<object>();
                  list2_2 .Add(100);
                  bwGo .ReportProgress(0, list2_2);

            }

            /// <summary>
            /// 补充库，匹配板宽
            /// </summary>
            /// <param name="id"></param>
            /// <param name="angle"></param>
            /// <param name="anglecombine"></param>
            private void AddLib(Stock s, string type, bool matchwidth, float matchrate)
            {
                  float limit = s .Height;
                  float width = s .Width;
                  for (int n = 0; n < _part .Count; n++)
                  {
                        PlateModel pm = _part[n];
                        int id = pm .id;
                        if (pm .PlateCount > 1
                              && (pm .Rect .Height > limit / 2 || pm .Rect .Width > limit / 2))
                        {
                              int count = 0;
                              for (int i = _partCombine .Count - 1; i >= 0; i--)
                              {
                                    if (_partCombine[i] .key .Contains("C" + id .ToString() + "/"))
                                    {
                                          if (_partCombine[i] .ifMatchWidth)
                                          {
                                                _partCombine .RemoveAt(i);
                                          }
                                          else
                                          {
                                                count++;
                                          }
                                    }
                              }
                              foreach (var item in _basicLib .ToList())
                              {
                                    if (item .Key .Contains("C" + id .ToString() + "/")
                                          && item .Value .ifMatchWidth)
                                    {
                                          _basicLib .Remove(item .Key);
                                    }
                              }
                              _part[n] .Kind2 = count;
                              if (matchwidth)
                              {
                                    GridHelper gh = new GridHelper();
                                    PlateHelper ph = new PlateHelper();
                                    List<PlateCombine> pc = new List<PlateCombine>();
                                    pc = ph .GetMinPlateCombineAll(pm, T, type, limit, matchwidth, matchrate);
                                    pm .Kind2 = pc .Count + count;
                                    for (int i = 0; i < pc .Count; i++)
                                    {
                                          string key = "C" + id .ToString() + "/" + (i + count) .ToString();//Cid/索引（索引范围在_part[i].Kind2)
                                          pc[i] .key = key;
                                          pc[i] .ifMatchWidth = true;
                                          _partCombine .Add(pc[i]);
                                          GridLib gl = new GridLib();
                                          gl = gpu .GetGridValueCombine(pc[i], T, gpuprogram);
                                          gl .Width = pc[i] .Rect .Width;
                                          gl .Height = pc[i] .Rect .Height;
                                          gl .ifMatchWidth = true;
                                          _basicLib .Add(key, gl);
                                    }
                              }
                        }
                  }
            }
            /// <summary>
            /// 计算栅格码，先从库中找
            /// </summary>
            /// <param name="basic"></param>
            /// <returns></returns>
            private GridLib GetBasicOne(int id, string type, int angle)
            {
                  try
                  {
                        string key = "";
                        if (type == "C")
                        {
                              key = "C" + id .ToString() + "/" + angle .ToString();
                        }
                        else
                        {
                              key = id .ToString() + "/" + angle .ToString();
                        }

                        GridLib gl = _basicLib[key];
                        return gl;
                  }
                  catch (Exception ex)
                  {
                        throw (ex);
                  }
            }
            /// <summary>
            /// 按面积排序
            /// </summary>
            public class CompareArea : IComparer<PlateModel>
            {
                  public int Compare(PlateModel pm1, PlateModel pm2)
                  {
                        return pm2 .Area .CompareTo(pm1 .Area);
                  }
            }
            /// <summary>
            /// 生成角度范围
            /// </summary>
            /// <param name="anglecount"></param>
            public void CreateAngelList(int anglecount)
            {
                  _anglelist .Clear();
                  for (int i = 0; i < anglecount; i++)
                  {
                        _anglelist .Add(i * (360 / anglecount));
                  }
            }

            /// <summary>
            /// 模拟移动
            /// </summary>
            /// <param name="s"></param>
            /// <param name="r"></param>
            /// <param name="array"></param>
            /// <param name="grid"></param>
            public Point VirtualMove(Stock s, List<MyRectangle> rect, int[,] array, List<GridData> grid, int index = 0)
            {
                  MyRectangle r = rect[0];
                  if (index > 0)
                  {
                        r = rect[index];
                  }
                  int width = array .GetLength(0);
                  int height = array .GetLength(1);

                  //微调，向左向上移动
                  int movex = 0;
                  int movey = 0;
                  string last = "first";
                  while (1 == 1)
                  {
                        //搜索方式：先找到距边缘最近的栅格，再从栅格处，反向搜索找到板上距离最近的栅格，计算距离
                        if (last == "first" || last == "up")//如果上一次移动是向上移动，则计算是否能向左移动。第一次都要计算，先向左移
                        {
                              int minx = -1;
                              for (int h = 0; h < height; h++)
                              {
                                    if (r .Y + h - movey >= s .Disable .GetLength(1))
                                          continue;
                                    int right = 0;
                                    while (right < width && array[right, h] <= 0)
                                    {
                                          right++;
                                    }
                                    int left = 0;
                                    while (r .X + right - left - 1 - movex >= s .Disable .GetLength(0)
                                          || (r .X + right - left - 1 - movex >= 0
                                                      && r .Y + h - movey >= 0
                                                      && s .Disable[r .X + right - left - 1 - movex, r .Y + h - movey] <= 0)
                                          )
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
                              if (minx > 0)
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
                                    if (r .X + w - movex >= s .Disable .GetLength(0))
                                          continue;
                                    int down = 0;
                                    while (down < height && array[w, down] <= 0)
                                    {
                                          down++;
                                    }
                                    int up = 0;
                                    while (r .Y + down - up - 1 - movey >= s .Disable .GetLength(1)
                                          || (r .Y + down - up - 1 - movey >= 0
                                          && r .X + w - movex >= 0
                                          && s .Disable[r .X + w - movex, r .Y + down - up - 1 - movey] <= 0)
                                          )
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
                              if (miny > 0)
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
                  return new Point(movex, movey);
            }
            /// <summary>
            /// 按左底原则排入
            /// </summary>
            /// <param name="s"></param>
            /// <param name="r"></param>
            /// <param name="array"></param>
            /// <param name="grid"></param>
            public Point ActionMove(Point pmove, ref Stock s, ref List<MyRectangle> rect, ref int[,] array, ref List<GridData> grid, int index = 0)
            {
                  MyRectangle r = rect[0];
                  if (index > 0)
                  {
                        r = rect[index];
                  }
                  int width = array .GetLength(0);
                  int height = array .GetLength(1);
                  //微调，向左向上移动
                  int movex = pmove .X;
                  int movey = pmove .Y;
                  //排入
                  for (int j = 0; j < grid .Count; j++)
                  {
                        int row = grid[j] .Row;
                        int col = grid[j] .Col;

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
            #endregion
            #region 栅格判断
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
            #endregion
            #region 矩形操作
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
            #region 保存dxf
            private void btnsavefile_Click(object sender, EventArgs e)
            {
                  if (_path == "")
                  {
                        YKMessageBox .ShowBox("请设置结果保存路径");
                        return;
                  }
                  string path = $@"{_path}\{DateTime .Now .ToString("yyyy-MM-dd")}";
                  _result .Add(_currentdna);
                  ResultAdd(_currentdna);
                  CountUse();
                  int count = _result .Count;
                  string filename = cmbetname .Text + cmblname .Text + cmbmfspec .Text + "(" + count .ToString() + ")"
                        + _currentdna .Stock[0] .Width .ToString() + "×" + _currentdna .Stock[0] .Height .ToString();
                  if (!Directory .Exists(path))
                  {
                        Directory .CreateDirectory(path);
                  }
                  SaveDXF($@"{path}\{filename}.dxf");
                  YKMessageBox .ShowBox("保存成功");
            }
            private void SaveDXF(string filepath)
            {
                  List<BaseShape> bs = cadInterfaceMain .currentShapes;
                  DxfDocument dxf = new DxfDocument();
                  netDxf .Tables .Layer layer_nest = new netDxf .Tables .Layer("nest");
                  netDxf .Tables .Layer layer_txt = new netDxf .Tables .Layer("txt");
                  for (int i = 0; i < bs .Count; i++)
                  {
                        if (bs[i] .ShapeClass == "Line")
                        {
                              Line l = (Line)bs[i];
                              netDxf .Entities .Line line = new netDxf .Entities .Line(new Vector2(l .StartPoint .X, l .StartPoint .Y)
                                    , new Vector2(l .EndPoint .X, l .EndPoint .Y));
                              line .Layer = layer_nest;
                              line .Color = new AciColor(l .PenColor);
                              dxf .AddEntity(line);
                        }
                        else if (bs[i] .ShapeClass == "Arc")
                        {
                              Arc a = (Arc)bs[i];
                              netDxf .Entities .Arc arc = new netDxf .Entities .Arc(new Vector2(a .CenterPoint .X, a .CenterPoint .Y), a .Radius, a .StartAngle, a .EndAngle);
                              arc .Layer = layer_nest;
                              arc .Color = new AciColor(a .PenColor);
                              dxf .AddEntity(arc);
                        }
                        else if (bs[i] .ShapeClass == "Circle")
                        {
                              Circle c = (Circle)bs[i];
                              netDxf .Entities .Circle circle = new netDxf .Entities .Circle(new Vector2(c .CenterPoint .X, c .CenterPoint .Y), c .Radius);
                              circle .Layer = layer_nest;
                              circle .Color = new AciColor(c .PenColor);
                              dxf .AddEntity(circle);
                        }
                        else if (bs[i] .ShapeClass == "Ellipse")
                        {
                              Ellipse e = (Ellipse)bs[i];
                              netDxf .Entities .Ellipse ellipse = new netDxf .Entities .Ellipse(new Vector2(e .CenterPoint .X, e .CenterPoint .Y), e .LongRadius * 2, e .ShortRadius * 2);
                              ellipse .StartAngle = e .StartAngle;
                              ellipse .EndAngle = e .EndAngle;
                              ellipse .Rotation = e .Angle;
                              ellipse .Layer = layer_nest;
                              ellipse .Color = new AciColor(e .PenColor);
                              dxf .AddEntity(ellipse);
                        }
                        else if (bs[i] .ShapeClass == "Text")
                        {
                              Text t = (Text)bs[i];
                              netDxf .Entities .Text text = new netDxf .Entities .Text(t .TextString, new Vector2(t .LocadPoint .X, t .LocadPoint .Y), 10);
                              text .Layer = layer_txt;
                              text .Color = new AciColor(t .PenColor);
                              dxf .AddEntity(text);
                        }
                  }
                  dxf .Save(filepath);
            }
            /// <summary>
            /// 画排料图，线图
            /// </summary>
            /// <param name="s"></param>
            private void ExportDXF(Stock s, string filepath)
            {

                  List<BaseShape> bs = cadInterfaceMain .currentShapes;
                  DXFLibrary .Document doc = new DXFLibrary .Document();

                  for (int i = 0; i < bs .Count; i++)
                  {
                        if (bs[i] .ShapeClass == "Line")
                        {
                              Line l = (Line)bs[i];
                              DXFLibrary .Line line = new DXFLibrary .Line("Doors"
                                    , l .StartPoint .X, l .StartPoint .Y
                                    , l .EndPoint .X, l .EndPoint .Y);
                              doc .add(line);
                        }
                        else if (bs[i] .ShapeClass == "Arc")
                        {
                              Arc a = (Arc)bs[i];
                              DXFLibrary .Arc arc = new DXFLibrary .Arc(a .CenterPoint .X, a .CenterPoint .Y, a .Radius, a .StartAngle, a .EndAngle, "Doors");
                              doc .add(arc);
                        }
                        else if (bs[i] .ShapeClass == "Circle")
                        {
                              Circle c = (Circle)bs[i];
                              DXFLibrary .Circle circle = new DXFLibrary .Circle(c .CenterPoint .X, c .CenterPoint .Y, c .Radius, "Doors");
                        }
                        else if (bs[i] .ShapeClass == "Text")
                        {
                              Text t = (Text)bs[i];
                              DXFLibrary .Text text = new DXFLibrary .Text(t .TextString, t .LocadPoint .X, t .LocadPoint .Y, 30, "Doors");
                              doc .add(text);
                        }
                  }
                  FileStream f1 = new FileStream(filepath, System .IO .FileMode .Create);
                  DXFLibrary .Writer .Write(doc, f1);
                  f1 .Close();
            }
            #endregion
            #region 读写配置文件
            /// <summary>
            /// 读取配置
            /// </summary>
            private void GetConfig()
            {
                  txtProtect .Text = Getini("protect");
                  cmbType .Text = Getini("combinetype");
                  txtSize .Text = Getini("minsize");
                  txtT .Text = Getini("t");
                  txtanglecount .Text = Getini("anglecount");
                  string pro = Getini("matchwidth");
                  chkmatchwidth .Checked = Convert .ToBoolean(pro == "" ? "true" : pro);
                  txtpath .Text = Getini("savepath");
                  cmbalgorithm .Text = Getini("algorithm");
                  string smallfill = Getini("smallfill");
                  chksmallfill .Checked = Convert .ToBoolean(smallfill == "" ? "false" : smallfill);
                  txtmatchrate .Text = Getini("matchrate");
                  //////////////////////////////
                  _protect = Convert .ToInt32(txtProtect .Text == "" ? "0" : txtProtect .Text);
                  string type = cmbType .Text;
                  if (type == "矩形")
                        _type = "rect";
                  else if (type == "平行四边形")
                        _type = "para";
                  else if (type == "混合")
                        _type = "mix";
                  else
                        _type = "";
                  _smallsize = Convert .ToInt32(txtSize .Text == "" ? "500" : txtSize .Text);
                  T = Convert .ToInt32(txtT .Text == "" ? "10" : txtT .Text);
                  int anglecount = Convert .ToInt32(txtanglecount .Text == "" ? "4" : txtanglecount .Text);
                  CreateAngelList(anglecount);
                  _matchwidth = chkmatchwidth .Checked;
                  _path = Getini("savepath");
                  _algorithm = cmbalgorithm .Text;
                  _smallfill = chksmallfill .Checked;
                  _matchrate = txtmatchrate .Text == "" ? 0.9f : Convert .ToSingle(txtmatchrate .Text);
            }
            /// <summary>
            /// 选择离开设置tab时自动保存
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
            {
                  try
                  {
                        if (_oldpageindex > 0 && tabMain .TabPages[_oldpageindex] == tabMain .TabPages["pageSet"])
                        {
                              SaveSet();
                        }
                        _oldpageindex = tabMain .SelectedIndex;
                  }
                  catch
                  { }
            }
            /// <summary>
            /// 保存配置
            /// </summary>
            private void SaveSet()
            {
                  Writeini("protect", txtProtect .Text);
                  Writeini("combinetype", cmbType .Text);
                  Writeini("minsize", txtSize .Text);
                  Writeini("t", txtT .Text);
                  Writeini("anglecount", txtanglecount .Text);
                  Writeini("matchwidth", chkmatchwidth .Checked .ToString());
                  Writeini("savepath", txtpath .Text);
                  Writeini("algorithm", cmbalgorithm .Text);
                  Writeini("smallfill", chksmallfill .Checked .ToString());
                  Writeini("matchrate", txtmatchrate .Text);
                  GetConfig();
            }
            /// <summary>
            /// 保存配置
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnSure_Click(object sender, EventArgs e)
            {
                  SaveSet();
            }

            /// <summary>
            /// 选择路径
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
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
            /// <summary>
            /// 读取ini
            /// </summary>
            /// <param name="select"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            private string Getini(string key)
            {
                  string currentpath = Directory .GetCurrentDirectory();
                  string path = currentpath + "\\nest.ini";
                  StringBuilder sb = new StringBuilder();
                  GetPrivateProfileString("NestConfig", key, "", sb, 1024, path);
                  return sb .ToString();
            }
            /// <summary> 
            /// 写入INI文件 
            /// </summary> 
            /// <param name="Section">项目名称(如 [TypeName] )</param> 
            /// <param name="Key">键</param> 
            /// <param name="Value">值</param> 
            public void Writeini(string key, string Value)
            {
                  string currentpath = Directory .GetCurrentDirectory();
                  string path = currentpath + "\\nest.ini";
                  WritePrivateProfileString("NestConfig", key, Value, path);
            }
            /// <summary>
            /// 恢复默认
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnrset_Click(object sender, EventArgs e)
            {
                  Writeini("protect", "5");
                  Writeini("combinetype", "平行四边形");
                  Writeini("minsize", "300");
                  Writeini("t", "10");
                  Writeini("anglecount", "4");
                  Writeini("matchwidth", "True");
                  Writeini("savepath", "");
                  Writeini("algorithm", "算法一");
                  Writeini("smallfill", "False");
                  Writeini("matchrate", "0.9");
                  GetConfig();
            }
            #endregion;
            #region 读取说明
            /// <summary>
            /// 读取txt文件内容
            /// </summary>
            /// <param name="Path">文件地址</param>
            public void Readme()
            {
                  try
                  {
                        string currentpath = Directory .GetCurrentDirectory();
                        string path = currentpath + "\\说明.rtf";
                        txtreadme .LoadFile(path);//读取
                        lnkreadme .Visible = false;
                  }
                  catch (Exception ex)
                  {
                        lnkreadme .Visible = true;
                  }
            }
            private void lnkreadme_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
                  Readme();
            }

            #endregion
            #region 后处理
            /// <summary>
            /// 增加结果表中
            /// </summary>
            private void ResultAdd(DNA dna)
            {
                  if (_dtresult .Rows .Count == 0)
                  {
                        _dtresult = DevCommon .DgvToTableEmptyByProperty(dgvr1);
                        _dtresultdetail = DevCommon .DgvToTableEmptyByProperty(dgvr2);
                  }
                  DataRow dr = _dtresult .NewRow();
                  dr["etName"] = _currentetname;
                  dr["lName"] = _currentlname;
                  dr["mfSpec"] = _currentmfspec;
                  dr["sLength"] = dna .Stock[0] .Width;
                  dr["sWidth"] = dna .Stock[0] .Height;
                  dr["rCount"] = _maxcount;
                  dr["rRate"] = dna .Fitness;
                  dr["rID"] = _currentid;
                  _dtresult .Rows .Add(dr);
                  for (int i = 0; i < dna .Stock[0] .PartInfoList .Count; i++)
                  {
                        PartInfo pi = dna .Stock[0] .PartInfoList[i];
                        int id = pi .ID;
                        if (pi .Type == "1")
                        {
                              PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                              string pname = pm .PlateName;
                              bool find = false;
                              for (int j = 0; j < _dtresultdetail .Rows .Count; j++)
                              {
                                    if (_dtresultdetail .Rows[j]["rpName"] .ToString() == pname
                                          && _dtresultdetail .Rows[j]["rID"] .ToString() == _currentid .ToString())
                                    {
                                          _dtresultdetail .Rows[j]["rpCount"] = Convert .ToInt32(_dtresultdetail .Rows[j]["rpCount"] .ToString()) + 1 * _maxcount;
                                          find = true;
                                    }
                              }
                              if (!find)
                              {
                                    DataRow ddr = _dtresultdetail .NewRow();
                                    ddr["rpName"] = pname;
                                    ddr["rpCount"] = 1 * _maxcount;
                                    ddr["rID"] = _currentid;
                                    _dtresultdetail .Rows .Add(ddr);
                              }
                              for (int k = 0; k < _part .Count; k++)
                              {
                                    if (_part[k] .id == id)
                                    {
                                          _part[k] .PlateCount = _part[k] .PlateCount - 1 * _maxcount;
                                    }
                              }
                        }
                        else
                        {
                              string key = "C" + id .ToString() + "/" + pi .combineType .ToString();
                              PlateCombine pc = _partCombine .Where(t => t .key == key) .ToList()[0];
                              string pname = pc .Plate1 .PlateName;
                              bool find = false;
                              for (int j = 0; j < _dtresultdetail .Rows .Count; j++)
                              {
                                    if (_dtresultdetail .Rows[j]["rpName"] .ToString() == pname
                                          && _dtresultdetail .Rows[j]["rID"] .ToString() == _currentid .ToString())
                                    {
                                          _dtresultdetail .Rows[j]["rpCount"] = Convert .ToInt32(_dtresultdetail .Rows[j]["rpCount"] .ToString()) + 2 * _maxcount;
                                          find = true;
                                    }
                              }
                              if (!find)
                              {
                                    DataRow ddr = _dtresultdetail .NewRow();
                                    ddr["rpName"] = pname;
                                    ddr["rpCount"] = 2 * _maxcount;
                                    ddr["rID"] = _currentid;
                                    _dtresultdetail .Rows .Add(ddr);
                              }
                              for (int k = 0; k < _part .Count; k++)
                              {
                                    if (_part[k] .id == id)
                                    {
                                          _part[k] .PlateCount = _part[k] .PlateCount - 2 * _maxcount;
                                    }
                              }
                        }
                  }
                  _currentid++;
                  dgvr1 .DataSource = _dtresult;
            }
            /// <summary>
            /// 查看结果明细
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void dgvr1_SelectionChanged(object sender, EventArgs e)
            {
                  int rid = Convert .ToInt32(dgvr1 .CurrentRow .Cells["rID"] .Value .ToString());
                  DataView dv = new DataView(_dtresultdetail);
                  dv .RowFilter = " rID = " + rid .ToString();
                  dgvr2 .DataSource = dv .ToTable();
            }
            /// <summary>
            /// 计算已使用量
            /// </summary>
            private void CountUse()
            {
                  for (int i = 0; i < dgvPart .Rows .Count; i++)
                  {
                        for (int j = 0; j < _part .Count; j++)
                        {
                              if (dgvPart .Rows[i] .Cells["pName"] .Value .ToString() == _part[j] .PlateName)
                              {
                                    dgvPart .Rows[i] .Cells["rCount"] .Value = _part[j] .PlateCount;
                              }
                        }
                  }
            }
            /// <summary>
            /// 计算最小数量
            /// </summary>
            /// <returns></returns>
            private int CountMin(DNA dna)
            {
                  int min = -1;
                  Dictionary<int, int> dic = new Dictionary<int, int>();
                  for (int i = 0; i < dna .Stock[0] .PartInfoList .Count; i++)
                  {
                        PartInfo pi = dna .Stock[0] .PartInfoList[i];
                        int id = pi .ID;
                        if (pi .Type == "1")
                        {
                              if (!dic .ContainsKey(id))
                              {
                                    dic .Add(id, 1);
                              }
                              else
                              {
                                    dic[id] += 1;
                              }
                        }
                        else
                        {
                              if (!dic .ContainsKey(id))
                              {
                                    dic .Add(id, 2);
                              }
                              else
                              {
                                    dic[id] += 2;
                              }
                        }
                  }
                  foreach (var d in dic)
                  {
                        int pcount = _part .Where(t => t .id == d .Key) .ToList()[0] .PlateCount;
                        if (min < 0 || pcount / d .Value < min)
                        {
                              min = pcount / d .Value;
                        }
                  }
                  if (min < 0)
                  {
                        min = 0;
                  }
                  return min;
            }
            /// <summary>
            /// 清除全部数据，重置id
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnDel_Click(object sender, EventArgs e)
            {
                  _dtresult = DevCommon .DgvToTableEmptyByProperty(dgvr1);
                  _dtresultdetail = DevCommon .DgvToTableEmptyByProperty(dgvr2);
                  _currentid = 1;
                  dgvr1 .DataSource = _dtresult;
                  dgvr2 .DataSource = _dtresultdetail;
                  _basicLib .Clear();
                  _part .Clear();
                  _partCombine .Clear();
                  _dtPart .Rows .Clear();
                  dgvPart .DataSource = _dtPart;
                  _currentPop .Clear();
                  _result .Clear();
                  DNA _currentdna = new DNA();
                  _maxcount = 0;

                  cadInterfaceMain .currentShapes .Clear();
                  cadInterfaceMain .currentPlates .Clear();
                  cadInterfaceMain .DrawShap();
            }
            #endregion

      }
}
