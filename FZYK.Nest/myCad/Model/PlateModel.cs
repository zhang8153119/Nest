using FZYK .Nest;
using myCad .Shape;
using System .Collections .Generic;
using System .Drawing;
using System .Linq;

namespace myCad .Model
{
      public class PlateModel
      {
            #region 参数

            private string plateName;                //件号名称
            private string plateNum;                 //件号代号 ，用于打印
            private string plateCode;                //件号在系统里面的编号
            private int inheritanceID;               //遗传算法基因编码
            private int plateCount;                  //件号数量
            private int plateUseCount;               //件号使用数量
            private string standard;                 //标准：国网，南网。。。。
            private string specifications;           //规格：-6，-8.。。。。
            private string material;                 //材质：Q235，Q345。。。。
            private BaseShape text;                  //文字图形
            private ModelStyle plateClass;           //钢板的类型
            private BaseModel outModel = new BaseModel();              //外部模型
            private List<BaseModel> innerModel = new List<BaseModel>();     //内部模型
            private float area = 0;                             //件号面积
            private RectangleF bound;                           //件号的边界长方形
            private bool isArc = false;                         //是否属于带弧图形
            private PointF rotateCenter = new PointF();         //旋转的中心（最小长方形时确定的中心）
            private PointF powCenter = new PointF();            //图形的重心，为图形中各个顶点的坐标平均值，最小长方形时计算一次，后随几何变换而变换。
            private bool hadUsedGene = false;                   //是否使用过该基因,使用过则为true;
            private int heightPower = 0;                        //宽度权重
            private int areaPower = 0;                          //面积权重
            private int widthPower = 0;                         //长度权重

            public List<GridData> GridValue { get; set; }//栅格值
            public int id { get; set; }
            public float GridLen { get; set; }
            public float GridWid { get; set; }
            public RectangleF Rect { get; set; }//最小包络矩形
            public bool Combine { get; set; }//是否是组合图形
            #endregion
            public PlateModel Copy()
            {
                  PlateModel pm = new Model .PlateModel();
                  pm .plateName = plateNum;
                  pm .plateNum = plateNum;
                  pm .plateCode = plateCode;
                  pm .inheritanceID = inheritanceID;
                  pm .plateCount = plateCount;
                  pm .plateUseCount = plateUseCount;
                  pm .standard = standard;
                  pm .specifications = specifications;
                  pm .material = material;
                  pm .text = text;
                  pm .plateClass = plateClass;
                  pm .outModel = outModel;
                  pm .innerModel = innerModel .ToList();
                  pm .area = area;
                  pm .bound = bound;
                  pm .isArc = isArc;
                  pm .rotateCenter = rotateCenter;
                  pm .powCenter = powCenter;
                  pm .hadUsedGene = hadUsedGene;
                  pm .heightPower = heightPower;
                  pm .areaPower = areaPower;
                  pm .widthPower = widthPower;
                  pm .GridValue = GridValue == null ? null : GridValue .ToList();
                  pm .id = id;
                  pm .GridLen = GridLen;
                  pm .GridWid = GridWid;
                  pm .Rect = Rect;
                  pm .Combine = Combine;
                  return pm;
            }
            #region set，get
            public string PlateName
            {
                  get
                  {
                        return plateName;
                  }

                  set
                  {
                        plateName = value;
                  }
            }

            public string PlateNum
            {
                  get
                  {
                        return plateNum;
                  }

                  set
                  {
                        plateNum = value;
                  }
            }

            public string PlateCode
            {
                  get
                  {
                        return plateCode;
                  }

                  set
                  {
                        plateCode = value;
                  }
            }

            public int PlateCount
            {
                  get
                  {
                        return plateCount;
                  }

                  set
                  {
                        plateCount = value;
                  }
            }

            public int PlateUseCount
            {
                  get
                  {
                        return plateUseCount;
                  }

                  set
                  {
                        plateUseCount = value;
                  }
            }

            public string Standard
            {
                  get
                  {
                        return standard;
                  }

                  set
                  {
                        standard = value;
                  }
            }

            public string Specifications
            {
                  get
                  {
                        return specifications;
                  }

                  set
                  {
                        specifications = value;
                  }
            }

            public string Material
            {
                  get
                  {
                        return material;
                  }

                  set
                  {
                        material = value;
                  }
            }

            public BaseShape Text
            {
                  get
                  {
                        return text;
                  }

                  set
                  {
                        text = value;
                  }
            }

            public ModelStyle PlateClass
            {
                  get
                  {
                        return plateClass;
                  }

                  set
                  {
                        plateClass = value;
                  }
            }

            internal BaseModel OutModel
            {
                  get
                  {
                        return outModel;
                  }

                  set
                  {
                        outModel = value;
                  }
            }

            internal List<BaseModel> InnerModel
            {
                  get
                  {
                        return innerModel;
                  }

                  set
                  {
                        innerModel = value;
                  }
            }

            public float Area
            {
                  get
                  {
                        return area;
                  }

                  set
                  {
                        area = value;
                  }
            }

            public RectangleF Bound
            {
                  get
                  {
                        return bound;
                  }

                  set
                  {
                        bound = value;
                  }
            }

            public bool IsArc
            {
                  get
                  {
                        return isArc;
                  }

                  set
                  {
                        isArc = value;
                  }
            }

            public PointF RotateCenter
            {
                  get
                  {
                        return rotateCenter;
                  }

                  set
                  {
                        rotateCenter = value;
                  }
            }

            public PointF PowCenter
            {
                  get
                  {
                        return powCenter;
                  }

                  set
                  {
                        powCenter = value;
                  }
            }

            public int InheritanceID
            {
                  get
                  {
                        return inheritanceID;
                  }

                  set
                  {
                        inheritanceID = value;
                  }
            }

            public bool HadUsedGene
            {
                  get
                  {
                        return hadUsedGene;
                  }

                  set
                  {
                        hadUsedGene = value;
                  }
            }

            public int HeightPower
            {
                  get
                  {
                        return heightPower;
                  }

                  set
                  {
                        heightPower = value;
                  }
            }

            public int AreaPower
            {
                  get
                  {
                        return areaPower;
                  }

                  set
                  {
                        areaPower = value;
                  }
            }

            public int WidthPower
            {
                  get
                  {
                        return widthPower;
                  }

                  set
                  {
                        widthPower = value;
                  }
            }

            #endregion

      }
}
