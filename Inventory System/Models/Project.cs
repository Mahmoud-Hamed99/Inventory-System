using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Project
    {
        public int ProjectId { set; get; }

        [Required(ErrorMessage = "من فضلك ادخل اسم المشروع")]
        public string ProjectName { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool ProjectFinished { get; set; } = false;

        public ICollection<ItemOutput> ItemOutputs { set; get; }
        
        public ICollection<ItemReturn> ItemReturns { set; get; }
       
    }
}