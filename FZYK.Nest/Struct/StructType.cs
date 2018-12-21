using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace FZYK .Nest 
{
      public struct MyLine
      {
            public MyPoint Start { get; set; }
            public MyPoint End { get; set; }

            public MyLine(MyPoint p1, MyPoint p2)
            {
                  Start = p1;
                  End = p2;
            }
      }


      public struct MyPoint
      {
            public float X { get; set; }
            public float Y { get; set; }

            public MyPoint(float x, float y)
            {
                  X = x;
                  Y = y;
            }
      }
}
