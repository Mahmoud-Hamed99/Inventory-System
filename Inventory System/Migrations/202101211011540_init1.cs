namespace Inventory_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemInputs", "Vendor_VendorId", "dbo.Vendors");
            DropIndex("dbo.ItemInputs", new[] { "Vendor_VendorId" });
            RenameColumn(table: "dbo.ItemInputs", name: "Vendor_VendorId", newName: "VendorId");
            AlterColumn("dbo.ItemInputs", "VendorId", c => c.Int(nullable: false));
            CreateIndex("dbo.ItemInputs", "VendorId");
            AddForeignKey("dbo.ItemInputs", "VendorId", "dbo.Vendors", "VendorId", cascadeDelete: true);
            DropColumn("dbo.ItemInputs", "ItemVendorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemInputs", "ItemVendorId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ItemInputs", "VendorId", "dbo.Vendors");
            DropIndex("dbo.ItemInputs", new[] { "VendorId" });
            AlterColumn("dbo.ItemInputs", "VendorId", c => c.Int());
            RenameColumn(table: "dbo.ItemInputs", name: "VendorId", newName: "Vendor_VendorId");
            CreateIndex("dbo.ItemInputs", "Vendor_VendorId");
            AddForeignKey("dbo.ItemInputs", "Vendor_VendorId", "dbo.Vendors", "VendorId");
        }
    }
}
