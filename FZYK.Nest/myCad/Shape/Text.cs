using myCad .CADInterfaceCtrl;
using System;
using System .Collections .Generic;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .Linq;
using System .Text;
using System .Threading .Tasks;
using System .Windows .Forms;

namespace myCad .Shape
{
      class Text : BaseShape
      {

            private string textString = "";
            private PointF locadPoint;
            private float angle = 0;
            private float textWidth = 0;

            public Text(string ts, PointF lp, float a, float tw)
            {
                  textString = ts;
                  locadPoint = lp;
                  angle = a;
                  textWidth = tw;
                  this .ShapeClass = "Text";
            }

            public string TextString
            {
                  get
                  {
                        return textString;
                  }

                  set
                  {
                        textString = value;
                  }
            }

            public PointF LocadPoint
            {
                  get
                  {
                        return locadPoint;
                  }

                  set
                  {
                        locadPoint = value;
                  }
            }

            public float Angle
            {
                  get
                  {
                        return angle;
                  }

                  set
                  {
                        angle = value;
                  }
            }

            public float TextWidth
            {
                  get
                  {
                        return textWidth;
                  }

                  set
                  {
                        textWidth = value;
                  }
            }

            private PointF LP;

            public override void Draw(Graphics g, float zoomNum)
            {
                  Pen pen = new Pen(this .PenColor);
                  Brush brush = pen .Brush;

                  Font font = new Font("宋体", textWidth * zoomNum);

                  LP = new PointF(0, 0);
                  Matrix oldMatrix = g .Transform;

                  #region 位置处理
                  g .MultiplyTransform(new Matrix(1, 0, 0, -1, 0, 0));           //镜像

                  g .MultiplyTransform(new Matrix(1, 0, 0, 1, locadPoint .X, -locadPoint .Y));   //移动

                  g .RotateTransform(360 - angle);      //旋转

                  //移动后偏移的距离，原本因该在旋转之前就要偏移，但是不知道为什么旋转前偏移会错，放在旋转后就会正确
                  //现在可以理解：MatrixOrder执行顺序，不指定的时候，当前的操作是在前一步完成之后的新坐标系中进行的。对应于矩阵算法相当于左乘。
                  //所以这个偏差的距离在旋转后移动才能移动到需要的位置，在旋转之前移动需要考虑旋转的角度计算，X轴和y轴的偏差。
                  //旋转之后再移动比较简便。
                  g .MultiplyTransform(new Matrix(1, 0, 0, 1, 0, -textWidth));

                  #endregion

                  g .ScaleTransform(1 / zoomNum, 1 / zoomNum);

                  g .DrawString(textString, font, brush, LP);

                  g .Transform = oldMatrix;

            }
      }
}
