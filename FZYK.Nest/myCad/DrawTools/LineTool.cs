using myCad .CADInterfaceCtrl;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .DrawTools
{
      class LineTool : BaseTool
      {
            private float spX = 0;
            private float spY = 0;
            private float epX = 0;
            private float epY = 0;
            private string layerName = "";
            private string dxfColor = "";
            private Color color = Color .White;

            public void createLine(List<string> listString)
            {
                  for (int i = 0; i < listString .Count; i++)
                  {
                        bool drawPic = false;
                        string s1 = listString[i] .Trim();
                        string s2 = listString[i = i + 1] .Trim();
                        if ("LINE" .Equals(s2) && "0" .Equals(s1) && (i - 1) != 0)
                        {
                              drawPic = true; //第二个线段开始前对上一个线段进行画图    
                        }
                        else if ("10" .Equals(s1)) { spX = float .Parse(s2); }
                        else if ("20" .Equals(s1)) { spY = float .Parse(s2); }
                        else if ("11" .Equals(s1)) { epX = float .Parse(s2); }
                        else if ("21" .Equals(s1)) { epY = float .Parse(s2); }
                        else if ("62" .Equals(s1)) { color = new BaseColor() .judgeColor(s2); }

                        if (i == listString .Count - 1)
                        {
                              drawPic = true; //最后一条直线，循环结束前进行画图
                        }

                        if (drawPic)
                        {
                              Line nLine = new Line(new PointF(spX, spY), new PointF(epX, epY));
                              nLine .PenColor = color;
                              //nLine .ShapeID = CADInterface .globalID;
                              //CADInterface .globalID = CADInterface .globalID + 1;
                              //CADInterface .currentShapes .Add(nLine);
                              reSet();   //重置变量
                        }
                  }
            }

            public List<BaseShape> getLineByColor(List<string> listString, Dictionary<string, string> colorDic)
            {
                  List<BaseShape> listShape = new List<BaseShape>();
                  for (int i = 0; i < listString .Count; i++)
                  {
                        bool drawPic = false;
                        string s1 = listString[i] .Trim();
                        string s2 = listString[i = i + 1] .Trim();
                        if ("LINE" .Equals(s2) && "0" .Equals(s1) && (i - 1) != 0)
                        {
                              drawPic = true; //第二个线段开始前对上一个线段进行画图    
                        }
                        else if ("8" .Equals(s1)) { layerName = s2; }
                        else if ("10" .Equals(s1)) { spX = Round(float .Parse(s2)); }
                        else if ("20" .Equals(s1)) { spY = Round(float .Parse(s2)); }
                        else if ("11" .Equals(s1)) { epX = Round(float .Parse(s2)); }
                        else if ("21" .Equals(s1)) { epY = Round(float .Parse(s2)); }
                        else if ("62" .Equals(s1)) { dxfColor = s2; color = new BaseColor() .judgeColor(s2); }
                        if (i == listString .Count - 1)
                        {
                              drawPic = true; //最后一条直线，循环结束前进行画图
                        }

                        if (drawPic)
                        {
                              if ((!"" .Equals(dxfColor) && "7" .Equals(dxfColor)) || ("" .Equals(dxfColor) && "7" .Equals(colorDic[layerName])))
                              {
                                    Line nLine = new Line(new PointF(spX, spY), new PointF(epX, epY));
                                    nLine .PenColor = color;
                                    //nLine .ShapeID = CADInterface .globalID;
                                    //CADInterface .globalID = CADInterface .globalID + 1;
                                    //CADInterface .currentShapes .Add(nLine);
                                    listShape .Add(nLine);
                              }
                              reSet();   //重置变量
                        }
                  }
                  return listShape;
            }

            private void reSet()
            {
                  spX = 0;
                  spY = 0;
                  epX = 0;
                  epY = 0;
                  layerName = "";
                  dxfColor = "";
                  color = Color .White;
            }

            public override void LoadTool()
            {
                  MouseShape .MStyle = MouseStyle .Draw;
                  MouseShape .LastMStyle = MouseStyle .Draw;
                  base .LoadTool();
            }
      }
}
