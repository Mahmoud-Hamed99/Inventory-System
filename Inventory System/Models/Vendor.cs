using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Vendor
    {
        public int VendorId { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل اسم المورد")]
        public string VendorName { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل رقم الهاتف للمورد")]
        public string VendorPhone { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemInput> ItemInputs { set; get; }
    }
}