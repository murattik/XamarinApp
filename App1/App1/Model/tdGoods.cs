using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    //модель товаров полученных с сервиса (не сохраняется)
    public class tdGoods
    {
        public int tdGoodsID { get; set; }
        public int CodeID { get; set; }
        public string CodeGoods { get; set; }
        public string BarCode { get; set; }
        public int Boxes { get; set; }
        public int Pallet { get; set; }
    }
}
