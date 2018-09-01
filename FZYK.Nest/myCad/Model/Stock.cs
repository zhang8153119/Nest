using myCad .CADInterfaceCtrl;
using myCad .Shape;
using myCad .ShapeOper;
using System;
using System .Collections .Generic;
using System .ComponentModel;
using System .Drawing;
using System .Drawing .Drawing2D;
using System .Linq;
using System .Text;
using System .Threading .Tasks;

namespace myCad .Model
{
      public class PartInfo
      {
            public int ID { get; set; }
            public string Type { get; set; }
            public float Angle { get; set; }
            public Point Location { get; set; }
            public PartInfo(int id, string type, float angle, Point location)
            {
                  ID = id;
                  Type = type;
                  Angle = angle;
                  Location = location;
            }
      }

      public class Stock
      {

            private float height = 0;                             //原材料整体的宽度
            private float width = 0;                              //原材料整体的长度
            private int num = 0;                                  //原材材料的数量     
            private PointF minPoint = new PointF();                //原材料的包络长方形的左下角点
            private PointF maxPoint;
            //原材料的包络长方形的右下角点
            private int useNum = 0;                                            //已使用的数量
            private int stockId = 0;
            [Bindable(false), Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility .Hidden)]                                   //原材料钢板编号，每添加一次有一个编号，每次的数量不定但是规格组成要一样,从0开始顺序增加
            public List<BaseModel> ListModel { get; set; } = new List<BaseModel>();  //原材料上可用碎片的集合

            public int[,] Disable { get; set; }
            public int id { get; set; }
            public float Use { get; set; } = 0f;
            public List<PartInfo> PartInfoList = new List<PartInfo>();

            [Bindable(false), Browsable(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility .Hidden)]
            public List<BaseShape> StockForm { get; set; } = new List<BaseShape>();
            public Stock Copy()
            {
                  Stock s = new Stock();
                  s .height = height;
                  s .width = width;
                  s .num = num;
                  s .minPoint = minPoint;
                  s .maxPoint = maxPoint;
                  s .useNum = useNum;
                  s .stockId = stockId;
                  s .ListModel = ListModel .ToList();
                  s .Disable = Disable;
                  s .id = id;
                  s .Use = Use;
                  s .PartInfoList = PartInfoList;
                  s .StockForm = StockForm .ToList();
                  return s;
            }
            public float Height
            {
                  get
                  {
                        return height;
                  }

                  set
                  {
                        height = value;
                  }
            }

            public float Width
            {
                  get
                  {
                        return width;
                  }

                  set
                  {
                        width = value;
                  }
            }

            public int Num
            {
                  get
                  {
                        return num;
                  }

                  set
                  {
                        num = value;
                  }
            }

            public PointF MinPoint
            {
                  get
                  {
                        return minPoint;
                  }

                  set
                  {
                        minPoint = value;
                  }
            }

            public PointF MaxPoint
            {
                  get
                  {
                        return maxPoint;
                  }

                  set
                  {
                        maxPoint = value;
                  }
            }


            public int UseNum
            {
                  get
                  {
                        return useNum;
                  }

                  set
                  {
                        useNum = value;
                  }
            }

            public int StockId
            {
                  get
                  {
                        return stockId;
                  }

                  set
                  {
                        stockId = value;
                  }
            }
            
      }
}
