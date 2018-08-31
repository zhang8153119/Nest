/**
 * 轮盘赌算法Model
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Model
{
    class RouletteAlgorithm
    {
        private PlateModel plate;
        private double jiLeiGaiLvMin = 0;                 //轮赌算法概率范围下限
        private double jiLeiGaiLvMax = 0;                 //轮赌算法概率范围上限

        public PlateModel Plate
        {
            get
            {
                return plate;
            }

            set
            {
                plate = value;
            }
        }

        public double JiLeiGaiLvMin
        {
            get
            {
                return jiLeiGaiLvMin;
            }

            set
            {
                jiLeiGaiLvMin = value;
            }
        }

        public double JiLeiGaiLvMax
        {
            get
            {
                return jiLeiGaiLvMax;
            }

            set
            {
                jiLeiGaiLvMax = value;
            }
        }
    }
}
