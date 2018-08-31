using myCad.CADInterfaceCtrl;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    public class BaseDistance
    {
        /// <summary>
        /// 计算点到线的距离
        /// </summary>
        /// <param name="baseShape"></param>
        /// <param name="pointF"></param>
        /// <param name="tag">1代表X轴方向的距离，2代表Y轴方向的距离</param>
        /// <returns></returns>
        public float calculateDistance(BaseShape baseShape, PointF point, int tag)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                return pointToLine((Line)baseShape, point, tag);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                return pointToCircle((Circle)baseShape, point, tag);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                return pointToArc((Arc)baseShape, point, tag);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                return pointToEllipse((Ellipse)baseShape, point, tag);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                return pointToEllipseArc((Ellipse)baseShape, point, tag);
            }
            else
            {
                //不判断的线段类型
                return -9999999;
            }
        }

        /// <summary>
        /// 求解点对应x轴和y轴方向和直线的交点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private float pointToLine(Line line, PointF point, int tag)
        {
            /*利用两点式求取直线方程带入对应的x或y值求取y轴方向或x轴方向的距离，不存在交点则返回一个无线小的数-9999999
             * **/
            if (tag == 1)
            {
                if ((point.Y >= line.StartPoint.Y && point.Y <= line.EndPoint.Y) || (point.Y <= line.StartPoint.Y && point.Y >= line.EndPoint.Y))
                {
                    //存在水平交点
                    float x = (point.Y - line.StartPoint.Y) * (line.EndPoint.X - line.StartPoint.X) / (line.EndPoint.Y - line.StartPoint.Y) + line.StartPoint.X;
                    return point.X - x;
                }
                else
                {
                    //不存在水平交点
                    return -9999999;
                }
            }
            else
            {
                if ((point.X >= line.StartPoint.X && point.X <= line.EndPoint.X) || (point.X <= line.StartPoint.X && point.X >= line.EndPoint.X))
                {
                    //存在交点
                    float y = (point.X - line.StartPoint.X) * (line.EndPoint.Y - line.StartPoint.Y) / (line.EndPoint.X - line.StartPoint.X) + line.StartPoint.Y;
                    return point.Y - y;
                }
                else
                {
                    //不存在交点
                    return -9999999;
                }
            }
        }

        /// <summary>
        /// 求解点对应x轴和y轴方向和圆的交点
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private float pointToCircle(Circle circle, PointF point, int tag)
        {
            /* 利用圆的位置关系求解点在x轴和y轴上和圆的关系，得出点在X轴和y轴上和圆的交点求出最近的距离，不存在交点则返回一个无线小的数-9999999
             * (x-a)(x-a)+(y-b)(y-b) = r*r
             * **/
            if (tag == 1)
            {
                if (point.Y <= circle.CenterPoint.Y + circle.Radius && point.Y >= circle.CenterPoint.Y - circle.Radius)
                {
                    //存在交点
                    //圆方程开根号
                    float yuanGen = (float)Math.Sqrt(circle.Radius * circle.Radius - (point.Y - circle.CenterPoint.Y) * (point.Y - circle.CenterPoint.Y));
                    float maxX = yuanGen + circle.CenterPoint.X;
                    float minX = -yuanGen + circle.CenterPoint.X;

                    return Math.Abs(point.X - maxX) > Math.Abs(point.X - minX) ? point.X - minX : point.X - maxX;
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
            else
            {
                if (point.X <= circle.CenterPoint.X + circle.Radius && point.X >= circle.CenterPoint.X - circle.Radius)
                {
                    //存在交点
                    float yuanGen = (float)Math.Sqrt(circle.Radius * circle.Radius - (point.X - circle.CenterPoint.X) * (point.X - circle.CenterPoint.X));
                    float maxY = yuanGen + circle.CenterPoint.Y;
                    float minY = -yuanGen + circle.CenterPoint.Y;

                    return Math.Abs(point.Y - maxY) > Math.Abs(point.Y - minY) ? point.Y - minY : point.Y - maxY;
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
        }

        /// <summary>
        /// 求解点对应x轴和y轴方向和椭圆的交点
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="point"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private float pointToEllipse(Ellipse ellipse, PointF point, int tag)
        {
            if (tag == 1)
            {
                if (point.Y >= ellipse.Bound.Y && point.Y <= ellipse.Bound.Y + ellipse.Bound.Height)
                {
                    //存在交点
                    //求X值
                    //(xcosθ + ysinθ)²/longRadius² + (ycosθ - xsinθ)²/shortRadius² = 1
                    //ax² + bx + c = 0
                    //a = shortRadius²*cos²θ+longRadius²*sin²θ;
                    //b = 2*y*（shortRadius²cosθsinθ-longRadius²cosθsinθ）
                    //c = y²*（shortRadius²sin²θ+longRadius²cos²θ）-longRadius²shortRadius²
                    //d = b²-4ac
                    //xInter =  (-b±√d）/ 2a;
                    float angle = (float)(ellipse.Angle * (Math.PI / 180));
                    float a = (float)(Math.Pow(ellipse.ShortRadius * Math.Cos(angle), 2) + Math.Pow(ellipse.LongRadius * Math.Sin(angle), 2));
                    float b = (float)(2 * (point.Y - ellipse.CenterPoint.Y) * Math.Cos(angle) * Math.Sin(angle) * (Math.Pow(ellipse.ShortRadius, 2) - Math.Pow(ellipse.LongRadius, 2)));
                    float c = (float)(Math.Pow((point.Y - ellipse.CenterPoint.Y) * ellipse.ShortRadius * Math.Sin(angle), 2) + Math.Pow((point.Y - ellipse.CenterPoint.Y) * ellipse.LongRadius * Math.Cos(angle), 2) - Math.Pow(ellipse.LongRadius * ellipse.ShortRadius, 2));
                    float d = (float)(Math.Pow(b, 2) - 4 * a * c);
                    float maxX = (float)((-b + Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.X);
                    float minX = (float)((-b - Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.X);

                    return Math.Abs(point.X - maxX) > Math.Abs(point.X - minX) ? point.X - minX : point.X - maxX;
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
            else
            {
                if (point.X <= ellipse.Bound.X + ellipse.Bound.Width && point.Y >= ellipse.Bound.X)
                {
                    //存在交点
                    //求Y值
                    //(xcosθ + ysinθ)²/longRadius² + (ycosθ - xsinθ)²/shortRadius² = 1
                    //ay² + by + c = 0
                    //a = shortRadius²*sin²θ+longRadius²*cos²θ;
                    //b = 2*x*（shortRadius²cosθsinθ-longRadius²cosθsinθ）
                    //c = x²*（shortRadius²*cos²θ+longRadius²*sin²θ）-longRadius²shortRadius²
                    //d = b²-4ac
                    //yInter =  (-b±√d）/ 2a;
                    float angle = (float)(ellipse.Angle * (Math.PI / 180));
                    float a = (float)(Math.Pow(ellipse.LongRadius * Math.Cos(angle), 2) + Math.Pow(ellipse.ShortRadius * Math.Sin(angle), 2));
                    float b = (float)(2 * (point.X - ellipse.CenterPoint.X) * Math.Cos(angle) * Math.Sin(angle) * (Math.Pow(ellipse.ShortRadius, 2) - Math.Pow(ellipse.LongRadius, 2)));
                    float c = (float)(Math.Pow((point.X - ellipse.CenterPoint.X) * ellipse.LongRadius * Math.Sin(angle), 2) + Math.Pow((point.X - ellipse.CenterPoint.X) * ellipse.ShortRadius * Math.Cos(angle), 2) - Math.Pow(ellipse.LongRadius * ellipse.ShortRadius, 2));
                    float d = (float)(Math.Pow(b, 2) - 4 * a * c);
                    float maxY = (float)((-b + Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.Y);
                    float minY = (float)((-b - Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.Y);

                    return Math.Abs(point.Y - maxY) > Math.Abs(point.Y - minY) ? point.Y - minY : point.Y - maxY;
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
        }

        /// <summary>
        /// 求解点对应x轴和y轴方向和圆弧的交点
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="point"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private float pointToArc(Arc arc, PointF point, int tag)
        {
            /* 利用圆的位置关系求解点在x轴和y轴上和圆的关系，得出点在X轴和y轴上和圆的交点
             * 判断交点是否在圆弧上,在圆弧上返回最短的距离
             * 不存在交点则返回一个无线小的数-9999999
             * (x-a)(x-a)+(y-b)(y-b) = r*r
             * **/
            if (tag == 1)
            {
                if (point.Y <= arc.CenterPoint.Y + arc.Radius && point.Y >= arc.CenterPoint.Y - arc.Radius)
                {
                    //圆存在交点，得出交点判断是否在圆弧上
                    float yuanGen = (float)Math.Sqrt(arc.Radius * arc.Radius - (point.Y - arc.CenterPoint.Y) * (point.Y - arc.CenterPoint.Y));
                    float maxX = yuanGen + arc.CenterPoint.X;
                    float minX = -yuanGen + arc.CenterPoint.X;

                    float pointOneAngle = (float)((180 / Math.PI) * Math.Atan2(point.Y - arc.CenterPoint.Y, maxX - arc.CenterPoint.X));
                    pointOneAngle = pointOneAngle < 0 ? 360 + pointOneAngle : pointOneAngle;
                    float pointTwoAngle = (float)((180 / Math.PI) * Math.Atan2(point.Y - arc.CenterPoint.Y, minX - arc.CenterPoint.X));
                    pointTwoAngle = pointTwoAngle < 0 ? 360 + pointTwoAngle : pointTwoAngle;
                    if (arc.StartAngle < arc.EndAngle)
                    {
                        #region 起始角度小于终止角度
                        if ((pointOneAngle >= arc.StartAngle && pointOneAngle <= arc.EndAngle) && (pointTwoAngle >= arc.StartAngle && pointTwoAngle <= arc.EndAngle))
                        {
                            return Math.Abs(point.X - maxX) > Math.Abs(point.X - minX) ? point.X - minX : point.X - maxX;
                        }
                        else if ((pointOneAngle >= arc.StartAngle && pointOneAngle <= arc.EndAngle) && !(pointTwoAngle >= arc.StartAngle && pointTwoAngle <= arc.EndAngle))
                        {
                            return point.X - maxX;
                        }
                        else if (!(pointOneAngle >= arc.StartAngle && pointOneAngle <= arc.EndAngle) && (pointTwoAngle >= arc.StartAngle && pointTwoAngle <= arc.EndAngle))
                        {
                            return point.X - minX;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else if (arc.StartAngle >= arc.EndAngle)
                    {
                        #region 起始角度大于终止角度
                        if (((pointOneAngle >= arc.StartAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= arc.EndAngle))
                           && ((pointTwoAngle >= arc.StartAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= arc.EndAngle)))
                        {
                            return Math.Abs(point.X - maxX) > Math.Abs(point.X - minX) ? point.X - minX : point.X - maxX;
                        }
                        else if (((pointOneAngle >= arc.StartAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= arc.EndAngle))
                           && !((pointTwoAngle >= arc.StartAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= arc.EndAngle)))
                        {
                            return point.X - maxX;
                        }
                        else if (!((pointOneAngle >= arc.StartAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= arc.EndAngle))
                           && ((pointTwoAngle >= arc.StartAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= arc.EndAngle)))
                        {
                            return point.X - minX;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else
                    {
                        //不存在交点
                        return -9999999;
                    }
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
            else
            {
                if (point.X <= arc.CenterPoint.X + arc.Radius && point.X >= arc.CenterPoint.X - arc.Radius)
                {
                    //圆存在交点，得出交点判断是否在圆弧上
                    float yuanGen = (float)Math.Sqrt(arc.Radius * arc.Radius - (point.X - arc.CenterPoint.X) * (point.X - arc.CenterPoint.X));
                    float maxY = yuanGen + arc.CenterPoint.Y;
                    float minY = -yuanGen + arc.CenterPoint.Y;

                    float pointOneAngle = (float)((180 / Math.PI) * Math.Atan2(maxY - arc.CenterPoint.Y, point.X - arc.CenterPoint.X));
                    pointOneAngle = pointOneAngle < 0 ? 360 + pointOneAngle : pointOneAngle;
                    float pointTwoAngle = (float)((180 / Math.PI) * Math.Atan2(minY - arc.CenterPoint.Y, point.X - arc.CenterPoint.X));
                    pointTwoAngle = pointTwoAngle < 0 ? 360 + pointTwoAngle : pointTwoAngle;

                    if (arc.StartAngle < arc.EndAngle)
                    {
                        #region 起始角度小于终止角度
                        if ((pointOneAngle >= arc.StartAngle && pointOneAngle <= arc.EndAngle) && (pointTwoAngle >= arc.StartAngle && pointTwoAngle <= arc.EndAngle))
                        {
                            return Math.Abs(point.Y - maxY) > Math.Abs(point.Y - minY) ? point.Y - minY : point.Y - maxY;
                        }
                        else if ((pointOneAngle >= arc.StartAngle && pointOneAngle <= arc.EndAngle) && !(pointTwoAngle >= arc.StartAngle && pointTwoAngle <= arc.EndAngle))
                        {
                            return point.Y - maxY;
                        }
                        else if (!(pointOneAngle >= arc.StartAngle && pointOneAngle <= arc.EndAngle) && (pointTwoAngle >= arc.StartAngle && pointTwoAngle <= arc.EndAngle))
                        {
                            return point.Y - minY;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else if (arc.StartAngle >= arc.EndAngle)
                    {
                        #region 起始角度大于终止角度
                        if (((pointOneAngle >= arc.StartAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= arc.EndAngle))
                           && ((pointTwoAngle >= arc.StartAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= arc.EndAngle)))
                        {
                            return Math.Abs(point.Y - maxY) > Math.Abs(point.Y - minY) ? point.Y - minY : point.Y - maxY;
                        }
                        else if (((pointOneAngle >= arc.StartAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= arc.EndAngle))
                           && !((pointTwoAngle >= arc.StartAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= arc.EndAngle)))
                        {
                            return point.Y - maxY;
                        }
                        else if (!((pointOneAngle >= arc.StartAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= arc.EndAngle))
                           && ((pointTwoAngle >= arc.StartAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= arc.EndAngle)))
                        {
                            return point.Y - minY;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else
                    {
                        //点不在圆弧上，即没有交点
                        return -9999999;
                    }
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
        }

        /// <summary>
        /// 求解点对应x轴和y轴方向和椭圆弧的交点
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="point"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private float pointToEllipseArc(Ellipse ellipse, PointF point, int tag)
        {
            if (tag == 1)
            {
                if (point.Y >= ellipse.Bound.Y && point.Y <= ellipse.Bound.Y + ellipse.Bound.Height)
                {
                    float angle = (float)(ellipse.Angle * (Math.PI / 180));
                    float a = (float)(Math.Pow(ellipse.ShortRadius * Math.Cos(angle), 2) + Math.Pow(ellipse.LongRadius * Math.Sin(angle), 2));
                    float b = (float)(2 * (point.Y - ellipse.CenterPoint.Y) * Math.Cos(angle) * Math.Sin(angle) * (Math.Pow(ellipse.ShortRadius, 2) - Math.Pow(ellipse.LongRadius, 2)));
                    float c = (float)(Math.Pow((point.Y - ellipse.CenterPoint.Y) * ellipse.ShortRadius * Math.Sin(angle), 2) + Math.Pow((point.Y - ellipse.CenterPoint.Y) * ellipse.LongRadius * Math.Cos(angle), 2) - Math.Pow(ellipse.LongRadius * ellipse.ShortRadius, 2));
                    float d = (float)(Math.Pow(b, 2) - 4 * a * c);
                    float maxX = (float)((-b + Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.X);
                    float minX = (float)((-b - Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.X);

                    float pointOneAngle = (float)((180 / Math.PI) * Math.Atan2(point.Y - ellipse.CenterPoint.Y, maxX - ellipse.CenterPoint.X));
                    pointOneAngle = pointOneAngle < 0 ? 360 + pointOneAngle : pointOneAngle;
                    float pointTwoAngle = (float)((180 / Math.PI) * Math.Atan2(point.Y - ellipse.CenterPoint.Y, minX - ellipse.CenterPoint.X));
                    pointTwoAngle = pointTwoAngle < 0 ? 360 + pointTwoAngle : pointTwoAngle;

                    //当前椭圆起点，相对于椭圆圆心的角度
                    float startAngle = ellipse.StartAngle + ellipse.Angle > 360 ? ellipse.StartAngle + ellipse.Angle - 360 : ellipse.StartAngle + ellipse.Angle;
                    //当前椭圆终点，相对于椭圆圆心的角度
                    float endAngle = ellipse.EndAngle + ellipse.Angle > 360 ? ellipse.EndAngle + ellipse.Angle - 360 : ellipse.EndAngle + ellipse.Angle;

                    if (startAngle < endAngle)
                    {
                        #region 起始角度小于终止角度
                        if ((pointOneAngle >= startAngle && pointOneAngle <= endAngle) && (pointTwoAngle >= startAngle && pointTwoAngle <= endAngle))
                        {
                            return Math.Abs(point.X - maxX) > Math.Abs(point.X - minX) ? point.X - minX : point.X - maxX;
                        }
                        else if ((pointOneAngle >= startAngle && pointOneAngle <= endAngle) && !(pointTwoAngle >= startAngle && pointTwoAngle <= endAngle))
                        {
                            return point.X - maxX;
                        }
                        else if (!(pointOneAngle >= startAngle && pointOneAngle <= endAngle) && (pointTwoAngle >= startAngle && pointTwoAngle <= endAngle))
                        {
                            return point.X - minX;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else if (startAngle >= endAngle)
                    {
                        #region 起始角度大于终止角度
                        if (((pointOneAngle >= startAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= endAngle))
                           && ((pointTwoAngle >= startAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= endAngle)))
                        {
                            return Math.Abs(point.X - maxX) > Math.Abs(point.X - minX) ? point.X - minX : point.X - maxX;
                        }
                        else if (((pointOneAngle >= startAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= endAngle))
                           && !((pointTwoAngle >= startAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= endAngle)))
                        {
                            return point.X - maxX;
                        }
                        else if (!((pointOneAngle >= startAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= endAngle))
                           && ((pointTwoAngle >= startAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= endAngle)))
                        {
                            return point.X - minX;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else
                    {
                        //不存在交点
                        return -9999999;
                    }
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
            else
            {
                if (point.X <= ellipse.Bound.X + ellipse.Bound.Width && point.Y >= ellipse.Bound.X)
                {
                    float angle = (float)(ellipse.Angle * (Math.PI / 180));
                    float a = (float)(Math.Pow(ellipse.LongRadius * Math.Cos(angle), 2) + Math.Pow(ellipse.ShortRadius * Math.Sin(angle), 2));
                    float b = (float)(2 * (point.X - ellipse.CenterPoint.X) * Math.Cos(angle) * Math.Sin(angle) * (Math.Pow(ellipse.ShortRadius, 2) - Math.Pow(ellipse.LongRadius, 2)));
                    float c = (float)(Math.Pow((point.X - ellipse.CenterPoint.X) * ellipse.LongRadius * Math.Sin(angle), 2) + Math.Pow((point.X - ellipse.CenterPoint.X) * ellipse.ShortRadius * Math.Cos(angle), 2) - Math.Pow(ellipse.LongRadius * ellipse.ShortRadius, 2));
                    float d = (float)(Math.Pow(b, 2) - 4 * a * c);
                    float maxY = (float)((-b + Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.Y);
                    float minY = (float)((-b - Math.Sqrt(d)) / (2 * a) + ellipse.CenterPoint.Y);


                    float pointOneAngle = (float)((180 / Math.PI) * Math.Atan2(maxY - ellipse.CenterPoint.Y, point.X - ellipse.CenterPoint.X));
                    pointOneAngle = pointOneAngle < 0 ? 360 + pointOneAngle : pointOneAngle;
                    float pointTwoAngle = (float)((180 / Math.PI) * Math.Atan2(minY - ellipse.CenterPoint.Y, point.X - ellipse.CenterPoint.X));
                    pointTwoAngle = pointTwoAngle < 0 ? 360 + pointTwoAngle : pointTwoAngle;

                    //当前椭圆起点，相对于椭圆圆心的角度
                    float startAngle = ellipse.StartAngle + ellipse.Angle > 360 ? ellipse.StartAngle + ellipse.Angle - 360 : ellipse.StartAngle + ellipse.Angle;
                    //当前椭圆终点，相对于椭圆圆心的角度
                    float endAngle = ellipse.EndAngle + ellipse.Angle > 360 ? ellipse.EndAngle + ellipse.Angle - 360 : ellipse.EndAngle + ellipse.Angle;

                    if (startAngle < endAngle)
                    {
                        #region 起始角度小于终止角度
                        if ((pointOneAngle >= startAngle && pointOneAngle <= endAngle) && (pointTwoAngle >= startAngle && pointTwoAngle <= endAngle))
                        {
                            return Math.Abs(point.Y - maxY) > Math.Abs(point.Y - minY) ? point.Y - minY : point.Y - maxY;
                        }
                        else if ((pointOneAngle >= startAngle && pointOneAngle <= endAngle) && !(pointTwoAngle >= startAngle && pointTwoAngle <= endAngle))
                        {
                            return point.Y - maxY;
                        }
                        else if (!(pointOneAngle >= startAngle && pointOneAngle <= endAngle) && (pointTwoAngle >= startAngle && pointTwoAngle <= endAngle))
                        {
                            return point.Y - minY;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else if (startAngle >= endAngle)
                    {
                        #region 起始角度大于终止角度
                        if (((pointOneAngle >= startAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= endAngle))
                           && ((pointTwoAngle >= startAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= endAngle)))
                        {
                            return Math.Abs(point.Y - maxY) > Math.Abs(point.Y - minY) ? point.Y - minY : point.Y - maxY;
                        }
                        else if (((pointOneAngle >= startAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= endAngle))
                           && !((pointTwoAngle >= startAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= endAngle)))
                        {
                            return point.Y - maxY;
                        }
                        else if (!((pointOneAngle >= startAngle && pointOneAngle <= 360) || (pointOneAngle >= 0 && pointOneAngle <= endAngle))
                           && ((pointTwoAngle >= startAngle && pointTwoAngle <= 360) || (pointTwoAngle >= 0 && pointTwoAngle <= endAngle)))
                        {
                            return point.Y - minY;
                        }
                        else
                        {
                            //不存在交点
                            return -9999999;
                        }
                        #endregion
                    }
                    else
                    {
                        //点不在圆弧上，即没有交点
                        return -9999999;
                    }
                }
                else
                {
                    //没有交点
                    return -9999999;
                }
            }
        }
    }
}
