using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.ShapeOper
{
    class CreateRegion
    {

        public CreateRegion() { }

        public GraphicsPath getModelPath(List<BaseShape> listBaseShape)
        {
            GraphicsPath gp = new GraphicsPath();
            Pen pen = new Pen(Color.White, 0);
            for (int i = 0; i < listBaseShape.Count; i++)
            {
                if ("Line".Equals(listBaseShape[i].ShapeClass))
                {
                    //GraphicsPath gpLine = new GraphicsPath();
                    Line line = (Line)listBaseShape[i];
                    gp.AddLine(line.StartPoint, line.EndPoint);
                }
                else if ("Circle".Equals(listBaseShape[i].ShapeClass))
                {
                    Circle circle = (Circle)listBaseShape[i];
                    gp.AddEllipse(circle.CenterPoint.X - circle.Radius, circle.CenterPoint.Y - circle.Radius, 2 * circle.Radius, 2 * circle.Radius);
                }
                else if ("Arc".Equals(listBaseShape[i].ShapeClass))
                {
                    Arc arc = (Arc)listBaseShape[i];
                    gp.AddArc(arc.CenterPoint.X - arc.Radius, arc.CenterPoint.Y - arc.Radius, 2 * arc.Radius, 2 * arc.Radius, arc.StartAngle, arc.EndAngle > arc.StartAngle ? arc.EndAngle - arc.StartAngle : 360 - arc.StartAngle + arc.EndAngle);
                    //gpArc.Widen(pen);
                }
                else if ("Ellipse".Equals(listBaseShape[i].ShapeClass))
                {
                    GraphicsPath gpEllipse = new GraphicsPath();
                    Ellipse ellipse = (Ellipse)listBaseShape[i];
                    if (ellipse.Complete)
                    {
                        gpEllipse.AddEllipse(0, 0, 2 * ellipse.LongRadius, 2 * ellipse.ShortRadius);
                    }
                    else
                    {
                        gpEllipse.AddArc(0, 0, 2 * ellipse.LongRadius, 2 * ellipse.ShortRadius, ellipse.StartAngle, ellipse.EndAngle > ellipse.StartAngle ? ellipse.EndAngle - ellipse.StartAngle : 360 - ellipse.StartAngle + ellipse.EndAngle);
                    }

                    Matrix matrix = new Matrix();
                    matrix.Translate(ellipse.CenterPoint.X - ellipse.LongRadius, ellipse.CenterPoint.Y - ellipse.ShortRadius, MatrixOrder.Append);
                    matrix.RotateAt(ellipse.Angle, ellipse.CenterPoint, MatrixOrder.Append);
                    gpEllipse.Transform(matrix);
                    //gpEllipse.Widen(pen);
                    gp.AddPath(gpEllipse, true);
                }
            }
            return gp;
        }

        public GraphicsPath getModelPathB(List<BaseShape> listBaseShape)
        {
            GraphicsPath gp = new GraphicsPath();
            Pen pen = new Pen(Color.White, 0);
            for (int i = 0; i < listBaseShape.Count; i++)
            {
                if ("Line".Equals(listBaseShape[i].ShapeClass))
                {
                    GraphicsPath gpLine = new GraphicsPath();
                    Line line = (Line)listBaseShape[i];
                    gpLine.AddLine(line.StartPoint, line.EndPoint);
                    gp.AddPath(gpLine, true);
                }
                else if ("Circle".Equals(listBaseShape[i].ShapeClass))
                {
                    GraphicsPath gpCircle = new GraphicsPath();
                    Circle circle = (Circle)listBaseShape[i];
                    gpCircle.AddEllipse(circle.CenterPoint.X - circle.Radius, circle.CenterPoint.Y - circle.Radius, 2 * circle.Radius, 2 * circle.Radius);
                    gp.AddPath(gpCircle, true);
                }
                else if ("Arc".Equals(listBaseShape[i].ShapeClass))
                {
                    GraphicsPath gpArc = new GraphicsPath();
                    Arc arc = (Arc)listBaseShape[i];
                    gpArc.AddArc(arc.CenterPoint.X - arc.Radius, arc.CenterPoint.Y - arc.Radius, 2 * arc.Radius, 2 * arc.Radius, arc.StartAngle, arc.EndAngle > arc.StartAngle ? arc.EndAngle - arc.StartAngle : 360 - arc.StartAngle + arc.EndAngle);
                    //gpArc.Widen(pen);
                    gp.AddPath(gpArc, true);
                }
                else if ("Ellipse".Equals(listBaseShape[i].ShapeClass))
                {
                    GraphicsPath gpEllipse = new GraphicsPath();
                    Ellipse ellipse = (Ellipse)listBaseShape[i];
                    if (ellipse.Complete)
                    {
                        gpEllipse.AddEllipse(0, 0, 2 * ellipse.LongRadius, 2 * ellipse.ShortRadius);
                    }
                    else
                    {
                        gpEllipse.AddArc(0, 0, 2 * ellipse.LongRadius, 2 * ellipse.ShortRadius, ellipse.StartAngle, ellipse.EndAngle > ellipse.StartAngle ? ellipse.EndAngle - ellipse.StartAngle : 360 - ellipse.StartAngle + ellipse.EndAngle);
                    }

                    Matrix matrix = new Matrix();
                    matrix.Translate(ellipse.CenterPoint.X - ellipse.LongRadius, ellipse.CenterPoint.Y - ellipse.ShortRadius, MatrixOrder.Append);
                    matrix.RotateAt(ellipse.Angle, ellipse.CenterPoint, MatrixOrder.Append);
                    gpEllipse.Transform(matrix);
                    //gpEllipse.Widen(pen);
                    gp.AddPath(gpEllipse, true);
                }
            }
            return gp;
        }

        public Region getModelRegion(GraphicsPath graphicsPath)
        {
            return new Region(graphicsPath);
        }


        public void createPlateRegion()
        {
            
        }

    }
}
