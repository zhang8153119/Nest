using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using Cloo;
using System .Diagnostics;
using System .Collections .ObjectModel;
using System .Runtime .InteropServices;
using System .Drawing;
using System .ComponentModel;
using myCad .Model;
using FZYK .Nest;

namespace GPU
{
      class GpuHelper<TFP> : GPU .IGpuHelper where TFP : struct
      {
            private ComputeContext context;
            private ComputeCommandQueue commands;
            ICollection<ComputeEventBase> events = new Collection<ComputeEventBase>();

            FPType fpType;

            public GpuHelper(ComputeContext context, FPType fptype)
            {
                  this .context = context;
                  commands = new ComputeCommandQueue(context, context .Devices[0], ComputeCommandQueueFlags .None);
                  this .fpType = fptype;
            }
            #region hello
            public void GetHello(float num, ref float result)
            {
                  TFP t = Hello(num);
                  result = Convert .ToSingle(t);
            }
            public TFP Hello(float num)
            {
                  ComputeBuffer<TFP> result = new ComputeBuffer<TFP>(context, ComputeMemoryFlags .WriteOnly, 1);
                  string source = Encoding .ASCII .GetString(FZYK .Nest .Properties .Resources .nest);

                  if (fpType == FPType .FP64AMD)
                  {
                        source = "#define AMDFP64\n" + source;
                  }
                  else if (fpType == FPType .FP64)
                  {
                        source = "#define FP64\n" + source;
                  }
                  ComputeProgram program = new ComputeProgram(context, source);
                  try
                  {
                        program .Build(null, null, null, IntPtr .Zero);
                  }
                  catch (Exception)
                  {
                        var log = program .GetBuildLog(context .Devices[0]);
                        Debugger .Break();
                  }

                  ComputeKernel kernel = program .CreateKernel("hello");

                  TFP[] myresult = RunKernalTest(num, result, kernel);
                  return myresult[0];
            }
            private TFP[] RunKernalTest(float num, ComputeBuffer<TFP> result, ComputeKernel kernel)
            {
                  kernel .SetMemoryArgument(0, result);
                  kernel .SetValueArgument(1, num);

                  // BUG: ATI Stream v2.2 crash if event list not null.
                  commands .Execute(kernel, null, new long[] { 1, 1 }, null, events);
                  //commands.Execute(kernel, null, new long[] { count }, null, null);

                  TFP[] myresult = new TFP[1];

                  GCHandle arrCHandle = GCHandle .Alloc(myresult, GCHandleType .Pinned);

                  commands .Read(result, true, 0, 1, arrCHandle .AddrOfPinnedObject(), events);

                  arrCHandle .Free();
                  return myresult;
            }
            #endregion
            #region ArrayTest
            public int ArrayTest(int[,] a1, int[,] b)
            {
                  Point f;
                  int[,] a = new int[2, 2];
                  MyPoint[] plist = new MyPoint[100000];
                  MyPoint p1 = new MyPoint() { X = 1, Y = 1 };
                  MyPoint p2 = new MyPoint() { X = 2, Y = 2 };
                  plist[0] = p1;
                  plist[1] = p2;


                  IntPtr dataaddr_a = Marshal .UnsafeAddrOfPinnedArrayElement(plist, 0);
                  ComputeBuffer<MyPoint> abuffer = new ComputeBuffer<MyPoint>(context, ComputeMemoryFlags .ReadOnly | ComputeMemoryFlags .CopyHostPointer, plist);

                  string source = Encoding .ASCII .GetString(FZYK .Nest .Properties .Resources .nest);
                  ComputeProgram program = new ComputeProgram(context, source);
                  try
                  {
                        program .Build(null, null, null, IntPtr .Zero);
                  }
                  catch (Exception)
                  {
                        var log = program .GetBuildLog(context .Devices[0]);
                        Debugger .Break();
                  }

                  ComputeKernel kernel = program .CreateKernel("Sum");

                  ComputeBuffer<MyPoint> outdata = new ComputeBuffer<MyPoint>(context, ComputeMemoryFlags .ReadWrite, plist .Length);
                  kernel .SetMemoryArgument(0, abuffer);
                  kernel .SetMemoryArgument(1, outdata);

                  commands .Execute(kernel, null, new long[] { 2 }, null, events);

                  MyPoint[] myresult = new MyPoint[2];

                  GCHandle arrCHandle = GCHandle .Alloc(myresult, GCHandleType .Pinned);
                  commands .Read(outdata, true, 0, 2, arrCHandle .AddrOfPinnedObject(), events);
                  MyPoint[] test = myresult;
                  arrCHandle .Free();

                  return 0;
            }

            #endregion
            /// <summary>
            /// 编译cl程序
            /// </summary>
            /// <returns></returns>
            public ComputeProgram Build()
            {
                  string source = Encoding .ASCII .GetString(FZYK .Nest .Properties .Resources .nest);
                  ComputeProgram program = new ComputeProgram(context, source);
                  try
                  {
                        program .Build(null, null, null, IntPtr .Zero);
                  }
                  catch (Exception)
                  {
                        var log = program .GetBuildLog(context .Devices[0]);
                        Debugger .Break();
                  }

                  return program;
            }
            #region GetGridValue
            /// <summary>
            /// 栅格化,范围gridlib
            /// </summary>
            /// <param name="pointlist"></param>
            /// <param name="W"></param>
            /// <param name="H"></param>
            /// <param name="T"></param>
            /// <param name="program"></param>
            /// <returns></returns>
            public GridLib GetGridValue(PlateModel pm, float T, ComputeProgram program)
            {
                  int[,] gridarray = GetGridArray(pm, T, program);
                  int WI = gridarray .GetLength(0);
                  int HI = gridarray .GetLength(1);
                  int[,] gridarrayzero = new int[WI, HI];
                  List<GridData> grid = new List<GridData>();
                  for (int i = WI - 1; i >= 0; i--)
                  {
                        int v = 1;
                        int vz = 1;
                        for (int j = HI - 1; j >= 0; j--)
                        {
                              if (gridarray[i, j] == 1)
                              {
                                    gridarray[i, j] = v;
                                    grid .Insert(0, new GridData(i, j, v));
                                    v++;

                                    gridarrayzero[i, j] = 0;
                                    vz = 1;
                              }
                              else
                              {
                                    v = 1;

                                    gridarrayzero[i, j] = vz;
                                    vz++;
                              }
                        }
                  }
                  return new GridLib(grid, gridarray, gridarrayzero);
            }
            /// <summary>
            /// 合并图形栅格数据
            /// </summary>
            /// <param name="pc"></param>
            /// <param name="T"></param>
            /// <returns></returns>
            public GridLib GetGridValueCombine(PlateCombine pc, float T, ComputeProgram program)
            {
                  int[,] array1 = GetGridArray(pc .Plate1, T, program);
                  int[,] array2 = GetGridArray(pc .Plate2, T, program);
                  int w1 = array1 .GetLength(0);
                  int h1 = array1 .GetLength(1);
                  int w2 = array2 .GetLength(0);
                  int h2 = array2 .GetLength(1);

                  int w = Math .Max(w1, w2);
                  int h = Math .Max(h1, h2);

                  int[,] array = new int[w, h];
                  int[,] arrayzero = new int[w, h];
                  List<GridData> grid = new List<GridData>();
                  for (int i = w - 1; i >= 0; i--)
                  {
                        int v = 1;
                        int vz = 1;
                        for (int j = h - 1; j >= 0; j--)
                        {
                              int v1 = 0;
                              int v2 = 0;
                              if (i < w1 && j < h1)
                                    v1 = array1[i, j];
                              if (i < w2 && j < h2)
                                    v2 = array2[i, j];
                              int v0 = v1 | v2;
                              if (v0 == 1)
                              {
                                    array[i, j] = v;
                                    grid .Insert(0, new GridData(i, j, v));
                                    v++;

                                    arrayzero[i, j] = 0;
                                    vz = 1;
                              }
                              else
                              {
                                    array[i, j] = 0;
                                    v = 1;

                                    arrayzero[i, j] = vz;
                                    vz++;
                              }
                        }
                  }
                  return new GridLib(grid, array, arrayzero);
            }
            /// <summary>
            /// 栅格化范围数组
            /// </summary>
            /// <param name="pm"></param>
            /// <param name="T"></param>
            /// <param name="program"></param>
            /// <returns></returns>
            public int[,] GetGridArray(PlateModel pm, float T, ComputeProgram program)
            {
                  List<PointF> pointlist = pm .OutModel .ExpandPoint;
                  float W = pm .Rect .Width;
                  float H = pm .Rect .Height;
                  int WI = Convert .ToInt32(Math .Ceiling(W / T));
                  int HI = Convert .ToInt32(Math .Ceiling(H / T));
                  int pc = pointlist .Count;
                  MyPoint[] plist = new MyPoint[pc];
                  for (int i = 0; i < pointlist .Count; i++)
                  {
                        plist[i] .X = pointlist[i] .X;
                        plist[i] .Y = pointlist[i] .Y;
                  }
                  ComputeBuffer<MyPoint> pbuffer = new ComputeBuffer<MyPoint>(context, ComputeMemoryFlags .ReadOnly | ComputeMemoryFlags .CopyHostPointer, plist);

                  int[,] myresult = new int[HI, WI];
                  ComputeBuffer<int> outdata = new ComputeBuffer<int>(context, ComputeMemoryFlags .WriteOnly, WI * HI);

                  ComputeKernel kernelArray = program .CreateKernel("InitArray");
                  kernelArray .SetMemoryArgument(0, outdata);
                  commands .Execute(kernelArray, null, new long[] { WI * HI }, null, events);

                  ComputeKernel kernel = program .CreateKernel("GetGridValue");
                  kernel .SetMemoryArgument(0, pbuffer);
                  kernel .SetValueArgument(1, pc);
                  kernel .SetValueArgument(2, W);
                  kernel .SetValueArgument(3, H);
                  kernel .SetValueArgument(4, T);
                  kernel .SetValueArgument(5, WI);
                  kernel .SetValueArgument(6, HI);
                  kernel .SetMemoryArgument(7, outdata);
                  commands .Execute(kernel, null, new long[] { WI + HI }, null, events);

                  GCHandle arrCHandle = GCHandle .Alloc(myresult, GCHandleType .Pinned);
                  commands .Read(outdata, true, 0, WI * HI, arrCHandle .AddrOfPinnedObject(), events);

                  arrCHandle .Free();
                  kernel .Dispose();
                  pbuffer .Dispose();
                  outdata .Dispose();
                  return myresult;
            }
            #endregion
            #region insert
            /// <summary>
            /// 栅格化
            /// </summary>
            /// <param name="pointlist"></param>
            /// <param name="W"></param>
            /// <param name="H"></param>
            /// <param name="T"></param>
            /// <param name="program"></param>
            /// <returns></returns>
            public int[] Insert(int[] test1, int[] test2, ComputeProgram program)
            {
                  int c1 = test1 .GetLength(0);
                  int c2 = test2 .GetLength(0);
                  ComputeBuffer<int> buffer1 = new ComputeBuffer<int>(context, ComputeMemoryFlags .ReadOnly | ComputeMemoryFlags .CopyHostPointer, test1);
                  ComputeBuffer<int> buffer2 = new ComputeBuffer<int>(context, ComputeMemoryFlags .ReadOnly | ComputeMemoryFlags .CopyHostPointer, test2);
                  ComputeBuffer<int> result = new ComputeBuffer<int>(context, ComputeMemoryFlags .ReadWrite, c1);

                  ComputeKernel kernel = program .CreateKernel("Insert");
                  kernel .SetMemoryArgument(0, buffer1);
                  kernel .SetMemoryArgument(1, buffer2);
                  kernel .SetMemoryArgument(2, result);
                  commands .Execute(kernel, null, new long[] { c1 }, null, events);

                  int[] resultnum = new int[c1];
                  GCHandle arrCHandle = GCHandle .Alloc(resultnum, GCHandleType .Pinned);
                  commands .Read(result, true, 0, c1, arrCHandle .AddrOfPinnedObject(), events);

                  arrCHandle .Free();
                  kernel .Dispose();
                  buffer1 .Dispose();
                  buffer2 .Dispose();
                  result .Dispose();
                  return resultnum;
            }
            #endregion
      }


}
