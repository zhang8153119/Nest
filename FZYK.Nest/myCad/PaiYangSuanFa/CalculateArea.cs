using myCad.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.PaiYangSuanFa
{
    public class CalculateArea
    {
        /// <summary>
        /// 获得件号的面积
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public PlateModel getPlateArea(PlateModel plate)
        {
            if (plate.InnerModel.Count > 0)
            {
                plate.Area = plate.OutModel.Area;
                for (int i = 0; i< plate.InnerModel.Count;i++)
                {
                    plate.Area = plate.Area - plate.InnerModel[i].Area;
                }
            }
            else
            {
                plate.Area = plate.OutModel.Area;
            }
            return plate;
        }

        public PlateModel getPlateBound(PlateModel plate)
        {
            plate.Bound = plate.OutModel.Bound;
            return plate;
        }
    }
}
