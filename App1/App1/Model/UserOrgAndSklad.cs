using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Model
{
    [Table("UserOrgAndSklad")]
    public class UserOrgAndSklad
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int OrgID { get; set; }
        public string NameOrg { get; set; }
        public int SkladID { get; set; }
        public string SkladName { get; set; }
    }
}
