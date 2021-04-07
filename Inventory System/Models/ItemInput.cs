using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemInput : helper.Classes.TableWithDate
    {
        public int ItemInputId { set; get; }

        public int ItemId { set; get; }

        public Item Item { set; get; }

        public double ItemPrice { set; get; }

        public int DocCode { set; get; } //رقم المستند

        [Required(ErrorMessage = "من فضلك ادخل الكميه")]
        public double ItemQuantity { set; get; }

        public double ItemTotalCost { set; get; }

        public int VendorId { set; get; }

        public Vendor Vendor { get; set; }

        public double ItemReturn { get; set; }

        public string Notes { get; set; }

       // public int MyProperty { get; set; }

        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}" )]
        public DateTime DateCreated { set; get; } = DateTime.Now;

        public ICollection<ItemReturn> ItemReturns{ get; set; }

    }
}