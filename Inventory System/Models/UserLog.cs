using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Action { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string TableName { get; set; }
        public int RowId { get; set; }
    }
}