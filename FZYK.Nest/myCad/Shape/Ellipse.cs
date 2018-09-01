using myCad .CADInterfaceCtrl;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .Linq;
using System .Text;
using System .Threading .Tasks;
using System .Windows .Forms;

namespace myCad .Shape
{
      class Ellipse : BaseShape
      {
            private PointF centerPoint;                        //椭圆圆心(必须有)
            private float startAngle;
            private float endAngle;
            private float longRadius;                          //椭圆长轴半径(必须有)
            private float shortRadius;
            private float longAndShort;                        //长短轴比值(必须有)
            private float perimeter;
            private float area;
            private float angle;                                 //椭圆旋转角度
            private float longRadiusX;                           //椭圆长轴偏移圆心量X(必须有)
            private float longRadiusY;                           //椭圆长轴偏移圆心量Y(必须有)
            private float startParameter;                        //起始参数，不是直接的角度值，是参数方程中的角度即椭圆外圆,内圆的角度(必须有)
            private float endParameter;                          //终止参数，不是直接的角度值，是参数方程中的角度即椭圆外圆,内圆的角度(必须有)
            private bool complete = false;
            private bool isArea = false;                           //是否计算面积
            private bool isBound = false;                          //是否计算范围
            private List<PointF> listPoint = new List<PointF>();    //圆弧线段，每5度分割一个线段点，用于后续移动等操作使用，随几何变换而变换

            public Ellipse(PointF cp, float lR, float sR, float angle, float sa, float ea, bool complete)
            {
                  centerPoint = cp;
                  longRadius = lR;
                  shortRadius = sR;
                  startAngle = sa;
                  endAngle = ea;
                  this .angle = angle;
                  this .Complete = complete;
                  this .ShapeClass = "Ellipse";
                  //AreaPen = new Pen(Color.White, 0);
            }
            public Ellipse(PointF cp, float lR, float sR, float angle, float sa, float ea, float lRX, float lRY, bool complete)
            {
                  centerPoint = cp;
                  longRadius = lR;
                  shortRadius = sR;
                  startAngle = sa;
                  endAngle = ea;
                  LongRadiusX = lRX;
                  LongRadiusY = lRY;
                  this .angle = angle;
                  this .Complete = complete;
                  this .ShapeClass = "Ellipse";
            }
            public Ellipse(PointF cp, float lR, float sR, float angle, float sa, float ea, float lRX, float lRY, float sPr, float ePr, bool complete)
            {
                  centerPoint = cp;
                  longRadius = lR;
                  shortRadius = sR;
                  startAngle = sa;
                  endAngle = ea;
                  LongRadiusX = lRX;
                  LongRadiusY = lRY;
                  StartParameter = sPr;
                  EndParameter = ePr;
                  this .angle = angle;
                  this .Complete = complete;
                  this .ShapeClass = "Ellipse";
            }
            public Ellipse(PointF cp, float lR, float sR, float angle, float lAS, float sa, float ea, float lRX, float lRY, float sPr, float ePr, bool complete)
            {
                  centerPoint = cp;
                  longRadius = lR;
                  shortRadius = sR;
                  startAngle = sa;
                  endAngle = ea;
                  LongRadiusX = lRX;
                  LongRadiusY = lRY;
                  StartParameter = sPr;
                  EndParameter = ePr;
                  LongAndShort = lAS;
                  this .angle = angle;
                  this .Complete = complete;
                  this .ShapeClass = "Ellipse";
            }
            public Ellipse(PointF cp, float lR, float sR, float angle, float lAS, float sa, float ea, float lRX, float lRY, float sPr, float ePr, PointF sPoint, PointF ePoint, bool complete)
            {
                  centerPoint = cp;
                  longRadius = lR;
                  shortRadius = sR;
                  startAngle = sa;
                  endAngle = ea;
                  LongRadiusX = lRX;
                  LongRadiusY = lRY;
                  StartParameter = sPr;
                  EndParameter = ePr;
                  LongAndShort = lAS;
                  StartPoint = sPoint;
                  EndPoint = ePoint;
                  this .angle = angle;
                  this .Complete = complete;
                  this .ShapeClass = "Ellipse";
            }

            public PointF CenterPoint
            {
                  get
                  {
                        return centerPoint;
                  }

                  set
                  {
                        centerPoint = value;
                  }
            }

            public float StartAngle
            {
                  get
                  {
                        return startAngle;
                  }

                  set
                  {
                        startAngle = value;
                  }
            }

            public float EndAngle
            {
                  get
                  {
                        return endAngle;
                  }

                  set
                  {
                        endAngle = value;
                  }
            }

            public float Perimeter
            {
                  get
                  {
                        return perimeter;
                  }

                  set
                  {
                        perimeter = value;
                  }
            }

            public float Area
            {
                  get
                  {
                        return area;
                  }

                  set
                  {
                        area = value;
                  }
            }

            public float LongRadius
            {
                  get
                  {
                        return longRadius;
                  }

                  set
                  {
                        longRadius = value;
                  }
            }

            public float ShortRadius
            {
                  get
                  {
                        return shortRadius;
                  }

                  set
                  {
                        shortRadius = value;
                  }
            }

            public bool Complete
            {
                  get
                  {
                        return complete;
                  }

                  set
                  {
                        complete = value;
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

            public float LongRadiusX
            {
                  get
                  {
                        return longRadiusX;
                  }

                  set
                  {
                        longRadiusX = value;
                  }
            }

            public float LongRadiusY
            {
                  get
                  {
                        return longRadiusY;
                  }

                  set
                  {
                        longRadiusY = value;
                  }
            }

            public float StartParameter
            {
                  get
                  {
                        return startParameter;
                  }

                  set
                  {
                        startParameter = value;
                  }
            }

            public float EndParameter
            {
                  get
                  {
                        return endParameter;
                  }

                  set
                  {
                        endParameter = value;
                  }
            }

            public float LongAndShort
            {
                  get
                  {
                        return longAndShort;
                  }

                  set
                  {
                        longAndShort = value;
                  }
            }

            public bool IsArea
            {
                  get
                  {
                        return isArea;
                  }

                  set
                  {
                        isArea = value;
                  }
            }

            public bool IsBound
            {
                  get
                  {
                        return isBound;
                  }

                  set
                  {
                        isBound = value;
                  }
            }

            public List<PointF> ListPoint
            {
                  get
                  {
                        return listPoint;
                  }

                  set
                  {
                        listPoint = value;
                  }
            }

            private float leftUpPX, leftUpPY, lR, sR;
            private PointF cep;

            public override void Draw(Graphics g, float zoomNum)
            {
                  if (Complete)
                  {
                        #region 完整的椭圆
                        Matrix old = g .Transform;
                        g .ScaleTransform(zoomNum, zoomNum);

                        leftUpPX = 0;
                        leftUpPY = 0;
                        cep .X = longRadius;
                        cep .Y = shortRadius;

                        sR = 2 * shortRadius;
                        lR = 2 * longRadius;

                        g .MultiplyTransform(new Matrix(1, 0, 0, 1, centerPoint .X - longRadius, centerPoint .Y - shortRadius));

                        Matrix rotate = g .Transform;
                        rotate .RotateAt(Angle, centerPoint, MatrixOrder .Append);
                        g .Transform = rotate;

                        Pen pen = new Pen(this .PenColor, 0.1f);
                        pen .DashStyle = this .PenStyle;


                        //g.DrawEllipse(pen, leftUpPX, leftUpPY, lR, sR);
                        switch (Show)
                        {
                              case DisplayStyle .Normal:
                                    g .DrawEllipse(pen, leftUpPX, leftUpPY, lR, sR);
                                    break;

                              case DisplayStyle .Hit:
                                    Pen newPen1 = new Pen(PenColor, 0.1f);
                                    newPen1 .DashStyle = DashStyle .Solid;
                                    g .DrawEllipse(newPen1, leftUpPX, leftUpPY, lR, sR);
                                    Pen newPen2 = new Pen(Color .Black, 1);
                                    newPen2 .DashStyle = DashStyle .Custom;
                                    newPen2 .DashPattern = new float[] { 3, 3 };
                                    g .DrawEllipse(newPen2, leftUpPX, leftUpPY, lR, sR);
                                    newPen1 .Dispose(); newPen2 .Dispose();
                                    break;

                              case DisplayStyle .Select:
                                    Pen newPen3 = new Pen(PenColor, 1);
                                    newPen3 .DashStyle = DashStyle .Custom;
                                    newPen3 .DashPattern = new float[] { 3, 3 };
                                    g .DrawEllipse(newPen3, leftUpPX, leftUpPY, lR, sR);
                                    RectangleF[] rects = new RectangleF[]
                                    {
                            new RectangleF(cep.X - 5, cep.Y - 5, 10, 10),
                            new RectangleF(leftUpPX - 5, leftUpPY  + sR / 2 - 5, 10, 10),
                            new RectangleF(leftUpPX + lR / 2 - 5, leftUpPY - 5, 10, 10),
                            new RectangleF(leftUpPX + lR - 5,  leftUpPY + sR / 2 - 5, 10, 10),
                            new RectangleF(leftUpPX + lR / 2 - 5,  leftUpPY + sR - 5, 10, 10),
                                    };
                                    g .FillRectangles(new SolidBrush(Color .FromArgb(0, 128, 255)), rects);
                                    g .DrawRectangles(new Pen(Color .Gray), rects);
                                    newPen3 .Dispose();
                                    break;
                        }
                        g .Transform = old;

                        //if (!IsBound)
                        //{
                        //    Matrix old2 = g.Transform;
                        //    g.ScaleTransform(CADInterface.globalZoomNum, CADInterface.globalZoomNum);
                        //    CreateObjects(false);
                        //    getBound(g);
                        //    g.DrawRectangle(pen, Bound.X, Bound.Y, Bound.Width, Bound.Height);
                        //    g.Transform = old2;
                        //    IsBound = true;
                        //}

                        #endregion
                  }
                  else
                  {
                        #region 不完整的椭圆
                        Matrix old = g .Transform;
                        g .ScaleTransform(zoomNum, zoomNum);

                        leftUpPX = 0;
                        leftUpPY = 0;
                        sR = 2 * shortRadius;
                        lR = 2 * longRadius;
                        cep .X = longRadius;
                        cep .Y = shortRadius;

                        g .MultiplyTransform(new Matrix(1, 0, 0, 1, centerPoint .X - longRadius, centerPoint .Y - shortRadius));

                        Matrix rotate = g .Transform;
                        rotate .RotateAt(Angle, centerPoint, MatrixOrder .Append);
                        g .Transform = rotate;

                        Pen pen = new Pen(this .PenColor, 0.1f);
                        pen .DashStyle = this .PenStyle;
                        //pen.
                        switch (Show)
                        {
                              case DisplayStyle .Normal:
                                    g .DrawArc(pen, leftUpPX, leftUpPY, lR, sR, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
                                    break;
                              case DisplayStyle .Edit:
                                    break;
                              case DisplayStyle .Hit:
                                    Pen newPen1 = new Pen(PenColor, 0.1f);
                                    newPen1 .DashStyle = DashStyle .Solid;
                                    g .DrawArc(newPen1, leftUpPX, leftUpPY, lR, sR, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
                                    Pen newPen2 = new Pen(Color .Black, 1);
                                    newPen2 .DashStyle = DashStyle .Custom;
                                    newPen2 .DashPattern = new float[] { 3, 3 };
                                    g .DrawArc(newPen2, leftUpPX, leftUpPY, lR, sR, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
                                    newPen1 .Dispose(); newPen2 .Dispose();
                                    break;
                              case DisplayStyle .Preview:
                                    break;
                              case DisplayStyle .Select:
                                    Pen newPen3 = new Pen(PenColor, 1);
                                    newPen3 .DashStyle = DashStyle .Custom;
                                    newPen3 .DashPattern = new float[] { 3, 3 };
                                    g .DrawArc(newPen3, leftUpPX, leftUpPY, lR, sR, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
                                    RectangleF[] rects = new RectangleF[]
                                    {
                            new RectangleF(cep.X - 5, cep.Y - 5, 10, 10),
                            new RectangleF(leftUpPX - 5, leftUpPY  + sR / 2 - 5, 10, 10),
                            new RectangleF(leftUpPX + lR / 2 - 5, leftUpPY - 5, 10, 10),
                            new RectangleF(leftUpPX + lR - 5,  leftUpPY + sR / 2 - 5, 10, 10),
                            new RectangleF(leftUpPX + lR / 2 - 5,  leftUpPY + sR - 5, 10, 10),
                                    };
                                    g .FillRectangles(new SolidBrush(Color .FromArgb(0, 128, 255)), rects);
                                    g .DrawRectangles(new Pen(Color .Gray), rects);
                                    newPen3 .Dispose();
                                    break;
                        }
                        g .Transform = old;

                        //if (!IsBound)
                        //{
                        //    Matrix old2 = g.Transform;
                        //    g.ScaleTransform(CADInterface.globalZoomNum, CADInterface.globalZoomNum);
                        //    CreateObjects(false);
                        //    getBound(g);
                        //    g.DrawRectangle(pen, Bound.X, Bound.Y, Bound.Width, Bound.Height);
                        //    g.Transform = old2;
                        //    IsBound = true;
                        //}

                        #endregion
                  }
            }

            /// <summary>
            /// 创建面域，计算面积true，计算范围false
            /// </summary>
            /// <param name="areaOrBound"></param>
            private void CreateObjects(bool areaOrBound)
            {
                  //AreaRegion.Dispose();
                  //AreaPath.Dispose();
                  //AreaPath = null;

                  //AreaPath = new GraphicsPath();
                  //AreaPen = new Pen(Color.White, 0);
                  //if (Complete)
                  //{
                  //    AreaPath.AddEllipse(leftUpPX, leftUpPY, 2 * LongRadius, 2 * ShortRadius);
                  //}
                  //else
                  //{
                  //    AreaPath.AddArc(leftUpPX, leftUpPY, 2 * LongRadius, 2 * ShortRadius, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
                  //}

                  //if (!areaOrBound)
                  //{
                  //    AreaPath.Widen(AreaPen);
                  //}

                  //Matrix matrix = new Matrix();
                  //matrix.Translate(centerPoint.X - longRadius, centerPoint.Y - shortRadius, MatrixOrder.Append);
                  //matrix.RotateAt(Angle, centerPoint, MatrixOrder.Append);
                  //AreaPath.Transform(matrix);
                  //AreaRegion = new Region(AreaPath);

            }

            private void getBound(Graphics g)
            {
                  Bound = AreaRegion .GetBounds(g);
            }

            public void getArea()
            {

                  //if (!IsArea && Complete) {
                  //    CreateObjects(true);

                  //    #region 面积
                  //    int scale = 5;
                  //    Matrix matrix = new Matrix();
                  //    matrix.Scale(scale, scale);
                  //    RectangleF[] listRectangle = AreaRegion.GetRegionScans(matrix);
                  //    Area = 0;
                  //    //MessageBox.Show(":"+ listRectangle.Length);
                  //    for (int i = 0; i < listRectangle.Length; i++)
                  //    {
                  //        Area = Area + listRectangle[i].Width * listRectangle[i].Height;
                  //    }
                  //    Area = Area / (scale * scale);

                  //    AreaRegion.Dispose();
                  //    AreaPath.Dispose();
                  //    AreaPath = null;
                  //    #endregion

                  //    IsArea = true;
                  //}
            }

            private void CreateObjects()
            {
                  if (AreaPath != null)
                  {
                        return;
                  }
                  AreaPath = new GraphicsPath();
                  if (Complete)
                  {
                        AreaPath .AddEllipse(leftUpPX, leftUpPY, 2 * LongRadius, 2 * ShortRadius);
                  }
                  else
                  {
                        AreaPath .AddArc(leftUpPX, leftUpPY, 2 * LongRadius, 2 * ShortRadius, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
                  }
                  AreaPen = new Pen(Color .White, 0);
                  AreaPath .Widen(AreaPen);

                  Matrix matrix = new Matrix();
                  matrix .Translate(centerPoint .X - longRadius, centerPoint .Y - shortRadius, MatrixOrder .Append);
                  matrix .RotateAt(Angle, centerPoint, MatrixOrder .Append);
                  AreaPath .Transform(matrix);

                  AreaRegion = new Region(AreaPath);
            }

            public override bool IntersectsWith(RectangleF rect)//反框选方法
            {
                  CreateObjects();
                  return AreaRegion .IsVisible(rect);
            }
      }
}
