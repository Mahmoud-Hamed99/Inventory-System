using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Safe
    {
        public int SafeId { get; set; }

        public int PermessionNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }

        public string TransactionType { get; set; }

        public double Deposit { get; set; }

        public double Withdraw { get; set; }

        public string Notes { get; set; }
    }
}