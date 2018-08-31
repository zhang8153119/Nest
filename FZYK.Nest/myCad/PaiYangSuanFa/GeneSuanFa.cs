using myCad.Model;
using myCad.ShapeOper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.PaiYangSuanFa
{
    class GeneSuanFa
    {

        private List<PaiYangFangAn> listFangAn = new List<PaiYangFangAn>();  //方案序列

        /// <summary>
        /// 给所有件号钢板进行基因编号
        /// </summary>
        /// <param name="listPlate"></param>
        /// <returns></returns>
        public List<PlateModel> createGeneCode(List<PlateModel> listPlate)
        {
            List<PlateModel> newGeneCode = new List<PlateModel>();
            int code = 0;
            CopyOper copy = new CopyOper();
            for (int i = 0; i < listPlate.Count; i++)
            {
                for (int j = 0; j < listPlate[i].PlateCount; j++)
                {
                    PlateModel newPlate = copy.CopyPlate(listPlate[i]);
                    newPlate.InheritanceID = code;
                    code = code + 1;
                    newGeneCode.Add(newPlate);
                }
            }
            return newGeneCode;
        }

        /// <summary>
        /// 获取最终选定的排样方案
        /// </summary>
        /// <param name="listPlate"></param>
        /// <param name="firstFangAn"></param>
        /// <returns></returns>
        public PaiYangFangAn getFinallyFangAn(List<PlateModel> listGene, PaiYangFangAn firstFangAn)
        {
            PaiYangFangAn finallyFangAn = new PaiYangFangAn();

            return finallyFangAn;
        }

        /// <summary>
        /// 对方案的基因序列进行变异，
        /// 变异基因后的基因不能存在于原方案中，变异量要足够的多
        /// </summary>
        /// <param name="listGene"></param>
        /// <param name="fangAn"></param>
        /// <param name="yiBianLv">异变概率</param>
        /// <returns></returns>
        public PaiYangFangAn getBianYiFangAn(List<PlateModel> listGene, PaiYangFangAn fangAn,float yiBianLv)
        {
            #region 创造轮盘
            List<RouletteAlgorithm> listLP = new List<RouletteAlgorithm>();
            for (int i = 0; i < listGene.Count; i++)
            {
                if (!listGene[i].HadUsedGene)
                {
                    RouletteAlgorithm ra = new RouletteAlgorithm();
                    ra.Plate = listGene[i];
                    listLP.Add(ra);
                }
            }
            listLP = getLunPan(listLP);     //生成轮盘概率
            #endregion

            Random rt = new Random();
            for (int i = 0;i< fangAn.ListPlate.Count; i++)
            {
                if (rt.NextDouble() < yiBianLv)
                {
                    //进行变异
                    double gaiLv = rt.NextDouble();
                    for (int j = 0; j < listLP.Count; j++)
                    {
                        if (gaiLv >= listLP[j].JiLeiGaiLvMin && gaiLv < listLP[j].JiLeiGaiLvMax)
                        {
                            PlateModel plate = listLP[j].Plate;
                            listLP[j].Plate = fangAn.ListPlate[i].Plate;
                            fangAn.ListPlate[i].Plate = plate;
                        }
                    }
                }
            }
            return fangAn;
        }

        /// <summary>
        /// 轮盘赌算法获取轮盘
        /// </summary>
        /// <returns></returns>
        private List<RouletteAlgorithm> getLunPan(List<RouletteAlgorithm> listLP)
        {
            double everyPart = 1 / listLP.Count;
            double sign = 0;
            for (int i = 0; i< listLP.Count;i++)
            {
                listLP[i].JiLeiGaiLvMin = sign;
                sign = sign + everyPart;
                listLP[i].JiLeiGaiLvMax = sign;
            }
            return listLP;
        }

        /// <summary>
        /// 对两个方案进行交叉遗传，最多生成四个子代
        /// 子代交叉后存在重复的时候交叉不成功，
        /// 按照交叉率为上线进行交叉变化
        /// </summary>
        /// <param name="firstFangAn"></param>
        /// <param name="secondFangAn"></param>
        /// <returns></returns>
        public List<PaiYangFangAn> getJiaoChaFangAn(PaiYangFangAn firstFangAn, PaiYangFangAn secondFangAn, float jiaoChaLv)
        {
            List<PaiYangFangAn> listPYFA = new List<PaiYangFangAn>();
            Random rt = new Random();
            if (rt.NextDouble() < jiaoChaLv)
            {

            }

            return listPYFA;
        }
    }
}
