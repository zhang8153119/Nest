using myCad .CADInterfaceCtrl;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;
using System .Windows .Forms;

namespace myCad .DrawTools
{
      public class BaseTool
      {

            protected static bool isHited = false;              //是否已经存在碰撞
            protected RectangleF positiveRect, negativeRect;    //正选反选框
            public static int count = 0;                        //取消工具的时候判断是否有正在画的图形，有则删除这个正在画的图形
                                                                // protected static bool shapesSel = false;//判断是否有图元被选取

            //动态选择，显示高亮但是不是选中
            public virtual void getHit()
            {
                  if (CADInterface .currentShapes == null) return;
                  for (int i = CADInterface .currentShapes .Count - 1; i >= 0; i--)//在图元重叠或相交处拾取时，最后画的先拾取。
                  {
                        if (CADInterface .currentShapes[i] .IsSelected == true) continue;
                        if (CADInterface .currentShapes[i] .IntersectsWith(MouseShape .HitRect))
                        {
                              CADInterface .currentShapes[i] .Show = DisplayStyle .Hit;
                              isHited = true;
                              return;
                        }
                        else
                        {
                              CADInterface .currentShapes[i] .Show = DisplayStyle .Normal;
                              isHited = false;
                        }
                  }
            }

            //点选图形
            public virtual void PointSelect()
            {
                  if (CADInterface .currentShapes == null) return;

                  //后画的优先被选中
                  for (int i = CADInterface .currentShapes .Count - 1; i >= 0; i--)
                  {
                        if (CADInterface .currentShapes[i] .IntersectsWith(MouseShape .HitRect))
                        {
                              CADInterface .currentShapes[i] .Show = DisplayStyle .Edit;
                              CADInterface .currentShapes[i] .IsSelected = true;
                              isHited = false;
                              return;
                        }
                  }
            }

            //正框选图元（图元必须整个被包围在矩形框内方可被选中）
            public virtual void RectSelectPositive()
            {
                  if (CADInterface .currentShapes == null) return;

                  for (int i = CADInterface .currentShapes .Count - 1; i >= 0; i--)
                  {
                        if (CADInterface .currentShapes[i] .SurroundedBy(positiveRect))
                        {
                              CADInterface .currentShapes[i] .Show = DisplayStyle .Edit;
                              CADInterface .currentShapes[i] .IsSelected = true;
                        }
                  }
            }

            //反框选图元（图元部分被包围在矩形框内即可被选中）
            public virtual void RectSelectNegative()
            {
                  if (CADInterface .currentShapes == null) return;

                  for (int i = CADInterface .currentShapes .Count - 1; i >= 0; i--)
                  {
                        if (CADInterface .currentShapes[i] .IntersectsWith(negativeRect))
                        {
                              CADInterface .currentShapes[i] .Show = DisplayStyle .Edit;
                              CADInterface .currentShapes[i] .IsSelected = true;
                        }
                  }
            }

            public virtual void MouseDown(MouseEventArgs e)
            {
                  //if (e.Button == MouseButtons.Right && count == 1)         //调试时的操作与ESC键效果一致，待修改。
                  //{
                  //    UnLoadTool();
                  //}
            }

            public virtual void MouseMove(MouseEventArgs e)
            {

            }

            public virtual void KeyDown(KeyEventArgs e)
            {
                  if (e .KeyCode == Keys .Escape)
                  {
                        foreach (BaseShape item in CADInterface .currentShapes)
                        {
                              item .IsSelected = false;
                              item .Show = DisplayStyle .Normal;
                        }

                        //UnLoadTool();
                  }
            }

            public virtual void LoadTool()
            {
                  foreach (BaseShape item in CADInterface .currentShapes)
                  {
                        item .IsSelected = false;
                        item .Show = DisplayStyle .Normal;
                  }
                  CADInterface .DrawShap();
            }

            public virtual void UnLoadTool()
            {
                  switch (count)
                  {
                        case 0:

                              break;

                        case 1:
                              if (CADInterface .currentShapes .Count == 0) return;
                              CADInterface .currentShapes .RemoveAt(CADInterface .currentShapes .Count - 1);
                              count = 0;
                              break;
                  }
                  MouseShape .MStyle = MouseStyle .Normal;
                  //SnapPointShape.SPStyle = SnapPointStyle.None;
                  //FormDoc.currentTool = FormDoc.registerTool["NOTOOLLOAD"];
                  //shapesSel = false;
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
