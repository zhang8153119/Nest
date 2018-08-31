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
    class AddOper
    {
        /// <summary>
        /// 根据线段列表生成新的model
        /// </summary>
        /// <param name="listLine"></param>
        /// <returns></returns>
        public BaseModel addModel(List<BaseShape> listLine)
        {
            BaseModel newStockModel = new BaseModel();
            CopyOper copy = new CopyOper();
            for (int i = 0; i < listLine.Count; i++)
            {
                newStockModel.ListShape.Add(copy.CopyBaseShape(listLine[i]));
            }
            
            newStockModel = new ModelOper().reSetModelList(newStockModel);
            newStockModel = new ModelOper().reSetPointShunXu(newStockModel, false);
            newStockModel.ModelId = CADInterface.globalModelID;
            CADInterface.globalModelID = CADInterface.globalModelID + 1;
            return newStockModel;
        }

        /// <summary>
        /// 根据点列表生成直线列表
        /// </summary>
        /// <param name="listPoint"></param>
        /// <param name="isAddToGlobal">判断线段要不要加到全局的变量中，true要</param>
        /// <returns></returns>
        public List<BaseShape> addLine(List<PointF> listPoint,bool isAddToGlobal)
        {
            List<BaseShape> listLine = new List<BaseShape>();
            for (int i = 0; i < listPoint.Count; i++)
            {
                if (i == listPoint.Count - 1)
                {
                    Line line = new Shape.Line(listPoint[i], listPoint[0]);
                    if (isAddToGlobal)
                    {
                        line.ShapeID = CADInterface.globalID;
                        CADInterface.globalID = CADInterface.globalID + 1;
                    }
                    listLine.Add(line);
                }
                else
                {
                    Line line = new Shape.Line(listPoint[i], listPoint[i + 1]);
                    if (isAddToGlobal)
                    {
                        line.ShapeID = CADInterface.globalID;
                        CADInterface.globalID = CADInterface.globalID + 1;
                    }
                    listLine.Add(line);
                }
            }
            return listLine;
        }

        public List<NFPLineAngle> addNFPLineAngle(List<PointF> listPoint)
        {
            List<NFPLineAngle> listNFPLine = new List<NFPLineAngle>();
            for (int i= 0;i<listPoint.Count;i++)
            {
                NFPLineAngle nfpLine = new NFPLineAngle();
                if (i == listPoint.Count - 1)
                {
                    nfpLine.StartInt = i;
                    nfpLine.EndInt = 0;
                }
                else
                {
                    nfpLine.StartInt = i;
                    nfpLine.EndInt = i + 1;
                }
                nfpLine.StartPoint = listPoint[nfpLine.StartInt];
                nfpLine.EndPoint = listPoint[nfpLine.EndInt];

                double angle = (180/Math.PI)*Math.Atan2(listPoint[nfpLine.EndInt].Y - listPoint[nfpLine.StartInt].Y, listPoint[nfpLine.EndInt].X - listPoint[nfpLine.StartInt].X);
                angle = angle < 0 ? angle + 360 : angle;
                nfpLine.Angle = (float)angle;
                listNFPLine.Add(nfpLine);
            }
            return listNFPLine;
        }
    }
}
