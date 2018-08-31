using myCad .BaseShapeOper;
using myCad .Model;
using myCad .Shape;
using System .Collections .Generic;
using System .Drawing;

namespace myCad .ShapeOper
{
      class MoveOper : BaseMove
      {
            /// <summary>
            /// 整板移动
            /// </summary>
            /// <param name="plate"></param>
            /// <param name="moveX"></param>
            /// <param name="moveY"></param>
            public PlateModel MovePlate(PlateModel plate, float moveX, float moveY)
            {
                  for (int i = 0; i < plate .OutModel .ListShape .Count; i++)
                  {
                        move(plate .OutModel .ListShape[i], moveX, moveY);
                  }
                  for (int i = 0; i < plate .OutModel .ExpandShape .Count; i++)
                  {
                        move(plate .OutModel .ExpandShape[i], moveX, moveY);
                  }
                  plate .OutModel .IsBound = false;
                  //plate .OutModel .Draw(CADInterface .bGrp .Graphics);
                  if (plate .InnerModel .Count > 0)
                  {
                        for (int i = 0; i < plate .InnerModel .Count; i++)
                        {
                              for (int j = 0; j < plate .InnerModel[i] .ListShape .Count; j++)
                              {
                                    move(plate .InnerModel[i] .ListShape[i], moveX, moveY);
                              }
                              plate .InnerModel[i] .IsBound = false;
                              //plate .InnerModel[i] .Draw(CADInterface .bGrp .Graphics);
                              for (int j = 0; j < plate .InnerModel[i] .ListPoint .Count; j++)
                              {
                                    plate .InnerModel[i] .ListPoint[i] = move(plate .InnerModel[i] .ListPoint[i], moveX, moveY);
                              }
                        }
                  }
                  for (int i = 0; i < plate .OutModel .ListPoint .Count; i++)
                  {
                        plate .OutModel .ListPoint[i] = move(plate .OutModel .ListPoint[i], moveX, moveY);
                  }
                  for (int i = 0; i < plate .OutModel .ExpandPoint .Count; i++)
                  {
                        plate .OutModel .ExpandPoint[i] = move(plate .OutModel .ExpandPoint[i], moveX, moveY);
                  }
                  plate .RotateCenter = move(plate .RotateCenter, moveX, moveY);
                  plate .PowCenter = move(plate .PowCenter, moveX, moveY);
                  plate .Bound = new RectangleF(plate .Bound .X + moveX, plate .Bound .Y + moveY, plate .Bound .Width, plate .Bound .Height);

                  return plate;
            }

            public void MoveShape(List<BaseShape> shape, float x, float y)
            {
                  for (int i = 0; i < shape .Count; i++)
                  {
                        move(shape[i], x, y);
                  }
            }
      }
}
