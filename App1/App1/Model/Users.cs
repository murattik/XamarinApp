using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Model
{
    [Table("Users")]
    public class Users
    {
        [PrimaryKey, Column("_id")]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string MapsUserName { get; set; }
        public int PinCode { get; set; }
        public int SkladID { get; set; }
        public string SkladName { get; set; }
        public int OrgID { get; set; }
        public string NameOrg { get; set; }
    }
}
