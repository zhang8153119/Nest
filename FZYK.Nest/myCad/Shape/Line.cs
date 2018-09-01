using myCad .CADInterfaceCtrl;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .Shape
{
      class Line : BaseShape
      {
            private PointF minPoint;
            private float angle;
            private float length;

            public Line(PointF sp, PointF ep)
            {
                  StartPoint = sp;
                  EndPoint = ep;
                  this .ShapeClass = "Line";
                  //AreaPen = new Pen(Color.White, 0);
            }

            public PointF MinPoint
            {
                  get
                  {
                        return minPoint;
                  }

                  set
                  {
                        minPoint = value;
                  }
            }

            public float Angle
            {
                  get
                  {
                        return angle;
                  }

                  set
                  {
                        angle = value;
                  }
            }

            public float Length
            {
                  get
                  {
                        return length;
                  }

                  set
                  {
                        length = value;
                  }
            }


            private PointF SP, EP;

            public override void Draw(Graphics g, float zoomNum)
            {
                  Pen pen = new Pen(this .PenColor, 0.1f);
                  pen .DashStyle = this .PenStyle;
                  SP = new PointF(StartPoint .X * zoomNum, StartPoint .Y * zoomNum);
                  EP = new PointF(EndPoint .X * zoomNum, EndPoint .Y * zoomNum);

                  switch (Show)
                  {
                        case DisplayStyle .Normal:
                              g .DrawLine(pen, SP, EP);
                              break;

                        case DisplayStyle .Edit:
                              break;
                        case DisplayStyle .Hit:
                              Pen newPen1 = new Pen(this .PenColor, 3);
                              newPen1 .DashStyle = DashStyle .Solid;
                              g .DrawLine(newPen1, SP, EP);
                              Pen newPen2 = new Pen(Color .Black, 1);
                              newPen2 .DashStyle = DashStyle .Custom;
                              newPen2 .DashPattern = new float[] { 3, 3 };
                              g .DrawLine(newPen2, SP, EP);
                              newPen1 .Dispose(); newPen2 .Dispose();
                              break;
                        case DisplayStyle .Preview:
                              break;
                        case DisplayStyle .Select:
                              Pen newPen3 = new Pen(this .PenColor, 1);
                              newPen3 .DashStyle = DashStyle .Custom;
                              newPen3 .DashPattern = new float[] { 3, 3 };
                              g .DrawLine(newPen3, SP, EP);
                              RectangleF[] rects = new RectangleF[]
                              {
                        new RectangleF(SP.X - 5, SP.Y - 5, 10, 10),
                        //new RectangleF(MP.X - 5, MP.Y - 5, 10, 10),
                        new RectangleF(EP.X - 5, EP.Y - 5, 10, 10)
                              };
                              g .FillRectangles(new SolidBrush(Color .FromArgb(0, 128, 255)), rects);
                              g .DrawRectangles(new Pen(Color .Gray), rects);
                              newPen3 .Dispose();
                              break;
                  }
            }

            private void CreateObjects()
            {
                  if (AreaPath != null)
                        return;

                  AreaPath = new GraphicsPath();
                  AreaPen = new Pen(Color .White, 0);
                  AreaPath .AddLine(StartPoint .X, StartPoint .Y, EndPoint .X, EndPoint .Y);
                  AreaPath .Widen(AreaPen);

                  AreaRegion = new Region(AreaPath);
            }

            public override bool IntersectsWith(RectangleF rect)//反框选方法
            {
                  CreateObjects();
                  return AreaRegion .IsVisible(rect);
            }

            public override bool SurroundedBy(RectangleF rect)//正框选方法，如果图形整个被包围在矩形选框内则返回true。
            {
                  PointF[] surroundPoint = new PointF[] { StartPoint, EndPoint };
                  foreach (PointF item in surroundPoint)
                  {
                        if (IsInBox(item, rect) == false)
                        {
                              return false;
                        }
                  }
                  return true;
            }
      }
}
