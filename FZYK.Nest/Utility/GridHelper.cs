using myCad .Model;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace FZYK .Nest
{
      public class GridHelper
      {
            float RE = 0.00001f;
            /// <summary>
            /// 获取栅格数据
            /// </summary>
            /// <param name="pm"></param>
            /// <returns></returns>
            public GridLib GetGridValue(PlateModel pm, float T)
            {
                  //分别用X扫描线，Y扫描线栅格化，取并集
                  RectangleF r = pm .Rect;
                  List<GridData> griddata = new List<GridData>();
                  List<GridData> griddataX = new List<GridData>();
                  List<GridData> griddataY = new List<GridData>();
                  int maxrow = 0;
                  int maxcol = 0;
                  bool start = false;
                  float startY = r .Y;
                  //X扫描
                  for (float t = 0; t <= r .Height + T; t += T)
                  {
                        float y = r .Y + t;
                        if (y > r .Height)
                        {
                              y = r .Y + r .Height;
                        }
                        List<PointF> plist = new List<PointF>();
                        for (int i = 0; i < pm .OutModel .ExpandShape .Count; i++)
                        {
                              plist = IntersectPointX(pm .OutModel .ExpandShape[i], y, plist);
                        }

                        for (int i = 0; i < pm .InnerModel .Count; i++)
                        {
                              for (int j = 0; j < pm .InnerModel[i] .ListShape .Count; j++)
                              {
                                    plist = IntersectPointX(pm .InnerModel[i] .ListShape[j], y, plist);
                              }
                        }
                        Tuple<List<PointF>, List<PointF>> tp = DisposeSpecialPointX(pm .OutModel .ExpandPoint, plist);
                        plist = tp .Item1;
                        List<PointF> line = tp .Item2;
                        for (int i = 0; i < pm .InnerModel .Count; i++)
                        {
                              Tuple<List<PointF>, List<PointF>> tp2 = DisposeSpecialPointX(pm .InnerModel[i] .ListPoint, plist);
                              plist = tp2 .Item1;
                              line .AddRange(tp2 .Item2);
                        }
                        CompareX comx = new CompareX();
                        plist .Sort(comx);
                        line .Sort(comx);
                        List<GridData> tmp = new List<GridData>();
                        for (int i = 0; i < plist .Count - 1; i = i + 2)//交点两两配对成水平直线
                        {
                              tmp .AddRange(LineToGridX(plist[i], plist[i + 1], new PointF(r .X, 0), T));
                        }
                        for (int i = 0; i < line .Count - 1; i = i + 2)//交点两两配对成水平直线
                        {
                              tmp .AddRange(LineToGridX(line[i], line[i + 1], new PointF(r .X, 0), T));
                        }
                        griddataX .AddRange(tmp);
                        if (tmp .Count > 0)
                        {
                              if (tmp[0] .Row > maxrow)
                                    maxrow = tmp[0] .Row;
                              if (tmp[tmp .Count - 1] .Col > maxcol)
                                    maxcol = tmp[tmp .Count - 1] .Col;
                        }
                  }
                  //Y扫描
                  for (float t = 0; t <= r .Width + T; t += T)
                  {
                        float x = r .X + t;
                        if (x > r .Width)
                        {
                              x = r .X + r .Width;
                        }
                        List<PointF> plist = new List<PointF>();
                        for (int i = 0; i < pm .OutModel .ExpandShape .Count; i++)
                        {
                              plist = IntersectPointY(pm .OutModel .ExpandShape[i], x, plist);
                        }

                        for (int i = 0; i < pm .InnerModel .Count; i++)
                        {
                              for (int j = 0; j < pm .InnerModel[i] .ListShape .Count; j++)
                              {
                                    plist = IntersectPointY(pm .InnerModel[i] .ListShape[j], x, plist);
                              }
                        }
                        Tuple<List<PointF>, List<PointF>> tp = DisposeSpecialPointY(pm .OutModel .ExpandPoint, plist);
                        plist = tp .Item1;
                        List<PointF> line = tp .Item2;
                        for (int i = 0; i < pm .InnerModel .Count; i++)
                        {
                              Tuple<List<PointF>, List<PointF>> tp2 = DisposeSpecialPointY(pm .InnerModel[i] .ListPoint, plist);
                              plist = tp2 .Item1;
                              line .AddRange(tp2 .Item2);
                        }
                        CompareY comy = new CompareY();
                        plist .Sort(comy);
                        line .Sort(comy);
                        List<GridData> tmp = new List<GridData>();
                        for (int i = 0; i < plist .Count - 1; i = i + 2)//交点两两配对成垂直直线
                        {
                              tmp .AddRange(LineToGridY(plist[i], plist[i + 1], new PointF(0, r .Y), T));
                        }
                        for (int i = 0; i < line .Count - 1; i = i + 2)
                        {
                              tmp .AddRange(LineToGridY(line[i], line[i + 1], new PointF(0, r .Y), T));
                        }
                        griddataY .AddRange(tmp);
                        if (tmp .Count > 0)
                        {
                              if (tmp[0] .Col > maxcol)
                                    maxcol = tmp[0] .Col;
                              if (tmp[tmp .Count - 1] .Row > maxrow)
                                    maxrow = tmp[tmp .Count - 1] .Row;
                        }
                  }
                  griddata = griddataX .Union(griddataY) .ToList();

                  int[,] gridarray = new int[maxrow + 1, maxcol + 1];
                  for (int i = 0; i < maxrow + 1; i++)
                  {
                        for (int j = 0; j < maxcol + 1; j++)
                        {
                              gridarray[i, j] = 0;
                        }
                  }
                  for (int i = 0; i < griddata .Count; i++)
                  {
                        gridarray[griddata[i] .Row, griddata[i] .Col] = 1;
                  }
                  List<GridData> grid = new List<GridData>();
                  for (int i = 0; i < gridarray .GetLength(0); i++)
                  {
                        int v = 1;
                        for (int j = gridarray .GetLength(1) - 1; j >= 0; j--)
                        {
                              if (gridarray[i, j] == 1)
                              {
                                    grid .Add(new GridData(i, j, v));
                                    v++;
                              }
                              else
                              {
                                    v = 1;
                              }
                        }
                  }
                  return new GridLib(grid, gridarray);
            }
            /// <summary>
            /// 合并图形栅格数据
            /// </summary>
            /// <param name="pc"></param>
            /// <param name="T"></param>
            /// <returns></returns>
            public GridLib GetGridValueCombine(PlateCombine pc, float T)
            {
                  GridLib gl1 = GetGridValue(pc .Plate1, T);
                  GridLib gl2 = GetGridValue(pc .Plate2, T);
                  List<GridData> griddata = gl1 .Grid .Union(gl2 .Grid) .ToList();

                  int maxrow = gl1 .GridArray .GetLength(0);
                  int maxcol = gl1 .GridArray .GetLength(1);
                  if (gl2 .GridArray .GetLength(0) > maxrow)
                        maxrow = gl2 .GridArray .GetLength(0);
                  if (gl2 .GridArray .GetLength(1) > maxcol)
                        maxcol = gl2 .GridArray .GetLength(1);

                  int[,] gridarray = new int[maxrow, maxcol];
                  for (int i = 0; i < maxrow; i++)
                  {
                        for (int j = 0; j < maxcol; j++)
                        {
                              gridarray[i, j] = 0;
                        }
                  }

                  for (int i = 0; i < griddata .Count; i++)
                  {
                        gridarray[griddata[i] .Row, griddata[i] .Col] = 1;
                  }
                  List<GridData> grid = new List<GridData>();
                  for (int i = 0; i < gridarray .GetLength(0); i++)
                  {
                        int v = 1;
                        for (int j = gridarray .GetLength(1) - 1; j >= 0; j--)
                        {
                              if (gridarray[i, j] == 1)
                              {
                                    grid .Add(new GridData(i, j, v));
                                    v++;
                              }
                              else
                              {
                                    v = 1;
                              }
                        }
                  }
                  return new GridLib(grid, gridarray);
                  /*RectangleF r = pc .Rect;
                  List<GridData> griddata = new List<GridData>();
                  List<GridData> griddataX = new List<GridData>();
                  List<GridData> griddataY = new List<GridData>();
                  int maxrow = 0;
                  int maxcol = 0;
                  bool start = false;
                  float startY = r .Y;

                  List<BaseShape> shapelist = new List<BaseShape>();
                  List<PointF> pointlist = new List<PointF>();

                  shapelist .AddRange(pc .Plate1 .OutModel .ListShape .ToList());
                  shapelist .AddRange(pc .Plate2 .OutModel .ListShape .ToList());

                  pointlist .AddRange(pc .Plate1 .OutModel .ListPoint .ToList());
                  pointlist .AddRange(pc .Plate2 .OutModel .ListPoint .ToList());

                  for (int i = 0; i < pc .Plate1 .InnerModel .Count; i++)
                  {
                        shapelist .AddRange(pc .Plate1 .InnerModel[i] .ListShape .ToList());
                        pointlist .AddRange(pc .Plate1 .InnerModel[i] .ListPoint .ToList());
                  }
                  for (int i = 0; i < pc .Plate2 .InnerModel .Count; i++)
                  {
                        shapelist .AddRange(pc .Plate2 .InnerModel[i] .ListShape .ToList());
                        pointlist .AddRange(pc .Plate2 .InnerModel[i] .ListPoint .ToList());
                  }
                  //X扫描
                  for (float t = 0; t + 0.5f * T <= r .Height + T; t += T)
                  {
                        float y = r .Y + t + 0.5f * T;
                        if (y > r .Height)
                        {
                              y = r .Y + r .Height;
                        }
                        List<PointF> plist = new List<PointF>();
                        for (int i = 0; i < shapelist .Count; i++)
                        {
                              plist = IntersectPointX(shapelist[i], y, plist);
                        }
                        plist = DisposeSpecialPointX(pointlist, plist);
                        CompareX comx = new CompareX();
                        plist .Sort(comx);
                        List<GridData> tmp = new List<GridData>();
                        for (int i = 0; i < plist .Count - 1; i = i + 2)//交点两两配对成水平直线
                        {
                              tmp .AddRange(LineToGridX(plist[i], plist[i + 1], new PointF(r .X, 0), T));
                        }
                        griddataX .AddRange(tmp);

                        if (tmp .Count > 0)
                        {
                              if (tmp[0] .Row > maxrow)
                                    maxrow = tmp[0] .Row;
                              if (tmp[tmp .Count - 1] .Col > maxcol)
                                    maxcol = tmp[tmp .Count - 1] .Col;
                        }
                  }
                  //Y扫描
                  for (float t = 0; t + 0.5f * T <= r .Width + T; t += T)
                  {
                        float x = r .X + t + 0.5f * T;
                        if (x > r .Width)
                        {
                              x = r .X + r .Width;
                        }
                        List<PointF> plist = new List<PointF>();
                        for (int i = 0; i < shapelist .Count; i++)
                        {
                              plist = IntersectPointY(shapelist[i], x, plist);
                        }

                        plist = DisposeSpecialPointY(pointlist, plist);
                        CompareY comy = new CompareY();
                        plist .Sort(comy);
                        List<GridData> tmp = new List<GridData>();
                        for (int i = 0; i < plist .Count - 1; i = i + 2)//交点两两配对成水平直线
                        {
                              tmp .AddRange(LineToGridY(plist[i], plist[i + 1], new PointF(0, r .Y), T));
                        }
                        griddataY .AddRange(tmp);

                        if (tmp .Count > 0)
                        {
                              if (tmp[0] .Col > maxcol)
                                    maxcol = tmp[0] .Col;
                              if (tmp[tmp .Count - 1] .Row > maxrow)
                                    maxrow = tmp[tmp .Count - 1] .Row;
                        }
                  }
                  griddata = griddataX .Union(griddataY) .ToList();

                  int[,] gridarray = new int[maxrow + 1, maxcol + 1];
                  for (int i = 0; i < maxrow + 1; i++)
                  {
                        for (int j = 0; j < maxcol + 1; j++)
                        {
                              gridarray[i, j] = 0;
                        }
                  }
                  for (int i = 0; i < griddata .Count; i++)
                  {
                        gridarray[griddata[i] .Row, griddata[i] .Col] = 1;
                  }

                  List<GridData> grid = new List<GridData>();
                  for (int i = 0; i < gridarray .GetLength(0); i++)
                  {
                        int v = 1;
                        for (int j = gridarray .GetLength(1) - 1; j >= 0; j--)
                        {
                              if (gridarray[i, j] == 1)
                              {
                                    grid .Add(new GridData(i, j, v));
                                    v++;
                              }
                              else
                              {
                                    v = 1;
                              }
                        }
                  }
                  return new GridLib(grid, gridarray);*/
            }
            /// <summary>
            /// 处理特殊点，扫描线通过顶点或边,X扫描
            /// </summary>
            /// <returns></returns>
            public Tuple<List<PointF>, List<PointF>> DisposeSpecialPointX(List<PointF> pointlist, List<PointF> plist)
            {
                  List<PointF> line = new List<PointF>();
                  for (int i = plist .Count - 1; i >= 0; i--)
                  {
                        bool find = false;
                        int index = 0;
                        //int last = 0;
                        //int next = 0;
                        for (int j = 0; j < pointlist .Count; j++)
                        {
                              if (Math .Abs(pointlist[j] .Y - plist[i] .Y) < RE
                                    && Math .Abs(pointlist[j] .X - plist[i] .X) < RE)
                              {
                                    find = true;
                                    index = j;
                                    /*if (index == 0)
                                    {
                                          last = pointlist .Count - 1;
                                          next = 1;
                                    }
                                    else if (index == pointlist .Count - 1)
                                    {
                                          last = pointlist .Count - 2;
                                          next = 1;
                                    }
                                    else
                                    {
                                          last = index - 1;
                                          next = index + 1;
                                    }*/
                                    break;
                              }
                        }
                        if (find)
                        {
                              int count = pointlist .Count;
                              int last = (index - 1 + count) % count;
                              int next = (index + 1 + count) % count;
                              int next2 = (index + 2 + count) % count;

                              PointF p = pointlist[index];
                              PointF pl = pointlist[last];
                              PointF pn = pointlist[next];
                              PointF pn2 = pointlist[next2];
                              if (p .Y == pl .Y && line .Contains(pl))//若前一个点与该点构成的线段与扫描线重合，删掉，加入到单独集合中
                              {
                                    line .Add(p);
                                    plist .RemoveAt(i);
                              }
                              else if (p .Y == pn .Y)//与扫描线重合，如果两个端点为极值点，全部删掉，否则删掉一个，加入到单独集合中
                              {
                                    line .Add(p);
                                    if ((pl .Y > p .Y && pn2 .Y > pn .Y)
                                          || (pl .Y < p .Y && pn2 .Y < pn .Y))
                                    {
                                          plist .RemoveAt(i);
                                    }
                              }
                              else if ((pl .Y > p .Y && pn .Y > p .Y)//不重合，为极值点
                                    || (pl .Y < p .Y && pl .Y < p .Y))
                              {
                                    plist .Add(plist[i]);
                              }
                              //其他情况保持不变，记录一次
                        }
                        /*if (find)
                        {
                              //如果相邻的2个顶点都比Y大，则记录两次，都没有比Y大则记录0次
                              int count = (pointlist[last] .Y > pointlist[index] .Y + RE ? 1 : 0)
                                    + (pointlist[next] .Y > pointlist[index] .Y + RE ? 1 : 0);
                              if (count == 0)
                              {
                                    plist .RemoveAt(i);
                              }
                              if (count == 2)
                              {
                                    plist .Add(plist[i]);
                              }
                        }*/
                  }
                  return new Tuple<List<PointF>, List<PointF>>(plist, line);
            }
            /// <summary>
            /// 处理特殊点，扫描线通过顶点或边，Y扫描
            /// </summary>
            /// <returns></returns>
            public Tuple<List<PointF>, List<PointF>> DisposeSpecialPointY(List<PointF> pointlist, List<PointF> plist)
            {
                  /*处理扫描线经过顶点
                   * 1，顶点为极值点，x值同时大于或小于两侧，记录2次
                   * 2，非极值点，记录1次
                   * 3，与扫描线重合，只记录一个点，另外单独处理重合线
                   */
                  List<PointF> line = new List<PointF>();
                  for (int i = plist .Count - 1; i >= 0; i--)
                  {
                        bool find = false;
                        int index = 0;
                        //int last = 0;
                        //int next = 0;
                        for (int j = 0; j < pointlist .Count; j++)
                        {
                              if (Math .Abs(pointlist[j] .X - plist[i] .X) < RE
                                    && Math .Abs(pointlist[j] .Y - plist[i] .Y) < RE)
                              {
                                    find = true;
                                    index = j;
                                    /*if (index == 0)
                                    {
                                          last = pointlist .Count - 1;
                                          next = 1;
                                    }
                                    else if (index == pointlist .Count - 1)
                                    {
                                          last = pointlist .Count - 2;
                                          next = 1;
                                    }
                                    else
                                    {
                                          last = index - 1;
                                          next = index + 1;
                                    }*/
                                    break;
                              }
                        }
                        if (find)
                        {
                              int count = pointlist .Count;
                              int last = (index - 1 + count) % count;
                              int next = (index + 1 + count) % count;
                              int next2 = (index + 2 + count) % count;

                              PointF p = pointlist[index];
                              PointF pl = pointlist[last];
                              PointF pn = pointlist[next];
                              PointF pn2 = pointlist[next2];
                              if (p .X == pl .X && line .Contains(pl))//若前一个点与该点构成的线段与扫描线重合，删掉，加入到单独集合中
                              {
                                    line .Add(p);
                                    plist .RemoveAt(i);
                              }
                              else if (p .X == pn .X)//与扫描线重合，如果两个端点为极值点，全部删掉，否则删掉一个，加入到单独集合中
                              {
                                    line .Add(p);
                                    if ((pl .X > p .X && pn2 .X > pn .X)
                                          || (pl .X < p .X && pn2 .X < pn .X))
                                    {
                                          plist .RemoveAt(i);
                                    }
                              }
                              else if ((pl .X > p .X && pn .X > p .X)//不重合，为极值点
                                    || (pl .X < p .X && pl .X < p .X))
                              {
                                    plist .Add(plist[i]);
                              }
                              //其他情况保持不变，记录一次
                        }
                  }
                  return new Tuple<List<PointF>, List<PointF>>(plist, line);
            }
            /// <summary>
            /// Y方向栅格化线段
            /// </summary>
            /// <param name="line"></param>
            /// <param name="ras"></param>
            /// <returns></returns>
            public List<GridData> LineToGridY(PointF pstart, PointF pend, PointF o, float T) //栅格化水平线段
            {
                  List<GridData> griddate = new List<GridData>();
                  GridData gstart = PointToGrid(pstart, o, T);
                  GridData gend = PointToGrid(pend, o, T);
                  GridData gstart2 = new GridData(gstart .Row, gstart .Col - 1, gstart .Value);
                  GridData gend2 = new GridData(gend .Row, gend .Col - 1, gend .Value);

                  griddate .Add(gstart);

                  if (!(gstart .Row == gend .Row && gstart .Col == gend .Col))
                  {
                        for (int i = 1; i < (gend .Row - gstart .Row); i++)
                        {
                              GridData g = new GridData(gstart .Row + i, gstart .Col, 1);
                              griddate .Add(g);
                        }

                        griddate .Add(gend);
                  }
                  if (gstart2 .Col >= 0)
                  {
                        griddate .Add(gstart2);
                        if (!(gstart2 .Row == gend2 .Row && gstart2 .Col == gend2 .Col))
                        {
                              for (int i = 1; i < (gend2 .Row - gstart2 .Row); i++)
                              {
                                    GridData g = new GridData(gstart2 .Row + i, gstart2 .Col, 1);
                                    griddate .Add(g);
                              }
                              griddate .Add(gend2);
                        }
                  }
                  return griddate;
            }
            /// <summary>
            /// X方向栅格化线段
            /// </summary>
            /// <param name="line"></param>
            /// <param name="ras"></param>
            /// <returns></returns>
            public List<GridData> LineToGridX(PointF pstart, PointF pend, PointF o, float T) //栅格化水平线段
            {
                  List<GridData> griddate = new List<GridData>();
                  GridData gstart = PointToGrid(pstart, o, T);
                  GridData gend = PointToGrid(pend, o, T);
                  GridData gstart2 = new GridData(gstart .Row - 1, gstart .Col, gstart .Value);
                  GridData gend2 = new GridData(gend .Row - 1, gend .Col, gend .Value);
                  griddate .Add(gstart);
                  if (!(gstart .Row == gend .Row && gstart .Col == gend .Col))
                  {
                        for (int i = 1; i < (gend .Col - gstart .Col); i++)
                        {
                              GridData g = new GridData(gstart .Row, gstart .Col + i, 1);
                              griddate .Add(g);
                        }
                        griddate .Add(gend);
                  }
                  if (gstart2 .Row >= 0)
                  {
                        griddate .Add(gstart2);

                        if (!(gstart2 .Row == gend2 .Row && gstart2 .Col == gend2 .Col))
                        {
                              for (int i = 1; i < (gend2 .Col - gstart2 .Col); i++)
                              {
                                    GridData g = new GridData(gstart2 .Row, gstart2 .Col + i, 1);
                                    griddate .Add(g);
                              }

                              griddate .Add(gend2);
                        }
                  }
                  return griddate;
            }
            /// <summary>
            /// 栅格化点
            /// </summary>
            /// <param name="pt"></param>
            /// <param name="ras"></param>
            /// <returns></returns>
            public GridData PointToGrid(PointF pt, PointF o, float T)//栅格化点
            {
                  int row = (int)((pt .Y - o .Y) / T);
                  int col = (int)((pt .X - o .X) / T);
                  GridData gd = new GridData(row, col, 1);
                  return gd;
            }
            /// <summary>
            /// Y扫描线与图形的交点
            /// </summary>
            /// <param name="bs"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public List<PointF> IntersectPointY(BaseShape bs, float x, List<PointF> plist)
            {
                  float y = 0;
                  switch (bs .ShapeClass)
                  {
                        case "Line":
                              Line line = (Line)bs;
                              //重合
                              if (Math .Abs(line .StartPoint .X - x) < RE && Math .Abs(line .EndPoint .X - x) < RE)
                              {
                                    if (!IfExists(plist, line .StartPoint))
                                    {
                                          plist .Add(line .StartPoint);
                                    }
                                    if (!IfExists(plist, line .EndPoint))
                                    {
                                          plist .Add(line .EndPoint);
                                    }
                              }
                              else if (Math .Abs(line .StartPoint .X - line .EndPoint .X) < RE)
                              {
                                    //与Y轴平行，没有交点
                              }
                              else if ((x - line .StartPoint .X < -RE && x - line .EndPoint .X < -RE)
                                    || (x - line .StartPoint .X > RE && x - line .EndPoint .X > RE))
                              {
                                    //在线段起止点外
                              }
                              else if (Math .Abs(line .StartPoint .Y - line .EndPoint .Y) < RE)
                              {
                                    //垂直
                                    if (!IfExists(plist, new PointF(x, line .StartPoint .Y)))
                                    {
                                          plist .Add(new PointF(x, line .StartPoint .Y));
                                    }
                              }
                              else
                              {
                                    float xielv = (line .EndPoint .Y - line .StartPoint .Y) / (line .EndPoint .X - line .StartPoint .X);
                                    y = (x - line .StartPoint .X) * xielv + line .StartPoint .Y;
                                    if (!IfExists(plist, new PointF(x, y)))
                                    {
                                          plist .Add(new PointF(x, y));
                                    }
                              }
                              break;
                        case "Circle":
                              Circle circle = (Circle)bs;
                              if (Math .Pow(circle .Radius, 2) - Math .Pow(x - circle .CenterPoint .X, 2) < 0)
                              {
                                    //无交点
                              }
                              else
                              {
                                    float ctmp = (float)(Math .Sqrt(Math .Pow(circle .Radius, 2) - Math .Pow(x - circle .CenterPoint .X, 2)));
                                    if (ctmp < RE)
                                    {
                                          if (!IfExists(plist, new PointF(x, circle .CenterPoint .Y)))
                                          {
                                                plist .Add(new PointF(x, circle .CenterPoint .Y));
                                          }
                                    }
                                    else
                                    {
                                          float y1 = -ctmp + circle .CenterPoint .Y;
                                          float y2 = ctmp + circle .CenterPoint .Y;
                                          if (!IfExists(plist, new PointF(x, y1)))
                                          {
                                                plist .Add(new PointF(x, y1));
                                          }
                                          if (!IfExists(plist, new PointF(x, y2)))
                                          {
                                                plist .Add(new PointF(x, y2));
                                          }
                                    }
                              }
                              break;
                        case "Arc":
                              Arc arc = (Arc)bs;
                              if (Math .Pow(arc .Radius, 2) - Math .Pow(x - arc .CenterPoint .X, 2) < 0)
                              {
                                    //无交点
                              }
                              else
                              {
                                    float atmp = (float)(Math .Sqrt(Math .Pow(arc .Radius, 2) - Math .Pow(x - arc .CenterPoint .X, 2)));
                                    if (atmp < RE)
                                    {
                                          y = arc .CenterPoint .Y;
                                          if (JudgePointOnArc(new PointF(x, y), arc))
                                          {
                                                if (!IfExists(plist, new PointF(x, y)))
                                                {
                                                      plist .Add(new PointF(x, y));
                                                }
                                          }
                                    }
                                    else
                                    {
                                          float y1 = -atmp + arc .CenterPoint .Y;
                                          float y2 = atmp + arc .CenterPoint .Y;

                                          if (JudgePointOnArc(new PointF(x, y1), arc))
                                          {
                                                if (!IfExists(plist, new PointF(x, y1)))
                                                {
                                                      plist .Add(new PointF(x, y1));
                                                }
                                          }
                                          if (JudgePointOnArc(new PointF(x, y2), arc))
                                          {
                                                if (!IfExists(plist, new PointF(x, y2)))
                                                {
                                                      plist .Add(new PointF(x, y2));
                                                }
                                          }
                                    }
                              }
                              break;
                        case "Ellipse":
                              Ellipse ellipse = (Ellipse)bs;
                              float newB = x - ellipse .CenterPoint .X;
                              float angle = (float)(ellipse .Angle * (Math .PI / 180));
                              float a = (float)(Math .Pow(ellipse .ShortRadius * Math .Cos(angle), 2) + Math .Pow(ellipse .LongRadius * Math .Sin(angle), 2));
                              float b = (float)(2 * newB * (Math .Pow(ellipse .ShortRadius, 2) * Math .Sin(angle) * Math .Cos(angle) + Math .Pow(ellipse .LongRadius, 2) * Math .Cos(angle) * (-Math .Sin(angle))));
                              float c = (float)(newB * newB * (Math .Pow(ellipse .ShortRadius * Math .Sin(angle), 2) + Math .Pow(ellipse .LongRadius * Math .Cos(angle), 2)) - Math .Pow(ellipse .LongRadius * ellipse .ShortRadius, 2));
                              float judge = b * b - (4 * a * c);
                              if (judge >= 0)
                              {
                                    if (judge < RE)
                                    {
                                          y = (float)((-b) / (2 * a) + ellipse .CenterPoint .Y);
                                          if (JudgePointOnEllipseArc(new PointF(x, y), ellipse))
                                          {
                                                if (!IfExists(plist, new PointF(x, y)))
                                                {
                                                      plist .Add(new PointF(x, y));
                                                }
                                          }
                                    }
                                    else
                                    {
                                          float y1 = (float)((-b - Math .Sqrt(judge)) / (2 * a) + ellipse .CenterPoint .Y);
                                          float y2 = (float)((-b + Math .Sqrt(judge)) / (2 * a) + ellipse .CenterPoint .Y);
                                          if (JudgePointOnEllipseArc(new PointF(x, y1), ellipse))
                                          {
                                                if (!IfExists(plist, new PointF(x, y1)))
                                                {
                                                      plist .Add(new PointF(x, y1));
                                                }
                                          }
                                          if (JudgePointOnEllipseArc(new PointF(x, y2), ellipse))
                                          {
                                                if (!IfExists(plist, new PointF(x, y2)))
                                                {
                                                      plist .Add(new PointF(x, y2));
                                                }
                                          }
                                    }
                              }
                              break;
                        default:
                              break;
                  }

                  return plist;
            }
            /// <summary>
            /// X扫描线与图形的交点
            /// </summary>
            /// <param name="bs"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public List<PointF> IntersectPointX(BaseShape bs, float y, List<PointF> plist)
            {
                  float x = 0;
                  switch (bs .ShapeClass)
                  {
                        case "Line":
                              Line line = (Line)bs;
                              //重合
                              if (Math .Abs(line .StartPoint .Y - y) < RE && Math .Abs(line .EndPoint .Y - y) < RE)
                              {
                                    if (!IfExists(plist, line .StartPoint))
                                    {
                                          plist .Add(line .StartPoint);
                                    }
                                    if (!IfExists(plist, line .EndPoint))
                                    {
                                          plist .Add(line .EndPoint);
                                    }
                              }
                              else if (Math .Abs(line .StartPoint .Y - line .EndPoint .Y) < RE)
                              {
                                    //与x轴平行，没有交点
                              }
                              else if ((y - line .StartPoint .Y < -RE && y - line .EndPoint .Y < -RE)
                                    || (y - line .StartPoint .Y > RE && y - line .EndPoint .Y > RE))
                              {
                                    //在线段起止点外
                              }
                              else if (Math .Abs(line .StartPoint .X - line .EndPoint .X) < RE)
                              {
                                    //垂直
                                    if (!IfExists(plist, new PointF(line .StartPoint .X, y)))
                                    {
                                          plist .Add(new PointF(line .StartPoint .X, y));
                                    }
                              }
                              else
                              {
                                    float xielv = (line .EndPoint .Y - line .StartPoint .Y) / (line .EndPoint .X - line .StartPoint .X);
                                    x = (y - line .StartPoint .Y) / xielv + line .StartPoint .X;
                                    if (!IfExists(plist, new PointF(x, y)))
                                    {
                                          plist .Add(new PointF(x, y));
                                    }
                              }
                              break;
                        case "Circle":
                              Circle circle = (Circle)bs;
                              if (Math .Pow(circle .Radius, 2) - Math .Pow(y - circle .CenterPoint .Y, 2) < 0)
                              {
                                    //无交点
                              }
                              else
                              {
                                    float ctmp = (float)(Math .Sqrt(Math .Pow(circle .Radius, 2) - Math .Pow(y - circle .CenterPoint .Y, 2)));
                                    if (ctmp < RE)
                                    {
                                          if (!IfExists(plist, new PointF(circle .CenterPoint .X, y)))
                                          {
                                                plist .Add(new PointF(circle .CenterPoint .X, y));
                                          }
                                    }
                                    else
                                    {
                                          float x1 = -ctmp + circle .CenterPoint .X;
                                          float x2 = ctmp + circle .CenterPoint .X;
                                          if (!IfExists(plist, new PointF(x1, y)))
                                          {
                                                plist .Add(new PointF(x1, y));
                                          }
                                          if (!IfExists(plist, new PointF(x2, y)))
                                          {
                                                plist .Add(new PointF(x2, y));
                                          }
                                    }
                              }
                              break;
                        case "Arc":
                              Arc arc = (Arc)bs;
                              if (Math .Pow(arc .Radius, 2) - Math .Pow(y - arc .CenterPoint .Y, 2) < 0)
                              {
                                    //无交点
                              }
                              else
                              {
                                    float atmp = (float)(Math .Sqrt(Math .Pow(arc .Radius, 2) - Math .Pow(y - arc .CenterPoint .Y, 2)));
                                    if (atmp < RE)
                                    {
                                          x = arc .CenterPoint .X;
                                          if (JudgePointOnArc(new PointF(x, y), arc))
                                          {
                                                if (!IfExists(plist, new PointF(x, y)))
                                                {
                                                      plist .Add(new PointF(x, y));
                                                }
                                          }
                                    }
                                    else
                                    {
                                          float x1 = -atmp + arc .CenterPoint .X;
                                          float x2 = atmp + arc .CenterPoint .X;

                                          if (JudgePointOnArc(new PointF(x1, y), arc))
                                          {
                                                if (!IfExists(plist, new PointF(x1, y)))
                                                {
                                                      plist .Add(new PointF(x1, y));
                                                }
                                          }
                                          if (JudgePointOnArc(new PointF(x2, y), arc))
                                          {
                                                if (!IfExists(plist, new PointF(x2, y)))
                                                {
                                                      plist .Add(new PointF(x2, y));
                                                }
                                          }
                                    }
                              }
                              break;
                        case "Ellipse":
                              Ellipse ellipse = (Ellipse)bs;
                              float newB = y - ellipse .CenterPoint .Y;
                              float angle = (float)(ellipse .Angle * (Math .PI / 180));
                              float a = (float)(Math .Pow(ellipse .ShortRadius * Math .Cos(angle), 2) + Math .Pow(ellipse .LongRadius * Math .Sin(angle), 2));
                              float b = (float)(2 * newB * (Math .Pow(ellipse .ShortRadius, 2) * Math .Sin(angle) * Math .Cos(angle) + Math .Pow(ellipse .LongRadius, 2) * Math .Cos(angle) * (-Math .Sin(angle))));
                              float c = (float)(newB * newB * (Math .Pow(ellipse .ShortRadius * Math .Sin(angle), 2) + Math .Pow(ellipse .LongRadius * Math .Cos(angle), 2)) - Math .Pow(ellipse .LongRadius * ellipse .ShortRadius, 2));
                              float judge = b * b - (4 * a * c);
                              if (judge >= 0)
                              {
                                    if (judge < RE)
                                    {
                                          x = (float)((-b) / (2 * a) + ellipse .CenterPoint .X);
                                          if (JudgePointOnEllipseArc(new PointF(x, y), ellipse))
                                          {
                                                if (!IfExists(plist, new PointF(x, y)))
                                                {
                                                      plist .Add(new PointF(x, y));
                                                }
                                          }
                                    }
                                    else
                                    {
                                          float x1 = (float)((-b - Math .Sqrt(judge)) / (2 * a) + ellipse .CenterPoint .X);
                                          float x2 = (float)((-b + Math .Sqrt(judge)) / (2 * a) + ellipse .CenterPoint .X);
                                          if (JudgePointOnEllipseArc(new PointF(x1, y), ellipse))
                                          {
                                                if (!IfExists(plist, new PointF(x1, y)))
                                                {
                                                      plist .Add(new PointF(x1, y));
                                                }
                                          }
                                          if (JudgePointOnEllipseArc(new PointF(x2, y), ellipse))
                                          {
                                                if (!IfExists(plist, new PointF(x2, y)))
                                                {
                                                      plist .Add(new PointF(x2, y));
                                                }
                                          }
                                    }
                              }
                              break;
                        default:
                              break;
                  }

                  return plist;
            }
            /// <summary>
            /// 判断点是否已经存在
            /// </summary>
            /// <param name="plist"></param>
            /// <param name="p"></param>
            /// <returns></returns>
            public bool IfExists(List<PointF> plist, PointF p)
            {
                  for (int i = 0; i < plist .Count; i++)
                  {
                        if (Math .Abs(plist[i] .X - p .X) < RE && Math .Abs(plist[i] .Y - p .Y) < RE)
                        {
                              return true;
                        }
                  }
                  return false;
            }
            /// <summary>
            /// 按x排序
            /// </summary>
            public class CompareX : IComparer<PointF>//X值排序类
            {
                  public int Compare(PointF pt1, PointF pt2)
                  {
                        return pt1 .X .CompareTo(pt2 .X);// Compare(pt1.X, pt2.X);
                  }
            }
            /// <summary>
            /// 按y排序
            /// </summary>
            public class CompareY : IComparer<PointF>//Y值排序类
            {
                  public int Compare(PointF pt1, PointF pt2)
                  {
                        return pt1 .Y .CompareTo(pt2 .Y);// Compare(pt1.X, pt2.X);
                  }
            }
            /// <summary>
            /// 判断点是否在圆弧上
            /// </summary>
            /// <param name="p"></param>
            /// <param name="arc"></param>
            /// <returns></returns>
            private bool JudgePointOnArc(PointF p, Arc arc)
            {
                  double a = 0;
                  if (p .X == arc .CenterPoint .X && p .Y > arc .CenterPoint .Y)
                  {
                        a = 90;
                  }
                  else if (p .X == arc .CenterPoint .X && p .Y < arc .CenterPoint .Y)
                  {
                        a = 270;
                  }
                  else if (p .X < arc .CenterPoint .X)
                  {
                        a = 180 + Math .Atan((p .Y - arc .CenterPoint .Y) / (p .X - arc .CenterPoint .X)) * 180 / Math .PI;
                  }
                  else if (p .X > arc .CenterPoint .X && p .Y < arc .CenterPoint .Y)
                  {
                        a = 360 + Math .Atan((p .Y - arc .CenterPoint .Y) / (p .X - arc .CenterPoint .X)) * 180 / Math .PI;
                  }
                  else
                  {
                        a = Math .Atan((p .Y - arc .CenterPoint .Y) / (p .X - arc .CenterPoint .X)) * 180 / Math .PI;
                  }

                  if (arc .StartAngle > arc .EndAngle)
                  {
                        if (a > arc .StartAngle || a < arc .EndAngle)
                        {
                              return true;
                        }
                        return false;
                  }
                  else
                  {
                        if (a > arc .StartAngle && a < arc .EndAngle)
                        {
                              return true;
                        }
                        return false;
                  }
            }
            /// <summary>
            /// 判断点是否在椭圆圆弧上
            /// </summary>
            /// <param name="p"></param>
            /// <param name="arc"></param>
            /// <returns></returns>
            private bool JudgePointOnEllipseArc(PointF p, Ellipse arc)
            {
                  float angle = (float)((180 / Math .PI) * Math .Atan2(p .Y - arc .CenterPoint .Y, p .X - arc .CenterPoint .X));
                  angle = angle < 0 ? 360 + angle : angle;

                  //当前椭圆起点，相对于椭圆圆心的角度
                  float startAngle = arc .StartAngle + arc .Angle > 360 ? arc .StartAngle + arc .Angle - 360 : arc .StartAngle + arc .Angle;
                  //当前椭圆终点，相对于椭圆圆心的角度
                  float endAngle = arc .EndAngle + arc .Angle > 360 ? arc .EndAngle + arc .Angle - 360 : arc .EndAngle + arc .Angle;

                  if (startAngle < endAngle)
                  {
                        if (angle >= startAngle && angle <= endAngle)
                        {
                              return true;
                        }
                        return false;
                  }
                  else if (startAngle >= endAngle)
                  {
                        if ((angle >= startAngle && angle <= 360) || (angle >= 0 && angle <= endAngle))
                        {
                              return true;
                        }
                        return false;
                  }
                  else
                  {
                        //不存在交点
                        return false;
                  }
            }
      }
      public class GridData
      {
            public int Row { get; set; }
            public int Col { get; set; }
            public int Value { get; set; }
            //public int ValueR { get; set; }
            public GridData(int row, int col, int value)
            {
                  Row = row;
                  Col = col;
                  Value = value;
            }

      }

      public class GridLib
      {
            public List<GridData> Grid { get; set; }
            public int[,] GridArray { get; set; }
            public GridLib()
            {

            }
            public GridLib(List<GridData> grid, int[,] gridarray)
            {
                  GridArray = gridarray;
                  Grid = grid;
            }
      }
}
