using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemReturn
    {
        public int ItemReturnId { set; get; }
      
        public int ItemId { set; get; }
        
        public Item Item { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل الكميه")]
        public double ItemQuantity { set; get; }
        
        public int projectId { set; get; }
        
        public Project Project { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

       
    }
}