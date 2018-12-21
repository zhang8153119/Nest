using myCad .Model;
using System;
using System .Collections .Generic;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace FZYK .Nest
{
      public class DNA
      {
            public List<Stock> Stock { get; set; }
            public List<Basic> Basic { get; set; }
            public float Fitness { get; set; }

            public DNA(List<Stock> stock, List<Basic> basic, float fitness = 0)
            {
                  Stock = stock;
                  Basic = basic;
                  Fitness = fitness;
            }
            public DNA()
            {
                  Stock = new List<Stock>();
                  Basic = new List<Basic>();
                  Fitness = 0f;
            }

            public DNA Copy()
            {
                  List<Stock> s = new List<Stock>();
                  for (int i = 0; i < Stock .Count; i++)
                  {
                        s .Add(Stock[i] .Copy());
                  }
                  List<Basic> b = new List<Basic>();
                  for (int i = 0; i < Basic .Count; i++)
                  {
                        b .Add(Basic[i] .Copy());
                  }
                  return new DNA(s, b, Fitness);
            }
      }

      public class Basic
      {
            public int Id { get; set; }
            public int Angle { get; set; }
            public int AngleCombine { get; set; }
            public float Area { get; set; }
            public int Count { get; set; }
            public int CombineType { get; set; }//1-16

            public Basic(int id, int angle, int anglecombine, float area, int count, int combinetype)
            {
                  Id = id;
                  Angle = angle;
                  AngleCombine = anglecombine;
                  Area = area;
                  Count = count;
                  CombineType = combinetype;
            }

            public Basic Copy()
            {
                  Basic newbasic = new Basic(Id
                        , Angle
                        , AngleCombine
                        , Area
                        , Count
                        , CombineType);
                  return newbasic;
            }
      }

      public class MyRectangle
      {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Type { get; set; }//类型，0为正常，1为孔

            public MyRectangle(int x, int y, int width, int height, int type = 0)
            {
                  X = x;
                  Y = y;
                  Width = width;
                  Height = height;
                  Type = type;
            }
            public MyRectangle Copy()
            {
                  return new MyRectangle(X, Y, Width, Height, Type);
            }
      }
}
