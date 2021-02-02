using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Project
    {
        public int ProjectId { set; get; }
     
        public string ProjectName { set; get; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool ProjectFinished { get; set; } = false;

        public ICollection<ItemOutput> ItemOutputs { set; get; }
        
        public ICollection<ItemReturn> ItemReturns { set; get; }
       
    }
}