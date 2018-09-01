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
    class CircleTool : BaseTool
    {
        private float centerPX = 0;
        private float centerPY = 0;
        private float radius = 0;
        private string layerName = "";
        private string dxfColor = "";
        private Color color = Color.White;

        public void createCircle(List<string> listString)
        {
            for (int i = 0; i < listString.Count; i++)
            {
                bool drawPic = false;
                string s1 = listString[i].Trim();
                string s2 = listString[i = i + 1].Trim();
                if ("CIRCLE".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个圆开始前对上一个线段进行画图    
                }
                else if ("10".Equals(s1)) { centerPX = float.Parse(s2); }
                else if ("20".Equals(s1)) { centerPY = float.Parse(s2); }
                else if ("40".Equals(s1)) { radius = float.Parse(s2); }
                else if ("62".Equals(s1)) { color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = true;  //最后一个圆，循环结束前进行画图 
                }

                if (drawPic)
                {
                    
                    Circle nCircle = new Circle(new PointF(centerPX, centerPY), radius,new PointF(centerPX + radius,centerPY),new PointF(centerPX + radius, centerPY));
                    nCircle.PenColor = color;
                    //nCircle.ShapeID = CADInterface.globalID;
                    //CADInterface.globalID = CADInterface.globalID + 1;
                    //CADInterface.currentShapes.Add(nCircle);
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
                if ("CIRCLE".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个圆开始前对上一个线段进行画图    
                }
                else if ("8".Equals(s1)) { layerName = s2; }
                else if ("10".Equals(s1)) { centerPX = float.Parse(s2); }
                else if ("20".Equals(s1)) { centerPY = float.Parse(s2); }
                else if ("40".Equals(s1)) { radius = float.Parse(s2); }
                else if ("62".Equals(s1)) { dxfColor = s2; color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = true;  //最后一个圆，循环结束前进行画图 
                }

                if (drawPic)
                {
                    if ((!"".Equals(dxfColor) && "7".Equals(dxfColor)) || ("".Equals(dxfColor) && "7".Equals(colorDic[layerName])))
                    {
                        Circle nCircle = new Circle(new PointF(centerPX, centerPY), radius, new PointF(centerPX + radius, centerPY), new PointF(centerPX + radius, centerPY));
                        nCircle.PenColor = color;
                        //nCircle.ShapeID = CADInterface.globalID;
                        //CADInterface.globalID = CADInterface.globalID + 1;
                        //CADInterface.currentShapes.Add(nCircle);
                        listShape.Add(nCircle);
                        int count = 360/5;
                        float spa = 0;
                        for (int j = 1; j < count; j++)
                        {
                            spa = (float)(Math.PI / 180) * (j * 5 + 0);
                            nCircle.ListPoint.Add(new PointF((float)(centerPX + radius * Math.Cos(spa)), (float)(centerPY + radius * Math.Sin(spa))));
                        }
                    }
                    reSet();   //重置变量
                }
            }
            return listShape;
        }

        private void reSet()
        {
            centerPX = 0;
            centerPY = 0;
            radius = 0;
            layerName = "";
            dxfColor = "";
            color = Color.White;
        }
    }
}
