using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public string NotificationName { get; set; }
        public string NotificationText { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}