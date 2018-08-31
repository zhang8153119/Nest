using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.DrawTools
{
    class LayerTool
    {
        private Dictionary<string, string> layerDic = new Dictionary<string, string>();
        private string layerName = "";
        private string colorIndex = "";

        public Dictionary<string, string> getLayer(List<string> listString)
        {
            for (int i = 0; i < listString.Count; i++)
            {
                bool drawPic = false;
                string s1 = listString[i].Trim();
                string s2 = listString[i = i + 1].Trim();
                if ("LAYER".Equals(s2) && "0".Equals(s1) && (i - 1) != 0)
                {
                    drawPic = true; //第二个layer开始前对上一个线段进行数据读取
                }
                else if ("2".Equals(s1)) { layerName = s2.Trim(); }
                else if ("62".Equals(s1)) { colorIndex = s2.Trim(); }

                if (i == listString.Count - 1)
                {
                    drawPic = true; //最后一个layer，循环结束前进行画图  
                }

                if (drawPic)
                {
                    layerDic.Add(layerName,colorIndex);
                    reSet();   //重置变量
                }
            }
            return layerDic;
        }
        private void reSet()
        {
            layerName = "";
            colorIndex = "";
        }
    }
}
