using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    public class TransformMatrix
    {
        //public static Matrix initMatrix = new Matrix(1, 0, 0, -1, 30, 550);

        public static Matrix initMatrix = new Matrix(1, 0, 0, -1, 20, 550);

        /// <summary>
        /// 获取鼠标的点
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static PointF GetMousePoint(float X, float Y)
        {
            Matrix potTransMatris = new Matrix(1 / initMatrix.Elements[0], 0, 0, 1 / initMatrix.Elements[3], -initMatrix.Elements[4] / initMatrix.Elements[0], -initMatrix.Elements[5] / initMatrix.Elements[3]);
            PointF[] pot = new PointF[] { new PointF(X, Y) };
            potTransMatris.TransformPoints(pot);
            return pot[0];
        }

        /// <summary>
        /// 缩放比例
        /// </summary>
        /// <param name="scaleFactor"></param>
        /// <param name="scalePoint"></param>
        /// <returns></returns>
        public static Matrix ScaleByPoint(float scaleFactor, PointF scalePoint)
        {
            Matrix grpTransMatrix = new Matrix(scaleFactor, 0, 0, scaleFactor, (1 - scaleFactor) * scalePoint.X, (1 - scaleFactor) * scalePoint.Y);
            initMatrix.Multiply(grpTransMatrix, MatrixOrder.Prepend);
            return initMatrix;
        }
    }
}
