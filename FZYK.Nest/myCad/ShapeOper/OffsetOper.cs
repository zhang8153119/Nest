using myCad .BaseShapeOper;
using myCad .Model;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .ShapeOper
{
      class OffsetOper : BaseOffset
      {
            public void OffsetPlate(PlateModel plate, float len)
            {
                  for (int i = 0; i < plate .OutModel .ListShape .Count; i++)
                  {
                        offset(plate .OutModel .ListShape[i], len);
                  }
            }

      }
}
