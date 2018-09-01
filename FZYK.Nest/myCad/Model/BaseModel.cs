using myCad .CADInterfaceCtrl;
using myCad .Shape;
using myCad .ShapeOper;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .Model
{
      public class BaseModel
      {

            private int modelId = 0;               //modelID
            private float area = 0;             //面积
            private float perimeter;            //周长
            private List<BaseShape> listShape = new List<BaseShape>();       //模型的线段
            private GraphicsPath graphicPath = new GraphicsPath();                         //模型的路径
            private Region pathRegion = new Region();                                     //路径的面域
            private bool isInner = false;       //是否内部图形
            private RectangleF bound;            //模型的范围
            private bool isBound = false;
            private bool isArea = false;
            private Pen pen = new Pen(Color .Yellow);
            private bool isArc = false;                //判断这个模型是不是带圆弧的模型
            private List<PointF> listPoint = new List<PointF>();   //模型的顶点
            private bool clockwise = false;                   //模型的顶点是顺时针还是逆时针，false代表逆时针，true代表顺时针

            public List<BaseShape> ExpandShape { get; set; } = new List<BaseShape>();
            public List<PointF> ExpandPoint { get; set; } = new List<PointF>();
            public virtual void Draw(Graphics g,float zoomNum)
            {
                  if (!IsBound)
                  {
                        Matrix old = g .Transform;
                        g .ScaleTransform(zoomNum, zoomNum);
                        GraphicPath = new CreateRegion() .getModelPath(listShape);
                        //GraphicPath.Flatten();
                        pathRegion = new CreateRegion() .getModelRegion(GraphicPath);
                        //pathRegion.GetRegionData();
                        Bound = new CreateBound() .getModelBound(GraphicPath, g);
                        if (!IsArea)
                        {
                              g .FillPath(Brushes .Pink, GraphicPath);
                              Area = 0;
                              int scale = 5;
                              Matrix matrix = new Matrix();
                              matrix .Scale(scale, scale);
                              RectangleF[] rtf = pathRegion .GetRegionScans(matrix);
                              for (int i = 0; i < rtf .Length; i++)
                              {
                                    Area = Area + rtf[i] .Width * rtf[i] .Height;
                              }
                              Area = Area / (scale * scale);
                              IsArea = true;
                        }
                        //g.DrawRectangle(pen, Bound.X, Bound.Y, Bound.Width, Bound.Height);
                        g .Transform = old;

                        //pathRegion.Dispose();
                        GraphicPath .Dispose();
                        IsBound = true;
                  }
                  #region 测试点的集合是否正确
                  //Matrix old1 = g.Transform;
                  //g.ScaleTransform(CADInterface.globalZoomNum, CADInterface.globalZoomNum);
                  //for (int i = 0; i < ListPoint.Count; i++)
                  //{
                  //    if (i == ListPoint.Count - 1)
                  //    {
                  //        g.DrawLine(new Pen(Color.Red, 0.01f), ListPoint[i], ListPoint[0]);
                  //    }
                  //    else if (i == 0)
                  //    {
                  //        g.DrawLine(new Pen(Color.Green, 0.01f), ListPoint[i], ListPoint[i + 1]);
                  //    }
                  //    else
                  //    {
                  //        g.DrawLine(new Pen(Color.Yellow, 0.01f), ListPoint[i], ListPoint[i + 1]);
                  //    }
                  //}
                  //g.Transform = old1;
                  #endregion
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

            public List<BaseShape> ListShape
            {
                  get
                  {
                        return listShape;
                  }

                  set
                  {
                        listShape = value;
                  }
            }

            public bool IsInner
            {
                  get
                  {
                        return isInner;
                  }

                  set
                  {
                        isInner = value;
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

            public GraphicsPath GraphicPath
            {
                  get
                  {
                        return graphicPath;
                  }

                  set
                  {
                        graphicPath = value;
                  }
            }

            public bool IsArc
            {
                  get
                  {
                        return isArc;
                  }

                  set
                  {
                        isArc = value;
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

            public bool Clockwise
            {
                  get
                  {
                        return clockwise;
                  }

                  set
                  {
                        clockwise = value;
                  }
            }

            public int ModelId
            {
                  get
                  {
                        return modelId;
                  }

                  set
                  {
                        modelId = value;
                  }
            }
      }
}
