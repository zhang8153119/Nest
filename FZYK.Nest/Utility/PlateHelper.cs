using myCad .Model;
using myCad .ShapeOper;
using System;
using System .Collections .Generic;
using System .Drawing;

namespace FZYK .Nest
{
      public class PlateCombine
      {
            public PlateModel Plate1 { get; set; }
            public PlateModel Plate2 { get; set; }
            public RectangleF Rect { get; set; }
            public List<GridData> GridValue { get; set; }
            public string flag { get; set; }//test
            public int id { get; set; }
            public float angle1 { get; set; }
            public float angle2 { get; set; }
            public string key { get; set; } = "";
            public bool ifMatchWidth { get; set; } = false;
            public PlateCombine()
            {
            }
            public PlateCombine(PlateModel p1, PlateModel p2, float a1 = 0, float a2 = 0)
            {
                  Plate1 = p1;
                  Plate2 = p2;
                  angle1 = a1;
                  angle2 = a2;
            }

            public object Copy()
            {
                  return new PlateCombine() as object;
            }
      }
      public class PlateHelper
      {
            float RE = 0.00001f;
            /// <summary>
            /// 获取最小包络矩形，旋转移动到坐标原点位置
            /// </summary>
            /// <param name="pm"></param>
            /// <returns></returns>
            public PlateModel GetMinPlate(PlateModel pm)
            {
                  RectHelper rect = new RectHelper();
                  Dictionary<string, object> dic = rect .MinRect(pm .OutModel .ExpandPoint);
                  PlateModel pmnew = RotateAndMove(pm, Convert .ToSingle(dic["angle"]));
                  pmnew .Rect = GetRect(pmnew .OutModel .ExpandPoint);
                  return pmnew;
            }

            public PlateCombine GetMinPlateCombine(PlateModel source, float T, string type, float limit = -1)
            {
                  RectHelper rect = new RectHelper();
                  CopyOper co = new CopyOper();
                  PlateModel pm = co .CopyPlate(source);
                  PlateModel pmnew = co .CopyPlate(source);//旋转180后的图形
                  pmnew = Rotate(pmnew, 180);

                  List<PointF> pnew = pmnew .OutModel .ExpandPoint;
                  List<PointF> chnew = rect .GetConvexHull(pmnew .OutModel .ExpandPoint);

                  List<PointF> p = pm .OutModel .ExpandPoint;
                  List<PointF> ch = rect .GetConvexHull(pm .OutModel .ExpandPoint);

                  Dictionary<string, object> diccombine = rect .Combine(p, ch, pnew, chnew, T, limit, type, 0.9f);
                  List<PointF> chcombine = (List<PointF>)diccombine["convexhull"];
                  Move(pmnew, Convert .ToSingle(diccombine["movex"]), Convert .ToSingle(diccombine["movey"]));
                  RotateAndMove(pm, pmnew, Convert .ToSingle(diccombine["angle"]));

                  PlateCombine pc = new PlateCombine(pm, pmnew);
                  pc .flag = diccombine["flag"] .ToString();
                  pc .Rect = pm .Rect = pmnew .Rect = GetRect(pm .OutModel .ExpandPoint, pmnew .OutModel .ExpandPoint);
                  return pc;
            }
            public List<PlateCombine> GetMinPlateCombineAll(PlateModel source, float T, string type, float limit = -1, bool _matchwidth = false
                  , float _matchrate = 0.9f)
            {
                  RectHelper rect = new RectHelper();
                  CopyOper co = new CopyOper();
                  PlateModel pm = co .CopyPlate(source);
                  PlateModel pmnew = co .CopyPlate(source);//旋转180后的图形
                  pmnew = Rotate(pmnew, 180);

                  List<PointF> pnew = pmnew .OutModel .ExpandPoint;
                  List<PointF> chnew = rect .GetConvexHull(pmnew .OutModel .ExpandPoint);

                  List<PointF> p = pm .OutModel .ExpandPoint;
                  List<PointF> ch = rect .GetConvexHull(pm .OutModel .ExpandPoint);
                  var tp = rect .CombineAll(p, ch, pnew, chnew, T, limit, type, _matchwidth, _matchrate);
                  List<PlateCombine> pc = new List<PlateCombine>();

                  if (Convert .ToSingle(tp .Item1["area"] .ToString()) > 0)
                  {
                        PlateModel pm1 = co .CopyPlate(pm);
                        PlateModel pmnew1 = co .CopyPlate(pmnew);
                        Move(pmnew1, Convert .ToSingle(tp .Item1["movex"]), Convert .ToSingle(tp .Item1["movey"]));
                        RotateAndMove(pm1, pmnew1, Convert .ToSingle(tp .Item1["angle"]));
                        PlateCombine pc1 = new PlateCombine(pm1, pmnew1, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc1 .Rect = pm1 .Rect = pmnew1 .Rect = GetRect(pm1 .OutModel .ExpandPoint, pmnew1 .OutModel .ExpandPoint);
                        pc .Add(pc1);
                  }
                  if (Convert .ToSingle(tp .Item2["area"] .ToString()) > 0)
                  {
                        PlateModel pm2 = co .CopyPlate(pm);
                        PlateModel pmnew2 = co .CopyPlate(pmnew);
                        Move(pmnew2, Convert .ToSingle(tp .Item2["movex"]), Convert .ToSingle(tp .Item2["movey"]));
                        RotateAndMove(pm2, pmnew2, Convert .ToSingle(tp .Item2["angle"]));
                        PlateCombine pc2 = new PlateCombine(pm2, pmnew2, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc2 .Rect = pm2 .Rect = pmnew2 .Rect = GetRect(pm2 .OutModel .ExpandPoint, pmnew2 .OutModel .ExpandPoint);
                        pc .Add(pc2);
                  }
                  if (Convert .ToSingle(tp .Item3["area"] .ToString()) > 0)
                  {
                        PlateModel pm3 = co .CopyPlate(pm);
                        PlateModel pmnew3 = co .CopyPlate(pmnew);
                        Move(pmnew3, Convert .ToSingle(tp .Item3["movex"]), Convert .ToSingle(tp .Item3["movey"]));
                        RotateAndMove(pm3, pmnew3, Convert .ToSingle(tp .Item3["angle"]));
                        PlateCombine pc3 = new PlateCombine(pm3, pmnew3, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc3 .Rect = pm3 .Rect = pmnew3 .Rect = GetRect(pm3 .OutModel .ExpandPoint, pmnew3 .OutModel .ExpandPoint);
                        pc .Add(pc3);
                  }
                  if (Convert .ToSingle(tp .Item4["area"] .ToString()) > 0)
                  {
                        PlateModel pm4 = co .CopyPlate(pm);
                        PlateModel pmnew4 = co .CopyPlate(pmnew);
                        Move(pmnew4, Convert .ToSingle(tp .Item4["movex"]), Convert .ToSingle(tp .Item4["movey"]));
                        RotateAndMove(pm4, pmnew4, Convert .ToSingle(tp .Item4["angle"]));
                        PlateCombine pc4 = new PlateCombine(pm4, pmnew4, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc4 .Rect = pm4 .Rect = pmnew4 .Rect = GetRect(pm4 .OutModel .ExpandPoint, pmnew4 .OutModel .ExpandPoint);
                        pc .Add(pc4);
                  }
                  if (Convert .ToSingle(tp .Item1["area"] .ToString()) > 0)
                  {
                        PlateModel pm5 = co .CopyPlate(pm);
                        PlateModel pmnew5 = co .CopyPlate(pmnew);
                        Move(pmnew5, Convert .ToSingle(tp .Item1["movex"]), Convert .ToSingle(tp .Item1["movey"]));
                        RotateAndMove(pm5, pmnew5, Convert .ToSingle(tp .Item1["angle2"]));
                        PlateCombine pc5 = new PlateCombine(pm5, pmnew5, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc5 .Rect = pm5 .Rect = pmnew5 .Rect = GetRect(pm5 .OutModel .ExpandPoint, pmnew5 .OutModel .ExpandPoint);
                        pc .Add(pc5);
                  }
                  if (Convert .ToSingle(tp .Item2["area"] .ToString()) > 0)
                  {
                        PlateModel pm6 = co .CopyPlate(pm);
                        PlateModel pmnew6 = co .CopyPlate(pmnew);
                        Move(pmnew6, Convert .ToSingle(tp .Item2["movex"]), Convert .ToSingle(tp .Item2["movey"]));
                        RotateAndMove(pm6, pmnew6, Convert .ToSingle(tp .Item2["angle2"]));
                        PlateCombine pc6 = new PlateCombine(pm6, pmnew6, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc6 .Rect = pm6 .Rect = pmnew6 .Rect = GetRect(pm6 .OutModel .ExpandPoint, pmnew6 .OutModel .ExpandPoint);
                        pc .Add(pc6);
                  }
                  if (Convert .ToSingle(tp .Item3["area"] .ToString()) > 0)
                  {
                        PlateModel pm7 = co .CopyPlate(pm);
                        PlateModel pmnew7 = co .CopyPlate(pmnew);
                        Move(pmnew7, Convert .ToSingle(tp .Item3["movex"]), Convert .ToSingle(tp .Item3["movey"]));
                        RotateAndMove(pm7, pmnew7, Convert .ToSingle(tp .Item3["angle2"]));
                        PlateCombine pc7 = new PlateCombine(pm7, pmnew7, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc7 .Rect = pm7 .Rect = pmnew7 .Rect = GetRect(pm7 .OutModel .ExpandPoint, pmnew7 .OutModel .ExpandPoint);
                        pc .Add(pc7);
                  }
                  if (Convert .ToSingle(tp .Item4["area"] .ToString()) > 0)
                  {
                        PlateModel pm8 = co .CopyPlate(pm);
                        PlateModel pmnew8 = co .CopyPlate(pmnew);
                        Move(pmnew8, Convert .ToSingle(tp .Item4["movex"]), Convert .ToSingle(tp .Item4["movey"]));
                        RotateAndMove(pm8, pmnew8, Convert .ToSingle(tp .Item4["angle2"]));
                        PlateCombine pc8 = new PlateCombine(pm8, pmnew8, Convert .ToSingle(tp .Item1["angle"]), Convert .ToSingle(tp .Item1["angle2"]));
                        pc8 .Rect = pm8 .Rect = pmnew8 .Rect = GetRect(pm8 .OutModel .ExpandPoint, pmnew8 .OutModel .ExpandPoint);
                        pc .Add(pc8);
                  }

                  return pc;
            }
            /// <summary>
            /// 获取边界矩形
            /// </summary>
            /// <param name="plist"></param>
            /// <returns></returns>
            public RectangleF GetRect(List<PointF> plist, List<PointF> plist2 = null)
            {
                  float minx = plist[0] .X;
                  float maxx = plist[0] .X;
                  float miny = plist[0] .Y;
                  float maxy = plist[0] .Y;
                  for (int i = 1; i < plist .Count; i++)
                  {
                        if (plist[i] .X < minx)
                              minx = plist[i] .X;
                        if (plist[i] .X > maxx)
                              maxx = plist[i] .X;
                        if (plist[i] .Y < miny)
                              miny = plist[i] .Y;
                        if (plist[i] .Y > maxy)
                              maxy = plist[i] .Y;
                  }
                  if (plist2 != null)
                  {
                        for (int i = 0; i < plist2 .Count; i++)
                        {
                              if (plist2[i] .X < minx)
                                    minx = plist2[i] .X;
                              if (plist2[i] .X > maxx)
                                    maxx = plist2[i] .X;
                              if (plist2[i] .Y < miny)
                                    miny = plist2[i] .Y;
                              if (plist2[i] .Y > maxy)
                                    maxy = plist2[i] .Y;
                        }
                  }
                  return new RectangleF(0, 0, maxx - minx, maxy - miny);
            }
            public PlateModel Move(PlateModel pm, float x, float y)
            {
                  PlateModel pmnew = new MoveOper() .MovePlate(pm, x, y);
                  return pmnew;
            }
            /// <summary>
            /// 旋转
            /// </summary>
            /// <param name="pm"></param>
            /// <param name="angle"></param>
            /// <returns></returns>
            public PlateModel Rotate(PlateModel pm, float angle)
            {
                  //PlateModel pmnew = new RotateOper() .RotatePlate(pm, new PointF(0, 0), angle);
                  PlateModel pmnew = new CopyOper() .CopyPlate(pm);
                  new RotateOper() .RotatePlate(pmnew, new PointF(0, 0), angle);
                  return pmnew;
            }
            /// <summary>
            /// 旋转后归位
            /// </summary>
            /// <param name="pm"></param>
            /// <param name="angle"></param>
            /// <returns></returns>
            public PlateModel RotateAndMove(PlateModel pmold, float angle)
            {
                  //PlateModel pm = new RotateOper() .RotatePlate(pmold, new PointF(0, 0), angle);
                  CopyOper co = new CopyOper();
                  PlateModel pm = co .CopyPlate(pmold);
                  new RotateOper() .RotatePlate(pm, new PointF(0, 0), angle);
                  float maxX = pm .OutModel .ExpandPoint[0] .X;
                  float maxY = pm .OutModel .ExpandPoint[0] .Y;
                  float minX = pm .OutModel .ExpandPoint[0] .X;
                  float minY = pm .OutModel .ExpandPoint[0] .Y;
                  for (int i = 0; i < pm .OutModel .ExpandPoint .Count; i++)
                  {
                        if (maxX < pm .OutModel .ExpandPoint[i] .X)
                              maxX = pm .OutModel .ExpandPoint[i] .X;
                        if (maxY < pm .OutModel .ExpandPoint[i] .Y)
                              maxY = pm .OutModel .ExpandPoint[i] .Y;

                        if (minX > pm .OutModel .ExpandPoint[i] .X)
                              minX = pm .OutModel .ExpandPoint[i] .X;
                        if (minY > pm .OutModel .ExpandPoint[i] .Y)
                              minY = pm .OutModel .ExpandPoint[i] .Y;
                  }
                  PlateModel pmmove = new MoveOper() .MovePlate(pm, -minX, -minY);
                  /*if (angle == 90 || angle == 270)
                        pmmove .Rect = new RectangleF(0, 0, pm .Rect .Height, pm .Rect .Width);
                  else if (angle == 0 || angle == 180)
                        pmmove .Rect = new RectangleF(0, 0, pm .Rect .Width, pm .Rect .Height);*/
                  pmmove .Rect = GetRect(pmmove .OutModel .ExpandPoint);
                  return pmmove;
            }
            /// <summary>
            /// 组合图形旋转归位
            /// </summary>
            /// <param name="pm"></param>
            /// <param name="angle"></param>
            /// <returns></returns>
            public PlateCombine RotateAndMove(PlateCombine pc, float angle)
            {
                  CopyOper co = new CopyOper();
                  PlateModel pm1 = co .CopyPlate(pc .Plate1);
                  PlateModel pm2 = co .CopyPlate(pc .Plate2);

                  RotateOper ro = new RotateOper();
                  pm1 = ro .RotatePlate(pm1, new PointF(0, 0), angle);
                  pm2 = ro .RotatePlate(pm2, new PointF(0, 0), angle);

                  float maxX = pm1 .OutModel .ExpandPoint[0] .X;
                  float maxY = pm1 .OutModel .ExpandPoint[0] .Y;
                  float minX = pm1 .OutModel .ExpandPoint[0] .X;
                  float minY = pm1 .OutModel .ExpandPoint[0] .Y;
                  for (int i = 0; i < pm1 .OutModel .ExpandPoint .Count; i++)
                  {
                        if (maxX < pm1 .OutModel .ExpandPoint[i] .X)
                              maxX = pm1 .OutModel .ExpandPoint[i] .X;
                        if (maxY < pm1 .OutModel .ExpandPoint[i] .Y)
                              maxY = pm1 .OutModel .ExpandPoint[i] .Y;

                        if (minX > pm1 .OutModel .ExpandPoint[i] .X)
                              minX = pm1 .OutModel .ExpandPoint[i] .X;
                        if (minY > pm1 .OutModel .ExpandPoint[i] .Y)
                              minY = pm1 .OutModel .ExpandPoint[i] .Y;
                  }
                  for (int i = 0; i < pm2 .OutModel .ExpandPoint .Count; i++)
                  {
                        if (maxX < pm2 .OutModel .ExpandPoint[i] .X)
                              maxX = pm2 .OutModel .ExpandPoint[i] .X;
                        if (maxY < pm2 .OutModel .ExpandPoint[i] .Y)
                              maxY = pm2 .OutModel .ExpandPoint[i] .Y;

                        if (minX > pm2 .OutModel .ExpandPoint[i] .X)
                              minX = pm2 .OutModel .ExpandPoint[i] .X;
                        if (minY > pm2 .OutModel .ExpandPoint[i] .Y)
                              minY = pm2 .OutModel .ExpandPoint[i] .Y;
                  }
                  MoveOper mo = new MoveOper();
                  mo .MovePlate(pm1, -minX, -minY);
                  mo .MovePlate(pm2, -minX, -minY);

                  PlateCombine pcnew = new PlateCombine(pm1, pm2);
                  pcnew .Rect = pc .Rect;
                  //if (angle == 90)
                  //{
                  //      pcnew .Rect = new RectangleF(0, 0, pc .Rect .Height, pc .Rect .Width);
                  //}
                  pcnew .Rect = GetRect(pm1 .OutModel .ExpandPoint, pm2 .OutModel .ExpandPoint);
                  return pcnew;
            }

            /// <summary>
            /// 组合图形旋转归位
            /// </summary>
            /// <param name="pm"></param>
            /// <param name="angle"></param>
            /// <returns></returns>
            public void RotateAndMove(PlateModel pm1, PlateModel pm2, float angle)
            {
                  pm1 = new RotateOper() .RotatePlate(pm1, new PointF(0, 0), angle);
                  pm2 = new RotateOper() .RotatePlate(pm2, new PointF(0, 0), angle);
                  float maxX = pm1 .OutModel .ExpandPoint[0] .X;
                  float maxY = pm1 .OutModel .ExpandPoint[0] .Y;
                  float minX = pm1 .OutModel .ExpandPoint[0] .X;
                  float minY = pm1 .OutModel .ExpandPoint[0] .Y;
                  for (int i = 0; i < pm1 .OutModel .ExpandPoint .Count; i++)
                  {
                        if (maxX < pm1 .OutModel .ExpandPoint[i] .X)
                              maxX = pm1 .OutModel .ExpandPoint[i] .X;
                        if (maxY < pm1 .OutModel .ExpandPoint[i] .Y)
                              maxY = pm1 .OutModel .ExpandPoint[i] .Y;

                        if (minX > pm1 .OutModel .ExpandPoint[i] .X)
                              minX = pm1 .OutModel .ExpandPoint[i] .X;
                        if (minY > pm1 .OutModel .ExpandPoint[i] .Y)
                              minY = pm1 .OutModel .ExpandPoint[i] .Y;
                  }
                  for (int i = 0; i < pm2 .OutModel .ExpandPoint .Count; i++)
                  {
                        if (maxX < pm2 .OutModel .ExpandPoint[i] .X)
                              maxX = pm2 .OutModel .ExpandPoint[i] .X;
                        if (maxY < pm2 .OutModel .ExpandPoint[i] .Y)
                              maxY = pm2 .OutModel .ExpandPoint[i] .Y;

                        if (minX > pm2 .OutModel .ExpandPoint[i] .X)
                              minX = pm2 .OutModel .ExpandPoint[i] .X;
                        if (minY > pm2 .OutModel .ExpandPoint[i] .Y)
                              minY = pm2 .OutModel .ExpandPoint[i] .Y;
                  }
                  new MoveOper() .MovePlate(pm1, -minX, -minY);
                  new MoveOper() .MovePlate(pm2, -minX, -minY);

                  pm1 .Rect = GetRect(pm1 .OutModel .ExpandPoint);
                  pm2 .Rect = GetRect(pm2 .OutModel .ExpandPoint);
            }

      }
}
