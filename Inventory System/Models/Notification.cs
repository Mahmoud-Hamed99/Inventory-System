using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        [Display(Name ="التنبيه")]
        public string NotificationName { get; set; }
        [Display(Name = "الرسالة")]
        public string NotificationText { get; set; }
        [Display(Name = "التاريخ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}