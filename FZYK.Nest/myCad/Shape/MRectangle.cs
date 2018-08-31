using myCad.CADInterfaceCtrl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    /// <summary>
    /// 矩形，矩形选框（正向选择，反向选择）
    /// </summary>
    public class MRectangle : BaseShape
    {

        private bool isSelectRect = true;          //true为绘制选择框，false为绘制矩形
        private SelectRect selRect;                //选择框形式
        private RectangleF rect;                   //矩形
        private PointF startPoint;                 //起始点
        private PointF endPoint;                   //终止点


        public MRectangle() { }

        public MRectangle(RectangleF rect)
        {
            Rect = rect;
        }
        public MRectangle(PointF startPoint, PointF endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        /// <summary>
        /// 枚举选择方式
        /// </summary>
        public enum SelectRect
        {
            positive,            //正选
            negative             //反选
        }

        private RectangleF rectShow; //缩放后显示的坐标范围

        public RectangleF Rect
        {
            get
            {
                return rect;
            }

            set
            {
                rect = value;
            }
        }

        public PointF StartPoint
        {
            get
            {
                return startPoint;
            }

            set
            {
                startPoint = value;
            }
        }

        public PointF EndPoint
        {
            get
            {
                return endPoint;
            }

            set
            {
                endPoint = value;
            }
        }

        public bool IsSelectRect
        {
            get
            {
                return isSelectRect;
            }

            set
            {
                isSelectRect = value;
            }
        }

        public SelectRect SelRect
        {
            get
            {
                return selRect;
            }

            set
            {
                selRect = value;
            }
        }

        public override void Draw(Graphics g)
        {

            rectShow.X = rect.X * CADInterface.globalZoomNum;//与Canvas类中的DrawShap方法中容器里的比例相反
            rectShow.Y = rect.Y * CADInterface.globalZoomNum;
            rectShow.Width = rect.Width * CADInterface.globalZoomNum;
            rectShow.Height = rect.Height * CADInterface.globalZoomNum;

            if (IsSelectRect)
            {
                switch (SelRect)//画矩形选框
                {
                    case SelectRect.positive:
                        g.FillRectangle(new SolidBrush(Color.FromArgb(80, 0, 100, 255)), rectShow.X, rectShow.Y, rectShow.Width, rectShow.Height);
                        g.DrawRectangle(new Pen(Color.White), rectShow.X, rectShow.Y, rectShow.Width, rectShow.Height);
                        break;
                    case SelectRect.negative:
                        g.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 255, 0)), rectShow.X, rectShow.Y, rectShow.Width, rectShow.Height);
                        Pen newPen = new Pen(Color.White, 1);
                        newPen.DashStyle = DashStyle.Custom;
                        newPen.DashPattern = new float[] { 3, 3 };
                        g.DrawRectangle(newPen, rectShow.X, rectShow.Y, rectShow.Width, rectShow.Height);
                        break;
                }
            }
        }
    }
}
