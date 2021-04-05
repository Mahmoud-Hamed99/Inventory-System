using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class ItemOutput:helper.Classes.TableWithDate
    {
        public int ItemOutputId { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل الكميه")]
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

        public string Notes { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}