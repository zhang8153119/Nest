using myCad.CADInterfaceCtrl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    class Arc : BaseShape
    {
        private float radius;                     //圆弧半径(必须有)
        private float startAngle;                 //以度为单位(必须有)
        private float endAngle;                   //以度为单位(必须有)
        private PointF centerPoint;               //
        private float length;
        private List<PointF> listPoint = new List<PointF>();    //圆弧线段，每5度分割一个线段点，用于后续移动等操作使用，随几何变换而变换

        public Arc(PointF centerP,float rad,float spAngle,float epAngle)
        {
            centerPoint = centerP;
            radius = rad;
            startAngle = spAngle;
            endAngle = epAngle;
            this.ShapeClass = "Arc";
        }
        public Arc(PointF centerP, float rad, float spAngle, float epAngle,PointF sPoint,PointF ePoint)
        {
            centerPoint = centerP;
            radius = rad;
            startAngle = spAngle;
            endAngle = epAngle;
            StartPoint = sPoint;
            EndPoint = ePoint;
            this.ShapeClass = "Arc";
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

        public float StartAngle
        {
            get
            {
                return startAngle;
            }

            set
            {
                startAngle = value;
            }
        }

        public float EndAngle
        {
            get
            {
                return endAngle;
            }

            set
            {
                endAngle = value;
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

        public float Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
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

        public override void Draw(Graphics g,float zoomNum)
        {
            leftUpPX = (centerPoint.X - radius) * zoomNum;
            leftUpPY = (centerPoint.Y - radius) * zoomNum;
            rad = 2 * radius * zoomNum;

            Pen pen = new Pen(this.PenColor, 0.1f);
            pen.DashStyle = this.PenStyle;
            g.DrawArc(pen, leftUpPX, leftUpPY, rad, rad, startAngle, endAngle > startAngle ? endAngle - startAngle : 360 - startAngle + endAngle);
        }
    }
}
