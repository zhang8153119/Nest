/***
 件号的运动轨迹
 */
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class Trajectory
    {
        private List<PointF> listPoint = new List<PointF>();           //件号的重心轨迹
        private float angle = 0;                                       //旋转角度
        private PlateModel plate;                                      //轨迹件号

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
    }
}
