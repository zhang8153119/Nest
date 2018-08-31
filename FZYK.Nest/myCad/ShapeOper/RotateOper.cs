using myCad .BaseShapeOper;
using myCad .Model;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .ShapeOper
{
      class RotateOper : BaseRotate
      {
            /// <summary>
            /// 旋转件号
            /// </summary>
            /// <param name="plate"></param>
            /// <param name="rotatePoint"></param>
            /// <param name="rotateAngle">角度以度的形式</param>
            public PlateModel RotatePlate(PlateModel plate, PointF rotatePoint, float rotateAngle)
            {
                  float angle = (float)(rotateAngle * (Math .PI / 180));
                  for (int i = 0; i < plate .OutModel .ListShape .Count; i++)
                  {
                        rotate(plate .OutModel .ListShape[i], rotatePoint, angle);
                  }
                  for (int i = 0; i < plate .OutModel .ExpandShape .Count; i++)
                  {
                        rotate(plate .OutModel .ExpandShape[i], rotatePoint, angle);
                  }
                  for (int i = 0; i < plate .OutModel .ListPoint .Count; i++)
                  {
                        plate .OutModel .ListPoint[i] = rotate(plate .OutModel .ListPoint[i], rotatePoint, angle);
                  }
                  for (int i = 0; i < plate .OutModel .ExpandPoint .Count; i++)
                  {
                        plate .OutModel .ExpandPoint[i] = rotate(plate .OutModel .ExpandPoint[i], rotatePoint, angle);
                  }
                  for (int i = 0; i < plate .InnerModel .Count; i++)
                  {
                        for (int j = 0; j < plate .InnerModel[i] .ListShape .Count; j++)
                        {
                              rotate(plate .InnerModel[i] .ListShape[j], rotatePoint, angle);
                        }
                  }
                  for (int i = 0; i < plate .InnerModel .Count; i++)
                  {
                        for (int j = 0; j < plate .InnerModel[i] .ListPoint .Count; j++)
                        {
                              plate .InnerModel[i] .ListPoint[j] = rotate(plate .InnerModel[i] .ListPoint[j], rotatePoint, angle);
                        }
                  }

                  plate .PowCenter = rotate(plate .PowCenter, rotatePoint, angle);
                  plate .RotateCenter = rotate(plate .RotateCenter, rotatePoint, angle);
                  return plate;
            }
            public void RotateCSYS(PlateModel plate, float movey)
            {
                  RotatePlate(plate, new PointF(0, 0), 90);
                  new MoveOper() .MovePlate(plate, 0, movey);
            }
            public void RotateCSYS(List<BaseShape> shape, float movey)
            {
                  for (int i = 0; i < shape .Count; i++)
                  {
                        rotate(shape[i], new PointF(0, 0), Convert .ToSingle(90 * (Math .PI / 180)));
                  }
                  new MoveOper() .MoveShape(shape, 0, movey);
            }

            public PointF RotatePoint(PointF point, PointF rotatePoint, float rotateAngle)
            {
                  float angle = (float)(rotateAngle * (Math .PI / 180));
                  return rotate(point, rotatePoint, angle);
            }
      }
}
