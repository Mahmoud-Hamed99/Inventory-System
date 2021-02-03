using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemInput
    {
        public int ItemInputId { set; get; }

        public int ItemId { set; get; }
        
        public Item Item { set; get; }
        
        public double ItemPrice { set; get; }
        
        public double ItemQuantity { set; get; }
        
        public double ItemTotalCost { set; get; }
        
        public int VendorId { set; get; }
        
        public Vendor Vendor { get; set; }

        public double ItemReturn { get; set; }

        public string Notes { get; set; }

        public DateTime DateCreated { set; get; } = DateTime.Now;

    }
}