using SQLite;
using System;

namespace App1
{
    [Table("BarecodeResult")]
    public class BarecodeResult
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string BarecodeFormat { get; set; }
        public string TextResult { get; set; }
        public string CodeGoods { get; set; }
        public int Status { get; set; }
        public DateTime dateTime { get; set; }
        public string UserName { get; set; }
    }
}
