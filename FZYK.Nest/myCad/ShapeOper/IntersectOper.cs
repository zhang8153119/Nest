using myCad.BaseShapeOper;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.ShapeOper
{
    class IntersectOper : BaseIntersect
    {
        /// <summary>
        /// 两线段相交
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="lineTwo"></param>
        /// <returns></returns>
        public List<PointF> getIntersert(Line lineOne,Line lineTwo)
        {
            List<PointF> listPointF = new List<PointF>();
            listPointF.AddRange(intersect(lineOne,lineTwo));
            bool first = false;
            bool second = false;
            float xMin = lineOne.StartPoint.X < lineOne.EndPoint.X ? lineOne.StartPoint.X : lineOne.EndPoint.X;
            float xMax = lineOne.StartPoint.X > lineOne.EndPoint.X ? lineOne.StartPoint.X : lineOne.EndPoint.X;
            float yMin = lineOne.StartPoint.Y < lineOne.EndPoint.Y ? lineOne.StartPoint.Y : lineOne.EndPoint.Y;
            float yMax = lineOne.StartPoint.Y > lineOne.EndPoint.Y ? lineOne.StartPoint.Y : lineOne.EndPoint.Y;
            if (listPointF.Count > 0 && (xMin <= listPointF[0].X && xMax >= listPointF[0].X)
                && (yMin <= listPointF[0].Y && yMax >= listPointF[0].Y))
            {
                first = true;
            }
            xMin = lineTwo.StartPoint.X < lineTwo.EndPoint.X ? lineTwo.StartPoint.X : lineTwo.EndPoint.X;
            xMax = lineTwo.StartPoint.X > lineTwo.EndPoint.X ? lineTwo.StartPoint.X : lineTwo.EndPoint.X;
            yMin = lineTwo.StartPoint.Y < lineTwo.EndPoint.Y ? lineTwo.StartPoint.Y : lineTwo.EndPoint.Y;
            yMax = lineTwo.StartPoint.Y > lineTwo.EndPoint.Y ? lineTwo.StartPoint.Y : lineTwo.EndPoint.Y;
            if (listPointF.Count > 0 && (xMin <= listPointF[0].X && xMax >= listPointF[0].X)
                && (yMin <= listPointF[0].Y && yMax >= listPointF[0].Y))
            {
                second = true;
            }
            if (first && second)
            {
                return listPointF;
            }
            else
            {
                return new List<PointF>();
            }

        }

        /// <summary>
        /// 判断点是否在直线上
        /// </summary>
        /// <param name="lineOne"></param>
        /// <param name="lineTwo"></param>
        /// <param name="judgePoint"></param>
        /// <returns></returns>
        public bool judgePointInLine(Line lineOne,Line lineTwo,PointF judgePoint)
        {
            List<PointF> listPointF = new List<PointF>();
            listPointF.AddRange(intersect(lineOne, lineTwo));
            if (listPointF.Count > 0)
            {
                if (pointToPoint(listPointF[0], judgePoint) < 0.1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
    }
}
