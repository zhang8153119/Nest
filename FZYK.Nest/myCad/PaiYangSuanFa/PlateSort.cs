using myCad.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.PaiYangSuanFa
{
    /// <summary>
    /// 钢板件号的排样分三类，面积排序，最小长方形宽度排序
    /// </summary>
    class PlateSort
    {
        /// <summary>
        /// 面积排序
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public List<PlateModel> getSortByArea(List<PlateModel> plateList)
        {
            for (int i = 0; i < plateList.Count - 1; i++)
            {
                for (int j = i + 1; j < plateList.Count; j++)
                {
                    if (plateList[i].Area < plateList[j].Area)
                    {
                        PlateModel plateModel = plateList[i];
                        plateList[i] = plateList[j];
                        plateList[j] = plateModel;
                    }
                }
            }
            for (int i = 0; i < plateList.Count; i++)
            {
                plateList[i].AreaPower = i;
            }
            return plateList;
        }

        /// <summary>
        /// 长度排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<PlateModel> getSortByWidth(List<PlateModel> plateList)
        {
            for (int i = 0; i < plateList.Count - 1; i++)
            {
                for (int j = i + 1; j < plateList.Count; j++)
                {
                    if (plateList[i].Bound.Width < plateList[j].Bound.Width)
                    {
                        PlateModel plateModel = plateList[i];
                        plateList[i] = plateList[j];
                        plateList[j] = plateModel;
                    }
                }
            }
            for (int i = 0; i < plateList.Count; i++)
            {
                plateList[i].WidthPower = i;
            }
            return plateList;
        }

        /// <summary>
        /// 根据宽度进行排序
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public List<PlateModel> getSortByHeight(List<PlateModel> plateList)
        {
            for (int i = 0; i < plateList.Count - 1; i++)
            {
                for (int j = i + 1; j < plateList.Count; j++)
                {
                    if (plateList[i].Bound.Height < plateList[j].Bound.Height)
                    {
                        PlateModel plateModel = plateList[i];
                        plateList[i] = plateList[j];
                        plateList[j] = plateModel;
                    }
                }
            }
            for (int i = 0; i < plateList.Count; i++)
            {
                plateList[i].HeightPower = i;
            }
            return plateList;
        }

        /// <summary>
        /// 根据宽度长度面积加权排序,权重相同，宽度一级，长度二级，面积三级
        /// </summary>
        /// <param name="plateList"></param>
        /// <returns></returns>
        public List<PlateModel> getSortByHWA(List<PlateModel> plateList)
        {
            for (int i = 0; i < plateList.Count - 1; i++)
            {
                for (int j = i + 1; j < plateList.Count; j++)
                {
                    //先判断权重
                    if ((plateList[i].HeightPower + plateList[i].WidthPower + plateList[i].AreaPower)
                        > (plateList[j].HeightPower + plateList[j].WidthPower + plateList[j].AreaPower))
                    {
                        PlateModel plateModel = plateList[i];
                        plateList[i] = plateList[j];
                        plateList[j] = plateModel;
                    }
                    else if ((plateList[i].HeightPower + plateList[i].WidthPower + plateList[i].AreaPower)
                       == (plateList[j].HeightPower + plateList[j].WidthPower + plateList[j].AreaPower))
                    {
                        //权重相同判断宽度
                        if (plateList[i].HeightPower > plateList[j].HeightPower)
                        {
                            PlateModel plateModel = plateList[i];
                            plateList[i] = plateList[j];
                            plateList[j] = plateModel;
                        }
                        else if (plateList[i].HeightPower == plateList[j].HeightPower)
                        {
                            //宽度相同判断长度
                            if (plateList[i].WidthPower > plateList[j].WidthPower)
                            {
                                PlateModel plateModel = plateList[i];
                                plateList[i] = plateList[j];
                                plateList[j] = plateModel;
                            }
                            else if (plateList[i].WidthPower == plateList[j].WidthPower)
                            {
                                //长度相同判断面积
                                if (plateList[i].AreaPower > plateList[j].AreaPower)
                                {
                                    PlateModel plateModel = plateList[i];
                                    plateList[i] = plateList[j];
                                    plateList[j] = plateModel;
                                }
                            }
                        }
                    }
                }
            }
            return plateList;
        }

        /// <summary>
        /// 排序，宽度一级，长度二级，面积三级
        /// </summary>
        /// <param name="plateList"></param>
        /// <returns></returns>
        public List<PlateModel> getSortByHToWToA(List<PlateModel> plateList)
        {
            for (int i = 0; i < plateList.Count - 1; i++)
            {
                for (int j = i + 1; j < plateList.Count; j++)
                {
                    if (plateList[i].Bound.Height < plateList[j].Bound.Height)
                    {
                        PlateModel plateModel = plateList[i];
                        plateList[i] = plateList[j];
                        plateList[j] = plateModel;
                    }
                    else if (plateList[i].Bound.Height == plateList[j].Bound.Height)
                    {
                        if (plateList[i].Bound.Width < plateList[j].Bound.Width)
                        {
                            PlateModel plateModel = plateList[i];
                            plateList[i] = plateList[j];
                            plateList[j] = plateModel;
                        }
                        else if (plateList[i].Bound.Width == plateList[j].Bound.Width)
                        {
                            if (plateList[i].Area < plateList[j].Area)
                            {
                                PlateModel plateModel = plateList[i];
                                plateList[i] = plateList[j];
                                plateList[j] = plateModel;
                            }
                        }
                    }
                }
            }
            return plateList;
        }
    }
}
