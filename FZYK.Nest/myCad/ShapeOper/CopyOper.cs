using myCad .BaseShapeOper;
using myCad .CADInterfaceCtrl;
using myCad .Model;
using myCad .Shape;
using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .ShapeOper
{
      class CopyOper : BaseCopy
      {
            /// <summary>
            /// 整版复制
            /// </summary>
            /// <param name="plate"></param>
            /// <returns></returns>
            public PlateModel CopyPlate(PlateModel plate)
            {
                  PlateModel copyPlate = new PlateModel();
                  BaseModel newOutModel = new BaseModel();
                  for (int i = 0; i < plate .OutModel .ListShape .Count; i++)
                  {
                        newOutModel .ListShape .Add(copy(plate .OutModel .ListShape[i]));
                  }
                  for (int i = 0; i < plate .OutModel .ExpandShape .Count; i++)
                  {
                        newOutModel .ExpandShape .Add(copy(plate .OutModel .ExpandShape[i]));
                  }
                  newOutModel .ListPoint .AddRange(plate .OutModel .ListPoint);
                  newOutModel .ExpandPoint .AddRange(plate .OutModel .ExpandPoint);
                  newOutModel .IsArc = plate .OutModel .IsArc;
                  newOutModel .IsInner = plate .OutModel .IsInner;
                  newOutModel .Area = plate .OutModel .Area;
                  newOutModel .Bound = plate .OutModel .Bound;
                  //newOutModel .ModelId = CADInterface .globalModelID;
                  //CADInterface .globalModelID = CADInterface .globalModelID + 1;
                  //CADInterface .currentPlates .Add(newOutModel);
                  //newOutModel.Draw(CADInterface.bGrp.Graphics);
                  copyPlate .OutModel = newOutModel;

                  if (plate .InnerModel .Count > 0)
                  {
                        for (int i = 0; i < plate .InnerModel .Count; i++)
                        {
                              BaseModel newInnerModel = new BaseModel();
                              for (int j = 0; j < plate .InnerModel[i] .ListShape .Count; j++)
                              {
                                    newInnerModel .ListShape .Add(copy(plate .InnerModel[i] .ListShape[i]));
                              }
                              newInnerModel .ListPoint .AddRange(plate .InnerModel[i] .ListPoint);
                              newInnerModel .IsArc = plate .InnerModel[i] .IsArc;
                              newInnerModel .IsInner = plate .InnerModel[i] .IsInner;
                              newInnerModel .Area = plate .InnerModel[i] .Area;
                              newInnerModel .Bound = plate .InnerModel[i] .Bound;
                              //newInnerModel.Draw(CADInterface.bGrp.Graphics);
                              //newInnerModel .ModelId = CADInterface .globalModelID;
                              //CADInterface .globalModelID = CADInterface .globalModelID + 1;
                              //CADInterface .currentPlates .Add(newInnerModel);
                              copyPlate .InnerModel .Add(newInnerModel);
                        }
                  }

                  copyPlate .RotateCenter = plate .RotateCenter;
                  copyPlate .PowCenter = plate .PowCenter;
                  copyPlate .IsArc = plate .IsArc;
                  copyPlate .PlateCode = plate .PlateCode;
                  copyPlate .PlateName = plate .PlateName;
                  copyPlate .PlateNum = plate .PlateNum;
                  copyPlate .Area = plate .Area;
                  copyPlate .Bound = plate .Bound;
                  copyPlate .Rect = plate .Rect;

                  copyPlate .GridValue = plate .GridValue == null ? null : plate .GridValue .ToList();
                  copyPlate .id = plate .id;
                  copyPlate .GridLen = plate .GridLen;
                  copyPlate .GridWid = plate .GridWid;
                  copyPlate .Rect = plate .Rect;
                  copyPlate .Combine = plate .Combine;
                  copyPlate .PlateCount = plate .PlateCount;

                  return copyPlate;
            }

            /// <summary>
            /// 复制单个基础线段
            /// </summary>
            /// <param name="baseShape"></param>
            /// <returns></returns>
            public BaseShape CopyBaseShape(BaseShape baseShape)
            {
                  return copy(baseShape);
            }

            public BaseModel CopyModel(BaseModel model)
            {
                  BaseModel newModel = new BaseModel();
                  newModel .ListPoint .AddRange(model .ListPoint);
                  for (int i = 0; i < model .ListShape .Count; i++)
                  {
                        newModel .ListShape .Add(copy(model .ListShape[i]));
                  }
                  //newModel .ModelId = CADInterface .globalModelID;
                  //CADInterface .globalModelID = CADInterface .globalModelID + 1;
                  //CADInterface .currentPlates .Add(newModel);

                  return newModel;
            }
      }
}
