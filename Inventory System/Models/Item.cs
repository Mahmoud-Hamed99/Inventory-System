using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Inventory_System.Models
{
    public class Item
    {
        public int ItemId { set; get; }

        [Required(ErrorMessage ="hoba")]
        public string ItemName { set; get; }
        [Required(ErrorMessage ="hoba")]
        public string ItemUnit { get; set; }
        [Required(ErrorMessage ="hoba3")]
        public double ItemQuantity { get; set; }

        public double ItemQuantityAdded { get; set; } //new

        public double ItemQuantityWithdraw { get; set; } //new

        public double ItemReturn { get; set; } // هالك

        public double ItemReminder { get; set; }

        public double ItemAvgPrice { set; get; }
        
        public int ItemSubCategoryId { set; get; }

        public ItemSubCategory ItemSubCategory { set; get; }

        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemInput> ItemInputs { set; get; }
        
        public ICollection<ItemOutput> ItemOutputs { set; get; }
        
        public ICollection<ItemReturn> ItemReturns { set; get; }
    }
}