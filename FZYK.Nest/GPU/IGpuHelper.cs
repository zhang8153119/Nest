using Cloo;
using FZYK .Nest;
using myCad .Model;
using System;
using System .Collections .Generic;
using System .Drawing;

namespace GPU
{
      interface IGpuHelper
      {
            void GetHello(float num, ref float str);

            GridLib GetGridValue(PlateModel pm, float T, ComputeProgram gpuprogram);
            GridLib GetGridValueCombine(PlateCombine pc, float T, ComputeProgram gpuprogram);
            int[,] GetGridArray(PlateModel pm, float T, ComputeProgram program);
                  
            int[] Insert(int[] test1, int[] test2, ComputeProgram program);
            ComputeProgram Build();
      }
}
