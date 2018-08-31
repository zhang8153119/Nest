using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.ShapeOper
{
    class CreateBound
    {
        public RectangleF getModelBound(GraphicsPath modelPath,Graphics g)
        {
            //Pen pen = new Pen(Color.White, 0);
            //modelPath.Widen(pen);
            return new Region(modelPath).GetBounds(g);
        }
        public RectangleF getModelBound(Region region, Graphics g)
        {
            return region.GetBounds(g);
        }

    }
}
