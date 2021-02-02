using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Vendor
    {
        public int VendorId { set; get; }
       
        public string VendorName { set; get; }
        
        public string VendorPhone { set; get; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemInput> ItemInputs { set; get; }
    }
}