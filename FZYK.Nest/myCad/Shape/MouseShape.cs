using myCad.CADInterfaceCtrl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    public class MouseShape 
    {
        public static MouseStyle MStyle = MouseStyle.Normal;    //鼠标状态

        public static MouseStyle LastMStyle = MouseStyle.Normal;//按住中键平移，松开中键后鼠标样式恢复此样式。

        private static PointF mousePoint;   //鼠标点坐标

        private static RectangleF hitRect;//碰撞矩形，鼠标移动时，中心矩形碰撞到图元是则图元高亮显示。

        /// <summary>
        /// 返回鼠标的坐标点
        /// </summary>
        public static PointF MousePoint
        {
            get { return MouseShape.mousePoint; }
            set { MouseShape.mousePoint = value; }
        }
        /// <summary>
        /// 返回旧的鼠标点
        /// </summary>
        public static PointF OldMouse { get; set; }
        /// <summary>
        /// 返回区域
        /// </summary>
        public static RectangleF HitRect
        {
            get { return MouseShape.hitRect; }
            set { MouseShape.hitRect = value; }
        }
        /// <summary>
        /// 绘制鼠标
        /// </summary>
        /// <param name="g">画板</param>
        public static void Draw(Graphics g)
        {

            g.TranslateTransform(mousePoint.X * CADInterface.globalZoomNum, mousePoint.Y * CADInterface.globalZoomNum);
            switch (MStyle)
            {
                case MouseStyle.Normal:
                    g.DrawLine(new Pen(Color.White, 0), -40, 0, 40, 0);
                    g.DrawLine(new Pen(Color.White, 0), 0, -40, 0, 40);
                    g.DrawRectangle(new Pen(Color.White, 0), -3, -3, 6, 6);//碰撞矩形HitRect
                    break;
                case MouseStyle.Draw:
                    g.DrawLine(new Pen(Color.White, 0), -40, 0, 40, 0);
                    g.DrawLine(new Pen(Color.White, 0), 0, -40, 0, 40);
                    break;
                case MouseStyle.Edite:
                    g.DrawRectangle(new Pen(Color.White, 0), -3, -3, 6, 6);//碰撞矩形HitRect
                    break;
                case MouseStyle.None:
                    break;
            }
        }
    }
}
