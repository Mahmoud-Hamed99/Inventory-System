using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemCategory
    {
        public int ItemCategoryId { set; get; }

        public string ItemCategoryName { set; get; }
        
        public DateTime DateCreated { get; set; } = DateTime.Now;
        
        public ICollection<ItemSubCategory> ItemSubCategories { set; get; }
    }
}