using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Item
    {
        public int ItemId { set; get; }
        
        public string ItemName { set; get; }
        
        public string ItemUnit { get; set; }
        
        public double ItemQuantity { get; set; }

        public double ItemAvgPrice { set; get; }
        
        public int ItemSubCategoryId { set; get; }
        
        public ItemSubCategory ItemSubCategory { set; get; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemInput> ItemInputs { set; get; }
        
        public ICollection<ItemOutput> ItemOutputs { set; get; }
        
        public ICollection<ItemReturn> ItemReturns { set; get; }

       

    }
}