using myCad.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myCad.ShapeOper;
using System.Drawing;
using myCad.Shape;
using myCad.CADInterfaceCtrl;
using System.Windows.Forms;
using myCad.BaseShapeOper;

namespace myCad.PaiYangSuanFa
{
    class NFPSuanFa
    {
        private List<int> listDeleteId = new List<int>();

        public MoveOper moveOper = new MoveOper();

        /// <summary>
        /// 获取初始解，优先排编码靠前的基因序列，简单的解情况，原材料碎片的利用，较小的碎片先利用，利用不了的时候采用较大的碎片
        /// </summary>
        public PaiYangFangAn createFirstFangAn(List<PlateModel> listGene, Stock stock,int angleTest)
        {
            PaiYangFangAn pyfa = new PaiYangFangAn();
            int signFirstGene = 0;          //判断第一次选择基因的起始点
            bool sAndLGArea = judgeStockAndListGeneArea(stock,listGene);     //判断原材料面积和钢板基因序列未使用的件号面积和
            bool bn = true;                 //算法判断开始
            int sigModel = 0;
            int count = 0;     //执行次数
            while (bn)
            {

                #region 获取当前最前面的原材料钢板碎片
                BaseModel stockDebris = stock.ListModel[sigModel];
                if (stockDebris.ListShape.Count == 0)
                {
                    //不存在可用碎片的时候退出方案
                    bn = false;
                    break;
                }
                #endregion

                int signGene = selectFirstGeneFalse(listGene, pyfa, signFirstGene);

                #region 判断是否存在基因序列
                if (signGene == -99)
                {
                    //不存在基因序列的时候退出方案
                    bn = false;
                    break;
                }
                #endregion

                #region 判断该基因序列的面积是否符合标准
                if (stockDebris.Area < listGene[signGene].Area)
                {
                    //当可排样碎片面积小于排样件号面积的时候重新选择件号，件号要求小于碎片
                    signGene = selectFirstSortByArea(signGene, listGene, pyfa, stockDebris);
                    if (signGene == -99)
                    {
                        //不存在基因序列的时候退出方案
                        bn = false;
                        break;
                    }
                }
                #endregion

                List<Trajectory> listTrajectory = new List<Trajectory>();   //记录重心的运行轨迹
                PointF SPointSign = listGene[signGene].RotateCenter;
                List<PointF> signPointList = new List<PointF>();
                signPointList.AddRange(listGene[signGene].OutModel.ListPoint);
                for (int i = 0; i < (angleTest == -9999 ? 360 / 5 : 360 / 360); i++)
                {
                    //有问题
                    #region 开始运行临界判断，记录可行的临界轨迹
                    //listGene[signGene] = new RotateOper().RotatePlate(listGene[signGene], listGene[signGene].RotateCenter, (angleTest == -9999 ? 5 * i : angleTest));
                    listGene[signGene] = new RotateOper().RotatePlate(listGene[signGene], SPointSign, (angleTest == -9999 ? 5 * i : angleTest));
                    FirstStockPoint startLoopPoint = getFirstPoint(stockDebris);        //作为初始位置
                    List<PointF> newListStockPoint = getNewListPoint(stockDebris.ListPoint, startLoopPoint.StartInt);    //获取新的原材料点顺序，起始点所在的线的点置为起始点
                    int signTop = getPlateFirstTopPoint(listGene[signGene].OutModel); //作为初始位置的对接点
                    List<PointF> newListPlatePoint = getNewListPoint(listGene[signGene].OutModel.ListPoint, signTop);
                    
                    List<NFPLineAngle> stockLine = new AddOper().addNFPLineAngle(newListStockPoint); //获取原材料碎片的边界线角度和起始
                    List<NFPLineAngle> plateLine = new AddOper().addNFPLineAngle(newListPlatePoint);
                    List<Line> listGuiJi = new List<Line>();      //记录内NFP的移动轨迹
                    for (int j = 0; j < stockLine.Count; j++)
                    {
                        if (judgePlusOrMinus(stockLine, j))
                        {
                            #region 为凸点，即件号上的点在原材料钢板上的线的边界做移动
                            PointF getSign = getSignGuiJiPointTu(stockLine[j], newListStockPoint, newListPlatePoint);
                            //移动到初始状态，起始点，获取重心坐标
                            float moveX = newListStockPoint[stockLine[j].StartInt].X - getSign.X;
                            float moveY = newListStockPoint[stockLine[j].StartInt].Y - getSign.Y;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                moveX,
                                moveY);
                            //CADInterface.DrawShap();
                            PointF lineSp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));

                            moveX = newListStockPoint[stockLine[j].EndInt].X - getSign.X;
                            moveY = newListStockPoint[stockLine[j].EndInt].Y - getSign.Y;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                               moveX,
                               moveY);
                            PointF lineEp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));

                            listGuiJi.Add(new Line(lineSp, lineEp));
                            if (count == 1)
                            {
                                //CADInterface.currentShapes.Add(new Line(lineSp, lineEp));
                            }
                            #endregion
                        }
                        else
                        {
                            #region 为凹点，即原材料上的点在件号上的线上移动
                            List<NFPLineAngle> newNFPLine = getSignGuiJiPointAo(stockLine, j, plateLine);
                            float allAngle = getAoAngleRange(stockLine, j);
                            for (int k = 0;k < newNFPLine.Count;k++)
                            {
                                if (stockLine[j].Angle + allAngle > 360)
                                {
                                    if ((newNFPLine[k].Angle >= stockLine[j].Angle && newNFPLine[k].Angle < 360)
                                        || (newNFPLine[k].Angle >= 0 && newNFPLine[k].Angle <= stockLine[j].Angle + allAngle - 360))
                                    {
                                        PointF getSignAo = stockLine[j].StartPoint;
                                        float moveXAo = getSignAo.X - newNFPLine[k].StartPoint.X;
                                        float moveYAo = getSignAo.Y - newNFPLine[k].StartPoint.Y;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            moveXAo,
                                            moveYAo);
                                        //CADInterface.DrawShap();
                                        PointF lineSpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));

                                        moveXAo = getSignAo.X - newNFPLine[k].EndPoint.X;
                                        moveYAo = getSignAo.Y - newNFPLine[k].EndPoint.Y;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            moveXAo,
                                            moveYAo);
                                        PointF lineEpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));

                                        listGuiJi.Add(new Line(lineSpAo, lineEpAo));

                                        if (count == 1)
                                        {
                                            //CADInterface.currentShapes.Add(new Line(lineSpAo, lineEpAo));
                                        }
                                    }
                                }
                                else
                                {
                                    if (newNFPLine[k].Angle >= stockLine[j].Angle && newNFPLine[k].Angle <= stockLine[j].Angle + allAngle)
                                    {
                                        PointF getSignAo = stockLine[j].StartPoint;
                                        float moveXAo = getSignAo.X - newNFPLine[k].StartPoint.X;
                                        float moveYAo = getSignAo.Y - newNFPLine[k].StartPoint.Y;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            moveXAo,
                                            moveYAo);
                                        //CADInterface.DrawShap();
                                        PointF lineSpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));

                                        moveXAo = getSignAo.X - newNFPLine[k].EndPoint.X;
                                        moveYAo = getSignAo.Y - newNFPLine[k].EndPoint.Y;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            moveXAo,
                                            moveYAo);
                                        PointF lineEpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));

                                        listGuiJi.Add(new Line(lineSpAo, lineEpAo));

                                        if (count == 1)
                                        {
                                            //CADInterface.currentShapes.Add(new Line(lineSpAo, lineEpAo));
                                        }
                                    }
                                }
                                
                            }
                            #endregion

                            #region 处理完凹点后处理凹点所在的线
                            PointF getSign = getSignGuiJiPointTu(stockLine[j], newListStockPoint, newListPlatePoint);
                            //移动到初始状态，起始点，获取重心坐标
                            float moveX = newListStockPoint[stockLine[j].StartInt].X - getSign.X;
                            float moveY = newListStockPoint[stockLine[j].StartInt].Y - getSign.Y;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                moveX,
                                moveY);
                            //CADInterface.DrawShap();
                            PointF lineSp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));

                            moveX = newListStockPoint[stockLine[j].EndInt].X - getSign.X;
                            moveY = newListStockPoint[stockLine[j].EndInt].Y - getSign.Y;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                               moveX,
                               moveY);
                            PointF lineEp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));

                            listGuiJi.Add(new Line(lineSp, lineEp));
                            if (count == 1)
                            {
                                //CADInterface.currentShapes.Add(new Line(lineSp, lineEp));
                            }
                            #endregion
                        }  
                    }
                    //listGene[signGene] = new RotateOper().RotatePlate(listGene[signGene], listGene[signGene].RotateCenter, (angleTest == -9999 ? -5 * i + 360: -angleTest + 360));
                    listGene[signGene] = new RotateOper().RotatePlate(listGene[signGene], SPointSign, (angleTest == -9999 ? -5 * i + 360: -angleTest + 360));
                    listGene[signGene].OutModel.ListPoint.Clear();
                    listGene[signGene].OutModel.ListPoint.AddRange(signPointList);
                    Trajectory tj = new Trajectory();
                    tj.Angle = (angleTest == -9999 ? 5 * i : angleTest);
                    tj.ListPoint.AddRange(getGuiJiDian(listGuiJi));
                    tj.Plate = listGene[signGene];
                    if (tj.ListPoint.Count > 0)
                    {
                        listTrajectory.Add(tj);
                    }
                    //CADInterface.DrawShap();
                    //for (int j = 0; j < listGuiJi.Count; j++)
                    //{
                    //    CADInterface.currentShapes.Add(listGuiJi[j]);
                    //}
                    #endregion
                }

                #region 筛选轨迹旋转最左最下的临界轨迹点
                if (listTrajectory.Count > 0)
                {
                    pyfa = getLeftDownPlate(listTrajectory, pyfa);
                    //pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate = new RotateOper().RotatePlate(
                    //    pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate,
                    //    pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter,
                    //    pyfa.ListPlate[pyfa.ListPlate.Count - 1].Angle);
                    pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate = new RotateOper().RotatePlate(
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate,
                        SPointSign,
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Angle);
                    //MessageBox.Show("" + pyfa.ListPlate[pyfa.ListPlate.Count - 1].Angle + "\n" +
                    //    (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.X - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.X)
                    //    + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.Y - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.Y)
                    //    + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.X)
                    //    + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.Y)
                    //    + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.X)
                    //    + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.Y)
                    //    + "\n" + SPointSign.X
                    //    + "\n" + SPointSign.Y);
                    pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate = new MoveOper().MovePlate(
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate,
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.X - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.X,
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.Y - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.Y);
                    //PointF sign = pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint[0];
                    //for (int i = 1; i< pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count;i++)
                    //{
                    //    sign = sign.X > pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint[i].X ? pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint[i] : sign;
                    //}
                    //MessageBox.Show(sign+"");
                    count = count + 1;
                    //CADInterface.DrawShap();
                    //System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    signFirstGene = signGene + 1;
                    continue;
                }
                #endregion

                #region 重新生成原材料碎片
                List<BaseModel> newListModel = rebuildStockModelList(stock.ListModel[sigModel],  pyfa);
                stock.ListModel[sigModel].ListPoint.Clear();
                stock.ListModel[sigModel].ListShape.Clear();
                stock.ListModel.RemoveAt(sigModel);
                stock.ListModel.AddRange(newListModel);
                #endregion

                //string ssssss = "";

                //CADInterface.DrawShap();

                BaseDelete bDelete = new BaseDelete();
                for (int i = 0; i < listDeleteId.Count; i++)
                {
                    bDelete.detele(listDeleteId[i]);
                }
                listDeleteId.Clear();

                //CADInterface.DrawShap();

                //if (count == 1)
                //{
                //    bn = false;
                //    break;
                //}

                for (int k = 0; k < stock.ListModel[stock.ListModel.Count -1].ListShape.Count; k++)
                {
                    Line line = new Line(stock.ListModel[stock.ListModel.Count - 1].ListShape[k].StartPoint,
                        stock.ListModel[stock.ListModel.Count - 1].ListShape[k].EndPoint);
                    line.PenColor = Color.Red;
                    //line.ShapeID = CADInterface.globalID;
                    listDeleteId.Add(line.ShapeID);
                    //CADInterface.globalID = CADInterface.globalID + 1;
                    //CADInterface.currentShapes.Add(line);
                    //ssssss = ssssss + line.StartPoint + " :: " + line.EndPoint + "\n";
                }
                //CADInterface.DrawShap();
                //MessageBox.Show(ssssss);

                #region 对原材料碎片进行排序，从大到小，或从大到小
                stock.ListModel = sAndLGArea ? new ModelOper().sortModel(stock.ListModel, true) : new ModelOper().sortModel(stock.ListModel, false);
                #endregion

                signFirstGene = 0;          //重置起始基因点为0

                //bn = false;
            }

            return pyfa;
        }

        /// <summary>
        /// 返回点是否在线段上，线段头尾根据误差进行判断，误差小于0.1也算在线段上
        /// </summary>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool judgePointInRange(Line line,PointF point)
        {
            bool bn = false;
            float length = pointToPoint(line.StartPoint,line.EndPoint);
            float pointToSp = pointToPoint(point, line.StartPoint);
            float pointToEp = pointToPoint(point, line.EndPoint);
            if (pointToSp < 0.1 || pointToEp < 0.1)
            {
                bn = true;
            }
            else if (pointToSp < length && pointToEp < length)
            {
                bn = true;
            }
            return bn;
        }

        /// <summary>
        /// 根据给予的方案件号顺序，重新进行排样
        /// 原材料碎片的利用，较小的碎片先利用，利用不了的时候采用较大的碎片
        /// 最后还有剩余碎片的时候继续从基因序列中再次选择可用的基因
        /// </summary>
        /// <param name="lastFangAn"></param>
        /// <param name="listGene"></param>
        /// <returns></returns>
        public PaiYangFangAn createFangAn(PaiYangFangAn fangAn, List<PlateModel> listGene)
        {
            return fangAn;
        }

        /// <summary>
        /// 搜索原材料上的负顶点，负顶点为内凹点，内凹点为原材料钢板上的点到件号上直线找轨迹
        /// </summary>
        /// <returns>true,为凸点，false为凹点</returns>
        private bool judgePlusOrMinus(List<NFPLineAngle> listStock, int nfpLAInt)
        {
            bool bn = false;
            float signAngle = 0;         //起始角度的反向角度。用于判断左右的范围
            NFPLineAngle startLine = nfpLAInt == 0 ? listStock[listStock.Count - 1] : listStock[nfpLAInt - 1];
            signAngle = startLine.Angle - 180;

            if (signAngle < 0)
            {
                if (listStock[nfpLAInt].Angle >= 0 && listStock[nfpLAInt].Angle < startLine.Angle)
                {
                    bn = false;
                }
                else if (listStock[nfpLAInt].Angle  >= signAngle + 360 && listStock[nfpLAInt].Angle  < 360)
                {
                    bn = false;
                }
                else
                {
                    bn = true;
                }
            }
            else
            {
                if (listStock[nfpLAInt].Angle >= signAngle && listStock[nfpLAInt].Angle < startLine.Angle)
                {
                    bn = false;
                }
                else
                {
                    bn = true;
                }
            }
            return bn;
        }

        /// <summary>
        /// 当为凹点的时候，获取凹点线段取值范围
        /// </summary>
        /// <param name="listStock"></param>
        /// <param name="nfpLAInt"></param>
        /// <returns></returns>
        private float getAoAngleRange(List<NFPLineAngle> listStock, int nfpLAInt)
        {
            float allAngle = 0;
            NFPLineAngle startLine = nfpLAInt == 0 ? listStock[listStock.Count - 1] : listStock[nfpLAInt - 1];
            allAngle = startLine.Angle < listStock[nfpLAInt].Angle
                ? 360 - listStock[nfpLAInt].Angle + (startLine.Angle)
                : startLine.Angle - listStock[nfpLAInt].Angle;
            return allAngle;
        }

        /// <summary>
        /// 选择基因序列中第一个未使用过的基因序列，并且在当前方案中还未使用
        /// </summary>
        /// <param name="geneList"></param>
        /// <returns></returns>
        private int selectFirstGeneFalse(List<PlateModel> geneList, PaiYangFangAn fangAn, int start)
        {
            int sign = -99;
            for (int i = start; i < geneList.Count; i++)
            {
                if (!geneList[i].HadUsedGene && !judgeFangAnHave(geneList[i].InheritanceID, fangAn))
                {
                    sign = i;
                    break;
                }
            }
            return sign;
        }

        /// <summary>
        /// 判断在方案中是否已经存在该基因
        /// </summary>
        /// <param name="geneID"></param>
        /// <param name="fangAn"></param>
        /// <returns></returns>
        public bool judgeFangAnHave(int geneID, PaiYangFangAn fangAn)
        {
            bool bn = false;
            for (int i = 0; i < fangAn.ListPlate.Count; i++)
            {
                if (geneID == fangAn.ListPlate[i].Plate.InheritanceID)
                {
                    bn = true;
                    break;
                }
            }
            return bn;
        }

        /// <summary>
        /// 从当前位置开始往后找面积符合的基因序列
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="geneList"></param>
        /// <param name="fangAn"></param>
        /// <param name="stockDebris"></param>
        /// <returns></returns>
        private int selectFirstSortByArea(int sign, List<PlateModel> geneList, PaiYangFangAn fangAn, BaseModel stockDebris)
        {
            if (sign == geneList.Count - 1)
            {
                sign = -99;
            }
            else
            {
                for (int i = sign + 1; i < geneList.Count; i++)
                {
                    if (!geneList[i].HadUsedGene && !judgeFangAnHave(geneList[i].InheritanceID, fangAn)
                        && (stockDebris.Area > geneList[i].Area))
                    {
                        sign = i;
                        break;
                    }
                }
            }
            return sign;
        }

        /// <summary>
        /// 获取原材料碎片上的起始运行点，取最高边的最右边的点，用于最后的轨迹方程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private FirstStockPoint getFirstPoint(BaseModel model)
        {
            int sign = 0;
            FirstStockPoint fsp = new FirstStockPoint();
            PointF signPoint = model.ListPoint[sign];
            for (int i = 1; i < model.ListPoint.Count; i++)
            {
                if (signPoint.Y < model.ListPoint[i].Y)
                {
                    sign = i;
                    signPoint = model.ListPoint[sign];
                }
                else if (signPoint.Y == model.ListPoint[i].Y)
                {
                    if (signPoint.X > model.ListPoint[i].X)
                    {
                        sign = i;
                        signPoint = model.ListPoint[sign];
                    }
                }
            }
            if (sign == 0)
            {
                if (model.ListPoint[model.ListPoint.Count - 1].Y > model.ListPoint[sign + 1].Y)
                {
                    signPoint.X = (model.ListPoint[model.ListPoint.Count - 1].X + model.ListPoint[sign].X) / 2;
                    signPoint.Y = (model.ListPoint[model.ListPoint.Count - 1].Y + model.ListPoint[sign].Y) / 2;
                    fsp.StartInt = model.ListPoint.Count - 1;
                    fsp.EndInt = sign;
                }
                else
                {
                    signPoint.X = (model.ListPoint[sign + 1].X + model.ListPoint[sign].X) / 2;
                    signPoint.Y = (model.ListPoint[sign + 1].Y + model.ListPoint[sign].Y) / 2;
                    fsp.StartInt = sign;
                    fsp.EndInt = sign + 1;
                }
            }
            else if (sign == model.ListPoint.Count - 1)
            {
                if (model.ListPoint[sign - 1].Y > model.ListPoint[0].Y)
                {
                    signPoint.X = (model.ListPoint[sign - 1].X + model.ListPoint[sign].X) / 2;
                    signPoint.Y = (model.ListPoint[sign - 1].Y + model.ListPoint[sign].Y) / 2;
                    fsp.StartInt = sign - 1;
                    fsp.EndInt = sign;
                }
                else
                {
                    signPoint.X = (model.ListPoint[0].X + model.ListPoint[sign].X) / 2;
                    signPoint.Y = (model.ListPoint[0].Y + model.ListPoint[sign].Y) / 2;
                    fsp.StartInt = sign;
                    fsp.EndInt = 0;
                }
            }
            else
            {
                if (model.ListPoint[sign - 1].Y > model.ListPoint[sign + 1].Y)
                {
                    signPoint.X = (model.ListPoint[sign - 1].X + model.ListPoint[sign].X) / 2;
                    signPoint.Y = (model.ListPoint[sign - 1].Y + model.ListPoint[sign].Y) / 2;
                    fsp.StartInt = sign - 1;
                    fsp.EndInt = sign;
                }
                else
                {
                    signPoint.X = (model.ListPoint[sign + 1].X + model.ListPoint[sign].X) / 2;
                    signPoint.Y = (model.ListPoint[sign + 1].Y + model.ListPoint[sign].Y) / 2;
                    fsp.StartInt = sign;
                    fsp.EndInt = sign + 1;
                }
            }
            fsp.Point = signPoint;
            return fsp;
        }

        /// <summary>
        /// 获取件号的顶点坐标
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private int getPlateFirstTopPoint(BaseModel model)
        {
            int sign = 0;
            PointF signPoint = model.ListPoint[sign];
            for (int i = 1; i < model.ListPoint.Count; i++)
            {
                if (signPoint.Y < model.ListPoint[i].Y)
                {
                    sign = i;
                    signPoint = model.ListPoint[sign];
                }
                else if (signPoint.Y == model.ListPoint[i].Y)
                {
                    if (signPoint.X > model.ListPoint[i].X)
                    {
                        sign = i;
                        signPoint = model.ListPoint[sign];
                    }
                }
            }
            return sign;
        }

        /// <summary>
        /// 获取最左边最下边的位置
        /// </summary>
        /// <param name="listTrajectory"></param>
        /// <param name="paiYangFangAn"></param>
        /// <returns></returns>
        private PaiYangFangAn getLeftDownPlate(List<Trajectory> listTrajectory, PaiYangFangAn paiYangFangAn)
        {
            PointF sign = listTrajectory[0].ListPoint[0];
            PlatePosition pp = new PlatePosition();
            for (int i = 0; i < listTrajectory.Count; i++)
            {
                for (int j = 0; j < listTrajectory[i].ListPoint.Count; j++)
                {
                    if (sign.X > listTrajectory[i].ListPoint[j].X)
                    {
                        sign = listTrajectory[i].ListPoint[j];
                        pp.Angle = listTrajectory[i].Angle;
                        pp.Plate = listTrajectory[i].Plate;
                        pp.Position = listTrajectory[i].ListPoint[j];
                    }
                    else if (sign.X == listTrajectory[i].ListPoint[j].X)
                    {
                        if (sign.Y >= listTrajectory[i].ListPoint[j].Y)
                        {
                            sign = listTrajectory[i].ListPoint[j];
                            pp.Angle = listTrajectory[i].Angle;
                            pp.Plate = listTrajectory[i].Plate;
                            pp.Position = listTrajectory[i].ListPoint[j];
                        }
                    }
                }
            }
            pp.Position = sign;
            paiYangFangAn.ListPlate.Add(pp);
            return paiYangFangAn;
        }

        /// <summary>
        /// 排样后重置该原材料上的可用碎片
        /// </summary>
        private List<BaseModel> rebuildStockModelList(BaseModel model, PaiYangFangAn fangAn)
        {
            List<BaseModel> newModelList = new List<BaseModel>();

            FirstStockPoint startLoopPoint = getFirstPoint(model);        //作为初始位置
            List<PointF> newListStockPoint = getNewListPoint(model.ListPoint, startLoopPoint.StartInt);    //获取新的原材料点顺序，起始点所在的线的点置为起始点
            FirstStockPoint plateLoopPoint = getFirstPoint(fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel);        //作为初始位置
            List<PointF> newListPLatePoint = getNewListPoint(fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, plateLoopPoint.StartInt);    //获取新的原材料点顺序，起始点所在的线的点置为起始点
            List<StockPlatePointSign> listPlatePoint = new List<StockPlatePointSign>();
            listPlatePoint.AddRange(getStockPlatePointSign(newListPLatePoint));
            IntersectOper ito = new IntersectOper();
            bool findInsert = false;                  //是否找到切入点；
            bool findOut = false;                     //找到回归点
            int startStockInt = 0;                    //在原材料上的一个起点
            int startPlateInt = 0;                    //件号上的一个点
            int endStockInt = 0;
            int endPlateInt = 0;
            bool bn = true;
            while (bn)
            {
                List<PointF> newListPoint = new List<PointF>();           //新的点集列表

                #region 从初始位置获取到第一个接触点为止的点
                for (int i = startStockInt; i < newListStockPoint.Count && !findInsert; i++)
                {
                    Line lineOne = stockPlateLine(i, newListStockPoint, true);
                    //增加线段和直线中段交点的判断，用于边线的缺漏判断。点不在线上但是线与线相交。
                    for (int j = 0; !findInsert && j < newListPLatePoint.Count; j++)
                    {
                        #region 件号上的点在原材料线上的交点
                        Line lineTwo = stockPlateLine(j, newListPLatePoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                            && judgePointInRange(lineOne, lineTwo.EndPoint))
                        {
                            startPlateInt = j == newListPLatePoint.Count - 1 ?
                                0 : j + 1;
                            startStockInt = i;
                            findInsert = true;
                            break;
                        }
                        else if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                            && judgePointInRange(lineOne, lineTwo.StartPoint))
                        {
                            startPlateInt = j;
                            startStockInt = i;
                            findInsert = true;
                            break;
                        }
                        else
                        {
                            lineTwo = stockPlateLine(j, newListPLatePoint, false);
                            if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                                && !ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                                && judgePointInRange(lineOne, lineTwo.StartPoint))
                            {
                                startPlateInt = j;
                                startStockInt = i;
                                findInsert = true;
                                break;
                            }
                        }
                        #endregion

                        #region 原材料上的点到件号上的线的交点
                        lineTwo = stockPlateLine(j, newListPLatePoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                            && judgePointInRange(lineTwo, lineOne.EndPoint))
                        {
                            startPlateInt = j;
                            startStockInt = i == newListPLatePoint.Count - 1 ?
                                0 : i + 1;
                            findInsert = true;
                            break;
                        }
                        else if (ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                            && judgePointInRange(lineTwo, lineOne.StartPoint))
                        {
                            startPlateInt = j;
                            startStockInt = i;
                            findInsert = true;
                            break;
                        }
                        else
                        {
                            lineOne = stockPlateLine(i, newListStockPoint, false);
                            if (ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                                && !ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                                && judgePointInRange(lineTwo, lineOne.StartPoint))
                            {
                                startPlateInt = j;
                                startStockInt = i;
                                findInsert = true;
                            }
                            lineOne = stockPlateLine(i, newListStockPoint, true);
                        }

                        #endregion
                    }
                }
                #endregion

                #region 从第一个接触点开始到第二个接触点的位置,第一部分
                for (int i = startPlateInt; i >= 0 && !findOut; i--)
                {
                    Line lineOne = stockPlateLine(i, newListPLatePoint, false);
                    for (int j = startStockInt; j < newListStockPoint.Count && !findOut; j++)
                    {
                        #region 判断件号的点是否在材料的线上
                        Line lineTwo = stockPlateLine(j, newListStockPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                            && judgePointInRange(lineTwo, lineOne.EndPoint))
                        {
                            listPlatePoint[i].Used = true;
                            newListPoint.Add(lineOne.StartPoint);
                            endPlateInt = i == 0 ? newListPLatePoint.Count - 1 : i - 1;
                            endStockInt = j == newListStockPoint.Count - 1 ? 0 : j + 1;
                            findOut = true;
                        }
                        else if (ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                            && judgePointInRange(lineTwo, lineOne.StartPoint)
                            && i != startPlateInt)
                        {
                            listPlatePoint[i].Used = true;
                            endPlateInt = i;
                            endStockInt = j == newListStockPoint.Count - 1 ? 0 : j + 1;
                            findOut = true;
                        }
                        #endregion

                        if (findOut)
                        {
                            #region 循环，件号的点在材料的线上
                            if (pointToPoint(lineOne.EndPoint, newListStockPoint[endStockInt]) > 1)
                            {
                                listPlatePoint[endPlateInt].Used = true;
                                newListPoint.Add(lineOne.EndPoint);
                            }
                            else
                            {
                                listPlatePoint[endPlateInt].Used = true;
                                newListPoint.Add(lineOne.EndPoint);
                                endStockInt = endStockInt == newListStockPoint.Count - 1 ? 0 : endStockInt + 1;
                            }
                            if (endStockInt > startStockInt)
                            {
                                for (int k = endStockInt; k < newListStockPoint.Count; k++)
                                {
                                    newListPoint.Add(newListStockPoint[k]);
                                }
                                for (int k = 0; k <= startStockInt; k++)
                                {
                                    newListPoint.Add(newListStockPoint[k]);
                                }
                            }
                            else if (endStockInt <= startStockInt)
                            {
                                for (int k = endStockInt; k <= startStockInt; k++)
                                {
                                    newListPoint.Add(newListStockPoint[k]);
                                }
                            }
                            #endregion
                            break;
                        }
                        else
                        {
                            #region 判断材料点是否在件号的线上
                            if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                                && ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                                && judgePointInRange(lineOne, lineTwo.EndPoint))
                            {
                                endPlateInt = i;
                                endStockInt = j;
                                findOut = true;

                            }
                            else if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                                && ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                                && judgePointInRange(lineOne, lineTwo.StartPoint))
                            {
                                endPlateInt = i;
                                endStockInt = j - 1;
                                findOut = true;
                            }
                            #endregion

                            if (findOut)
                            {
                                #region 发现
                                if (endStockInt > startStockInt)
                                {
                                    for (int k = endStockInt; k < newListStockPoint.Count; k++)
                                    {
                                        newListPoint.Add(newListStockPoint[k]);
                                    }
                                    for (int k = 0; k <= startStockInt; k++)
                                    {
                                        newListPoint.Add(newListStockPoint[k]);
                                    }
                                }
                                else if (endStockInt <= startStockInt)
                                {
                                    for (int k = endStockInt; k <= startStockInt; k++)
                                    {
                                        newListPoint.Add(newListStockPoint[k]);
                                    }
                                }
                                #endregion
                                break;
                            }

                        }
                       
                    }
                    #region 本次循环没有找到出点的时候
                    if (!findOut)
                    {
                        listPlatePoint[i].Used = true;
                        newListPoint.Add(lineOne.StartPoint);
                    }
                    #endregion
                }
                #endregion

                #region 第二部分
                for (int i = newListPLatePoint.Count -1; i > startPlateInt && !findOut; i--)
                {
                    Line lineOne = stockPlateLine(i, newListPLatePoint, false);
                    for (int j = startStockInt; j < newListStockPoint.Count && !findOut; j++)
                    {
                        #region 判断件号的点是否在材料的线上
                        Line lineTwo = stockPlateLine(j, newListStockPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                            && judgePointInRange(lineTwo, lineOne.EndPoint))
                        {
                            listPlatePoint[i].Used = true;
                            newListPoint.Add(lineOne.StartPoint);
                            endPlateInt = i == 0 ? newListPLatePoint.Count - 1 : i - 1;
                            endStockInt = j == newListStockPoint.Count - 1 ? 0 : j + 1;
                            findOut = true;
                        }
                        else if (ito.judgePointInLine(lineOne, lineTwo, lineOne.StartPoint)
                            && !ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint)
                            && judgePointInRange(lineTwo, lineOne.StartPoint))
                        {
                            listPlatePoint[i].Used = true;
                            endPlateInt = i;
                            endStockInt = j == newListStockPoint.Count - 1 ? 0 : j + 1;
                            findOut = true;
                        }
                        #endregion

                        if (findOut)
                        {
                            #region 循环，件号的点在材料的线上
                            if (pointToPoint(lineOne.EndPoint, newListStockPoint[endStockInt]) > 1)
                            {
                                listPlatePoint[endPlateInt].Used = true;
                                newListPoint.Add(lineOne.EndPoint);
                            }
                            else
                            {
                                listPlatePoint[endPlateInt].Used = true;
                                newListPoint.Add(lineOne.EndPoint);
                                endStockInt = endStockInt == newListStockPoint.Count - 1 ? 0 : endStockInt + 1;
                            }
                            if (endStockInt > startStockInt)
                            {
                                for (int k = endStockInt; k < newListStockPoint.Count; k++)
                                {
                                    newListPoint.Add(newListStockPoint[k]);
                                }
                                for (int k = 0; k <= startStockInt; k++)
                                {
                                    newListPoint.Add(newListStockPoint[k]);
                                }
                            }
                            else if (endStockInt <= startStockInt)
                            {
                                for (int k = endStockInt; k <= startStockInt; k++)
                                {
                                    newListPoint.Add(newListStockPoint[k]);
                                }
                            }
                            #endregion
                            break;
                        }
                        else
                        {
                            #region 判断材料点是否在件号的线上
                            if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                                && ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                                && judgePointInRange(lineOne, lineTwo.EndPoint))
                            {
                                endPlateInt = i;
                                endStockInt = j;
                                findOut = true;

                            }
                            else if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint)
                                && ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint)
                                && judgePointInRange(lineOne, lineTwo.StartPoint))
                            {
                                endPlateInt = i;
                                endStockInt = j - 1;
                                findOut = true;
                            }
                            #endregion

                            if (findOut)
                            {
                                #region 发现
                                if (endStockInt > startStockInt)
                                {
                                    for (int k = endStockInt; k < newListStockPoint.Count; k++)
                                    {
                                        newListPoint.Add(newListStockPoint[k]);
                                    }
                                    for (int k = 0; k <= startStockInt; k++)
                                    {
                                        newListPoint.Add(newListStockPoint[k]);
                                    }
                                }
                                else if (endStockInt <= startStockInt)
                                {
                                    for (int k = endStockInt; k <= startStockInt; k++)
                                    {
                                        newListPoint.Add(newListStockPoint[k]);
                                    }
                                }
                                #endregion
                                break;
                            }

                        }

                    }
                    #region 本次循环没有找到出点的时候
                    if (!findOut)
                    {
                        listPlatePoint[i].Used = true;
                        newListPoint.Add(lineOne.StartPoint);
                    }
                    #endregion
                }
                #endregion

                #region 第三部，判断头尾，起始点
                //CADInterface.DrawShap();

                List<BaseShape> newListBaseShape = new AddOper().addLine(newListPoint, false);

                BaseModel newBaseModel = new AddOper().addModel(newListBaseShape);

                newModelList.Add(newBaseModel);
                //for (int i = 0; i < newModelList[newModelList.Count - 1].ListShape.Count; i++)
                //{
                //    Line line = (Line)newModelList[newModelList.Count - 1].ListShape[i];
                //    line.PenColor = Color.Red;
                //    CADInterface.currentShapes.Add(line);
                //}
                //newModelList[newModelList.Count - 1].Draw(CADInterface.bGrp.Graphics);
                #endregion

                #region 判断是不是继续进行循环
                bn = false;
                for (int i = 0; i < listPlatePoint.Count; i++)
                {
                    if (!listPlatePoint[i].Used)
                    {
                        bn = true;
                        break;
                    }
                }
                #endregion

                bn = false;
            }
            return newModelList;
        }

        private List<BaseModel> rebuildStockModelListC(BaseModel model, PaiYangFangAn fangAn)
        {
            List<BaseModel> newModelList = new List<BaseModel>();
            FirstStockPoint startLoopPoint = getFirstPoint(model);        //作为初始位置
            List<PointF> newListStockPoint = getNewListPoint(model.ListPoint, startLoopPoint.StartInt);    //获取新的原材料点顺序，起始点所在的线的点置为起始点
            List<StockPlatePointSign> listPlatePoint = new List<StockPlatePointSign>();
            listPlatePoint.AddRange(getStockPlatePointSign(fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint));
            IntersectOper ito = new IntersectOper();
            bool findInsert = false;                  //是否找到切入点；
            bool findOut = false;                     //找到回归点
            int startStockInt = 0;                    //在原材料上的一个起点
            int startPlateInt = 0;                    //件号上的一个点
            int endStockInt = 0;
            int endPlateInt = 0;
            bool bn = true;
            while (bn)
            {
                List<PointF> newListPoint = new List<PointF>();           //新的点集列表

                #region 从初始位置获取到第一个接触点为止的点
                for (int i = startStockInt; i < newListStockPoint.Count && !findInsert; i++)
                {
                    Line lineOne = stockPlateLine(i, newListStockPoint, true);
                    //增加线段和直线中段交点的判断，用于边线的缺漏判断。点不在线上但是线与线相交。
                    for (int j = 0; !findInsert && j < fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count; j++)
                    {
                        Line lineTwo = stockPlateLine(j, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint) && !ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint))
                        {
                            //newListPoint.Add(lineOne.StartPoint);
                            startPlateInt = j == fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1 ?
                                0 : j + 1;
                            startStockInt = i;
                            findInsert = true;
                            break;
                        }
                        else if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint) && !ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint))
                        {
                            startPlateInt = j;
                            startStockInt = i;
                            findInsert = true;
                            break;
                        }
                        if (lineTwo.StartPoint.Y > 300)
                        {
                            MessageBox.Show("ads");
                        }
                    }
                    for (int j = 0; !findInsert && j < fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count; j++)
                    {
                        Line lineTwo = stockPlateLine(j, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint))
                        {
                            //newListPoint.Add(lineOne.StartPoint);
                            //newListPoint.Add(lineOne.EndPoint);
                            startPlateInt = j;
                            startStockInt = i == newListStockPoint.Count - 1 ? 0 : i + 1;
                            findInsert = true;
                            break;
                        }
                    }
                    if (!findInsert)
                    {
                        //newListPoint.Add(lineOne.StartPoint);
                    }
                }
                #endregion

                #region 从第一个接触点开始到第二个接触点的位置,第一部分
                for (int i = startPlateInt; i >= 0 && !findOut; i--)
                {
                    Line lineOne = stockPlateLine(i, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, false);
                    for (int j = startStockInt; j < newListStockPoint.Count; j++)
                    {
                        Line lineTwo = stockPlateLine(j, newListStockPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint) && pointToPoint(lineOne.EndPoint, lineTwo.EndPoint) > 1)
                        {
                            listPlatePoint[i].Used = true;
                            newListPoint.Add(lineOne.StartPoint);
                            endPlateInt = i == 0 ? fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1 : i - 1;
                            endStockInt = j == newListStockPoint.Count - 1 ? 0 : j + 1;
                            findOut = true;
                            break;
                        }
                    }

                    if (findOut)
                    {
                        #region 找到
                        if (pointToPoint(lineOne.EndPoint, newListStockPoint[endStockInt]) > 1)
                        {
                            listPlatePoint[endPlateInt].Used = true;
                            newListPoint.Add(lineOne.EndPoint);
                        }
                        else
                        {
                            listPlatePoint[endPlateInt].Used = true;
                            newListPoint.Add(lineOne.EndPoint);
                            endStockInt = endStockInt == newListStockPoint.Count - 1 ? 0 : endStockInt + 1;
                        }
                        if (endStockInt > startStockInt)
                        {
                            for (int j = endStockInt; j < newListStockPoint.Count; j++)
                            {
                                newListPoint.Add(newListStockPoint[j]);
                            }
                            for (int j = 0; j <= startStockInt; j++)
                            {
                                newListPoint.Add(newListStockPoint[j]);
                            }
                        }
                        else if (endStockInt <= startStockInt)
                        {
                            for (int j = endStockInt; j <= startStockInt; j++)
                            {
                                newListPoint.Add(newListStockPoint[j]);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        for (int j = startStockInt; j < newListStockPoint.Count && !findOut; j++)
                        {
                            Line lineTwo = stockPlateLine(j, newListStockPoint, true);
                            if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint) && pointToPoint(lineOne.EndPoint, lineTwo.EndPoint) > 1)
                            {
                                endPlateInt = i;
                                endStockInt = j;
                                findOut = true;
                                break;
                            }
                        }
                        if (findOut)
                        {
                            #region 发现
                            if (endStockInt > startStockInt)
                            {
                                for (int j = endStockInt; j < newListStockPoint.Count; j++)
                                {
                                    newListPoint.Add(newListStockPoint[j]);
                                }
                                for (int j = 0; j <= startStockInt; j++)
                                {
                                    newListPoint.Add(newListStockPoint[j]);
                                }
                            }
                            else if (endStockInt <= startStockInt)
                            {
                                for (int j = endStockInt; j <= startStockInt; j++)
                                {
                                    newListPoint.Add(newListStockPoint[j]);
                                }
                            }
                            #endregion
                        }
                    }
                    if (!findOut)
                    {
                        listPlatePoint[i].Used = true;
                        newListPoint.Add(lineOne.StartPoint);
                    }
                }
                #endregion

                #region 第二部分
                for (int i = fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1; i > startPlateInt && !findOut; i--)
                {
                    Line lineOne = stockPlateLine(i, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, false);
                    for (int j = startStockInt; j < newListStockPoint.Count; j++)
                    {
                        Line lineTwo = stockPlateLine(j, newListStockPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint) && pointToPoint(lineOne.EndPoint, lineTwo.EndPoint) > 1)
                        {
                            listPlatePoint[i].Used = true;
                            newListPoint.Add(lineOne.StartPoint);
                            endPlateInt = i == 0 ? fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1 : i - 1;
                            endStockInt = j == newListStockPoint.Count - 1 ? 0 : j + 1;
                            findOut = true;
                            break;
                        }
                    }
                    if (findOut)
                    {
                        #region 找到
                        if (pointToPoint(lineOne.EndPoint, newListStockPoint[endStockInt]) > 1)
                        {
                            listPlatePoint[endPlateInt].Used = true;
                            newListPoint.Add(lineOne.EndPoint);
                        }
                        else
                        {
                            listPlatePoint[endPlateInt].Used = true;
                            newListPoint.Add(lineOne.EndPoint);
                            endStockInt = endStockInt == newListStockPoint.Count - 1 ? 0 : endStockInt + 1;
                        }
                        if (endStockInt > startStockInt)
                        {
                            for (int j = endStockInt; j < newListStockPoint.Count; j++)
                            {
                                newListPoint.Add(newListStockPoint[j]);
                            }
                            for (int j = 0; j <= startStockInt; j++)
                            {
                                newListPoint.Add(newListStockPoint[j]);
                            }
                        }
                        else if (endStockInt <= startStockInt)
                        {
                            for (int j = endStockInt; j <= startStockInt; j++)
                            {
                                newListPoint.Add(newListStockPoint[j]);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        for (int j = startStockInt; j < newListStockPoint.Count && !findOut; j++)
                        {
                            Line lineTwo = stockPlateLine(j, newListStockPoint, true);
                            if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.StartPoint) && pointToPoint(lineOne.EndPoint, lineTwo.EndPoint) > 1)
                            {
                                endPlateInt = i;
                                endStockInt = j;
                                findOut = true;
                                break;
                            }
                        }
                        if (findOut)
                        {
                            #region 发现
                            if (endStockInt > startStockInt)
                            {
                                for (int j = endStockInt; j < newListStockPoint.Count; j++)
                                {
                                    newListPoint.Add(newListStockPoint[j]);
                                }
                                for (int j = 0; j <= startStockInt; j++)
                                {
                                    newListPoint.Add(newListStockPoint[j]);
                                }
                            }
                            else if (endStockInt <= startStockInt)
                            {
                                for (int j = endStockInt; j <= startStockInt; j++)
                                {
                                    newListPoint.Add(newListStockPoint[j]);
                                }
                            }
                            #endregion
                        }
                    }
                    if (!findOut)
                    {
                        listPlatePoint[i].Used = true;
                        newListPoint.Add(lineOne.StartPoint);
                    }
                }
                #endregion

                #region 第三部，判断头尾，起始点
                newModelList.Add(new AddOper().addModel(new AddOper().addLine(newListPoint, false)));
                for (int i = 0; i < newModelList[newModelList.Count - 1].ListShape.Count; i++)
                {
                    Line line = (Line)newModelList[newModelList.Count - 1].ListShape[i];
                    line.PenColor = Color.Red;
                    //CADInterface.currentShapes.Add(line);
                }
                //newModelList[newModelList.Count - 1].Draw(CADInterface.bGrp.Graphics);
                #endregion

                #region 判断是不是继续进行循环
                bn = false;
                for (int i = 0; i < listPlatePoint.Count; i++)
                {
                    if (!listPlatePoint[i].Used)
                    {
                        bn = true;
                        break;
                    }
                }
                #endregion

                bn = false;
            }
            return newModelList;
        }

        private List<BaseModel> rebuildStockModelListB(BaseModel model, int signModel, PaiYangFangAn fangAn)
        {
            List<BaseModel> newModelList = new List<BaseModel>();
            FirstStockPoint startLoopPoint = getFirstPoint(model);        //作为初始位置
            List<PointF> newListStockPoint = getNewListPoint(model.ListPoint, startLoopPoint.StartInt);    //获取新的原材料点顺序，起始点所在的线的点置为起始点
            List<StockPlatePointSign> listPlatePoint = new List<StockPlatePointSign>();
            listPlatePoint.AddRange(getStockPlatePointSign(fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint));
            IntersectOper ito = new IntersectOper();
            int startStockInt = 0;                       //初始位置设为 0 
            int startStockEndInt = -99;                       //初始位置设为 0 
            int startPlateInt = 0;                       //初始位置设为 0
            int startPlateEndInt = -99;                        //初始位置设为 0
            bool bn = true;
            while (bn)
            {
                #region 选择初始位置
                for (int i = startStockInt; i < newListStockPoint.Count; i++)
                {
                    Line lineOne = stockPlateLine(i, newListStockPoint, true);
                    for (int j = 0; j < listPlatePoint.Count; j++)
                    {
                        Line lineTwo = stockPlateLine(j, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint))
                        {
                            startStockInt = i;
                            startPlateInt = j == fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1 ?
                                0 : j;
                            break;
                        }
                    }
                }
                #endregion

                #region 反向搜索，两种情况原材料点和件号线段相交，
                for (int i = startStockInt; i >= 0; i--)
                {
                    Line lineOne = stockPlateLine(i, newListStockPoint, false);
                    for (int j = 0; j < listPlatePoint.Count; j++)
                    {
                        Line lineTwo = stockPlateLine(j, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, true);
                        if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint))
                        {
                            startStockEndInt = i;
                            startPlateEndInt = j == fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1 ?
                                0 : j;
                            if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint))
                            {
                                startStockEndInt = i == 0 ? newListStockPoint.Count - 1 : i - 1;
                                startPlateEndInt = j;
                            }
                            break;
                        }
                        else if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint))
                        {
                            startStockEndInt = i == 0 ? newListStockPoint.Count - 1 : i - 1;
                            startPlateEndInt = j;
                            break;
                        }
                    }
                }
                if (startStockEndInt == -99)
                {
                    for (int i = newListStockPoint.Count - 1; i > startStockInt; i--)
                    {
                        Line lineOne = stockPlateLine(i, newListStockPoint, false);
                        for (int j = 0; j < listPlatePoint.Count; j++)
                        {
                            Line lineTwo = stockPlateLine(j, fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint, true);
                            if (ito.judgePointInLine(lineOne, lineTwo, lineTwo.EndPoint))
                            {
                                startStockEndInt = i;
                                startPlateEndInt = j == fangAn.ListPlate[fangAn.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count - 1 ?
                                    0 : j;
                                if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint))
                                {
                                    startStockEndInt = i - 1;
                                    startPlateEndInt = j;
                                }
                                break;
                            }
                            else if (ito.judgePointInLine(lineOne, lineTwo, lineOne.EndPoint))
                            {
                                startStockEndInt = i - 1;
                                startPlateEndInt = j;
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region 判断是不是继续进行循环
                bn = false;
                for (int i = 0; i < listPlatePoint.Count; i++)
                {
                    if (!listPlatePoint[i].Used)
                    {
                        bn = true;
                        break;
                    }
                }
                #endregion

                bn = false;
            }
            return newModelList;
        }

        /// <summary>
        /// 用于点列表按照点的位置生成直线线段
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="listPoint"></param>
        /// <param name="shunXu">顺序或者，逆序，true为顺序</param>
        /// <returns></returns>
        private Line stockPlateLine(int sign ,List<PointF> listPoint,bool shunXu)
        {
            if (shunXu)
            {
                Line line = sign == listPoint.Count - 1 ?
                     new Line(listPoint[sign], listPoint[0])
                    : new Line(listPoint[sign], listPoint[sign + 1]);
                return line;
            }
            else
            {
                Line line = sign == 0 ?
                    new Line(listPoint[sign], listPoint[listPoint.Count -1])
                   : new Line(listPoint[sign], listPoint[sign - 1]);
                return line;
            }
        }

        /// <summary>
        /// 对需要进行碎片重构的点进行序列化
        /// </summary>
        /// <param name="listPoint"></param>
        /// <returns></returns>
        private List<StockPlatePointSign> getStockPlatePointSign(List<PointF> listPoint )
        {
            List<StockPlatePointSign> stockPlatePointSign = new List<StockPlatePointSign>();
            for (int i = 0;i <listPoint.Count;i++)
            {
                StockPlatePointSign spps = new StockPlatePointSign();
                spps.Sign = i;
                spps.Used = false;
                spps.Point = listPoint[i];
                stockPlatePointSign.Add(spps);
            }
            return stockPlatePointSign;
        }

        /// <summary>
        /// 进入临界循环前判断原材料和基因面积的和的大小关系一次，原材料大为true，原材料小为false
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="listPlate"></param>
        /// <returns></returns>
        private bool judgeStockAndListGeneArea(Stock stock, List<PlateModel> listPlate)
        {
            bool bn = false;
            float area = 0;
            for (int i = 0; i < listPlate.Count; i++)
            {
                if (!listPlate[i].HadUsedGene)
                {
                    area = area + listPlate[i].Area;
                }
            }
            bn = stock.ListModel[0].Area > area ? true : false;
            return bn;
        }

        /// <summary>
        ///  根据起始点重组点的顺序
        /// </summary>
        /// <param name="stockDebris"></param>
        /// <param name="startLoopPoint"></param>
        /// <returns></returns>
        private List<PointF> getNewListPoint(List<PointF> listPoint, int startInt)
        {
            List<PointF> newList = new List<PointF>();
            for (int i = startInt; i < listPoint.Count; i++)
            {
                newList.Add(listPoint[i]);
            }
            for (int i = 0; i < startInt; i++)
            {
                newList.Add(listPoint[i]);
            }
            return newList;
        }

        /// <summary>
        /// 起始点接触点，获取轨迹的初始点，全部为件号上的点绕原材料的线做刚体运行,凸边的情况
        /// </summary>
        /// <param name="nfpLie"></param>
        /// <param name="stockPoint"></param>
        /// <param name="listPoint"></param>
        /// <returns></returns>
        private PointF getSignGuiJiPointTu(NFPLineAngle nfpLie,List<PointF> stockPoint,List<PointF> listPoint)
        {
            RotateOper rotate = new RotateOper();

            PointF rotatePoint = stockPoint[nfpLie.StartInt];

            for (int i = 0; i < listPoint.Count; i++)
            {
                //listPoint[i] = rotate.RotatePoint(listPoint[i], stockPoint[nfpLie.StartInt],nfpLie.Angle);
                listPoint[i] = rotate.RotatePoint(listPoint[i], rotatePoint, nfpLie.Angle);
            }
            PointF signPoint = listPoint[0];
            int sign = 0;
            for (int i = 1; i < listPoint.Count; i++)
            {
                if (signPoint.Y > listPoint[i].Y)
                {
                    sign = i;
                    signPoint = listPoint[i];
                }
                else if (signPoint.Y == listPoint[i].Y)
                {
                    if (signPoint.X < listPoint[i].X)
                    {
                        sign = i;
                        signPoint = listPoint[i];
                    }
                }
            }
            for (int i = 0; i < listPoint.Count; i++)
            {
                //listPoint[i] = rotate.RotatePoint(listPoint[i], stockPoint[nfpLie.StartInt], -nfpLie.Angle);
                listPoint[i] = rotate.RotatePoint(listPoint[i], rotatePoint, -nfpLie.Angle);
            }
            return listPoint[sign];
        }

        /// <summary>
        /// 起始点接触点，获取轨迹的初始点，全部为材料上的点绕件号上的线做刚体运行,凹边的情况
        /// </summary>
        /// <param name="nfpLie"></param>
        /// <param name="stockPoint"></param>
        /// <param name="listPoint"></param>
        /// <returns></returns>
        private List<NFPLineAngle> getSignGuiJiPointAo(List<NFPLineAngle> listStockNFP, int stockInt, List<NFPLineAngle> listPlateNFP)
        {
            List<NFPLineAngle> listLine = new List<NFPLineAngle>();
            if (listStockNFP[stockInt].Angle >= 0 && listStockNFP[stockInt].Angle < 90)
            {
                listLine.AddRange(getMaxMinInt(listPlateNFP, false, false, true, false));
            }
            else if (listStockNFP[stockInt].Angle >= 90 && listStockNFP[stockInt].Angle < 180)
            {
                listLine.AddRange(getMaxMinInt(listPlateNFP, false, true, false, false));
            }
            else if (listStockNFP[stockInt].Angle >= 180 && listStockNFP[stockInt].Angle < 270)
            {
                listLine.AddRange(getMaxMinInt(listPlateNFP, true, false, false, false));
            }
            else if (listStockNFP[stockInt].Angle >= 270 && listStockNFP[stockInt].Angle < 360)
            {
                listLine.AddRange(getMaxMinInt(listPlateNFP, false, false, false, true));
            }
            return listLine;
        }

        /// <summary>
        /// 获取件号最上端的起点，和最下端的起点,返回之间的线段
        /// </summary>
        /// <param name="listPlateNFP"></param>
        private List<NFPLineAngle> getMaxMinInt(List<NFPLineAngle> listPlateNFP, bool left, bool up, bool right, bool down)
        {
            //string sss = "";
            //for (int i= 0; i< listPlateNFP.Count;i++)
            //{
            //    sss = sss + listPlateNFP[i].StartPoint + "\n" + listPlateNFP[i].EndPoint+ "\n";
            //}
            //MessageBox.Show(sss);
            int maxInt = 0;
            int minInt = 0;
            for (int i = 1; i < listPlateNFP.Count; i++)
            {
                if (up)
                {
                    #region 取上半部分
                    if (listPlateNFP[maxInt].EndPoint.X > listPlateNFP[i].EndPoint.X)
                    {
                        maxInt = i;
                    }
                    else if (Math.Abs(listPlateNFP[maxInt].EndPoint.X - listPlateNFP[i].EndPoint.X) < 0.1)
                    {
                        if (listPlateNFP[maxInt].EndPoint.Y < listPlateNFP[i].EndPoint.Y)
                        {
                            maxInt = i;
                        }
                    }

                    if (listPlateNFP[minInt].StartPoint.X < listPlateNFP[i].StartPoint.X)
                    {
                        minInt = i;
                    }
                    else if (Math.Abs(listPlateNFP[minInt].StartPoint.X - listPlateNFP[i].StartPoint.X) < 0.1)
                    {
                        if (listPlateNFP[minInt].StartPoint.Y < listPlateNFP[i].StartPoint.Y)
                        {
                            minInt = i;
                        }
                    }
                    #endregion
                }
                else if (left)
                {
                    #region 取左半部分
                    if (listPlateNFP[maxInt].EndPoint.Y > listPlateNFP[i].EndPoint.Y)
                    {
                        maxInt = i;
                    }
                    else if (Math.Abs(listPlateNFP[maxInt].EndPoint.Y - listPlateNFP[i].EndPoint.Y) < 0.1)
                    {
                        if ( listPlateNFP[maxInt].EndPoint.X > listPlateNFP[i].EndPoint.X)
                        {
                            maxInt = i;
                        }
                    }

                    if (listPlateNFP[minInt].StartPoint.Y < listPlateNFP[i].StartPoint.Y)
                    {
                        minInt = i;
                    }
                    else if (Math.Abs(listPlateNFP[minInt].StartPoint.Y - listPlateNFP[i].StartPoint.Y ) <0.1)
                    {
                        if (listPlateNFP[minInt].StartPoint.X > listPlateNFP[i].StartPoint.X)
                        {
                            minInt = i;
                        }
                    }
                    #endregion
                }
                else if (down)
                {
                    #region 取下半部分 取值不对
                    if (listPlateNFP[maxInt].EndPoint.X < listPlateNFP[i].EndPoint.X)
                    {
                        maxInt = i;
                    }
                    else if (Math.Abs( listPlateNFP[maxInt].EndPoint.X - listPlateNFP[i].EndPoint.X )< 0.1)
                    {
                        if (listPlateNFP[maxInt].EndPoint.Y > listPlateNFP[i].EndPoint.Y)
                        {
                            maxInt = i;
                        }
                    }

                    if (listPlateNFP[minInt].StartPoint.X > listPlateNFP[i].StartPoint.X )
                    {
                        minInt = i;
                    }
                    else if (Math.Abs(listPlateNFP[i].StartPoint.X - listPlateNFP[minInt].StartPoint.X )< 0.1)
                    {
                        if (listPlateNFP[minInt].StartPoint.Y > listPlateNFP[i].StartPoint.Y)
                        {
                            minInt = i;
                        }
                    }
                    #endregion
                }
                else if (right)
                {
                    #region 取右半部分
                    if (listPlateNFP[maxInt].EndPoint.Y < listPlateNFP[i].EndPoint.Y)
                    {
                        maxInt = i;
                    }
                    else if (Math.Abs(listPlateNFP[maxInt].EndPoint.Y - listPlateNFP[i].EndPoint.Y) < 0.1)
                    {
                        if (listPlateNFP[maxInt].EndPoint.X < listPlateNFP[i].EndPoint.X)
                        {
                            maxInt = i;
                        }
                    }

                    if (listPlateNFP[minInt].StartPoint.Y > listPlateNFP[i].StartPoint.Y)
                    {
                        minInt = i;
                    }
                    else if (Math.Abs( listPlateNFP[minInt].StartPoint.Y - listPlateNFP[i].StartPoint.Y) < 0.1)
                    {
                        if (listPlateNFP[minInt].StartPoint.X < listPlateNFP[i].StartPoint.X)
                        {
                            minInt = i;
                        }
                    }
                    #endregion
                }
            }
            List<NFPLineAngle> newList = new List<NFPLineAngle>();
            if (maxInt >= minInt)
            {
                for (int i = minInt ; i <= maxInt; i++)
                {
                    newList.Add(listPlateNFP[i]);
                }
            }
            else
            {
                for (int i = minInt; i < listPlateNFP.Count; i++)
                {
                    newList.Add(listPlateNFP[i]);
                }
                for (int i = 0; i <= maxInt; i++)
                {
                    newList.Add(listPlateNFP[i]);
                }
            }

            return newList;
        }

        /// <summary>
        /// 选定放置位置
        /// </summary>
        /// <param name="listGuiJi"></param>
        /// <returns></returns>
        private List<PointF> getGuiJiDian(List<Line> listGuiJi)
        {
            List<PointF> newPoint = new List<PointF>();
            for (int i = 0; i < listGuiJi.Count; i++)
            {
                if (i == listGuiJi.Count - 1)
                {
                    newPoint.AddRange(new IntersectOper().getIntersert(listGuiJi[i], listGuiJi[0]));
                }
                else
                {
                    newPoint.AddRange(new IntersectOper().getIntersert(listGuiJi[i], listGuiJi[i + 1]));
                }
            }

            //选定放置位置
            if (newPoint.Count > 1)
            {
                for (int i = 1; i < newPoint.Count; i++)
                {
                    if (newPoint[0].X - newPoint[i].X > 1)
                    {
                        newPoint[0] = newPoint[i];
                    }
                    else if (Math.Abs(newPoint[0].X - newPoint[i].X) <= 1)
                    {
                        if (newPoint[0].Y > newPoint[i].Y)
                        {
                            newPoint[0] = newPoint[i];
                        }
                    }
                }
                newPoint.RemoveRange(1,newPoint.Count -1);
                }
            return newPoint;
        }

        private float pointToPoint(PointF pointOne, PointF pointTwo)
        {
            float distance = (float)Math.Sqrt(Math.Pow((pointOne.Y - pointTwo.Y), 2) + Math.Pow((pointOne.X - pointTwo.X), 2));
            return distance;
        }
    }
}
