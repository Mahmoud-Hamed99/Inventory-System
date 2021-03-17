using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class User
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string Password { get; set; } = "123456";
        public string Roles { get; set; }
        public string Fullname { get; set; }
        
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}