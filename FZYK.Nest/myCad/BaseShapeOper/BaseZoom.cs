using myCad.CADInterfaceCtrl;
using myCad.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.BaseShapeOper
{
    class BaseZoom
    {
        /// <summary>
        /// 基础窗体缩放
        /// </summary>
        protected void zoom(Control fromControl)
        {
            UserControl userControl = new UserControl();
            foreach (System.Windows.Forms.Control control in fromControl.Controls)
            {
                if (control is CADInterface)
                {
                    userControl = (CADInterface)control;
                }
            }
            if (CADInterface.nowStock.StockForm.Count <= 0)
            {
                CADInterface.globalZoomNum = 1;
                CADInterface.scaleNum = 1;
                TransformMatrix.initMatrix = new System.Drawing.Drawing2D.Matrix(1, 0, 0, -1, 20, 550);
            }
            else
            {
                CADInterface.globalZoomNum = 1;
                CADInterface.scaleNum = 1;
                TransformMatrix.initMatrix = new System.Drawing.Drawing2D.Matrix(
                    1, 0, 0, -1,
                    (float)0.5 * (userControl.Width - CADInterface.nowStock.Width),
                    (float)0.5 * (userControl.Height + CADInterface.nowStock.Height));

                float zoomNum = CADInterface.nowStock.Height / userControl.Height > CADInterface.nowStock.Width / userControl.Width
                    ? userControl.Height / CADInterface.nowStock.Height
                    : userControl.Width / CADInterface.nowStock.Width;

                CADInterface.globalZoomNum = zoomNum;
                CADInterface.scaleNum = zoomNum;
                CADInterface.bGrp.Graphics.Transform = TransformMatrix.ScaleByPoint(
                    CADInterface.scaleNum,
                    new PointF((float)0.5 * CADInterface.nowStock.Width, (float)0.5 * CADInterface.nowStock.Height));
            }
        }

        /// <summary>
        /// 全部内容缩放
        /// </summary>
        private void zoomAll()
        { }

        /// <summary>
        /// 按照范围进行缩放
        /// </summary>
        /// <param name="pointS"></param>
        /// <param name="pointE"></param>
        private void zoom(PointF pointS,PointF pointE)
        { }
    }
}
