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
    class TextTool : BaseTool
    {
        private float angle = 0;
        private float localPointX = 0;
        private float localPointY = 0;
        private string text = "";
        private float textWidth = 0;
        private Color color = Color.White;

        public void createText(List<string> listString)
        {
            for (int i = 0; i < listString.Count; i++)
            {
                bool drawPic = false;
                string s1 = listString[i].Trim();
                string s2 = listString[i = i + 1].Trim();
                if ("TEXT".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = "".Equals(text) ? false : true; //第二个文字开始前对上一个线段进行画图    
                }
                else if ("10".Equals(s1)) { localPointX = float.Parse(s2); }
                else if ("20".Equals(s1)) { localPointY = float.Parse(s2); }
                else if ("40".Equals(s1)) { textWidth = float.Parse(s2); }
                else if ("1".Equals(s1)) { text = s2; }
                else if ("50".Equals(s1)) { angle = float.Parse(s2) < 0 ? float.Parse(s2) + 360 : float.Parse(s2); }
                else if ("62".Equals(s1)) { color = new BaseColor().judgeColor(s2); }
                if (i == listString.Count - 1)
                {
                    drawPic = "".Equals(text) ? false : true; //最后一个文字，循环结束前进行画图  
                }

                if (drawPic)
                {
                    Text nText = new Text(text, new PointF(localPointX, localPointY), angle, textWidth);
                    nText.PenColor = color;
                    //nText.ShapeID = CADInterface.globalID;
                    //CADInterface.globalID = CADInterface.globalID + 1;
                    //CADInterface.currentShapes.Add(nText);
                    reSet();      //重置变量
                }
            }
        }

        private void reSet()
        {
            angle = 0;
            localPointX = 0;
            localPointY = 0;
            text = "";
            textWidth = 0;
            color = Color.White;
        }
    }
}
