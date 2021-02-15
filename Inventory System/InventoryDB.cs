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

        public System.Data.Entity.DbSet<Inventory_System.Models.TechnicalDepartment> TechnicalDepartments { get; set; }
    }
}