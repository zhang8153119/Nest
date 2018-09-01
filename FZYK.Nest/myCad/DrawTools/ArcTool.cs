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
    class ArcTool : BaseTool
    {
        private float spAngle = 0;
        private float epAngle = 0;
        private float centerPX = 0;
        private float centerPY = 0;
        private float radius = 0;
        private string layerName = "";
        private string dxfColor = "";
        private Color color = Color.White;

        public void createArc(List<string> listString)
        {
            for (int i = 0; i < listString.Count; i++)
            {
                bool drawPic = false;
                string s1 = listString[i].Trim();
                string s2 = listString[i = i + 1].Trim();
                if ("ARC".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个圆弧开始前对上一个线段进行画图    
                }
                else if ("10".Equals(s1)) { centerPX = float.Parse(s2); }
                else if ("20".Equals(s1)) { centerPY = float.Parse(s2); }
                else if ("40".Equals(s1)) { radius = float.Parse(s2); }
                else if ("50".Equals(s1)) { spAngle = float.Parse(s2) < 0 ? float.Parse(s2) + 360 : float.Parse(s2); }
                else if ("51".Equals(s1)) { epAngle = float.Parse(s2) < 0 ? float.Parse(s2) + 360 : float.Parse(s2); }
                else if ("62".Equals(s1)) { color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = true; //最后一个圆弧，循环结束前进行画图  
                }

                if (drawPic)
                {
                    float spa = (float)(Math.PI / 180) * spAngle;
                    float epa = (float)(Math.PI / 180) * epAngle;
                    Arc nArc = new Arc(new PointF(centerPX, centerPY), radius, spAngle, epAngle, new PointF((float)(centerPX + radius * Math.Cos(spa)), (float)(centerPY + radius * Math.Sin(spa))), new PointF((float)(centerPX + radius * Math.Cos(epa)), (float)(centerPY + radius * Math.Sin(epa))));
                    nArc.PenColor = color;
                    //nArc.ShapeID = CADInterface.globalID;
                    //CADInterface.globalID = CADInterface.globalID + 1;
                    //CADInterface.currentShapes.Add(nArc);
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
                if ("ARC".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个圆弧开始前对上一个线段进行画图    
                }
                else if ("8".Equals(s1)) { layerName = s2; }
                else if ("10".Equals(s1)) { centerPX = float.Parse(s2); }
                else if ("20".Equals(s1)) { centerPY = float.Parse(s2); }
                else if ("40".Equals(s1)) { radius = float.Parse(s2); }
                else if ("50".Equals(s1)) { spAngle = float.Parse(s2) < 0 ? float.Parse(s2) + 360 : float.Parse(s2); }
                else if ("51".Equals(s1)) { epAngle = float.Parse(s2) < 0 ? float.Parse(s2) + 360 : float.Parse(s2); }
                else if ("62".Equals(s1)) { dxfColor = s2; color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = true; //最后一个圆弧，循环结束前进行画图  
                }

                if (drawPic)
                {
                    if ((!"".Equals(dxfColor) && "7".Equals(dxfColor)) || ("".Equals(dxfColor) && "7".Equals(colorDic[layerName])))
                    {
                        float spa = (float)(Math.PI / 180) * spAngle;
                        float epa = (float)(Math.PI / 180) * epAngle;
                        Arc nArc = new Arc(new PointF(centerPX, centerPY), radius, spAngle, epAngle,
                            new PointF((float)(centerPX + radius * Math.Cos(spa)), (float)(centerPY + radius * Math.Sin(spa))),
                            new PointF((float)(centerPX + radius * Math.Cos(epa)), (float)(centerPY + radius * Math.Sin(epa))));
                        nArc.PenColor = color;
                        //nArc.ShapeID = CADInterface.globalID;
                        //CADInterface.globalID = CADInterface.globalID + 1;
                        //CADInterface.currentShapes.Add(nArc);
                        listShape.Add(nArc);

                        #region 获取圆弧上的分割点，每5°获取一个值
                        int count = (int)((epAngle - spAngle < 0 ? epAngle - spAngle + 360 : epAngle - spAngle) / 5 );
                        for (int j = 1; j < count; j++)
                        {
                            spa = (float)(Math.PI / 180) * (j * 5 + spAngle);
                            nArc.ListPoint.Add(new PointF((float)(centerPX + radius * Math.Cos(spa)), (float)(centerPY + radius * Math.Sin(spa))));
                        }
                        #endregion
                    }
                    reSet();   //重置变量
                }
            }
            return listShape;
        }

        private void reSet()
        {
            spAngle = 0;
            epAngle = 0;
            centerPX = 0;
            centerPY = 0;
            radius = 0;
            layerName = "";
            dxfColor = "";
            color = Color.White;
        }
    }
}
