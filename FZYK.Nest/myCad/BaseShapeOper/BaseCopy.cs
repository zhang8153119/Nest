using myCad.CADInterfaceCtrl;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.BaseShapeOper
{
    class BaseCopy
    {
        /// <summary>
        /// 基本复制
        /// </summary>
        /// <param name="baseShape"></param>
        protected BaseShape copy(BaseShape baseShape)
        {
            if ("Line".Equals(baseShape.ShapeClass))
            {
                return copyLine((Line)baseShape);
            }
            else if ("Circle".Equals(baseShape.ShapeClass))
            {
                return copyCircle((Circle)baseShape);
            }
            else if ("Arc".Equals(baseShape.ShapeClass))
            {
                return copyArc((Arc)baseShape);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && ((Ellipse)baseShape).Complete)
            {
                //完整的椭圆
                return copyEllipse((Ellipse)baseShape);
            }
            else if ("Ellipse".Equals(baseShape.ShapeClass) && !((Ellipse)baseShape).Complete)
            {
                //不完整的椭圆
                return copyEllipseArc((Ellipse)baseShape);
            }
            else
            {
                //不判断的线段类型
                return null;
            }
        }

        /// <summary>
        /// 复制线段
        /// </summary>
        /// <param name="line"></param>
        private BaseShape copyLine(Line line)
        {
            Line nLine = new Line(line.StartPoint, line.EndPoint);
            nLine.PenColor = line.PenColor;
            nLine.ShapeID = CADInterface.globalID;
            CADInterface.globalID = CADInterface.globalID + 1;
            //CADInterface.currentShapes.Add(nLine);
            return nLine;
        }

        /// <summary>
        /// 复制圆
        /// </summary>
        /// <param name="circle"></param>
        private BaseShape copyCircle(Circle circle)
        {
            Circle nCircle = new Circle(circle.CenterPoint, circle.Radius,circle.StartPoint,circle.EndPoint);
            nCircle.PenColor = circle.PenColor;
            nCircle.ShapeID = CADInterface.globalID;
            nCircle.ListPoint.AddRange(circle.ListPoint);

            CADInterface.globalID = CADInterface.globalID + 1;
            //CADInterface.currentShapes.Add(nCircle);
            return nCircle;
        }

        /// <summary>
        /// 复制椭圆
        /// </summary>
        /// <param name="ellipse"></param>
        private BaseShape copyEllipse(Ellipse ellipse)
        {
            Ellipse nEllipse = new Ellipse(
                ellipse.CenterPoint, 
                ellipse.LongRadius,ellipse.ShortRadius,
                ellipse.Angle, 
                ellipse.LongAndShort,
                ellipse.StartAngle, ellipse.EndAngle,
                ellipse.LongRadiusX, ellipse.LongRadiusY,
                ellipse.StartParameter,ellipse.EndParameter,
                ellipse.StartPoint,ellipse.EndPoint,
                ellipse.Complete);
            nEllipse.PenColor = ellipse.PenColor;
            nEllipse.ShapeID = CADInterface.globalID;
            nEllipse.ListPoint.AddRange(ellipse.ListPoint);

            CADInterface.globalID = CADInterface.globalID + 1;
            //CADInterface.currentShapes.Add(nEllipse);

            return nEllipse;
        }

        /// <summary>
        /// 复制圆弧
        /// </summary>
        /// <param name="arc"></param>
        private BaseShape copyArc(Arc arc)
        {
            Arc nArc = new Arc(arc.CenterPoint, arc.Radius, arc.StartAngle, arc.EndAngle,arc.StartPoint,arc.EndPoint);
            nArc.PenColor = arc.PenColor;
            nArc.ShapeID = CADInterface.globalID;
            nArc.ListPoint.AddRange(arc.ListPoint);

            CADInterface.globalID = CADInterface.globalID + 1;
            //CADInterface.currentShapes.Add(nArc);
            return nArc;
        }

        /// <summary>
        /// 复制椭圆弧
        /// </summary>
        /// <param name="ellipse"></param>
        private BaseShape copyEllipseArc(Ellipse ellipse)
        {
            Ellipse nEllipse = new Ellipse(
                ellipse.CenterPoint,
                ellipse.LongRadius, ellipse.ShortRadius,
                ellipse.Angle,
                ellipse.LongAndShort,
                ellipse.StartAngle, ellipse.EndAngle,
                ellipse.LongRadiusX, ellipse.LongRadiusY, 
                ellipse.StartParameter,ellipse.EndParameter,
                ellipse.StartPoint, ellipse.EndPoint,
                ellipse.Complete);
            nEllipse.PenColor = ellipse.PenColor;
            nEllipse.ShapeID = CADInterface.globalID;
            nEllipse.ListPoint.AddRange(ellipse.ListPoint);

            CADInterface.globalID = CADInterface.globalID + 1;
            //CADInterface.currentShapes.Add(nEllipse);
            return nEllipse;
        }
    }
}
