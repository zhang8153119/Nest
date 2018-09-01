using myCad.CADInterfaceCtrl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.Shape
{
    class Circle : BaseShape
    {
        private float radius;
        private PointF centerPoint;
        private float perimeter;       //周长
        private float area;            //面积
        public RectangleF[] testRectangleF;
        private Graphics graphics;     //画板
        private List<PointF> listPoint = new List<PointF>();    //圆弧线段，每5度分割一个线段点，用于后续移动等操作使用，随几何变换而变换

        public Circle(PointF centerP, float rad, PointF sPoint, PointF ePoint)
        {
            centerPoint = centerP;
            radius = rad;
            StartPoint = sPoint;
            EndPoint = ePoint;
            this.ShapeClass = "Circle";
        }

        public float Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public PointF CenterPoint
        {
            get
            {
                return centerPoint;
            }

            set
            {
                centerPoint = value;
            }
        }

        public float Perimeter
        {
            get
            {
                return perimeter;
            }

            set
            {
                perimeter = value;
            }
        }

        public float Area
        {
            get
            {
                return area;
            }

            set
            {
                area = value;
            }
        }

        public List<PointF> ListPoint
        {
            get
            {
                return listPoint;
            }

            set
            {
                listPoint = value;
            }
        }

        private float leftUpPX, leftUpPY, rad;
        private PointF cep;                //中心点

        public override void Draw(Graphics g,float zoomNum)
        {
            leftUpPX = (centerPoint.X - radius) * zoomNum;
            leftUpPY = (centerPoint.Y - radius) * zoomNum;      
            rad = 2 * radius * zoomNum;
            cep.X = centerPoint.X * zoomNum;
            cep.Y = centerPoint.Y * zoomNum;

            Pen pen = new Pen(this.PenColor, 0.1f);
            pen.DashStyle = this.PenStyle;

            switch (Show)
            {
                case DisplayStyle.Normal:

                    g.DrawEllipse(pen, leftUpPX, leftUpPY, rad, rad);
                    break;
                case DisplayStyle.Preview:

                    //g.DrawEllipse(pen, leftUpPX, leftUpPY, rad, rad);
                    //g.DrawLine(pen, CenP, CirP);
                    break;
                case DisplayStyle.Hit:
                    Pen newPen1 = new Pen(PenColor, 3);
                    newPen1.DashStyle = DashStyle.Solid;
                    g.DrawEllipse(newPen1, leftUpPX, leftUpPY, rad, rad);
                    Pen newPen2 = new Pen(Color.Black, 1);
                    newPen2.DashStyle = DashStyle.Custom;
                    newPen2.DashPattern = new float[] { 3, 3 };
                    g.DrawEllipse(newPen2, leftUpPX, leftUpPY, rad, rad);
                    newPen1.Dispose(); newPen2.Dispose();
                    break;
                case DisplayStyle.Select:
                    Pen newPen3 = new Pen(PenColor, 1);
                    newPen3.DashStyle = DashStyle.Custom;
                    newPen3.DashPattern = new float[] { 3, 3 };
                    g.DrawEllipse(newPen3, leftUpPX, leftUpPY, rad, rad);
                    RectangleF[] rects = new RectangleF[]
                    {
                        new RectangleF(cep.X - 5, cep.Y - 5, 10, 10),
                        new RectangleF(leftUpPX - 5, leftUpPY  + rad / 2 - 5, 10, 10),
                        new RectangleF(leftUpPX + rad / 2 - 5, leftUpPY - 5, 10, 10),
                        new RectangleF(leftUpPX + rad - 5,  leftUpPY + rad / 2 - 5, 10, 10),
                        new RectangleF(leftUpPX + rad / 2 - 5,  leftUpPY + rad - 5, 10, 10),
                    };
                    g.FillRectangles(new SolidBrush(Color.FromArgb(0, 128, 255)), rects);
                    g.DrawRectangles(new Pen(Color.Gray), rects);
                    newPen3.Dispose();
                    break;
                case DisplayStyle.Edit:
                    Pen newPen4 = new Pen(PenColor, 1);
                    newPen4.DashStyle = DashStyle.Custom;
                    newPen4.DashPattern = new float[] { 3, 3 };
                    g.DrawEllipse(newPen4, leftUpPX, leftUpPY, rad, rad);
                    newPen4.Dispose();
                    break;
            }
        }

        public override bool SurroundedBy(RectangleF rectangle)//正框选方法，如果图形整个被包围在矩形选框内则返回true。
        {
            PointF[] surroundPoint = new PointF[] 
            { new PointF(CenterPoint.X - Radius, CenterPoint.Y - Radius),
              new PointF(CenterPoint.X + Radius, CenterPoint.Y + Radius) };

            foreach (PointF item in surroundPoint)
            {
                if (IsInBox(item, rectangle) == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void CreateObjects()
        {
            if (AreaPath != null)
                return;

            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.White, 0);
            AreaPath.AddEllipse(CenterPoint.X - Radius, CenterPoint.Y - Radius, 2 * Radius, 2 * Radius);
            AreaRegion = new Region(AreaPath);
        }

        /// <summary>
        /// 判断相交，反选区域相交
        /// </summary>
        /// <param name="negativerect"></param>
        /// <returns></returns>
        public override bool IntersectsWith(RectangleF negativerect)       
        {
            CreateObjects();
            return AreaRegion.IsVisible(negativerect);
        }

        private RectangleF[] testGetRectangle(Matrix matrix)
        {
            Matrix matrix1 = new Matrix();
            matrix1.Scale(10,10);
            return AreaRegion.GetRegionScans(matrix1);
        }

    }
}
