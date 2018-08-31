using myCad.CADInterfaceCtrl;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.DrawTools
{
    class NoToolLoad : BaseTool
    {
        private PointF firstPoint, secondPoint;

        public override void MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isHited == true)
            {
                MouseShape.MStyle = MouseStyle.Normal;

                PointSelect();
            }
            else if (e.Button == MouseButtons.Left && isHited == false)
            {
                switch (count)
                {
                    case 0:
                        count = 1;
                        MouseShape.MStyle = MouseStyle.None;
                        firstPoint = MouseShape.MousePoint;
                        CADInterface.selectRect = new MRectangle(MouseShape.MousePoint, MouseShape.MousePoint);
                        CADInterface.selectRect.IsSelectRect = true;//是否为矩形选框项，如为true则NCRectangle类画矩形选框；如为false则NCRectangle类画矩形图形。
                        break;

                    case 1:
                        secondPoint = MouseShape.MousePoint;
                        RectangleF rect = new RectangleF();
                        rect.Width = Math.Abs(secondPoint.X - firstPoint.X);
                        rect.Height = Math.Abs(secondPoint.Y - firstPoint.Y);
                        rect.Y = firstPoint.Y < secondPoint.Y ? firstPoint.Y : secondPoint.Y;
                        if (firstPoint.X < secondPoint.X)//判断该矩形框为正选框还是反选框。
                        {
                            rect.X = firstPoint.X;
                            positiveRect = rect;
                            RectSelectPositive();
                        }
                        else
                        {
                            rect.X = secondPoint.X;
                            negativeRect = rect;
                            RectSelectNegative();
                        }
                        count = 0;
                        CADInterface.selectRect = new MRectangle();
                        //UnLoadTool();
                        break;
                }
            }
        }

        public override void MouseMove(MouseEventArgs e)
        {
            if (count == 1)
            {
                secondPoint = MouseShape.MousePoint;
                RectangleF rect = new RectangleF();
                if (firstPoint.X < secondPoint.X)
                {
                    rect.X = firstPoint.X;
                    CADInterface.selectRect.SelRect = MRectangle.SelectRect.positive;
                }
                else
                {
                    rect.X = secondPoint.X;
                    CADInterface.selectRect.SelRect = MRectangle.SelectRect.negative;
                }
                rect.Y = firstPoint.Y < secondPoint.Y ? firstPoint.Y : secondPoint.Y;
                rect.Width = Math.Abs(secondPoint.X - firstPoint.X);
                rect.Height = Math.Abs(secondPoint.Y - firstPoint.Y);
                CADInterface.selectRect.Rect = rect;
                return;
            }

            MouseShape.MStyle = MouseStyle.Normal;

            getHit();
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (CADInterface.currentShapes == null) return;
            if (e.KeyCode == Keys.Escape && count != 1)
            {
                for (int i = CADInterface.currentShapes.Count - 1; i >= 0; i--)
                {
                    CADInterface.currentShapes[i].Show = DisplayStyle.Normal;
                    CADInterface.currentShapes[i].IsSelected = false;
                }
            }
            else if (e.KeyCode == Keys.Escape && count == 1)
            {
                count = 0;
                CADInterface.selectRect = new MRectangle();//选框画完即消失。
                //UnLoadTool();
            }
        }

        public override void PointSelect()
        {
            if (CADInterface.currentShapes == null) return;

            for (int i = CADInterface.currentShapes.Count - 1; i >= 0; i--)
            {
                if (CADInterface.currentShapes[i].IntersectsWith(MouseShape.HitRect))
                {
                    CADInterface.currentShapes[i].Show = DisplayStyle.Select;
                    CADInterface.currentShapes[i].IsSelected = true;
                    isHited = false;
                    //shapesSel = true;
                    return;
                }
            }
        }

        public override void RectSelectPositive()
        {
            if (CADInterface.currentShapes == null) return;

            for (int i = CADInterface.currentShapes.Count - 1; i >= 0; i--)
            {
                if (CADInterface.currentShapes[i].SurroundedBy(positiveRect))
                {
                    CADInterface.currentShapes[i].Show = DisplayStyle.Select;
                    CADInterface.currentShapes[i].IsSelected = true;
                    //shapesSel = true;
                }
            }
        }

        public override void RectSelectNegative()
        {
            if (CADInterface.currentShapes == null) return;

            for (int i = CADInterface.currentShapes.Count - 1; i >= 0; i--)
            {
                if (CADInterface.currentShapes[i].IntersectsWith(negativeRect))
                {
                    CADInterface.currentShapes[i].Show = DisplayStyle.Select;
                    CADInterface.currentShapes[i].IsSelected = true;
                    //shapesSel = true;
                }
            }
        }
    }
}
