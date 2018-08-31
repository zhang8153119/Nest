using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseMove
    {
        /// <summary>
        /// 移动点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        /// <returns></returns>
        protected PointF move(PointF point, float moveX, float moveY)
        {
            point = new PointF(point.X + moveX, point.Y + moveY);
            return point;
        }
        /// <summary>
        /// 基本移动接口
        /// </summary>
        /// <param name="baseShape"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        /// 
        protected void move(BaseShape baseShape,float moveX,float moveY)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                moveLine((Line)baseShape, moveX, moveY);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                moveCircle((Circle)baseShape, moveX, moveY);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                moveArc((Arc)baseShape, moveX, moveY);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                moveEllipse((Ellipse)baseShape, moveX, moveY);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                moveEllipseArc((Ellipse)baseShape, moveX, moveY);
            }
            else {
                //其他类型的图形不判断不移动
            }
        }

        /// <summary>
        /// 直线移动
        /// </summary>
        /// <param name="line"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        private void moveLine(Line line,float moveX,float moveY)
        {
            line.StartPoint = new PointF(line.StartPoint.X + moveX, line.StartPoint.Y + moveY);
            line.EndPoint = new PointF(line.EndPoint.X + moveX, line.EndPoint.Y + moveY);
        }

        /// <summary>
        /// 圆移动
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        private void moveCircle(Circle circle, float moveX, float moveY)
        {
            circle.CenterPoint = new PointF(circle.CenterPoint.X + moveX, circle.CenterPoint.Y + moveY);

            circle.StartPoint = new PointF(
                circle.CenterPoint.X + circle.Radius,
                circle.CenterPoint.Y
                );
            circle.EndPoint = new PointF(
                circle.CenterPoint.X + circle.Radius,
                circle.CenterPoint.Y
                );

            for (int i = 0; i < circle.ListPoint.Count; i++)
            {
                circle.ListPoint[i] = new PointF(circle.ListPoint[i].X + moveX, circle.ListPoint[i].Y + moveY);
            }
        }

        /// <summary>
        /// 椭圆移动
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        private void moveEllipse(Ellipse ellipse, float moveX, float moveY)
        {
            ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X + moveX, ellipse.CenterPoint.Y + moveY);
            ellipse.StartPoint = new PointF(ellipse.StartPoint.X + moveX, ellipse.StartPoint.Y + moveY);
            ellipse.EndPoint = new PointF(ellipse.EndPoint.X + moveX, ellipse.EndPoint.Y + moveY);

            for (int i = 0; i < ellipse.ListPoint.Count; i++)
            {
                ellipse.ListPoint[i] = new PointF(ellipse.ListPoint[i].X + moveX, ellipse.ListPoint[i].Y + moveY);
            }
        }

        /// <summary>
        /// 圆弧移动
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        private void moveArc(Arc arc, float moveX, float moveY)
        {
            arc.CenterPoint = new PointF(arc.CenterPoint.X + moveX, arc.CenterPoint.Y + moveY);
            arc.StartPoint = new PointF(arc.StartPoint.X + moveX, arc.StartPoint.Y + moveY);
            arc.EndPoint = new PointF(arc.EndPoint.X + moveX, arc.EndPoint.Y + moveY);

            for (int i = 0; i < arc.ListPoint.Count; i++)
            {
                arc.ListPoint[i] = new PointF(arc.ListPoint[i].X + moveX, arc.ListPoint[i].Y + moveY);
            }
        }

        /// <summary>
        /// 椭圆弧移动
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="moveX"></param>
        /// <param name="moveY"></param>
        private void moveEllipseArc(Ellipse ellipse, float moveX, float moveY)
        {
            ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X + moveX, ellipse.CenterPoint.Y + moveY);
            ellipse.StartPoint = new PointF(ellipse.StartPoint.X + moveX, ellipse.StartPoint.Y + moveY);
            ellipse.EndPoint = new PointF(ellipse.EndPoint.X + moveX, ellipse.EndPoint.Y + moveY);

            for (int i = 0; i < ellipse.ListPoint.Count; i++)
            {
                ellipse.ListPoint[i] = new PointF(ellipse.ListPoint[i].X + moveX, ellipse.ListPoint[i].Y + moveY);
            }
        }

    }
}
