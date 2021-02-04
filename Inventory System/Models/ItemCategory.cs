using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemCategory
    {
        
        public int ItemCategoryId { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل اسم الصنف")]
        public string ItemCategoryName { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        
        public ICollection<ItemSubCategory> ItemSubCategories { set; get; }
    }
}