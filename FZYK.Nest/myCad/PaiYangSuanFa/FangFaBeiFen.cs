/**
 方法备份
 */

using myCad.CADInterfaceCtrl;
using myCad.Model;
using myCad.Shape;
using myCad.ShapeOper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.PaiYangSuanFa
{
    class FangFaBeiFen
    {
        public MoveOper moveOper = new MoveOper();

        /// <summary>
        /// NFP方案第一次生成
        /// </summary>
        /// <param name="listGene"></param>
        /// <param name="stock"></param>
        /// <param name="angleTest"></param>
        /// <returns></returns>
        public PaiYangFangAn createFirstFangAn(List<PlateModel> listGene, Stock stock, int angleTest)
        {
            PaiYangFangAn pyfa = new PaiYangFangAn();
            int signFirstGene = 0;          //判断第一次选择基因的起始点
            bool sAndLGArea = judgeStockAndListGeneArea(stock, listGene);     //判断原材料面积和钢板基因序列未使用的件号面积和
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

                            PointF lineSp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));
                            //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                            //   (SPointSign.X - listGene[signGene].RotateCenter.X),
                            //   (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                            moveX = newListStockPoint[stockLine[j].EndInt].X - getSign.X;
                            moveY = newListStockPoint[stockLine[j].EndInt].Y - getSign.Y;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                               moveX,
                               moveY);
                            PointF lineEp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));
                            //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                            //   (SPointSign.X - listGene[signGene].RotateCenter.X),
                            //   (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                            listGuiJi.Add(new Line(lineSp, lineEp));
                            if (count >= 0)
                            {
                                CADInterface.currentShapes.Add(new Line(lineSp, lineEp));
                            }
                            #endregion
                        }
                        else
                        {
                            #region 为凹点，即原材料上的点在件号上的线上移动
                            List<NFPLineAngle> newNFPLine = getSignGuiJiPointAo(stockLine, j, plateLine);
                            float allAngle = getAoAngleRange(stockLine, j);
                            for (int k = 0; k < newNFPLine.Count; k++)
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
                                        PointF lineSpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));
                                        //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                        //    (SPointSign.X - listGene[signGene].RotateCenter.X),
                                        //    (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                                        moveXAo = getSignAo.X - newNFPLine[k].EndPoint.X;
                                        moveYAo = getSignAo.Y - newNFPLine[k].EndPoint.Y;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            moveXAo,
                                            moveYAo);
                                        PointF lineEpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));
                                        //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                        //   (SPointSign.X - listGene[signGene].RotateCenter.X),
                                        //   (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                                        listGuiJi.Add(new Line(lineSpAo, lineEpAo));

                                        if (count >= 0)
                                        {
                                            CADInterface.currentShapes.Add(new Line(lineSpAo, lineEpAo));
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
                                        PointF lineSpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));
                                        //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                        //   (SPointSign.X - listGene[signGene].RotateCenter.X),
                                        //   (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                                        moveXAo = getSignAo.X - newNFPLine[k].EndPoint.X;
                                        moveYAo = getSignAo.Y - newNFPLine[k].EndPoint.Y;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            moveXAo,
                                            moveYAo);
                                        PointF lineEpAo = listGene[signGene].RotateCenter;
                                        listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                            -(moveXAo),
                                            -(moveYAo));
                                        //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                        //   (SPointSign.X - listGene[signGene].RotateCenter.X),
                                        //   (SPointSign.Y - listGene[signGene].RotateCenter.Y));

                                        listGuiJi.Add(new Line(lineSpAo, lineEpAo));

                                        if (count >= 0)
                                        {
                                            CADInterface.currentShapes.Add(new Line(lineSpAo, lineEpAo));
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
                            PointF lineSp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));
                            //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                            //    (SPointSign.X - listGene[signGene].RotateCenter.X),
                            //    (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                            moveX = newListStockPoint[stockLine[j].EndInt].X - getSign.X;
                            moveY = newListStockPoint[stockLine[j].EndInt].Y - getSign.Y;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                               moveX,
                               moveY);
                            PointF lineEp = listGene[signGene].RotateCenter;
                            listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                                -(moveX),
                                -(moveY));
                            //listGene[signGene] = moveOper.MovePlate(listGene[signGene],
                            //    (SPointSign.X - listGene[signGene].RotateCenter.X),
                            //    (SPointSign.Y - listGene[signGene].RotateCenter.Y));
                            listGuiJi.Add(new Line(lineSp, lineEp));
                            if (count >= 0)
                            {
                                CADInterface.currentShapes.Add(new Line(lineSp, lineEp));
                            }
                            #endregion
                        }
                    }
                    //listGene[signGene] = new RotateOper().RotatePlate(listGene[signGene], listGene[signGene].RotateCenter, (angleTest == -9999 ? -5 * i + 360: -angleTest + 360));
                    listGene[signGene] = new RotateOper().RotatePlate(listGene[signGene], SPointSign, (angleTest == -9999 ? -5 * i + 360 : -angleTest + 360));

                    Trajectory tj = new Trajectory();
                    tj.Angle = (angleTest == -9999 ? 5 * i : angleTest);
                    tj.ListPoint.Add(getGuiJiDian(listGuiJi));
                    tj.Plate = listGene[signGene];
                    listTrajectory.Add(tj);
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
                    MessageBox.Show("" + pyfa.ListPlate[pyfa.ListPlate.Count - 1].Angle + "\n" +
                        (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.X - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.X)
                        + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.Y - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.Y)
                        + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.X)
                        + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.Y)
                        + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.X)
                        + "\n" + (pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.Y)
                        + "\n" + SPointSign.X
                        + "\n" + SPointSign.Y);
                    pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate = new MoveOper().MovePlate(
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate,
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.X - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.X,
                        pyfa.ListPlate[pyfa.ListPlate.Count - 1].Position.Y - pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.RotateCenter.Y);
                    PointF sign = pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint[0];
                    for (int i = 1; i < pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint.Count; i++)
                    {
                        sign = sign.X > pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint[i].X ? pyfa.ListPlate[pyfa.ListPlate.Count - 1].Plate.OutModel.ListPoint[i] : sign;
                    }
                    MessageBox.Show(sign + "");
                    count = count + 1;
                    CADInterface.DrawShap();
                    //System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    signFirstGene = signGene + 1;
                    continue;
                }
                #endregion

                #region 重新生成原材料碎片
                List<BaseModel> newListModel = rebuildStockModelList(stock.ListModel[sigModel], pyfa);
                stock.ListModel.RemoveAt(sigModel);
                stock.ListModel.AddRange(newListModel);
                #endregion

                #region 对原材料碎片进行排序，从大到小，或从大到小
                stock.ListModel = sAndLGArea ? new ModelOper().sortModel(stock.ListModel, true) : new ModelOper().sortModel(stock.ListModel, false);
                #endregion

                signFirstGene = 0;          //重置起始基因点为0

                bn = false;
            }

            return pyfa;
        }

        private List<BaseModel> rebuildStockModelList(BaseModel baseModel, PaiYangFangAn pyfa)
        {
            throw new NotImplementedException();
        }

        private PaiYangFangAn getLeftDownPlate(List<Trajectory> listTrajectory, PaiYangFangAn pyfa)
        {
            throw new NotImplementedException();
        }

        private float getAoAngleRange(List<NFPLineAngle> stockLine, int j)
        {
            throw new NotImplementedException();
        }

        private List<NFPLineAngle> getSignGuiJiPointAo(List<NFPLineAngle> stockLine, int j, List<NFPLineAngle> plateLine)
        {
            throw new NotImplementedException();
        }

        private bool judgePlusOrMinus(List<NFPLineAngle> stockLine, int j)
        {
            throw new NotImplementedException();
        }

        private PointF getSignGuiJiPointTu(NFPLineAngle nFPLineAngle, List<PointF> newListStockPoint, List<PointF> newListPlatePoint)
        {
            throw new NotImplementedException();
        }

        private PointF getGuiJiDian(List<Line> listGuiJi)
        {
            throw new NotImplementedException();
        }

        private int getPlateFirstTopPoint(BaseModel outModel)
        {
            throw new NotImplementedException();
        }

        private List<PointF> getNewListPoint(List<PointF> listPoint, int startInt)
        {
            throw new NotImplementedException();
        }

        private FirstStockPoint getFirstPoint(BaseModel stockDebris)
        {
            throw new NotImplementedException();
        }

        private int selectFirstSortByArea(int signGene, List<PlateModel> listGene, PaiYangFangAn pyfa, BaseModel stockDebris)
        {
            throw new NotImplementedException();
        }

        private int selectFirstGeneFalse(List<PlateModel> listGene, PaiYangFangAn pyfa, int signFirstGene)
        {
            throw new NotImplementedException();
        }

        private bool judgeStockAndListGeneArea(Stock stock, List<PlateModel> listGene)
        {
            throw new NotImplementedException();
        }
    }
}
