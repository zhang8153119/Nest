using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class PaiYangFangAn
    {
        private List<PlatePosition> listPlate = new List<PlatePosition>();     //方案件号集合
        private Stock stock = new Stock();                                     //方案所在的原材料钢板
        //private 
        private float liYouLv = 0;                                             //方案利用率          
        private int fangAnId = 0;                                              //方案编号,利用率计算出来后才加上编号        

        public float LiYouLv
        {
            get
            {
                return liYouLv;
            }

            set
            {
                liYouLv = value;
            }
        }

        public int FangAnId
        {
            get
            {
                return fangAnId;
            }

            set
            {
                fangAnId = value;
            }
        }

        internal List<PlatePosition> ListPlate
        {
            get
            {
                return listPlate;
            }

            set
            {
                listPlate = value;
            }
        }

        public Stock Stock
        {
            get
            {
                return stock;
            }

            set
            {
                stock = value;
            }
        }
    }
}
