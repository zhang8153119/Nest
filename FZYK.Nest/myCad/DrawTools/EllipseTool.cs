using myCad.CADInterfaceCtrl;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.DrawTools
{
    class EllipseTool : BaseTool
    {
        private double centerPX = 0;
        private double centerPY = 0;
        private double longRadiusX = 0;             //长轴相对于中心点的x偏移量
        private double longRadiusY = 0;             //长轴相对于中心点的y偏移量
        private float longAndShort = 0;             //长短轴比值
        //完整的椭圆该参数为0,用于计算端点值 ,不能直接用来算端点角度，直接算出来的角度和CAD显示的角度不同
        //spX = centerX + longRadius * cos(startParameter);
        //spY = centerY + shortRadius * sin(startParameter);
        private double startParameter = 0;          
        private double endParameter = 0;            //完整的椭圆该参数为2pi

        private string layerName = "";
        private string dxfColor = "";
        private Color color = Color.White;

        public void createEllipse(List<string> listString)
        {
            for (int i = 0; i < listString.Count; i++)
            {
                bool drawPic = false;
                string s1 = listString[i].Trim();
                string s2 = listString[i = i + 1].Trim();
                if ("ELLIPSE".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个圆开始前对上一个线段进行画图    
                }
                else if ("10".Equals(s1)) { centerPX = float.Parse(s2); }
                else if ("20".Equals(s1)) { centerPY = float.Parse(s2); }
                else if ("11".Equals(s1)) { longRadiusX = float.Parse(s2); }
                else if ("21".Equals(s1)) { longRadiusY = float.Parse(s2); }
                else if ("40".Equals(s1)) { longAndShort = float.Parse(s2); }
                else if ("41".Equals(s1)) { startParameter = float.Parse(s2); }
                else if ("42".Equals(s1)) { endParameter = float.Parse(s2); }
                else if ("62".Equals(s1)) { color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = true;  //最后一个圆，循环结束前进行画图 
                }

                if (drawPic)
                {
                    bool complete = Math.Abs(startParameter - 0) < 0.00001 && Math.Abs(endParameter - 2 * Math.PI) < 0.00001 ? true : false;
                    float longRadius = (float)Math.Sqrt(Math.Pow(longRadiusX, 2) + Math.Pow(longRadiusY, 2));
                    float shortRadius = longRadius * longAndShort;
                    //float angle = (float)((Math.Atan(longRadiusY / longRadiusX) < 0.000001 ? 0 : Math.Atan(longRadiusY / longRadiusX)) * (180 / Math.PI));
                    float angle = (float)(Math.Atan2(longRadiusY, longRadiusX) * (180 / Math.PI));
                    angle = angle < 0 ? angle + 360 : angle;

                    float startAngle = 0;
                    float endAngle = 0;
                    PointF newSp = new PointF();
                    PointF newEp = new PointF();
                    if (!complete)
                    {
                        startAngle = (float)(Math.Atan2(shortRadius * Math.Sin(startParameter), longRadius * Math.Cos(startParameter)) * (180 / Math.PI));
                        startAngle = startAngle < 0 ? startAngle + 360 : startAngle;
                        endAngle = (float)(Math.Atan2(shortRadius * Math.Sin(endParameter), longRadius * Math.Cos(endParameter)) * (180 / Math.PI));
                        endAngle = endAngle < 0 ? endAngle + 360 : endAngle;

                        float originToSp = (float)Math.Sqrt(Math.Pow(shortRadius * Math.Sin(startParameter), 2) + Math.Pow(longRadius * Math.Cos(startParameter), 2));
                        float originToEp = (float)Math.Sqrt(Math.Pow(shortRadius * Math.Sin(endParameter), 2) + Math.Pow(longRadius * Math.Cos(endParameter), 2));

                        float spAngleToRotate = angle + startAngle > 360 ? angle + startAngle - 360 : angle + startAngle;
                        float epAngleToRotate = angle + endAngle > 360 ? angle + endAngle - 360 : angle + endAngle;

                        newSp.X = (float)(centerPX + longRadius * Math.Cos(spAngleToRotate * (Math.PI / 180)));
                        newSp.Y = (float)(centerPY + longRadius * Math.Sin(spAngleToRotate * (Math.PI / 180)));

                        newEp.X = (float)(centerPX + longRadius * Math.Cos(epAngleToRotate * (Math.PI / 180)));
                        newEp.Y = (float)(centerPY + longRadius * Math.Sin(epAngleToRotate * (Math.PI / 180)));
                    }
                    else
                    {
                        newSp.X = (float)(centerPX + longRadius * Math.Cos(angle * (Math.PI / 180)));
                        newSp.Y = (float)(centerPY + longRadius * Math.Sin(angle * (Math.PI / 180)));

                        newEp.X = newSp.X;
                        newEp.Y = newSp.Y;
                    }
                    Ellipse nEllipse = new Ellipse(
                        new PointF((float)centerPX, (float)centerPY),
                        longRadius, shortRadius,
                        angle,
                        longAndShort,
                        startAngle, endAngle,
                        (float)longRadiusX, (float)longRadiusY,
                        (float)startParameter, (float)endParameter,
                        newSp,newEp,
                        complete);
                    nEllipse.PenColor = color;
                    nEllipse.ShapeID = CADInterface.globalID;
                    CADInterface.globalID = CADInterface.globalID + 1;
                    CADInterface.currentShapes.Add(nEllipse);
                    reSet();   //重置变量
                }
            }
        }

        public List<BaseShape> getLineByColor(List<string> listString, Dictionary<string, string> colorDic)
        {
            List<BaseShape> listShape = new List<BaseShape>();

            for (int i = 0; i < listString.Count; i++)
            {
                bool drawPic = false;
                string s1 = listString[i].Trim();
                string s2 = listString[i = i + 1].Trim();
                if ("ELLIPSE".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个圆开始前对上一个线段进行画图    
                }
                else if ("8".Equals(s1)) { layerName = s2; }
                else if ("10".Equals(s1)) { centerPX = float.Parse(s2); }
                else if ("20".Equals(s1)) { centerPY = float.Parse(s2); }
                else if ("11".Equals(s1)) { longRadiusX = float.Parse(s2); }
                else if ("21".Equals(s1)) { longRadiusY = float.Parse(s2); }
                else if ("40".Equals(s1)) { longAndShort = float.Parse(s2); }
                else if ("41".Equals(s1)) { startParameter = float.Parse(s2); }
                else if ("42".Equals(s1)) { endParameter = float.Parse(s2); }
                else if ("62".Equals(s1)) { dxfColor = s2; color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = true;  //最后一个圆，循环结束前进行画图 
                }

                if (drawPic)
                {
                    if ((!"".Equals(dxfColor) && "7".Equals(dxfColor)) || ("".Equals(dxfColor) && "7".Equals(colorDic[layerName])))
                    {
                        bool complete = Math.Abs(startParameter - 0) < 0.00001 && Math.Abs(endParameter - 2 * Math.PI) < 0.00001 ? true : false;
                        float longRadius = (float)Math.Sqrt(Math.Pow(longRadiusX, 2) + Math.Pow(longRadiusY, 2));
                        float shortRadius = longRadius * longAndShort;
                        float angle = (float)(Math.Atan2(longRadiusY, longRadiusX) * (180 / Math.PI));
                        angle = angle < 0 ? angle + 360 : angle;

                        float startAngle = 0;
                        float endAngle = 0;
                        PointF newSp = new PointF();
                        PointF newEp = new PointF();
                        List<PointF> listPoint = new List<PointF>();

                        if (!complete)
                        {
                            startAngle = (float)(Math.Atan2(shortRadius * Math.Sin(startParameter), longRadius * Math.Cos(startParameter)) * (180 / Math.PI));
                            startAngle = startAngle < 0 ? startAngle + 360 : startAngle;
                            endAngle = (float)(Math.Atan2(shortRadius * Math.Sin(endParameter), longRadius * Math.Cos(endParameter)) * (180 / Math.PI));
                            endAngle = endAngle < 0 ? endAngle + 360 : endAngle;
                            float originToSp = (float)Math.Sqrt(Math.Pow(shortRadius * Math.Sin(startParameter), 2) + Math.Pow(longRadius * Math.Cos(startParameter), 2));
                            float originToEp = (float)Math.Sqrt(Math.Pow(shortRadius * Math.Sin(endParameter), 2) + Math.Pow(longRadius * Math.Cos(endParameter), 2));

                            float spAngleToRotate = angle + startAngle > 360 ? angle + startAngle - 360 : angle + startAngle;
                            float epAngleToRotate = angle + endAngle > 360 ? angle + endAngle - 360 : angle + endAngle;

                            newSp.X = (float)(centerPX + originToSp * Math.Cos(spAngleToRotate * (Math.PI / 180)));
                            newSp.Y = (float)(centerPY + originToSp * Math.Sin(spAngleToRotate * (Math.PI / 180)));

                            newEp.X = (float)(centerPX + originToEp * Math.Cos(epAngleToRotate * (Math.PI / 180)));
                            newEp.Y = (float)(centerPY + originToEp * Math.Sin(epAngleToRotate * (Math.PI / 180)));

                            int count =(int)((endAngle - startAngle < 0 ? endAngle - startAngle +360 : endAngle - startAngle) / 5);
                            float spa = 0;
                            for (int j = 1; j < count; j++)
                            {
                                spa = (float)(Math.PI / 180) * (j * 5 + startAngle);
                                float ellX = (float)((longRadius * shortRadius * Math.Cos(spa)) / Math.Sqrt(Math.Pow(longRadius * Math.Sin(spa), 2) + Math.Pow(shortRadius * Math.Cos(spa), 2)));
                                float ellY = (float)(Math.Sqrt((Math.Pow(longRadius * shortRadius, 2) - Math.Pow(shortRadius * ellX, 2)) / (Math.Pow(longRadius, 2))));
                                PointF point = new PointF(ellX, ellY);
                                point = reSetPoint(point, (j * 5 + startAngle));

                                float originToFp = (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
                                float fpAngleToRotate = angle + (j * 5 + startAngle) > 360 ? angle + (j * 5 + startAngle) - 360 : angle + (j * 5 + startAngle);

                                point.X = (float)(centerPX + originToFp * Math.Cos(fpAngleToRotate * (Math.PI / 180)));
                                point.Y = (float)(centerPY + originToFp * Math.Sin(fpAngleToRotate * (Math.PI / 180)));

                                listPoint.Add(point);
                            }
                        }
                        else
                        {
                            newSp.X = (float)(centerPX + longRadius * Math.Cos(angle * (Math.PI / 180)));
                            newSp.Y = (float)(centerPY + longRadius * Math.Sin(angle * (Math.PI / 180)));

                            newEp.X = newSp.X;
                            newEp.Y = newSp.Y;
                            /**
                             假设椭圆在圆心并且没有旋转，求出所有的点，然后计算旋转过后的点，再根据移动距离计算最后的点坐标
                             */
                            int count = 360 / 5;
                            float spa = 0;
                            for (int j = 1; j < count; j++)
                            {
                                spa = (float)(Math.PI / 180) * (j * 5 + 0);
                                float ellX = (float)((longRadius * shortRadius * Math.Cos(spa)) / Math.Sqrt(Math.Pow(longRadius*Math.Sin(spa),2)+Math.Pow(shortRadius*Math.Cos(spa),2)));
                                float ellY = (float)(Math.Sqrt((Math.Pow(longRadius*shortRadius,2)-Math.Pow(shortRadius*ellX,2))/(Math.Pow(longRadius,2))));
                                PointF point = new PointF(ellX,ellY);
                                point = reSetPoint(point, (j * 5 + 0));

                                float originToSp = (float)Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2));
                                float spAngleToRotate = angle + (j * 5 + 0) > 360 ? angle + (j * 5 + 0) - 360 : angle + (j * 5 + 0);

                                point.X = (float)(centerPX + originToSp * Math.Cos(spAngleToRotate * (Math.PI / 180)));
                                point.Y = (float)(centerPY + originToSp * Math.Sin(spAngleToRotate * (Math.PI / 180)));

                                listPoint.Add(point);
                            }
                        }
                        Ellipse nEllipse = new Ellipse(
                            new PointF((float)centerPX, (float)centerPY),
                            longRadius, shortRadius,
                            angle,
                            longAndShort,
                            startAngle, endAngle,
                            (float)longRadiusX, (float)longRadiusY,
                            (float)startParameter, (float)endParameter,
                            newSp, newEp,
                            complete);
                        nEllipse.PenColor = color;
                        nEllipse.ShapeID = CADInterface.globalID;
                        nEllipse.ListPoint.AddRange(listPoint);
                        CADInterface.globalID = CADInterface.globalID + 1;
                        CADInterface.currentShapes.Add(nEllipse);
                        listShape.Add(nEllipse);
                    }
                    reSet();   //重置变量
                }
            }
            return listShape;
        }

        private PointF reSetPoint(PointF point,float signAngle)
        {
            if (signAngle >= 0 && signAngle <= 180)
            {
                point.Y = Math.Abs(point.Y);
            }
            else
            {
                point.Y = -Math.Abs(point.Y);
            }

            if (signAngle >= 90 && signAngle <= 270)
            {
                point.X = -Math.Abs(point.X);
            }
            else
            {
                point.X = Math.Abs(point.X);
            }

            return point;
        }

        private void reSet()
        {
            centerPX = 0;
            centerPY = 0;
            longRadiusX = 0;             //长轴相对于中心点的x偏移量
            longRadiusY = 0;             //长轴相对于中心点的y偏移量
            longAndShort = 0;            //长短轴比值
            startParameter = 0;          //完整的椭圆该参数为0
            endParameter = 0;            //完整的椭圆该参数为2pi
            layerName = "";
            dxfColor = "";
            color = Color.White;
        }
    }
}
