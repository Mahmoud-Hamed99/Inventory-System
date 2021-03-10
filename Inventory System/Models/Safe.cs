using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Safe
    {
        public int SafeId { get; set; }

        public int PermessionNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public string TransactionType { get; set; }

        public double Deposit { get; set; }

        public double Withdraw { get; set; }

        public string Notes { get; set; }
    }
}