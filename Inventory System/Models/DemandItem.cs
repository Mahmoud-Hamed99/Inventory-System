using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class DemandItem
    {
        public int DemandItemId { get; set; }

        public int ItemOutputId { get; set; }

        public ItemOutput ItemOutput  { get; set; }

        public double DemandItemQuantity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DemandItemPriority { get; set; } = DateTime.Now;

        public bool DemandItemApproval { get; set; } = false;

        public bool PurchasingApproval { get; set; } = false;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}