using myCad.BaseShapeOper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myCad.ShapeOper
{
    class ZoomOper : BaseZoom
    {
        public void zoomYuanCaiLiao(Control fromControl)
        {
            zoom(fromControl);
        }
    }
}
