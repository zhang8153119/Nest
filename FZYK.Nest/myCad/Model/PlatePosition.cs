using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class PlatePosition
    {
        private PlateModel plate;                           //排样的件号
        private PointF position;                            //该件号重心所在的位置
        private float angle;                                //以件号中点旋转的角度

        public PlateModel Plate
        {
            get
            {
                return plate;
            }

            set
            {
                plate = value;
            }
        }

        public PointF Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
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
    }
}
