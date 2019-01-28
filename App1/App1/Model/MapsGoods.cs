using SQLite;

namespace App1
{
    //локальная БД для хранения товаров с сервиса
    [Table("MapsGoods")]
    public class MapsGoods
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int GoodsID { get; set; }
        public int CodeID { get; set; }
        public string CodeGoods { get; set; }
        public string BarCode { get; set; }
        public int Boxes { get; set; }
        public int Pallet { get; set; }
    }
}
