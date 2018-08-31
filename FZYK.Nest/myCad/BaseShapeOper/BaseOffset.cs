using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseOffset
    {
        protected void offset(BaseShape baseShape, float offsetLength)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                offsetLine((Line)baseShape,offsetLength);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                offsetCircle((Circle)baseShape, offsetLength);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                offsetArc((Arc)baseShape, offsetLength);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                offsetEllipse((Ellipse)baseShape, offsetLength);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                offsetEllipseArc((Ellipse)baseShape, offsetLength);
            }
            else
            {
                //不判断的线段类型
            }
        }

        /// <summary>
        /// 偏移直线
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offsetLength"></param>
        private void offsetLine(Line line, float offsetLength)
        {
            /* 直线偏移，小于的0的偏移，偏移往
             * **/
            if (line.StartPoint.Y == line.EndPoint.Y)
            {
                //直线平行于x轴，上方为正，下方为负
                line.StartPoint = new PointF(line.StartPoint.X, line.StartPoint.Y + offsetLength);
                line.EndPoint = new PointF(line.EndPoint.X, line.EndPoint.Y + offsetLength);
            }
            else if (line.StartPoint.X == line.EndPoint.X)
            {
                //直线平行于y轴，右方为正，左边为负
                line.StartPoint = new PointF(line.StartPoint.X + offsetLength, line.StartPoint.Y);
                line.EndPoint = new PointF(line.EndPoint.X + offsetLength, line.EndPoint.Y);
            }
            else
            {
                //倾斜的时候，上方为正，下方为负
                //角度在y轴的正半轴呈现正确的对应关系，所以角度都转化为0-180度,Math.Atan2 => -pi···pi
                float jiaoDu = (float)(Math.Atan2(line.EndPoint.Y - line.StartPoint.Y, line.EndPoint.X - line.StartPoint.X) + Math.PI / 2);
                //jiaoDu = jiaoDu < 0 ? (float)(jiaoDu + Math.PI * 2) : jiaoDu;
                //jiaoDu = jiaoDu > Math.PI ? (float)(jiaoDu - Math.PI) : jiaoDu;
                jiaoDu = jiaoDu < 0 ? (float)(jiaoDu + Math.PI ) : jiaoDu;
                line.StartPoint = new PointF((float)(line.StartPoint.X + Math.Cos(jiaoDu) * offsetLength), (float)(line.StartPoint.Y + Math.Sin(jiaoDu) * offsetLength));
                line.EndPoint = new PointF((float)(line.EndPoint.X + Math.Cos(jiaoDu) * offsetLength), (float)(line.EndPoint.Y + Math.Sin(jiaoDu) * offsetLength));
            }
        }

        /// <summary>
        /// 圆形偏移
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="offsetLength"></param>
        private void offsetCircle(Circle circle, float offsetLength)
        {
            circle.Radius = circle.Radius + offsetLength;
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
        /// 椭圆偏移
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="offsetLength"></param>
        private void offsetEllipse(Ellipse ellipse, float offsetLength)
        {
            ellipse.LongRadius = ellipse.LongRadius + offsetLength;
            ellipse.ShortRadius = ellipse.ShortRadius + offsetLength;

            float lRx = (float)(ellipse.LongRadius * Math.Cos(ellipse.Angle * (Math.PI / 180)));
            float lRy = (float)(ellipse.LongRadius * Math.Sin(ellipse.Angle * (Math.PI / 180)));

            ellipse.LongRadiusX = lRx;
            ellipse.LongRadiusY = lRy;

            ellipse.StartPoint = ellipse.EndPoint = new PointF(ellipse.CenterPoint.X + lRx, ellipse.CenterPoint.Y + lRy);

        }

        /// <summary>
        /// 圆弧偏移
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="offsetLength"></param>
        private void offsetArc(Arc arc, float offsetLength)
        {
            arc.Radius = arc.Radius + offsetLength;
            float spa = (float)(Math.PI / 180) * arc.StartAngle;
            float epa = (float)(Math.PI / 180) * arc.EndAngle;
            arc.StartPoint = new PointF((float)(arc.CenterPoint.X + arc.Radius * Math.Cos(spa)), (float)(arc.CenterPoint.Y + arc.Radius * Math.Sin(spa)));
            arc.EndPoint = new PointF((float)(arc.CenterPoint.X + arc.Radius * Math.Cos(epa)), (float)(arc.CenterPoint.Y + arc.Radius * Math.Sin(epa)));
        }

        /// <summary>
        /// 椭圆弧偏移
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="offsetLength"></param>
        private void offsetEllipseArc(Ellipse ellipse, float offsetLength)
        {
            ellipse.LongRadius = ellipse.LongRadius + offsetLength;
            ellipse.ShortRadius = ellipse.ShortRadius + offsetLength;

            float lRx = (float)(ellipse.LongRadius * Math.Cos(ellipse.Angle * (Math.PI / 180)));
            float lRy = (float)(ellipse.LongRadius * Math.Sin(ellipse.Angle * (Math.PI / 180)));

            ellipse.LongRadiusX = lRx;
            ellipse.LongRadiusY = lRy;

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
