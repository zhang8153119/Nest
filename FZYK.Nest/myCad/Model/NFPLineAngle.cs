using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class NFPLineAngle
    {
        private int startInt = 0;                 //线段的起始点
        private int endInt = 0;                   //线段的终止点
        private float angle = 0;                  //线段的角度
        private PointF startPoint = new PointF();  //线段起点
        private PointF endPoint = new PointF();  //线段终点

        public int StartInt
        {
            get
            {
                return startInt;
            }

            set
            {
                startInt = value;
            }
        }

        public int EndInt
        {
            get
            {
                return endInt;
            }

            set
            {
                endInt = value;
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
            }
        }

        public PointF StartPoint
        {
            get
            {
                return startPoint;
            }

            set
            {
                startPoint = value;
            }
        }

        public PointF EndPoint
        {
            get
            {
                return endPoint;
            }

            set
            {
                endPoint = value;
            }
        }
    }
}
