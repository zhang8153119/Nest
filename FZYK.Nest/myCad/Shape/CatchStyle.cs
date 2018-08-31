using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    //捕捉状态
    public enum CatchStyle
    {
        None,                     //无
        Vertex,                   //
        MidPoint,                 //线段中点
        Centre,                   //中心
        Intersection,
        Extension,
        Nearest,
        Quadrant,
        Tangency,
        Node,
        Perpendicular,
        PolarTracking
    }
}
