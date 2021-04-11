using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using helper.Classes;

namespace Inventory_System.Models
{
    public class Item: TableWithDate
    {
        public int ItemId { set; get; }

        [Required(ErrorMessage ="من فضلك ادخل اسم الصنف")]
        public string ItemName { set; get; }

        [Required(ErrorMessage ="من فضلك ادخل وحده الصنف")]
        public string ItemUnit { get; set; }

        [Required(ErrorMessage ="من فضلك ادخل الكميه")]
        public double ItemQuantity { get; set; }

        [Display(Name ="بن كود")]
        public string BinCode { get; set; }

        public double ItemQuantityAdded { get; set; } //new

        public double ItemQuantityWithdraw { get; set; } //new

        public double ItemReturn { get; set; } // هالك

        public int ItemMinQuantity { get; set; }

        public double ItemReminder { get; set; }

        public double ItemAvgPrice { set; get; }

        public int ItemSubCategoryId { set; get; }
     
        public ItemSubCategory ItemSubCategory { set; get; }

        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemInput> ItemInputs { set; get; }
        
        public ICollection<ItemOutput> ItemOutputs { set; get; }
        
        public ICollection<ItemReturn> ItemReturns { set; get; }

        public ICollection<DemandItem> DemandItems { set; get; }
    }
}