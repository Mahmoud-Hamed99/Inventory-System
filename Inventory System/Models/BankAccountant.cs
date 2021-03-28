using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }

        public string BankName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }

        public int TransitionNumber { get; set; }

        public string Statement { get; set; }

        public string TransitionType { get; set; }

        public double Deposit { get; set; }

        public double Withdraw { get; set; }

        public double Balance { get; set; }

        public bool CheckIsPaied { get; set; } = false; 
    }
}