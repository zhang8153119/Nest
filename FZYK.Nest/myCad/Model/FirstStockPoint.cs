using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class FirstStockPoint
    {
        private int startInt = 0;                                   //起始点边的起始点,在model点列表中的位置
        private int endInt = 0;                                   //起始点边的终点,在model点列表中的位置
        private PointF point = new PointF();                      //起始点的点值

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

        public PointF Point
        {
            get
            {
                return point;
            }

            set
            {
                point = value;
            }
        }
    }
}
