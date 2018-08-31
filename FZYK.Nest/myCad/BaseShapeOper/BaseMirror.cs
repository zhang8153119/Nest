using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseMirror
    {
        /// <summary>
        /// 基础镜像
        /// </summary>
        /// <param name="baseShape"></param>
        /// <param name="mirrorSp"></param>
        /// <param name="mirrorEp"></param>
        private void mirror(BaseShape baseShape,PointF mirrorSp,PointF mirrorEp)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                mirrorLine((Line)baseShape, mirrorSp, mirrorEp);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                mirrorCircle((Circle)baseShape, mirrorSp, mirrorEp);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                mirrorArc((Arc)baseShape, mirrorSp, mirrorEp);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                mirrorEllipse((Ellipse)baseShape, mirrorSp, mirrorEp);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                mirrorEllipseArc((Ellipse)baseShape, mirrorSp, mirrorEp);
            }
            else
            {
                //不判断的线段类型
            }
        }

        /// <summary>
        /// 镜像直线
        /// </summary>
        /// <param name="line"></param>
        /// <param name="mirrorSp"></param>
        /// <param name="mirrorEp"></param>
        private void mirrorLine(Line line, PointF mirrorSp, PointF mirrorEp)
        {
            /* 计算对称后的起始点和终止点
             * **/
            if (mirrorSp.X == mirrorEp.X && mirrorSp.Y != mirrorEp.Y)
            {
                //镜像线为垂直于X轴
                line.StartPoint = new PointF(line.StartPoint.X + 2 * (mirrorSp.X - line.StartPoint.X), line.StartPoint.Y);
                line.EndPoint = new PointF(line.EndPoint.X + 2 * (mirrorEp.X - line.EndPoint.X), line.EndPoint.Y);
            }
            else if (mirrorSp.X != mirrorEp.X && mirrorSp.Y == mirrorEp.Y)
            {
                //镜像线为平行于X轴
                line.StartPoint = new PointF(line.StartPoint.X, line.StartPoint.Y + 2 * (mirrorSp.Y - line.StartPoint.Y));
                line.EndPoint = new PointF(line.EndPoint.X, line.EndPoint.Y + 2 * (mirrorEp.Y - line.EndPoint.Y));
            }
            else
            {
                //镜像线不为平行或垂直于X轴
                /* 分别过起始点和终止点做镜像线的垂直线
                 * 求取垂直线和镜像线的交点，根据起始点和终止点和交点的位置差，求取镜像后的起始点和终止点坐标
                 * 通过两点法求取镜像线的方程和斜率
                 * 通过 y = kx + b 起始点坐标 终止点坐标 求取b值得到垂直于镜像线的直线方程
                 * 求取垂直于镜像线的方程和镜像线的交点
                 * **/
                float jingXiangK = (mirrorEp.Y - mirrorSp.Y) / (mirrorEp.X - mirrorSp.X);
                float k = -1 / jingXiangK;
                float jingXiangB = mirrorSp.Y - jingXiangK * mirrorSp.X;
                float spB = line.StartPoint.Y - k * line.StartPoint.X;
                //求取和镜像线的交点
                PointF spInterMirrorLine = new PointF(
                    (spB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - spB * jingXiangK) / (k - jingXiangK)
                    );

                line.StartPoint = new PointF(line.StartPoint.X + 2 * (spInterMirrorLine.X - line.StartPoint.X), line.StartPoint.Y + 2 * (spInterMirrorLine.Y - line.StartPoint.Y));

                float epB = line.EndPoint.Y - k * line.EndPoint.X;
                PointF epInterMirrorLine = new PointF(
                    (epB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - epB * jingXiangK) / (k - jingXiangK)
                    );

                line.EndPoint = new PointF(line.EndPoint.X + 2 * (epInterMirrorLine.X - line.EndPoint.X), line.EndPoint.Y + 2 * (epInterMirrorLine.Y - line.EndPoint.Y));
            }
        }

        /// <summary>
        /// 镜像圆形
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="mirrorSp"></param>
        /// <param name="mirrorEp"></param>
        private void mirrorCircle(Circle circle, PointF mirrorSp, PointF mirrorEp)
        {
            /* 圆求取圆心的镜像位置
             * **/
            /* 计算对称后的起始点和终止点
           * **/
            if (mirrorSp.X == mirrorEp.X && mirrorSp.Y != mirrorEp.Y)
            {
                //镜像线为垂直于X轴
                circle.CenterPoint = new PointF(circle.CenterPoint.X + 2 * (mirrorSp.X - circle.CenterPoint.X), circle.CenterPoint.Y);
                for (int i = 0; i< circle.ListPoint.Count;i++ )
                {
                    circle.ListPoint[i] = new PointF(circle.ListPoint[i].X + 2 * (mirrorSp.X - circle.ListPoint[i].X), circle.ListPoint[i].Y);
                }
            }
            else if (mirrorSp.X != mirrorEp.X && mirrorSp.Y == mirrorEp.Y)
            {
                //镜像线为平行于X轴
                circle.CenterPoint = new PointF(circle.CenterPoint.X, circle.CenterPoint.Y + 2 * (mirrorSp.Y - circle.CenterPoint.Y));
                for (int i = 0; i < circle.ListPoint.Count; i++)
                {
                    circle.ListPoint[i] = new PointF(circle.ListPoint[i].X, circle.ListPoint[i].Y + 2 * (mirrorSp.Y - circle.ListPoint[i].Y));
                }
            }
            else
            {
                float jingXiangK = (mirrorEp.Y - mirrorSp.Y) / (mirrorEp.X - mirrorSp.X);
                float k = -1 / jingXiangK;
                float jingXiangB = mirrorSp.Y - jingXiangK * mirrorSp.X;
                float cpB = circle.CenterPoint.Y - k * circle.CenterPoint.X;
                //求取和镜像线的交点
                PointF cpInterMirrorLine = new PointF(
                    (cpB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - cpB * jingXiangK) / (k - jingXiangK)
                    );

                circle.CenterPoint = new PointF(circle.CenterPoint.X + 2 * (cpInterMirrorLine.X - circle.CenterPoint.X), circle.CenterPoint.Y + 2 * (cpInterMirrorLine.Y - circle.CenterPoint.Y));
            }

            circle.StartPoint = new PointF(
                circle.CenterPoint.X + circle.Radius,
                circle.CenterPoint.Y
                );
            circle.EndPoint = new PointF(
                circle.CenterPoint.X + circle.Radius,
                circle.CenterPoint.Y
                );

            //缺弧线上点的镜像点
        }
        
        /// <summary>
        /// 镜像椭圆
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="mirrorSp"></param>
        /// <param name="mirrorEp"></param>
        private void mirrorEllipse(Ellipse ellipse, PointF mirrorSp, PointF mirrorEp)
        {
            /* 完整的椭圆镜像对称
             * 先进行圆心的对称
             * 对称后求取旋转的点的偏移
             * **/
            PointF longRadiusPoint = new PointF(
               ellipse.CenterPoint.X + ellipse.LongRadiusX, ellipse.CenterPoint.Y + ellipse.LongRadiusY
               );
            float oldSpR = ellipse.StartParameter;
            float oldEpR = ellipse.EndParameter;
            ellipse.StartParameter = (float)(-oldEpR + 2 * Math.PI);
            ellipse.EndParameter = (float)(-oldSpR + 2 * Math.PI);

            if (mirrorSp.X == mirrorEp.X && mirrorSp.Y != mirrorEp.Y)
            {
                //镜像线为垂直于X轴
                ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X + 2 * (mirrorSp.X - ellipse.CenterPoint.X), ellipse.CenterPoint.Y);
                longRadiusPoint = new PointF(longRadiusPoint.X + 2 * (mirrorSp.X - longRadiusPoint.X), longRadiusPoint.Y);

                ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
                ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

                ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
                ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

                PointF newEp = new PointF(ellipse.StartPoint.X + 2 * (mirrorSp.X - ellipse.StartPoint.X), ellipse.StartPoint.Y);
                PointF newSp = new PointF(ellipse.EndPoint.X + 2 * (mirrorSp.X - ellipse.EndPoint.X), ellipse.EndPoint.Y);

                ellipse.EndPoint = newEp;
                ellipse.StartPoint = newSp;

                float newSpM = ellipse.EndParameter;
                ellipse.EndParameter = ellipse.StartParameter;
                ellipse.StartParameter = newSpM;

                for (int i = 0;i<ellipse.ListPoint.Count;i++)
                {
                    ellipse.ListPoint[i] = new PointF(ellipse.ListPoint[i].X + 2 * (mirrorSp.X - ellipse.ListPoint[i].X), ellipse.ListPoint[i].Y);
                }

            }
            else if (mirrorSp.X != mirrorEp.X && mirrorSp.Y == mirrorEp.Y)
            {
                //镜像线为平行于X轴
                ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X, ellipse.CenterPoint.Y + 2 * (mirrorSp.Y - ellipse.CenterPoint.Y));
                longRadiusPoint = new PointF(longRadiusPoint.X, longRadiusPoint.Y + 2 * (mirrorSp.Y - longRadiusPoint.Y));

                ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
                ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

                ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
                ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

                PointF newEp = new PointF(ellipse.StartPoint.X, ellipse.StartPoint.Y + 2 * (mirrorSp.Y - ellipse.StartPoint.Y));
                PointF newSp = new PointF(ellipse.EndPoint.X, ellipse.EndPoint.Y + 2 * (mirrorSp.Y - ellipse.EndPoint.Y));

                ellipse.EndPoint = newEp;
                ellipse.StartPoint = newSp;

                float newSpM = ellipse.EndParameter;
                ellipse.EndParameter = ellipse.StartParameter;
                ellipse.StartParameter = newSpM;

                for (int i = 0; i < ellipse.ListPoint.Count; i++)
                {
                    ellipse.ListPoint[i] = new PointF(ellipse.ListPoint[i].X, ellipse.ListPoint[i].Y + 2 * (mirrorSp.Y - ellipse.ListPoint[i].Y));
                }
            }
            else
            {
                float jingXiangK = (mirrorEp.Y - mirrorSp.Y) / (mirrorEp.X - mirrorSp.X);
                float k = -1 / jingXiangK;
                float jingXiangB = mirrorSp.Y - jingXiangK * mirrorSp.X;
                float cpB = ellipse.CenterPoint.Y - k * ellipse.CenterPoint.X;
                //求取和镜像线的交点
                PointF cpInterMirrorLine = new PointF(
                    (cpB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - cpB * jingXiangK) / (k - jingXiangK)
                    );

                ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X + 2 * (cpInterMirrorLine.X - ellipse.CenterPoint.X), ellipse.CenterPoint.Y + 2 * (cpInterMirrorLine.Y - ellipse.CenterPoint.Y));

                float lRpB = longRadiusPoint.Y - k * longRadiusPoint.X;
                PointF lRpInterMirrorLine = new PointF(
                    (lRpB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - lRpB * jingXiangK) / (k - jingXiangK)
                    );

                longRadiusPoint = new PointF(longRadiusPoint.X + 2 * (lRpInterMirrorLine.X - longRadiusPoint.X), longRadiusPoint.Y + 2 * (lRpInterMirrorLine.Y - longRadiusPoint.Y));

                ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
                ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

                ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
                ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

                ellipse.StartPoint = ellipse.EndPoint = longRadiusPoint;

                float newSpM = ellipse.EndParameter;
                ellipse.EndParameter = ellipse.StartParameter;
                ellipse.StartParameter = newSpM;

                //缺弧线上点的镜像点
            }
        }

        /// <summary>
        /// 镜像圆弧
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="mirrorSp"></param>
        /// <param name="mirrorEp"></param>
        private void mirrorArc(Arc arc, PointF mirrorSp, PointF mirrorEp)
        {
            /* 圆弧，求取圆心镜像的位置
             * 求取圆弧的起始点和终止点
             * 求取圆弧的起始点终止点的镜像，
             * 原起始点镜像后为终止点，终止点镜像后为起始点
             * 求取镜像后的起始角终止角
             * **/
            if (mirrorSp.X == mirrorEp.X && mirrorSp.Y != mirrorEp.Y)
            {
                //镜像线为垂直于X轴
                arc.CenterPoint = new PointF(arc.CenterPoint.X + 2 * (mirrorSp.X - arc.CenterPoint.X), arc.CenterPoint.Y);
                PointF newEp = new PointF(arc.StartPoint.X + 2 * (mirrorSp.X - arc.StartPoint.X), arc.StartPoint.Y);
                PointF newSp = new PointF(arc.EndPoint.X + 2 * (mirrorSp.X - arc.EndPoint.X), arc.EndPoint.Y);
                //角度以y轴方向镜像时，镜像后的角度为180度减去原角度
                float ySpAngle = arc.StartAngle;
                float yEpAngle = arc.EndAngle;
                arc.StartAngle = 180 - yEpAngle < 0 ? 180 - yEpAngle + 360 : 180 - yEpAngle;
                arc.EndAngle = 180 - ySpAngle < 0 ? 180 - ySpAngle +360 : 180 - ySpAngle;

                arc.EndPoint = newEp;
                arc.StartPoint = newSp;

                for (int i = 0; i < arc.ListPoint.Count; i++)
                {
                    arc.ListPoint[i] = new PointF(arc.ListPoint[i].X + 2 * (mirrorSp.X - arc.ListPoint[i].X), arc.ListPoint[i].Y);
                }
            }
            else if (mirrorSp.X != mirrorEp.X && mirrorSp.Y == mirrorEp.Y)
            {
                //镜像线为平行于X轴
                arc.CenterPoint = new PointF(arc.CenterPoint.X, arc.CenterPoint.Y + 2 * (mirrorSp.Y - arc.CenterPoint.Y));
                PointF newEp = new PointF(arc.StartPoint.X, arc.StartPoint.Y + 2 * (mirrorSp.Y - arc.StartPoint.Y));
                PointF newSp = new PointF(arc.EndPoint.X, arc.EndPoint.Y + 2 * (mirrorSp.Y - arc.EndPoint.Y));
                float ySpAngle = arc.StartAngle;
                float yEpAngle = arc.EndAngle;
                arc.StartAngle = 360 - yEpAngle;
                arc.EndAngle = 360 - ySpAngle;

                arc.EndPoint = newEp;
                arc.StartPoint = newSp;

                for (int i = 0; i < arc.ListPoint.Count; i++)
                {
                    arc.ListPoint[i] = new PointF(arc.ListPoint[i].X, arc.ListPoint[i].Y + 2 * (mirrorSp.Y - arc.ListPoint[i].Y));
                }
            }
            else
            {
                float jingXiangK = (mirrorEp.Y - mirrorSp.Y) / (mirrorEp.X - mirrorSp.X);
                float k = -1 / jingXiangK;
                float jingXiangB = mirrorSp.Y - jingXiangK * mirrorSp.X;
                float cpB = arc.CenterPoint.Y - k * arc.CenterPoint.X;
                //求取和镜像线的交点
                PointF cpInterMirrorLine = new PointF(
                    (cpB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - cpB * jingXiangK) / (k - jingXiangK)
                    );

                arc.CenterPoint = new PointF(arc.CenterPoint.X + 2 * (cpInterMirrorLine.X - arc.CenterPoint.X), arc.CenterPoint.Y + 2 * (cpInterMirrorLine.Y - arc.CenterPoint.Y));

                float spB = arc.StartPoint.Y - k * arc.StartPoint.X;
                //求取和镜像线的交点
                PointF spInterMirrorLine = new PointF(
                    (spB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - spB * jingXiangK) / (k - jingXiangK)
                    );

                PointF newEp= new PointF(arc.StartPoint.X + 2 * (spInterMirrorLine.X - arc.StartPoint.X), arc.StartPoint.Y + 2 * (spInterMirrorLine.Y - arc.StartPoint.Y));
                arc.EndAngle = (float)(Math.Atan2(newEp.Y - arc.CenterPoint.Y, newEp.X - arc.CenterPoint.X) * (180 / Math.PI));
                
                float epB = arc.EndPoint.Y - k * arc.EndPoint.X;
                PointF epInterMirrorLine = new PointF(
                    (epB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - epB * jingXiangK) / (k - jingXiangK)
                    );

                PointF newSp = new PointF(arc.EndPoint.X + 2 * (epInterMirrorLine.X - arc.EndPoint.X), arc.EndPoint.Y + 2 * (epInterMirrorLine.Y - arc.EndPoint.Y));
                arc.StartAngle = (float)(Math.Atan2(newSp.Y - arc.CenterPoint.Y, newSp.X - arc.CenterPoint.X) * (180 / Math.PI));

                arc.EndPoint = newEp;
                arc.StartPoint = newSp;

                //缺弧线上点的镜像点
            }
        }

        /// <summary>
        /// 镜像椭圆弧
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="mirrorSp"></param>
        /// <param name="mirrorEp"></param>
        private void mirrorEllipseArc(Ellipse ellipse, PointF mirrorSp, PointF mirrorEp)
        {
            /* 不完整的椭圆镜像对称
             * 先进行圆心的对称
             * **/
            PointF longRadiusPoint = new PointF(
               ellipse.CenterPoint.X + ellipse.LongRadiusX, ellipse.CenterPoint.Y + ellipse.LongRadiusY
               );
            float oldSpR = ellipse.StartParameter;
            float oldEpR = ellipse.EndParameter;
            ellipse.StartParameter = (float)(-oldEpR + 2 * Math.PI);
            ellipse.EndParameter = (float)(-oldSpR + 2 * Math.PI);

            ellipse.StartAngle = (float)(Math.Atan2(ellipse.ShortRadius * Math.Sin(ellipse.StartParameter), ellipse.LongRadius * Math.Cos(ellipse.StartParameter)) * (180 / Math.PI));
            ellipse.StartAngle = ellipse.StartAngle < 0 ? ellipse.StartAngle + 360 : ellipse.StartAngle;
            ellipse.EndAngle = (float)(Math.Atan2(ellipse.ShortRadius * Math.Sin(ellipse.EndParameter), ellipse.LongRadius * Math.Cos(ellipse.EndParameter)) * (180 / Math.PI));
            ellipse.EndAngle = ellipse.EndAngle < 0 ? ellipse.EndAngle + 360 : ellipse.EndAngle;

            if (mirrorSp.X == mirrorEp.X && mirrorSp.Y != mirrorEp.Y)
            {
                //镜像线为垂直于X轴
                ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X + 2 * (mirrorSp.X - ellipse.CenterPoint.X), ellipse.CenterPoint.Y);
                longRadiusPoint = new PointF(longRadiusPoint.X + 2 * (mirrorSp.X - longRadiusPoint.X), longRadiusPoint.Y);

                ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
                ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

                ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
                ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

                float ySpAngle = ellipse.StartAngle;
                float yEpAngle = ellipse.EndAngle;
                ellipse.StartAngle = 360 - yEpAngle;
                ellipse.EndAngle = 360 - ySpAngle;


                PointF newEp = new PointF(ellipse.StartPoint.X + 2 * (mirrorSp.X - ellipse.StartPoint.X), ellipse.StartPoint.Y);
                PointF newSp = new PointF(ellipse.EndPoint.X + 2 * (mirrorSp.X - ellipse.EndPoint.X), ellipse.EndPoint.Y);

                ellipse.EndPoint = newEp;
                ellipse.StartPoint = newSp;

                float ySpM = ellipse.EndParameter;
                float yEpM = ellipse.StartParameter;

                ellipse.StartParameter = (float)(2 * Math.PI - ySpM);
                ellipse.EndParameter = (float)(2*Math.PI - yEpM);

                for (int i = 0; i < ellipse.ListPoint.Count; i++)
                {
                    ellipse.ListPoint[i] = new PointF(ellipse.ListPoint[i].X + 2 * (mirrorSp.X - ellipse.ListPoint[i].X), ellipse.ListPoint[i].Y);
                }

            }
            else if (mirrorSp.X != mirrorEp.X && mirrorSp.Y == mirrorEp.Y)
            {
                //镜像线为平行于X轴
                ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X, ellipse.CenterPoint.Y + 2 * (mirrorSp.Y - ellipse.CenterPoint.Y));
                longRadiusPoint = new PointF(longRadiusPoint.X, longRadiusPoint.Y + 2 * (mirrorSp.Y - longRadiusPoint.Y));

                ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
                ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

                ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
                ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

                float ySpAngle = ellipse.StartAngle;
                float yEpAngle = ellipse.EndAngle;
                ellipse.StartAngle = 360 - yEpAngle;
                ellipse.EndAngle = 360 - ySpAngle;

                PointF newEp = new PointF(ellipse.StartPoint.X, ellipse.StartPoint.Y + 2 * (mirrorSp.Y - ellipse.StartPoint.Y));
                PointF newSp = new PointF(ellipse.EndPoint.X, ellipse.EndPoint.Y + 2 * (mirrorSp.Y - ellipse.EndPoint.Y));

                ellipse.EndPoint = newEp;
                ellipse.StartPoint = newSp;

                float ySpM = ellipse.EndParameter;
                float yEpM = ellipse.StartParameter;

                ellipse.StartParameter = (float)(2 * Math.PI - ySpM);
                ellipse.EndParameter = (float)(2 * Math.PI - yEpM);

                for (int i = 0; i < ellipse.ListPoint.Count; i++)
                {
                    ellipse.ListPoint[i] = new PointF(ellipse.ListPoint[i].X, ellipse.ListPoint[i].Y + 2 * (mirrorSp.Y - ellipse.ListPoint[i].Y));
                }
            }
            else
            {
                float jingXiangK = (mirrorEp.Y - mirrorSp.Y) / (mirrorEp.X - mirrorSp.X);
                float k = -1 / jingXiangK;
                float jingXiangB = mirrorSp.Y - jingXiangK * mirrorSp.X;
                float cpB = ellipse.CenterPoint.Y - k * ellipse.CenterPoint.X;
                //求取和镜像线的交点
                PointF cpInterMirrorLine = new PointF(
                    (cpB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - cpB * jingXiangK) / (k - jingXiangK)
                    );

                ellipse.CenterPoint = new PointF(ellipse.CenterPoint.X + 2 * (cpInterMirrorLine.X - ellipse.CenterPoint.X), ellipse.CenterPoint.Y + 2 * (cpInterMirrorLine.Y - ellipse.CenterPoint.Y));

                float lRpB = longRadiusPoint.Y - k * longRadiusPoint.X;
                PointF lRpInterMirrorLine = new PointF(
                    (lRpB - jingXiangB) / (jingXiangK - k),
                    (jingXiangB * k - lRpB * jingXiangK) / (k - jingXiangK)
                    );

                longRadiusPoint = new PointF(longRadiusPoint.X + 2 * (lRpInterMirrorLine.X - longRadiusPoint.X), longRadiusPoint.Y + 2 * (lRpInterMirrorLine.Y - longRadiusPoint.Y));

                ellipse.LongRadiusX = longRadiusPoint.X - ellipse.CenterPoint.X;
                ellipse.LongRadiusY = longRadiusPoint.Y - ellipse.CenterPoint.Y;

                ellipse.Angle = (float)(Math.Atan2(ellipse.LongRadiusY, ellipse.LongRadiusX) * (180 / Math.PI));
                ellipse.Angle = ellipse.Angle < 0 ? ellipse.Angle + 360 : ellipse.Angle;

                float ySpAngle = ellipse.StartAngle;
                float yEpAngle = ellipse.EndAngle;
                ellipse.StartAngle = 360 - yEpAngle;
                ellipse.EndAngle = 360 - ySpAngle;

                float ySpM = ellipse.EndParameter;
                float yEpM = ellipse.StartParameter;

                ellipse.StartParameter = (float)(2 * Math.PI - ySpM);
                ellipse.EndParameter = (float)(2 * Math.PI - yEpM);

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

                //缺弧线上点的镜像点

            }
        }

    }
}
