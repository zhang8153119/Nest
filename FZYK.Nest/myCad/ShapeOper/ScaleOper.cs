using myCad.BaseShapeOper;
using myCad.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.ShapeOper
{
    class ScaleOper : BaseScaling
    {
        public void ScalePlate(PlateModel plate, PointF scalingPoint, float scale)
        {
            for (int i = 0; i < plate.OutModel.ListShape.Count; i++)
            {
                scaling(plate.OutModel.ListShape[i], scalingPoint, scale);
            }
            for (int i = 0; i < plate.InnerModel.Count; i++)
            {
                for (int j = 0; j < plate.InnerModel[i].ListShape.Count; i++)
                {
                    scaling(plate.InnerModel[i].ListShape[j], scalingPoint, scale);
                }
            }
        }
    }
}
