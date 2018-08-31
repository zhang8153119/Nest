/**
 * 导入dxf文件
 */
using myCad.DrawTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.DXFOper
{
    public class DxfInputA : FileBaseOper
    {
        public void mainmethod()
        {
            testAnalysis();
        }

        private void testAnalysis()
        {
            string url = select("选择dxf文件", new string[] { "dxf" });
            if (url != null)
            {
                analyticalPaper(url);
            }
            else
            {
                MessageBox.Show("没有选择对应的文件");
            }
        }

        private void analyticalPaper(string url)
        {

            //DateTime beforDT = System.DateTime.Now;

            #region 加载dxf文件，获取文件每行的内容
            List<string> listDxfInfo = new List<string>();
            StreamReader sr = new StreamReader(url, Encoding.Default);
            String input;
            while ((input = sr.ReadLine()) != "EOF")
            {
                listDxfInfo.Add(input);
            }
            listDxfInfo.Add("EOF");
            sr.Close();
            #endregion

            #region 提取需要的信息内容，这里按图形和文字进行分类提取
            List<string> listLine = new List<string>();               //直线类型
            List<string> listCircle = new List<string>();             //圆类型
            List<string> listArc = new List<string>();                //圆弧类型
            List<string> listEllipse = new List<string>();            //椭圆类型
            List<string> listText = new List<string>();               //单行文字类型
            List<string> listMtext = new List<string>();              //多行文字类型
            List<string> listLayer = new List<string>();              //图层信息

            for (int i = 0; i < listDxfInfo.Count; i++)
            {
                string s1 = listDxfInfo[i].Trim();
                string s2 = listDxfInfo[i = i + 1].Trim();

                if ("LINE".Equals(s2) && "0".Equals(s1))
                {
                    List<string> getList = createListByClass(i - 1, listDxfInfo);
                    listLine.AddRange(getList);
                    i = i + getList.Count - 2;
                    getList.Clear();
                }
                else if ("CIRCLE".Equals(s2) && "0".Equals(s1))
                {
                    List<string> getList = createListByClass(i - 1, listDxfInfo);
                    listCircle.AddRange(getList);
                    i = i + getList.Count - 2;
                    getList.Clear();
                }
                else if ("ARC".Equals(s2) && "0".Equals(s1))
                {
                    List<string> getList = createListByClass(i - 1, listDxfInfo);
                    listArc.AddRange(getList);
                    i = i + getList.Count - 2;
                    getList.Clear();
                }
                else if ("ELLIPSE".Equals(s2) && "0".Equals(s1))
                {
                    List<string> getList = createListByClass(i - 1, listDxfInfo);
                    listEllipse.AddRange(getList);
                    i = i + getList.Count - 2;
                    getList.Clear();
                }
                else if ("TEXT".Equals(s2) && "0".Equals(s1))
                {
                    List<string> getList = createListByClass(i - 1, listDxfInfo);
                    listText.AddRange(getList);
                    i = i + getList.Count - 2;
                    getList.Clear();
                }
                else if ("MTEXT".Equals(s2) && "0".Equals(s1))
                {
                    List<string> getList = createListByClass(i - 1, listDxfInfo);
                    listMtext.AddRange(getList);
                    i = i + getList.Count - 2;
                    getList.Clear();
                }
            }

            listDxfInfo.Clear();
            #endregion

            #region 利用GDI+在自己的cad面板中画出提取出的内容
            if (listLine.Count > 0) { new LineTool().createLine(listLine); }
            if (listCircle.Count > 0) { new CircleTool().createCircle(listCircle); }
            if (listArc.Count > 0) { new ArcTool().createArc(listArc); }
            if (listText.Count > 0) { new TextTool().createText(listText); }
            if (listEllipse.Count > 0) { new EllipseTool().createEllipse(listEllipse); }
            #endregion


            //DateTime afterDT = System.DateTime.Now;
            //TimeSpan ts = afterDT.Subtract(beforDT);
            //MessageBox.Show("DateTime总共花费:"+ ts.TotalMilliseconds);
        }

        /// <summary>
        /// 返回一个list，用于提取选中类别中的数据内容
        /// </summary>
        /// <param name="startInt">读取开始的点</param>
        /// <param name="listOld">获取的dxf的每行内容</param>
        /// <returns></returns>
        private List<string> createListByClass(int startInt,List<string> listOld)
        {
            List<string> newList = new List<string>();
            for(int i = startInt; i< listOld.Count;i ++)
            {
                string s1 = listOld[i].Trim();
                string s2 = listOld[i = i + 1].Trim();
                if (!("0".Equals(s1) && i != startInt +1 ))
                {
                    newList.Add(s1);
                    newList.Add(s2);
                }
                else
                {
                    break;
                }
            }
            return newList;
        }

    }
}
