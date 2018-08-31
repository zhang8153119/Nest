using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
     public enum DisplayStyle
    {
        Normal,     //常规状态
        Preview,    //预览状态
        Hit,        //碰撞状态,显示高亮，但是不置为选择
        Select,     //选择状态
        Edit        //编辑状态
    }
}
