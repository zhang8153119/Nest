using myCad.CADInterfaceCtrl;
using myCad.Model;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.ShapeOper
{
    class CreateCenterPoint
    {
        public PlateModel getRotateCenter(PlateModel plate)
        {
            plate.RotateCenter = new System.Drawing.PointF(plate.Bound.X + (plate.Bound.Width / 2), plate.Bound.Y + (plate.Bound.Height / 2));
            return plate;
        }
        public PlateModel getPowCenter(PlateModel plate)
        {
            float powX = 0;
            float powY = 0;
            for (int i = 0; i < plate.OutModel.ListPoint.Count; i++)
            {
                powX = powX + plate.OutModel.ListPoint[i].X;
                powY = powY + plate.OutModel.ListPoint[i].Y;
            }
            powX = powX / plate.OutModel.ListPoint.Count;
            powY = powY / plate.OutModel.ListPoint.Count;

            plate.PowCenter = new PointF(powX, powY);
            //CADInterface.currentShapes.Add(new Line(plate.OutModel.ListPoint[0], plate.PowCenter));
            return plate;
        }
    }
}
