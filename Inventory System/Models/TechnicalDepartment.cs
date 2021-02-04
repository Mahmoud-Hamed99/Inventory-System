using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class TechnicalDepartment
    {
        public int TechnicalDepartmentId { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القسم")]
        public String TechnicalDepartmentName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemOutput> ItemOutputs { set; get; }

      
    }
}