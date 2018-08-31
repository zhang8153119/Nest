using myCad.Model;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseIntersect
    {
        private List<PointF> listPoint = new List<PointF>();

        /// <summary>
        /// 判断顺序，Line，Circle，Arc ，Ellipse,返回交点，不存在则交点为0，不存在判断则返回null
        /// </summary>
        /// <param name="bsOne"></param>
        /// <param name="bsTwo"></param>
        /// <returns></returns>
        protected List<PointF> intersect(BaseShape bsOne, BaseShape bsTwo)
        {
            listPoint.Clear();
            #region Line和其他线的交点，Line,Circle，Arc，Ellipse
            if ("Line".Equals(bsOne.ShapeClass) && "Line".Equals(bsTwo.ShapeClass))
            {
                lineWithLine((Line)bsOne, (Line)bsTwo);
                return listPoint;
            }
            else if ("Line".Equals(bsOne.ShapeClass) && "Circle".Equals(bsTwo.ShapeClass))
            {
                lineWithCircle((Line)bsOne, (Circle)bsTwo);
                return listPoint;
            }
            else if ("Line".Equals(bsOne.ShapeClass) && "Arc".Equals(bsTwo.ShapeClass))
            {
                lineWithArc((Line)bsOne, (Arc)bsTwo);
                return listPoint;
            }
            else if ("Line".Equals(bsOne.ShapeClass) && "Ellipse".Equals(bsTwo.ShapeClass) && ((Ellipse)bsTwo).Complete)
            {
                lineWithEllipse((Line)bsOne, (Ellipse)bsTwo);
                return listPoint;
            }
            else if ("Line".Equals(bsOne.ShapeClass) && "Ellipse".Equals(bsTwo.ShapeClass) && !((Ellipse)bsTwo).Complete)
            {
                lineWithEllipseArc((Line)bsOne, (Ellipse)bsTwo);
                return listPoint;
            }
            #endregion
            #region Circle 和其他线的交点 ,Circle,Arc,Ellipse
            else if ("Circle".Equals(bsOne.ShapeClass) && "Circle".Equals(bsTwo.ShapeClass))
            {
                circleWithCircle((Circle)bsOne, (Circle)bsTwo);
                return listPoint;
            }
            else if ("Circle".Equals(bsOne.ShapeClass) && "Arc".Equals(bsTwo.ShapeClass))
            {
                circleWithArc((Circle)bsOne, (Arc)bsTwo);
                return listPoint;
            }
            else if ("Circle".Equals(bsOne.ShapeClass) && "Ellipse".Equals(bsTwo.ShapeClass) && ((Ellipse)bsTwo).Complete)
            {
                circleWithEllipse((Circle)bsOne, (Ellipse)bsTwo);
                return listPoint;
            }
            else if ("Circle".Equals(bsOne.ShapeClass) && "Ellipse".Equals(bsTwo.ShapeClass) && !((Ellipse)bsTwo).Complete)
            {
                return circleWithEllipseArc((Circle)bsOne, (Ellipse)bsTwo);
            }
            #endregion
            #region Arc 和其他线的交点 ,Arc,Ellipse
            else if ("Arc".Equals(bsOne.ShapeClass) && "Arc".Equals(bsTwo.ShapeClass))
            {
                arcWithArc((Arc)bsOne, (Arc)bsTwo);
                return listPoint;
            }
            else if ("Arc".Equals(bsOne.ShapeClass) && "Ellipse".Equals(bsTwo.ShapeClass) && ((Ellipse)bsTwo).Complete)
            {
                return arcWithEllipse((Arc)bsOne, (Ellipse)bsTwo);
            }
            else if ("Arc".Equals(bsOne.ShapeClass) && "Ellipse".Equals(bsTwo.ShapeClass) && !((Ellipse)bsTwo).Complete)
            {
                return arcWithEllipseArc((Arc)bsOne, (Ellipse)bsTwo);
            }
            #endregion
            #region Ellipse 和其他线的交点 Ellipse
            else if ("Ellipse".Equals(bsOne.ShapeClass) && ((Ellipse)bsOne).Complete && "Ellipse".Equals(bsTwo.ShapeClass) && ((Ellipse)bsTwo).Complete)
            {
                return ellipseWithEllipse((Ellipse)bsOne, (Ellipse)bsTwo);
            }
            else if ("Ellipse".Equals(bsOne.ShapeClass) && ((Ellipse)bsOne).Complete && "Ellipse".Equals(bsTwo.ShapeClass) && !((Ellipse)bsTwo).Complete)
            {
                return ellipseWithEllipseArc((Ellipse)bsOne, (Ellipse)bsTwo);
            }
            #endregion
            #region Ellipse 和其他线的交点 Ellipse
            else if ("Ellipse".Equals(bsOne.ShapeClass) && !((Ellipse)bsOne).Complete && "Ellipse".Equals(bsTwo.ShapeClass) && !((Ellipse)bsTwo).Complete)
            {
                return ellipseArcWithEllipseArc((Ellipse)bsOne, (Ellipse)bsTwo);
            }
            #endregion
            else
            {
                //不属于上述类型的相交判断时返回null值
                return null;
            }
        }

        /// <summary>
        /// 判断两块板是否存在交集
        /// </summary>
        /// <param name="plateOne"></param>
        /// <param name="plateTwo"></param>
        /// <returns></returns>
        //public bool plateIntersectPlate(BasePlate plateOne, BasePlate plateTwo)
        //{
        //    //将两块件号板子面域后判断面域是否存在交集，判断面域

        //    return true;
        //}

        /// <summary>
        /// 两直线相交，延长线的交点
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="lineTwo"></param>
        /// <returns>返回交点</returns>
        public virtual void lineWithLine(Line lineOne, Line lineTwo)
        {
            /*
             * 采用两点式求解直线方程判断是否获得交点。
             *(x-x1)/(x2-x1) = (y-y1)/(y2-y1)
             * f(x,y) = (x - x1)/(x2 - x1) - (y - y1)/(y2 - y1) = 0
             * y = Kx+b;
             * **/

            if (Math.Abs(lineOne.EndPoint.X - lineOne.StartPoint.X) < 0.0001 && Math.Abs(lineTwo.EndPoint.X - lineTwo.StartPoint.X) < 0.0001)
            {
                //两线平行于y轴，没有交点
            }
            else if (Math.Abs(lineOne.EndPoint.Y - lineOne.StartPoint.Y) < 0.0001 && Math.Abs(lineTwo.EndPoint.Y - lineTwo.StartPoint.Y) < 0.0001)
            {
                //两线平行于x轴，没有交点
            }
            else if ((Math.Abs(lineOne.EndPoint.X - lineOne.StartPoint.X) < 0.0001 && Math.Abs(lineTwo.EndPoint.Y - lineTwo.StartPoint.Y) < 0.0001))
            {
                //lineOne平行于y轴，lineTwo平行于X轴
                listPoint.Add(new PointF(lineOne.StartPoint.X, lineTwo.StartPoint.Y));
            }
            else if ((Math.Abs(lineOne.EndPoint.X - lineOne.StartPoint.X) < 0.0001 && !(Math.Abs(lineTwo.EndPoint.Y - lineTwo.StartPoint.Y) < 0.0001)))
            {
                //lineOne平行于y轴，lineTwo不平行于X轴
                float lineTwoXieLv = (lineTwo.EndPoint.Y - lineTwo.StartPoint.Y) / (lineTwo.EndPoint.X - lineTwo.StartPoint.X);    //线段二的斜率
                float bTwo = lineTwo.StartPoint.Y - lineTwoXieLv * lineTwo.StartPoint.X;
                listPoint.Add(new PointF(lineOne.StartPoint.X, lineOne.StartPoint.X * lineTwoXieLv + bTwo));
            }
            else if (Math.Abs(lineOne.EndPoint.Y - lineOne.StartPoint.Y) < 0.0001 && Math.Abs(lineTwo.EndPoint.X - lineTwo.StartPoint.X) < 0.0001)
            {
                //lineOne平行于x轴，lineTwo平行于y轴
                listPoint.Add(new PointF(lineTwo.StartPoint.X, lineOne.StartPoint.Y));
            }
            else if (!(Math.Abs(lineOne.EndPoint.Y - lineOne.StartPoint.Y) < 0.0001) && Math.Abs(lineTwo.EndPoint.X - lineTwo.StartPoint.X) < 0.0001)
            {
                //lineOne不平行于轴，lineTwo平行于y轴
                float lineOneXieLv = (lineOne.EndPoint.Y - lineOne.StartPoint.Y) / (lineOne.EndPoint.X - lineOne.StartPoint.X);    //线段一的斜率
                float bOne = lineOne.StartPoint.Y - lineOneXieLv * lineOne.StartPoint.X;
                listPoint.Add(new PointF(lineTwo.StartPoint.X, lineTwo.StartPoint.X * lineOneXieLv + bOne));
            }
            else
            {
                #region 两线都不平行于x轴或y轴，求解斜率，看是否平行
                float lineOneXieLv = (lineOne.EndPoint.Y - lineOne.StartPoint.Y) / (lineOne.EndPoint.X - lineOne.StartPoint.X);    //线段一的斜率
                float lineTwoXieLv = (lineTwo.EndPoint.Y - lineTwo.StartPoint.Y) / (lineTwo.EndPoint.X - lineTwo.StartPoint.X);    //线段二的斜率
                if (Math.Abs(lineOneXieLv - lineTwoXieLv) < 0.00001)
                {
                    //趋近于平行的线判断为没有交点
                }
                else
                {
                    float bOne = lineOne.StartPoint.Y - lineOneXieLv * lineOne.StartPoint.X;
                    float bTwo = lineTwo.StartPoint.Y - lineTwoXieLv * lineTwo.StartPoint.X;
                    //求交点
                    float jiaoX = (bTwo - bOne) / (lineOneXieLv - lineTwoXieLv);
                    float jiaoY = lineOneXieLv * jiaoX + bOne;
                    listPoint.Add(new PointF(jiaoX, jiaoY));
                }
                #endregion
            }
        }

        /// <summary>
        /// 直线和圆相交，延长线的交点
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="circleTwo"></param>
        public virtual void lineWithCircle(Line lineOne, Circle circleTwo)
        {
            /*
             * 利用直线和半径的交集求解直线与圆的关系
             * 利用斜率和圆心点求解过圆心的直线的方程
             * (x-x0)²+（y - y0）² = r²
             * **/
            if (Math.Abs(lineOne.StartPoint.X - lineOne.EndPoint.X) < 0.0001)
            {
                #region 直线平行于y轴
                //直线平行于y轴
                if (lineOne.StartPoint.X >= circleTwo.CenterPoint.X - circleTwo.Radius && lineOne.StartPoint.X <= circleTwo.CenterPoint.X - circleTwo.Radius)
                {
                    float a = (float)(Math.Sqrt(Math.Pow(circleTwo.Radius, 2) - Math.Pow(lineOne.StartPoint.X - circleTwo.CenterPoint.X, 2)));
                    if (a < 0.0001)
                    {
                        listPoint.Add(new PointF(lineOne.StartPoint.X, circleTwo.CenterPoint.Y));
                    }
                    else
                    {
                        float y1 = -a + circleTwo.CenterPoint.Y;
                        float y2 = a + circleTwo.CenterPoint.Y;
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y1));
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y2));
                    }
                }
                #endregion
            }
            else if (Math.Abs(lineOne.StartPoint.Y - lineOne.EndPoint.Y) < 0.0001)
            {
                #region 直线平行于x轴
                //直线平行于x轴
                float a = (float)(Math.Sqrt(Math.Pow(circleTwo.Radius, 2) - Math.Pow(lineOne.StartPoint.Y - circleTwo.CenterPoint.Y, 2)));
                if (a < 0.0001)
                {
                    listPoint.Add(new PointF(circleTwo.CenterPoint.X, lineOne.StartPoint.Y));
                }
                else
                {
                    float x1 = -a + circleTwo.CenterPoint.X;
                    float x2 = a + circleTwo.CenterPoint.X;
                    listPoint.Add(new PointF(x1, lineOne.StartPoint.Y));
                    listPoint.Add(new PointF(x2, lineOne.StartPoint.Y));
                }
                #endregion
            }
            else
            {
                #region 直线不平行于x轴，也不平行于y轴
                //直线不平行于x轴，也不平行于y轴
                //y = kx + b
                //(x-x0)²+（y - y0）² = r²
                //x²(k²+1)+x(2k(b-y0)-2x0)+(x0²+(b-y0)²-r²)=0
                //a' = (k²+1)
                //b' = (2k(b-y0)-2x0) =2(k(b-y0)-x0)
                //c' = (x0²+(b-y0)²-r²)
                //∆ = b'² - 4a'c'
                //x =( -b'±√∆ )/2a'
                float lineXieLv = (lineOne.EndPoint.Y - lineOne.StartPoint.Y) / (lineOne.EndPoint.X - lineOne.StartPoint.X);
                float lineB = lineOne.StartPoint.Y - lineOne.StartPoint.X * lineXieLv;
                float a = lineXieLv * lineXieLv + 1;
                float b = 2 * (lineXieLv * (lineB - circleTwo.CenterPoint.Y) - circleTwo.CenterPoint.X);
                float c = (float)(Math.Pow(circleTwo.CenterPoint.X, 2) + Math.Pow(lineB - circleTwo.CenterPoint.Y, 2) - Math.Pow(circleTwo.Radius, 2));
                float judge = b * b - (4 * a * c);
                if (judge >= 0)
                {
                    if (judge < 0.0001)
                    {
                        float x = (float)((-b) / (2 * a));
                        listPoint.Add(new PointF(x, x * lineXieLv + lineB));
                    }
                    else
                    {
                        float x1 = (float)((-b - Math.Sqrt(judge)) / (2 * a));
                        float x2 = (float)((-b + Math.Sqrt(judge)) / (2 * a));
                        listPoint.Add(new PointF(x1, x1 * lineXieLv + lineB));
                        listPoint.Add(new PointF(x2, x2 * lineXieLv + lineB));
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 直线和圆弧相交，延长线的交点
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="arcTwo"></param>
        public virtual void lineWithArc(Line lineOne, Arc arcTwo)
        {
            /*
              * 利用直线和半径的交集求解直线与圆的关系
              * 利用斜率和圆心点求解过圆心的直线的方程
              * (x-x0)²+（y - y0）² = r²
              * **/
            if (Math.Abs(lineOne.StartPoint.X - lineOne.EndPoint.X) < 0.0001)
            {
                #region 直线平行于y轴
                //直线平行于y轴
                if (lineOne.StartPoint.X >= arcTwo.CenterPoint.X - arcTwo.Radius && lineOne.StartPoint.X <= arcTwo.CenterPoint.X - arcTwo.Radius)
                {
                    float a = (float)(Math.Sqrt(Math.Pow(arcTwo.Radius, 2) - Math.Pow(lineOne.StartPoint.X - arcTwo.CenterPoint.X, 2)));
                    if (a < 0.0001)
                    {
                        listPoint.Add(new PointF(lineOne.StartPoint.X, arcTwo.CenterPoint.Y));
                    }
                    else
                    {
                        float y1 = -a + arcTwo.CenterPoint.Y;
                        float y2 = a + arcTwo.CenterPoint.Y;
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y1));
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y2));
                    }
                }
                #endregion
            }
            else if (Math.Abs(lineOne.StartPoint.Y - lineOne.EndPoint.Y) < 0.0001)
            {
                #region 直线平行于x轴
                //直线平行于x轴
                float a = (float)(Math.Sqrt(Math.Pow(arcTwo.Radius, 2) - Math.Pow(lineOne.StartPoint.Y - arcTwo.CenterPoint.Y, 2)));
                if (a < 0.0001)
                {
                    listPoint.Add(new PointF(arcTwo.CenterPoint.X, lineOne.StartPoint.Y));
                }
                else
                {
                    float x1 = -a + arcTwo.CenterPoint.X;
                    float x2 = a + arcTwo.CenterPoint.X;
                    listPoint.Add(new PointF(x1, lineOne.StartPoint.Y));
                    listPoint.Add(new PointF(x2, lineOne.StartPoint.Y));
                }
                #endregion
            }
            else
            {
                #region 直线不平行于x轴，也不平行于y轴
                //直线不平行于x轴，也不平行于y轴
                //y = kx + b
                //(x-x0)²+（y - y0）² = r²
                //x²(k²+1)+x(2k(b-y0)-2x0)+(x0²+(b-y0)²-r²)=0
                //a' = (k²+1)
                //b' = (2k(b-y0)-2x0) =2(k(b-y0)-x0)
                //c' = (x0²+(b-y0)²-r²)
                //∆ = b'² - 4a'c'
                //x =( -b'±√∆ )/2a'
                float lineXieLv = (lineOne.EndPoint.Y - lineOne.StartPoint.Y) / (lineOne.EndPoint.X - lineOne.StartPoint.X);
                float lineB = lineOne.StartPoint.Y - lineOne.StartPoint.X * lineXieLv;
                float a = lineXieLv * lineXieLv + 1;
                float b = 2 * (lineXieLv * (lineB - arcTwo.CenterPoint.Y) - arcTwo.CenterPoint.X);
                float c = (float)(Math.Pow(arcTwo.CenterPoint.X, 2) + Math.Pow(lineB - arcTwo.CenterPoint.Y, 2) - Math.Pow(arcTwo.Radius, 2));
                float judge = b * b - (4 * a * c);
                if (judge >= 0)
                {
                    if (judge < 0.001)
                    {
                        float x = (float)((-b) / (2 * a));
                        listPoint.Add(new PointF(x, x * lineXieLv + lineB));
                    }
                    else
                    {
                        float x1 = (float)((-b - Math.Sqrt(judge)) / (2 * a));
                        float x2 = (float)((-b + Math.Sqrt(judge)) / (2 * a));
                        listPoint.Add(new PointF(x1, x1 * lineXieLv + lineB));
                        listPoint.Add(new PointF(x2, x2 * lineXieLv + lineB));
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 直线和椭圆相交，延长线的交点
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="ellipseTwo"></param>
        public virtual void lineWithEllipse(Line lineOne, Ellipse ellipseTwo)
        {
            //利用直线和椭圆方程联立求解，其中直线按照椭圆圆心相对原点的偏离，进行移动，形成新的直线方程。
            //(xCosθ + ySinθ)²/a² + (yCosθ - xSinθ)²/b² = 1
            //y = kx + lb
            //y + y0 = k(x+x0)+lb
            //y = kx + kx0 + lb - y0
            //newB = kx0 + lb - y0
            //x²(b²(cosθ+ ksinθ)²+a²(kcosθ - sinθ)²) + 2xnewB(b²sinθ(cosθ + ksinθ) + a²cosθ(kcosθ-sinθ)) + newB²(b²sin²θ+a²cos²θ) -a²b² =0
            //a' = (b²(cosθ+ ksinθ)²+a²(kcosθ - sinθ)²)
            //b' = 2newB(b²sinθ(cosθ + ksinθ) + a²cosθ(kcosθ-sinθ))
            //c' = newB²(b²sin²θ+a²cos²θ) -a²b²
            //∆ = b'² - 4a'c'
            //x = (-b'±√∆ )/2a'
            //k = (y2 - y1)/(x2 - x1)
            //a = shortRadius²*sin²θ+longRadius²*cos²θ;
            //b = 2*x*（shortRadius²cosθsinθ-longRadius²cosθsinθ）
            //c = x²*（shortRadius²*cos²θ+longRadius²*sin²θ）-longRadius²shortRadius²
            if (Math.Abs(lineOne.EndPoint.X - lineOne.StartPoint.X) < 0.0001)
            {
                //直线平行于y轴
                float angle = (float)(ellipseTwo.Angle * (Math.PI / 180));
                float a = (float)(Math.Pow(ellipseTwo.ShortRadius * Math.Sin(angle), 2) + Math.Pow(ellipseTwo.LongRadius * Math.Cos(angle), 2));
                float b = (float)(2 * (lineOne.StartPoint.X - ellipseTwo.CenterPoint.X) * Math.Cos(angle) * Math.Sin(angle) * (Math.Pow(ellipseTwo.ShortRadius, 2) - Math.Pow(ellipseTwo.LongRadius, 2)));
                float c = (float)(Math.Pow((lineOne.StartPoint.X - ellipseTwo.CenterPoint.X) * ellipseTwo.ShortRadius * Math.Cos(angle), 2) + Math.Pow((lineOne.StartPoint.X - ellipseTwo.CenterPoint.X) * ellipseTwo.LongRadius * Math.Sin(angle), 2) - Math.Pow(ellipseTwo.LongRadius * ellipseTwo.ShortRadius, 2));
                float judge = b * b - (4 * a * c);
                if (judge >= 0)
                {
                    if (judge < 0.0001)
                    {
                        float y = (float)((-b) / (2 * a) + ellipseTwo.CenterPoint.Y);
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y));
                    }
                    else
                    {
                        float y1 = (float)((-b - Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.Y);
                        float y2 = (float)((-b + Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.Y);
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y1));
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y2));
                    }
                }
            }
            else
            {
                float lineXieLv = (lineOne.EndPoint.Y - lineOne.StartPoint.Y) / (lineOne.EndPoint.X - lineOne.StartPoint.X);
                float lineB = lineOne.StartPoint.Y - lineXieLv * lineOne.StartPoint.X;
                float newB = lineXieLv * ellipseTwo.CenterPoint.X + lineB - ellipseTwo.CenterPoint.Y;
                float angle = (float)(ellipseTwo.Angle * (Math.PI / 180));
                float a = (float)(Math.Pow(ellipseTwo.ShortRadius * (Math.Cos(angle) + lineXieLv * Math.Sin(angle)), 2) + Math.Pow(ellipseTwo.LongRadius * (lineXieLv * Math.Cos(angle) - Math.Sin(angle)), 2));
                float b = (float)(2 * newB * (Math.Pow(ellipseTwo.ShortRadius, 2) * Math.Sin(angle) * (Math.Cos(angle) + lineXieLv * Math.Sin(angle)) + Math.Pow(ellipseTwo.LongRadius, 2) * Math.Cos(angle) * (lineXieLv * Math.Cos(angle) - Math.Sin(angle))));
                float c = (float)(newB * newB * (Math.Pow(ellipseTwo.ShortRadius * Math.Sin(angle), 2) + Math.Pow(ellipseTwo.LongRadius * Math.Cos(angle), 2)) - Math.Pow(ellipseTwo.LongRadius * ellipseTwo.ShortRadius, 2));
                float judge = b * b - (4 * a * c);
                if (judge >= 0)
                {
                    if (judge < 0.0001)
                    {
                        float x = (float)((-b) / (2 * a) + ellipseTwo.CenterPoint.X);
                        listPoint.Add(new PointF(x, x * lineXieLv + lineB));
                    }
                    else
                    {
                        float x1 = (float)((-b - Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.X);
                        float x2 = (float)((-b + Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.X);
                        listPoint.Add(new PointF(x1, x1 * lineXieLv + lineB));
                        listPoint.Add(new PointF(x2, x2 * lineXieLv + lineB));
                    }
                }
            }
        }

        /// <summary>
        /// 直线和椭圆弧相交，延长线的交点
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="ellipseTwo"></param>
        public virtual void lineWithEllipseArc(Line lineOne, Ellipse ellipseTwo)
        {
            //利用直线和椭圆方程联立求解，其中直线按照椭圆圆心相对原点的偏离，进行移动，形成新的直线方程。
            //(xCosθ + ySinθ)²/a² + (yCosθ - xSinθ)²/b² = 1
            //y = kx + lb
            //y + y0 = k(x+x0)+lb
            //y = kx + kx0 + lb - y0
            //newB = kx0 + lb - y0
            //x²(b²(cosθ+ ksinθ)²+a²(kcosθ - sinθ)²) + 2xnewB(b²sinθ(cosθ + ksinθ) + a²cosθ(kcosθ-sinθ)) + newB²(b²sin²θ+a²cos²θ) -a²b² =0
            //a' = (b²(cosθ+ ksinθ)²+a²(kcosθ - sinθ)²)
            //b' = 2newB(b²sinθ(cosθ + ksinθ) + a²cosθ(kcosθ-sinθ))
            //c' = newB²(b²sin²θ+a²cos²θ) -a²b²
            //∆ = b'² - 4a'c'
            //x = (-b'±√∆ )/2a'
            //k = (y2 - y1)/(x2 - x1)
            //a = shortRadius²*sin²θ+longRadius²*cos²θ;
            //b = 2*x*（shortRadius²cosθsinθ-longRadius²cosθsinθ）
            //c = x²*（shortRadius²*cos²θ+longRadius²*sin²θ）-longRadius²shortRadius²
            if (Math.Abs(lineOne.EndPoint.X - lineOne.StartPoint.X) < 0.0001)
            {
                //直线平行于y轴
                float angle = (float)(ellipseTwo.Angle * (Math.PI / 180));
                float a = (float)(Math.Pow(ellipseTwo.ShortRadius * Math.Sin(angle), 2) + Math.Pow(ellipseTwo.LongRadius * Math.Cos(angle), 2));
                float b = (float)(2 * (lineOne.StartPoint.X - ellipseTwo.CenterPoint.X) * Math.Cos(angle) * Math.Sin(angle) * (Math.Pow(ellipseTwo.ShortRadius, 2) - Math.Pow(ellipseTwo.LongRadius, 2)));
                float c = (float)(Math.Pow((lineOne.StartPoint.X - ellipseTwo.CenterPoint.X) * ellipseTwo.ShortRadius * Math.Cos(angle), 2) + Math.Pow((lineOne.StartPoint.X - ellipseTwo.CenterPoint.X) * ellipseTwo.LongRadius * Math.Sin(angle), 2) - Math.Pow(ellipseTwo.LongRadius * ellipseTwo.ShortRadius, 2));
                float judge = b * b - (4 * a * c);
                if (judge >= 0)
                {
                    if (judge < 0.0001)
                    {
                        float y = (float)((-b) / (2 * a) + ellipseTwo.CenterPoint.Y);
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y));
                    }
                    else
                    {
                        float y1 = (float)((-b - Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.Y);
                        float y2 = (float)((-b + Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.Y);
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y1));
                        listPoint.Add(new PointF(lineOne.StartPoint.X, y2));
                    }
                }
            }
            else
            {
                float lineXieLv = (lineOne.EndPoint.Y - lineOne.StartPoint.Y) / (lineOne.EndPoint.X - lineOne.StartPoint.X);
                float lineB = lineOne.StartPoint.Y - lineXieLv * lineOne.StartPoint.X;
                float newB = lineXieLv * ellipseTwo.CenterPoint.X + lineB - ellipseTwo.CenterPoint.Y;
                float angle = (float)(ellipseTwo.Angle * (Math.PI / 180));
                float a = (float)(Math.Pow(ellipseTwo.ShortRadius * (Math.Cos(angle) + lineXieLv * Math.Sin(angle)), 2) + Math.Pow(ellipseTwo.LongRadius * (lineXieLv * Math.Cos(angle) - Math.Sin(angle)), 2));
                float b = (float)(2 * newB * (Math.Pow(ellipseTwo.ShortRadius, 2) * Math.Sin(angle) * (Math.Cos(angle) + lineXieLv * Math.Sin(angle)) + Math.Pow(ellipseTwo.LongRadius, 2) * Math.Cos(angle) * (lineXieLv * Math.Cos(angle) - Math.Sin(angle))));
                float c = (float)(newB * newB * (Math.Pow(ellipseTwo.ShortRadius * Math.Sin(angle), 2) + Math.Pow(ellipseTwo.LongRadius * Math.Cos(angle), 2)) - Math.Pow(ellipseTwo.LongRadius * ellipseTwo.ShortRadius, 2));
                float judge = b * b - (4 * a * c);
                if (judge >= 0)
                {
                    if (judge < 0.0001)
                    {
                        float x = (float)((-b) / (2 * a) + ellipseTwo.CenterPoint.X);
                        listPoint.Add(new PointF(x, x * lineXieLv + lineB));
                    }
                    else
                    {
                        float x1 = (float)((-b - Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.X);
                        float x2 = (float)((-b + Math.Sqrt(judge)) / (2 * a) + ellipseTwo.CenterPoint.X);
                        listPoint.Add(new PointF(x1, x1 * lineXieLv + lineB));
                        listPoint.Add(new PointF(x2, x2 * lineXieLv + lineB));
                    }
                }
            }
        }

        /// <summary>
        /// 圆和圆相交，延长线的交点
        /// </summary>
        /// <param name="circleOne"></param>
        /// <param name="circleTwo"></param>
        public virtual void circleWithCircle(Circle circleOne, Circle circleTwo)
        {
            //利用圆和圆之间的位置关系进行求解
            //两圆联立方程式
            //y*2*(y2-y1)= (r1²-r2²) - (x1²-x2²) - (y1²-y2²) - x*2*(x2-x1)
            //k = - (x2 -x1)/(y2 - y1)
            //b = (r1²-r2² - x1²+ x2² - y1²+y2²)/(2*y2 - 2*y1)
            //y = kx + b
            //代入其中一个圆方程求解
            //(x-x0)²+（y - y0）² = r²
            //x²(k²+1)+x(2k(b-y0)-2x0)+(x0²+(b-y0)²-r²)=0
            //a' = (k²+1)
            //b' = (2k(b-y0)-2x0) =2(k(b-y0)-x0)
            //c' = (x0²+(b-y0)²-r²)
            //∆ = b'² - 4a'c'
            //x =( -b'±√∆ )/2a'
            if (pointToPoint(circleOne.CenterPoint,circleTwo.CenterPoint) <= (circleOne.Radius + circleTwo.Radius) )
            {
                if (Math.Abs(circleTwo.CenterPoint.Y - circleOne.CenterPoint.Y) < 0.0001)
                {
                    //x=(r1² - r2² - x1² + x2²)/(2*x2 -2*x1)
                    //y = ±√(r²-(x - x0)²) + y0;
                    float x = (float)(Math.Pow(circleOne.Radius, 2) - Math.Pow(circleTwo.Radius, 2) - Math.Pow(circleOne.CenterPoint.X, 2) + Math.Pow(circleTwo.CenterPoint.X, 2)) / (2 * circleTwo.CenterPoint.X - 2 * circleOne.CenterPoint.X);
                    float judge = (float)Math.Sqrt(Math.Pow(circleOne.Radius, 2) - Math.Pow(x - circleOne.CenterPoint.X, 2));
                    if (judge < 0.0001)
                    {
                        listPoint.Add(new PointF(x, circleOne.CenterPoint.Y));
                    }
                    else
                    {
                        listPoint.Add(new PointF(x, -judge + circleOne.CenterPoint.Y));
                        listPoint.Add(new PointF(x, judge + circleOne.CenterPoint.Y));
                    }
                }
                else
                {
                    float lineXieLv = -(circleTwo.CenterPoint.X - circleOne.CenterPoint.X) / (circleTwo.CenterPoint.Y - circleOne.CenterPoint.Y);
                    float lineB = (float)(Math.Pow(circleOne.Radius, 2) - Math.Pow(circleTwo.Radius, 2) - Math.Pow(circleOne.CenterPoint.X, 2) + Math.Pow(circleTwo.CenterPoint.X, 2) - Math.Pow(circleOne.CenterPoint.Y, 2) + Math.Pow(circleTwo.CenterPoint.Y, 2)) / (2 * circleTwo.CenterPoint.Y - 2 * circleOne.CenterPoint.Y);
                    float a = lineXieLv * lineXieLv + 1;
                    float b = 2 * (lineXieLv * (lineB - circleOne.CenterPoint.Y) - circleOne.CenterPoint.X);
                    float c = circleOne.CenterPoint.X * circleOne.CenterPoint.X + (lineB - circleOne.CenterPoint.Y) * (lineB - circleOne.CenterPoint.Y) - circleOne.Radius * circleOne.Radius;
                    float judge = b * b - 4 * a * c;
                    if (judge >= 0)
                    {
                        if (judge < 0.0001)
                        {
                            float x = (-b) / (2 * a);
                            listPoint.Add(new PointF(x, lineXieLv * x + lineB));
                        }
                        else
                        {
                            float x1 = (float)(-b - Math.Sqrt(judge)) / (2 * a);
                            listPoint.Add(new PointF(x1, lineXieLv * x1 + lineB));
                            float x2 = (float)(-b + Math.Sqrt(judge)) / (2 * a);
                            listPoint.Add(new PointF(x2, lineXieLv * x2 + lineB));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 圆和圆弧相交，延长线的交点
        /// </summary>
        /// <param name="circleOne"></param>
        /// <param name="arcTwo"></param>
        public virtual void circleWithArc(Circle circleOne, Arc arcTwo)
        {
            //利用圆和圆之间的位置关系进行求解
            //两圆联立方程式
            //y*2*(y2-y1)= (r1²-r2²) - (x1²-x2²) - (y1²-y2²) - x*2*(x2-x1)
            //k = - (x2 -x1)/(y2 - y1)
            //b = (r1²-r2² - x1²+ x2² - y1²+y2²)/(2*y2 - 2*y1)
            //y = kx + b
            //代入其中一个圆方程求解
            //(x-x0)²+（y - y0）² = r²
            //x²(k²+1)+x(2k(b-y0)-2x0)+(x0²+(b-y0)²-r²)=0
            //a' = (k²+1)
            //b' = (2k(b-y0)-2x0) =2(k(b-y0)-x0)
            //c' = (x0²+(b-y0)²-r²)
            //∆ = b'² - 4a'c'
            //x =( -b'±√∆ )/2a'
            if (pointToPoint(circleOne.CenterPoint, arcTwo.CenterPoint) <= (circleOne.Radius + arcTwo.Radius))
            {
                if (Math.Abs(arcTwo.CenterPoint.Y - circleOne.CenterPoint.Y) < 0.0001)
                {
                    //x=(r1² - r2² - x1² + x2²)/(2*x2 -2*x1)
                    //y = ±√(r²-(x - x0)²) + y0;
                    float x = (float)(Math.Pow(circleOne.Radius, 2) - Math.Pow(arcTwo.Radius, 2) - Math.Pow(circleOne.CenterPoint.X, 2) + Math.Pow(arcTwo.CenterPoint.X, 2)) / (2 * arcTwo.CenterPoint.X - 2 * circleOne.CenterPoint.X);
                    float judge = (float)Math.Sqrt(Math.Pow(circleOne.Radius, 2) - Math.Pow(x - circleOne.CenterPoint.X, 2));
                    if (judge < 0.0001)
                    {
                        listPoint.Add(new PointF(x, circleOne.CenterPoint.Y));
                    }
                    else
                    {
                        listPoint.Add(new PointF(x, -judge + circleOne.CenterPoint.Y));
                        listPoint.Add(new PointF(x, judge + circleOne.CenterPoint.Y));
                    }
                }
                else
                {
                    float lineXieLv = -(arcTwo.CenterPoint.X - circleOne.CenterPoint.X) / (arcTwo.CenterPoint.Y - circleOne.CenterPoint.Y);
                    float lineB = (float)(Math.Pow(circleOne.Radius, 2) - Math.Pow(arcTwo.Radius, 2) - Math.Pow(circleOne.CenterPoint.X, 2) + Math.Pow(arcTwo.CenterPoint.X, 2) - Math.Pow(circleOne.CenterPoint.Y, 2) + Math.Pow(arcTwo.CenterPoint.Y, 2)) / (2 * arcTwo.CenterPoint.Y - 2 * circleOne.CenterPoint.Y);
                    float a = lineXieLv * lineXieLv + 1;
                    float b = 2 * (lineXieLv * (lineB - circleOne.CenterPoint.Y) - circleOne.CenterPoint.X);
                    float c = circleOne.CenterPoint.X * circleOne.CenterPoint.X + (lineB - circleOne.CenterPoint.Y) * (lineB - circleOne.CenterPoint.Y) - circleOne.Radius * circleOne.Radius;
                    float judge = b * b - 4 * a * c;
                    if (judge >= 0)
                    {
                        if (judge < 0.0001)
                        {
                            float x = (-b) / (2 * a);
                            listPoint.Add(new PointF(x, lineXieLv * x + lineB));
                        }
                        else
                        {
                            float x1 = (float)(-b - Math.Sqrt(judge)) / (2 * a);
                            listPoint.Add(new PointF(x1, lineXieLv * x1 + lineB));
                            float x2 = (float)(-b + Math.Sqrt(judge)) / (2 * a);
                            listPoint.Add(new PointF(x2, lineXieLv * x2 + lineB));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 圆和椭圆相交，延长线的交点
        /// </summary>
        /// <param name="circleOne"></param>
        /// <param name="ellipseTwo"></param>
        public virtual void circleWithEllipse(Circle circleOne, Ellipse ellipseTwo)
        {
            //利用椭圆和圆的图形方程联立方程式求解二元二次方程

        }

        /// <summary>
        /// 圆和椭圆弧相交，延长线的交点
        /// </summary>
        /// <param name="circleOne"></param>
        /// <param name="ellipseTwo"></param>
        private List<PointF> circleWithEllipseArc(Circle circleOne, Ellipse ellipseTwo)
        {
            return new List<PointF>();
        }

        /// <summary>
        /// 圆弧和圆弧相交，延长线的交点
        /// </summary>
        /// <param name="arcOne"></param>
        /// <param name="arcTwo"></param>
        public virtual void arcWithArc(Arc arcOne, Arc arcTwo)
        {
            //利用圆和圆之间的位置关系进行求解
            //两圆联立方程式
            //y*2*(y2-y1)= (r1²-r2²) - (x1²-x2²) - (y1²-y2²) - x*2*(x2-x1)
            //k = - (x2 -x1)/(y2 - y1)
            //b = (r1²-r2² - x1²+ x2² - y1²+y2²)/(2*y2 - 2*y1)
            //y = kx + b
            //代入其中一个圆方程求解
            //(x-x0)²+（y - y0）² = r²
            //x²(k²+1)+x(2k(b-y0)-2x0)+(x0²+(b-y0)²-r²)=0
            //a' = (k²+1)
            //b' = (2k(b-y0)-2x0) =2(k(b-y0)-x0)
            //c' = (x0²+(b-y0)²-r²)
            //∆ = b'² - 4a'c'
            //x =( -b'±√∆ )/2a'
            if (pointToPoint(arcOne.CenterPoint, arcTwo.CenterPoint) <= (arcOne.Radius + arcTwo.Radius))
            {
                if (Math.Abs(arcTwo.CenterPoint.Y - arcOne.CenterPoint.Y) < 0.0001)
                {
                    //x=(r1² - r2² - x1² + x2²)/(2*x2 -2*x1)
                    //y = ±√(r²-(x - x0)²) + y0;
                    float x = (float)(Math.Pow(arcOne.Radius, 2) - Math.Pow(arcTwo.Radius, 2) - Math.Pow(arcOne.CenterPoint.X, 2) + Math.Pow(arcTwo.CenterPoint.X, 2)) / (2 * arcTwo.CenterPoint.X - 2 * arcOne.CenterPoint.X);
                    float judge = (float)Math.Sqrt(Math.Pow(arcOne.Radius, 2) - Math.Pow(x - arcOne.CenterPoint.X, 2));
                    if (judge < 0.0001)
                    {
                        listPoint.Add(new PointF(x, arcOne.CenterPoint.Y));
                    }
                    else
                    {
                        listPoint.Add(new PointF(x, -judge + arcOne.CenterPoint.Y));
                        listPoint.Add(new PointF(x, judge + arcOne.CenterPoint.Y));
                    }
                }
                else
                {
                    float lineXieLv = -(arcTwo.CenterPoint.X - arcOne.CenterPoint.X) / (arcTwo.CenterPoint.Y - arcOne.CenterPoint.Y);
                    float lineB = (float)(Math.Pow(arcOne.Radius, 2) - Math.Pow(arcTwo.Radius, 2) - Math.Pow(arcOne.CenterPoint.X, 2) + Math.Pow(arcTwo.CenterPoint.X, 2) - Math.Pow(arcOne.CenterPoint.Y, 2) + Math.Pow(arcTwo.CenterPoint.Y, 2)) / (2 * arcTwo.CenterPoint.Y - 2 * arcOne.CenterPoint.Y);
                    float a = lineXieLv * lineXieLv + 1;
                    float b = 2 * (lineXieLv * (lineB - arcOne.CenterPoint.Y) - arcOne.CenterPoint.X);
                    float c = arcOne.CenterPoint.X * arcOne.CenterPoint.X + (lineB - arcOne.CenterPoint.Y) * (lineB - arcOne.CenterPoint.Y) - arcOne.Radius * arcOne.Radius;
                    float judge = b * b - 4 * a * c;
                    if (judge >= 0)
                    {
                        if (judge < 0.0001)
                        {
                            float x = (-b) / (2 * a);
                            listPoint.Add(new PointF(x, lineXieLv * x + lineB));
                        }
                        else
                        {
                            float x1 = (float)(-b - Math.Sqrt(judge)) / (2 * a);
                            listPoint.Add(new PointF(x1, lineXieLv * x1 + lineB));
                            float x2 = (float)(-b + Math.Sqrt(judge)) / (2 * a);
                            listPoint.Add(new PointF(x2, lineXieLv * x2 + lineB));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 圆弧和椭圆，延长线的交点
        /// </summary>
        /// <param name="arcOne"></param>
        /// <param name="ellipseTwo"></param>
        /// <returns></returns>
        private List<PointF> arcWithEllipse(Arc arcOne, Ellipse ellipseTwo)
        {
            return new List<PointF>();
        }

        /// <summary>
        /// 圆弧和椭圆弧，延长线的交点
        /// </summary>
        /// <param name="arcOne"></param>
        /// <param name="ellipseTwo"></param>
        /// <returns></returns>
        private List<PointF> arcWithEllipseArc(Arc arcOne, Ellipse ellipseTwo)
        {
            return new List<PointF>();
        }

        /// <summary>
        /// 椭圆和椭圆，延长线的交点
        /// </summary>
        /// <param name="ellipseOne"></param>
        /// <param name="ellipseTwo"></param>
        /// <returns></returns>
        private List<PointF> ellipseWithEllipse(Ellipse ellipseOne, Ellipse ellipseTwo)
        {
            return new List<PointF>();
        }

        /// <summary>
        /// 椭圆和椭圆弧，延长线的交点
        /// </summary>
        /// <param name="ellipseOne"></param>
        /// <param name="ellipseTwo"></param>
        /// <returns></returns>
        private List<PointF> ellipseWithEllipseArc(Ellipse ellipseOne, Ellipse ellipseTwo)
        {
            return new List<PointF>();
        }

        /// <summary>
        /// 椭圆弧和椭圆弧，延长线的交点
        /// </summary>
        /// <param name="ellipseOne"></param>
        /// <param name="ellipseTwo"></param>
        /// <returns></returns>
        private List<PointF> ellipseArcWithEllipseArc(Ellipse ellipseOne, Ellipse ellipseTwo)
        {
            return new List<PointF>();
        }

        protected float pointToPoint(PointF pointOne, PointF pointTwo)
        {
            float distance = (float)Math.Sqrt(Math.Pow((pointOne.Y - pointTwo.Y), 2) + Math.Pow((pointOne.X - pointTwo.X), 2));
            return distance;
        }
    }
}
