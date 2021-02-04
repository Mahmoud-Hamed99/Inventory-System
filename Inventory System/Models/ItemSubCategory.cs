using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemSubCategory
    {
        public int ItemSubCategoryId { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل اسم الصنف الفرعي")]
        public string ItemSubCategoryName { set; get; }
        
        public int ItemCategoryId { get; set; }
        
        public ItemCategory ItemCategory { set; get; }

        public ICollection<Item> Items { set; get; }
    }
}