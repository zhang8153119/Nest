using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    class CoordinateShape
    {
        /// <summary>
        /// 界面坐标图形
        /// </summary>
        /// <param name="g"></param>
        public static void Draw(Graphics g)
        {

            //g.DrawLine(new Pen(Color.White, 0), 0, 0, 60, 60);
            g.DrawLine(new Pen(Color.White, 0), -6, 0, 60, 0);
            g.DrawLine(new Pen(Color.White, 0), 0, -6, 0, 60);
            g.DrawRectangle(new Pen(Color.White, 0), -6, -6, 12, 12);
            PointF[] XP = new PointF[] { new PointF(60, 6), new PointF(60, -6), new PointF(75, 0) };
            PointF[] YP = new PointF[] { new PointF(-6, 60), new PointF(6, 60), new PointF(0, 75) };
            g.DrawPolygon(new Pen(Color.White, 0), XP);
            g.DrawPolygon(new Pen(Color.White, 0), YP);

            g.DrawLine(new Pen(Color.White, 0), 90, 8, 102, -8);//绘制X
            g.DrawLine(new Pen(Color.White, 0), 90, -8, 102, 8);

            g.DrawLine(new Pen(Color.White, 0), 6, 101, 0, 93);//绘制Y
            g.DrawLine(new Pen(Color.White, 0), -6, 101, 0, 93);
            g.DrawLine(new Pen(Color.White, 0), 0, 93, 0, 85);

        }
    }
}
