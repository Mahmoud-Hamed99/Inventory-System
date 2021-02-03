using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemReturn
    {
        public int ItemReturnId { set; get; }
      
        public int ItemId { set; get; }
        
        public Item Item { set; get; }
        
        public double ItemQuantity { set; get; }
        
        public int projectId { set; get; }
        
        public Project Project { set; get; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

       
    }
}