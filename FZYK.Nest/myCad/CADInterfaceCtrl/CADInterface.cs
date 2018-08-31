using System;
using System .Drawing;
using System .Windows .Forms;
using System .Drawing .Drawing2D;
using myCad .Shape;
using System .Collections .Generic;
using myCad .Model;

namespace myCad .CADInterfaceCtrl
{
      public partial class CADInterface : UserControl
      {
            public CADInterface()
            {
                  InitializeComponent();
            }

            #region 参数
            public static BufferedGraphics bGrp = null;                                   //缓冲
            public static float globalZoomNum = 1;                                 //记录每次比例缩放后的总体比例系数。(变化比例)
            public static float scaleNum = 1;                                      //界面缩放比例
            public static RectangleF drawRegionRect;                               //
            public static List<BaseShape> currentShapes = new List<BaseShape>();   //当前界面上得图形
            public static List<BaseModel> currentPlates = new List<BaseModel>();   //当前界面上的钢板
            public static Stock nowStock = new Stock();                            //当前原材料钢板
            public static MRectangle selectRect = new MRectangle();
            public static int globalID = 0;                                         //记录线段的ID
            public static int globalModelID = 0;                                    //记录model的ID

            #endregion

            /// <summary>
            /// 重写加载
            /// </summary>
            /// <param name="e"></param>
            protected override void OnLoad(EventArgs e)
            {
                  base .OnLoad(e);
                  Graphics g = this .CreateGraphics();
                  bGrp = BufferedGraphicsManager .Current .Allocate(g, ClientRectangle);
            }

            /// <summary>
            /// 重写重绘
            /// </summary>
            /// <param name="e"></param>
            protected override void OnPaint(PaintEventArgs e)
            {
                  base .OnPaint(e);
                  DrawShap();
            }

            /// <summary>
            /// 窗体大小改变的时候
            /// </summary>
            /// <param name="e">画板</param>
            protected override void OnSizeChanged(EventArgs e)
            {
                  try
                  {
                        base .OnSizeChanged(e);
                        Graphics g = this .CreateGraphics();
                        bGrp = BufferedGraphicsManager .Current .Allocate(g, ClientRectangle);
                  }
                  catch { }
            }

            /// <summary>
            /// 重写鼠标移动
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMouseMove(MouseEventArgs e)
            {
                  base .OnMouseMove(e);
                  GetDrawRegionRect();
                  MouseShape .MousePoint = TransformMatrix .GetMousePoint(e .X, e .Y);
                  MouseShape .HitRect = new RectangleF(MouseShape .MousePoint .X - 3.5f / globalZoomNum, MouseShape .MousePoint .Y - 3.5f / globalZoomNum, 7 / globalZoomNum, 7 / globalZoomNum);
                  //SnapPointShape.SnapHitRect = new RectangleF(MouseShape.MousePoint.X - 7 / globalZoomNum, MouseShape.MousePoint.Y - 7 / globalZoomNum, 14 / globalZoomNum, 14 / globalZoomNum);
                  if (e .Button == MouseButtons .Middle)
                  {
                        MouseShape .MStyle = MouseStyle .None;
                        Matrix x = new Matrix(1, 0, 0, 1, MouseShape .MousePoint .X - MouseShape .OldMouse .X, MouseShape .MousePoint .Y - MouseShape .OldMouse .Y);
                        TransformMatrix .initMatrix .Multiply(x, MatrixOrder .Prepend);
                  }
                  DrawShap();
            }

            /// <summary>
            /// 重写鼠标点击事件，点击滚轮的时候出现移动小手
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMouseDown(MouseEventArgs e)
            {
                  base .OnMouseDown(e);
                  if (e .Button == MouseButtons .Middle)
                  {
                        this .Cursor = Cursors .Hand;
                        MouseShape .MStyle = MouseStyle .None;
                        MouseShape .OldMouse = MouseShape .MousePoint;
                  }
                  DrawShap();
            }

            /// <summary>
            /// 重写鼠标点击事件，离开点击滚轮的时候恢复原本的鼠标形态
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMouseUp(MouseEventArgs e)
            {
                  base .OnMouseUp(e);
                  if (e .Button == MouseButtons .Middle)
                  {
                        //this.Cursor = new Cursor("CursorBlank.cur");
                        this .Cursor = Cursors .Default;
                        MouseShape .MStyle = MouseShape .LastMStyle;
                  }
                  DrawShap();
            }

            /// <summary>
            /// 重写鼠标滚轮事件
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMouseWheel(MouseEventArgs e)
            {
                  base .OnMouseWheel(e);

                  if (e .Delta > 0)
                  {
                        scaleNum = 1.5f;
                        globalZoomNum *= 1.5f;
                  }
                  else
                  {
                        scaleNum = 0.67f;
                        globalZoomNum *= 0.67f;
                  }
                  MouseShape .HitRect = new RectangleF(MouseShape .MousePoint .X - 3.5f / globalZoomNum, MouseShape .MousePoint .Y - 3.5f / globalZoomNum, 7 / globalZoomNum, 7 / globalZoomNum);
                  bGrp .Graphics .Clear(this .BackColor);
                  bGrp .Graphics .Transform = TransformMatrix .ScaleByPoint(scaleNum, MouseShape .MousePoint);
                  //GetDrawRegionRect();
                  DrawShap();
            }

            /// <summary>
            /// 
            /// </summary>
            private void GetDrawRegionRect()
            {
                  PointF leftUpPot = TransformMatrix .GetMousePoint(0, 0);
                  PointF reightDownPot = TransformMatrix .GetMousePoint(this .Width, this .Height);
                  drawRegionRect = new RectangleF(leftUpPot .X, reightDownPot .Y, Math .Abs(reightDownPot .X - leftUpPot .X), Math .Abs(leftUpPot .Y - reightDownPot .Y));
            }

            /// <summary>
            /// 原始界面画图
            /// </summary>
            public static void DrawShap()
            {
                  try
                  {
                        bGrp .Graphics .Clear(Color .Black);

                        bGrp .Graphics .Transform = TransformMatrix .initMatrix;

                        GraphicsContainer drawContainer = bGrp .Graphics .BeginContainer();
                        bGrp .Graphics .ScaleTransform(1 / globalZoomNum, 1 / globalZoomNum);

                        foreach (BaseShape item in currentShapes)
                        {
                              if (!"Text" .Equals(item .ShapeClass))
                              {
                                    item .Draw(bGrp .Graphics);
                              }
                        }

                        if (nowStock .StockForm .Count > 0)
                        {
                              foreach (BaseShape item in nowStock .StockForm)
                              {
                                    item .Draw(bGrp .Graphics);
                              }
                        }

                        foreach (BaseModel item in currentPlates)
                        {
                              item .Draw(bGrp .Graphics);
                        }

                        //SnapPointShape.Draw(bGrp.Graphics);

                        selectRect .Draw(bGrp .Graphics);

                        CoordinateShape .Draw(bGrp .Graphics);

                        MouseShape .Draw(bGrp .Graphics);

                        bGrp .Graphics .EndContainer(drawContainer);

                        //bGrp.Render();

                        //bGrp.Dispose(); //这里释放资源，需要重新添加资源

                        foreach (BaseShape item in currentShapes)
                        {
                              if ("Text" .Equals(item .ShapeClass))
                              {
                                    item .Draw(bGrp .Graphics);
                              }
                        }

                        bGrp .Render();
                  }
                  catch (System .Exception e)
                  {
                        //MessageBox.Show("show show show");
                  }
            }
      }
}
