using myCad .Model;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace FZYK .Nest
{
      public class PlateCombine
      {
            public PlateModel Plate1 { get; set; }
            public PlateModel Plate2 { get; set; }
            public RectangleF Rect { get; set; }
            public List<GridData> GridValue { get; set; }
            public int id { get; set; }
            public PlateCombine()
            {
            }
            public PlateCombine(PlateModel p1, PlateModel p2)
            {
                  Plate1 = p1;
                  Plate2 = p2;
            }

            public object Copy()
            {
                  return new PlateCombine() as object;
            }
      }
}
