using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class DemandItem
    {
        public int DemandItemId { get; set; }

        public int ItemId { get; set; }

        public Item Item  { get; set; }

        public double DemandItemQuantity { get; set; }

        public int DemandItemPriority { get; set; }

        public bool DemandItemApproval { get; set; } = false;

        public bool PurchasingApproval { get; set; } = false;

    }
}