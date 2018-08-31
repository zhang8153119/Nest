using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseScaling
    {
        /// <summary>
        /// 基础缩放
        /// </summary>
        /// <param name="baseShape"></param>
        /// <param name="scalingPoint"></param>
        /// <param name="scale"></param>
        protected void scaling(BaseShape baseShape,PointF scalingPoint,float scale)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                scalingLine((Line)baseShape, scalingPoint, scale);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                scalingCircle((Circle)baseShape, scalingPoint, scale);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                scalingArc((Arc)baseShape, scalingPoint, scale);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                scalingEllipse((Ellipse)baseShape, scalingPoint, scale);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                scalingEllipseArc((Ellipse)baseShape, scalingPoint, scale);
            }
            else
            {
                //不判断的线段类型
            }
        }

        /// <summary>
        /// 直线缩放
        /// </summary>
        /// <param name="line"></param>
        /// <param name="scalingPoint"></param>
        /// <param name="scale"></param>
        private void scalingLine(Line line, PointF scalingPoint, float scale)
        {
            /* 缩放直线，以缩放点为圆心，收缩
             * **/
            line.StartPoint = new PointF(
               scalingPoint.X - scale * (scalingPoint.X - line.StartPoint.X),
               scalingPoint.Y - scale * (scalingPoint.Y - line.StartPoint.Y)
               );
            line.EndPoint = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - line.EndPoint.X),
                scalingPoint.Y - scale * (scalingPoint.Y - line.EndPoint.Y)
                );
        }

        /// <summary>
        /// 圆形缩放
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="scalingPoint"></param>
        /// <param name="scale"></param>
        private void scalingCircle(Circle circle, PointF scalingPoint, float scale)
        {
            /* 缩放圆形，以缩放点为缩放中心，缩放，圆半径按比例缩放
             * **/
            circle.Radius = circle.Radius * scale;

            circle.CenterPoint = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - circle.CenterPoint.X),
                scalingPoint.Y - scale * (scalingPoint.Y - circle.CenterPoint.Y)
                );
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
                circle.ListPoint[i] = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - circle.ListPoint[i].X),
                scalingPoint.Y - scale * (scalingPoint.Y - circle.ListPoint[i].Y)
                );
            }
        }

        /// <summary>
        /// 椭圆缩放
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="scalingPoint"></param>
        /// <param name="scale"></param>
        private void scalingEllipse(Ellipse ellipse, PointF scalingPoint, float scale)
        {
            ellipse.LongRadius = ellipse.LongRadius * scale;
            ellipse.ShortRadius = ellipse.ShortRadius * scale;
            ellipse.LongRadiusX = ellipse.LongRadiusX * scale;
            ellipse.LongRadiusY = ellipse.LongRadiusY * scale;

            ellipse.CenterPoint = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - ellipse.CenterPoint.X),
                scalingPoint.Y - scale * (scalingPoint.Y - ellipse.CenterPoint.Y)
                );

            ellipse.StartPoint = ellipse.EndPoint = new PointF(ellipse.CenterPoint.X + ellipse.LongRadiusX, ellipse.CenterPoint.Y + ellipse.LongRadiusY);

            for (int i = 0; i < ellipse.ListPoint.Count; i++)
            {
                ellipse.ListPoint[i] = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - ellipse.ListPoint[i].X),
                scalingPoint.Y - scale * (scalingPoint.Y - ellipse.ListPoint[i].Y)
                );
            }

        }

        /// <summary>
        /// 圆弧缩放
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="scalingPoint"></param>
        /// <param name="scale"></param>
        private void scalingArc(Arc arc, PointF scalingPoint, float scale)
        {
            arc.Radius = arc.Radius * scale;

            arc.CenterPoint = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - arc.CenterPoint.X),
                scalingPoint.Y - scale * (scalingPoint.Y - arc.CenterPoint.Y)
                );

            arc.EndPoint = new PointF(
                (float)(arc.CenterPoint.X + arc.Radius * Math.Cos(arc.EndAngle * (Math.PI / 180))),
                (float)(arc.CenterPoint.Y + arc.Radius * Math.Sin(arc.EndAngle * (Math.PI / 180))));
            arc.StartPoint = new PointF(
                (float)(arc.CenterPoint.X + arc.Radius * Math.Cos(arc.StartAngle * (Math.PI / 180))),
                (float)(arc.CenterPoint.Y + arc.Radius * Math.Sin(arc.StartAngle * (Math.PI / 180))));

            for (int i = 0; i < arc.ListPoint.Count; i++)
            {
                arc.ListPoint[i] = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - arc.ListPoint[i].X),
                scalingPoint.Y - scale * (scalingPoint.Y - arc.ListPoint[i].Y)
                );
            }
        }

        /// <summary>
        /// 椭圆弧缩放
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="scalingPoint"></param>
        /// <param name="scale"></param>
        private void scalingEllipseArc(Ellipse ellipse, PointF scalingPoint, float scale)
        {
            ellipse.LongRadius = ellipse.LongRadius * scale;
            ellipse.ShortRadius = ellipse.ShortRadius * scale;
            ellipse.LongRadiusX = ellipse.LongRadiusX * scale;
            ellipse.LongRadiusY = ellipse.LongRadiusY * scale;

            ellipse.CenterPoint = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - ellipse.CenterPoint.X),
                scalingPoint.Y - scale * (scalingPoint.Y - ellipse.CenterPoint.Y)
                );

            float originToSp = (float)Math.Sqrt(Math.Pow(ellipse.ShortRadius * Math.Sin(ellipse.StartParameter), 2) + Math.Pow(ellipse.LongRadius * Math.Cos(ellipse.StartParameter), 2));
            float originToEp = (float)Math.Sqrt(Math.Pow(ellipse.ShortRadius * Math.Sin(ellipse.EndParameter), 2) + Math.Pow(ellipse.LongRadius * Math.Cos(ellipse.EndParameter), 2));

            float spAngleToRotate = ellipse.Angle + ellipse.StartAngle > 360 ? ellipse.Angle + ellipse.StartAngle - 360 : ellipse.Angle + ellipse.StartAngle;
            float epAngleToRotate = ellipse.Angle + ellipse.EndAngle > 360 ? ellipse.Angle + ellipse.EndAngle - 360 : ellipse.Angle + ellipse.EndAngle;

            PointF newSp = new PointF();
            PointF newEp = new PointF();

            newSp.X = (float)(ellipse.CenterPoint.X + originToSp * Math.Cos(spAngleToRotate * (Math.PI / 180)));
            newSp.Y = (float)(ellipse.CenterPoint.Y + originToSp * Math.Sin(spAngleToRotate * (Math.PI / 180)));

            newEp.X = (float)(ellipse.CenterPoint.X + originToEp * Math.Cos(epAngleToRotate * (Math.PI / 180)));
            newEp.Y = (float)(ellipse.CenterPoint.Y + originToEp * Math.Sin(epAngleToRotate * (Math.PI / 180)));

            ellipse.StartPoint = newSp;
            ellipse.EndPoint = newEp;

            for (int i = 0; i < ellipse.ListPoint.Count; i++)
            {
                ellipse.ListPoint[i] = new PointF(
                scalingPoint.X - scale * (scalingPoint.X - ellipse.ListPoint[i].X),
                scalingPoint.Y - scale * (scalingPoint.Y - ellipse.ListPoint[i].Y)
                );
            }
        }
    }
}
