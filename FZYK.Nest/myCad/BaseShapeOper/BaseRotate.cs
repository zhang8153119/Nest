using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseRotate
    {

        /// <summary>
        /// 基础旋转,顺时针旋转的
        /// </summary>
        /// <param name="baseShape"></param>
        /// <param name="rotatePoint"></param>
        /// <param name="rotateAngle">角度以弧度的形式</param>
        protected void rotate(BaseShape baseShape, PointF rotatePoint, float rotateAngle)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                rotateLine((Line)baseShape, rotatePoint, rotateAngle);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                rotateCircle((Circle)baseShape, rotatePoint, rotateAngle);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                rotateArc((Arc)baseShape, rotatePoint, rotateAngle);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                rotateEllipse((Ellipse)baseShape, rotatePoint, rotateAngle);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                rotateEllipseArc((Ellipse)baseShape, rotatePoint, rotateAngle);
            }
            else
            {
                //不判断的线段类型
            }
        }

        protected PointF rotate(PointF point, PointF rotatePoint, float rotateAngle)
        {
            float pointToFp = (float)Math.Sqrt(Math.Pow(point.Y - rotatePoint.Y, 2) + Math.Pow(point.X - rotatePoint.X, 2));
            float fpAngle = (float)Math.Atan2(point.Y - rotatePoint.Y, point.X - rotatePoint.X);
            fpAngle = fpAngle - rotateAngle;
            point = new PointF(
                (float)(pointToFp * Math.Cos(fpAngle) + (rotatePoint.X - 0)),
                (float)(pointToFp * Math.Sin(fpAngle) + (rotatePoint.Y - 0))
                );
            return point;
        }
        /// <summary>
        /// 直线旋转方法
        /// </summary>
        /// <param name="line"></param>
        /// <param name="rotatePoint"></param>
        /// <param name="rotateAngle"></param>
        private void rotateLine(Line line, PointF rotatePoint, float rotateAngle)
        {
            /*
             * 以旋转点为原点，得出起点和终点以旋转点为圆心时的角度，以顺时针旋转为正，将先前得出的角度减去顺时针旋转的角度计算出旋转后的角度
             * 得出旋转后的角度后，以起点和终点到旋转的的距离计算出旋转后以旋转点为原点时的距离
             * 计算得出起点和终点的最终坐标值。
             * **/

            //线段起始点到旋转点的距离
            float pointToSp = (float)Math.Sqrt(Math.Pow(line.StartPoint.Y - rotatePoint.Y, 2) + Math.Pow(line.StartPoint.X - rotatePoint.X, 2));
            //以旋转点为原点的时候，线段起始点的角度
            float spAngle = (float)Math.Atan2(line.StartPoint.Y - rotatePoint.Y, line.StartPoint.X - rotatePoint.X);
            //旋转之后的角度
            spAngle = spAngle - rotateAngle;
            //获取以旋转点为原点时，旋转后的线段起始点的坐标，换算到原坐标轴，得到原坐标轴中线段起始点旋转后的角度
            line.StartPoint = new PointF(
                (float)(pointToSp * Math.Cos(spAngle) + (rotatePoint.X - 0)),
                (float)(pointToSp * Math.Sin(spAngle) + (rotatePoint.Y - 0))
                );

            float pointToEp = (float)Math.Sqrt(Math.Pow(line.EndPoint.Y - rotatePoint.Y, 2) + Math.Pow(line.EndPoint.X - rotatePoint.X, 2));
            float epAngle = (float)Math.Atan2(line.EndPoint.Y - rotatePoint.Y, line.EndPoint.X - rotatePoint.X);
            epAngle = epAngle - rotateAngle;
            line.EndPoint = new PointF(
                (float)(pointToEp * Math.Cos(epAngle) + (rotatePoint.X - 0)),
                (float)(pointToEp * Math.Sin(epAngle) + (rotatePoint.Y - 0))
                );

        }

        /// <summary>
        /// 圆旋转方法
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="rotatePoint"></param>
        /// <param name="rotateAngle"></param>
        private void rotateCircle(Circle circle, PointF rotatePoint, float rotateAngle)
        {
            float pointToCp = (float)Math.Sqrt(Math.Pow(circle.CenterPoint.Y - rotatePoint.Y,2)+Math.Pow(circle.CenterPoint.X - rotatePoint.X,2));
            float cpAngle = (float)Math.Atan2(circle.CenterPoint.Y - rotatePoint.Y,circle.CenterPoint.X - rotatePoint.X);
            cpAngle = cpAngle - rotateAngle;
            circle.CenterPoint = new PointF(
                (float)(pointToCp * Math.Cos(cpAngle) + (rotatePoint.X - 0)),
                (float)(pointToCp * Math.Sin(cpAngle) + (rotatePoint.Y - 0))
                );
            circle.StartPoint = new PointF(
                circle.CenterPoint.X + circle.Radius,
                circle.CenterPoint.Y
                );
            circle.EndPoint = new PointF(
                circle.CenterPoint.X + circle.Radius,
                circle.CenterPoint.Y
                );
        }

        /// <summary>
        /// 椭圆旋转方法
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="rotatePoint"></param>
        /// <param name="rotateAngle"></param>
        private void rotateEllipse(Ellipse ellipse, PointF rotatePoint, float rotateAngle)
        {
            /* 椭圆需要计算旋转后的圆心
             * 旋转后额椭圆旋转角度
             * **/

            //长轴点
            PointF longRadiusPoint = new PointF(
                ellipse.CenterPoint.X + ellipse.LongRadiusX, ellipse.CenterPoint.Y + ellipse.LongRadiusY
                );

            float pointToLRp = (float)Math.Sqrt(Math.Pow(longRadiusPoint.Y - rotatePoint.Y, 2) + Math.Pow(longRadiusPoint.X - rotatePoint.X, 2));
            float lRpAngle = (float)Math.Atan2(longRadiusPoint.Y - rotatePoint.Y, longRadiusPoint.X - rotatePoint.X);
            lRpAngle = lRpAngle - rotateAngle;

            longRadiusPoint = new PointF(
                (float)(pointToLRp * Math.Cos(lRpAngle) + (rotatePoint.X - 0)),
                (float)(pointToLRp * Math.Sin(lRpAngle) + (rotatePoint.Y - 0))
                );

            //计算旋转后的圆心
            float pointToCp = (float)Math.Sqrt(Math.Pow(ellipse.CenterPoint.Y - rotatePoint.Y, 2) + Math.Pow(ellipse.CenterPoint.X - rotatePoint.X, 2));
            float cpAngle = (float)Math.Atan2(ellipse.CenterPoint.Y - rotatePoint.Y, ellipse.CenterPoint.X - rotatePoint.X);
            cpAngle = cpAngle - rotateAngle;
            ellipse.CenterPoint = new PointF(
                (float)(pointToCp * Math.Cos(cpAngle) + (rotatePoint.X - 0)),
                (float)(pointToCp * Math.Sin(cpAngle) + (rotatePoint.Y - 0))
                );

            //旋转后椭圆的旋转角
            

            ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
            ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

            ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
            ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

            ellipse.StartPoint = ellipse.EndPoint = longRadiusPoint;
        }

        /// <summary>
        /// 圆弧旋转方法,顺时真为正，逆时针为负
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="rotatePoint"></param>
        /// <param name="rotateAngle"></param>
        private void rotateArc(Arc arc, PointF rotatePoint, float rotateAngle)
        {
            float pointToCp = (float)Math.Sqrt(Math.Pow(arc.CenterPoint.Y - rotatePoint.Y, 2) + Math.Pow(arc.CenterPoint.X - rotatePoint.X, 2));
            float cpAngle = (float)Math.Atan2(arc.CenterPoint.Y - rotatePoint.Y, arc.CenterPoint.X - rotatePoint.X);
            cpAngle = cpAngle - rotateAngle ;
            arc.CenterPoint = new PointF(
                (float)(pointToCp * Math.Cos(cpAngle) + (rotatePoint.X - 0)),
                (float)(pointToCp * Math.Sin(cpAngle) + (rotatePoint.Y - 0))
                );
            //通过圆弧的起始角计算圆弧的起始点
            PointF sp = arc.StartPoint;
            float pointToSp = (float)Math.Sqrt(Math.Pow(sp.Y - rotatePoint.Y, 2) + Math.Pow(sp.X - rotatePoint.X, 2));
            float spAngle = (float)Math.Atan2(sp.Y - rotatePoint.Y, sp.X - rotatePoint.X);
            spAngle = spAngle - rotateAngle ;
            sp = new PointF(
                (float)(pointToSp * Math.Cos(spAngle) + (rotatePoint.X - 0)),
                (float)(pointToSp * Math.Sin(spAngle) + (rotatePoint.Y - 0))
                );
            //计算出旋转过后的圆弧起始角度
            arc.StartAngle = (float)(Math.Atan2(sp.Y - arc.CenterPoint.Y,sp.X - arc.CenterPoint.X) * (180/Math.PI));
            arc.StartAngle = arc.StartAngle < 0 ? arc.StartAngle + 360 : arc.StartAngle;
            arc.StartPoint = sp;

            //通过圆弧的终止角计算圆弧的终止点
            PointF ep = arc.EndPoint;
            float pointToEp = (float)Math.Sqrt(Math.Pow(ep.Y - rotatePoint.Y, 2) + Math.Pow(ep.X - rotatePoint.X, 2));
            float epAngle = (float)Math.Atan2(ep.Y - rotatePoint.Y, ep.X - rotatePoint.X);
            epAngle = epAngle - rotateAngle ;
            ep = new PointF(
                (float)(pointToEp * Math.Cos(epAngle) + (rotatePoint.X - 0)),
                (float)(pointToEp * Math.Sin(epAngle) + (rotatePoint.Y - 0))
                );

            //计算出旋转后的圆弧终止角度
            arc.EndAngle = (float)(Math.Atan2(ep.Y - arc.CenterPoint.Y, ep.X - arc.CenterPoint.X) * (180 / Math.PI));
            arc.EndAngle = arc.EndAngle < 0 ? arc.EndAngle + 360 :arc.EndAngle;
            arc.EndPoint = ep;
        }

        /// <summary>
        /// 椭圆弧旋转方法
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="rotatePoint"></param>
        /// <param name="rotateAngle"></param>
        private void rotateEllipseArc(Ellipse ellipse, PointF rotatePoint, float rotateAngle)
        {
            /* 椭圆需要计算旋转后的圆心
             * 旋转后额椭圆旋转角度
             * **/

            PointF longRadiusPoint = new PointF(
               ellipse.CenterPoint.X + ellipse.LongRadiusX, ellipse.CenterPoint.Y + ellipse.LongRadiusY
               );

            float pointToLRp = (float)Math.Sqrt(Math.Pow(longRadiusPoint.Y - rotatePoint.Y, 2) + Math.Pow(longRadiusPoint.X - rotatePoint.X, 2));
            float lRpAngle = (float)Math.Atan2(longRadiusPoint.Y - rotatePoint.Y, longRadiusPoint.X - rotatePoint.X);
            lRpAngle = lRpAngle - rotateAngle;
            longRadiusPoint = new PointF(
                (float)(pointToLRp * Math.Cos(lRpAngle) + (rotatePoint.X - 0)),
                (float)(pointToLRp * Math.Sin(lRpAngle) + (rotatePoint.Y - 0))
                );

            //计算旋转后的圆心
            float pointToCp = (float)Math.Sqrt(Math.Pow(ellipse.CenterPoint.Y - rotatePoint.Y, 2) + Math.Pow(ellipse.CenterPoint.X - rotatePoint.X, 2));
            float cpAngle = (float)Math.Atan2(ellipse.CenterPoint.Y - rotatePoint.Y, ellipse.CenterPoint.X - rotatePoint.X);
            cpAngle = cpAngle - rotateAngle;
            ellipse.CenterPoint = new PointF(
                (float)(pointToCp * Math.Cos(cpAngle) + (rotatePoint.X - 0)),
                (float)(pointToCp * Math.Sin(cpAngle) + (rotatePoint.Y - 0))
                );

            //旋转后椭圆的旋转角
            //长轴点，需要获取旋转变化前的点

            ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
            ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

            ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
            ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

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
        }
    }
}
