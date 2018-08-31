using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myCad.Shape
{
    class BaseColor
    {
        public Color judgeColor(string s)
        {
            if ("1".Equals(s))
            {
                return Color.Red;
            }
            else if ("2".Equals(s))
            {
                return Color.Yellow;
            }
            else if ("3".Equals(s))
            {
                return Color.Green;
            }
            else if ("4".Equals(s))
            {
                return Color.FromArgb(0, 255, 255); ;
            }
            else if ("5".Equals(s))
            {
                return Color.FromArgb(0, 0, 255); ;
            }
            else if ("6".Equals(s))
            {
                return Color.FromArgb(255, 0, 255); ;
            }
            else if ("7".Equals(s))
            {
                return Color.White;
            }
            else
            {
                return Color.White;
            }
        }
    }
}
