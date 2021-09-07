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

        private double _demanditemquantity;
        public double DemandItemQuantity { get=>_demanditemquantity;
            set {
                _demanditemquantity = value;
                PurchasedItemQuantity = value;
            }
        }

        public double PurchasedItemQuantity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DemandItemPriority { get; set; } = DateTime.Now;

        public bool? DemandItemApproval { get; set; }

        private bool? _purchasingapproval;
        public bool? PurchasingApproval { get=>_purchasingapproval; 
            set {
                _purchasingapproval = value;
                PurchaseApprovalDate = DateTime.Now;
            } 
        }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PurchaseApprovalDate { get; set; }
    }
}