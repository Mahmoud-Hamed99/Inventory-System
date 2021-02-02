using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class TechnicalDepartment
    {
        public int TechnicalDepartmentId { get; set; }
        
        public String TechnicalDepartmentName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<ItemOutput> ItemOutputs { set; get; }
    }
}