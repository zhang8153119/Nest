using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class StockPlatePointSign
    {
        private int sign = 0;                            //点在原来的点集合中所在的位置
        private bool used = false;                       //这个点是否已经使用过了
        private PointF point = new PointF();

        public int Sign
        {
            get
            {
                return sign;
            }

            set
            {
                sign = value;
            }
        }

        public bool Used
        {
            get
            {
                return used;
            }

            set
            {
                used = value;
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
