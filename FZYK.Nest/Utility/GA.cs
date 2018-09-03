using myCad .Model;
using myCad .ShapeOper;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace FZYK .Nest
{
      public class GA
      {
            //基础数据
            public List<PlateModel> _part { get; set; } = new List<PlateModel>();
            public List<PlateCombine> _partCombine { get; set; } = new List<PlateCombine>();
            public List<Stock> _stock { get; set; } = new List<Stock>();
            public float T { get; set; } = 0f;
            public int _size { get; set; } = 0;//pop size
            public bool _rotate { get; set; } = true;
            public string _type { get; set; } = "";
            public float _small { get; set; } = 0f;// small size
            public decimal _pcrossover { get; set; } = 0;
            public decimal _pmutation { get; set; } = 0;
            public Dictionary<string, GridLib> _basicLib { get; set; } = new Dictionary<string, GridLib>();//基因库

            CopyOper co = new CopyOper();
            PlateHelper ph = new PlateHelper();
            GridHelper gh = new GridHelper();
            public GA()
            {

            }
            public GA(List<PlateModel> part, List<PlateCombine> partCombine, List<Stock> stock
                  , float t, int size, bool rotate, string type, float small, decimal pcrossover, decimal pmutation)
            {
                  _part = part;
                  _partCombine = partCombine;
                  _stock = stock;
                  T = t;
                  _size = size;
                  _rotate = rotate;
                  _type = type;
                  _small = small;
                  _pcrossover = pcrossover;
                  _pmutation = pmutation;
            }
            #region 初始种群
            /// <summary>
            /// 生成初始种群
            /// </summary>
            public List<DNA> Create()
            {
                  List<DNA> pop = new List<DNA>();
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
                              CreateLib(id, angle, angleCombine);
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
                        pop .Add(dnathis);
                  }
                  //Parallel .ForEach(_pop, item => item = CountFitnessRectangle(item));//, new ParallelOptions { MaxDegreeOfParallelism = 5 }
                  return pop;
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
            /// 创建基因库
            /// </summary>
            /// <param name="id"></param>
            /// <param name="angle"></param>
            /// <param name="anglecombine"></param>
            private void CreateLib(int id, float angle, float anglecombine)
            {
                  string key = id .ToString() + "/" + angle .ToString();
                  string key2 = "C" + id .ToString() + "/" + anglecombine .ToString();
                  string key3 = id .ToString() + "/" + ((angle + 90) % 360) .ToString();
                  string key4 = id .ToString() + "/" + ((angle + 180) % 360) .ToString();

                  if (!_basicLib .ContainsKey(key))
                  {
                        GridLib gl = new GridLib();
                        PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                        gl = gh .GetGridValue(ph .RotateAndMove(pm, Convert .ToSingle(angle)), T);
                        _basicLib .Add(key, gl);
                  }

                  if (!_basicLib .ContainsKey(key2))
                  {
                        GridLib gl2 = new GridLib();
                        List<PlateCombine> pclist = _partCombine .Where(t => t .id == id) .ToList();
                        if (pclist .Count > 0)
                        {
                              PlateCombine pc = pclist[0];
                              gl2 = gh .GetGridValueCombine(ph .RotateAndMove(pc, Convert .ToSingle(anglecombine)), T);
                              _basicLib .Add(key2, gl2);
                        }
                  }

                  if (!_basicLib .ContainsKey(key3))
                  {
                        GridLib gl3 = new GridLib();
                        PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                        gl3 = gh .GetGridValue(ph .RotateAndMove(pm, Convert .ToSingle((angle + 90) % 360)), T);
                        _basicLib .Add(key3, gl3);
                  }

                  if (!_basicLib .ContainsKey(key4))
                  {
                        GridLib gl4 = new GridLib();
                        PlateModel pm = _part .Where(t => t .id == id) .ToList()[0];
                        gl4 = gh .GetGridValue(ph .RotateAndMove(pm, Convert .ToSingle((angle + 180) % 360)), T);
                        _basicLib .Add(key4, gl4);
                  }
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
            #endregion
            #region 交叉、变异、选择
            /// <summary>
            /// 交叉
            /// </summary>
            /// <param name="dna1"></param>
            /// <param name="dna2"></param>
            public Tuple<DNA, DNA> Crossover(DNA dna1, DNA dna2)
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
            public DNA Mutation(DNA dna)
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
            private List<DNA> Filter(List<DNA> pop)
            {
                  /*CompareDNA comp = new CompareDNA();
                  _pop .Sort(comp);
                  for (int i = _pop .Count - 1; i >= _size; i--)
                  {
                        _pop .RemoveAt(i);
                  }*/

                  List<DNA> dnalist = new List<DNA>();
                  CompareDNA comp = new CompareDNA();
                  pop .Sort(comp);
                  dnalist .Add(pop[0] .Copy());
                  pop .RemoveAt(0);

                  for (int i = 0; i < _size; i++)
                  {
                        DNA dna = SelectOne(pop);
                        pop .Remove(dna);
                        dnalist .Add(dna .Copy());
                  }
                  return dnalist;
            }
            /// <summary>
            /// 按适应度排序
            /// </summary>
            /// <param name="pop"></param>
            public void SortByFitness(ref List<DNA> pop)
            {
                  CompareDNA comp = new CompareDNA();
                  pop .Sort(comp);
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
            public DNA SelectOne(List<DNA> pop)
            {
                  Random r = new Random(GetRandomSeed());
                  double pick = r .NextDouble();
                  float sumfitness = 0;
                  float sum = 0;
                  int index = 0;
                  for (int i = 0; i < pop .Count; i++)
                  {
                        sumfitness += pop[i] .Fitness;
                  }
                  for (int i = 0; i < pop .Count; i++)
                  {
                        sum += pop[i] .Fitness / sumfitness;
                        if (sum >= pick)
                        {
                              index = i;
                              break;
                        }
                  }
                  return pop[index];
            }
            #endregion
            #region 剩余矩形动态匹配算法
            /// <summary>
            /// 剩余矩形动态匹配算法解码
            /// </summary>
            /// <param name="d"></param>
            /// <returns></returns>
            public DNA CountFitnessRectangle(DNA d)
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
                        //if (!chkPress .Checked)
                        //      break;
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
                        //if (s .Disable[row + r .X, col + r .Y] != 0)
                        //{
                        //      int test = 1;
                        //}
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
      }
}
