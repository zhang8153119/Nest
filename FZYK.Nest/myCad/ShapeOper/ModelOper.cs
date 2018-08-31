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
    class ModelOper
    {

        /// <summary>
        /// 对model面积进行排序
        /// </summary>
        /// <param name="list"></param>
        /// <param name="desc">true从大到小，false从小到大</param>
        /// <returns></returns>
        public List<BaseModel> sortModel(List<BaseModel> list,bool desc)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (desc)
                    {
                        if (list[i].Area < list[j].Area)
                        {
                            BaseModel newModel = list[i];
                            list[i] = list[j];
                            list[j] = newModel; 
                        }
                    }
                    else
                    {
                        if (list[i].Area > list[j].Area)
                        {
                            BaseModel newModel = list[i];
                            list[i] = list[j];
                            list[j] = newModel;

                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 将读取初来的线分成一个一个的model先存放在InnerModel中
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public PlateModel baseShapeSort(PlateModel plate)
        {
            List<BaseModel> listModel = new List<BaseModel>();
            while (plate.OutModel.ListShape.Count > 0)
            {
                BaseModel newModel = new BaseModel();
                BaseShape shape = plate.OutModel.ListShape[0];
                if ("Circle".Equals(shape.ShapeClass) || ("Ellipse".Equals(shape.ShapeClass) && ((Ellipse)shape).Complete))
                {
                    #region 单个自成循环
                    newModel.ListShape.Add(shape);
                    newModel.IsArc = true;
                    plate.OutModel.ListShape.RemoveAt(0);
                    newModel.ListPoint.Add(shape.StartPoint);
                    if ("Circle".Equals(shape.ShapeClass))
                    {
                        newModel.ListPoint.AddRange(((Circle)shape).ListPoint);
                    }
                    else
                    {
                        newModel.ListPoint.AddRange(((Ellipse)shape).ListPoint);
                    }
                    #endregion
                }
                else if (plate.OutModel.ListShape.Count > 1)
                {
                    #region 多段线形成循环
                    newModel.ListShape.Add(shape);
                    PointF signSp = new PointF(shape.StartPoint.X, shape.StartPoint.Y);
                    PointF signEp = new PointF(shape.EndPoint.X, shape.EndPoint.Y);
                    newModel.ListPoint.Add(signEp);
                    bool endLoop = false;
                    bool reSetOrder = false;                //判断是否需要重置起点和终点
                    while (!endLoop)
                    {
                        int sign = -99;
                        if (plate.OutModel.ListShape.Count > 1)
                        {
                            #region 剩下的线段数量大于1进行判断
                            for (int i = 1; i < plate.OutModel.ListShape.Count; i++)
                            {
                                BaseShape getShape = plate.OutModel.ListShape[i];
                                if (pointToPoint(signEp, getShape.StartPoint) < 0.1)
                                {
                                    newModel.ListShape.Add(getShape);
                                    signEp = new PointF(getShape.EndPoint.X, getShape.EndPoint.Y);
                                    sign = i;
                                    if ("Arc".Equals(getShape.ShapeClass) || "Ellipse".Equals(getShape.ShapeClass))
                                    {
                                        if ("Arc".Equals(getShape.ShapeClass))
                                        {
                                            newModel.ListPoint.AddRange(((Arc)getShape).ListPoint);
                                        }
                                        else
                                        {
                                            newModel.ListPoint.AddRange(((Ellipse)getShape).ListPoint);
                                        }
                                        newModel.ListPoint.Add(signEp);
                                    }
                                    else
                                    {
                                        newModel.ListPoint.Add(signEp);
                                    }
                                    break;
                                }
                                else if (pointToPoint(signEp, getShape.EndPoint) < 0.1)
                                {
                                    newModel.ListShape.Add(getShape);
                                    signEp = new PointF(getShape.StartPoint.X, getShape.StartPoint.Y);
                                    sign = i;
                                    reSetOrder = true;
                                    break;
                                }
                            }
                            #endregion
                        }

                        #region 没有找到线段，跳出while循环
                        if (sign == -99)
                        {
                            endLoop = true;
                            break;
                        }
                        else
                        {
                            plate.OutModel.ListShape.RemoveAt(sign);
                        }
                        #endregion

                        #region 如果线段已经和起始线段相交，则直接退出本次循环
                        if (pointToPoint(signSp, signEp) < 0.1)
                        {
                            endLoop = true;
                            break;
                        }
                        #endregion

                    }
                    plate.OutModel.ListShape.RemoveAt(0);

                    if (reSetOrder)
                    {
                        newModel.ListPoint.Clear();
                        newModel = reSetSpEpOrder(newModel);
                    }
                    #endregion
                }
                newModel.ModelId = CADInterface.globalModelID;
                CADInterface.globalModelID = CADInterface.globalModelID + 1;
                listModel.Add(newModel);
            }
            plate.InnerModel.AddRange(listModel);
            return plate;
        }

        /// <summary>
        /// 重置线段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private BaseModel baseShapeSort(BaseModel model, int sign)
        {
            #region 重新构造线段顺序
            List<BaseShape> newList = new List<BaseShape>();
            PointF signPoint = new PointF();
            if (sign == 0)
            {
                newList.Add(model.ListShape[sign + 1]);
                newList.Add(model.ListShape[sign]);
                model.ListShape.RemoveAt(sign + 1);
                model.ListShape.RemoveAt(sign);
                signPoint = newList[1].StartPoint;
            }
            else if (sign == model.ListShape.Count - 1)
            {
                newList.Add(model.ListShape[sign]);
                newList.Add(model.ListShape[sign - 1]);
                model.ListShape.RemoveAt(sign);
                model.ListShape.RemoveAt(sign - 1);
                signPoint = pointToPoint(newList[0].EndPoint, newList[1].StartPoint) < 0.1 ? newList[1].EndPoint : newList[1].StartPoint;
            }
            else
            {
                newList.Add(model.ListShape[sign]);
                newList.Add(model.ListShape[sign - 1]);
                model.ListShape.RemoveAt(sign);
                model.ListShape.RemoveAt(sign - 1);
                signPoint = pointToPoint(newList[0].EndPoint, newList[1].StartPoint) < 0.1 ? newList[1].EndPoint : newList[1].StartPoint;
            }
            bool getNewList = true;
            while (getNewList)
            {
                if (model.ListShape.Count > 0)
                {
                    int getSign = -99;
                    for (int i = 0; i < model.ListShape.Count; i++)
                    {
                        if (pointToPoint(signPoint, model.ListShape[i].StartPoint) < 0.1)
                        {
                            newList.Add(model.ListShape[i]);
                            signPoint = model.ListShape[i].EndPoint;
                            getSign = i;
                            break;
                        }
                        else if (pointToPoint(signPoint, model.ListShape[i].EndPoint) < 0.1)
                        {
                            newList.Add(model.ListShape[i]);
                            signPoint = model.ListShape[i].StartPoint;
                            getSign = i;
                            break;
                        }
                    }
                    model.ListShape.RemoveAt(getSign);
                }
                else
                {
                    getNewList = false;
                }
            }
            model.ListShape.AddRange(newList);
            #endregion
            return model;
        }

        /// <summary>
        /// 重置线段的起始点，让线段一定的顺序方向进行组合
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseModel reSetSpEpOrder(BaseModel model)
        {
            bool bn = true;
            while (bn)
            {
                int sign = -99;  //调整参照点，选取圆弧和椭圆弧为参照点，无则以第一点开始为参照点
                int countArc = 0;
                for (int i = 0; i < model.ListShape.Count; i++)
                {
                    if ("Arc".Equals(model.ListShape[i].ShapeClass) || "Ellipse".Equals(model.ListShape[i].ShapeClass))
                    {
                        sign = sign == -99 ? i : sign;
                        countArc = countArc + 1;
                        sign = countArc == 2 ? i - 1 : sign;
                        //break;
                    }
                    else
                    {
                        countArc = 0;
                    }
                }
                sign = sign == -99 ? 0 : sign;

                bool end = true;

                #region 基点顺序正确的时候

                #region 标记处之前的线段

                PointF sp = model.ListShape[sign].StartPoint;
                PointF ep = model.ListShape[sign].EndPoint;
                for (int i = sign - 1; i >= 0; i--)
                {
                    if ("Line".Equals(model.ListShape[i].ShapeClass))
                    {
                        if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                        {
                            if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1)
                            {
                                PointF sPoint = model.ListShape[i].StartPoint;
                                model.ListShape[i].StartPoint = model.ListShape[i].EndPoint;
                                model.ListShape[i].EndPoint = sPoint;
                                sp = model.ListShape[i].StartPoint;
                            }
                            else
                            {
                                sp = model.ListShape[i].StartPoint;
                            }
                        }
                        else if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                        {
                            end = false;
                            break;
                        }
                    }
                    else
                    {
                        if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                        {
                            if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1)
                            {
                                sp = model.ListShape[i].EndPoint;
                            }
                            else
                            {
                                sp = model.ListShape[i].StartPoint;
                            }
                        }
                    }
                }
                #endregion

                #region 标记处之后的线段
                if (end)
                {
                    sp = model.ListShape[sign].StartPoint;
                    ep = model.ListShape[sign].EndPoint;
                    for (int i = sign + 1; i < model.ListShape.Count; i++)
                    {
                        if ("Line".Equals(model.ListShape[i].ShapeClass))
                        {
                            if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                            {
                                if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1)
                                {
                                    PointF ePoint = model.ListShape[i].StartPoint;
                                    model.ListShape[i].StartPoint = model.ListShape[i].EndPoint;
                                    model.ListShape[i].EndPoint = ePoint;
                                    ep = model.ListShape[i].EndPoint;
                                }
                                else
                                {
                                    ep = model.ListShape[i].EndPoint;
                                }
                            }
                            else if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                            {
                                end = false;
                                break;
                            }
                        }
                        else
                        {
                            if (pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1)
                            {
                                if (pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                                {
                                    ep = model.ListShape[i].EndPoint;
                                }
                                else
                                {
                                    ep = model.ListShape[i].StartPoint;
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion

                if (end)
                {
                    bn = false;
                }
                else
                {
                    model = baseShapeSort(model, sign);
                }
            }
            model = reSetModelList(model);
            return model;
        }

        /// <summary>
        /// 按顺序获取model中的顶点坐标
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseModel reSetModelList(BaseModel model)
        {
            BaseShape shape = model.ListShape[0];
            PointF signSp = new PointF(shape.StartPoint.X, shape.StartPoint.Y);
            PointF signEp = new PointF(shape.EndPoint.X, shape.EndPoint.Y);
            if ("Arc".Equals(shape.ShapeClass) || "Ellipse".Equals(shape.ShapeClass))
            {
                if ("Arc".Equals(shape.ShapeClass))
                {
                    model.ListPoint.AddRange(((Arc)shape).ListPoint);
                }
                else
                {
                    model.ListPoint.AddRange(((Ellipse)shape).ListPoint);
                }
                model.ListPoint.Add(signEp);
            }
            else
            {
                model.ListPoint.Add(signEp);
            }
            for (int i = 1; i < model.ListShape.Count; i++)
            {
                BaseShape getShape = model.ListShape[i];
                if (pointToPoint(signEp, getShape.StartPoint) < 0.1)
                {
                    signEp = new PointF(getShape.EndPoint.X, getShape.EndPoint.Y);
                    if ("Arc".Equals(getShape.ShapeClass) || "Ellipse".Equals(getShape.ShapeClass))
                    {
                        if ("Arc".Equals(getShape.ShapeClass))
                        {
                            model.ListPoint.AddRange(((Arc)getShape).ListPoint);
                        }
                        else
                        {
                            model.ListPoint.AddRange(((Ellipse)getShape).ListPoint);
                        }
                        model.ListPoint.Add(signEp);
                    }
                    else
                    {
                        model.ListPoint.Add(signEp);
                    }
                }
                else if (pointToPoint(signEp, getShape.EndPoint) < 0.1)
                {
                    signEp = new PointF(getShape.StartPoint.X, getShape.StartPoint.Y);
                    if ("Arc".Equals(getShape.ShapeClass) || "Ellipse".Equals(getShape.ShapeClass))
                    {
                        if ("Arc".Equals(getShape.ShapeClass))
                        {
                            for (int j = ((Arc)getShape).ListPoint.Count - 1; j >= 0; j--)
                            {
                                model.ListPoint.Add(((Arc)getShape).ListPoint[j]);
                            }
                        }
                        else
                        {
                            for (int j = ((Ellipse)getShape).ListPoint.Count - 1; j >= 0; j--)
                            {
                                model.ListPoint.Add(((Ellipse)getShape).ListPoint[j]);
                            }
                        }
                        model.ListPoint.Add(signEp);
                    }
                    else
                    {
                        model.ListPoint.Add(signEp);
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 将model进行内外的分类，区分是内model还是外model
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public PlateModel reSetOutInner(PlateModel plate)
        {
            if (plate.InnerModel.Count == 1)
            {
                plate.OutModel = plate.InnerModel[0];
                plate.InnerModel.RemoveAt(0);
            }
            else if (plate.InnerModel.Count > 1)
            {
                //面积排序
                for (int j = 1; j < plate.InnerModel.Count; j++)
                {
                    if (plate.InnerModel[0].Area < plate.InnerModel[j].Area)
                    {
                        BaseModel model = plate.InnerModel[0];
                        plate.InnerModel[0] = plate.InnerModel[j];
                        plate.InnerModel[j] = model;
                    }
                }
                plate.OutModel = plate.InnerModel[0];
                plate.InnerModel.RemoveAt(0);
                for (int i = 0; i < plate.InnerModel.Count; i++)
                {
                    plate.InnerModel[i].IsInner = true;
                }
            }
            return plate;
        }

        /// <summary>
        /// 判断件号是不是圆弧类型的件号
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public PlateModel juedeIsArc(PlateModel plate)
        {
            for (int i = 0; i < plate.OutModel.ListShape.Count; i++)
            {
                if ("Arc".Equals(plate.OutModel.ListShape[i].ShapeClass)
                    || "Ellipse".Equals(plate.OutModel.ListShape[i].ShapeClass)
                    || "Circle".Equals(plate.OutModel.ListShape[i].ShapeClass))
                {
                    plate.OutModel.IsArc = true;
                    plate.IsArc = true;
                    break;
                }
            }

            for (int i = 0; i < plate.InnerModel.Count; i++)
            {
                for (int j = 0; j < plate.InnerModel[i].ListShape.Count; j++)
                {
                    plate.InnerModel[i].IsArc = true;
                    break;
                }
            }
            return plate;
        }

        /// <summary>
        /// Model的点获取后，根据要求，重置节点顺序
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isClockwise"> true 为顺时针，false为逆时针</param>
        /// <returns></returns>
        public BaseModel reSetPointShunXu(BaseModel model, bool isClockwise)
        {
            int sign = 0;
            PointF point = model.ListPoint[sign];
            for (int i = 1; i < model.ListPoint.Count; i++)
            {
                if (model.ListPoint[i].Y > point.Y)
                {
                    sign = i;
                    point = model.ListPoint[sign];
                }
            }
            bool clockwise = false;            //false代表逆时针，true代表顺时针
            if (sign == 0)
            {
                PointF sp = model.ListPoint[model.ListPoint.Count - 1];
                PointF ep = model.ListPoint[sign + 1];
                double angleEp = Math.Atan2(ep.Y - model.ListPoint[sign].Y, ep.X - model.ListPoint[sign].X) * (180 / Math.PI);
                angleEp = angleEp <= 0 ? angleEp + 360 : angleEp;
                double angleSp = Math.Atan2(sp.Y - model.ListPoint[sign].Y, sp.X - model.ListPoint[sign].X) * (180 / Math.PI);
                angleSp = angleSp <= 0 ? angleSp + 360 : angleSp;
                if (!(angleSp > angleEp))
                {
                    clockwise = true;
                }
            }
            else if (sign == model.ListPoint.Count - 1)
            {
                PointF sp = model.ListPoint[sign - 1];
                PointF ep = model.ListPoint[0];
                double angleEp = Math.Atan2(ep.Y - model.ListPoint[sign].Y, ep.X - model.ListPoint[sign].X) * (180 / Math.PI);
                angleEp = angleEp <= 0 ? angleEp + 360 : angleEp;
                double angleSp = Math.Atan2(sp.Y - model.ListPoint[sign].Y, sp.X - model.ListPoint[sign].X) * (180 / Math.PI);
                angleSp = angleSp <= 0 ? angleSp + 360 : angleSp;
                if (!(angleSp > angleEp))
                {
                    clockwise = true;
                }
            }
            else
            {
                PointF sp = model.ListPoint[sign - 1];
                PointF ep = model.ListPoint[sign + 1];
                double angleEp = Math.Atan2(ep.Y - model.ListPoint[sign].Y, ep.X - model.ListPoint[sign].X) * (180 / Math.PI);
                angleEp = angleEp <= 0 ? angleEp + 360 : angleEp;
                double angleSp = Math.Atan2(sp.Y - model.ListPoint[sign].Y, sp.X - model.ListPoint[sign].X) * (180 / Math.PI);
                angleSp = angleSp <= 0 ? angleSp + 360 : angleSp;
                if (!(angleSp > angleEp))
                {
                    clockwise = true;
                }
            }
            if ((isClockwise && !clockwise) || (!isClockwise && clockwise))
            {
                List<PointF> newList = new List<PointF>();
                for (int i = model.ListPoint.Count - 1; i >= 0; i--)
                {
                    newList.Add(model.ListPoint[i]);
                }
                model.ListPoint.Clear();
                model.ListPoint.AddRange(newList);
            }
            return model;
        }

        /// <summary>
        /// 点到点的距离
        /// </summary>
        /// <param name="pointOne"></param>
        /// <param name="pointTwo"></param>
        /// <returns></returns>
        private float pointToPoint(PointF pointOne, PointF pointTwo)
        {
            float distance = (float)Math.Sqrt(Math.Pow((pointOne.Y - pointTwo.Y), 2) + Math.Pow((pointOne.X - pointTwo.X), 2));
            return distance;
        }
    }
}
