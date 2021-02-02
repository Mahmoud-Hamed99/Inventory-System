using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemOutput
    {
        public int ItemOutputId { set; get; }

        public double ItemOutputQuantity { set; get; }

        public int ItemId { set; get; }
        
        public Item Item { set; get; }
               
        public int ProjectId { set; get; }
        
        public Project Project { set; get; }

        //Add department link
        public int TechnicalDepartmentId { get; set; }

        public TechnicalDepartment TechnicalDepartment { set; get; }

        //Approved with default false
        public bool ItemOutputApproved { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}