using myCad .CADInterfaceCtrl;
using myCad .DrawTools;
using myCad .Model;
using myCad .PaiYangSuanFa;
using myCad .Shape;
using myCad .ShapeOper;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .IO;
using System .Linq;
using System .Text;
using System .Threading .Tasks;
using System .Windows .Forms;

namespace myCad .DXFOper
{
      class DxfInputB : FileBaseOper
      {
            public List<PlateModel> mainmethod(CADInterface cadinterface1)
            {
                  List<PlateModel> listPlate = new List<PlateModel>();
                  List<string> listFileName = selectList("选择dxf文件", new string[] { "dxf" });

                  if (listFileName .Count > 0)
                  {
                        //DateTime beforDT = System.DateTime.Now;
                        for (int i = 0; i < listFileName .Count; i++)
                        {
                              listPlate .Add(analyticalPaper(listFileName[i], cadinterface1));
                        }
                        //DateTime afterDT = System.DateTime.Now;
                        //TimeSpan ts = afterDT.Subtract(beforDT);
                        //Console.WriteLine("时间：{0}", ts.TotalSeconds);

                  }
                  else
                  {
                        //MessageBox .Show("没有选择对应的文件");
                  }
                  return listPlate;
            }
            private PlateModel analyticalPaper(string url, CADInterface cadinterface)
            {
                  PlateModel plate = new PlateModel();
                  string[] urlStr = url .Split('\\');
                  string[] nameAndNum = urlStr[urlStr .Length - 1] .Split('-');
                  plate .PlateName = nameAndNum[0];
                  plate .PlateCode = nameAndNum[0];
                  plate .PlateCount = int .Parse(nameAndNum[1] .Substring(0, nameAndNum[1] .Length - 4));

                  #region 加载dxf文件，获取文件每行的内容
                  List<string> listDxfInfo = new List<string>();
                  StreamReader sr = new StreamReader(url, Encoding .Default);
                  String input;
                  while ((input = sr .ReadLine()) != "EOF")
                  {
                        listDxfInfo .Add(input);
                  }
                  listDxfInfo .Add("EOF");
                  sr .Close();
                  #endregion

                  #region 提取需要的信息内容，这里按图形和文字进行分类提取
                  List<string> listLine = new List<string>();               //直线类型
                  List<string> listCircle = new List<string>();             //圆类型
                  List<string> listArc = new List<string>();                //圆弧类型
                  List<string> listEllipse = new List<string>();            //椭圆类型
                  List<string> listText = new List<string>();               //单行文字类型
                  List<string> listMtext = new List<string>();              //多行文字类型
                  List<string> listLayer = new List<string>();              //图层信息
                  Dictionary<string, string> layerNandC = new Dictionary<string, string>();                    //图层颜色和信息                    

                  for (int i = 0; i < listDxfInfo .Count; i++)
                  {
                        string s1 = listDxfInfo[i] .Trim();
                        string s2 = listDxfInfo[i = i + 1] .Trim();

                        if ("LAYER" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listLayer .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                        else if ("LINE" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listLine .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                        else if ("CIRCLE" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listCircle .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                        else if ("ARC" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listArc .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                        else if ("ELLIPSE" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listEllipse .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                        else if ("TEXT" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listText .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                        else if ("MTEXT" .Equals(s2) && "0" .Equals(s1))
                        {
                              List<string> getList = createListByClass(i - 1, listDxfInfo);
                              listMtext .AddRange(getList);
                              i = i + getList .Count - 2;
                              getList .Clear();
                        }
                  }

                  listDxfInfo .Clear();
                  #endregion

                  #region 利用GDI+在自己的cad面板中画出提取出的内容
                  if (listLayer .Count > 0) { layerNandC = new LayerTool() .getLayer(listLayer); }
                  if (listLine .Count > 0) { plate .OutModel .ListShape .AddRange(new LineTool() .getLineByColor(listLine, layerNandC)); }
                  if (listCircle .Count > 0) { plate .OutModel .ListShape .AddRange(new CircleTool() .getLineByColor(listCircle, layerNandC)); }
                  if (listArc .Count > 0) { plate .OutModel .ListShape .AddRange(new ArcTool() .getLineByColor(listArc, layerNandC)); }
                  //if (listText.Count > 0) { new TextTool().createText(listText); }
                  if (listEllipse .Count > 0) { plate .OutModel .ListShape .AddRange(new EllipseTool() .getLineByColor(listEllipse, layerNandC)); }
                  #endregion

                  listEllipse .Clear();
                  listText .Clear();
                  listArc .Clear();
                  listCircle .Clear();
                  listLine .Clear();
                  listLayer .Clear();
                  layerNandC .Clear();

                  ModelOper modelOper = new ModelOper();

                  plate = modelOper .baseShapeSort(plate);
                  for (int k = 0; k < plate .InnerModel .Count; k++)
                  {
                        plate .InnerModel[k] = modelOper .reSetPointShunXu(plate .InnerModel[k], false);
                  }

                  //for (int i = 0; i < plate.InnerModel[0].ListShape.Count; i++)
                  //{
                  //    Console.WriteLine("{0},{1},{2}",
                  //       plate.InnerModel[0].ListShape[i].ShapeClass, plate.InnerModel[0].ListShape[i].StartPoint, plate.InnerModel[0].ListShape[i].EndPoint);
                  //}

                  cadinterface .currentPlates .Add(plate .OutModel);          //这里不存在外围面域，下面的判断之后才选定外围面域
                  cadinterface .currentPlates .AddRange(plate .InnerModel);
                  cadinterface .DrawShap();
                  plate = modelOper .reSetOutInner(plate);
                  plate = modelOper .juedeIsArc(plate);
                  plate = new CalculateArea() .getPlateArea(plate);
                  plate = new CalculateArea() .getPlateBound(plate);
                  plate = new CreateCenterPoint() .getRotateCenter(plate);
                  plate = new CreateCenterPoint() .getPowCenter(plate);

                  //DateTime afterDT = System.DateTime.Now;
                  //TimeSpan ts = afterDT.Subtract(beforDT);
                  //Console.WriteLine("件号：{0},时间：{1}",
                  //        plate.PlateName, ts.TotalSeconds);
                  cadinterface .currentPlates .Clear();
                  cadinterface .currentShapes .Clear();
                  cadinterface .DrawShap();
                  return plate;

                  //DateTime afterDT = System.DateTime.Now;
                  //TimeSpan ts = afterDT.Subtract(beforDT);
                  //MessageBox.Show("DateTime总共花费:" + ts.TotalMilliseconds);
            }

            /// <summary>
            /// 返回一个list，用于提取选中类别中的数据内容
            /// </summary>
            /// <param name="startInt">读取开始的点</param>
            /// <param name="listOld">获取的dxf的每行内容</param>
            /// <returns></returns>
            private List<string> createListByClass(int startInt, List<string> listOld)
            {
                  List<string> newList = new List<string>();
                  for (int i = startInt; i < listOld .Count; i++)
                  {
                        string s1 = listOld[i] .Trim();
                        string s2 = listOld[i = i + 1] .Trim();
                        if (!("0" .Equals(s1) && i != startInt + 1))
                        {
                              newList .Add(s1);
                              newList .Add(s2);
                        }
                        else
                        {
                              break;
                        }
                  }
                  return newList;
            }



            public BaseModel reSetSpEpOrderB(BaseModel model)
            {
                  //int sign = 0;  //调整参照点，选取圆弧和椭圆弧为参照点，无则以第一点开始为参照点
                  //for (int i = 0; i < model.ListShape.Count; i++)
                  //{
                  //    if ("Arc".Equals(model.ListShape[i].ShapeClass) || "Ellipse".Equals(model.ListShape[i].ShapeClass))
                  //    {
                  //        sign = i;
                  //        break;
                  //        //Console.WriteLine("{0},{1},{2}",
                  //        //model.ListShape[i].ShapeClass, model.ListShape[i].StartPoint, model.ListShape[i].EndPoint);
                  //    }
                  //}
                  //PointF sp = model.ListShape[sign].StartPoint;
                  //PointF ep = model.ListShape[sign].EndPoint;
                  //#region 标记处之前的线段
                  //for (int i = sign - 1; i >= 0; i--)
                  //{
                  //    if ("Line".Equals(model.ListShape[i].ShapeClass))
                  //    {
                  //        if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1)
                  //            {
                  //                PointF sPoint = model.ListShape[i].StartPoint;
                  //                model.ListShape[i].StartPoint = model.ListShape[i].EndPoint;
                  //                model.ListShape[i].EndPoint = sPoint;
                  //                sp = model.ListShape[i].StartPoint;
                  //            }
                  //            else
                  //            {
                  //                sp = model.ListShape[i].StartPoint;
                  //            }
                  //        }
                  //        else if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1)
                  //            {
                  //                ep = sp;
                  //                sp = model.ListShape[i].StartPoint;
                  //            }
                  //            else
                  //            {
                  //                PointF sPoint = model.ListShape[i].StartPoint;
                  //                model.ListShape[i].StartPoint = model.ListShape[i].EndPoint;
                  //                model.ListShape[i].EndPoint = sPoint;
                  //                ep = sp;
                  //                sp = model.ListShape[i].StartPoint;
                  //            }
                  //        }
                  //    }
                  //    else
                  //    {
                  //        if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1)
                  //            {
                  //                sp = model.ListShape[i].EndPoint;
                  //            }
                  //            else
                  //            {
                  //                sp = model.ListShape[i].StartPoint;
                  //            }
                  //        }
                  //        else if (pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                  //            {

                  //            }
                  //            else
                  //            {

                  //            }
                  //        }
                  //    }
                  //}
                  //#endregion

                  //#region 标记处之后的线段
                  //for (int i = sign + 1; i < model.ListShape.Count; i++)
                  //{
                  //    if ("Line".Equals(model.ListShape[i].ShapeClass))
                  //    {
                  //        if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1)
                  //            {
                  //                PointF ePoint = model.ListShape[i].StartPoint;
                  //                model.ListShape[i].StartPoint = model.ListShape[i].EndPoint;
                  //                model.ListShape[i].EndPoint = ePoint;
                  //                ep = model.ListShape[i].EndPoint;
                  //            }
                  //            else
                  //            {
                  //                ep = model.ListShape[i].EndPoint;
                  //            }
                  //        }
                  //        else if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1)
                  //            {
                  //                sp = ep;
                  //                ep = model.ListShape[i].EndPoint;
                  //            }
                  //            else
                  //            {
                  //                PointF ePoint = model.ListShape[i].StartPoint;
                  //                model.ListShape[i].StartPoint = model.ListShape[i].EndPoint;
                  //                model.ListShape[i].EndPoint = ePoint;
                  //                sp = ep;
                  //                ep = model.ListShape[i].EndPoint;
                  //            }
                  //        }
                  //    }
                  //    else
                  //    {
                  //        if (pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(ep, model.ListShape[i].EndPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(ep, model.ListShape[i].StartPoint) < 0.1)
                  //            {
                  //                ep = model.ListShape[i].EndPoint;
                  //            }
                  //            else
                  //            {
                  //                ep = model.ListShape[i].StartPoint;
                  //            }
                  //        }
                  //        else if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1 || pointToPoint(sp, model.ListShape[i].EndPoint) < 0.1)
                  //        {
                  //            if (pointToPoint(sp, model.ListShape[i].StartPoint) < 0.1)
                  //            {

                  //            }
                  //            else
                  //            {

                  //            }
                  //        }
                  //    }
                  //}
                  //#endregion

                  return model;
            }




      }
}
