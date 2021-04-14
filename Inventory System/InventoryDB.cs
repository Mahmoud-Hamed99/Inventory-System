using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Inventory_System
{
    public class InventoryDB : DbContext
    {
        public InventoryDB():base("name=InventoryDB")
        {
            Database.Initialize(false);
        }

        public DbSet<Models.Item> Items { set; get; }
        public DbSet<Models.ItemInput> ItemInputs { set; get; }
        public DbSet<Models.ItemOutput> ItemOutputs { set; get; }
        public DbSet<Models.Project> Projects { set; get; }
        public DbSet<Models.ItemReturn> ItemReturns { set; get; }
        public DbSet<Models.ItemSubCategory> ItemSubCategories { set; get; }
        public DbSet<Models.ItemCategory> ItemCategories { set; get; }
        public DbSet<Models.Vendor>Vendors { set; get; }
        public DbSet<Models.Notification> Notifications { set; get; }
        public DbSet<Models.DemandItem> DemandItems { set; get; }
        public DbSet<Models.TechnicalDepartment> TechnicalDepartments { get; set; }
        public DbSet<Models.BankAccount> BankAccountants { get; set; }
        public DbSet<Models.Safe> Safe { get; set; }
        public DbSet<Models.SafeCategory> SafeCategories { get; set; }
        public DbSet<Models.SafeSubCategory> safeSubCategories { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.UserLog> UserLogs { get; set; }
    }
}