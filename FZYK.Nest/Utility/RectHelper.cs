using myCad .Model;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Runtime .InteropServices;

namespace FZYK .Nest
{
      public class RectHelper
      {
            #region 卡壳算法计算最小包络矩形
            public Dictionary<string, object> MinRect(List<PointF> p)
            {
                  List<PointF> pConvexhull = GetConvexHull(p .ToList());
                  //计算最小矩形
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic = Rotating(pConvexhull);
                  dic .Add("convexhull", pConvexhull);
                  return dic;
            }
            /// <summary>
            /// 旋转卡壳
            /// </summary>
            /// <param name="ch"></param>
            /// <returns></returns>
            public Dictionary<string, object> Rotating(List<PointF> ch)
            {
                  float height = 0f;
                  float length = 0f;
                  float area = -1f;
                  float angle = 0f;
                  float angle2 = 0f;
                  int m;
                  m = ch .Count;
                  int i = 1, j = 1, k = 1;
                  for (int t = 2; t < m; t++)
                        if (Dis(ch[t], ch[0], ch[1]) >= Dis(ch[k], ch[0], ch[1]) || t == 2)
                              k = t;
                  for (int t = 2; t < m; t++)
                        if (Dot(Sub(ch[1], ch[0]), Sub(ch[t], ch[1])) >= Dot(Sub(ch[1], ch[0]), Sub(ch[i], ch[1])) || t == 2)
                              i = t;
                  for (int t = 2; t < m; t++)
                        if (Dot(Sub(ch[0], ch[1]), Sub(ch[t], ch[0])) >= Dot(Sub(ch[0], ch[1]), Sub(ch[j], ch[0])) || t == 2)
                              // ch[0]-ch[1] ch[t]-ch[1] 的点积最大，那么投影在向量上的长度就最大。 
                              j = t;
                  float h = Dis(ch[k], ch[0], ch[1]);
                  float l = Len(Sub(ch[0], ch[1])) + Math .Abs(Dot(Sub(ch[1], ch[0]), Sub(ch[i], ch[1]))) / Len(Sub(ch[0], ch[1])) + Math .Abs(Dot(Sub(ch[0], ch[1]), Sub(ch[j], ch[0]))) / Len(Sub(ch[0], ch[1]));
                  if (Max(area, h * l))
                  {
                        height = h;
                        length = l;
                        area = h * l;
                        angle = Angle(ch[0], ch[1]);
                  }
                  ch .Add(ch[0]);
                  //ch[m] = ch[0];
                  for (int t = 1; t < m; t++)
                  {
                        while (Dis(ch[k], ch[t], ch[t + 1]) <= Dis(ch[(k + 1) % m], ch[t], ch[t + 1]))
                              k = (k + 1) % m;
                        if (i == t + 1)
                              i = (i + 1) % m;
                        while (Dot(Sub(ch[t + 1], ch[t]), Sub(ch[i], ch[t + 1])) <= Dot(Sub(ch[t + 1], ch[t]), Sub(ch[(i + 1) % m], ch[t + 1])))
                              i = (i + 1) % m;
                        while (Dot(Sub(ch[t], ch[t + 1]), Sub(ch[j], ch[t])) <= Dot(Sub(ch[t], ch[t + 1]), Sub(ch[(j + 1) % m], ch[t])))
                              j = (j + 1) % m;
                        if (j == t)
                              j = ((j - 1) % m + m) % m;
                        h = Dis(ch[k], ch[t], ch[t + 1]);
                        l = Len(Sub(ch[t], ch[t + 1])) + Math .Abs(Dot(Sub(ch[t + 1], ch[t]), Sub(ch[i], ch[t + 1]))) / Len(Sub(ch[t], ch[t + 1])) + Math .Abs(Dot(Sub(ch[t], ch[t + 1]), Sub(ch[j], ch[t]))) / Len(Sub(ch[t], ch[t + 1]));
                        if (Max(area, h * l))
                        {
                              height = h;
                              length = l;
                              area = h * l;
                              angle = Angle(ch[t], ch[t + 1]);
                        }
                  }
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("angle", angle);
                  dic .Add("angle2", angle2);
                  dic .Add("area", area);
                  dic .Add("length", length);
                  dic .Add("height", height);

                  return dic;
            }

            #endregion
            #region 中位线算法计算最小包络平行四边形
            public Dictionary<string, object> MinParallelogram(List<PointF> p)
            {
                  List<PointF> pConvexhull = GetConvexHull(p .ToList());
                  //计算最小平行四边形
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic = Median(pConvexhull);
                  dic .Add("convexhull", pConvexhull);
                  return dic;
            }
            /// <summary>
            /// 中位线法求最小平行四边形，因只处理合并图形，图形为对称，偶数个顶点和边
            /// </summary>
            /// <param name="plist"></param>
            public Dictionary<string, object> Median(List<PointF> ch)
            {
                  float height = 0f;
                  float length = 0f;
                  float area = -1f;
                  float angle = 0f;
                  float angle2 = 0f;
                  int n = ch .Count;
                  List<int> indexlist = new List<int>();//已经和其他边组成平行四边形，就跳过搜索
                  float mins = -1;
                  for (int i = 0; i < n / 2; i++)
                  {
                        if (indexlist .Contains(i))
                              continue;
                        PointF p0 = ch[i];
                        PointF p1 = ch[i + 1];
                        PointF p2 = ch[(i + n / 2) % n];//交叉对应
                        PointF p3 = ch[(i + 1 + n / 2) % n];

                        PointF m0 = new PointF((p0 .X + p3 .X) / 2, (p0 .Y + p3 .Y) / 2);//中位线上两点坐标
                        PointF m1 = new PointF((p1 .X + p2 .X) / 2, (p1 .Y + p2 .Y) / 2);

                        float cos = 1f;
                        PointF a0 = new PointF(0, 0);
                        PointF a1 = new PointF(0, 0);
                        PointF a2 = new PointF(0, 0);
                        PointF a3 = new PointF(0, 0);
                        int index = 0;
                        for (int j = 0; j < n; j++)//找出与中位线相交的边
                        {
                              int jnext = (j + 1) % n;
                              if (Intersect(m0, m1, ch[j], ch[jnext]))
                              {
                                    if (AngleCos(p0, p1, ch[j], ch[jnext]) < cos)
                                    {
                                          cos = AngleCos(p0, p1, ch[j], ch[jnext]);
                                          a0 = ch[j];
                                          a1 = ch[jnext];
                                          a2 = ch[(j + n / 2) % n];
                                          a3 = ch[(jnext + n / 2) % n];
                                          index = j;
                                    }
                              }
                        }
                        indexlist .Add(index);
                        Vector p0p1 = Sub(p0, p1);
                        Vector a0a1 = Sub(a0, a1);
                        float s = Math .Abs(Dis(p2, p0, p1) * Dis(a2, a0, a1) / (Cross(p0p1, a0a1) / Len(p0p1) / Len(a0a1)));
                        float h1 = Dis(p2, p0, p1);
                        float h2 = Dis(a2, a0, a1);
                        if (mins < 0 || s < mins)
                        {
                              mins = s;
                              area = mins;
                              angle = Angle(ch[i], ch[i + 1]);
                              angle2 = Angle(ch[index], ch[(index + 1) % n]);
                              height = Math .Min(h1, h2);
                              length = Math .Max(h1, h2);
                        }
                  }
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("angle", angle);
                  dic .Add("angle2", angle2);
                  dic .Add("area", area);
                  dic .Add("length", length);
                  dic .Add("height", height);
                  return dic;
            }
            #endregion
            #region 多边形获取凸包
            /// <summary>
            /// 删除凹点，获取凸包(逆时针)【注意有的凸点不在凸包上】
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public List<PointF> GetConvexHull(List<PointF> plist)
            {
                  List<PointF> p = plist .ToList();
                  int i = 0;
                  bool ao = false;
                  while (i < p .Count)
                  {
                        PointF p0 = (i == 0) ? p[p .Count - 1] : p[i - 1];
                        PointF p1 = p[i];
                        PointF p2 = (i == p .Count - 1) ? p[0] : p[i + 1];
                        double tmp = Cross(Sub(p1, p0), Sub(p2, p1));
                        if (tmp <= 0)
                        {
                              ao = true;
                              p .RemoveAt(i);
                        }
                        else
                              i++;

                        if (i == p .Count && ao)
                        {
                              //如果本轮循环中存在凹点，则有可能存在凸点不在凸包上的情况，重新循环判断
                              i = 0;
                              ao = false;
                        }
                  }
                  return p;
            }
            #endregion
            #region 向量
            /// <summary>
            /// 向量
            /// </summary>
            [StructLayout(LayoutKind .Sequential)]
            public struct Vector
            {
                  public float X { get; set; }
                  public float Y { get; set; }

                  public Vector(float x, float y)
                  {
                        X = x;
                        Y = y;
                  }

            }
            #endregion
            #region 向量运算
            public Vector Sub(PointF a, PointF b)
            {
                  return new Vector(a .X - b .X, a .Y - b .Y);
            }
            public Vector Sub(Vector a, Vector b)
            {
                  return new Vector(a .X - b .X, a .Y - b .Y);
            }
            public float Cross(Vector a, Vector b)
            {
                  return a .X * b .Y - a .Y * b .X;
            }
            public float Dot(Vector a, Vector b)
            {
                  return a .X * b .X + a .Y * b .Y;
            }
            public PointF PointMove(PointF p, Vector v)
            {
                  return new PointF(p .X + v .X, p .Y + v .Y);
            }
            public Vector Unit(Vector a)
            {
                  float r = Len(a);
                  Vector v = new Vector(a .X / r, a .Y / r);
                  return v;
            }
            public float Len(Vector a)
            {
                  return Convert .ToSingle(Math .Sqrt(a .X * a .X + a .Y * a .Y));
            }
            public float Dis(PointF p, PointF a, PointF b)
            {
                  Vector v = Sub(p, a);
                  Vector u = Sub(b, a);
                  return Math .Abs(Cross(v, u)) / Len(u);
            }
            /// <summary>
            /// 判断p点在ab的左侧还是右侧
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <param name="a"></param>
            /// <returns></returns>
            public float Side(PointF a, PointF b, PointF p)
            {
                  Vector v = Sub(p, a);
                  Vector u = Sub(b, a);
                  return Cross(v, u);
            }
            /// <summary>
            /// 判断直线ab，是否与线段p1p2相交
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <returns></returns>
            public bool Intersect(PointF a, PointF b, PointF p1, PointF p2)
            {
                  float s1 = Side(a, b, p1);
                  float s2 = Side(a, b, p2);
                  if (s1 * s2 <= 0)
                        return true;
                  return false;
            }
            public bool Max(float a, float b)
            {
                  if (a < 0)
                        return true;
                  else if (b < 0)
                        return false;
                  else
                        return a > b;
            }
            public float Angle(PointF a, PointF b)
            {
                  if (a .X == b .X)
                  {
                        return 90;
                  }
                  else
                  {
                        return Convert .ToSingle(Math .Atan((b .Y - a .Y) / (b .X - a .X)) * 180 / Math .PI);
                        //return Convert .ToSingle(Math .Acos((b .X - a .X) / Math .Sqrt((b .Y - a .Y) * (b .Y - a .Y) + (b .X - a .X) * (b .X - a .X))) * 180 / Math .PI);
                  }
            }
            /// <summary>
            /// 夹角余弦0-90，90度时为0
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public float AngleCos(PointF a, PointF b, PointF p1, PointF p2)
            {
                  Vector u = Sub(a, b);
                  Vector v = Sub(p1, p2);
                  return Math .Abs(Dot(u, v) / Len(u) / Len(v));
            }
            #endregion
            #region 扩大
            /// <summary>
            /// 扩大
            /// </summary>
            /// <param name="pm"></param>
            /// <param name="dis"></param>
            /// <returns></returns>
            public PlateModel Expand(PlateModel pm, float dis)
            {
                  List<PointF> p = new List<PointF>();
                  p = Expand(pm .OutModel .ListPoint, dis);
                  List<BaseShape> bs = new List<BaseShape>();
                  int count = p .Count;
                  for (int i = 0; i < p .Count; i++)
                  {
                        myCad .Shape .Line l = new myCad .Shape .Line(p[i], p[(i + 1) % count]);
                        bs .Add(l);
                  }
                  pm .OutModel .ExpandPoint = p;
                  pm .OutModel .ExpandShape = bs;
                  return pm;
            }
            public List<PointF> Expand(List<PointF> p, float dis)
            {
                  List<PointF> pnew = new List<PointF>();
                  int count = p .Count;
                  List<Vector> vlist = new List<Vector>();
                  List<Vector> vUnitlist = new List<Vector>();

                  for (int i = 0; i < count; i++)
                  {
                        PointF pnext = p[(i + 1) % count];
                        PointF plast = p[(i - 1 + count) % count];
                        Vector v = Sub(pnext, p[i]);
                        vlist .Add(v);
                  }
                  for (int i = 0; i < count; i++)
                  {
                        vUnitlist .Add(Unit(vlist[i]));
                  }
                  for (int i = 0; i < count; i++)
                  {
                        int startindex = (i - 1 + count) % count;
                        int endindex = i;
                        float sina = Cross(vUnitlist[startindex], vUnitlist[endindex]);
                        float len = dis / sina;
                        Vector v = Sub(vUnitlist[endindex], vUnitlist[startindex]);
                        PointF p2 = PointMove(p[i], new Vector(v .X * len * -1, v .Y * len * -1));
                        pnew .Add(p2);
                  }
                  return pnew;
            }
            #endregion
            #region 合并凸包
            /*
            /// <summary>
            /// 上下合并凸包,原凸包是逆时针
            /// </summary>
            /// <param name="ch1"></param>
            /// <param name="ch2"></param>
            /// <returns></returns>

            public List<PointF> ConvexHullCombineY(List<PointF> chup, List<PointF> chdown)
            {
                  List<PointF> ch1 = chup .ToList();
                  List<PointF> ch2 = chdown .ToList();
                  float ymax = -99999;
                  float ymin = 99999;

                  int index_maxy = 0;
                  int index_miny = 0;
                  for (int i = 0; i < ch1 .Count; i++)
                  {
                        if (ch1[i] .Y < ymin)
                        {
                              ymin = ch1[i] .Y;
                              index_miny = i;
                        }
                  }
                  for (int i = 0; i < ch2 .Count; i++)
                  {
                        if (ch2[i] .Y > ymax)
                        {
                              ymax = ch2[i] .Y;
                              index_maxy = i;
                        }
                  }
                  List<int> del1 = new List<int>();
                  List<int> del2 = new List<int>();
                  int m1 = ch1 .Count;
                  int m2 = ch2 .Count;
                  int index1 = index_miny;
                  int index2 = index_maxy;
                  //向左寻找公切线
                  while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) <= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) <= 0
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) >= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) >= 0))
                  {
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) <= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) <= 0))
                        {
                              del2 .Add(index2);
                              index2 = (index2 + m2 + 1) % m2;
                        }
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) >= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) >= 0))
                        {
                              del1 .Add(index1);
                              index1 = (index1 + m1 - 1) % m1;
                        }
                  }
                  index1 = index_maxy;
                  index2 = index_miny;
                  //向右寻找公切线
                  while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) >= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) >= 0
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) <= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) <= 0))
                  {
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) >= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) >= 0))
                        {
                              del2 .Add(index2);
                              index2 = (index2 + m2 - 1) % m2;
                        }
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) <= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) <= 0))
                        {
                              del1 .Add(index1);
                              index1 = (index1 + m1 + 1) % m1;
                        }
                  }
                  //排序去重
                  del1 = del1 .Distinct() .ToList();
                  del1 .Sort();
                  del2 = del2 .Distinct() .ToList();
                  del2 .Sort();
                  //按顺序将凸包上的点集合起来
                  List<PointF> chCombine = new List<PointF>();
                  int maxdel1 = del1[del1 .Count - 1];
                  int start1 = (maxdel1 + m1 + 1) % m1;
                  while (start1 != maxdel1)
                  {
                        if (!del1 .Contains(start1))
                        {
                              chCombine .Add(ch1[start1]);
                        }
                        start1 = (start1 + m1 + 1) % m1;
                  }
                  int maxdel2 = del2[del2 .Count - 1];
                  int start2 = (maxdel2 + m2 + 1) % m2;
                  while (start2 != maxdel2)
                  {
                        if (!del2 .Contains(start2))
                        {
                              chCombine .Add(ch2[start2]);
                        }
                        start2 = (start2 + m2 + 1) % m2;
                  }
                  return chCombine;
            }
            /// <summary>
            /// 左右合并凸包,原凸包是逆时针
            /// </summary>
            /// <param name="ch1"></param>
            /// <param name="ch2"></param>
            /// <returns></returns>
            public List<PointF> ConvexHullCombineX(List<PointF> chleft, List<PointF> chright)
            {
                  List<PointF> ch1 = chleft .ToList();
                  List<PointF> ch2 = chright .ToList();
                  float xmax = -99999;
                  float xmin = 99999;

                  int index_maxx = 0;
                  int index_minx = 0;
                  for (int i = 0; i < ch1 .Count; i++)
                  {
                        if (ch1[i] .X > xmax)
                        {
                              xmax = ch1[i] .X;
                              index_maxx = i;
                        }
                  }
                  for (int i = 0; i < ch2 .Count; i++)
                  {
                        if (ch2[i] .X < xmin)
                        {
                              xmin = ch2[i] .X;
                              index_minx = i;
                        }
                  }
                  List<int> del1 = new List<int>();
                  List<int> del2 = new List<int>();
                  int m1 = ch1 .Count;
                  int m2 = ch2 .Count;
                  int index1 = index_maxx;
                  int index2 = index_minx;
                  //向上寻找公切线
                  while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) >= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) >= 0
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) <= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) <= 0))
                  {
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) >= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) >= 0))
                        {
                              del2 .Add(index2);
                              index2 = (index2 + m2 - 1) % m2;
                        }
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) <= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) <= 0))
                        {
                              del1 .Add(index1);
                              index1 = (index1 + m1 + 1) % m1;
                        }
                  }
                  index1 = index_maxx;
                  index2 = index_minx;
                  //向下寻找
                  while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) <= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) <= 0
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) >= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) >= 0))
                  {
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch1[index1], ch2[index2], Next(ch2, index2)) <= 0
                        && Side(ch1[index1], ch2[index2], Last(ch2, index2)) <= 0))
                        {
                              del2 .Add(index2);
                              index2 = (index2 + m2 + 1) % m2;
                        }
                        while (!(ch1[index1] != ch2[index2]
                        && Side(ch2[index2], ch1[index1], Next(ch1, index1)) >= 0
                        && Side(ch2[index2], ch1[index1], Last(ch1, index1)) >= 0))
                        {
                              del1 .Add(index1);
                              index1 = (index1 + m1 - 1) % m1;
                        }
                  }
                  //排序去重
                  del1 = del1 .Distinct() .ToList();
                  del1 .Sort();
                  del2 = del2 .Distinct() .ToList();
                  del2 .Sort();
                  //按顺序将凸包上的点集合起来
                  List<PointF> chCombine = new List<PointF>();
                  int maxdel1 = del1[del1 .Count - 1];
                  int start1 = (maxdel1 + m1 + 1) % m1;
                  while (start1 != maxdel1)
                  {
                        if (!del1 .Contains(start1))
                        {
                              chCombine .Add(ch1[start1]);
                        }
                        start1 = (start1 + m1 + 1) % m1;
                  }
                  int maxdel2 = del2[del2 .Count - 1];
                  int start2 = (maxdel2 + m2 + 1) % m2;
                  while (start2 != maxdel2)
                  {
                        if (!del2 .Contains(start2))
                        {
                              chCombine .Add(ch2[start2]);
                        }
                        start2 = (start2 + m2 + 1) % m2;
                  }
                  return chCombine;
            }
            */
            /// <summary>
            /// 计算凸包，按逆时针方向存放,Jarvis算法
            /// </summary>
            /// <param name="chleft"></param>
            /// <param name="chright"></param>
            /// <returns></returns>
            public List<PointF> ConvexHullCombine(List<PointF> chleft, List<PointF> chright)
            {
                  List<PointF> ch = new List<PointF>();
                  ch .AddRange(chleft .ToList());
                  ch .AddRange(chright .ToList());
                  int index = 0;
                  float miny = 99999;
                  float minx = 99999;
                  for (int i = 0; i < ch .Count; i++)
                  {
                        if (ch[i] .Y < miny
                              || (ch[i] .Y == miny && ch[i] .X < minx))
                        {
                              index = i;
                              miny = ch[i] .Y;
                              minx = ch[i] .X;
                        }
                  }
                  int m = ch .Count;
                  List<PointF> result = new List<PointF>();
                  PointF pend = ch[index];
                  PointF p0 = ch[index];
                  PointF pright = ch[(index + 1 + m) % m];
                  int rightindex = (index + 1 + m) % m;
                  do
                  {
                        if (ch .Count > 1)
                        {
                              PointF p1 = ch[0];
                              for (int i = 0; i < ch .Count; i++)
                              {
                                    p1 = ch[i];
                                    if (Side(p0, pright, p1) > 0)
                                    {
                                          pright = ch[i];
                                          rightindex = i;
                                    }
                              }
                        }
                        result .Add(pright);
                        ch .RemoveAt(rightindex);
                        p0 = pright;
                        pright = ch[0];
                        rightindex = 0;
                  } while (pend != p0);
                  return result;
            }
            /// <summary>
            /// 循环寻找上一个
            /// </summary>
            /// <param name="list"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            private PointF Last(List<PointF> list, int index)
            {
                  int m = list .Count;
                  int lastindex = (index + m - 1) % m;
                  return list[lastindex];
            }
            /// <summary>
            /// 循环寻找下一个
            /// </summary>
            /// <param name="list"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            private PointF Next(List<PointF> list, int index)
            {
                  int m = list .Count;
                  int nextindex = (index + m + 1) % m;
                  return list[nextindex];
            }
            #endregion
            #region 图形组合

            /// <summary>
            /// 从4个方向进行组合，取出包络矩形面积最小的组合
            /// </summary>
            /// <param name="p"></param>
            /// <param name="ch"></param>
            /// <param name="pnew"></param>
            /// <param name="chnew"></param>
            /// <param name="T"></param>
            /// <returns></returns>
            public Dictionary<string, object> Combine(List<PointF> p, List<PointF> ch, List<PointF> pnew, List<PointF> chnew, float T
                  , float limit, string type)
            {
                  Dictionary<string, object> dicLeft = CombineLeft(p, ch, pnew, chnew, T, limit, type);
                  Dictionary<string, object> dicRight = CombineRight(p, ch, pnew, chnew, T, limit, type);
                  Dictionary<string, object> dicUp = CombineUp(p, ch, pnew, chnew, T, limit, type);
                  Dictionary<string, object> dicDown = CombineDown(p, ch, pnew, chnew, T, limit, type);

                  float minarea = -1;
                  string result = "";
                  if (Convert .ToSingle(dicLeft["area"]) > 0 && (Convert .ToSingle(dicLeft["area"]) < minarea || minarea < 0))
                  {
                        minarea = Convert .ToSingle(dicLeft["area"]);
                        result = "left";
                  }
                  if (Convert .ToSingle(dicRight["area"]) > 0 && (Convert .ToSingle(dicRight["area"]) < minarea || minarea < 0))
                  {
                        minarea = Convert .ToSingle(dicRight["area"]);
                        result = "right";
                  }
                  if (Convert .ToSingle(dicUp["area"]) > 0 && (Convert .ToSingle(dicUp["area"]) < minarea || minarea < 0))
                  {
                        minarea = Convert .ToSingle(dicUp["area"]);
                        result = "up";
                  }
                  if (Convert .ToSingle(dicDown["area"]) > 0 && (Convert .ToSingle(dicDown["area"]) < minarea || minarea < 0))
                  {
                        minarea = Convert .ToSingle(dicDown["area"]);
                        result = "down";
                  }

                  if (result == "left")
                        return dicLeft;
                  else if (result == "right")
                        return dicRight;
                  else if (result == "up")
                        return dicUp;
                  else if (result == "down")
                        return dicDown;
                  return null;
            }
            //左下--左上
            public Dictionary<string, object> CombineLeft(List<PointF> p, List<PointF> ch, List<PointF> pnew, List<PointF> chnew
                  , float T, float limit, string type)
            {
                  //右边界
                  float maxy = -99999;
                  float miny = 99999;
                  float maxx = -99999;
                  float minx = 99999;
                  float rightx = -99999;
                  for (int i = 0; i < ch .Count; i++)
                  {
                        if (maxy < ch[i] .Y)
                        {
                              maxy = ch[i] .Y;
                              maxx = ch[i] .X;
                        }
                        else if (maxy == ch[i] .Y)
                        {
                              maxx = Math .Max(maxx, ch[i] .X);
                        }
                        if (miny > ch[i] .Y)
                        {
                              miny = ch[i] .Y;
                              minx = ch[i] .X;
                        }
                        else if (miny == ch[i] .Y)
                        {
                              minx = Math .Max(minx, ch[i] .X);
                        }
                  }
                  rightx = Math .Max(minx, maxx);
                  //左边界
                  float maxy2 = -99999;
                  float miny2 = 99999;
                  float maxx2 = -99999;
                  float minx2 = 99999;
                  float leftx = 99999;
                  for (int i = 0; i < chnew .Count; i++)
                  {
                        if (maxy2 < chnew[i] .Y)
                        {
                              maxy2 = chnew[i] .Y;
                              maxx2 = chnew[i] .X;
                        }
                        else if (maxy2 == chnew[i] .Y)
                        {
                              maxx2 = Math .Min(maxx2, chnew[i] .X);
                        }
                        if (miny2 > chnew[i] .Y)
                        {
                              miny2 = chnew[i] .Y;
                              minx2 = chnew[i] .X;
                        }
                        else if (miny2 == chnew[i] .Y)
                        {
                              minx2 = Math .Min(minx2, chnew[i] .X);
                        }
                  }
                  leftx = Math .Min(minx2, maxx2);
                  //高度
                  float h = maxy - miny;
                  //最小矩形面积
                  float area = -1;
                  float movex = 0;
                  float movey = 0;
                  float angle = 0;
                  float length = 0f;
                  float height = 0f;
                  List<PointF> chCombine = new List<PointF>();
                  //副图向上移动，计算最小平移距离，再向右移动与主图进行组合，求最小包络矩形
                  for (int i = 1; i < Math .Floor(2 * h / T); i++)
                  {
                        //上边界，下边界
                        float upy = Math .Min(maxy, maxy2 + i * T);
                        float downy = Math .Max(miny, miny2 + i * T);

                        List<PointF> chnew_move = Move(chnew, 0, i * T);
                        List<PointF> pnew_move = Move(pnew, 0, i * T);

                        List<PointF> p1 = p .Where(t => t .Y <= upy && t .Y >= downy && t .X <= rightx) .ToList();
                        List<PointF> p2 = pnew_move .Where(t => t .Y <= upy && t .Y >= downy && t .X >= leftx) .ToList();

                        List<Line> l1 = new List<Line>();
                        List<Line> l2 = new List<Line>();
                        for (int j = 0; j < p .Count; j++)
                        {
                              PointF start = p[j];
                              PointF end = Next(p, j);
                              if ((start .Y <= upy && start .Y >= downy && start .X <= rightx)
                                  || (end .Y <= upy && end .Y >= downy && end .X <= rightx))
                              {
                                    l1 .Add(new Line(start, end));
                              }
                        }
                        for (int j = 0; j < pnew_move .Count; j++)
                        {
                              PointF start = pnew_move[j];
                              PointF end = Next(pnew_move, j);
                              if ((start .Y <= upy && start .Y >= downy && start .X >= leftx)
                                  || (end .Y <= upy && end .Y >= downy && end .X >= leftx))
                              {
                                    l2 .Add(new Line(start, end));
                              }
                        }
                        //计算最小平移距离
                        float mindis = 99999;
                        //p1-->l2
                        for (int j = 0; j < p1 .Count; j++)
                        {
                              for (int k = 0; k < l2 .Count; k++)
                              {
                                    PointF pt = p1[j];
                                    Line le = l2[k];
                                    if ((pt .Y >= le .Start .Y && pt .Y <= le .End .Y)
                                          || (pt .Y <= le .Start .Y && pt .Y >= le .End .Y))
                                    {
                                          float dis = Math .Abs((pt .Y - le .Start .Y) * (le .End .X - le .Start .X) / (le .End .Y - le .Start .Y) + le .Start .X - pt .X);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //p2-->l1
                        for (int j = 0; j < p2 .Count; j++)
                        {
                              for (int k = 0; k < l1 .Count; k++)
                              {
                                    PointF pt = p2[j];
                                    Line le = l1[k];
                                    if ((pt .Y >= le .Start .Y && pt .Y <= le .End .Y)
                                          || (pt .Y <= le .Start .Y && pt .Y >= le .End .Y))
                                    {
                                          float dis = Math .Abs((pt .Y - le .Start .Y) * (le .End .X - le .Start .X) / (le .End .Y - le .Start .Y) + le .Start .X - pt .X);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //副图凸包向右移动与主图组合
                        if (mindis > 0)//图形已加保护量，移动时不考虑
                        {
                              chnew_move = Move(chnew_move, mindis, 0);
                        }
                        chCombine = ConvexHullCombine(chnew_move, ch);
                        if (chCombine .Count == 0)
                        {
                              return null;
                        }
                        Dictionary<string, object> dicCombine = new Dictionary<string, object>();
                        bool accept = false;
                        if (type == "rect")
                        {
                              Dictionary<string, object> dic0 = MinRect(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                    (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "para")
                        {
                              Dictionary<string, object> dic0 = MinParallelogram(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                   (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "mix" || limit > 0)
                        {
                              Dictionary<string, object> dic1 = MinRect(chCombine);
                              Dictionary<string, object> dic2 = MinParallelogram(chCombine);
                              float area1 = Convert .ToSingle(dic1["area"]);
                              float area2 = Convert .ToSingle(dic2["area"]);
                              float length1 = Convert .ToSingle(dic1["length"]);
                              float length2 = Convert .ToSingle(dic2["length"]);
                              float height1 = Convert .ToSingle(dic1["height"]);
                              float height2 = Convert .ToSingle(dic2["height"]);
                              if (limit > 0 && length1 <= limit && height1 <= limit && !(length2 <= limit && height2 <= limit))
                              {
                                    if (area < 0 || area > area1)
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                              }
                              else if (limit > 0 && length2 <= limit && height2 <= limit && !(length1 <= limit && height1 <= limit))
                              {
                                    if (area < 0 || area > area2)
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                              else if (limit < 0
                                   || (limit > 0 && length2 <= limit && height2 <= limit && length1 <= limit && height1 <= limit))
                              {
                                    if (area1 <= area2 && (area < 0 || area > area1))
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                                    else if (area1 <= area2 && (area < 0 || area > area2))
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                        }
                        if (accept)
                        {
                              area = Convert .ToSingle(dicCombine["area"]);
                              movex = mindis;
                              movey = i * T;
                              angle = Convert .ToSingle(dicCombine["angle"]);
                              length = Convert .ToSingle(dicCombine["length"]);
                              height = Convert .ToSingle(dicCombine["height"]);
                              chCombine = (List<PointF>)dicCombine["convexhull"];
                        }
                  }
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("area", area);
                  dic .Add("movex", movex);
                  dic .Add("movey", movey);
                  dic .Add("angle", angle);
                  dic .Add("length", length);
                  dic .Add("height", height);
                  dic .Add("convexhull", chCombine);
                  return dic;
            }
            //右下--右上
            public Dictionary<string, object> CombineRight(List<PointF> p, List<PointF> ch, List<PointF> pnew, List<PointF> chnew
                  , float T, float limit, string type)
            {
                  //左边界
                  float maxy2 = -99999;
                  float miny2 = 99999;
                  float maxx2 = -99999;
                  float minx2 = 99999;
                  float leftx = 99999;
                  float left = 99999;
                  float right = -99999;
                  for (int i = 0; i < ch .Count; i++)
                  {
                        if (maxy2 < ch[i] .Y)
                        {
                              maxy2 = ch[i] .Y;
                              maxx2 = ch[i] .X;
                        }
                        else if (maxy2 == ch[i] .Y)
                        {
                              maxx2 = Math .Min(maxx2, ch[i] .X);
                        }
                        if (miny2 > ch[i] .Y)
                        {
                              miny2 = ch[i] .Y;
                              minx2 = ch[i] .X;
                        }
                        else if (miny2 == ch[i] .Y)
                        {
                              minx2 = Math .Min(minx2, ch[i] .X);
                        }
                        if (right < ch[i] .X)
                        {
                              right = ch[i] .X;
                        }
                        if (left > ch[i] .X)
                        {
                              left = ch[i] .X;
                        }
                  }
                  //高度，宽度
                  float h = maxy2 - miny2;
                  float l = right - left;
                  leftx = Math .Min(minx2, maxx2);
                  //右边界
                  float maxy = -99999;
                  float miny = 99999;
                  float maxx = -99999;
                  float minx = 99999;
                  float rightx = -99999;
                  for (int i = 0; i < chnew .Count; i++)
                  {
                        if (maxy < chnew[i] .Y)
                        {
                              maxy = chnew[i] .Y;
                              maxx = chnew[i] .X;
                        }
                        else if (maxy == chnew[i] .Y)
                        {
                              maxx = Math .Max(maxx, chnew[i] .X);
                        }
                        if (miny > chnew[i] .Y)
                        {
                              miny = chnew[i] .Y;
                              minx = chnew[i] .X;
                        }
                        else if (miny == chnew[i] .Y)
                        {
                              minx = Math .Max(minx, chnew[i] .X);
                        }
                  }
                  rightx = Math .Max(minx, maxx) + 2 * l;

                  //最小矩形面积
                  float area = -1;
                  float movex = 0;
                  float movey = 0;
                  float angle = 0;
                  float length = 0;
                  float height = 0;
                  List<PointF> chCombine = new List<PointF>();
                  //副图向上移动，计算最小平移距离，再向左移动与主图进行组合，求最小包络矩形
                  for (int i = 1; i < Math .Floor(2 * h / T); i++)
                  {
                        //上边界，下边界
                        float upy = Math .Min(maxy + i * T, maxy2);
                        float downy = Math .Max(miny + i * T, miny2);

                        List<PointF> chnew_move = Move(chnew, l * 2, i * T);
                        List<PointF> pnew_move = Move(pnew, l * 2, i * T);

                        List<PointF> p1 = pnew_move .Where(t => t .Y <= upy && t .Y >= downy && t .X <= rightx) .ToList();
                        List<PointF> p2 = p .Where(t => t .Y <= upy && t .Y >= downy && t .X >= leftx) .ToList();

                        List<Line> l1 = new List<Line>();
                        List<Line> l2 = new List<Line>();
                        for (int j = 0; j < pnew_move .Count; j++)
                        {
                              PointF start = pnew_move[j];
                              PointF end = Next(pnew_move, j);
                              if ((start .Y <= upy && start .Y >= downy && start .X <= rightx)
                                  || (end .Y <= upy && end .Y >= downy && end .X <= rightx))
                              {
                                    l1 .Add(new Line(start, end));
                              }
                        }
                        for (int j = 0; j < p .Count; j++)
                        {
                              PointF start = p[j];
                              PointF end = Next(p, j);
                              if ((start .Y <= upy && start .Y >= downy && start .X >= leftx)
                                  || (end .Y <= upy && end .Y >= downy && end .X >= leftx))
                              {
                                    l2 .Add(new Line(start, end));
                              }
                        }
                        //计算最小平移距离
                        float mindis = 99999;
                        //p1-->l2
                        for (int j = 0; j < p1 .Count; j++)
                        {
                              for (int k = 0; k < l2 .Count; k++)
                              {
                                    PointF pt = p1[j];
                                    Line le = l2[k];
                                    if ((pt .Y >= le .Start .Y && pt .Y <= le .End .Y)
                                          || (pt .Y <= le .Start .Y && pt .Y >= le .End .Y))
                                    {
                                          float dis = Math .Abs((pt .Y - le .Start .Y) * (le .End .X - le .Start .X) / (le .End .Y - le .Start .Y) + le .Start .X - pt .X);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //p2-->l1
                        for (int j = 0; j < p2 .Count; j++)
                        {
                              for (int k = 0; k < l1 .Count; k++)
                              {
                                    PointF pt = p2[j];
                                    Line le = l1[k];
                                    if ((pt .Y >= le .Start .Y && pt .Y <= le .End .Y)
                                          || (pt .Y <= le .Start .Y && pt .Y >= le .End .Y))
                                    {
                                          float dis = Math .Abs((pt .Y - le .Start .Y) * (le .End .X - le .Start .X) / (le .End .Y - le .Start .Y) + le .Start .X - pt .X);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //副图凸包向左移动与主图组合
                        if (mindis > 0)
                        {
                              chnew_move = Move(chnew_move, -mindis, 0);
                        }
                        chCombine = ConvexHullCombine(chnew_move, ch);
                        if (chCombine .Count == 0)
                        {
                              return null;
                        }
                        Dictionary<string, object> dicCombine = new Dictionary<string, object>();
                        bool accept = false;
                        if (type == "rect")
                        {
                              Dictionary<string, object> dic0 = MinRect(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                    (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "para")
                        {
                              Dictionary<string, object> dic0 = MinParallelogram(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                   (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "mix" || limit > 0)
                        {
                              Dictionary<string, object> dic1 = MinRect(chCombine);
                              Dictionary<string, object> dic2 = MinParallelogram(chCombine);
                              float area1 = Convert .ToSingle(dic1["area"]);
                              float area2 = Convert .ToSingle(dic2["area"]);
                              float length1 = Convert .ToSingle(dic1["length"]);
                              float length2 = Convert .ToSingle(dic2["length"]);
                              float height1 = Convert .ToSingle(dic1["height"]);
                              float height2 = Convert .ToSingle(dic2["height"]);
                              if (limit > 0 && length1 <= limit && height1 <= limit && !(length2 <= limit && height2 <= limit))
                              {
                                    if (area < 0 || area > area1)
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                              }
                              else if (limit > 0 && length2 <= limit && height2 <= limit && !(length1 <= limit && height1 <= limit))
                              {
                                    if (area < 0 || area > area2)
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                              else if (limit < 0
                                   || (limit > 0 && length2 <= limit && height2 <= limit && length1 <= limit && height1 <= limit))
                              {
                                    if (area1 <= area2 && (area < 0 || area > area1))
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                                    else if (area1 <= area2 && (area < 0 || area > area2))
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                        }
                        if (accept)
                        {
                              area = Convert .ToSingle(dicCombine["area"]);
                              movex = l * 2 - mindis;
                              movey = i * T;
                              angle = Convert .ToSingle(dicCombine["angle"]);
                              length = Convert .ToSingle(dicCombine["length"]);
                              height = Convert .ToSingle(dicCombine["height"]);
                              chCombine = (List<PointF>)dicCombine["convexhull"];
                        }
                  }
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("area", area);
                  dic .Add("movex", movex);
                  dic .Add("movey", movey);
                  dic .Add("angle", angle);
                  dic .Add("length", length);
                  dic .Add("height", height);
                  dic .Add("convexhull", chCombine);
                  return dic;
            }
            //左下--右下
            public Dictionary<string, object> CombineDown(List<PointF> p, List<PointF> ch, List<PointF> pnew, List<PointF> chnew
                  , float T, float limit, string type)
            {
                  //上边界
                  float maxy = -99999;
                  float miny = 99999;
                  float maxx = -99999;
                  float minx = 99999;
                  float upy = -99999;
                  for (int i = 0; i < ch .Count; i++)
                  {
                        if (maxx < ch[i] .X)
                        {
                              maxy = ch[i] .Y;
                              maxx = ch[i] .X;
                        }
                        else if (maxx == ch[i] .X)
                        {
                              maxy = Math .Max(maxy, ch[i] .Y);
                        }
                        if (minx > ch[i] .X)
                        {
                              miny = ch[i] .Y;
                              minx = ch[i] .X;
                        }
                        else if (minx == ch[i] .X)
                        {
                              miny = Math .Max(miny, ch[i] .Y);
                        }
                  }
                  upy = Math .Max(miny, maxy);
                  //左边界
                  float maxy2 = -99999;
                  float miny2 = 99999;
                  float maxx2 = -99999;
                  float minx2 = 99999;
                  float downy = 99999;
                  for (int i = 0; i < chnew .Count; i++)
                  {
                        if (maxx2 < chnew[i] .X)
                        {
                              maxy2 = chnew[i] .Y;
                              maxx2 = chnew[i] .X;
                        }
                        else if (maxx2 == chnew[i] .X)
                        {
                              maxy2 = Math .Min(maxy2, chnew[i] .Y);
                        }
                        if (minx2 > chnew[i] .X)
                        {
                              miny2 = chnew[i] .Y;
                              minx2 = chnew[i] .X;
                        }
                        else if (minx2 == chnew[i] .X)
                        {
                              miny2 = Math .Min(miny2, chnew[i] .Y);
                        }
                  }
                  downy = Math .Min(miny2, maxy2);
                  //移动长度
                  float l = maxx - minx;
                  //最小矩形面积
                  float area = -1;
                  float movex = 0;
                  float movey = 0;
                  float angle = 0;
                  float length = 0;
                  float height = 0;
                  List<PointF> chCombine = new List<PointF>();
                  //副图向上移动，计算最小平移距离，再向右移动与主图进行组合，求最小包络矩形
                  for (int i = 1; i < Math .Floor(2 * l / T); i++)
                  {
                        //左右边界
                        float rightx = Math .Min(maxx, maxx2 + i * T);
                        float leftx = Math .Max(minx, minx2 + i * T);

                        List<PointF> chnew_move = Move(chnew, i * T, 0);
                        List<PointF> pnew_move = Move(pnew, i * T, 0);

                        List<PointF> p1 = p .Where(t => t .X <= rightx && t .X >= leftx && t .Y <= upy) .ToList();
                        List<PointF> p2 = pnew_move .Where(t => t .X <= rightx && t .X >= leftx && t .Y >= downy) .ToList();

                        List<Line> l1 = new List<Line>();
                        List<Line> l2 = new List<Line>();
                        for (int j = 0; j < p .Count; j++)
                        {
                              PointF start = p[j];
                              PointF end = Next(p, j);
                              if ((start .X <= rightx && start .X >= leftx && start .Y <= upy)
                                  || (end .X <= rightx && end .X >= leftx && end .Y <= upy))
                              {
                                    l1 .Add(new Line(start, end));
                              }
                        }
                        for (int j = 0; j < pnew_move .Count; j++)
                        {
                              PointF start = pnew_move[j];
                              PointF end = Next(pnew_move, j);
                              if ((start .X <= rightx && start .X >= leftx && start .Y >= downy)
                                  || (end .X <= rightx && end .X >= leftx && end .Y >= downy))
                              {
                                    l2 .Add(new Line(start, end));
                              }
                        }
                        //计算最小平移距离
                        float mindis = 99999;
                        //p1-->l2
                        for (int j = 0; j < p1 .Count; j++)
                        {
                              for (int k = 0; k < l2 .Count; k++)
                              {
                                    PointF pt = p1[j];
                                    Line le = l2[k];
                                    if ((pt .X >= le .Start .X && pt .X <= le .End .X)
                                          || (pt .X <= le .Start .X && pt .X >= le .End .X))
                                    {
                                          float dis = Math .Abs((pt .X - le .Start .X) * (le .End .Y - le .Start .Y) / (le .End .X - le .Start .X) + le .Start .Y - pt .Y);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //p2-->l1
                        for (int j = 0; j < p2 .Count; j++)
                        {
                              for (int k = 0; k < l1 .Count; k++)
                              {
                                    PointF pt = p2[j];
                                    Line le = l1[k];
                                    if ((pt .Y >= le .Start .X && pt .X <= le .End .X)
                                          || (pt .X <= le .Start .X && pt .X >= le .End .X))
                                    {
                                          float dis = Math .Abs((pt .X - le .Start .X) * (le .End .Y - le .Start .Y) / (le .End .X - le .Start .X) + le .Start .Y - pt .Y);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //副图凸包向右移动与主图组合
                        if (mindis > 0)
                        {
                              chnew_move = Move(chnew_move, 0, mindis);
                        }
                        chCombine = ConvexHullCombine(chnew_move, ch);
                        if (chCombine .Count == 0)
                        {
                              return null;
                        }
                        Dictionary<string, object> dicCombine = new Dictionary<string, object>();
                        bool accept = false;
                        if (type == "rect")
                        {
                              Dictionary<string, object> dic0 = MinRect(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                    (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "para")
                        {
                              Dictionary<string, object> dic0 = MinParallelogram(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                   (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "mix" || limit > 0)
                        {
                              Dictionary<string, object> dic1 = MinRect(chCombine);
                              Dictionary<string, object> dic2 = MinParallelogram(chCombine);
                              float area1 = Convert .ToSingle(dic1["area"]);
                              float area2 = Convert .ToSingle(dic2["area"]);
                              float length1 = Convert .ToSingle(dic1["length"]);
                              float length2 = Convert .ToSingle(dic2["length"]);
                              float height1 = Convert .ToSingle(dic1["height"]);
                              float height2 = Convert .ToSingle(dic2["height"]);
                              if (limit > 0 && length1 <= limit && height1 <= limit && !(length2 <= limit && height2 <= limit))
                              {
                                    if (area < 0 || area > area1)
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                              }
                              else if (limit > 0 && length2 <= limit && height2 <= limit && !(length1 <= limit && height1 <= limit))
                              {
                                    if (area < 0 || area > area2)
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                              else if (limit < 0
                                   || (limit > 0 && length2 <= limit && height2 <= limit && length1 <= limit && height1 <= limit))
                              {
                                    if (area1 <= area2 && (area < 0 || area > area1))
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                                    else if (area1 <= area2 && (area < 0 || area > area2))
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                        }
                        if (accept)
                        {
                              area = Convert .ToSingle(dicCombine["area"]);
                              movex = i * T;
                              movey = mindis;
                              angle = Convert .ToSingle(dicCombine["angle"]);
                              length = Convert .ToSingle(dicCombine["length"]);
                              height = Convert .ToSingle(dicCombine["height"]);
                              chCombine = (List<PointF>)dicCombine["convexhull"];
                        }
                  }
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("area", area);
                  dic .Add("movex", movex);
                  dic .Add("movey", movey);
                  dic .Add("angle", angle);
                  dic .Add("length", length);
                  dic .Add("height", height);
                  dic .Add("convexhull", chCombine);
                  return dic;
            }
            //左上--右上
            public Dictionary<string, object> CombineUp(List<PointF> p, List<PointF> ch, List<PointF> pnew, List<PointF> chnew
                  , float T, float limit, string type)
            {
                  //下边界
                  float maxy2 = -99999;
                  float miny2 = 99999;
                  float maxx2 = -99999;
                  float minx2 = 99999;
                  float downy = 99999;
                  float down = 99999;
                  float up = -99999;
                  for (int i = 0; i < ch .Count; i++)
                  {
                        if (maxx2 < ch[i] .X)
                        {
                              maxy2 = ch[i] .Y;
                              maxx2 = ch[i] .X;
                        }
                        else if (maxx2 == ch[i] .X)
                        {
                              maxy2 = Math .Min(maxy2, ch[i] .Y);
                        }
                        if (minx2 > ch[i] .X)
                        {
                              miny2 = ch[i] .Y;
                              minx2 = ch[i] .X;
                        }
                        else if (minx2 == ch[i] .X)
                        {
                              miny2 = Math .Min(miny2, ch[i] .Y);
                        }
                        if (up < ch[i] .Y)
                        {
                              up = ch[i] .Y;
                        }
                        if (down > ch[i] .Y)
                        {
                              down = ch[i] .Y;
                        }
                  }
                  //高度，宽度
                  float h = up - down;
                  float l = maxx2 - minx2;
                  downy = Math .Min(miny2, maxy2);
                  //上边界
                  float maxy = -99999;
                  float miny = 99999;
                  float maxx = -99999;
                  float minx = 99999;
                  float upy = -99999;
                  for (int i = 0; i < chnew .Count; i++)
                  {
                        if (maxx < chnew[i] .X)
                        {
                              maxy = chnew[i] .Y;
                              maxx = chnew[i] .X;
                        }
                        else if (maxx == chnew[i] .X)
                        {
                              maxy = Math .Max(maxy, chnew[i] .Y);
                        }
                        if (minx > chnew[i] .X)
                        {
                              miny = chnew[i] .Y;
                              minx = chnew[i] .X;
                        }
                        else if (minx == chnew[i] .X)
                        {
                              miny = Math .Max(miny, chnew[i] .Y);
                        }
                  }
                  upy = Math .Max(miny, maxy) + 2 * h;

                  //最小矩形面积
                  float area = -1;
                  float movex = 0;
                  float movey = 0;
                  float angle = 0;
                  float length = 0;
                  float height = 0;
                  List<PointF> chCombine = new List<PointF>();
                  //副图向上移动，计算最小平移距离，再向左移动与主图进行组合，求最小包络矩形
                  for (int i = 1; i < Math .Floor(2 * l / T); i++)
                  {
                        //上边界，下边界
                        float rightx = Math .Min(maxx + i * T, maxx2);
                        float leftx = Math .Max(minx + i * T, minx2);

                        List<PointF> chnew_move = Move(chnew, i * T, h * 2);
                        List<PointF> pnew_move = Move(pnew, i * T, h * 2);

                        List<PointF> p1 = pnew_move .Where(t => t .X <= rightx && t .X >= leftx && t .Y <= upy) .ToList();
                        List<PointF> p2 = p .Where(t => t .X <= rightx && t .X >= leftx && t .Y >= downy) .ToList();

                        List<Line> l1 = new List<Line>();
                        List<Line> l2 = new List<Line>();
                        for (int j = 0; j < pnew_move .Count; j++)
                        {
                              PointF start = pnew_move[j];
                              PointF end = Next(pnew_move, j);
                              if ((start .X <= rightx && start .X >= leftx && start .Y <= upy)
                                  || (end .X <= rightx && end .X >= leftx && end .Y <= upy))
                              {
                                    l1 .Add(new Line(start, end));
                              }
                        }
                        for (int j = 0; j < p .Count; j++)
                        {
                              PointF start = p[j];
                              PointF end = Next(p, j);
                              if ((start .X <= rightx && start .X >= leftx && start .Y >= downy)
                                  || (end .X <= rightx && end .X >= leftx && end .Y >= downy))
                              {
                                    l2 .Add(new Line(start, end));
                              }
                        }
                        //计算最小平移距离
                        float mindis = 99999;
                        //p1-->l2
                        for (int j = 0; j < p1 .Count; j++)
                        {
                              for (int k = 0; k < l2 .Count; k++)
                              {
                                    PointF pt = p1[j];
                                    Line le = l2[k];
                                    if ((pt .X >= le .Start .X && pt .X <= le .End .X)
                                          || (pt .X <= le .Start .X && pt .X >= le .End .X))
                                    {
                                          float dis = Math .Abs((pt .X - le .Start .X) * (le .End .Y - le .Start .Y) / (le .End .X - le .Start .X) + le .Start .Y - pt .Y);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //p2-->l1
                        for (int j = 0; j < p2 .Count; j++)
                        {
                              for (int k = 0; k < l1 .Count; k++)
                              {
                                    PointF pt = p2[j];
                                    Line le = l1[k];
                                    if ((pt .X >= le .Start .X && pt .X <= le .End .X)
                                          || (pt .X <= le .Start .X && pt .X >= le .End .X))
                                    {
                                          float dis = Math .Abs((pt .X - le .Start .X) * (le .End .Y - le .Start .Y) / (le .End .X - le .Start .X) + le .Start .Y - pt .Y);
                                          if (dis < mindis)
                                          {
                                                mindis = dis;
                                          }
                                    }
                              }
                        }
                        //副图凸包向左移动与主图组合
                        if (mindis > 0)
                        {
                              chnew_move = Move(chnew_move, 0, -mindis);
                        }
                        chCombine = ConvexHullCombine(chnew_move, ch);
                        if (chCombine .Count == 0)
                        {
                              return null;
                        }
                        Dictionary<string, object> dicCombine = new Dictionary<string, object>();
                        bool accept = false;
                        if (type == "rect")
                        {
                              Dictionary<string, object> dic0 = MinRect(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                    (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "para")
                        {
                              Dictionary<string, object> dic0 = MinParallelogram(chCombine);
                              float area0 = Convert .ToSingle(dic0["area"]);
                              float length0 = Convert .ToSingle(dic0["length"]);
                              float height0 = Convert .ToSingle(dic0["height"]);
                              if (limit < 0 ||
                                   (length0 <= limit && height0 <= limit))
                              {
                                    if (area < 0 || area > area0)
                                    {
                                          dicCombine = dic0;
                                          accept = true;
                                    }
                              }
                        }
                        else if (type == "mix" || limit > 0)
                        {
                              Dictionary<string, object> dic1 = MinRect(chCombine);
                              Dictionary<string, object> dic2 = MinParallelogram(chCombine);
                              float area1 = Convert .ToSingle(dic1["area"]);
                              float area2 = Convert .ToSingle(dic2["area"]);
                              float length1 = Convert .ToSingle(dic1["length"]);
                              float length2 = Convert .ToSingle(dic2["length"]);
                              float height1 = Convert .ToSingle(dic1["height"]);
                              float height2 = Convert .ToSingle(dic2["height"]);
                              if (limit > 0 && length1 <= limit && height1 <= limit && !(length2 <= limit && height2 <= limit))
                              {
                                    if (area < 0 || area > area1)
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                              }
                              else if (limit > 0 && length2 <= limit && height2 <= limit && !(length1 <= limit && height1 <= limit))
                              {
                                    if (area < 0 || area > area2)
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                              else if (limit < 0
                                   || (limit > 0 && length2 <= limit && height2 <= limit && length1 <= limit && height1 <= limit))
                              {
                                    if (area1 <= area2 && (area < 0 || area > area1))
                                    {
                                          accept = true;
                                          dicCombine = dic1;
                                    }
                                    else if (area1 <= area2 && (area < 0 || area > area2))
                                    {
                                          accept = true;
                                          dicCombine = dic2;
                                    }
                              }
                        }
                        if (accept)
                        {
                              area = Convert .ToSingle(dicCombine["area"]);
                              movex = i * T;
                              movey = h * 2 - mindis;
                              angle = Convert .ToSingle(dicCombine["angle"]);
                              length = Convert .ToSingle(dicCombine["length"]);
                              height = Convert .ToSingle(dicCombine["height"]);
                              chCombine = (List<PointF>)dicCombine["convexhull"];
                        }
                  }
                  Dictionary<string, object> dic = new Dictionary<string, object>();
                  dic .Add("area", area);
                  dic .Add("movex", movex);
                  dic .Add("movey", movey);
                  dic .Add("angle", angle);
                  dic .Add("length", length);
                  dic .Add("height", height);
                  dic .Add("convexhull", chCombine);
                  return dic;
            }
            public List<PointF> Move(List<PointF> p, float x, float y)
            {
                  List<PointF> pnew = p .ToList();
                  for (int i = 0; i < pnew .Count; i++)
                  {
                        pnew[i] = new PointF(pnew[i] .X + x, pnew[i] .Y + y);
                  }
                  return pnew;
            }

            private struct Line
            {
                  public PointF Start;
                  public PointF End;

                  public Line(PointF s, PointF e)
                  {
                        Start = s;
                        End = e;
                  }
            }
            #endregion
      }
}
