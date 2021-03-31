using helper.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_System.Models
{
    public class Safe : TableWithDepositWithdraw
    {
        public int SafeId { get; set; }

        public int PermessionNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }

        public string TransactionType { get; set; }

        public double Deposit { get; set; }

        public double Withdraw { get; set; }

        public string Notes { get; set; }
        [Display(Name = "النوع")]
        public SafeSubCategory SafeSubCategory { get; set; }
        [Display(Name ="النوع")]
        public int? SafeSubCategoryId { get; set; }
    }
    public class SafeCategory
    {
        public int SafeCategoryId { get; set; }
        [Display(Name = "إسم التوجيه")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "تاريخ الإضافة")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public List<SafeSubCategory> SafeSubCategories { get; set; }
    }

    public class SafeSubCategory
    {
        public int SafeSubCategoryId { get; set; }
        [Display(Name= "إسم نوع المصروف")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "تاريخ الإضافة")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Display(Name = "الجهة")]
        public SafeCategory SafeCategory { get; set; }
        [Display(Name = "الجهة")]
        public int SafeCategoryId { get; set; }
        public List<Safe> Saves { get; set; }
    }
}