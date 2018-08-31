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
      public abstract class BaseShape
      {
            private string shapeClass = "";                              //图形的类型
            private DisplayStyle show = DisplayStyle .Normal;             //操作状态
            private Color penColor = Color .White;                        //画笔颜色
            private DashStyle penStyle = DashStyle .Solid;                //画笔类型
            private int penWidth = 0;
            private int shapeID = 0;
            private GraphicsPath areaPath = null;                        //区域路径画图
            private Pen areaPen = null;                                  //区域路径画笔
            private Region areaRegion = null;                            //区域路径范围

            private PointF startPoint;                                   //起点坐标
            private PointF endPoint;                                     //终点坐标

            private RectangleF bound;                                    //图形的矩形包络

            private bool isSelected = false;                             //是否选中

            public abstract void Draw(Graphics g);                   //画图抽象接口

            public DisplayStyle Show
            {
                  get
                  {
                        return show;
                  }

                  set
                  {
                        show = value;
                  }
            }

            public Color PenColor
            {
                  get
                  {
                        return penColor;
                  }

                  set
                  {
                        penColor = value;
                  }
            }

            public DashStyle PenStyle
            {
                  get
                  {
                        return penStyle;
                  }

                  set
                  {
                        penStyle = value;
                  }
            }

            public string ShapeClass
            {
                  get
                  {
                        return shapeClass;
                  }

                  set
                  {
                        shapeClass = value;
                  }
            }

            public int ShapeID
            {
                  get
                  {
                        return shapeID;
                  }

                  set
                  {
                        shapeID = value;
                  }
            }

            public bool IsSelected
            {
                  get
                  {
                        return isSelected;
                  }

                  set
                  {
                        isSelected = value;
                  }
            }

            public GraphicsPath AreaPath
            {
                  get
                  {
                        return areaPath;
                  }

                  set
                  {
                        areaPath = value;
                  }
            }

            public Pen AreaPen
            {
                  get
                  {
                        return areaPen;
                  }

                  set
                  {
                        areaPen = value;
                  }
            }

            public Region AreaRegion
            {
                  get
                  {
                        return areaRegion;
                  }

                  set
                  {
                        areaRegion = value;
                  }
            }

            public RectangleF Bound
            {
                  get
                  {
                        return bound;
                  }

                  set
                  {
                        bound = value;
                  }
            }

            public PointF StartPoint
            {
                  get
                  {
                        return startPoint;
                  }

                  set
                  {
                        startPoint = new PointF(Round(value .X), Round(value .Y));
                  }
            }

            public PointF EndPoint
            {
                  get
                  {
                        return endPoint;
                  }

                  set
                  {
                        endPoint = new PointF(Round(value .X), Round(value .Y));
                  }
            }

            /// <summary>
            /// 判断点是否在方框内
            /// </summary>
            /// <param name="Point"></param>
            /// <param name="rect"></param>
            /// <returns></returns>
            public virtual bool IsInBox(PointF Point, RectangleF rect)
            {
                  float minX = rect .X;
                  float maxX = rect .X + rect .Width;
                  float minY = rect .Y;
                  float maxY = rect .Y + rect .Height;
                  if (Point .X > minX && Point .X < maxX && Point .Y > minY && Point .Y < maxY)
                  {
                        return true;
                  }
                  else
                  {
                        return false;
                  }
            }

            /// <summary>
            /// 判断是否和矩形区域相交
            /// </summary>
            /// <param name="rect"></param>
            /// <returns></returns>
            public virtual bool IntersectsWith(RectangleF rect)
            {
                  return false;
            }

            /// <summary>
            /// 包围范围
            /// </summary>
            /// <param name="rect"></param>
            /// <returns></returns>
            public virtual bool SurroundedBy(RectangleF rect)
            {
                  return false;
            }

            public float Round(float v)
            {
                  if (Math .Abs(v) < 0.00001)
                  {
                        return 0f;
                  }
                  return v;
            }
      }
}
